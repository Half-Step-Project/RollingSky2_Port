using System;
using System.IO;
using Foundation;
using UnityEngine;

public class JumpDistanceTrigger : BaseEnemy
{
	[Serializable]
	public struct TriggerData : IReadWriteBytes
	{
		public float LimitTime;

		public float JumpDistance;

		public float JumpHeight;

		public float MoveSpeed;

		public float DeltaMovement;

		public float MoveScaler;

		public bool IfShowTrail;

		public bool IfHideOnCollide;

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
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	public TriggerData data;

	[Range(0f, 2f)]
	public float TestPercent;

	public float TestBaseSpeed = 7.25f;

	public float TestTime;

	private Vector3 beginPos;

	private Vector3 endPos;

	private float collidePos;

	private Transform model;

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
			data = (TriggerData)objs[0];
		}
	}

	public override void Initialize()
	{
		base.Initialize();
		if (model == null)
		{
			model = base.transform.Find("model");
		}
		beginPos = (endPos = StartLocalPos);
		collidePos = 10000f;
	}

	public override void ResetElement()
	{
		base.ResetElement();
		collidePos = 10000f;
	}

	public override void UpdateElement()
	{
	}

	public override void TriggerEnter(BaseRole ball)
	{
		if ((bool)ball)
		{
			bool flag = data.LimitTime < 0f;
			if (ball.IfJumpingDown)
			{
				ball.CallEndJump(base.transform.position.y, true);
				ball.CallBeginJump(base.transform.position, base.transform.position + data.JumpDistance * ball.transform.forward, ball.transform.forward, data.JumpHeight, BaseRole.JumpType.QTEBetween, !flag);
				collidePos = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z;
				PlaySoundEffect();
			}
			else if (!ball.IfJumping)
			{
				ball.CallBeginJump(base.transform.position, base.transform.position + data.JumpDistance * ball.transform.forward, ball.transform.forward, data.JumpHeight, BaseRole.JumpType.QTE, !flag);
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
		data = JsonUtility.FromJson<TriggerData>(info);
	}

	public override string Write()
	{
		return JsonUtility.ToJson(data);
	}

	public override void ReadBytes(byte[] bytes)
	{
		data = StructTranslatorUtility.ToStructure<TriggerData>(bytes);
	}

	public override byte[] WriteBytes()
	{
		return StructTranslatorUtility.ToByteArray(data);
	}

	public override void OnDrawGizmos()
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

	[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
	public override void RebirthReadData(object rd_data)
	{
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override object RebirthWriteData()
	{
		return string.Empty;
	}

	[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
	public override void RebirthStartGame(object rd_data)
	{
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
	}

	public override byte[] RebirthWriteByteData()
	{
		return null;
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
	}
}
