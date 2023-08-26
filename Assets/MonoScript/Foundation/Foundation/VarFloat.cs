namespace Foundation
{
	public sealed class VarFloat : Var<float>
	{
		public VarFloat()
		{
		}

		public VarFloat(float value)
			: base(value)
		{
		}

		public static implicit operator VarFloat(float value)
		{
			return new VarFloat(value);
		}

		public static implicit operator float(VarFloat value)
		{
			return value.Value;
		}
	}
}
