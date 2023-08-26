namespace Foundation
{
	public static class NullableUtility
	{
		public static bool IsNullable<T>(T t)
		{
			return false;
		}

		public static bool IsNullable<T>(T? t) where T : struct
		{
			return true;
		}
	}
}
