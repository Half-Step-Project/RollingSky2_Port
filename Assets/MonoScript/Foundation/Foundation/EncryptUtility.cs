namespace Foundation
{
	public static class EncryptUtility
	{
		public const int QuickEncryptLength = 220;

		public static byte[] GetQuickXorBytes(byte[] bytes, byte[] code)
		{
			return GetXorBytes(bytes, code, 220);
		}

		public static byte[] GetXorBytes(byte[] bytes, byte[] code, int length = 0)
		{
			if (bytes == null)
			{
				return null;
			}
			if (code == null)
			{
				Log.Warning("Code is invalid.");
				return null;
			}
			int num = code.Length;
			if (num <= 0)
			{
				Log.Warning("Code length is invalid.");
				return null;
			}
			int num2 = bytes.Length;
			if (length <= 0 || length > num2)
			{
				length = num2;
			}
			int num3 = 0;
			for (int i = 0; i < length; i++)
			{
				bytes[i] ^= code[num3++];
				num3 %= num;
			}
			return bytes;
		}
	}
}
