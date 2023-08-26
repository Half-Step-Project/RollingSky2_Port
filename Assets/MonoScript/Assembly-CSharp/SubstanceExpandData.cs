using User.TileMap;

public struct SubstanceExpandData
{
	public int m_gridID;

	public Point m_point;

	public TileObjectType m_brushType;

	public int m_brushID;

	public SubstanceExpandData(int gridID, Point point, TileObjectType brushType, int brushID)
	{
		m_gridID = gridID;
		m_point = point;
		m_brushType = brushType;
		m_brushID = brushID;
	}

	public override int GetHashCode()
	{
		return m_gridID.GetHashCode() ^ m_point.GetHashCode() ^ m_brushType.GetHashCode() ^ m_brushID.GetHashCode();
	}

	public bool Equals(SubstanceExpandData other)
	{
		if (m_gridID == other.m_gridID && m_point == other.m_point && m_brushType == other.m_brushType && m_brushID == other.m_brushID)
		{
			return true;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is SubstanceExpandData)
		{
			return Equals((SubstanceExpandData)obj);
		}
		return false;
	}
}
