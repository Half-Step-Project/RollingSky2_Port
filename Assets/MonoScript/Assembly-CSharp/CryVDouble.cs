using System;

public class CryVDouble : IComparable<CryVDouble>
{
	public CryVBase m_dataBase;

	public double Value
	{
		get
		{
			return BitConverter.ToDouble(m_dataBase.Value, 0);
		}
		set
		{
			byte[] bytes = BitConverter.GetBytes(value);
			m_dataBase.Value = bytes;
		}
	}

	private CryVDouble(double realInt)
	{
		byte[] bytes = BitConverter.GetBytes(realInt);
		m_dataBase = new CryVBase(bytes);
		Value = realInt;
	}

	public static implicit operator double(CryVDouble m)
	{
		if (m == null)
		{
			return 0.0;
		}
		return m.Value;
	}

	public static implicit operator CryVDouble(double value)
	{
		return new CryVDouble(value);
	}

	public bool Equals(CryVDouble x, CryVDouble y)
	{
		return x.Value == y.Value;
	}

	int IComparable<CryVDouble>.CompareTo(CryVDouble other)
	{
		if (Value > other.Value)
		{
			return 1;
		}
		if (Value == other.Value)
		{
			return 0;
		}
		return -1;
	}
}
