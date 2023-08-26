using System.Collections.Generic;

namespace User.TileMap
{
	public class PathData
	{
		public List<GridData> pathPoints = new List<GridData>();

		public GridData worldGridData = new GridData();

		public bool isSearchFinished;

		public Dictionary<string, int> bufferData = new Dictionary<string, int>();

		public PathData(GridData _gridData)
		{
			pathPoints.Add(_gridData);
		}

		public PathData(List<GridData> _pathPoints, GridData _gridData)
		{
			pathPoints.AddRange(_pathPoints);
			pathPoints.Add(_gridData);
		}

		public void AddPathPoint(GridData _gridData)
		{
			pathPoints.Add(_gridData);
		}

		public void EndSearchPath()
		{
			isSearchFinished = true;
			RankByPoint();
			GetBufferSizeByRowNum();
		}

		public int GetEndGridDataId()
		{
			return pathPoints[pathPoints.Count - 1].m_id;
		}

		public void RankByPoint()
		{
			int num = 0;
			for (int i = 0; i < pathPoints.Count; i++)
			{
				pathPoints[i].m_gridCellDatas.Sort(delegate(GridCellData r, GridCellData l)
				{
					int result = 0;
					if (r.m_point.m_y == l.m_point.m_y)
					{
						if (r.m_point.m_x == l.m_point.m_x)
						{
							result = 0;
						}
						else if (r.m_point.m_x > l.m_point.m_x)
						{
							result = 1;
						}
						else if (r.m_point.m_x < l.m_point.m_x)
						{
							result = -1;
						}
					}
					else if (r.m_point.m_y > l.m_point.m_y)
					{
						result = 1;
					}
					else if (r.m_point.m_y < l.m_point.m_y)
					{
						result = -1;
					}
					return result;
				});
				for (int j = 0; j < pathPoints[i].m_gridCellDatas.Count; j++)
				{
					GridCellData gridCellData = pathPoints[i].m_gridCellDatas[j];
					worldGridData.m_gridCellDatas.Add(gridCellData);
					Point key = new Point(gridCellData.m_point.m_x, num + gridCellData.m_point.m_y);
					worldGridData.m_gridCellDatasDictionary[key] = pathPoints[i].m_gridCellDatas[j];
				}
				num += pathPoints[i].m_height;
				worldGridData.m_width = pathPoints[i].m_width;
			}
			worldGridData.m_height = num;
		}

		public void GetBufferSizeByRowNum()
		{
			for (int i = 0; i <= worldGridData.m_height - TileMapTableParentFilter.BufferedRowNum; i++)
			{
				Dictionary<string, int> dictionary = new Dictionary<string, int>();
				for (int j = i; j <= i + TileMapTableParentFilter.BufferedRowNum; j++)
				{
					for (int k = 0; k < worldGridData.m_width; k++)
					{
						Point key = new Point(k, j);
						if (!worldGridData.m_gridCellDatasDictionary.ContainsKey(key))
						{
							continue;
						}
						GridCellData gridCellData = worldGridData.m_gridCellDatasDictionary[key];
						for (int l = 0; l < gridCellData.m_substanceDatas.Count; l++)
						{
							SubstanceData substanceData = gridCellData.m_substanceDatas[l];
							if (substanceData.m_type != 3)
							{
								string key2 = substanceData.m_type + "_" + substanceData.m_id;
								if (dictionary.ContainsKey(key2))
								{
									dictionary[key2]++;
								}
								else
								{
									dictionary[key2] = 1;
								}
							}
						}
					}
				}
				Dictionary<string, int>.Enumerator enumerator = dictionary.GetEnumerator();
				while (enumerator.MoveNext())
				{
					string key3 = enumerator.Current.Key;
					int value = enumerator.Current.Value;
					if (bufferData.ContainsKey(key3))
					{
						if (bufferData[key3] < value)
						{
							bufferData[key3] = value;
						}
					}
					else
					{
						bufferData[key3] = value;
					}
				}
			}
		}
	}
}
