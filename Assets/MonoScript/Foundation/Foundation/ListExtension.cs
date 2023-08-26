using System.Collections.Generic;

namespace Foundation
{
	public static class ListExtension
	{
		public static T RemoveLast<T>(this List<T> list)
		{
			int index = list.Count - 1;
			T result = list[index];
			list.RemoveAt(index);
			return result;
		}
	}
}
