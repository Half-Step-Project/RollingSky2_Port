using UnityEngine;

namespace Foundation
{
	public sealed class VarQuaternion : Var<Quaternion>
	{
		public VarQuaternion()
		{
		}

		public VarQuaternion(Quaternion value)
			: base(value)
		{
		}

		public static implicit operator VarQuaternion(Quaternion value)
		{
			return new VarQuaternion(value);
		}

		public static implicit operator Quaternion(VarQuaternion value)
		{
			return value.Value;
		}
	}
}
