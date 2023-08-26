namespace Foundation
{
	public sealed class VarDouble : Var<double>
	{
		public VarDouble()
		{
		}

		public VarDouble(double value)
			: base(value)
		{
		}

		public static implicit operator VarDouble(double value)
		{
			return new VarDouble(value);
		}

		public static implicit operator double(VarDouble value)
		{
			return value.Value;
		}
	}
}
