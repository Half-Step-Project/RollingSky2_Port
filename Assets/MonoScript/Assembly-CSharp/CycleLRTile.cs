using System;
using System.IO;
using Foundation;
using UnityEngine;

public class CycleLRTile : BaseTile
{
	[Serializable]
	public struct TileData : IReadWriteBytes
	{
		public float BeginDistance;

		public bool ifToLeft;

		public float MoveSpeed;

		public float MoveOffset;

		public int GroupSize;

		public int GroupIndex;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
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
		data.MoveOffset = StartLocalPos.x - (float)GRID_WIDTH * 0.5f + (float)HALF_ROAD_SIZE;
	}

	public override void UpdateElement()
	{
		float distance = base.transform.parent.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z - data.BeginDistance;
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
}
