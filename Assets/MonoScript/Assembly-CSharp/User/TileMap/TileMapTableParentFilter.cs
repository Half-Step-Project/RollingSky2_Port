using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using RS2;
using UnityEngine;

namespace User.TileMap
{
	public static class TileMapTableParentFilter
	{
		private class MathGridData
		{
			public int m_startMapIndex;

			public GridData m_gridData;

			private int m_currentIndex;

			public MathGridData(int startMapIndex, GridData gridData)
			{
				m_startMapIndex = startMapIndex;
				m_gridData = gridData;
			}

			public bool IsCanRun()
			{
				if (m_currentIndex + 1 <= m_gridData.m_height)
				{
					return true;
				}
				return false;
			}

			public void Run(ref GridData gridData)
			{
				for (int i = 0; i < m_gridData.m_width; i++)
				{
					GridCellData gridCellDataByPoint = m_gridData.m_gridCellDatas.GetGridCellDataByPoint(new Point(i, m_currentIndex));
					GridCellData gridCellData = gridData.m_gridCellDatas.GetGridCellDataByPoint(new Point(i, m_startMapIndex + m_currentIndex));
					if (gridCellDataByPoint != null)
					{
						if (gridCellData == null)
						{
							gridCellData = new GridCellData();
							gridCellData.m_point = new Point(i, m_startMapIndex + m_currentIndex);
							gridData.m_gridCellDatas.Add(gridCellData);
							gridCellData.m_substanceDatas.Clear();
						}
						gridCellData.m_substanceDatas.AddRange(gridCellDataByPoint.m_substanceDatas);
					}
				}
				m_currentIndex++;
			}

			public void Reset()
			{
				m_currentIndex = 0;
			}
		}

		private class MergeData
		{
			public GridData m_gridData;

			public List<MathGridData> m_mathGridDatas;
		}

		private class PointDistance
		{
			public Point m_startPoint;

			public Point m_endPoint;
		}

		private static List<PathData> PathGroup = new List<PathData>();

		private static List<PathData> EndPathGroup = new List<PathData>();

		private static List<GridData> BranchGroup = new List<GridData>();

		private static Dictionary<string, int> bufferData = new Dictionary<string, int>();

		public static int BufferedRowNum;

		public static TileMapScripTable DealParent(this TileMapScripTable tileMap)
		{
			for (int i = 0; i < tileMap.m_gridDatas.Count; i++)
			{
				GridData gridData = tileMap.m_gridDatas[i];
				for (int j = 0; j < gridData.m_gridCellDatas.Count; j++)
				{
					GridCellData gridCellData = gridData.m_gridCellDatas[j];
					SubstanceData substanceData = null;
					int num = 0;
					for (int k = 0; k < gridCellData.m_substanceDatas.Count; k++)
					{
						SubstanceData substanceData2 = gridCellData.m_substanceDatas[k];
						substanceData2.m_parentData = default(ParentInfo);
						substanceData2.m_parentData.Reset();
						substanceData2.m_enemyChildData = default(ChildInfo);
						substanceData2.m_enemyChildData.Reset();
						substanceData2.m_tileChildData = default(ChildInfo);
						substanceData2.m_tileChildData.Reset();
						if (substanceData2.m_type == 0)
						{
							substanceData = substanceData2;
							num++;
							if (num > 1)
							{
								Debug.LogError(string.Format("TileConflict!Grid[{0}]&Item[{1}]&Point[{2},{3}]", gridData.m_id, j, gridCellData.m_point.m_x, gridCellData.m_point.m_y));
							}
						}
					}
					for (int l = 0; l < gridCellData.m_substanceDatas.Count; l++)
					{
						SubstanceData substanceData3 = gridCellData.m_substanceDatas[l];
						if (substanceData3.m_type == 1 && substanceData != null)
						{
							substanceData3.m_parentData.m_isGroup = 0;
							substanceData3.m_parentData.m_gridId = gridData.m_id;
							substanceData3.m_parentData.m_point = gridCellData.m_point;
							substanceData.m_enemyChildData.m_childNum++;
						}
					}
				}
			}
			return tileMap;
		}

