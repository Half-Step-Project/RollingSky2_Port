using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Foundation
{
	public sealed class ResourceNameComparer : IEqualityComparer<ResourceName>, IComparer<ResourceName>
	{
		[CompilerGenerated]
		private static readonly ResourceNameComparer _003CDefault_003Ek__BackingField = new ResourceNameComparer();

		public static ResourceNameComparer Default
		{
			[CompilerGenerated]
			get
			{
				return _003CDefault_003Ek__BackingField;
			}
		}

		public bool Equals(ResourceName x, ResourceName y)
		{
			return x.Equals(y);
		}

		public int GetHashCode(ResourceName obj)
		{
			return obj.GetHashCode();
		}

		public int Compare(ResourceName x, ResourceName y)
		{
			return x.CompareTo(y);
		}
	}
}
