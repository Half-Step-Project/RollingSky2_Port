namespace Foundation
{
	public sealed class VarShort : Var<short>
	{
		public VarShort()
		{
		}

		public VarShort(short value)
			: base(value)
		{
		}

		public static implicit operator VarShort(short value)
		{
			return new VarShort(value);
		}

		public static implicit operator short(VarShort value)
		{
			return value.Value;
		}
	}
}
