using UnityEngine;

namespace Foundation
{
	public sealed class VarMaterial : Var<Material>
	{
		public VarMaterial()
		{
		}

		public VarMaterial(Material value)
			: base(value)
		{
		}

		public static implicit operator VarMaterial(Material value)
		{
			return new VarMaterial(value);
		}

		public static implicit operator Material(VarMaterial value)
		{
			return value.Value;
		}
	}
}
