using System;

namespace Foundation
{
	public static class WebUtility
	{
		public static string EscapeString(string uri)
		{
			return Uri.EscapeDataString(uri);
		}

		public static string UnEscapeString(string uri)
		{
			return Uri.UnescapeDataString(uri);
		}
	}
}
