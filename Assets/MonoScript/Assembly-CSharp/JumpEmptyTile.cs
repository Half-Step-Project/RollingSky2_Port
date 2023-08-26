using System;
using System.IO;
using Foundation;
using UnityEngine;

public class JumpEmptyTile : BaseTile
{
	[Serializable]
	public struct TileData : IReadWriteBytes
	{
		public float LimitTime;

		public float JumpDistance;

		public float JumpHeight;

		public float MoveSpeed;

		public float DeltaMovement;

		public float MoveScaler;

		public bool IfShowTrail;

		public bool IfHideOnCollide;

		public float DefaultEmission;

		public float TargetEmission;

		public float EmissionScaler;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			LimitTime = bytes.GetSingle(ref startIndex);
			JumpDistance = bytes.GetSingle(ref startIndex);
			JumpHeight = bytes.GetSingle(ref startIndex);
			MoveSpeed = bytes.GetSingle(ref startIndex);
			DeltaMovement = bytes.GetSingle(ref startIndex);
			MoveScaler = bytes.GetSingle(ref startIndex);
			IfShowTrail = bytes.GetBoolean(ref startIndex);
			IfHideOnCollide = bytes.GetBoolean(ref startIndex);
			DefaultEmission = bytes.GetSingle(ref startIndex);
			TargetEmission = bytes.GetSingle(ref startIndex);
			EmissionScaler = bytes.GetSingle(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(LimitTime.GetBytes(), ref offset);
				memoryStream.WriteByteArray(JumpDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(JumpHeight.GetBytes(), ref offset);
				memoryStream.WriteByteArray(MoveSpeed.GetBytes(), ref offset);
				memoryStream.WriteByteArray(DeltaMovement.GetBytes(), ref offset);
				memoryStream.WriteByteArray(MoveScaler.GetBytes(), ref offset);
				memoryStream.WriteByteArray(IfShowTrail.GetBytes(), ref offset);
				memoryStream.WriteByteArray(IfHideOnCollide.GetBytes(), ref offset);
				memoryStream.WriteByteArray(DefaultEmission.GetBytes(), ref offset);
				memoryStream.WriteByteArray(TargetEmission.GetBytes(), ref offset);
				memoryStream.WriteByteArray(EmissionScaler.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	public TileData data;

	[Range(0f, 2f)]
	public float TestPercent;

	public float TestBaseSpeed = 7.25f;

	public float TestTime;

	private Vector3 beginPos;

	private Vector3 endPos;

	private float collidePos;

	private Transform state1;

	private Transform state2;

	private Transform model;

	private Transform effect;

	protected ParticleSystem[] particles;

	private Material jumpMat;

	private RD_JumpDistanceQTETile_DATA m_rebirthData;

	public override bool IfRebirthRecord
	{
		get
		{
			return true;
		}
	}

	public override void SetDefaultValue(object[] objs)
	{
		if (objs != null)
		{
			data = (TileData)objs[0];
		}
	}

	public override void Initialize()
	{
		base.Initialize();
		if (state1 == null || state2 == null)
		{
			state1 = base.transform.Find("model/state1");
			state2 = base.transform.Find("model/state2");
		}
		if (model == null)
		{
			model = base.transform.Find("model");
		}
		if (effect == null)
		{
			effect = base.transform.Find("effect");
		}
		if ((bool)state1)
		{
			state1.gameObject.SetActive(true);
			jumpMat = state1.GetComponent<MeshRenderer>().material;
			jumpMat.SetFloat("_Emmission", data.DefaultEmission);
		}
		if ((bool)state2)
		{
			state2.gameObject.SetActive(false);
		}
		if ((bool)model)
		{
			model.gameObject.SetActive(true);
		}
		if ((bool)effect)
		{
			particles = effect.GetComponentsInChildren<ParticleSystem>();
			StopParticle();
		}
		beginPos = (endPos = StartLocalPos);
		collidePos = 10000f;
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
	}

	public override void UpdateElement()
	{
		float distance = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - collidePos;
		PlayByPercent(GetPercent(distance));
		ChangeEmissionByPercent(GetEmissionPercent(distance));
	}

	public override float GetPercent(float distance)
	{
		return Mathf.Min(2f, Mathf.Max(0f, distance));
	}

	public override void PlayByPercent(float percent)
	{
		if (percent > 0f && percent <= 1f)
		{
			float num = data.MoveSpeed - data.MoveSpeed * (percent - 1f) * (percent - 1f);
			base.transform.localPosition = new Vector3(endPos.x, endPos.y + num, endPos.z);
		}
		else if (percent > 1f && percent < 2f)
		{
			float num2 = data.MoveSpeed - data.MoveSpeed * (percent - 1f) * (percent - 1f);
			base.transform.localPosition = new Vector3(endPos.x, endPos.y + num2, endPos.z);
		}
		else if (percent <= 0f || percent >= 2f)
		{
			base.transform.localPosition = endPos;
		}
	}

	private float GetEmissionPercent(float distance)
	{
		return Mathf.Min(1f, Mathf.Max(0f, distance * data.EmissionScaler));
	}

	private void ChangeEmissionByPercent(float percent)
	{
		if (percent > 0f && percent < 1f && (bool)jumpMat && data.DefaultEmission >= -1f)
		{
			jumpMat.SetFloat("_Emmission", Mathf.Lerp(data.DefaultEmission, data.TargetEmission, percent));
		}
	}

	public override void TriggerEnter(BaseRole ball)
	{
		if ((bool)ball)
		{
			if ((bool)model && data.IfHideOnCollide)
			{
				model.gameObject.SetActive(false);
			}
			PlayParticle();
			ball.OnTileEnter(this);
			if (ball.IfJumpingDown)
			{
				ball.CallEndJump(base.transform.position.y, true);
				ball.CallBeginJump(base.transform.position, base.transform.position + data.JumpDistance * ball.transform.forward, ball.transform.forward, data.JumpHeight, BaseRole.JumpType.NoAnim);
				collidePos = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z;
				PlaySoundEffect();
			}
			else if (!ball.IfJumping)
			{
				ball.CallBeginJump(base.transform.position, base.transform.position + data.JumpDistance * ball.transform.forward, ball.transform.forward, data.JumpHeight, BaseRole.JumpType.NoAnim);
				collidePos = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z;
				PlaySoundEffect();
			}
			else if (ball.IfDropping)
			{
				Debug.LogError("Don't use jumpDistance Tile with DropTile");
			}
		}
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
		m_rebirthData = JsonUtility.FromJson<RD_JumpDistanceQTETile_DATA>(rd_data as string);
		state1.SetTransData(m_rebirthData.m_state1Transform);
		state2.SetTransData(m_rebirthData.m_state2Transform);
		model.SetTransData(m_rebirthData.m_modelTransform);
		effect.SetTransData(m_rebirthData.m_effectTransform);
		particles.SetParticlesData(m_rebirthData.m_effect, ProcessState.Pause);
		if (jumpMat != null)
		{
			jumpMat.SetFloat("_Emmission", m_rebirthData.m_jumpMat);
		}
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override object RebirthWriteData()
	{
		string empty = string.Empty;
		RD_JumpDistanceQTETile_DATA rD_JumpDistanceQTETile_DATA = new RD_JumpDistanceQTETile_DATA();
		rD_JumpDistanceQTETile_DATA.m_state1Transform = state1.GetTransData();
		rD_JumpDistanceQTETile_DATA.m_state2Transform = state2.GetTransData();
		rD_JumpDistanceQTETile_DATA.m_modelTransform = model.GetTransData();
		rD_JumpDistanceQTETile_DATA.m_effectTransform = effect.GetTransData();
		rD_JumpDistanceQTETile_DATA.m_effect = particles.GetParticlesData();
		if (jumpMat != null)
		{
			rD_JumpDistanceQTETile_DATA.m_jumpMat = jumpMat.GetFloat("_Emmission");
		}
		return JsonUtility.ToJson(rD_JumpDistanceQTETile_DATA);
	}

	[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
	public override void RebirthStartGame(object rd_data)
	{
		particles.SetParticlesData(m_rebirthData.m_effect, ProcessState.UnPause);
		m_rebirthData = null;
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
		m_rebirthData = Bson.ToObject<RD_JumpDistanceQTETile_DATA>(rd_data);
		state1.SetTransData(m_rebirthData.m_state1Transform);
		state2.SetTransData(m_rebirthData.m_state2Transform);
		model.SetTransData(m_rebirthData.m_modelTransform);
		effect.SetTransData(m_rebirthData.m_effectTransform);
		particles.SetParticlesData(m_rebirthData.m_effect, ProcessState.Pause);
		if (jumpMat != null)
		{
			jumpMat.SetFloat("_Emmission", m_rebirthData.m_jumpMat);
		}
	}

	public override byte[] RebirthWriteByteData()
	{
		RD_JumpDistanceQTETile_DATA rD_JumpDistanceQTETile_DATA = new RD_JumpDistanceQTETile_DATA();
		rD_JumpDistanceQTETile_DATA.m_state1Transform = state1.GetTransData();
		rD_JumpDistanceQTETile_DATA.m_state2Transform = state2.GetTransData();
		rD_JumpDistanceQTETile_DATA.m_modelTransform = model.GetTransData();
		rD_JumpDistanceQTETile_DATA.m_effectTransform = effect.GetTransData();
		rD_JumpDistanceQTETile_DATA.m_effect = particles.GetParticlesData();
		if (jumpMat != null)
		{
			rD_JumpDistanceQTETile_DATA.m_jumpMat = jumpMat.GetFloat("_Emmission");
		}
		return Bson.ToBson(rD_JumpDistanceQTETile_DATA);
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
		particles.SetParticlesData(m_rebirthData.m_effect, ProcessState.UnPause);
		m_rebirthData = null;
	}
}
