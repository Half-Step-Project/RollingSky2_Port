using System.Collections.Generic;
using UnityEngine;

namespace User.TileMap
{
	public static class TileMapScripTableExtended
	{
		public static GridData GetGridDataByID(this List<GridData> gridDatas, int id)
		{
			GridData result = null;
			for (int i = 0; i < gridDatas.Count; i++)
			{
				if (gridDatas[i] != null && gridDatas[i].m_id == id)
				{
					result = gridDatas[i];
					break;
				}
			}
			return result;
		}

		public static GridData GetGridDataByParentID(this List<GridData> gridDatas, int parentID)
		{
			GridData gridData = null;
			for (int i = 0; i < gridDatas.Count; i++)
			{
				if (gridDatas[i] == null)
				{
					continue;
				}
				for (int j = 0; j < gridDatas[i].m_parentIds.Count; j++)
				{
					if (gridDatas[i].m_parentIds[j] == parentID)
					{
						gridData = gridDatas[i];
						break;
					}
				}
				if (gridData != null)
				{
					break;
				}
			}
			return gridData;
		}

		public static List<GridData> GetGridDatasByParentID(this List<GridData> gridDatas, int parentID)
		{
			List<GridData> list = new List<GridData>();
			for (int i = 0; i < gridDatas.Count; i++)
			{
				if (gridDatas[i] == null)
				{
					continue;
				}
				for (int j = 0; j < gridDatas[i].m_parentIds.Count; j++)
				{
					if (gridDatas[i].m_parentIds[j] == parentID)
					{
						list.Add(gridDatas[i]);
					}
				}
			}
			return list;
		}

		public static Dictionary<int, GridData> GetDictionaryByGridDatas(this List<GridData> gridDatas)
		{
			Dictionary<int, GridData> dictionary = new Dictionary<int, GridData>();
			for (int i = 0; i < gridDatas.Count; i++)
			{
				dictionary[gridDatas[i].m_id] = gridDatas[i];
			}
			return dictionary;
		}

		public static GridCellData GetGridCellDataByPoint(this List<GridCellData> gridCellDatas, Point point)
		{
			GridCellData result = null;
			for (int i = 0; i < gridCellDatas.Count; i++)
			{
				if (gridCellDatas[i] != null && gridCellDatas[i].m_point == point)
				{
					result = gridCellDatas[i];
					break;
				}
			}
			return result;
		}

		public static Dictionary<Point, GridCellData> ListToDictionary(this List<GridCellData> gridCellDatas)
		{
			Dictionary<Point, GridCellData> dictionary = new Dictionary<Point, GridCellData>(gridCellDatas.Count);
			for (int i = 0; i < gridCellDatas.Count; i++)
			{
				GridCellData gridCellData = gridCellDatas[i];
				dictionary.Add(gridCellData.m_point, gridCellData);
			}
			return dictionary;
		}

		public static Dictionary<int, List<GridCellData>> ListToRowDictionary(this Dictionary<Point, GridCellData> GridCellDataDic, int row, int height)
		{
			Dictionary<int, List<GridCellData>> dictionary = new Dictionary<int, List<GridCellData>>(height);
			for (int i = 0; i < height; i++)
			{
				List<GridCellData> list = new List<GridCellData>();
				for (int j = 0; j < row; j++)
				{
					GridCellData value = null;
					GridCellDataDic.TryGetValue(new Point(j, i), out value);
					list.Add(value);
				}
				dictionary[i] = list;
			}
			return dictionary;
		}

		public static Dictionary<int, List<GridCellData>> ListToRowDictionary(this List<GridCellData> gridCellDatas, int row, int height)
		{
			return gridCellDatas.ListToDictionary().ListToRowDictionary(row, height);
		}

		public static GridCellData GetGridCellDataByPoint(this Dictionary<Point, GridCellData> gridCellDatas, Point point)
		{
			GridCellData value = null;
			gridCellDatas.TryGetValue(point, out value);
			return value;
		}

