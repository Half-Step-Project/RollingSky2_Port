using UnityEngine;

namespace Foundation
{
	public sealed class VarUObject : Var<Object>
	{
		public VarUObject()
		{
		}

		public VarUObject(Object value)
			: base(value)
		{
		}

		public static implicit operator VarUObject(Object value)
		{
			return new VarUObject(value);
		}

		public static implicit operator Object(VarUObject value)
		{
			return value.Value;
		}
	}
}
