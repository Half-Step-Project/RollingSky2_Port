namespace Foundation
{
	public interface IZipHandler
	{
		byte[] Compress(byte[] bytes);

		byte[] Decompress(byte[] bytes);
	}
}
