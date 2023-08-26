using UnityEngine;

namespace Foundation
{
	public sealed class VarVector3 : Var<Vector3>
	{
		public VarVector3()
		{
		}

		public VarVector3(Vector3 value)
			: base(value)
		{
		}

		public static implicit operator VarVector3(Vector3 value)
		{
			return new VarVector3(value);
		}

		public static implicit operator Vector3(VarVector3 value)
		{
			return value.Value;
		}
	}
}
