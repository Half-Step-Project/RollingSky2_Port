using System;
using System.IO;
using Foundation;

[Serializable]
public struct TrapTileData : IReadWriteBytes
{
	public TileDirection MoveDirection;

	public float MoveDistance;

	public float SpeedScaler;

	public float SecondPercent;

	public float MoveDistance2;

	public bool LeftBlock;

	public bool RightBlock;

	public bool UpBlock;

	public bool DownBlock;

	public void ReadBytes(byte[] bytes)
	{
		int startIndex = 0;
		MoveDirection = (TileDirection)bytes.GetInt32(ref startIndex);
		MoveDistance = bytes.GetSingle(ref startIndex);
		SpeedScaler = bytes.GetSingle(ref startIndex);
		SecondPercent = bytes.GetSingle(ref startIndex);
		MoveDistance2 = bytes.GetSingle(ref startIndex);
		LeftBlock = bytes.GetBoolean(ref startIndex);
		RightBlock = bytes.GetBoolean(ref startIndex);
		UpBlock = bytes.GetBoolean(ref startIndex);
		DownBlock = bytes.GetBoolean(ref startIndex);
	}

	public byte[] WriteBytes()
	{
		using (MemoryStream memoryStream = new MemoryStream())
		{
			int offset = 0;
			memoryStream.WriteByteArray(((int)MoveDirection).GetBytes(), ref offset);
			memoryStream.WriteByteArray(MoveDistance.GetBytes(), ref offset);
			memoryStream.WriteByteArray(SpeedScaler.GetBytes(), ref offset);
			memoryStream.WriteByteArray(SecondPercent.GetBytes(), ref offset);
			memoryStream.WriteByteArray(MoveDistance2.GetBytes(), ref offset);
			memoryStream.WriteByteArray(LeftBlock.GetBytes(), ref offset);
			memoryStream.WriteByteArray(RightBlock.GetBytes(), ref offset);
			memoryStream.WriteByteArray(UpBlock.GetBytes(), ref offset);
			memoryStream.WriteByteArray(DownBlock.GetBytes(), ref offset);
			memoryStream.Flush();
			memoryStream.Seek(0L, SeekOrigin.Begin);
			return memoryStream.ToArray();
		}
	}
}
