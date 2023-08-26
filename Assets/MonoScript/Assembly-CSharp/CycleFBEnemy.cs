using System;
using System.IO;
using Foundation;
using UnityEngine;

public class CycleFBEnemy : BaseEnemy
{
	[Serializable]
	public struct EnemyData : IReadWriteBytes
	{
		public float BeginDistance;

		public bool ifToForward;

		public float MoveSpeed;

		public float MoveOffset;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
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

	public EnemyData data;

	public override bool IfRebirthRecord
	{
		get
		{
			return false;
		}
	}

	public override void Initialize()
	{
		base.Initialize();
		data.MoveSpeed = Mathf.Abs(data.MoveSpeed);
		data.MoveOffset = Mathf.Abs(data.MoveOffset);
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
		num = ((!data.ifToForward) ? (GetZBackOffset(percent * data.MoveSpeed) + data.MoveOffset) : GetZForwardOffset(percent * data.MoveSpeed));
		base.transform.localPosition = StartLocalPos + new Vector3(0f, 0f, num);
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
		data = JsonUtility.FromJson<EnemyData>(info);
	}

	public override string Write()
	{
		return JsonUtility.ToJson(data);
	}

	public override void ReadBytes(byte[] bytes)
	{
		data = StructTranslatorUtility.ToStructure<EnemyData>(bytes);
	}

	public override byte[] WriteBytes()
	{
		return StructTranslatorUtility.ToByteArray(data);
	}
}