		public static TileMapScripTable DealMagicCubeParent(this TileMapScripTable tileMap)
		{
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
			Type typeFromHandle = typeof(MagicCubeTile3x3);
			Type typeFromHandle2 = typeof(MagicCubeTile5x5);
			Dictionary<int, BrushData>.Enumerator enumerator = BrushsManager.GetBrushDatasByType(TileObjectType.Tile).GetEnumerator();
			int num = 0;
			BrushData brushData = null;
			while (enumerator.MoveNext())
			{
				num = enumerator.Current.Key;
				brushData = enumerator.Current.Value;
				if (brushData.m_typeClass == typeFromHandle)
				{
					dictionary.Add(num, num);
				}
				else if (brushData.m_typeClass == typeFromHandle2)
				{
					dictionary2.Add(num, num);
				}
			}
			for (int i = 0; i < tileMap.m_gridDatas.Count; i++)
			{
				GridData gridData = tileMap.m_gridDatas[i];
				Dictionary<Point, SubstanceData> dictionary3 = new Dictionary<Point, SubstanceData>();
				Dictionary<Point, SubstanceData> dictionary4 = new Dictionary<Point, SubstanceData>();
				for (int j = 0; j < gridData.m_gridCellDatas.Count; j++)
				{
					GridCellData gridCellData = gridData.m_gridCellDatas[j];
					for (int k = 0; k < gridCellData.m_substanceDatas.Count; k++)
					{
						SubstanceData substanceData = gridCellData.m_substanceDatas[k];
						if (substanceData.m_type == 0)
						{
							if (dictionary.ContainsKey(substanceData.m_id))
							{
								dictionary3.Add(gridCellData.m_point, substanceData);
							}
							else if (dictionary2.ContainsKey(substanceData.m_id))
							{
								dictionary4.Add(gridCellData.m_point, substanceData);
							}
						}
					}
				}
				Dictionary<Point, SubstanceData>.Enumerator enumerator2 = dictionary3.GetEnumerator();
				while (enumerator2.MoveNext())
				{
					Point key = enumerator2.Current.Key;
					SubstanceData value = enumerator2.Current.Value;
					value.m_enemyChildData.Reset();
					List<Point> list = new List<Point>();
					list.Add(key);
					list.Add(new Point(key.m_x - 1, key.m_y));
					list.Add(new Point(key.m_x - 2, key.m_y));
					list.Add(new Point(key.m_x - 3, key.m_y));
					list.Add(new Point(key.m_x + 1, key.m_y));
					list.Add(new Point(key.m_x + 2, key.m_y));
					list.Add(new Point(key.m_x + 3, key.m_y));
					for (int l = 0; l < list.Count; l++)
					{
						GridCellData value2 = null;
						if (!gridData.m_gridCellDatasDictionary.TryGetValue(list[l], out value2))
						{
							continue;
						}
						for (int m = 0; m < value2.m_substanceDatas.Count; m++)
						{
							SubstanceData substanceData2 = value2.m_substanceDatas[m];
							if (substanceData2.m_type == 1)
							{
								substanceData2.m_parentData.Reset();
								substanceData2.m_parentData.m_isGroup = 0;
								substanceData2.m_parentData.m_gridId = gridData.m_id;
								substanceData2.m_parentData.m_point = key;
								value.m_enemyChildData.m_childNum++;
								Debug.Log(string.Format("CubeConnect3x3:Item[{0}]&Point[{1},{2}]", value.m_id, value2.m_point.m_x, value2.m_point.m_y));
							}
						}
					}
				}
				Dictionary<Point, SubstanceData>.Enumerator enumerator3 = dictionary4.GetEnumerator();
				while (enumerator3.MoveNext())
				{
					Point key2 = enumerator3.Current.Key;
					SubstanceData value3 = enumerator3.Current.Value;
					value3.m_enemyChildData.Reset();
					List<Point> list2 = new List<Point>();
					list2.Add(key2);
					list2.Add(new Point(key2.m_x - 1, key2.m_y));
					list2.Add(new Point(key2.m_x - 2, key2.m_y));
					list2.Add(new Point(key2.m_x - 3, key2.m_y));
					list2.Add(new Point(key2.m_x + 1, key2.m_y));
					list2.Add(new Point(key2.m_x + 2, key2.m_y));
					list2.Add(new Point(key2.m_x + 3, key2.m_y));
					for (int n = 0; n < list2.Count; n++)
					{
						GridCellData value4 = null;
						if (!gridData.m_gridCellDatasDictionary.TryGetValue(list2[n], out value4))
						{
							continue;
						}
						for (int num2 = 0; num2 < value4.m_substanceDatas.Count; num2++)
						{
							SubstanceData substanceData3 = value4.m_substanceDatas[num2];
							if (substanceData3.m_type == 1)
							{
								substanceData3.m_parentData.Reset();
								substanceData3.m_parentData.m_isGroup = 0;
								substanceData3.m_parentData.m_gridId = gridData.m_id;
								substanceData3.m_parentData.m_point = key2;
								value3.m_enemyChildData.m_childNum++;
								Debug.Log(string.Format("CubeConnect5x5:Item[{0}]&Point[{1},{2}]", value3.m_id, value4.m_point.m_x, value4.m_point.m_y));
							}
						}
					}
				}
			}
			return tileMap;
		}

		public static TileMapScripTable DealTrapGroup(this TileMapScripTable tileMap)
		{
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
			Type typeFromHandle = typeof(TrapRootTile);
			Type typeFromHandle2 = typeof(TrapTriggerTile);
			Type typeFromHandle3 = typeof(TrapChildTile);
			Dictionary<int, BrushData>.Enumerator enumerator = BrushsManager.GetBrushDatasByType(TileObjectType.Tile).GetEnumerator();
			while (enumerator.MoveNext())
			{
				int key = enumerator.Current.Key;
				BrushData value = enumerator.Current.Value;
				if (value.m_typeClass == typeFromHandle)
				{
					dictionary.Add(key, key);
				}
				else if (value.m_typeClass == typeFromHandle2)
				{
					dictionary.Add(key, key);
				}
				else if (value.m_typeClass == typeFromHandle3)
				{
					dictionary2.Add(key, key);
				}
			}
			for (int i = 0; i < tileMap.m_gridDatas.Count; i++)
			{
				Dictionary<Point, GroupBlockData> dictionary3 = new Dictionary<Point, GroupBlockData>();
				Dictionary<Point, GroupBlockData> dictionary4 = new Dictionary<Point, GroupBlockData>();
				GridData gridData = tileMap.m_gridDatas[i];
				for (int j = 0; j < gridData.m_gridCellDatas.Count; j++)
				{
					GridCellData gridCellData = gridData.m_gridCellDatas[j];
					for (int k = 0; k < gridCellData.m_substanceDatas.Count; k++)
					{
						SubstanceData substanceData = gridCellData.m_substanceDatas[k];
						if (substanceData.m_type == 0)
						{
							GroupBlockData groupBlockData = null;
							if (dictionary.ContainsKey(substanceData.m_id))
							{
								groupBlockData = GetTrapGenerateDataBySubData(substanceData);
								dictionary3.Add(gridCellData.m_point, groupBlockData);
							}
							else if (dictionary2.ContainsKey(substanceData.m_id))
							{
								groupBlockData = GetTrapGenerateDataBySubData(substanceData);
								dictionary4.Add(gridCellData.m_point, groupBlockData);
							}
						}
					}
				}
				Dictionary<Point, GroupBlockData>.Enumerator enumerator2 = dictionary3.GetEnumerator();
				while (enumerator2.MoveNext())
				{
					Point key2 = enumerator2.Current.Key;
					GroupBlockData value2 = enumerator2.Current.Value;
					List<Point> list = new List<Point>();
					list.Add(key2);
					for (int l = 0; l < list.Count; l++)
					{
						Point key3 = list[l];
						GroupBlockData value3 = null;
						if (!dictionary3.TryGetValue(key3, out value3) && !dictionary4.TryGetValue(key3, out value3))
						{
							continue;
						}
						List<Point> list2 = new List<Point>();
						if (!value3.m_leftBlock)
						{
							list2.Add(new Point(key3.m_x - 1, key3.m_y));
						}
						if (!value3.m_rightBlock)
						{
							list2.Add(new Point(key3.m_x + 1, key3.m_y));
						}
						if (!value3.m_upBlock)
						{
							list2.Add(new Point(key3.m_x, key3.m_y + 1));
						}
						if (!value3.m_downBlock)
						{
							list2.Add(new Point(key3.m_x, key3.m_y - 1));
						}
						for (int m = 0; m < list2.Count; m++)
						{
							GroupBlockData value4;
							if (!list.Contains(list2[m]) && dictionary4.TryGetValue(list2[m], out value4))
							{
								list.Add(list2[m]);
								value4.m_substanceData.m_parentData.m_isGroup = 0;
								value4.m_substanceData.m_parentData.m_gridId = gridData.m_id;
								value4.m_substanceData.m_parentData.m_point = key2;
								value2.m_substanceData.m_tileChildData.m_childNum++;
							}
						}
					}
				}
			}
			return tileMap;
		}

