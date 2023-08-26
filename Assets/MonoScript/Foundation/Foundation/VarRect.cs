using UnityEngine;

namespace Foundation
{
	public sealed class VarRect : Var<Rect>
	{
		public VarRect()
		{
		}

		public VarRect(Rect value)
			: base(value)
		{
		}

		public static implicit operator VarRect(Rect value)
		{
			return new VarRect(value);
		}

		public static implicit operator Rect(VarRect value)
		{
			return value.Value;
		}
	}
}
