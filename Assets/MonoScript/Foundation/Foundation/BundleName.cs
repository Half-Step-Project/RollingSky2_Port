using System;

namespace Foundation
{
	public struct BundleName : IEquatable<BundleName>, IComparable<BundleName>
	{
		private readonly int _hashCode;

		private readonly string _fullName;

		private readonly string _name;

		private readonly string _variant;

		public string Name
		{
			get
			{
				return _name;
			}
		}

		public string Variant
		{
			get
			{
				return _variant;
			}
		}

		public bool IsVariant
		{
			get
			{
				return !string.IsNullOrEmpty(_variant);
			}
		}

		public BundleName(string name, string variant)
		{
			if (string.IsNullOrEmpty(name))
			{
				Log.Error("name is null.");
			}
			_name = name;
			_variant = variant;
			_fullName = (string.IsNullOrEmpty(variant) ? name : (name + "." + variant));
			string fullName = _fullName;
			_hashCode = ((fullName != null) ? fullName.Length : 0);
		}

		public override string ToString()
		{
			return _fullName;
		}

		public override int GetHashCode()
		{
			return _hashCode;
		}

		public override bool Equals(object obj)
		{
			object obj2;
			if ((obj2 = obj) is BundleName)
			{
				BundleName other = (BundleName)obj2;
				return Equals(other);
			}
			return false;
		}

		public bool Equals(BundleName other)
		{
			return _fullName.Equals(other._fullName, StringComparison.OrdinalIgnoreCase);
		}

		public int CompareTo(BundleName other)
		{
			return string.Compare(_fullName, other._fullName, StringComparison.OrdinalIgnoreCase);
		}

		public static bool operator ==(BundleName x, BundleName y)
		{
			return x.Equals(y);
		}

		public static bool operator !=(BundleName x, BundleName y)
		{
			return !x.Equals(y);
		}
	}
}
