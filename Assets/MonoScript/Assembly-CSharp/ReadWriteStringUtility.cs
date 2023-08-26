using System.Text;
using Foundation;

public static class ReadWriteStringUtility
{
	public static string GetStringWithSize(this byte[] bytes, ref int startIndex)
	{
		string result = string.Empty;
		if (bytes == null)
		{
			Log.Warning("Value is invalid.");
			return result;
		}
		int @int = bytes.GetInt32(ref startIndex);
		if (@int > 0)
		{
			result = Encoding.UTF8.GetString(bytes, startIndex, @int);
		}
		startIndex += @int;
		return result;
	}

	public static byte[] GetBytesWithSize(this string value)
	{
		byte[] bytes = value.Length.GetBytes();
		byte[] array = new byte[0];
		if (!string.IsNullOrEmpty(value))
		{
			array = Encoding.UTF8.GetBytes(value);
		}
		byte[] array2 = new byte[bytes.Length + array.Length];
		bytes.CopyTo(array2, 0);
		array.CopyTo(array2, bytes.Length);
		return array2;
	}
}
