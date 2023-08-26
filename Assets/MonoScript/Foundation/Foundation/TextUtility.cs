using System.Collections.Generic;

namespace Foundation
{
	public static class TextUtility
	{
		public static string[] SplitToLines(string text)
		{
			List<string> list = new List<string>();
			int position = 0;
			string item;
			while ((item = ReadLine(text, ref position)) != null)
			{
				list.Add(item);
			}
			return list.ToArray();
		}

		private static string ReadLine(string text, ref int position)
		{
			if (text == null)
			{
				return null;
			}
			int length = text.Length;
			int i;
			for (i = position; i < length; i++)
			{
				char c = text[i];
				if (c == '\n' || c == '\r')
				{
					string result = text.Substring(position, i - position);
					position = i + 1;
					if (c == '\r' && position < length && text[position] == '\n')
					{
						position++;
					}
					return result;
				}
			}
			if (i > position)
			{
				string result2 = text.Substring(position, i - position);
				position = i;
				return result2;
			}
			return null;
		}
	}
}
