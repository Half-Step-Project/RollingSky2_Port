using System;
using System.Runtime.InteropServices;
using Foundation;
using UnityEngine;

public sealed class EndQTEJumpTile : BaseTile
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct TileData : IReadWriteBytes
	{
		public void ReadBytes(byte[] bytes)
		{
		}

		public byte[] WriteBytes()
		{
			return new byte[0];
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

	public override void Initialize()
	{
		base.Initialize();
	}

	protected override void OnCollideBall(BaseRole ball)
	{
		base.OnCollideBall(ball);
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
