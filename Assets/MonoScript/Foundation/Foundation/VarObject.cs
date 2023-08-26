namespace Foundation
{
	public sealed class VarObject : Var<object>
	{
		public VarObject()
		{
		}

		public VarObject(object value)
			: base(value)
		{
		}
	}
}
