using System;
using System.IO;
using Foundation;
using UnityEngine;

public class SnakeDepartVehicleTrigger : BaseTriggerBox
{
	[Serializable]
	public struct TriggerData : IReadWriteBytes
	{
		public float JumpDistance;

		public float JumpHeight;

		public bool IfShowTrail;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			JumpDistance = bytes.GetSingle(ref startIndex);
			JumpHeight = bytes.GetSingle(ref startIndex);
			IfShowTrail = bytes.GetBoolean(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(JumpDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(JumpHeight.GetBytes(), ref offset);
				memoryStream.WriteByteArray(IfShowTrail.GetBytes(), ref offset);
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

	public override bool IfRebirthRecord
	{
		get
		{
			return false;
		}
	}

	public override void SetDefaultValue(object[] objs)
	{
		data.JumpDistance = (float)objs[0];
		data.JumpHeight = (float)objs[1];
		data.IfShowTrail = (bool)objs[2];
	}

	public override void TriggerEnter(BaseRole ball)
	{
		if ((bool)ball)
		{
			ball.DepartVehicle(true);
			ball.ChangeToMove();
			if (!ball.IfJumping)
			{
				ball.CallBeginJump(base.transform.position, base.transform.position + data.JumpDistance * ball.transform.forward, ball.transform.forward, data.JumpHeight, BaseRole.JumpType.Super);
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
		base.OnDrawGizmos();
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
		TestTime = (float)num6 + (TestTime - (float)num6) * 0.6f;
	}
}
