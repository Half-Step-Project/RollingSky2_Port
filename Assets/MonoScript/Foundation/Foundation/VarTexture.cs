using UnityEngine;

namespace Foundation
{
	public sealed class VarTexture : Var<Texture>
	{
		public VarTexture()
		{
		}

		public VarTexture(Texture value)
			: base(value)
		{
		}

		public static implicit operator VarTexture(Texture value)
		{
			return new VarTexture(value);
		}

		public static implicit operator Texture(VarTexture value)
		{
			return value.Value;
		}
	}
}
