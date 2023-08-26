using System;

namespace User.TileMap
{
	[Serializable]
	public struct ParentInfo
	{
		public static readonly int GroupScaler = 100000000;

		public static readonly int GridScaler = 1000000;

		public static readonly int CellXScaler = 1000;

		public static readonly int CellYScaler = 1;

		public int m_gridId;

		public Point m_point;

		public int m_isGroup;

		public int m_key
		{
			get
			{
				return m_isGroup * 100000000 + m_gridId * 1000000 + m_point.m_x * 1000 + m_point.m_y;
			}
		}

		public void Reset()
		{
			m_gridId = -1;
			m_isGroup = 0;
			m_point = default(Point);
		}

		public static int GetKeyBy(GridData grid, GridCellData cell, bool isGroup = false)
		{
			return (isGroup ? 1 : 0) * GroupScaler + grid.m_id * GridScaler + cell.m_point.m_x * CellXScaler + cell.m_point.m_y * CellYScaler;
		}

		public static int GetKeyBy(int gridId, Point point, bool isGroup = false)
		{
			return (isGroup ? 1 : 0) * GroupScaler + gridId * GridScaler + point.m_x * CellXScaler + point.m_y * CellYScaler;
		}

		public static bool IfGroup(int parentKey)
		{
			return parentKey >= GroupScaler;
		}
	}
}
