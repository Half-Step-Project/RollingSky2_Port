namespace Foundation
{
	public static class Zip
	{
		public static IZipHandler Handler { get; set; }

		public static byte[] Compress(byte[] bytes)
		{
			if (Handler == null)
			{
				Log.Warning("Zip handler is invalid.");
				return bytes;
			}
			return Handler.Compress(bytes);
		}

		public static byte[] Decompress(byte[] bytes)
		{
			if (Handler == null)
			{
				Log.Warning("Zip handler is invalid.");
				return bytes;
			}
			return Handler.Decompress(bytes);
		}
	}
}
