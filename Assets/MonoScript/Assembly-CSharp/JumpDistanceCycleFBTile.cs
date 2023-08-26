using System;
using System.IO;
using Foundation;
using UnityEngine;

public class JumpDistanceCycleFBTile : BaseTile
{
	[Serializable]
	public struct TileData : IReadWriteBytes
	{
		public float JumpDistance;

		public float JumpHeight;

		public bool IfShowTrail;

		public float BeginDistance;

		public bool ifToForward;

		public float MoveSpeed;

		public float MoveOffset;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			JumpDistance = bytes.GetSingle(ref startIndex);
			JumpHeight = bytes.GetSingle(ref startIndex);
			IfShowTrail = bytes.GetBoolean(ref startIndex);
			BeginDistance = bytes.GetSingle(ref startIndex);
			ifToForward = bytes.GetBoolean(ref startIndex);
			MoveSpeed = bytes.GetSingle(ref startIndex);
			MoveOffset = bytes.GetSingle(ref startIndex);
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
				memoryStream.WriteByteArray(ifToForward.GetBytes(), ref offset);
				memoryStream.WriteByteArray(MoveSpeed.GetBytes(), ref offset);
				memoryStream.WriteByteArray(MoveOffset.GetBytes(), ref offset);
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

	private Transform model;

	private Transform effect;

	protected ParticleSystem[] particles;

	private Animation anim;

	private Material jumpMat;

	public override bool IfRebirthRecord
	{
		get
		{
			return false;
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
		data.MoveSpeed = Mathf.Abs(data.MoveSpeed);
		data.MoveOffset = Mathf.Abs(data.MoveOffset);
		commonState = CommonState.None;
	}

	public override void ResetElement()
	{
		base.ResetElement();
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
		return distance;
	}

	public override void PlayByPercent(float percent)
	{
		float num = 0f;
		num = ((!data.ifToForward) ? (0f - GetZForwardOffset(percent * data.MoveSpeed)) : GetZForwardOffset(percent * data.MoveSpeed));
		base.transform.localPosition = StartLocalPos + new Vector3(0f, 0f, num);
	}

	public override void TriggerEnter(BaseRole ball)
	{
		if (commonState == CommonState.Active || !ball)
		{
			return;
		}
		ball.OnTileEnter(this);
		if (ball.IfJumpingDown)
		{
			ball.CallEndJump(base.transform.position.y, true);
			ball.CallBeginJump(base.transform.position, base.transform.position + data.JumpDistance * ball.transform.forward, ball.transform.forward, data.JumpHeight, BaseRole.JumpType.QTEBetween);
			PlaySoundEffect();
			return;
		}
		if (!ball.IfJumping)
		{
			ball.CallBeginJump(base.transform.position, base.transform.position + data.JumpDistance * ball.transform.forward, ball.transform.forward, data.JumpHeight, BaseRole.JumpType.QTE);
			PlaySoundEffect();
			return;
		}
		if (ball.IfDropping)
		{
			Debug.LogError("Don't use jumpDistance Tile with DropTile");
		}
		commonState = CommonState.Active;
	}

	private float GetZForwardOffset(float movement)
	{
		movement = Mathf.Abs(movement);
		float result = 0f;
		int num = (int)(movement / data.MoveOffset);
		float num2 = movement - (float)(num * (int)data.MoveOffset);
		switch (num % 2)
		{
		case 0:
			result = num2;
			break;
		case 1:
			result = data.MoveOffset - num2;
			break;
		}
		return result;
	}

	private float GetZBackOffset(float movement)
	{
		movement = Mathf.Abs(movement);
		float num = 0f;
		int num2 = (int)(movement / data.MoveOffset);
		float num3 = movement - (float)(num2 * (int)data.MoveOffset);
		switch (num2 % 2)
		{
		case 0:
			num = num3;
			break;
		case 1:
			num = data.MoveOffset - num3;
			break;
		}
		return 0f - num;
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
}
