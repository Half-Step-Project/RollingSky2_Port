namespace Foundation
{
	public sealed class VarUShort : Var<ushort>
	{
		public VarUShort()
		{
		}

		public VarUShort(ushort value)
			: base(value)
		{
		}

		public static implicit operator VarUShort(ushort value)
		{
			return new VarUShort(value);
		}

		public static implicit operator ushort(VarUShort value)
		{
			return value.Value;
		}
	}
}
