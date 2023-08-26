using User.TileMap;

public struct BrushUUIDCreator
{
	public int m_gridID;

	public Point m_point;

	public TileObjectType m_brushType;

	public int m_brushID;

	public int m_index;

	public BrushUUIDCreator(int gridID, Point point, TileObjectType brushType, int brushID, int index)
	{
		m_gridID = gridID;
		m_point = point;
		m_brushType = brushType;
		m_brushID = brushID;
		m_index = index;
	}

	public override int GetHashCode()
	{
		return m_gridID.GetHashCode() ^ m_point.GetHashCode() ^ m_brushType.GetHashCode() ^ m_brushID.GetHashCode() ^ m_index.GetHashCode();
	}

	public bool Equals(BrushUUIDCreator other)
	{
		if (m_gridID == other.m_gridID && m_point == other.m_point && m_brushType == other.m_brushType && m_brushID == other.m_brushID && m_index == other.m_index)
		{
			return true;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is BrushUUIDCreator)
		{
			return Equals((BrushUUIDCreator)obj);
		}
		return false;
	}
}
