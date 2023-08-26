using System;
using System.Runtime.CompilerServices;

namespace Foundation
{
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class ExecuteOrderAttribute : Attribute
	{
		[CompilerGenerated]
		private readonly int _003COrder_003Ek__BackingField;

		public int Order
		{
			[CompilerGenerated]
			get
			{
				return _003COrder_003Ek__BackingField;
			}
		}

		public ExecuteOrderAttribute(int order)
		{
			_003COrder_003Ek__BackingField = order;
		}
	}
}
