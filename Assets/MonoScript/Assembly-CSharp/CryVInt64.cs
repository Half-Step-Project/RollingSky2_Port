using System;

public class CryVInt64 : IComparable<CryVInt64>
{
	public CryVBase m_dataBase;

	public long Value
	{
		get
		{
			return BitConverter.ToInt64(m_dataBase.Value, 0);
		}
		set
		{
			byte[] bytes = BitConverter.GetBytes(value);
			m_dataBase.Value = bytes;
		}
	}

	private CryVInt64(long realInt)
	{
		byte[] bytes = BitConverter.GetBytes(realInt);
		m_dataBase = new CryVBase(bytes);
		Value = realInt;
	}

	public static implicit operator long(CryVInt64 m)
	{
		if (m == null)
		{
			return 0L;
		}
		return m.Value;
	}

	public static implicit operator CryVInt64(long value)
	{
		return new CryVInt64(value);
	}

	public bool Equals(CryVInt64 x, CryVInt64 y)
	{
		return x.Value == y.Value;
	}

	int IComparable<CryVInt64>.CompareTo(CryVInt64 other)
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