		public static SubstanceUUIDPointer GetSubstanceDataBySubstanceUUIDPointer(this List<SubstanceUUIDPointer> pointers, int uuid)
		{
			SubstanceUUIDPointer result = null;
			for (int i = 0; i < pointers.Count; i++)
			{
				if (pointers[i].m_uuid == uuid)
				{
					result = pointers[i];
				}
			}
			return result;
		}

		private static void OnSpliceGrid(ref GridData currentGridData, GridData gridData)
		{
			if (currentGridData != null && gridData != null)
			{
				int height = currentGridData.m_height;
				currentGridData.m_width = Mathf.Max(currentGridData.m_width, gridData.m_width);
				currentGridData.m_height += gridData.m_height;
				currentGridData.m_samplingInterval = Mathf.Max(currentGridData.m_samplingInterval, gridData.m_samplingInterval);
				currentGridData.m_samplingCenterY = Mathf.Max(currentGridData.m_samplingCenterY, gridData.m_samplingCenterY);
				Dictionary<Point, GridCellData>.Enumerator enumerator = gridData.m_gridCellDatasDictionary.GetEnumerator();
				while (enumerator.MoveNext())
				{
					GridCellData gridCellData = new GridCellData();
					gridCellData.m_point = new Point(enumerator.Current.Key.m_x, enumerator.Current.Key.m_y + height);
					gridCellData.m_substanceDatas = enumerator.Current.Value.m_substanceDatas;
					currentGridData.m_gridCellDatas.Add(gridCellData);
				}
			}
		}

		public static GridData SpliceGrid(this GridData currentGridData, params GridData[] gridDatas)
		{
			if (currentGridData == null || gridDatas == null)
			{
				return null;
			}
			GridData currentGridData2 = JsonUtility.FromJson<GridData>(JsonUtility.ToJson(currentGridData));
			for (int i = 0; i < gridDatas.Length; i++)
			{
				OnSpliceGrid(ref currentGridData2, gridDatas[i]);
			}
			return currentGridData2;
		}

		public static void OnMergeGrid(ref GridData currentGridData, GridData gridData)
		{
			if (currentGridData == null || gridData == null)
			{
				return;
			}
			currentGridData.m_width = Mathf.Max(currentGridData.m_width, gridData.m_width);
			currentGridData.m_height = Mathf.Max(currentGridData.m_height, gridData.m_height);
			currentGridData.m_samplingInterval = Mathf.Max(currentGridData.m_samplingInterval, gridData.m_samplingInterval);
			currentGridData.m_samplingCenterY = Mathf.Max(currentGridData.m_samplingCenterY, gridData.m_samplingCenterY);
			currentGridData.m_gridCellDatasDictionary = currentGridData.m_gridCellDatas.ListToDictionary();
			Dictionary<Point, GridCellData>.Enumerator enumerator = gridData.m_gridCellDatas.ListToDictionary().GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (currentGridData.m_gridCellDatasDictionary.ContainsKey(enumerator.Current.Key))
				{
					currentGridData.m_gridCellDatasDictionary[enumerator.Current.Key].m_substanceDatas.AddRange(enumerator.Current.Value.m_substanceDatas);
				}
				else
				{
					currentGridData.m_gridCellDatasDictionary.Add(enumerator.Current.Key, enumerator.Current.Value);
				}
			}
			currentGridData.m_gridCellDatas.Clear();
			Dictionary<Point, GridCellData>.Enumerator enumerator2 = currentGridData.m_gridCellDatasDictionary.GetEnumerator();
			while (enumerator2.MoveNext())
			{
				currentGridData.m_gridCellDatas.Add(enumerator2.Current.Value);
			}
		}

		public static GridData MergeGrid(this GridData currentGridData, params GridData[] gridDatas)
		{
			if (currentGridData == null || gridDatas == null)
			{
				return null;
			}
			GridData currentGridData2 = JsonUtility.FromJson<GridData>(JsonUtility.ToJson(currentGridData));
			for (int i = 0; i < gridDatas.Length; i++)
			{
				OnMergeGrid(ref currentGridData2, gridDatas[i]);
			}
			return currentGridData2;
		}
	}
}
