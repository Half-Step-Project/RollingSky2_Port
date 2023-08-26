namespace Foundation
{
	public sealed class VarSByte : Var<sbyte>
	{
		public VarSByte()
		{
		}

		public VarSByte(sbyte value)
			: base(value)
		{
		}

		public static implicit operator VarSByte(sbyte value)
		{
			return new VarSByte(value);
		}

		public static implicit operator sbyte(VarSByte value)
		{
			return value.Value;
		}
	}
}
