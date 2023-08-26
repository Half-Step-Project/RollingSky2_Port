using System;

namespace Foundation
{
	public abstract class Var<T> : IVar
	{
		private T _value;

		public Type ValueType
		{
			get
			{
				return typeof(T);
			}
		}

		public T Value
		{
			get
			{
				return _value;
			}
			set
			{
				_value = value;
			}
		}

		object IVar.Value
		{
			get
			{
				return _value;
			}
			set
			{
				_value = (T)value;
			}
		}

		protected Var()
		{
			_value = default(T);
		}

		protected Var(T value)
		{
			_value = value;
		}

		public void Reset()
		{
			_value = default(T);
		}

		public override string ToString()
		{
			return _value.ToString();
		}
	}
}
