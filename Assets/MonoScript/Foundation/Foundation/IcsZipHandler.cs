using System.IO;
using System.Runtime.CompilerServices;
using ICSharpCode.SharpZipLib.GZip;

namespace Foundation
{
	public sealed class IcsZipHandler : IZipHandler
	{
		[CompilerGenerated]
		private static readonly IcsZipHandler _003CDefault_003Ek__BackingField = new IcsZipHandler();

		public static IcsZipHandler Default
		{
			[CompilerGenerated]
			get
			{
				return _003CDefault_003Ek__BackingField;
			}
		}

		public byte[] Compress(byte[] bytes)
		{
			Log.Warning("Compress is not implemented.");
			return bytes;
		}

		public byte[] Decompress(byte[] bytes)
		{
			if (bytes == null || bytes.Length == 0)
			{
				Log.Warning("Argument is invalid.");
				return bytes;
			}
			MemoryStream memoryStream = null;
			MemoryStream memoryStream2 = null;
			try
			{
				memoryStream2 = new MemoryStream(bytes);
				memoryStream = new MemoryStream();
				using (GZipInputStream gZipInputStream = new GZipInputStream(memoryStream2))
				{
					memoryStream2 = null;
					byte[] array = new byte[4096];
					int count;
					while ((count = gZipInputStream.Read(array, 0, array.Length)) != 0)
					{
						memoryStream.Write(array, 0, count);
					}
				}
				return memoryStream.ToArray();
			}
			finally
			{
				if (memoryStream != null)
				{
					memoryStream.Dispose();
				}
				if (memoryStream2 != null)
				{
					memoryStream2.Dispose();
				}
			}
		}
	}
}
