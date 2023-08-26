using UnityEngine;

namespace Foundation
{
	public sealed class VarTransform : Var<Transform>
	{
		public VarTransform()
		{
		}

		public VarTransform(Transform value)
			: base(value)
		{
		}

		public static implicit operator VarTransform(Transform value)
		{
			return new VarTransform(value);
		}

		public static implicit operator Transform(VarTransform value)
		{
			return value.Value;
		}
	}
}
