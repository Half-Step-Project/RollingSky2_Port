namespace Foundation
{
	public sealed class VarInt : Var<int>
	{
		public VarInt()
		{
		}

		public VarInt(int value)
			: base(value)
		{
		}

		public static implicit operator VarInt(int value)
		{
			return new VarInt(value);
		}

		public static implicit operator int(VarInt value)
		{
			return value.Value;
		}
	}
}
