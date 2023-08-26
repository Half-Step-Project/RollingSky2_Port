using System;
using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.Rendering;
using User.LightingMap;
using User.TileMap;

using Grid = User.TileMap.Grid;

public class MapController : MonoBehaviour, IOriginRebirth
{
	private struct BrushID : IEquatable<BrushID>
	{
		public int m_type;

		public int m_id;

		public BrushID(int type, int id)
		{
			m_type = type;
			m_id = id;
		}

		public override int GetHashCode()
		{
			return m_type.GetHashCode() ^ m_id.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj is BrushID)
			{
				return Equals((BrushID)obj);
			}
			return false;
		}

		public bool Equals(BrushID other)
		{
			if (m_type == other.m_type)
			{
				return m_id == other.m_id;
			}
			return false;
		}
	}

	public class GroupData
	{
		public GridData m_gridData;

		public int m_createRowNum;

		public int m_worldRowNum;

		public GroupData(GridData _gridData, int _createRowNum, int _worldRowNum)
		{
			m_gridData = _gridData;
			m_createRowNum = _createRowNum;
			m_worldRowNum = _worldRowNum;
		}

		public override string ToString()
		{
			return "GroupData(" + m_gridData.m_id + "," + m_createRowNum + "," + m_worldRowNum + " ) - " + m_gridData.m_height;
		}
	}

	[Serializable]
	private struct AllElementData
	{
		public List<ElementData> Datas;

		[Obsolete("this is Obsolete")]
		public static AllElementData GetFromJson(string data)
		{
			return JsonUtility.FromJson<AllElementData>(data);
		}

		[Obsolete("this is Obsolete")]
		public string ToJosn()
		{
			return JsonUtility.ToJson(this);
		}
	}

	[Serializable]
	private struct ElementData
	{
		public int UUID;

		public int GridID;

		[Obsolete("this is Obsolete, please the InnerBsonData")]
		public string InnerData;

		public byte[] InnerBsonData;

		public ElementData(int uuid, int gridID, string innerData, byte[] innerBsonData)
		{
			UUID = uuid;
			GridID = gridID;
			InnerData = innerData;
			InnerBsonData = innerBsonData;
		}
	}

	[Serializable]
	private struct MapData
	{
		public int m_gridID;

		public Point m_point;

		public int m_createRow;

		public int m_roleStartRow;

		public OriginRebirthGroupData[] OriginRebirthGroupDatas;

		public MapData(int gridID, Point curPoint, int createRow, int roleStartRow, OriginRebirthGroupData[] originRebirthGroupDatas)
		{
			m_gridID = gridID;
			m_point = curPoint;
			m_createRow = createRow;
			m_roleStartRow = roleStartRow;
			OriginRebirthGroupDatas = originRebirthGroupDatas;
		}

		[Obsolete("this is Obsolete !")]
		public static MapData GetFromJson(string info)
		{
			return JsonUtility.FromJson<MapData>(info);
		}

		[Obsolete("this is Obsolete !")]
		public string ToJson()
		{
			return JsonUtility.ToJson(this);
		}
	}

	[Serializable]
	private struct OriginRebirthGroupData
	{
		public int CurrentGridDataId;

		public int CreateRowNum;

		public int WorldRow;

		public OriginRebirthGroupData(int gridDataId, int createRowNum, int worldRow)
		{
			CurrentGridDataId = gridDataId;
			CreateRowNum = createRowNum;
			WorldRow = worldRow;
		}
	}

	public static MapController Instance;

	private TileMapScripTable tileMapScripTable;

	private List<Grid> m_grids = new List<Grid>();

	private List<string> m_levelBrushPathList = new List<string>();

	private GameObject m_BufferedRoot;

	private GameObject m_MapRoot;

	public int ShowRowNum = 65;

	private int MapStartNum;

	private List<GroupData> curGroupList = new List<GroupData>();

	private List<BaseElement> allElementList = new List<BaseElement>();

	private Dictionary<BrushID, Queue<GameObject>> elementBufferedPool = new Dictionary<BrushID, Queue<GameObject>>();

	private Dictionary<int, BaseGroup> groupTileRootDic = new Dictionary<int, BaseGroup>();

	private Dictionary<int, BaseGroup> groupEnemyRootDic = new Dictionary<int, BaseGroup>();

	private Dictionary<int, List<BaseTile>> groupTileChildDic = new Dictionary<int, List<BaseTile>>();

	private Dictionary<int, List<BaseEnemy>> groupEnemyChildDic = new Dictionary<int, List<BaseEnemy>>();

	private Dictionary<int, BaseTile> tileTileRootDic = new Dictionary<int, BaseTile>();

	private Dictionary<int, List<BaseTile>> tileTileChildDic = new Dictionary<int, List<BaseTile>>();

	private Dictionary<int, BaseTile> tileEnemyRootDic = new Dictionary<int, BaseTile>();

	private Dictionary<int, List<BaseEnemy>> tileEnemyChildDic = new Dictionary<int, List<BaseEnemy>>();

	private LightmapData[] m_lightmapData;

	private Grid _curGrid;

	private Point m_curPoint = Point.DefaultValue;

	private int m_createRow;

	private int m_roleStartRow = -1;

	private Dictionary<int, BaseElement> mStaticElements = new Dictionary<int, BaseElement>();

	private List<ElementData> m_DataList = new List<ElementData>();

	private AllElementData allElementData;

	public List<Grid> GetGrids
	{
		get
		{
			return m_grids;
		}
	}

	public LightmapData[] GetLightmapData
	{
		get
		{
			if (m_lightmapData == null)
			{
				m_lightmapData = LightmapSettings.lightmaps;
			}
			return m_lightmapData;
		}
	}

	public Grid CurrentRoleGrid
	{
		get
		{
			return _curGrid;
		}
	}

	public Point CurrentRolePoint
	{
		get
		{
			return m_curPoint;
		}
	}

	private InsideGameDataModule GetInsideGameDataModule
	{
		get
		{
			return Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule);
		}
	}

	public bool IsRecordOriginRebirth
	{
		get
		{
			return true;
		}
	}

	private void Awake()
	{
		Instance = this;
		BaseElement[] array = UnityEngine.Object.FindObjectsOfType<BaseElement>();
		mStaticElements = new Dictionary<int, BaseElement>(array.Length);
		for (int i = 0; i < array.Length; i++)
		{
			Grid componentInParent = array[i].transform.GetComponentInParent<Grid>();
			array[i].SetGrid(componentInParent);
			mStaticElements[array[i].m_uuId] = array[i];
		}
		m_BufferedRoot = new GameObject("BufferedRoot");
		m_BufferedRoot.transform.parent = base.gameObject.transform;
		m_BufferedRoot.transform.localPosition = new Vector3(1000000f, 1000000f, 0f);
		m_BufferedRoot.SetActive(false);
		m_MapRoot = new GameObject("MapRoot");
		m_MapRoot.transform.parent = base.gameObject.transform;
		m_MapRoot.transform.localPosition = Vector3.zero;
	}

	private void OnDestroy()
	{
		mStaticElements.Clear();
		if (Instance != null)
		{
			Instance = null;
		}
	}

	public void LoadMapTable(bool isStart = false)
	{
		tileMapScripTable = LevelResources.theResource.TileMap;
		if (!tileMapScripTable)
		{
			Log.Error(string.Format("{0} is no-exist", base.name));
		}
	}

	public void LoadDevMapTable(GridGroup gridGroup)
	{
		gridGroup.RefreshBrushUUID();
		tileMapScripTable = gridGroup.EntryTileMapScripTable().DealParent().DealMagicCubeParent()
			.DealTrapGroup()
			.DealGlassGroup()
			.DealRowGroup()
			.DealBufferData(DevelopmentTool.Instance.BufferLength + 22)
			.DealBuyOutPercent()
			.DealGroupRelation()
			.DealRandomAnimRelation();
		gridGroup.gameObject.SetActive(false);
	}

	public List<RelatedAssetData> GetRelatedAssetData()
	{
		if (tileMapScripTable != null)
		{
			return tileMapScripTable.m_relatedAsset;
		}
		return null;
	}

	public AnimationClip GetRelatedAnimationClipByPath(string path)
	{
		return LevelResources.theResource.GetLevelResource(path) as AnimationClip;
	}

	public void CreateLevelMap()
	{
		CreateGrids();
		_curGrid = m_grids[0];
		m_roleStartRow = -1;
		m_createRow = 0;
		m_curPoint = Point.DefaultValue;
		curGroupList.Add(new GroupData(tileMapScripTable.m_gridDatas[0], 0, 0));
		CreateTileRow(ShowRowNum);
	}

	[Obsolete("之前的复活，现在没用了，可删除")]
	public void CreateLevelMap(RebirthBoxData data)
	{
		int num = 5;
		CreateGrids();
		_curGrid = m_grids[data.m_gridId];
		curGroupList.Add(new GroupData(tileMapScripTable.m_gridDatas[data.m_gridId], data.m_point.m_y - num, 0));
		m_roleStartRow = -1;
		m_createRow = 0;
		m_curPoint = Point.DefaultValue;
		CreateTileRow(ShowRowNum + num);
	}

	public Grid CreateNode()
	{
		GameObject obj = new GameObject("Grid_" + (m_MapRoot.transform.childCount + 1));
		Grid result = obj.AddComponent<Grid>();
		obj.transform.parent = m_MapRoot.transform;
		return result;
	}

	private void CreateTileRow(int row)
	{
		m_createRow += row;
		int count = curGroupList.Count;
		bool isHaveRemove = false;
		for (int i = 0; i < count; i++)
		{
			CreateGroupTile(row, curGroupList[i], out isHaveRemove);
		}
		if (!isHaveRemove)
		{
			return;
		}
		for (int j = 0; j < curGroupList.Count; j++)
		{
			if (curGroupList[j].m_createRowNum >= curGroupList[j].m_gridData.m_height)
			{
				curGroupList.RemoveAt(j);
				j--;
			}
		}
	}

	public void CreateGroupTile(int addrowNum, GroupData groupdata, out bool isHaveRemove)
	{
		isHaveRemove = false;
		if (addrowNum <= 0)
		{
			return;
		}
		GridData gridData = groupdata.m_gridData;
		int num = groupdata.m_createRowNum;
		if (num < 0)
		{
			num = 0;
		}
		int num2 = Mathf.Min(num + addrowNum, groupdata.m_gridData.m_height);
		if (num != num2)
		{
			for (int i = num; i < num2; i++)
			{
				int worldRowNum = i + groupdata.m_worldRowNum;
				for (int j = 0; j < gridData.m_width; j++)
				{
					Point key = new Point(j, i);
					if (gridData.m_gridCellDatasDictionary.ContainsKey(key))
					{
						CreateCellElement(gridData.m_gridCellDatasDictionary[key], m_grids[gridData.m_id], gridData, worldRowNum);
					}
				}
			}
		}
		groupdata.m_createRowNum = num2;
		if (groupdata.m_createRowNum < gridData.m_height)
		{
			return;
		}
		isHaveRemove = true;
		List<GridData> gridDatasByParentID = tileMapScripTable.m_gridDatas.GetGridDatasByParentID(gridData.m_id);
		if (gridDatasByParentID.Count <= 0)
		{
			return;
		}
		int num3 = num2 - num;
		int addrowNum2 = addrowNum - num3;
		bool isHaveRemove2 = false;
		for (int k = 0; k < gridDatasByParentID.Count; k++)
		{
			GroupData groupData = new GroupData(gridDatasByParentID[k], 0, gridDatasByParentID[k].m_beforeTheLength);
			CreateGroupTile(addrowNum2, groupData, out isHaveRemove2);
			if (isHaveRemove2)
			{
				isHaveRemove = true;
			}
			curGroupList.Add(groupData);
		}
	}

	private void CreateCellElement(GridCellData _cellData, Grid _grid, GridData _gridData, int _worldRowNum)
	{
		for (int i = 0; i < _cellData.m_substanceDatas.Count; i++)
		{
			if (_cellData.m_substanceDatas[i].m_type == 0)
			{
				CreateCellTile(_cellData, i, _grid, _gridData, _worldRowNum);
			}
		}
		for (int j = 0; j < _cellData.m_substanceDatas.Count; j++)
		{
			if (_cellData.m_substanceDatas[j].m_type == 1)
			{
				CreateCellEnemy(_cellData, j, _grid, _gridData, _worldRowNum);
			}
			else if (_cellData.m_substanceDatas[j].m_type == 2)
			{
				CreateCellMidground(_cellData, j, _grid, _gridData, _worldRowNum);
			}
			else if (_cellData.m_substanceDatas[j].m_type == 3)
			{
				CreateCellTrigger(_cellData, j, _grid, _gridData, _worldRowNum);
			}
		}
		for (int k = 0; k < _cellData.m_substanceDatas.Count; k++)
		{
			if (_cellData.m_substanceDatas[k].m_type == 4)
			{
				CreateCellGroup(_cellData, k, _grid, _gridData, _worldRowNum);
			}
		}
	}

	private void CreateCellTile(GridCellData _cellData, int _subIndexs, Grid _grid, GridData _gridData, int worldRowNum)
	{
		SubstanceData substanceData = _cellData.m_substanceDatas[_subIndexs];
		BrushID key = new BrushID(substanceData.m_type, substanceData.m_id);
		GameObject obj = elementBufferedPool[key].Dequeue();
		obj.transform.parent = _grid.gameObject.transform;
		obj.transform.position = substanceData.m_position;
		obj.transform.eulerAngles = substanceData.m_rotation;
		obj.transform.localScale = substanceData.m_localScale;
		BaseTile gameComponent = obj.GetGameComponent<BaseTile>();
		gameComponent.groupTransform = _grid.gameObject.transform;
		gameComponent.EnemyChildCount = substanceData.m_enemyChildData.m_childNum;
		gameComponent.TileChildCount = substanceData.m_tileChildData.m_childNum;
		gameComponent.ParentKey = substanceData.m_parentData.m_key;
		if (substanceData.m_enemyChildData.IfHaveChild)
		{
			List<BaseEnemy> value;
			if (tileEnemyChildDic.TryGetValue(ParentInfo.GetKeyBy(_gridData, _cellData), out value))
			{
				if (value.Count < gameComponent.EnemyChildCount)
				{
					tileEnemyRootDic.Add(ParentInfo.GetKeyBy(_gridData, _cellData), gameComponent);
					for (int i = 0; i < value.Count; i++)
					{
						value[i].transform.parent = gameComponent.transform;
						gameComponent.EnemyChildCount--;
					}
					value.Clear();
				}
				else
				{
					for (int j = 0; j < value.Count; j++)
					{
						value[j].transform.parent = gameComponent.transform;
						gameComponent.EnemyChildCount--;
					}
					value.Clear();
					tileEnemyChildDic.Remove(ParentInfo.GetKeyBy(_gridData, _cellData));
				}
			}
			else
			{
				tileEnemyRootDic.Add(ParentInfo.GetKeyBy(_gridData, _cellData), gameComponent);
			}
		}
		if (substanceData.m_tileChildData.IfHaveChild)
		{
			List<BaseTile> value2;
			if (tileTileChildDic.TryGetValue(ParentInfo.GetKeyBy(_gridData, _cellData), out value2))
			{
				if (value2.Count < gameComponent.TileChildCount)
				{
					tileTileRootDic.Add(ParentInfo.GetKeyBy(_gridData, _cellData), gameComponent);
					for (int k = 0; k < value2.Count; k++)
					{
						value2[k].transform.parent = gameComponent.transform;
						gameComponent.TileChildCount--;
					}
					value2.Clear();
				}
				else
				{
					for (int l = 0; l < value2.Count; l++)
					{
						value2[l].transform.parent = gameComponent.transform;
						gameComponent.TileChildCount--;
					}
					value2.Clear();
					tileTileChildDic.Remove(ParentInfo.GetKeyBy(_gridData, _cellData));
				}
			}
			else
			{
				tileTileRootDic.Add(ParentInfo.GetKeyBy(_gridData, _cellData), gameComponent);
			}
		}
		if (gameComponent.ParentKey > 0)
		{
			BaseTile value4;
			if (ParentInfo.IfGroup(gameComponent.ParentKey))
			{
				BaseGroup value3;
				if (groupTileRootDic.TryGetValue(gameComponent.ParentKey, out value3))
				{
					gameComponent.transform.parent = value3.transform;
					value3.TileChildCount--;
					if (value3.TileChildCount == 0)
					{
						groupTileRootDic.Remove(gameComponent.ParentKey);
					}
				}
				else if (groupTileChildDic.ContainsKey(gameComponent.ParentKey))
				{
					groupTileChildDic[gameComponent.ParentKey].Add(gameComponent);
				}
				else
				{
					List<BaseTile> list = new List<BaseTile>();
					list.Add(gameComponent);
					groupTileChildDic.Add(gameComponent.ParentKey, list);
				}
			}
			else if (tileTileRootDic.TryGetValue(gameComponent.ParentKey, out value4))
			{
				gameComponent.transform.parent = value4.transform;
				value4.TileChildCount--;
				if (value4.TileChildCount == 0)
				{
					tileTileRootDic.Remove(gameComponent.ParentKey);
				}
			}
			else if (tileTileChildDic.ContainsKey(gameComponent.ParentKey))
			{
				tileTileChildDic[gameComponent.ParentKey].Add(gameComponent);
			}
			else
			{
				List<BaseTile> list2 = new List<BaseTile>();
				list2.Add(gameComponent);
				tileTileChildDic.Add(gameComponent.ParentKey, list2);
			}
		}
		gameComponent.ReadBytes(substanceData.m_saveAttrs);
		gameComponent.point = _cellData.m_point;
		gameComponent.m_gridId = _gridData.m_id;
		gameComponent.worldRow = worldRowNum;
		gameComponent.m_uuId = substanceData.m_uuID;
		gameComponent.Initialize();
		allElementList.Add(gameComponent);
		if ((bool)gameComponent.colider)
		{
			gameComponent.colider.SetActive(true);
		}
		List<LightingMapSubstanceData> lightingMapSubstanceDatas = _cellData.m_substanceDatas[_subIndexs].m_lightingMapSubstanceGroupData.m_lightingMapSubstanceDatas;
		ApplyLightmapAndDisableShadow(gameComponent, lightingMapSubstanceDatas);
		gameComponent.LateInitialize();
	}

	private void CreateCellEnemy(GridCellData _cellData, int _subIndexs, Grid _grid, GridData _gridData, int worldRowNum)
	{
		SubstanceData substanceData = _cellData.m_substanceDatas[_subIndexs];
		BrushID key = new BrushID(substanceData.m_type, substanceData.m_id);
		GameObject obj = elementBufferedPool[key].Dequeue();
		obj.transform.parent = _grid.gameObject.transform;
		obj.transform.position = substanceData.m_position;
		obj.transform.eulerAngles = substanceData.m_rotation;
		obj.transform.localScale = substanceData.m_localScale;
		BaseEnemy gameComponent = obj.GetGameComponent<BaseEnemy>();
		gameComponent.groupTransform = _grid.gameObject.transform;
		gameComponent.ParentKey = substanceData.m_parentData.m_key;
		if (gameComponent.ParentKey > 0)
		{
			BaseTile value2;
			if (ParentInfo.IfGroup(gameComponent.ParentKey))
			{
				BaseGroup value;
				if (groupEnemyRootDic.TryGetValue(gameComponent.ParentKey, out value))
				{
					gameComponent.transform.parent = value.transform;
					gameComponent.name += "Child";
					value.EnemyChildCount--;
					if (value.EnemyChildCount == 0)
					{
						groupEnemyRootDic.Remove(gameComponent.ParentKey);
					}
				}
				else if (groupEnemyChildDic.ContainsKey(gameComponent.ParentKey))
				{
					groupEnemyChildDic[gameComponent.ParentKey].Add(gameComponent);
				}
				else
				{
					List<BaseEnemy> list = new List<BaseEnemy>();
					list.Add(gameComponent);
					groupEnemyChildDic.Add(gameComponent.ParentKey, list);
				}
			}
			else if (tileEnemyRootDic.TryGetValue(gameComponent.ParentKey, out value2))
			{
				gameComponent.transform.parent = value2.transform;
				gameComponent.name += "Child";
				value2.EnemyChildCount--;
				if (value2.EnemyChildCount == 0)
				{
					tileEnemyRootDic.Remove(gameComponent.ParentKey);
				}
			}
			else if (tileEnemyChildDic.ContainsKey(gameComponent.ParentKey))
			{
				tileEnemyChildDic[gameComponent.ParentKey].Add(gameComponent);
			}
			else
			{
				List<BaseEnemy> list2 = new List<BaseEnemy>();
				list2.Add(gameComponent);
				tileEnemyChildDic.Add(gameComponent.ParentKey, list2);
			}
		}
		gameComponent.ReadBytes(substanceData.m_saveAttrs);
		gameComponent.point = _cellData.m_point;
		gameComponent.m_gridId = _gridData.m_id;
		gameComponent.m_uuId = substanceData.m_uuID;
		gameComponent.Initialize();
		gameComponent.worldRow = worldRowNum;
		allElementList.Add(gameComponent);
		if ((bool)gameComponent.colider)
		{
			gameComponent.colider.SetActive(true);
		}
		List<LightingMapSubstanceData> lightingMapSubstanceDatas = _cellData.m_substanceDatas[_subIndexs].m_lightingMapSubstanceGroupData.m_lightingMapSubstanceDatas;
		ApplyLightmap(gameComponent, lightingMapSubstanceDatas);
		gameComponent.LateInitialize();
	}

	private void CreateCellTrigger(GridCellData _cellData, int _subIndexs, Grid _grid, GridData _gridData, int worldRowNum)
	{
		SubstanceData substanceData = _cellData.m_substanceDatas[_subIndexs];
		BrushID key = new BrushID(substanceData.m_type, substanceData.m_id);
		GameObject obj = elementBufferedPool[key].Dequeue();
		obj.transform.parent = _grid.gameObject.transform;
		obj.transform.position = substanceData.m_position;
		obj.transform.eulerAngles = substanceData.m_rotation;
		obj.transform.localScale = substanceData.m_localScale;
		BaseTriggerBox gameComponent = obj.GetGameComponent<BaseTriggerBox>();
		gameComponent.groupTransform = _grid.gameObject.transform;
		gameComponent.ReadBytes(substanceData.m_saveAttrs);
		gameComponent.point = _cellData.m_point;
		gameComponent.m_gridId = _gridData.m_id;
		gameComponent.m_uuId = substanceData.m_uuID;
		gameComponent.Initialize();
		allElementList.Add(gameComponent);
		if ((bool)gameComponent.colider)
		{
			gameComponent.colider.SetActive(true);
		}
		List<LightingMapSubstanceData> lightingMapSubstanceDatas = _cellData.m_substanceDatas[_subIndexs].m_lightingMapSubstanceGroupData.m_lightingMapSubstanceDatas;
		ApplyLightmap(gameComponent, lightingMapSubstanceDatas);
		gameComponent.LateInitialize();
	}

	private void CreateCellMidground(GridCellData _cellData, int _subIndexs, Grid _grid, GridData _gridData, int worldRowNum)
	{
		SubstanceData substanceData = _cellData.m_substanceDatas[_subIndexs];
		BrushID key = new BrushID(substanceData.m_type, substanceData.m_id);
		GameObject obj = elementBufferedPool[key].Dequeue();
		obj.transform.parent = _grid.gameObject.transform;
		obj.transform.position = substanceData.m_position;
		obj.transform.eulerAngles = substanceData.m_rotation;
		obj.transform.localScale = substanceData.m_localScale;
		BaseMidground gameComponent = obj.GetGameComponent<BaseMidground>();
		gameComponent.groupTransform = _grid.gameObject.transform;
		gameComponent.ReadBytes(substanceData.m_saveAttrs);
		gameComponent.point = _cellData.m_point;
		gameComponent.m_gridId = _gridData.m_id;
		gameComponent.m_uuId = substanceData.m_uuID;
		gameComponent.Initialize();
		allElementList.Add(gameComponent);
		if ((bool)gameComponent.colider)
		{
			gameComponent.colider.SetActive(true);
		}
		List<LightingMapSubstanceData> lightingMapSubstanceDatas = _cellData.m_substanceDatas[_subIndexs].m_lightingMapSubstanceGroupData.m_lightingMapSubstanceDatas;
		ApplyLightmap(gameComponent, lightingMapSubstanceDatas);
		gameComponent.LateInitialize();
	}

	private void CreateCellGroup(GridCellData _cellData, int _subIndexs, Grid _grid, GridData _gridData, int worldRowNum)
	{
		SubstanceData substanceData = _cellData.m_substanceDatas[_subIndexs];
		BrushID key = new BrushID(substanceData.m_type, substanceData.m_id);
		GameObject obj = elementBufferedPool[key].Dequeue();
		obj.transform.parent = _grid.gameObject.transform;
		obj.transform.position = substanceData.m_position;
		obj.transform.eulerAngles = substanceData.m_rotation;
		obj.transform.localScale = substanceData.m_localScale;
		BaseGroup gameComponent = obj.GetGameComponent<BaseGroup>();
		gameComponent.groupTransform = _grid.gameObject.transform;
		gameComponent.EnemyChildCount = substanceData.m_enemyChildData.m_childNum;
		gameComponent.TileChildCount = substanceData.m_tileChildData.m_childNum;
		gameComponent.ParentKey = substanceData.m_parentData.m_key;
		if (substanceData.m_enemyChildData.IfHaveChild)
		{
			List<BaseEnemy> value;
			if (groupEnemyChildDic.TryGetValue(ParentInfo.GetKeyBy(_gridData, _cellData, true), out value))
			{
				if (value.Count < gameComponent.EnemyChildCount)
				{
					groupEnemyRootDic.Add(ParentInfo.GetKeyBy(_gridData, _cellData, true), gameComponent);
					for (int i = 0; i < value.Count; i++)
					{
						value[i].transform.parent = gameComponent.transform;
						gameComponent.EnemyChildCount--;
					}
					value.Clear();
				}
				else
				{
					for (int j = 0; j < value.Count; j++)
					{
						value[j].transform.parent = gameComponent.transform;
						gameComponent.EnemyChildCount--;
					}
					value.Clear();
					groupEnemyChildDic.Remove(ParentInfo.GetKeyBy(_gridData, _cellData, true));
				}
			}
			else
			{
				groupEnemyRootDic.Add(ParentInfo.GetKeyBy(_gridData, _cellData, true), gameComponent);
			}
		}
		if (substanceData.m_tileChildData.IfHaveChild)
		{
			List<BaseTile> value2;
			if (groupTileChildDic.TryGetValue(ParentInfo.GetKeyBy(_gridData, _cellData, true), out value2))
			{
				if (value2.Count < gameComponent.TileChildCount)
				{
					groupTileRootDic.Add(ParentInfo.GetKeyBy(_gridData, _cellData, true), gameComponent);
					for (int k = 0; k < value2.Count; k++)
					{
						value2[k].transform.parent = gameComponent.transform;
						gameComponent.TileChildCount--;
					}
					value2.Clear();
				}
				else
				{
					for (int l = 0; l < value2.Count; l++)
					{
						value2[l].transform.parent = gameComponent.transform;
						gameComponent.TileChildCount--;
					}
					value2.Clear();
					groupTileChildDic.Remove(ParentInfo.GetKeyBy(_gridData, _cellData, true));
				}
			}
			else
			{
				groupTileRootDic.Add(ParentInfo.GetKeyBy(_gridData, _cellData, true), gameComponent);
			}
		}
		gameComponent.ReadBytes(substanceData.m_saveAttrs);
		gameComponent.point = _cellData.m_point;
		gameComponent.m_gridId = _gridData.m_id;
		gameComponent.worldRow = worldRowNum;
		gameComponent.m_uuId = substanceData.m_uuID;
		gameComponent.Initialize();
		allElementList.Add(gameComponent);
		if ((bool)gameComponent.colider)
		{
			gameComponent.colider.SetActive(true);
		}
		List<LightingMapSubstanceData> lightingMapSubstanceDatas = _cellData.m_substanceDatas[_subIndexs].m_lightingMapSubstanceGroupData.m_lightingMapSubstanceDatas;
		ApplyLightmapAndDisableShadow(gameComponent, lightingMapSubstanceDatas);
		gameComponent.LateInitialize();
	}

	public void CreateGrids()
	{
		for (int i = 0; i < tileMapScripTable.m_gridDatas.Count; i++)
		{
			tileMapScripTable.m_gridDatas[i].m_gridCellDatasDictionary = tileMapScripTable.m_gridDatas[i].m_gridCellDatas.ListToDictionary();
			Grid grid = CreateNode();
			grid.m_id = tileMapScripTable.m_gridDatas[i].m_id;
			grid.m_x = tileMapScripTable.m_gridDatas[i].m_height;
			grid.m_y = tileMapScripTable.m_gridDatas[i].m_width;
			grid.transform.position = tileMapScripTable.m_gridDatas[i].m_position;
			grid.transform.eulerAngles = tileMapScripTable.m_gridDatas[i].m_rotation;
			grid.m_samplingInterval = tileMapScripTable.m_gridDatas[i].m_samplingInterval;
			grid.m_samplingCenterY = tileMapScripTable.m_gridDatas[i].m_samplingCenterY;
			m_grids.Add(grid);
		}
		for (int j = 0; j < tileMapScripTable.m_gridDatas.Count; j++)
		{
			m_grids[j].m_parentGrids = new List<Grid>();
			for (int k = 0; k < tileMapScripTable.m_gridDatas[j].m_parentIds.Count; k++)
			{
				m_grids[j].m_parentGrids.Add(m_grids[tileMapScripTable.m_gridDatas[j].m_parentIds[k]]);
			}
		}
	}

	public void ClearWorld()
	{
		int num;
		for (num = 0; num < allElementList.Count; num++)
		{
			GiveToBuffered(num);
			num--;
		}
		_curGrid = m_grids[0];
		m_roleStartRow = -1;
		m_createRow = 0;
		m_curPoint = Point.DefaultValue;
		curGroupList.Clear();
		allElementList.Clear();
		groupTileRootDic.Clear();
		groupEnemyRootDic.Clear();
		groupTileChildDic.Clear();
		groupEnemyChildDic.Clear();
		tileTileRootDic.Clear();
		tileTileChildDic.Clear();
		tileEnemyRootDic.Clear();
		tileEnemyChildDic.Clear();
		for (int i = 0; i < m_grids.Count; i++)
		{
			UnityEngine.Object.Destroy(m_grids[i].gameObject);
		}
		if (elementBufferedPool.Count > 0)
		{
			Dictionary<BrushID, Queue<GameObject>>.Enumerator enumerator = elementBufferedPool.GetEnumerator();
			while (enumerator.MoveNext())
			{
				Queue<GameObject> value = enumerator.Current.Value;
				while (value.Count != 0)
				{
					UnityEngine.Object.Destroy(value.Dequeue());
				}
			}
			elementBufferedPool.Clear();
		}
		m_grids.Clear();
		GC.Collect();
	}

	public void ResetWorld()
	{
		int num;
		for (num = 0; num < allElementList.Count; num++)
		{
			GiveToBuffered(num);
			num--;
		}
		_curGrid = m_grids[0];
		m_roleStartRow = -1;
		m_createRow = 0;
		m_curPoint = Point.DefaultValue;
		curGroupList.Clear();
		allElementList.Clear();
		groupTileRootDic.Clear();
		groupEnemyRootDic.Clear();
		groupTileChildDic.Clear();
		groupEnemyChildDic.Clear();
		tileTileRootDic.Clear();
		tileTileChildDic.Clear();
		tileEnemyRootDic.Clear();
		tileEnemyChildDic.Clear();
		allElementData = default(AllElementData);
		for (int i = 0; i < m_grids.Count; i++)
		{
			UnityEngine.Object.Destroy(m_grids[i].gameObject);
		}
		m_grids.Clear();
		GC.Collect();
	}

	private void GiveToBuffered(int allElementIndex)
	{
		BaseElement baseElement = allElementList[allElementIndex];
		Transform transform = baseElement.transform;
		if (baseElement.TileChildCount + baseElement.EnemyChildCount > 0)
		{
			BaseElement[] componentsInChildren = baseElement.GetComponentsInChildren<BaseElement>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i] != baseElement)
				{
					componentsInChildren[i].transform.parent = baseElement.transform.parent;
				}
			}
		}
		if ((bool)baseElement.colider)
		{
			baseElement.colider.SetActive(false);
		}
		baseElement.ResetElement();
		BrushID key = new BrushID((int)baseElement.GetTileObjectType, baseElement.m_id);
		transform.parent = m_BufferedRoot.transform;
		transform.localPosition = Vector3.zero;
		elementBufferedPool[key].Enqueue(allElementList[allElementIndex].gameObject);
		allElementList[allElementIndex] = null;
		allElementList.RemoveAt(allElementIndex);
	}

	public void MapUpdate()
	{
		if (BaseRole.theBall == null || _curGrid == null)
		{
			return;
		}
		int id = _curGrid.m_id;
		Point pointByPosition = _curGrid.GetPointByPosition(BaseRole.theBall.transform.position);
		if (pointByPosition.m_y > _curGrid.m_x || pointByPosition.m_y < 0)
		{
			for (int i = 0; i < m_grids.Count; i++)
			{
				if (m_grids[i].IsOnGrid(BaseRole.theBall.transform.position))
				{
					id = m_grids[i].m_id;
					pointByPosition = m_grids[i].GetPointByPosition(BaseRole.theBall.transform.position);
					break;
				}
			}
		}
		if (m_roleStartRow == -1)
		{
			m_roleStartRow = tileMapScripTable.m_gridDatas[id].m_beforeTheLength + pointByPosition.m_y;
			m_curPoint = pointByPosition;
		}
		if (id != _curGrid.m_id || m_curPoint.m_y != pointByPosition.m_y)
		{
			_curGrid = m_grids[id];
			m_curPoint = pointByPosition;
			RemoveTile();
			int num = ShowRowNum - m_roleStartRow;
			int num2 = tileMapScripTable.m_gridDatas[id].m_beforeTheLength + pointByPosition.m_y;
			int num3 = num + num2;
			if (num3 > m_createRow)
			{
				int row = num3 - m_createRow;
				CreateTileRow(row);
			}
		}
	}

	public void InitElement()
	{
		for (int i = 0; i < allElementList.Count; i++)
		{
			allElementList[i].InitElement();
		}
	}

	public void UpdateElement()
	{
		if (allElementList.Count >= 1)
		{
			for (int i = 0; i < allElementList.Count; i++)
			{
				allElementList[i].UpdateElement();
			}
		}
	}

	private void RemoveTile()
	{
		for (int i = 0; i < allElementList.Count; i++)
		{
			if (allElementList[i].GetTileObjectType != TileObjectType.Trigger && allElementList[i].CanRecycle)
			{
				GiveToBuffered(i);
				i--;
			}
		}
	}

	public void RecordingInit(int gridId, int initRowNum)
	{
		GridData gridDataByID = tileMapScripTable.m_gridDatas.GetGridDataByID(gridId);
		if (gridDataByID == null)
		{
			Debug.LogError("grid id not exist!!!");
		}
		curGroupList.Add(new GroupData(gridDataByID, initRowNum, 0));
		MapStartNum = gridDataByID.m_height;
		CreateTileRow(MapStartNum);
	}

	public void RecordingReset(int gridid, int rowNumber)
	{
		curGroupList.Clear();
		int num;
		for (num = 0; num < allElementList.Count; num++)
		{
			GiveToBuffered(num);
			num--;
		}
		allElementList.Clear();
		groupTileRootDic.Clear();
		groupEnemyRootDic.Clear();
		groupTileChildDic.Clear();
		groupEnemyChildDic.Clear();
		tileTileRootDic.Clear();
		tileTileChildDic.Clear();
		tileEnemyRootDic.Clear();
		tileEnemyChildDic.Clear();
		RecordingInit(gridid, rowNumber);
	}

	public BaseElement GetBaseElementForAll(int gridId, Point point, TileObjectType type, int id)
	{
		BaseElement result = null;
		for (int i = 0; i < allElementList.Count; i++)
		{
			BaseElement baseElement = allElementList[i];
			if (baseElement.m_gridId == gridId && baseElement.point == point && baseElement.GetTileObjectType == type && baseElement.m_id == id)
			{
				result = baseElement;
				break;
			}
		}
		return result;
	}

	public GridData GetGridDataById(int id)
	{
		if (tileMapScripTable == null)
		{
			return null;
		}
		return tileMapScripTable.m_gridDatas.GetGridDataByID(id);
	}

	public Grid GetGridById(int id)
	{
		if (m_grids == null)
		{
			return null;
		}
		for (int i = 0; i < m_grids.Count; i++)
		{
			if (m_grids[i].m_id == id)
			{
				return m_grids[i];
			}
		}
		return null;
	}

	public Grid GetGridByFirst()
	{
		if (m_grids == null)
		{
			return null;
		}
		for (int i = 0; i < m_grids.Count; i++)
		{
			if (m_grids[i].m_id == 0)
			{
				return m_grids[i];
			}
		}
		return null;
	}

	public List<string> GetLevelBrushPathList()
	{
		m_levelBrushPathList.Clear();
		List<BrushBufferData> brushBufferDataList = tileMapScripTable.m_brushBufferDataList;
		for (int i = 0; i < brushBufferDataList.Count; i++)
		{
			BrushBufferData brushBufferData = brushBufferDataList[i];
			BrushData brushDataByTypeAddBrushID = BrushsManager.GetBrushDataByTypeAddBrushID((TileObjectType)brushBufferData.m_type, brushBufferData.m_id);
			if (brushDataByTypeAddBrushID != null)
			{
				string resBrushesAsset = AssetUtility.GetResBrushesAsset(brushDataByTypeAddBrushID.m_resourcesPath);
				if (!m_levelBrushPathList.Contains(resBrushesAsset))
				{
					m_levelBrushPathList.Add(resBrushesAsset);
				}
			}
			else
			{
				TileObjectType type = (TileObjectType)brushBufferData.m_type;
				Debug.LogError(string.Format("brushData is null,type = {0} , id = {1}", type.ToString(), brushBufferData.m_id));
			}
		}
		return m_levelBrushPathList;
	}

	public void InstantiationBrush()
	{
		List<BrushBufferData> brushBufferDataList = tileMapScripTable.m_brushBufferDataList;
		for (int i = 0; i < brushBufferDataList.Count; i++)
		{
			BrushBufferData brushBufferData = brushBufferDataList[i];
			GameObject gameObject = null;
			BrushData brushDataByTypeAddBrushID = BrushsManager.GetBrushDataByTypeAddBrushID((TileObjectType)brushBufferData.m_type, brushBufferData.m_id);
			string resourcesPath = brushDataByTypeAddBrushID.m_resourcesPath;
			gameObject = LevelResources.theResource.GetLevelResource(AssetUtility.GetResBrushesAsset(resourcesPath)) as GameObject;
			if (gameObject == null)
			{
				Debug.LogError("Cannot Find Prefab :" + resourcesPath);
				continue;
			}
			BrushID key = new BrushID(brushBufferData.m_type, brushDataByTypeAddBrushID.m_id);
			for (int j = 0; j < brushBufferData.m_size; j++)
			{
				GameObject gameObject2 = null;
				gameObject2 = UnityEngine.Object.Instantiate(gameObject);
				BaseElement baseElement = gameObject2.AddComponent(brushDataByTypeAddBrushID.m_typeClass) as BaseElement;
				baseElement.m_id = brushDataByTypeAddBrushID.m_id;
				gameObject2.transform.parent = m_BufferedRoot.transform;
				gameObject2.transform.localPosition = Vector3.zero;
				Transform transform = gameObject2.transform.Find("collider");
				if ((bool)transform)
				{
					baseElement.colider = transform.gameObject;
					transform.gameObject.SetActive(false);
				}
				Queue<GameObject> value;
				if (elementBufferedPool.TryGetValue(key, out value))
				{
					value.Enqueue(gameObject2);
					continue;
				}
				value = new Queue<GameObject>();
				value.Enqueue(gameObject2);
				elementBufferedPool[key] = value;
			}
		}
	}

	public int GetLevelLength()
	{
		int num = 0;
		num += tileMapScripTable.m_gridDatas[0].m_height;
		for (int i = 0; i < tileMapScripTable.m_gridDatas.Count - 1; i++)
		{
			List<GridData> gridDatasByParentID = tileMapScripTable.m_gridDatas.GetGridDatasByParentID(tileMapScripTable.m_gridDatas[i].m_id);
			int num2 = 0;
			if (gridDatasByParentID.Count >= 1)
			{
				for (int j = 0; j < gridDatasByParentID.Count; j++)
				{
					if (gridDatasByParentID[j].m_height > num2)
					{
						num2 = gridDatasByParentID[j].m_height;
					}
				}
			}
			num += num2;
		}
		return num;
	}

	public int GetCurrentGridRow(Vector3 position)
	{
		return MathUtils.Ceiled(_curGrid.transform.InverseTransformPoint(position).z);
	}

	public BaseElement CreateSubstanceFormBuffer(Grid grid, Point point, SubstanceData substanceData)
	{
		if (grid == null || substanceData == null)
		{
			return null;
		}
		BrushID key = new BrushID(substanceData.m_type, substanceData.m_id);
		GameObject obj = elementBufferedPool[key].Dequeue();
		obj.transform.parent = grid.gameObject.transform;
		obj.transform.position = substanceData.m_position;
		obj.transform.eulerAngles = substanceData.m_rotation;
		obj.transform.localScale = substanceData.m_localScale;
		BaseElement gameComponent = obj.GetGameComponent<BaseElement>();
		gameComponent.groupTransform = grid.gameObject.transform;
		gameComponent.ReadBytes(substanceData.m_saveAttrs);
		gameComponent.point = point;
		gameComponent.m_gridId = grid.m_id;
		gameComponent.Initialize();
		allElementList.Add(gameComponent);
		if ((bool)gameComponent.colider)
		{
			gameComponent.colider.SetActive(true);
		}
		gameComponent.LateInitialize();
		return gameComponent;
	}

	public bool TryGetElementByUUID<T>(int uuid, out T element) where T : BaseElement
	{
		for (int i = 0; i < allElementList.Count; i++)
		{
			if (allElementList[i].m_uuId == uuid)
			{
				element = allElementList[i] as T;
				return true;
			}
		}
		BaseElement value = null;
		if (mStaticElements.TryGetValue(uuid, out value) && value != null)
		{
			element = value as T;
			return true;
		}
		element = null;
		return false;
	}

	private void ApplyLightmap(BaseElement element, List<LightingMapSubstanceData> lightmapList)
	{
		if (lightmapList.Count > 0 && GetLightmapData.Length != 0)
		{
			Renderer[] componentsInChildren = element.GetComponentsInChildren<Renderer>();
			for (int i = 0; i < componentsInChildren.Length && i < lightmapList.Count; i++)
			{
				componentsInChildren[i].lightmapIndex = lightmapList[i].m_lightmapIndex;
				componentsInChildren[i].lightmapScaleOffset = lightmapList[i].m_lightmapScaleOffset;
			}
		}
	}

	private void ApplyLightmapAndDisableShadow(BaseElement element, List<LightingMapSubstanceData> lightmapList)
	{
		if (lightmapList.Count > 0 && GetLightmapData.Length != 0)
		{
			Renderer[] componentsInChildren = element.GetComponentsInChildren<Renderer>();
			for (int i = 0; i < componentsInChildren.Length && i < lightmapList.Count; i++)
			{
				componentsInChildren[i].lightmapIndex = lightmapList[i].m_lightmapIndex;
				componentsInChildren[i].lightmapScaleOffset = lightmapList[i].m_lightmapScaleOffset;
				componentsInChildren[i].shadowCastingMode = ShadowCastingMode.Off;
				componentsInChildren[i].receiveShadows = false;
			}
		}
	}

	public static float GetProgress(Grid grid, GridData gridData, Point targetPoint, int startRow, int actualLength)
	{
		float result = 0f;
		if (grid != null && gridData != null)
		{
			float num = (float)(gridData.m_beforeTheLength + targetPoint.m_y - startRow) * 1f;
			result = Mathf.FloorToInt(((num <= 0f) ? 0f : num) / (float)((actualLength <= 0) ? 1 : actualLength) * 100f);
		}
		return result;
	}

	public static float GetProgress(TileMapScripTable tableMapScript, Grid grid, GridData gridData, Point targetPoint)
	{
		return GetProgress(grid, gridData, targetPoint, tableMapScript.m_progressData.m_startRow, tableMapScript.m_progressData.m_actualLength);
	}

	public RebirthMapData GetRebirthData(Vector3 position, Quaternion rotation, Vector3 scale)
	{
		RebirthMapData rebirthMapData = new RebirthMapData();
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		for (int i = 0; i < allElementList.Count; i++)
		{
			IRebirth rebirth = allElementList[i] as IRebirth;
			if (rebirth != null && rebirth.IsRecordRebirth())
			{
				string key = string.Format("{0}_{1}_{2}", allElementList[i].m_gridId.ToString(), allElementList[i].point.ToString(), allElementList[i].m_id.ToString());
				object rebirthData = rebirth.GetRebirthData(position, rotation, scale);
				dictionary.Add(key, rebirthData);
			}
		}
		rebirthMapData.m_brushsData = dictionary;
		return rebirthMapData;
	}

	public void ResetBySavePointData(RebirthBoxData data)
	{
		if (data == null || data.m_mapData == null)
		{
			return;
		}
		Dictionary<string, object> brushsData = data.m_mapData.m_brushsData;
		if (brushsData == null || brushsData.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < allElementList.Count; i++)
		{
			IRebirth rebirth = allElementList[i] as IRebirth;
			if (rebirth != null && rebirth.IsRecordRebirth())
			{
				string key = string.Format("{0}_{1}_{2}", allElementList[i].m_gridId.ToString(), allElementList[i].point.ToString(), allElementList[i].m_id.ToString());
				object value = null;
				if (brushsData.TryGetValue(key, out value) && value != null)
				{
					rebirth.ResetBySavePointData(value);
				}
			}
		}
	}

	public void StartRunningForRebirthData(RebirthBoxData data)
	{
		if (data == null || data.m_mapData == null)
		{
			return;
		}
		Dictionary<string, object> brushsData = data.m_mapData.m_brushsData;
		if (brushsData == null || brushsData.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < allElementList.Count; i++)
		{
			IRebirth rebirth = allElementList[i] as IRebirth;
			if (rebirth != null && rebirth.IsRecordRebirth())
			{
				string key = string.Format("{0}_{1}_{2}", allElementList[i].m_gridId.ToString(), allElementList[i].point.ToString(), allElementList[i].m_id.ToString());
				object value = null;
				if (brushsData.TryGetValue(key, out value) && value != null)
				{
					rebirth.StartRunningForRebirthData(value);
				}
			}
		}
	}

	public string GetElementData()
	{
		m_DataList.Clear();
		foreach (BaseElement allElement in allElementList)
		{
			int uuId = allElement.m_uuId;
			int gridId = allElement.m_gridId;
			string innerData = string.Empty;
			if (allElement.IfRebirthRecord)
			{
				innerData = allElement.RebirthWriteData() as string;
			}
			m_DataList.Add(new ElementData(uuId, gridId, innerData, null));
		}
		AllElementData allElementData = default(AllElementData);
		allElementData.Datas = m_DataList;
		return allElementData.ToJosn();
	}

	public void SetElementData(string data)
	{
		AllElementData fromJson = AllElementData.GetFromJson(data);
		List<string> list = new List<string>();
		Dictionary<int, SubstanceUUIDPointer> dictionary = new Dictionary<int, SubstanceUUIDPointer>();
		int count = tileMapScripTable.m_uuidData.m_pointers.Count;
		for (int i = 0; i < count; i++)
		{
			dictionary[tileMapScripTable.m_uuidData.m_pointers[i].m_uuid] = tileMapScripTable.m_uuidData.m_pointers[i];
		}
		for (int j = 0; j < fromJson.Datas.Count; j++)
		{
			ElementData elementData = fromJson.Datas[j];
			SubstanceUUIDPointer value = null;
			if (dictionary.TryGetValue(elementData.UUID, out value))
			{
				GridData gridData = tileMapScripTable.m_gridDatas[value.m_pointer.m_gridIndex];
				GridCellData gridCellData = gridData.m_gridCellDatas[value.m_pointer.m_gridCellIndex];
				SubstanceData substanceData = gridCellData.m_substanceDatas[value.m_pointer.m_substanceIndex];
				int id = gridData.m_id;
				Point point = gridCellData.m_point;
				CreateElement(substanceData, id, point, elementData.InnerData);
				list.Add(elementData.InnerData);
			}
		}
		for (int k = 0; k < allElementList.Count; k++)
		{
			BaseElement baseElement = allElementList[k];
			if (baseElement.IfRebirthRecord)
			{
				try
				{
					baseElement.RebirthReadData(list[k]);
				}
				catch (Exception)
				{
					Debug.Log(baseElement.gameObject.name + "," + baseElement.m_id + "," + baseElement.m_uuId);
				}
			}
			DropData? dropData = GetInsideGameDataModule.FindDropDataByUUID(baseElement.m_uuId);
			if (dropData.HasValue && dropData.HasValue)
			{
				baseElement.RebirthReadDataForDrop(list[k]);
			}
		}
	}

	public void StartRunByElementData(string data)
	{
		AllElementData fromJson = AllElementData.GetFromJson(data);
		List<string> list = new List<string>();
		for (int i = 0; i < fromJson.Datas.Count; i++)
		{
			list.Add(fromJson.Datas[i].InnerData);
		}
		for (int j = 0; j < allElementList.Count; j++)
		{
			BaseElement baseElement = allElementList[j];
			if (baseElement.IfRebirthRecord)
			{
				baseElement.RebirthStartGame(list[j]);
			}
			DropData? dropData = GetInsideGameDataModule.FindDropDataByUUID(baseElement.m_uuId);
			if (dropData.HasValue && dropData.HasValue)
			{
				baseElement.RebirthStartGameForDrop(list[j]);
			}
		}
	}

	public byte[] GetElementBsonData()
	{
		m_DataList.Clear();
		foreach (BaseElement allElement in allElementList)
		{
			int uuId = allElement.m_uuId;
			int gridId = allElement.m_gridId;
			byte[] innerBsonData = null;
			if (allElement.IfRebirthRecord)
			{
				innerBsonData = allElement.RebirthWriteByteData();
			}
			m_DataList.Add(new ElementData(uuId, gridId, string.Empty, innerBsonData));
		}
		AllElementData allElementData = default(AllElementData);
		allElementData.Datas = m_DataList;
		return Bson.ToBson(allElementData);
	}

	public void SetElementBsonData(byte[] data, bool ifCache = false)
	{
		allElementData = Bson.ToObject<AllElementData>(data);
		List<byte[]> list = new List<byte[]>();
		Dictionary<int, SubstanceUUIDPointer> dictionary = new Dictionary<int, SubstanceUUIDPointer>();
		int count = tileMapScripTable.m_uuidData.m_pointers.Count;
		for (int i = 0; i < count; i++)
		{
			dictionary[tileMapScripTable.m_uuidData.m_pointers[i].m_uuid] = tileMapScripTable.m_uuidData.m_pointers[i];
		}
		for (int j = 0; j < allElementData.Datas.Count; j++)
		{
			ElementData elementData = allElementData.Datas[j];
			SubstanceUUIDPointer value = null;
			if (dictionary.TryGetValue(elementData.UUID, out value))
			{
				GridData gridData = tileMapScripTable.m_gridDatas[value.m_pointer.m_gridIndex];
				GridCellData gridCellData = gridData.m_gridCellDatas[value.m_pointer.m_gridCellIndex];
				SubstanceData substanceData = gridCellData.m_substanceDatas[value.m_pointer.m_substanceIndex];
				int id = gridData.m_id;
				Point point = gridCellData.m_point;
				CreateElement(substanceData, id, point, elementData.InnerBsonData);
				list.Add(elementData.InnerBsonData);
			}
		}
		for (int k = 0; k < allElementList.Count; k++)
		{
			BaseElement baseElement = allElementList[k];
			if (baseElement.IfRebirthRecord)
			{
				try
				{
					baseElement.RebirthReadByteData(list[k]);
				}
				catch (Exception)
				{
					Debug.Log(baseElement.gameObject.name + "," + baseElement.m_id + "," + baseElement.m_uuId);
				}
			}
			DropData? dropData = GetInsideGameDataModule.FindDropDataByUUID(baseElement.m_uuId);
			if (dropData.HasValue && dropData.HasValue)
			{
				baseElement.RebirthReadByteDataForDrop(list[k]);
			}
		}
		if (!ifCache)
		{
			allElementData = default(AllElementData);
		}
	}

	public void StartRunByElementBsonData(byte[] data, bool ifCache = false)
	{
		if (!ifCache)
		{
			allElementData = Bson.ToObject<AllElementData>(data);
		}
		List<byte[]> list = new List<byte[]>();
		for (int i = 0; i < allElementData.Datas.Count; i++)
		{
			list.Add(allElementData.Datas[i].InnerBsonData);
		}
		for (int j = 0; j < allElementList.Count; j++)
		{
			BaseElement baseElement = allElementList[j];
			if (baseElement.IfRebirthRecord)
			{
				baseElement.RebirthStartByteGame(list[j]);
			}
			DropData? dropData = GetInsideGameDataModule.FindDropDataByUUID(baseElement.m_uuId);
			if (dropData.HasValue && dropData.HasValue)
			{
				baseElement.RebirthStartGameByteDataForDrop(list[j]);
			}
		}
		allElementData = default(AllElementData);
	}

	[Obsolete("this is Obsolete, please the CreateElement(byte[])")]
	private BaseElement CreateElement(SubstanceData substanceData, int gridID, Point point, string elementData)
	{
		Grid grid = m_grids[gridID];
		BrushID key = new BrushID(substanceData.m_type, substanceData.m_id);
		GameObject obj = elementBufferedPool[key].Dequeue();
		obj.transform.parent = grid.gameObject.transform;
		obj.transform.position = substanceData.m_position;
		obj.transform.eulerAngles = substanceData.m_rotation;
		obj.transform.localScale = substanceData.m_localScale;
		BaseElement component = obj.GetComponent<BaseElement>();
		component.groupTransform = grid.gameObject.transform;
		component.EnemyChildCount = substanceData.m_enemyChildData.m_childNum;
		component.TileChildCount = substanceData.m_tileChildData.m_childNum;
		component.ParentKey = substanceData.m_parentData.m_key;
		component.ReadBytes(substanceData.m_saveAttrs);
		component.point = point;
		component.m_gridId = grid.m_id;
		component.m_uuId = substanceData.m_uuID;
		allElementList.Add(component);
		if (substanceData.m_type == 0)
		{
			ApplyEnemyChild(substanceData, component, point);
			ApplyTileChild(substanceData, component, point);
			ApplyTileParent(component);
			ApplyGroupTileParent(component);
		}
		else if (substanceData.m_type == 1)
		{
			ApplyEnemyParent(component);
			ApplyGroupEnemyParent(component);
		}
		else if (substanceData.m_type == 4)
		{
			ApplyGroupEnemyChild(substanceData, component, point);
			ApplyGroupTileChild(substanceData, component, point);
		}
		component.Initialize();
		if ((bool)component.colider)
		{
			component.colider.SetActive(true);
		}
		ApplyLightmapData(substanceData, component);
		component.LateInitialize();
		return component;
	}

	private BaseElement CreateElement(SubstanceData substanceData, int gridID, Point point, byte[] elementData)
	{
		Grid grid = m_grids[gridID];
		BrushID key = new BrushID(substanceData.m_type, substanceData.m_id);
		GameObject obj = elementBufferedPool[key].Dequeue();
		obj.transform.parent = grid.gameObject.transform;
		obj.transform.position = substanceData.m_position;
		obj.transform.eulerAngles = substanceData.m_rotation;
		obj.transform.localScale = substanceData.m_localScale;
		BaseElement component = obj.GetComponent<BaseElement>();
		component.groupTransform = grid.gameObject.transform;
		component.EnemyChildCount = substanceData.m_enemyChildData.m_childNum;
		component.TileChildCount = substanceData.m_tileChildData.m_childNum;
		component.ParentKey = substanceData.m_parentData.m_key;
		component.ReadBytes(substanceData.m_saveAttrs);
		component.point = point;
		component.m_gridId = grid.m_id;
		component.m_uuId = substanceData.m_uuID;
		allElementList.Add(component);
		if (substanceData.m_type == 0)
		{
			ApplyEnemyChild(substanceData, component, point);
			ApplyTileChild(substanceData, component, point);
			ApplyTileParent(component);
			ApplyGroupTileParent(component);
		}
		else if (substanceData.m_type == 1)
		{
			ApplyEnemyParent(component);
			ApplyGroupEnemyParent(component);
		}
		else if (substanceData.m_type == 4)
		{
			ApplyGroupEnemyChild(substanceData, component, point);
			ApplyGroupTileChild(substanceData, component, point);
		}
		component.Initialize();
		if ((bool)component.colider)
		{
			component.colider.SetActive(true);
		}
		ApplyLightmapData(substanceData, component);
		component.LateInitialize();
		return component;
	}

	private void ApplyEnemyChild(SubstanceData substanceData, BaseElement brushBase, Point point)
	{
		int keyBy = ParentInfo.GetKeyBy(brushBase.m_gridId, point);
		List<BaseEnemy> value;
		if (tileEnemyChildDic.TryGetValue(keyBy, out value))
		{
			if (value.Count < brushBase.EnemyChildCount)
			{
				tileEnemyRootDic.Add(keyBy, (BaseTile)brushBase);
				for (int i = 0; i < value.Count; i++)
				{
					value[i].transform.parent = brushBase.transform;
					brushBase.EnemyChildCount--;
				}
				value.Clear();
			}
			else
			{
				for (int j = 0; j < value.Count; j++)
				{
					value[j].transform.parent = brushBase.transform;
					brushBase.EnemyChildCount--;
				}
				value.Clear();
				tileEnemyChildDic.Remove(keyBy);
			}
		}
		else
		{
			tileEnemyRootDic.Add(keyBy, (BaseTile)brushBase);
		}
	}

	private void ApplyGroupEnemyChild(SubstanceData substanceData, BaseElement brushBase, Point point)
	{
		int keyBy = ParentInfo.GetKeyBy(brushBase.m_gridId, point, true);
		List<BaseEnemy> value;
		if (groupEnemyChildDic.TryGetValue(keyBy, out value))
		{
			if (value.Count < brushBase.EnemyChildCount)
			{
				groupEnemyRootDic.Add(keyBy, (BaseGroup)brushBase);
				for (int i = 0; i < value.Count; i++)
				{
					value[i].transform.parent = brushBase.transform;
					brushBase.EnemyChildCount--;
				}
				value.Clear();
			}
			else
			{
				for (int j = 0; j < value.Count; j++)
				{
					value[j].transform.parent = brushBase.transform;
					brushBase.EnemyChildCount--;
				}
				value.Clear();
				groupEnemyChildDic.Remove(keyBy);
			}
		}
		else
		{
			groupEnemyRootDic.Add(keyBy, (BaseGroup)brushBase);
		}
	}

	private void ApplyTileChild(SubstanceData substanceData, BaseElement brushBase, Point point)
	{
		int keyBy = ParentInfo.GetKeyBy(brushBase.m_gridId, point);
		List<BaseTile> value;
		if (tileTileChildDic.TryGetValue(keyBy, out value))
		{
			if (value.Count < brushBase.TileChildCount)
			{
				tileTileRootDic.Add(keyBy, (BaseTile)brushBase);
				for (int i = 0; i < value.Count; i++)
				{
					value[i].transform.parent = brushBase.transform;
					brushBase.TileChildCount--;
				}
				value.Clear();
			}
			else
			{
				for (int j = 0; j < value.Count; j++)
				{
					value[j].transform.parent = brushBase.transform;
					brushBase.TileChildCount--;
				}
				value.Clear();
				tileTileChildDic.Remove(keyBy);
			}
		}
		else
		{
			tileTileRootDic.Add(keyBy, (BaseTile)brushBase);
		}
	}

	private void ApplyGroupTileChild(SubstanceData substanceData, BaseElement brushBase, Point point)
	{
		int keyBy = ParentInfo.GetKeyBy(brushBase.m_gridId, point, true);
		List<BaseTile> value;
		if (groupTileChildDic.TryGetValue(keyBy, out value))
		{
			if (value.Count < brushBase.TileChildCount)
			{
				groupTileRootDic.Add(keyBy, (BaseGroup)brushBase);
				for (int i = 0; i < value.Count; i++)
				{
					value[i].transform.parent = brushBase.transform;
					brushBase.TileChildCount--;
				}
				value.Clear();
			}
			else
			{
				for (int j = 0; j < value.Count; j++)
				{
					value[j].transform.parent = brushBase.transform;
					brushBase.TileChildCount--;
				}
				value.Clear();
				groupTileChildDic.Remove(keyBy);
			}
		}
		else
		{
			groupTileRootDic.Add(keyBy, (BaseGroup)brushBase);
		}
	}

	private void ApplyTileParent(BaseElement brushBase)
	{
		if (ParentInfo.IfGroup(brushBase.ParentKey))
		{
			return;
		}
		BaseTile value;
		if (tileTileRootDic.TryGetValue(brushBase.ParentKey, out value))
		{
			brushBase.transform.parent = value.transform;
			value.TileChildCount--;
			if (value.TileChildCount == 0)
			{
				tileTileRootDic.Remove(brushBase.ParentKey);
			}
		}
		else if (tileTileChildDic.ContainsKey(brushBase.ParentKey))
		{
			tileTileChildDic[brushBase.ParentKey].Add((BaseTile)brushBase);
		}
		else
		{
			List<BaseTile> list = new List<BaseTile>();
			list.Add((BaseTile)brushBase);
			tileTileChildDic.Add(brushBase.ParentKey, list);
		}
	}

	private void ApplyGroupTileParent(BaseElement brushBase)
	{
		if (!ParentInfo.IfGroup(brushBase.ParentKey))
		{
			return;
		}
		BaseGroup value;
		if (groupTileRootDic.TryGetValue(brushBase.ParentKey, out value))
		{
			brushBase.transform.parent = value.transform;
			value.TileChildCount--;
			if (value.TileChildCount == 0)
			{
				groupTileRootDic.Remove(brushBase.ParentKey);
			}
		}
		else if (groupTileChildDic.ContainsKey(brushBase.ParentKey))
		{
			groupTileChildDic[brushBase.ParentKey].Add(brushBase as BaseTile);
		}
		else
		{
			List<BaseTile> list = new List<BaseTile>();
			list.Add(brushBase as BaseTile);
			groupTileChildDic.Add(brushBase.ParentKey, list);
		}
	}

	private void ApplyGroupEnemyParent(BaseElement brushBase)
	{
		if (!ParentInfo.IfGroup(brushBase.ParentKey))
		{
			return;
		}
		BaseGroup value;
		if (groupEnemyRootDic.TryGetValue(brushBase.ParentKey, out value))
		{
			brushBase.transform.parent = value.transform;
			brushBase.name += "Child";
			value.EnemyChildCount--;
			if (value.EnemyChildCount == 0)
			{
				groupEnemyRootDic.Remove(brushBase.ParentKey);
			}
		}
		else if (groupEnemyChildDic.ContainsKey(brushBase.ParentKey))
		{
			groupEnemyChildDic[brushBase.ParentKey].Add(brushBase as BaseEnemy);
		}
		else
		{
			List<BaseEnemy> list = new List<BaseEnemy>();
			list.Add(brushBase as BaseEnemy);
			groupEnemyChildDic.Add(brushBase.ParentKey, list);
		}
	}

	private void ApplyEnemyParent(BaseElement brushBase)
	{
		BaseTile value;
		if (tileEnemyRootDic.TryGetValue(brushBase.ParentKey, out value))
		{
			brushBase.transform.parent = value.transform;
			brushBase.name += "Child";
			value.EnemyChildCount--;
			if (value.EnemyChildCount == 0)
			{
				tileEnemyRootDic.Remove(brushBase.ParentKey);
			}
		}
		else if (tileEnemyChildDic.ContainsKey(brushBase.ParentKey))
		{
			tileEnemyChildDic[brushBase.ParentKey].Add((BaseEnemy)brushBase);
		}
		else
		{
			List<BaseEnemy> list = new List<BaseEnemy>();
			list.Add((BaseEnemy)brushBase);
			tileEnemyChildDic.Add(brushBase.ParentKey, list);
		}
	}

	private void ApplyLightmapData(SubstanceData substanceData, BaseElement brushBase)
	{
		List<LightingMapSubstanceData> lightingMapSubstanceDatas = substanceData.m_lightingMapSubstanceGroupData.m_lightingMapSubstanceDatas;
		if (lightingMapSubstanceDatas.Count > 0 && GetLightmapData.Length != 0)
		{
			Renderer[] componentsInChildren = brushBase.GetComponentsInChildren<Renderer>();
			for (int i = 0; i < componentsInChildren.Length && i < lightingMapSubstanceDatas.Count; i++)
			{
				componentsInChildren[i].lightmapIndex = lightingMapSubstanceDatas[i].m_lightmapIndex;
				componentsInChildren[i].lightmapScaleOffset = lightingMapSubstanceDatas[i].m_lightmapScaleOffset;
			}
		}
	}

	public object GetOriginRebirthData(object obj = null)
	{
		List<OriginRebirthGroupData> list = new List<OriginRebirthGroupData>();
		for (int i = 0; i < curGroupList.Count; i++)
		{
			list.Add(new OriginRebirthGroupData(curGroupList[i].m_gridData.m_id, curGroupList[i].m_createRowNum, curGroupList[i].m_worldRowNum));
		}
		return new MapData(_curGrid.m_id, m_curPoint, m_createRow, m_roleStartRow, list.ToArray()).ToJson();
	}

	public void SetOriginRebirthData(object dataInfo)
	{
		MapData fromJson = MapData.GetFromJson((string)dataInfo);
		_curGrid = m_grids[fromJson.m_gridID];
		m_curPoint = fromJson.m_point;
		m_createRow = fromJson.m_createRow;
		m_roleStartRow = fromJson.m_roleStartRow;
		for (int i = 0; i < fromJson.OriginRebirthGroupDatas.Length; i++)
		{
			OriginRebirthGroupData originRebirthGroupData = fromJson.OriginRebirthGroupDatas[i];
			curGroupList.Add(new GroupData(tileMapScripTable.m_gridDatas[originRebirthGroupData.CurrentGridDataId], originRebirthGroupData.CreateRowNum, originRebirthGroupData.WorldRow));
		}
	}

	public void StartRunByOriginRebirthData(object dataInfo)
	{
	}

	public byte[] GetOriginRebirthBsonData(object obj = null)
	{
		List<OriginRebirthGroupData> list = new List<OriginRebirthGroupData>();
		for (int i = 0; i < curGroupList.Count; i++)
		{
			list.Add(new OriginRebirthGroupData(curGroupList[i].m_gridData.m_id, curGroupList[i].m_createRowNum, curGroupList[i].m_worldRowNum));
		}
		return Bson.ToBson(new MapData(_curGrid.m_id, m_curPoint, m_createRow, m_roleStartRow, list.ToArray()));
	}

	public void SetOriginRebirthBsonData(byte[] dataInfo)
	{
		MapData mapData = Bson.ToObject<MapData>(dataInfo);
		_curGrid = m_grids[mapData.m_gridID];
		m_curPoint = mapData.m_point;
		m_createRow = mapData.m_createRow;
		m_roleStartRow = mapData.m_roleStartRow;
		for (int i = 0; i < mapData.OriginRebirthGroupDatas.Length; i++)
		{
			OriginRebirthGroupData originRebirthGroupData = mapData.OriginRebirthGroupDatas[i];
			curGroupList.Add(new GroupData(tileMapScripTable.m_gridDatas[originRebirthGroupData.CurrentGridDataId], originRebirthGroupData.CreateRowNum, originRebirthGroupData.WorldRow));
		}
	}

	public void StartRunByOriginRebirthBsonData(byte[] dataInfo)
	{
	}
}