		public static TileMapScripTable DealGlassGroup(this TileMapScripTable tileMap)
		{
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
			Type typeFromHandle = typeof(GlassRootTile);
			Type typeFromHandle2 = typeof(GlassChildTile);
			Dictionary<int, BrushData>.Enumerator enumerator = BrushsManager.GetBrushDatasByType(TileObjectType.Tile).GetEnumerator();
			while (enumerator.MoveNext())
			{
				int key = enumerator.Current.Key;
				BrushData value = enumerator.Current.Value;
				if (value.m_typeClass == typeFromHandle)
				{
					dictionary.Add(key, key);
				}
				else if (value.m_typeClass == typeFromHandle2)
				{
					dictionary2.Add(key, key);
				}
			}
			for (int i = 0; i < tileMap.m_gridDatas.Count; i++)
			{
				Dictionary<Point, GroupBlockData> dictionary3 = new Dictionary<Point, GroupBlockData>();
				Dictionary<Point, GroupBlockData> dictionary4 = new Dictionary<Point, GroupBlockData>();
				GridData gridData = tileMap.m_gridDatas[i];
				for (int j = 0; j < gridData.m_gridCellDatas.Count; j++)
				{
					GridCellData gridCellData = gridData.m_gridCellDatas[j];
					for (int k = 0; k < gridCellData.m_substanceDatas.Count; k++)
					{
						SubstanceData substanceData = gridCellData.m_substanceDatas[k];
						if (substanceData.m_type == 0)
						{
							GroupBlockData groupBlockData = null;
							if (dictionary.ContainsKey(substanceData.m_id))
							{
								groupBlockData = GetGlassGenerateDataBySubData(substanceData);
								dictionary3.Add(gridCellData.m_point, groupBlockData);
							}
							else if (dictionary2.ContainsKey(substanceData.m_id))
							{
								groupBlockData = GetGlassGenerateDataBySubData(substanceData);
								dictionary4.Add(gridCellData.m_point, groupBlockData);
							}
						}
					}
				}
				Dictionary<Point, GroupBlockData>.Enumerator enumerator2 = dictionary3.GetEnumerator();
				while (enumerator2.MoveNext())
				{
					Point key2 = enumerator2.Current.Key;
					GroupBlockData value2 = enumerator2.Current.Value;
					List<Point> list = new List<Point>();
					list.Add(key2);
					for (int l = 0; l < list.Count; l++)
					{
						Point key3 = list[l];
						GroupBlockData value3 = null;
						if (!dictionary3.TryGetValue(key3, out value3) && !dictionary4.TryGetValue(key3, out value3))
						{
							continue;
						}
						List<Point> list2 = new List<Point>();
						if (!value3.m_leftBlock)
						{
							list2.Add(new Point(key3.m_x - 1, key3.m_y));
						}
						if (!value3.m_rightBlock)
						{
							list2.Add(new Point(key3.m_x + 1, key3.m_y));
						}
						if (!value3.m_upBlock)
						{
							list2.Add(new Point(key3.m_x, key3.m_y + 1));
						}
						if (!value3.m_downBlock)
						{
							list2.Add(new Point(key3.m_x, key3.m_y - 1));
						}
						for (int m = 0; m < list2.Count; m++)
						{
							GroupBlockData value4;
							if (!list.Contains(list2[m]) && dictionary4.TryGetValue(list2[m], out value4))
							{
								list.Add(list2[m]);
								value4.m_substanceData.m_parentData.m_isGroup = 0;
								value4.m_substanceData.m_parentData.m_gridId = gridData.m_id;
								value4.m_substanceData.m_parentData.m_point = key2;
								value2.m_substanceData.m_tileChildData.m_childNum++;
							}
						}
					}
				}
			}
			return tileMap;
		}

		private static GroupBlockData GetTrapGenerateDataBySubData(SubstanceData subData)
		{
			return new GroupBlockData
			{
				m_substanceData = subData
			};
		}

		private static GroupBlockData GetGlassGenerateDataBySubData(SubstanceData subData)
		{
			return new GroupBlockData
			{
				m_substanceData = subData
			};
		}

