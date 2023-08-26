using System;

namespace Foundation
{
	public struct ResourceName : IEquatable<ResourceName>, IComparable<ResourceName>
	{
		private readonly int _hashCode;

		private readonly string _name;

		public ResourceName(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				Log.Error("name is null.");
			}
			_name = name;
			string name2 = _name;
			_hashCode = ((name2 != null) ? name2.Length : 0);
		}

		public override string ToString()
		{
			return _name;
		}

		public override int GetHashCode()
		{
			return _hashCode;
		}

		public override bool Equals(object obj)
		{
			object obj2;
			if ((obj2 = obj) is ResourceName)
			{
				ResourceName other = (ResourceName)obj2;
				return Equals(other);
			}
			return false;
		}

		public bool Equals(ResourceName other)
		{
			return _name.Equals(other._name, StringComparison.OrdinalIgnoreCase);
		}

		public int CompareTo(ResourceName other)
		{
			return string.Compare(_name, other._name, StringComparison.OrdinalIgnoreCase);
		}

		public static bool operator ==(ResourceName x, ResourceName y)
		{
			return x.Equals(y);
		}

		public static bool operator !=(ResourceName x, ResourceName y)
		{
			return !x.Equals(y);
		}
	}
}
