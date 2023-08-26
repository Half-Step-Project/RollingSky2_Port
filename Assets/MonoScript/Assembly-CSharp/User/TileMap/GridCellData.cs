using System;
using System.Collections.Generic;

namespace User.TileMap
{
	[Serializable]
	public class GridCellData
	{
		public Point m_point;

		public List<SubstanceData> m_substanceDatas = new List<SubstanceData>();
	}
}