		public static TileMapScripTable DealRandomAnimRelation(this TileMapScripTable tileMap)
		{
			HashSet<int> hashSet = new HashSet<int>();
			HashSet<int> hashSet2 = new HashSet<int>();
			HashSet<int> hashSet3 = new HashSet<int>();
			HashSet<int> hashSet4 = new HashSet<int>();
			HashSet<int> hashSet5 = new HashSet<int>();
			List<SubstanceData> list = new List<SubstanceData>();
			List<SubstanceData> list2 = new List<SubstanceData>();
			List<SubstanceData> list3 = new List<SubstanceData>();
			List<SubstanceData> list4 = new List<SubstanceData>();
			List<SubstanceData> list5 = new List<SubstanceData>();
			Type typeFromHandle = typeof(RandomAnimTrigger);
			Type typeFromHandle2 = typeof(RandomAnimEnemy);
			Type typeFromHandle3 = typeof(RandomTileTrigger);
			Type typeFromHandle4 = typeof(RandomAnimTile);
			Type typeFromHandle5 = typeof(RandomNormalDiamond);
			Type typeFromHandle6 = typeof(RandomNormalCrown);
			Dictionary<int, BrushData> brushDatasByType = BrushsManager.GetBrushDatasByType(TileObjectType.Trigger);
			Dictionary<int, BrushData> brushDatasByType2 = BrushsManager.GetBrushDatasByType(TileObjectType.Enemy);
			Dictionary<int, BrushData> brushDatasByType3 = BrushsManager.GetBrushDatasByType(TileObjectType.Tile);
			foreach (int key in brushDatasByType.Keys)
			{
				Type typeClass = brushDatasByType[key].m_typeClass;
				if (typeClass == typeFromHandle)
				{
					hashSet.Add(key);
				}
				else if (typeClass == typeFromHandle3)
				{
					hashSet3.Add(key);
				}
			}
			foreach (int key2 in brushDatasByType2.Keys)
			{
				Type typeClass2 = brushDatasByType2[key2].m_typeClass;
				if (typeClass2 == typeFromHandle2)
				{
					hashSet2.Add(key2);
				}
				else if (typeClass2 == typeFromHandle5)
				{
					hashSet5.Add(key2);
				}
				else if (typeClass2 == typeFromHandle6)
				{
					hashSet5.Add(key2);
				}
			}
			foreach (int key3 in brushDatasByType3.Keys)
			{
				if (brushDatasByType3[key3].m_typeClass == typeFromHandle4)
				{
					hashSet4.Add(key3);
				}
			}
			for (int i = 0; i < tileMap.m_gridDatas.Count; i++)
			{
				GridData gridData = tileMap.m_gridDatas[i];
				for (int j = 0; j < gridData.m_gridCellDatas.Count; j++)
				{
					GridCellData gridCellData = gridData.m_gridCellDatas[j];
					for (int k = 0; k < gridCellData.m_substanceDatas.Count; k++)
					{
						SubstanceData substanceData = gridCellData.m_substanceDatas[k];
						if (substanceData.m_type == 3)
						{
							if (hashSet.Contains(substanceData.m_id))
							{
								list.Add(substanceData);
							}
							else if (hashSet3.Contains(substanceData.m_id))
							{
								list3.Add(substanceData);
							}
						}
						else if (substanceData.m_type == 1)
						{
							if (hashSet2.Contains(substanceData.m_id))
							{
								list2.Add(substanceData);
							}
							else if (hashSet5.Contains(substanceData.m_id))
							{
								list5.Add(substanceData);
							}
						}
						else if (substanceData.m_type == 0 && hashSet4.Contains(substanceData.m_id))
						{
							list4.Add(substanceData);
						}
					}
				}
			}
			foreach (SubstanceData item in list)
			{
				int uuID = item.m_uuID;
				RandomAnimTrigger.TriggerData triggerData = default(RandomAnimTrigger.TriggerData);
				triggerData.ReadBytes(item.m_saveAttrs);
				Bounds collideBounds = triggerData.CollideBounds;
				foreach (SubstanceData item2 in list2)
				{
					if (collideBounds.Contains(item2.m_position))
					{
						RandomAnimEnemy.EnemyData enemyData = default(RandomAnimEnemy.EnemyData);
						enemyData.ReadBytes(item2.m_saveAttrs);
						if (enemyData.TriggerUuid > 0)
						{
							Debug.LogError("RandomAnimEenmy Used by:" + enemyData.TriggerUuid + " & " + uuID);
						}
						enemyData.TriggerUuid = uuID;
						item2.m_saveAttrs = enemyData.WriteBytes();
						Debug.Log(item2.m_uuID + " control by trigger :" + uuID);
					}
				}
				foreach (SubstanceData item3 in list5)
				{
					if (collideBounds.Contains(item3.m_position))
					{
						RandomNormalAward.AwardData awardData = default(RandomNormalAward.AwardData);
						awardData.ReadBytes(item3.m_saveAttrs);
						if (awardData.TriggerUuid > 0)
						{
							Debug.LogError("RandomNormalAward Used by:" + awardData.TriggerUuid + " & " + uuID);
						}
						awardData.TriggerUuid = uuID;
						item3.m_saveAttrs = awardData.WriteBytes();
						Debug.Log(item3.m_uuID + " control by trigger :" + uuID);
					}
				}
			}
			foreach (SubstanceData item4 in list3)
			{
				int uuID2 = item4.m_uuID;
				RandomTileTrigger.TriggerData triggerData2 = default(RandomTileTrigger.TriggerData);
				triggerData2.ReadBytes(item4.m_saveAttrs);
				Bounds collideBounds2 = triggerData2.CollideBounds;
				foreach (SubstanceData item5 in list4)
				{
					if (collideBounds2.Contains(item5.m_position))
					{
						RandomAnimTile.TileData tileData = default(RandomAnimTile.TileData);
						tileData.ReadBytes(item5.m_saveAttrs);
						if (tileData.TriggerUuid > 0)
						{
							Debug.LogError("RandomAnimTile Used by:" + tileData.TriggerUuid + " & " + uuID2);
						}
						tileData.TriggerUuid = uuID2;
						item5.m_saveAttrs = tileData.WriteBytes();
						Debug.Log(item5.m_uuID + " control by trigger :" + uuID2);
					}
				}
			}
			return tileMap;
		}

		public static TileMapScripTable DealRowGroup(this TileMapScripTable tileMap)
		{
			Type typeFromHandle = typeof(HorizonMoveTile);
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			Dictionary<int, BrushData>.Enumerator enumerator = BrushsManager.GetBrushDatasByType(TileObjectType.Tile).GetEnumerator();
			while (enumerator.MoveNext())
			{
				int key = enumerator.Current.Key;
				if (enumerator.Current.Value.m_typeClass == typeFromHandle)
				{
					dictionary.Add(key, key);
				}
			}
			for (int i = 0; i < tileMap.m_gridDatas.Count; i++)
			{
				GridData gridData = tileMap.m_gridDatas[i];
				Dictionary<Point, SubstanceData> dictionary2 = new Dictionary<Point, SubstanceData>();
				Dictionary<Point, SubstanceData> dictionary3 = new Dictionary<Point, SubstanceData>();
				for (int j = 0; j < gridData.m_gridCellDatas.Count; j++)
				{
					GridCellData gridCellData = gridData.m_gridCellDatas[j];
					for (int k = 0; k < gridCellData.m_substanceDatas.Count; k++)
					{
						SubstanceData substanceData = gridCellData.m_substanceDatas[k];
						if (substanceData.m_type == 0)
						{
							dictionary.ContainsKey(substanceData.m_id);
						}
					}
				}
				Dictionary<Point, SubstanceData>.Enumerator enumerator2 = dictionary2.GetEnumerator();
				while (enumerator2.MoveNext())
				{
					Point key2 = enumerator2.Current.Key;
					SubstanceData value = enumerator2.Current.Value;
					int y = key2.m_y;
					for (int l = 0; l < 21; l++)
					{
						Point key3 = new Point(l, y);
						SubstanceData value2 = null;
						if (dictionary3.TryGetValue(key3, out value2))
						{
							value2.m_parentData.m_isGroup = 0;
							value2.m_parentData.m_gridId = gridData.m_id;
							value2.m_parentData.m_point = key2;
							value.m_tileChildData.m_childNum++;
						}
					}
				}
			}
			return tileMap;
		}

