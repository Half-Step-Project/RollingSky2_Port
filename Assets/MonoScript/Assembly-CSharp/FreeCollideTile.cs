using System;
using System.IO;
using Foundation;
using UnityEngine;

public class FreeCollideTile : BaseTile
{
	[Serializable]
	public struct TileData : IReadWriteBytes
	{
		public float TileWidth;

		public float TileHeight;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			TileWidth = bytes.GetSingle(ref startIndex);
			TileHeight = bytes.GetSingle(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(TileWidth.GetBytes(), ref offset);
				memoryStream.WriteByteArray(TileHeight.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	public TileData data;

	public override float TileWidth
	{
		get
		{
			return data.TileWidth * 0.5f + BaseTile.RecycleWidthTolerance;
		}
	}

	public override float TileHeight
	{
		get
		{
			return data.TileHeight * 0.5f + BaseTile.RecycleHeightTolerance;
		}
	}

	public override bool IfRebirthRecord
	{
		get
		{
			return false;
		}
	}

	public override void SetDefaultValue(object[] objs)
	{
		if (objs != null)
		{
			data.TileWidth = (float)objs[0];
			data.TileHeight = (float)objs[1];
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
