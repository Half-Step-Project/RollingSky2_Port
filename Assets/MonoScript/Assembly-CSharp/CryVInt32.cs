using System;
using System.Collections.Generic;

public class CryVInt32 : IEqualityComparer<CryVInt32>
{
	public CryVBase m_dataBase;

	public int Value
	{
		get
		{
			return BitConverter.ToInt32(m_dataBase.Value, 0);
		}
		set
		{
			byte[] bytes = BitConverter.GetBytes(value);
			m_dataBase.Value = bytes;
		}
	}

	private CryVInt32(int realInt)
	{
		byte[] bytes = BitConverter.GetBytes(realInt);
		m_dataBase = new CryVBase(bytes);
		Value = realInt;
	}

	public static implicit operator int(CryVInt32 m)
	{
		if (m == null)
		{
			return 0;
		}
		return m.Value;
	}

	public static implicit operator CryVInt32(int value)
	{
		return new CryVInt32(value);
	}

	public bool Equals(CryVInt32 x, CryVInt32 y)
	{
		return x.Value == y.Value;
	}

	public int GetHashCode(CryVInt32 obj)
	{
		return obj.Value;
	}
}