		public static TileMapScripTable DealBuyOutPercent(this TileMapScripTable tileMap)
		{
			return tileMap;
		}

		public static TileMapScripTable DealBuyOutRealPercent(this TileMapScripTable tileMap)
		{
			LevelResources levelResources = UnityEngine.Object.FindObjectOfType<LevelResources>();
			Railway railway = UnityEngine.Object.FindObjectOfType<Railway>();
			Grid[] array = UnityEngine.Object.FindObjectsOfType<Grid>();
			BuyOutRebirthBoxTrigger[] array2 = UnityEngine.Object.FindObjectsOfType<BuyOutRebirthBoxTrigger>();
			Transform transform = null;
			WorldConfigureScriptTable worldConfigureScriptTable = null;
			Grid grid = null;
			if ((bool)railway)
			{
				transform = railway.transform.Find(Railway.NodeRoleStart);
			}
			if ((bool)levelResources)
			{
				worldConfigureScriptTable = levelResources.WorldConfigureTable;
			}
			if (array != null && array.Length != 0)
			{
				grid = array.Where((Grid g) => g.m_id == 0).First();
			}
			if (levelResources != null && railway != null && transform != null && grid != null && array2 != null && array2.Length != 0)
			{
				int buyOutLevelLength = GetBuyOutLevelLength(tileMap);
				int levelDeltaLength = worldConfigureScriptTable.m_worldStartInfo.m_levelDeltaLength;
				int num = MathUtils.Ceiled(grid.transform.InverseTransformPoint(railway.transform.TransformPoint(transform.position)).z);
				buyOutLevelLength = buyOutLevelLength - num + levelDeltaLength;
				int num2 = MathUtils.Ceiled(grid.transform.InverseTransformPoint(transform.position).z);
				HashSet<int> hashSet = new HashSet<int>();
				Type typeFromHandle = typeof(BuyOutRebirthBoxTrigger);
				Dictionary<int, BrushData> brushDatasByType = BrushsManager.GetBrushDatasByType(TileObjectType.Trigger);
				foreach (int key in brushDatasByType.Keys)
				{
					if (brushDatasByType[key].m_typeClass == typeFromHandle)
					{
						hashSet.Add(key);
					}
				}
				for (int i = 0; i < tileMap.m_gridDatas.Count; i++)
				{
					GridData gridData = tileMap.m_gridDatas[i];
					for (int j = 0; j < gridData.m_gridCellDatas.Count; j++)
					{
						GridCellData cellData = gridData.m_gridCellDatas[j];
						for (int k = 0; k < cellData.m_substanceDatas.Count; k++)
						{
							SubstanceData substanceData = cellData.m_substanceDatas[k];
							if (substanceData.m_type == 3 && hashSet.Contains(substanceData.m_id))
							{
								BuyOutRebirthBoxTrigger buyOutRebirthBoxTrigger = array2.Where((BuyOutRebirthBoxTrigger buyOutItem) => buyOutItem.point == cellData.m_point).First();
								int num3 = GetCurrentItemLength(tileMap, buyOutRebirthBoxTrigger.GetComponentInParent<Grid>(), buyOutRebirthBoxTrigger.m_gridId, buyOutRebirthBoxTrigger) - num2;
								int num4 = Mathf.FloorToInt((float)num3 * 100f / (float)buyOutLevelLength);
								BuyOutRebirthBoxTrigger.TileData tileData = default(BuyOutRebirthBoxTrigger.TileData);
								tileData.ReadBytes(substanceData.m_saveAttrs);
								tileData.progress = num4;
								substanceData.m_saveAttrs = tileData.WriteBytes();
								Log.Info("BuyOut Percent =>> " + num4 + ":" + num3 + " / " + buyOutLevelLength);
							}
						}
					}
				}
			}
			return tileMap;
		}

		public static TileMapScripTable DealBuyOutPercentByRebirth(this TileMapScripTable tileMap, OriginRebirthData rebirthData)
		{
			Grid[] array = UnityEngine.Object.FindObjectsOfType<Grid>();
			int num = -1;
			Type typeFromHandle = typeof(BuyOutRebirthBoxTrigger);
			Dictionary<int, BrushData>.Enumerator enumerator = BrushsManager.GetBrushDatasByType(TileObjectType.Trigger).GetEnumerator();
			int num2 = 0;
			while (enumerator.MoveNext())
			{
				num2 = enumerator.Current.Key;
				if (enumerator.Current.Value.m_typeClass == typeFromHandle)
				{
					num = num2;
					break;
				}
			}
			Dictionary<int, SubstanceUUIDPointer> dictionary = new Dictionary<int, SubstanceUUIDPointer>();
			if (tileMap != null && rebirthData != null && array != null && num > 0)
			{
				int count = tileMap.m_uuidData.m_pointers.Count;
				for (int i = 0; i < count; i++)
				{
					dictionary[tileMap.m_uuidData.m_pointers[i].m_uuid] = tileMap.m_uuidData.m_pointers[i];
				}
				for (int j = 0; j < tileMap.m_gridDatas.Count; j++)
				{
					GridData gridData = tileMap.m_gridDatas[j];
					for (int k = 0; k < gridData.m_gridCellDatas.Count; k++)
					{
						GridCellData gridCellData = gridData.m_gridCellDatas[k];
						for (int l = 0; l < gridCellData.m_substanceDatas.Count; l++)
						{
							SubstanceData substanceData = gridCellData.m_substanceDatas[l];
							if (substanceData.m_type == 3 && substanceData.m_id == num)
							{
								BuyOutRebirthBoxTrigger.TileData tileData = default(BuyOutRebirthBoxTrigger.TileData);
								tileData.ReadBytes(substanceData.m_saveAttrs);
								SingleOriginRebirthForRowData originRebirthForRowValueByPosition = GameController.GetOriginRebirthForRowValueByPosition(array, tileMap, rebirthData, substanceData.m_position);
								OriginRebirthProgressData originRebirthProgressData = default(OriginRebirthProgressData);
								originRebirthProgressData = ((!RecordOriginRebirthManager.m_isBson) ? ProgressController.LoadData(originRebirthForRowValueByPosition.m_rowValue.m_sceneData.m_progressData) : ProgressController.LoadData(originRebirthForRowValueByPosition.m_rowValue.m_sceneData.ProgressData));
								Log.Info("BuyOut Percent =>> " + originRebirthProgressData.m_levelProgress);
								tileData.progress = originRebirthProgressData.m_levelProgress;
								substanceData.m_saveAttrs = tileData.WriteBytes();
							}
						}
					}
				}
			}
			return tileMap;
		}

