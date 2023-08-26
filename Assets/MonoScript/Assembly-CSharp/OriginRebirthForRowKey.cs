using System;

[Serializable]
public struct OriginRebirthForRowKey : IEquatable<OriginRebirthForRowKey>
{
	public int m_gridID;

	public int m_row;

	public OriginRebirthForRowKey(int gridID, int row)
	{
		m_gridID = gridID;
		m_row = row;
	}

	public void SetData(int gridID, int row)
	{
		m_gridID = gridID;
		m_row = row;
	}

	public override int GetHashCode()
	{
		return m_gridID.GetHashCode() ^ m_row.GetHashCode();
	}

	public bool Equals(OriginRebirthForRowKey other)
	{
		if (m_gridID == other.m_gridID && m_row == other.m_row)
		{
			return true;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is OriginRebirthForRowKey)
		{
			return Equals((OriginRebirthForRowKey)obj);
		}
		return false;
	}

	public override string ToString()
	{
		return string.Format("gridID={0},row={1}", m_gridID, m_row);
	}
}
