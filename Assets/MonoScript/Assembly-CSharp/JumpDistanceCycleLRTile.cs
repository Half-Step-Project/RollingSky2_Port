using System;
using System.IO;
using Foundation;
using UnityEngine;

public class JumpDistanceCycleLRTile : BaseTile
{
	[Serializable]
	public struct TileData : IReadWriteBytes
	{
		public float JumpDistance;

		public float JumpHeight;

		public bool IfShowTrail;

		public float BeginDistance;

		public bool ifToLeft;

		public float MoveSpeed;

		public float MoveOffset;

		public int GroupSize;

		public int GroupIndex;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			JumpDistance = bytes.GetSingle(ref startIndex);
			JumpHeight = bytes.GetSingle(ref startIndex);
			IfShowTrail = bytes.GetBoolean(ref startIndex);
			BeginDistance = bytes.GetSingle(ref startIndex);
			ifToLeft = bytes.GetBoolean(ref startIndex);
			MoveSpeed = bytes.GetSingle(ref startIndex);
			MoveOffset = bytes.GetSingle(ref startIndex);
			GroupSize = bytes.GetInt32(ref startIndex);
			GroupIndex = bytes.GetInt32(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(JumpDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(JumpHeight.GetBytes(), ref offset);
				memoryStream.WriteByteArray(IfShowTrail.GetBytes(), ref offset);
				memoryStream.WriteByteArray(BeginDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(ifToLeft.GetBytes(), ref offset);
				memoryStream.WriteByteArray(MoveSpeed.GetBytes(), ref offset);
				memoryStream.WriteByteArray(MoveOffset.GetBytes(), ref offset);
				memoryStream.WriteByteArray(GroupSize.GetBytes(), ref offset);
				memoryStream.WriteByteArray(GroupIndex.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	public static readonly int GRID_WIDTH = 21;

	public static readonly int ROAD_SIZE = 5;

	public static readonly int HALF_ROAD_SIZE = ROAD_SIZE / 2;

	public TileData data;

	[Range(0f, 2f)]
	public float TestPercent;

	public float TestBaseSpeed = 7.25f;

	public float TestTime;

	private float collidePos;

	private Transform model;

	private Transform effect;

	protected ParticleSystem[] particles;

	private Animation anim;

	private RD_JumpDistanceCycleLRTile_DATA m_rebirthData;

	public override bool IfRebirthRecord
	{
		get
		{
			return true;
		}
	}

	public override void SetDefaultValue(object[] objs)
	{
	}

	public override void Initialize()
	{
		base.Initialize();
		if (model == null)
		{
			model = base.transform.Find("model");
		}
		if (effect == null)
		{
			effect = base.transform.Find("effect");
		}
		if ((bool)model)
		{
			model.gameObject.SetActive(true);
			anim = model.GetComponentInChildren<Animation>();
			if ((bool)anim)
			{
				anim["anim01"].normalizedTime = 0f;
				anim.Play();
			}
		}
		if ((bool)effect)
		{
			particles = effect.GetComponentsInChildren<ParticleSystem>();
			PlayParticle();
		}
		collidePos = 10000f;
		data.MoveOffset = StartLocalPos.x - (float)GRID_WIDTH * 0.5f + (float)HALF_ROAD_SIZE;
		commonState = CommonState.None;
	}

	public override void ResetElement()
	{
		base.ResetElement();
		collidePos = 10000f;
		if ((bool)model)
		{
			model.gameObject.SetActive(true);
		}
		StopParticle();
		commonState = CommonState.None;
	}

	public override void UpdateElement()
	{
		float distance = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(StartPos).z - data.BeginDistance;
		PlayByPercent(GetPercent(distance));
	}

	public override float GetPercent(float distance)
	{
		if (!data.ifToLeft)
		{
			return distance;
		}
		return 0f - distance;
	}

	public override void PlayByPercent(float percent)
	{
		float movement = percent * data.MoveSpeed;
		float posXNTile = GetPosXNTile(movement, data.GroupSize, data.GroupIndex, data.MoveOffset);
		base.transform.localPosition = StartLocalPos + new Vector3(posXNTile - data.MoveOffset + (float)HALF_ROAD_SIZE, 0f, 0f);
	}

	public override void TriggerEnter(BaseRole ball)
	{
		if (!ball || commonState == CommonState.Active)
		{
			return;
		}
		ball.OnTileEnter(this);
		if (ball.IfJumpingDown)
		{
			ball.CallEndJump(base.transform.position.y, true);
			ball.CallBeginJump(base.transform.position, base.transform.position + data.JumpDistance * ball.transform.forward, ball.transform.forward, data.JumpHeight, BaseRole.JumpType.QTEBetween);
			collidePos = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z;
			PlaySoundEffect();
		}
		else if (!ball.IfJumping)
		{
			ball.CallBeginJump(base.transform.position, base.transform.position + data.JumpDistance * ball.transform.forward, ball.transform.forward, data.JumpHeight, BaseRole.JumpType.QTE);
			collidePos = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z;
			PlaySoundEffect();
		}
		else
		{
			if (ball.IfDropping)
			{
				Debug.LogError("Don't use jumpDistance Tile with DropTile");
			}
			commonState = CommonState.Active;
		}
	}

	public float GetPosXNTile(float movement, int gSize, int gIndex, float moveOffset)
	{
		movement += moveOffset;
		float num = 0f;
		int num2 = ROAD_SIZE - gSize;
		if (num2 != 0)
		{
			int leftN = GetLeftN(movement, num2);
			int num3 = leftN % 2;
			float f = movement - (float)(leftN * num2);
			switch (num3)
			{
			case 0:
				num = Mathf.Abs(f) + (float)gIndex;
				break;
			case -1:
			case 1:
				num = (float)num2 - Mathf.Abs(f) + (float)gIndex;
				break;
			}
			return num - (float)Mathf.FloorToInt((float)ROAD_SIZE * 0.5f);
		}
		return gIndex - Mathf.FloorToInt((float)ROAD_SIZE * 0.5f);
	}

	private float GetPosXOneTile(float movement, int gIndex, float moveOffset)
	{
		movement += moveOffset;
		float num = 0f;
		int leftN = GetLeftN(movement, 4);
		int num2 = leftN % 2;
		float f = movement - (float)(leftN * 4);
		switch (num2)
		{
		case 0:
			num = Mathf.Abs(f) + (float)gIndex;
			break;
		case -1:
		case 1:
			num = 4f - Mathf.Abs(f) + (float)gIndex;
			break;
		}
		return num - 2f;
	}

	private float GetPosXTwoTile(float movement, int gIndex, float moveOffset)
	{
		movement += moveOffset;
		float num = 0f;
		int leftN = GetLeftN(movement, 3);
		int num2 = leftN % 2;
		float f = movement - (float)(leftN * 3);
		switch (num2)
		{
		case 0:
			num = Mathf.Abs(f) + (float)gIndex;
			break;
		case -1:
		case 1:
			num = 3f - Mathf.Abs(f) + (float)gIndex;
			break;
		}
		return num - 2f;
	}

	private float GetPosXThreeTile(float movement, int gIndex, float moveOffset)
	{
		movement += moveOffset;
		float num = 0f;
		int leftN = GetLeftN(movement, 2);
		int num2 = leftN % 2;
		float f = movement - (float)(leftN * 2);
		switch (num2)
		{
		case 0:
			num = Mathf.Abs(f) + (float)gIndex;
			break;
		case -1:
		case 1:
			num = 2f - Mathf.Abs(f) + (float)gIndex;
			break;
		}
		return num - 2f;
	}

	private float GetPosXFourTile(float movement, int gIndex, float moveOffset)
	{
		movement += moveOffset;
		float num = 0f;
		int leftN = GetLeftN(movement, 1);
		int num2 = leftN % 2;
		float f = movement - (float)leftN;
		switch (num2)
		{
		case 0:
			num = Mathf.Abs(f) + (float)gIndex;
			break;
		case -1:
		case 1:
			num = 1f - Mathf.Abs(f) + (float)gIndex;
			break;
		}
		return num - 2f;
	}

	private int GetLeftN(float num, int n)
	{
		if (n == 0)
		{
			Debug.LogError("Error:N Cannot be 0");
			return -1;
		}
		return (int)(num / (float)n);
	}

	public override void Read(string info)
	{
		data = JsonUtility.FromJson<TileData>(info);
	}

	public override string Write()
	{
		return JsonUtility.ToJson(data);
	}

	public override void ReadBytes(byte[] bytes)
	{
		data = StructTranslatorUtility.ToStructure<TileData>(bytes);
	}

	public override byte[] WriteBytes()
	{
		return StructTranslatorUtility.ToByteArray(data);
	}

	private void OnDrawGizmos()
	{
		Vector3 position = base.transform.position;
		Vector3 forward = base.transform.forward;
		float jumpHeight = data.JumpHeight;
		Vector3 vector = base.transform.position;
		int num = Mathf.Max(3, Mathf.CeilToInt(data.JumpDistance * 2.5f));
		float num2 = 0.5f;
		Vector3[] array = new Vector3[num + 1];
		array[0] = vector;
		Color color = Gizmos.color;
		Gizmos.color = Color.yellow;
		for (int i = 0; i < num; i++)
		{
			Vector3 vector2 = vector + forward * num2;
			float num3 = 0f - Vector3.Dot(position - vector2, forward);
			float y = jumpHeight - 4f * jumpHeight / Mathf.Pow(data.JumpDistance, 2f) * Mathf.Pow(num3 - data.JumpDistance / 2f, 2f);
			array[i + 1] = vector2 + new Vector3(0f, y, 0f);
			Gizmos.DrawLine(array[i], array[i + 1]);
			vector = vector2;
		}
		Gizmos.color = color;
		float num4 = data.JumpDistance * TestPercent;
		float num5 = jumpHeight - 4f * jumpHeight / Mathf.Pow(data.JumpDistance, 2f) * Mathf.Pow(num4 - data.JumpDistance / 2f, 2f);
		Vector3 center = position;
		center.z += num4;
		center.y += num5;
		Gizmos.DrawCube(center, Vector3.one);
		TestTime = num4 / TestBaseSpeed;
		int num6 = Mathf.FloorToInt(TestTime);
		TestTime = (float)num6 + (TestTime - (float)num6 * 0.6f);
	}

	protected void PlayParticle()
	{
		if (particles != null)
		{
			for (int i = 0; i < particles.Length; i++)
			{
				particles[i].Play();
			}
		}
	}

	protected void StopParticle()
	{
		if (particles != null)
		{
			for (int i = 0; i < particles.Length; i++)
			{
				particles[i].Stop();
			}
		}
	}

	[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
	public override void RebirthReadData(object rd_data)
	{
		m_rebirthData = JsonUtility.FromJson<RD_JumpDistanceCycleLRTile_DATA>(rd_data as string);
		collidePos = m_rebirthData.collidePos;
		base.transform.SetTransData(m_rebirthData.trans);
		model.SetTransData(m_rebirthData.model);
		effect.SetTransData(m_rebirthData.effect);
		particles.SetParticlesData(m_rebirthData.particles, ProcessState.Pause);
		anim.SetAnimData(m_rebirthData.anim, ProcessState.Pause);
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override object RebirthWriteData()
	{
		return JsonUtility.ToJson(new RD_JumpDistanceCycleLRTile_DATA
		{
			collidePos = collidePos,
			trans = base.transform.GetTransData(),
			model = model.GetTransData(),
			effect = effect.GetTransData(),
			particles = particles.GetParticlesData(),
			anim = anim.GetAnimData()
		});
	}

	[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
	public override void RebirthStartGame(object rd_data)
	{
		particles.SetParticlesData(m_rebirthData.particles, ProcessState.UnPause);
		anim.SetAnimData(m_rebirthData.anim, ProcessState.UnPause);
		m_rebirthData = null;
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
		m_rebirthData = Bson.ToObject<RD_JumpDistanceCycleLRTile_DATA>(rd_data);
		collidePos = m_rebirthData.collidePos;
		base.transform.SetTransData(m_rebirthData.trans);
		model.SetTransData(m_rebirthData.model);
		effect.SetTransData(m_rebirthData.effect);
		particles.SetParticlesData(m_rebirthData.particles, ProcessState.Pause);
		anim.SetAnimData(m_rebirthData.anim, ProcessState.Pause);
	}

	public override byte[] RebirthWriteByteData()
	{
		return Bson.ToBson(new RD_JumpDistanceCycleLRTile_DATA
		{
			collidePos = collidePos,
			trans = base.transform.GetTransData(),
			model = model.GetTransData(),
			effect = effect.GetTransData(),
			particles = particles.GetParticlesData(),
			anim = anim.GetAnimData()
		});
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
		particles.SetParticlesData(m_rebirthData.particles, ProcessState.UnPause);
		anim.SetAnimData(m_rebirthData.anim, ProcessState.UnPause);
		m_rebirthData = null;
	}
}
