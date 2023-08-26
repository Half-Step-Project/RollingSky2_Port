using System.IO;

namespace Foundation
{
	public static class StreamExtension
	{
		public static void WriteByteArray(this MemoryStream stream, byte[] bytes, ref int offset)
		{
			int num = bytes.Length;
			stream.Write(bytes, 0, num);
			offset += num;
		}
	}
}