		private static int GetBuyOutLevelLength(TileMapScripTable tileMap)
		{
			int num = 0;
			num += tileMap.m_gridDatas[0].m_height;
			for (int i = 0; i < tileMap.m_gridDatas.Count - 1; i++)
			{
				List<GridData> gridDatasByParentID = tileMap.m_gridDatas.GetGridDatasByParentID(tileMap.m_gridDatas[i].m_id);
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

		private static int GetCurrentItemLength(TileMapScripTable tileMap, Grid gridItem, int gridId, BaseElement element)
		{
			int result = 0;
			GridData gridData2 = tileMap.m_gridDatas.Where((GridData gridData) => gridData.m_id == gridItem.m_id).First();
			if (gridData2 != null)
			{
				int beforeTheLength = gridData2.m_beforeTheLength;
				int num = MathUtils.Ceiled(gridItem.transform.InverseTransformPoint(element.transform.position).z);
				result = beforeTheLength + num;
			}
			return result;
		}

		public static Grid[] GetGridsByIsFirst(this Grid[] grids)
		{
			List<Grid> list = new List<Grid>();
			for (int i = 0; i < grids.Length; i++)
			{
				if (grids[i].m_isFrist)
				{
					list.Insert(0, grids[i]);
				}
				else
				{
					list.Add(grids[i]);
				}
			}
			return list.ToArray();
		}

		private static void GetPathBufferByParentId(TileMapScripTable tileMapScripTable)
		{
			for (int i = 0; i < PathGroup.Count; i++)
			{
				if (PathGroup[i].isSearchFinished)
				{
					continue;
				}
				List<GridData> gridDatasByParentID = tileMapScripTable.m_gridDatas.GetGridDatasByParentID(PathGroup[i].GetEndGridDataId());
				if (gridDatasByParentID.Count == 0)
				{
					PathGroup[i].EndSearchPath();
					EndPathGroup.Add(PathGroup[i]);
					PathGroup.RemoveAt(i);
					i--;
				}
				List<GridData> pathPoints = null;
				for (int j = 0; j < gridDatasByParentID.Count; j++)
				{
					if (gridDatasByParentID.Count > 1)
					{
						BranchGroup.Add(gridDatasByParentID[j]);
					}
					if (j == 0)
					{
						pathPoints = new List<GridData>(PathGroup[i].pathPoints);
						PathGroup[i].AddPathPoint(gridDatasByParentID[j]);
					}
					else
					{
						PathGroup.Add(new PathData(pathPoints, gridDatasByParentID[j]));
					}
				}
			}
			if (PathGroup.Count > 0)
			{
				GetPathBufferByParentId(tileMapScripTable);
			}
		}

		private static void BranchBufferSize()
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			for (int i = 0; i < BranchGroup.Count; i++)
			{
				Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
				GridData gridData = BranchGroup[i];
				int num = ((gridData.m_height > BufferedRowNum) ? BufferedRowNum : gridData.m_height);
				for (int j = 0; j < num; j++)
				{
					for (int k = 0; k < gridData.m_width; k++)
					{
						Point key = new Point(k, j);
						if (!gridData.m_gridCellDatasDictionary.ContainsKey(key))
						{
							continue;
						}
						GridCellData gridCellData = gridData.m_gridCellDatasDictionary[key];
						for (int l = 0; l < gridCellData.m_substanceDatas.Count; l++)
						{
							SubstanceData substanceData = gridCellData.m_substanceDatas[l];
							string key2 = substanceData.m_type + "_" + substanceData.m_id;
							if (dictionary2.ContainsKey(key2))
							{
								dictionary2[key2]++;
							}
							else
							{
								dictionary2[key2] = 1;
							}
						}
					}
				}
				Dictionary<string, int>.Enumerator enumerator = dictionary2.GetEnumerator();
				while (enumerator.MoveNext())
				{
					string key3 = enumerator.Current.Key;
					int value = enumerator.Current.Value;
					if (dictionary.ContainsKey(key3))
					{
						if (dictionary[key3] < value)
						{
							dictionary[key3] = value;
						}
					}
					else
					{
						dictionary[key3] = value;
					}
				}
			}
			Dictionary<string, int>.Enumerator enumerator2 = dictionary.GetEnumerator();
			while (enumerator2.MoveNext())
			{
				string key4 = enumerator2.Current.Key;
				int value2 = enumerator2.Current.Value;
				if (bufferData.ContainsKey(key4))
				{
					bufferData[key4] += value2;
				}
				else
				{
					bufferData[key4] = value2;
				}
			}
		}

		public static TileMapScripTable DealBufferData(this TileMapScripTable tileMapScriptTable, int lenght)
		{
			MergeData mergeData = MergeAllGrid(tileMapScriptTable.m_gridDatas);
			GridData gridData = mergeData.m_gridData;
			SetBeforeTheLength(tileMapScriptTable, mergeData);
			tileMapScriptTable.m_brushBufferDataList = MathBufferData(gridData, lenght);
			tileMapScriptTable.m_progressData = MathProgressData(gridData);
			return tileMapScriptTable;
		}

		private static MergeData MergeAllGrid(List<GridData> gridDatas)
		{
			MergeData mergeData = new MergeData();
			GridData gridData = new GridData();
			GridData gridDataByID = gridDatas.GetGridDataByID(0);
			int width = gridDataByID.m_width;
			gridData.m_width = gridDataByID.m_width;
			for (int i = 0; i < gridDatas.Count; i++)
			{
				if (width < gridDatas[i].m_width)
				{
					width = gridDatas[i].m_width;
				}
			}
			if (gridDataByID == null)
			{
				return null;
			}
			int num = 0;
			List<MathGridData> list = new List<MathGridData>();
			List<MathGridData> list2 = new List<MathGridData>();
			list.Add(new MathGridData(0, gridDataByID));
			list2.Add(new MathGridData(0, gridDataByID));
			gridData.m_gridCellDatas = new List<GridCellData>();
			while (true)
			{
				List<MathGridData> list3 = new List<MathGridData>();
				List<MathGridData> list4 = new List<MathGridData>();
				for (int j = 0; j < list.Count; j++)
				{
					if (!list[j].IsCanRun())
					{
						list3.Add(list[j]);
						List<GridData> gridDatasByParentID = gridDatas.GetGridDatasByParentID(list[j].m_gridData.m_id);
						for (int k = 0; k < gridDatasByParentID.Count; k++)
						{
							list2.Add(new MathGridData(num, gridDatasByParentID[k]));
							list4.Add(new MathGridData(num, gridDatasByParentID[k]));
						}
					}
				}
				for (int l = 0; l < list3.Count; l++)
				{
					list.Remove(list3[l]);
				}
				for (int m = 0; m < list4.Count; m++)
				{
					list.Add(list4[m]);
				}
				for (int n = 0; n < list.Count; n++)
				{
					list[n].Run(ref gridData);
				}
				if (list.Count <= 0)
				{
					break;
				}
				num++;
			}
			gridData.m_position = gridDataByID.m_position;
			gridData.m_rotation = gridDataByID.m_rotation;
			gridData.m_samplingInterval = gridDataByID.m_samplingInterval;
			gridData.m_samplingCenterY = gridDataByID.m_samplingCenterY;
			gridData.m_id = gridDataByID.m_id;
			gridData.m_height = num;
			gridData.m_width = width;
			mergeData.m_gridData = gridData;
			mergeData.m_mathGridDatas = list2;
			return mergeData;
		}

		private static PointDistance GetPointDistance(GridData gridData)
		{
			int num = 2;
			int num2 = 72;
			PointDistance pointDistance = new PointDistance();
			pointDistance.m_startPoint = new Point(0, 0);
			pointDistance.m_endPoint = new Point(0, gridData.m_height - 1);
			for (int i = 0; i < gridData.m_gridCellDatas.Count; i++)
			{
				GridCellData gridCellData = gridData.m_gridCellDatas[i];
				for (int j = 0; j < gridCellData.m_substanceDatas.Count; j++)
				{
					SubstanceData substanceData = gridCellData.m_substanceDatas[j];
					if (substanceData.m_type == 3 && substanceData.m_id == num)
					{
						pointDistance.m_startPoint = gridCellData.m_point;
					}
					if (substanceData.m_type == 0 && substanceData.m_id == num2)
					{
						pointDistance.m_endPoint = gridCellData.m_point;
					}
				}
			}
			return pointDistance;
		}

		private static void SetBeforeTheLength(TileMapScripTable tileMapScriptTable, MergeData mergeData)
		{
			for (int i = 0; i < tileMapScriptTable.m_gridDatas.Count; i++)
			{
				GridData gridData = tileMapScriptTable.m_gridDatas[i];
				for (int j = 0; j < mergeData.m_mathGridDatas.Count; j++)
				{
					GridData gridData2 = mergeData.m_mathGridDatas[j].m_gridData;
					if (gridData.m_id == gridData2.m_id)
					{
						gridData.m_beforeTheLength = mergeData.m_mathGridDatas[j].m_startMapIndex;
					}
				}
			}
		}

		private static List<BrushBufferData> MathBufferData(GridData gridData, int deffaultHeight = 80)
		{
			List<BrushBufferData> list = new List<BrushBufferData>();
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			gridData.m_gridCellDatasDictionary = gridData.m_gridCellDatas.ListToDictionary();
			int num = 0;
			while (true)
			{
				Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
				int num2 = ((num + deffaultHeight >= gridData.m_height) ? gridData.m_height : (num + deffaultHeight));
				for (int i = num; i < num2; i++)
				{
					for (int j = 0; j < gridData.m_width; j++)
					{
						GridCellData gridCellData = (gridData.m_gridCellDatasDictionary.ContainsKey(new Point(j, i)) ? gridData.m_gridCellDatasDictionary[new Point(j, i)] : null);
						if (gridCellData == null)
						{
							continue;
						}
						for (int k = 0; k < gridCellData.m_substanceDatas.Count; k++)
						{
							bool flag = true;
							string key = gridCellData.m_substanceDatas[k].m_type + "_" + gridCellData.m_substanceDatas[k].m_id;
							if (gridCellData.m_substanceDatas[k].m_type == 3)
							{
								flag = false;
							}
							if (gridCellData.m_substanceDatas[k].m_type == 2 && BrushsManager.GetBrushDataByTypeAddBrushID(TileObjectType.Midground, gridCellData.m_substanceDatas[k].m_id).m_typeClass.ToString() == typeof(UnrecycleMidground).ToString())
							{
								flag = false;
							}
							if (flag)
							{
								if (!dictionary2.ContainsKey(key))
								{
									dictionary2.Add(key, 0);
								}
								dictionary2[key]++;
							}
						}
					}
				}
				Dictionary<string, int>.Enumerator enumerator = dictionary2.GetEnumerator();
				while (enumerator.MoveNext())
				{
					string key2 = enumerator.Current.Key;
					int value = enumerator.Current.Value;
					if (!dictionary.ContainsKey(key2))
					{
						dictionary[key2] = value;
					}
					else if (dictionary[key2] < value)
					{
						dictionary[key2] = value;
					}
				}
				if (num2 == gridData.m_height)
				{
					break;
				}
				num++;
			}
			for (int l = 0; l < gridData.m_height; l++)
			{
				for (int m = 0; m < gridData.m_width; m++)
				{
					GridCellData gridCellData2 = (gridData.m_gridCellDatasDictionary.ContainsKey(new Point(m, l)) ? gridData.m_gridCellDatasDictionary[new Point(m, l)] : null);
					if (gridCellData2 == null)
					{
						continue;
					}
					for (int n = 0; n < gridCellData2.m_substanceDatas.Count; n++)
					{
						bool flag2 = false;
						string key3 = gridCellData2.m_substanceDatas[n].m_type + "_" + gridCellData2.m_substanceDatas[n].m_id;
						if (gridCellData2.m_substanceDatas[n].m_type == 3)
						{
							flag2 = true;
						}
						if (gridCellData2.m_substanceDatas[n].m_type == 2 && BrushsManager.GetBrushDataByTypeAddBrushID(TileObjectType.Midground, gridCellData2.m_substanceDatas[n].m_id).m_typeClass.ToString() == typeof(UnrecycleMidground).ToString())
						{
							flag2 = true;
						}
						if (flag2)
						{
							if (!dictionary.ContainsKey(key3))
							{
								dictionary.Add(key3, 0);
							}
							dictionary[key3]++;
						}
					}
				}
			}
			Dictionary<string, int>.Enumerator enumerator2 = dictionary.GetEnumerator();
			while (enumerator2.MoveNext())
			{
				BrushBufferData item = default(BrushBufferData);
				string[] array = enumerator2.Current.Key.Split('_');
				item.m_type = int.Parse(array[0]);
				item.m_id = int.Parse(array[1]);
				item.m_size = enumerator2.Current.Value;
				list.Add(item);
			}
			return (from data in list
				orderby data.m_type, data.m_id
				select data).ToList();
		}

		private static ProgressData MathProgressData(GridData mergeGridData)
		{
			ProgressData result = default(ProgressData);
			PointDistance pointDistance = GetPointDistance(mergeGridData);
			result.m_length = mergeGridData.m_height;
			result.m_startRow = pointDistance.m_startPoint.m_y;
			result.m_endRow = pointDistance.m_endPoint.m_y;
			result.m_actualLength = result.m_endRow - result.m_startRow;
			return result;
		}

		public static TileMapScripTable DealUUID(this TileMapScripTable tileMapScriptTable)
		{
			int num = 10000;
			List<GridData> gridDatas = tileMapScriptTable.m_gridDatas;
			SubstanceUUIDData substanceUUIDData = new SubstanceUUIDData();
			for (int i = 0; i < gridDatas.Count; i++)
			{
				List<GridCellData> gridCellDatas = tileMapScriptTable.m_gridDatas[i].m_gridCellDatas;
				for (int j = 0; j < gridCellDatas.Count; j++)
				{
					List<SubstanceData> substanceDatas = gridCellDatas[j].m_substanceDatas;
					for (int k = 0; k < substanceDatas.Count; k++)
					{
						BrushsManager.GetBrushDataByTypeAddBrushID((TileObjectType)substanceDatas[k].m_type, substanceDatas[k].m_id);
						substanceDatas[k].m_index = k;
						SubstanceUUIDPointer substanceUUIDPointer = new SubstanceUUIDPointer();
						substanceDatas[k].m_uuID = num;
						substanceUUIDPointer.m_uuid = num;
						num++;
						substanceUUIDPointer.m_pointer = new SubstancePointer();
						substanceUUIDPointer.m_pointer.m_gridIndex = i;
						substanceUUIDPointer.m_pointer.m_gridCellIndex = j;
						substanceUUIDPointer.m_pointer.m_substanceIndex = k;
						substanceUUIDData.m_pointers.Add(substanceUUIDPointer);
					}
				}
			}
			tileMapScriptTable.m_uuidData = substanceUUIDData;
			return tileMapScriptTable;
		}

		public static TileMapScripTable DealGroupRelation(this TileMapScripTable tileMapScriptTable)
		{
			HashSet<int> hashSet = new HashSet<int>();
			List<SubstanceData> list = new List<SubstanceData>();
			List<Point> list2 = new List<Point>();
			Type typeFromHandle = typeof(FollowRotatePlatform);
			BrushsManager.GetBrushDatasByType(TileObjectType.Trigger);
			BrushsManager.GetBrushDatasByType(TileObjectType.Enemy);
			BrushsManager.GetBrushDatasByType(TileObjectType.Tile);
			Dictionary<int, BrushData> brushDatasByType = BrushsManager.GetBrushDatasByType(TileObjectType.Group);
			foreach (int key in brushDatasByType.Keys)
			{
				if (brushDatasByType[key].m_typeClass == typeFromHandle)
				{
					hashSet.Add(key);
				}
			}
			for (int i = 0; i < tileMapScriptTable.m_gridDatas.Count; i++)
			{
				GridData gridData = tileMapScriptTable.m_gridDatas[i];
				for (int j = 0; j < gridData.m_gridCellDatas.Count; j++)
				{
					GridCellData gridCellData = gridData.m_gridCellDatas[j];
					for (int k = 0; k < gridCellData.m_substanceDatas.Count; k++)
					{
						SubstanceData substanceData = gridCellData.m_substanceDatas[k];
						if (substanceData.m_type == 4 && hashSet.Contains(substanceData.m_id))
						{
							list.Add(substanceData);
							list2.Add(gridCellData.m_point);
						}
					}
				}
			}
			for (int l = 0; l < list.Count; l++)
			{
				SubstanceData substanceData2 = list[l];
				Point point = list2[l];
				FollowRotatePlatform.GroupData groupData = default(FollowRotatePlatform.GroupData);
				groupData.ReadBytes(substanceData2.m_saveAttrs);
				Bounds collideBounds = groupData.CollideBounds;
				for (int m = 0; m < tileMapScriptTable.m_gridDatas.Count; m++)
				{
					GridData gridData2 = tileMapScriptTable.m_gridDatas[m];
					for (int n = 0; n < gridData2.m_gridCellDatas.Count; n++)
					{
						GridCellData gridCellData2 = gridData2.m_gridCellDatas[n];
						for (int num = 0; num < gridCellData2.m_substanceDatas.Count; num++)
						{
							SubstanceData substanceData3 = gridCellData2.m_substanceDatas[num];
							if (substanceData3.m_parentData.m_key < 0 && collideBounds.Contains(substanceData3.m_position))
							{
								if (substanceData3.m_type == 0)
								{
									substanceData3.m_parentData.m_isGroup = 1;
									substanceData3.m_parentData.m_gridId = gridData2.m_id;
									substanceData3.m_parentData.m_point = point;
									substanceData2.m_tileChildData.m_childNum++;
								}
								else if (substanceData3.m_type == 1)
								{
									substanceData3.m_parentData.m_isGroup = 1;
									substanceData3.m_parentData.m_gridId = gridData2.m_id;
									substanceData3.m_parentData.m_point = point;
									substanceData2.m_enemyChildData.m_childNum++;
								}
							}
						}
					}
				}
			}
			return tileMapScriptTable;
		}
	}
}
