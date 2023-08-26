using System;
using System.IO;
using Foundation;

[Serializable]
public struct GlassTileData : IReadWriteBytes
{
	public float BeginDistance;

	public float EndDistance;

	public float Acceleration;

	public float SpeedScaler;

	public bool LeftBlock;

	public bool RightBlock;

	public bool UpBlock;

	public bool DownBlock;

	public void ReadBytes(byte[] bytes)
	{
		int startIndex = 0;
		BeginDistance = bytes.GetSingle(ref startIndex);
		EndDistance = bytes.GetSingle(ref startIndex);
		Acceleration = bytes.GetSingle(ref startIndex);
		SpeedScaler = bytes.GetSingle(ref startIndex);
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
			memoryStream.WriteByteArray(BeginDistance.GetBytes(), ref offset);
			memoryStream.WriteByteArray(EndDistance.GetBytes(), ref offset);
			memoryStream.WriteByteArray(Acceleration.GetBytes(), ref offset);
			memoryStream.WriteByteArray(SpeedScaler.GetBytes(), ref offset);
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
