namespace Foundation
{
	public sealed class VarChars : Var<char[]>
	{
		public VarChars()
		{
		}

		public VarChars(char[] value)
			: base(value)
		{
		}

		public static implicit operator VarChars(char[] value)
		{
			return new VarChars(value);
		}

		public static implicit operator char[](VarChars value)
		{
			return value.Value;
		}
	}
}
