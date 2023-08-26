using UnityEngine;

namespace Foundation
{
	public sealed class VarGObject : Var<GameObject>
	{
		public VarGObject()
		{
		}

		public VarGObject(GameObject value)
			: base(value)
		{
		}

		public static implicit operator VarGObject(GameObject value)
		{
			return new VarGObject(value);
		}

		public static implicit operator GameObject(VarGObject value)
		{
			return value.Value;
		}
	}
}
