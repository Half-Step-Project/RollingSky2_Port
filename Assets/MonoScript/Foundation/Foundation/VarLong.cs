namespace Foundation
{
	public sealed class VarLong : Var<long>
	{
		public VarLong()
		{
		}

		public VarLong(long value)
			: base(value)
		{
		}

		public static implicit operator VarLong(long value)
		{
			return new VarLong(value);
		}

		public static implicit operator long(VarLong value)
		{
			return value.Value;
		}
	}
}
