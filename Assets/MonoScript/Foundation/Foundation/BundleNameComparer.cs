using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Foundation
{
	public sealed class BundleNameComparer : IEqualityComparer<BundleName>, IComparer<BundleName>
	{
		[CompilerGenerated]
		private static readonly BundleNameComparer _003CDefault_003Ek__BackingField = new BundleNameComparer();

		public static BundleNameComparer Default
		{
			[CompilerGenerated]
			get
			{
				return _003CDefault_003Ek__BackingField;
			}
		}

		public bool Equals(BundleName x, BundleName y)
		{
			return x.Equals(y);
		}

		public int GetHashCode(BundleName obj)
		{
			return obj.GetHashCode();
		}

		public int Compare(BundleName x, BundleName y)
		{
			return x.CompareTo(y);
		}
	}
}
