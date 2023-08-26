using System;

public struct DropData : IEquatable<DropData>
{
	public int m_uuid;

	public DropType m_type;

	public int m_sortID;

	public int m_number;

	public DropData(int uuid, DropType type, int sortID, int number = 1)
	{
		m_uuid = uuid;
		m_type = type;
		m_number = number;
		m_sortID = sortID;
	}

	public override int GetHashCode()
	{
		return (m_uuid << 1) ^ ((int)m_type << 2);
	}

	public override bool Equals(object obj)
	{
		if (obj != null && obj is DropData)
		{
			return Equals((DropData)obj);
		}
		return false;
	}

	public bool Equals(DropData other)
	{
		if (m_uuid == other.m_uuid)
		{
			return m_type == other.m_type;
		}
		return false;
	}
}
