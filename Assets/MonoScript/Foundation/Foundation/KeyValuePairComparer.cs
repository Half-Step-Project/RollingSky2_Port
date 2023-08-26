using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Foundation
{
	public sealed class KeyValuePairComparer : IComparer<KeyValuePair<object, string>>, IEqualityComparer<KeyValuePair<object, string>>
	{
		[CompilerGenerated]
		private static readonly KeyValuePairComparer _003CDefault_003Ek__BackingField = new KeyValuePairComparer();

		public static KeyValuePairComparer Default
		{
			[CompilerGenerated]
			get
			{
				return _003CDefault_003Ek__BackingField;
			}
		}

		public int Compare(KeyValuePair<object, string> x, KeyValuePair<object, string> y)
		{
			if (x.Key == y.Key)
			{
				return string.Compare(x.Value, y.Value, StringComparison.Ordinal);
			}
			if (x.Key == null)
			{
				return -1;
			}
			return 1;
		}

		public bool Equals(KeyValuePair<object, string> x, KeyValuePair<object, string> y)
		{
			if (x.Key == y.Key)
			{
				return x.Value == y.Value;
			}
			return false;
		}

		public int GetHashCode(KeyValuePair<object, string> obj)
		{
			return obj.Key.GetHashCode() ^ obj.Value.GetHashCode();
		}
	}
}
