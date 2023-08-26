using UnityEngine;

namespace Foundation
{
	public sealed class VarVector2 : Var<Vector2>
	{
		public VarVector2()
		{
		}

		public VarVector2(Vector2 value)
			: base(value)
		{
		}

		public static implicit operator VarVector2(Vector2 value)
		{
			return new VarVector2(value);
		}

		public static implicit operator Vector2(VarVector2 value)
		{
			return value.Value;
		}
	}
}
