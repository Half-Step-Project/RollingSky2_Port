using System;

namespace Foundation
{
	public interface IVar
	{
		object Value { get; set; }

		Type ValueType { get; }

		void Reset();
	}
}
