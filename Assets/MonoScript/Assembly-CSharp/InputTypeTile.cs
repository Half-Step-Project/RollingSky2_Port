using System;
using Foundation;
using UnityEngine;

public class InputTypeTile : BaseTile
{
	[Serializable]
	public struct TileData : IReadWriteBytes
	{
		public BaseRole.InputType InputType;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			InputType = (BaseRole.InputType)bytes.GetInt32(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			return ((int)InputType).GetBytes();
		}
	}

	public TileData data;

	public override bool IfRebirthRecord
	{
		get
		{
			return false;
		}
	}

	protected override void OnCollideBall(BaseRole ball)
	{
		base.OnCollideBall(ball);
		ball.ChangeInputType(data.InputType);
	}

	public override void Read(string info)
	{
		if (!string.IsNullOrEmpty(info))
		{
			data = JsonUtility.FromJson<TileData>(info);
		}
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
