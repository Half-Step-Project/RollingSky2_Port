using System.Collections.Generic;
using RS2;
using UnityEngine;
using User.LightingMap;

namespace User.TileMap
{
	public class GridGroup : MonoBehaviour
	{
		public delegate void CallBack();

		public CallBack m_callBack;

		public Grid CreateNode(bool isFrist = false)
		{
			GameObject obj = new GameObject("Grid_" + (base.transform.childCount + 1));
			Grid grid = obj.AddComponent<Grid>();
			obj.transform.parent = base.transform;
			obj.layer = LayerMask.NameToLayer("TileMapGrid");
			grid.m_isFrist = isFrist;
			return grid;
		}

		public Grid IsOnGrid(Vector3 position)
		{
			Grid[] componentsInChildren = base.transform.GetComponentsInChildren<Grid>();
			Grid result = null;
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].IsOnGrid(position))
				{
					result = componentsInChildren[i];
					break;
				}
			}
			return result;
		}

		public Grid[] GetAllGrid()
		{
			return base.transform.GetComponentsInChildren<Grid>();
		}

		public TileMapScripTable EntryTileMapScripTable(bool isCreateLightingMapData = false)
		{
			TileMapScripTable tileMapScripTable = ScriptableObject.CreateInstance<TileMapScripTable>();
			Grid[] allGrid = GetAllGrid();
			for (int i = 0; i < allGrid.Length; i++)
			{
				allGrid[i].m_id = i;
			}
			for (int j = 0; j < allGrid.Length; j++)
			{
				GridData gridData = new GridData();
				gridData.m_id = j;
				gridData.m_parentIds = new List<int>();
				for (int k = 0; k < allGrid[j].m_parentGrids.Count; k++)
				{
					if (allGrid[j].m_parentGrids != null)
					{
						gridData.m_parentIds.Add(allGrid[j].m_parentGrids[k].m_id);
					}
				}
				gridData.m_samplingInterval = allGrid[j].m_samplingInterval;
				gridData.m_samplingCenterY = allGrid[j].m_samplingCenterY;
				gridData.m_position = allGrid[j].transform.position;
				gridData.m_rotation = allGrid[j].transform.eulerAngles;
				gridData.m_height = allGrid[j].m_x;
				gridData.m_width = allGrid[j].m_y;
				tileMapScripTable.m_gridDatas.Add(gridData);
			}
			BaseTile[] allBaseTiles = GetAllBaseTiles();
			BaseElement[] brushBases = allBaseTiles;
			Entry(ref tileMapScripTable, brushBases, isCreateLightingMapData);
			BaseEnemy[] allBaseEnemys = GetAllBaseEnemys();
			brushBases = allBaseEnemys;
			Entry(ref tileMapScripTable, brushBases, isCreateLightingMapData);
			BaseMidground[] allBaseMidgrounds = GetAllBaseMidgrounds();
			brushBases = allBaseMidgrounds;
			Entry(ref tileMapScripTable, brushBases, isCreateLightingMapData);
			BaseTriggerBox[] allBaseTriggerBoxs = GetAllBaseTriggerBoxs();
			brushBases = allBaseTriggerBoxs;
			Entry(ref tileMapScripTable, brushBases, isCreateLightingMapData);
			BaseGroup[] allBaseGroups = GetAllBaseGroups();
			brushBases = allBaseGroups;
			Entry(ref tileMapScripTable, brushBases, isCreateLightingMapData);
			for (int l = 0; l < tileMapScripTable.m_gridDatas.Count; l++)
			{
				tileMapScripTable.m_gridDatas[l].m_gridCellDatasDictionary = tileMapScripTable.m_gridDatas[l].m_gridCellDatas.ListToDictionary();
				List<GridCellData> list = new List<GridCellData>();
				for (int m = 0; m < tileMapScripTable.m_gridDatas[l].m_height; m++)
				{
					for (int n = 0; n < tileMapScripTable.m_gridDatas[l].m_width; n++)
					{
						if (tileMapScripTable.m_gridDatas[l].m_gridCellDatasDictionary.ContainsKey(new Point(n, m)))
						{
							list.Add(tileMapScripTable.m_gridDatas[l].m_gridCellDatasDictionary[new Point(n, m)]);
						}
					}
				}
				tileMapScripTable.m_gridDatas[l].m_gridCellDatas.Clear();
				tileMapScripTable.m_gridDatas[l].m_gridCellDatas.AddRange(list);
				tileMapScripTable.m_gridDatas[l].m_gridCellDatasDictionary = tileMapScripTable.m_gridDatas[l].m_gridCellDatas.ListToDictionary();
			}
			return tileMapScripTable;
		}

		public TileMapScripTable EntryTileMapFroGivenBrush<T>(bool isCreateLightingMapData, ref T[] ts) where T : BaseElement
		{
			TileMapScripTable tileMapScripTable = ScriptableObject.CreateInstance<TileMapScripTable>();
			Grid[] allGrid = GetAllGrid();
			for (int i = 0; i < allGrid.Length; i++)
			{
				allGrid[i].m_id = i;
			}
			for (int j = 0; j < allGrid.Length; j++)
			{
				GridData gridData = new GridData();
				gridData.m_id = j;
				gridData.m_parentIds = new List<int>();
				for (int k = 0; k < allGrid[j].m_parentGrids.Count; k++)
				{
					if (allGrid[j].m_parentGrids != null)
					{
						gridData.m_parentIds.Add(allGrid[j].m_parentGrids[k].m_id);
					}
				}
				gridData.m_samplingInterval = allGrid[j].m_samplingInterval;
				gridData.m_samplingCenterY = allGrid[j].m_samplingCenterY;
				gridData.m_position = allGrid[j].transform.position;
				gridData.m_rotation = allGrid[j].transform.eulerAngles;
				gridData.m_height = allGrid[j].m_x;
				gridData.m_width = allGrid[j].m_y;
				tileMapScripTable.m_gridDatas.Add(gridData);
			}
			BaseElement[] array = Object.FindObjectsOfType<T>();
			BaseElement[] array2 = array;
			ts = (T[])array2;
			Entry(ref tileMapScripTable, array2, isCreateLightingMapData);
			for (int l = 0; l < tileMapScripTable.m_gridDatas.Count; l++)
			{
				tileMapScripTable.m_gridDatas[l].m_gridCellDatasDictionary = tileMapScripTable.m_gridDatas[l].m_gridCellDatas.ListToDictionary();
				List<GridCellData> list = new List<GridCellData>();
				for (int m = 0; m < tileMapScripTable.m_gridDatas[l].m_height; m++)
				{
					for (int n = 0; n < tileMapScripTable.m_gridDatas[l].m_width; n++)
					{
						if (tileMapScripTable.m_gridDatas[l].m_gridCellDatasDictionary.ContainsKey(new Point(n, m)))
						{
							list.Add(tileMapScripTable.m_gridDatas[l].m_gridCellDatasDictionary[new Point(n, m)]);
						}
					}
				}
				tileMapScripTable.m_gridDatas[l].m_gridCellDatas.Clear();
				tileMapScripTable.m_gridDatas[l].m_gridCellDatas.AddRange(list);
				tileMapScripTable.m_gridDatas[l].m_gridCellDatasDictionary = tileMapScripTable.m_gridDatas[l].m_gridCellDatas.ListToDictionary();
			}
			return tileMapScripTable;
		}

		public GameObject[] GetNotInGrid()
		{
			List<GameObject> list = new List<GameObject>();
			BaseElement[] allBaseElements = GetAllBaseElements();
			for (int i = 0; i < allBaseElements.Length; i++)
			{
				if (IsOnGrid(allBaseElements[i].transform.position) == null)
				{
					list.Add(allBaseElements[i].gameObject);
				}
			}
			return list.ToArray();
		}

		public GameObject[] GetNotContainsKeyObjects()
		{
			List<GameObject> list = new List<GameObject>();
			BaseElement[] allBaseElements = GetAllBaseElements();
			for (int i = 0; i < allBaseElements.Length; i++)
			{
				if (BrushsManager.GetBrushDataByTypeAddBrushID(allBaseElements[i].GetTileObjectType, allBaseElements[i].m_id) == null)
				{
					list.Add(allBaseElements[i].gameObject);
				}
			}
			return list.ToArray();
		}

		public List<BaseElement> GetBaseElmentsByGridAddPoint(Grid grid, Point point)
		{
			List<BaseElement> list = new List<BaseElement>();
			BaseElement[] allBaseElements = GetAllBaseElements();
			if (grid.m_x >= point.m_y && grid.m_y >= point.m_x)
			{
				for (int i = 0; i < allBaseElements.Length; i++)
				{
					if (grid.IsOnGrid(allBaseElements[i].transform.position) && grid.GetPointByPosition(allBaseElements[i].transform.position) == point)
					{
						list.Add(allBaseElements[i]);
					}
				}
			}
			return list;
		}

		private TileMapScripTable Entry(ref TileMapScripTable tileMapScripTable, BaseElement[] brushBases, bool isCreateLightingMapData = false)
		{
			List<GameObject> list = new List<GameObject>();
			for (int i = 0; i < brushBases.Length; i++)
			{
				BaseElement baseElement = brushBases[i];
				Grid grid = IsOnGrid(baseElement.transform.position);
				if (grid != null)
				{
					Point pointByPosition = grid.GetPointByPosition(brushBases[i].transform.position);
					GridData gridDataByID = tileMapScripTable.m_gridDatas.GetGridDataByID(grid.m_id);
					if (gridDataByID == null)
					{
						Debug.Log("数据出错,无id为" + grid.m_id + "的数据");
						tileMapScripTable = null;
						break;
					}
					GridCellData gridCellData = gridDataByID.m_gridCellDatas.GetGridCellDataByPoint(pointByPosition);
					if (gridCellData == null)
					{
						gridCellData = new GridCellData();
						gridCellData.m_point = pointByPosition;
						gridDataByID.m_gridCellDatas.Add(gridCellData);
					}
					SubstanceData substanceData = new SubstanceData();
					substanceData.m_type = (int)baseElement.GetTileObjectType;
					substanceData.m_id = baseElement.m_id;
					substanceData.m_position = baseElement.transform.position;
					substanceData.m_rotation = baseElement.transform.eulerAngles;
					substanceData.m_localScale = baseElement.transform.localScale;
					substanceData.m_uuID = baseElement.m_uuId;
					substanceData.m_saveAttrs = baseElement.WriteBytes();
					baseElement.AppendRelatedInfo(ref tileMapScripTable.m_relatedAsset);
					if (isCreateLightingMapData)
					{
						substanceData.m_lightingMapSubstanceGroupData = LightingMapManager.GetSubstanceGroupDataByGameOject(brushBases[i].gameObject);
					}
					else
					{
						substanceData.m_lightingMapSubstanceGroupData = new LightingMapSubstanceGroupData();
					}
					gridCellData.m_substanceDatas.Add(substanceData);
				}
				else
				{
					Debug.Log("未在网格内" + brushBases[i].name + "的数据");
					list.Add(brushBases[i].gameObject);
				}
			}
			return tileMapScripTable;
		}

		public BaseTile[] GetAllBaseTiles()
		{
			return Object.FindObjectsOfType<BaseTile>();
		}

		public BaseEnemy[] GetAllBaseEnemys()
		{
			return Object.FindObjectsOfType<BaseEnemy>();
		}

		public BaseMidground[] GetAllBaseMidgrounds()
		{
			return Object.FindObjectsOfType<BaseMidground>();
		}

		public BaseTriggerBox[] GetAllBaseTriggerBoxs()
		{
			return Object.FindObjectsOfType<BaseTriggerBox>();
		}

		public BaseGroup[] GetAllBaseGroups()
		{
			return Object.FindObjectsOfType<BaseGroup>();
		}

		public BaseElement[] GetAllBaseElementsByGrid(Grid grid)
		{
			BaseElement[] allBaseElements = GetAllBaseElements();
			List<BaseElement> list = new List<BaseElement>();
			for (int i = 0; i < allBaseElements.Length; i++)
			{
				if (grid.IsOnGrid(allBaseElements[i].transform.position))
				{
					list.Add(allBaseElements[i]);
				}
			}
			return list.ToArray();
		}

		public Dictionary<int, List<BaseElement>> GetGridDictionaryByAllBaseElements()
		{
			BaseElement[] allBaseElements = GetAllBaseElements();
			return GetGridDictionaryByBaseElements(allBaseElements);
		}

		public Dictionary<int, List<BaseElement>> GetGridDictionaryByBaseElements(BaseElement[] baseElements)
		{
			Grid[] allGrid = GetAllGrid();
			Dictionary<int, List<BaseElement>> dictionary = new Dictionary<int, List<BaseElement>>();
			for (int i = 0; i < allGrid.Length; i++)
			{
				dictionary.Add(allGrid[i].m_id, new List<BaseElement>());
			}
			for (int j = 0; j < baseElements.Length; j++)
			{
				for (int k = 0; k < allGrid.Length; k++)
				{
					if (allGrid[j].IsOnGrid(baseElements[j].transform.position))
					{
						dictionary[allGrid[k].m_id].Add(baseElements[j]);
					}
				}
			}
			return dictionary;
		}

		public BaseElement[] GetAllBaseElements()
		{
			BaseElement[] array = new BaseElement[0];
			return Object.FindObjectsOfType<BaseElement>();
		}

		public void RefreshBrushUUID()
		{
			int num = 100;
			BaseElement[] allBaseElements = GetAllBaseElements();
			for (int i = 0; i < allBaseElements.Length; i++)
			{
				allBaseElements[i].m_uuId = num;
				num++;
			}
		}

		public void DestroyChindrens()
		{
			List<Transform> list = new List<Transform>();
			for (int i = 0; i < base.transform.childCount; i++)
			{
				list.Add(base.transform.GetChild(i));
			}
			for (int j = 0; j < list.Count; j++)
			{
				Object.Destroy(list[j].gameObject);
			}
		}

		private void OnDrawGizmos()
		{
			if (m_callBack != null)
			{
				m_callBack();
			}
		}
	}
}
