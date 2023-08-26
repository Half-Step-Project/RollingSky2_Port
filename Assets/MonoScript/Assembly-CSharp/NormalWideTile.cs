using System;
using Foundation;
using UnityEngine;

public class NormalWideTile : BaseTile
{
	[Serializable]
	public struct TileData : IReadWriteBytes
	{
		public float TileWidth;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			TileWidth = bytes.GetInt32(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			return TileWidth.GetBytes();
		}
	}

	public TileData data;

	public override float TileWidth
	{
		get
		{
			return data.TileWidth;
		}
	}

	public override bool IfRebirthRecord
	{
		get
		{
			return false;
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
}
