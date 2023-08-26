using System;
using System.Collections.Generic;
using Foundation;
using UnityEngine;
using User.TileMap;

namespace RS2
{
	public class LevelResources : MonoBehaviour
	{
		public static LevelResources theResource;

		private List<string> m_ResourcePathList;

		private Dictionary<string, UnityEngine.Object> m_AllLevelResources;

		public UnityEngine.Object m_LevelResourceTable;

		public TileMapScripTable m_Tilemap;

		public WorldConfigureScriptTable m_worldConfigureTable;

		public OriginRebirthData m_originRebirthData;

		public RolePathTable m_rolePathTable;

		public TileMapScripTable TileMap
		{
			get
			{
				if (m_Tilemap != null)
				{
					return m_Tilemap;
				}
				Log.Error(string.Format("Scene({0})'s Tilemap is null !", base.gameObject.scene.name), base.gameObject);
				return null;
			}
			set
			{
				m_Tilemap = value;
			}
		}

		public WorldConfigureScriptTable WorldConfigureTable
		{
			get
			{
				return m_worldConfigureTable;
			}
		}

		public OriginRebirthData OriginRebirth
		{
			get
			{
				return m_originRebirthData;
			}
			set
			{
				m_originRebirthData = value;
			}
		}

		public RolePathTable RolePath
		{
			get
			{
				return m_rolePathTable;
			}
		}

		private void Awake()
		{
			if (theResource == null)
			{
				theResource = this;
			}
			if (m_LevelResourceTable == null)
			{
				Log.Error("Level Resource Table is Null !");
				return;
			}
			TextAsset textAsset = m_LevelResourceTable as TextAsset;
			m_ResourcePathList = new List<string>(textAsset.text.Split(new char[2] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));
			m_AllLevelResources = new Dictionary<string, UnityEngine.Object>(StringComparer.OrdinalIgnoreCase);
		}

		public List<string> GetLevelResourcesPath()
		{
			return m_ResourcePathList;
		}

		public bool IsResourcLoaded(string path)
		{
			return m_AllLevelResources.ContainsKey(path);
		}

		public void AddResource(string key, UnityEngine.Object obj)
		{
			if (!m_AllLevelResources.ContainsKey(key))
			{
				m_AllLevelResources.Add(key, obj);
			}
		}

		public UnityEngine.Object GetLevelResource(string key)
		{
			UnityEngine.Object value;
			if (m_AllLevelResources.TryGetValue(key, out value))
			{
				return value;
			}
			Log.Info(string.Format("{0} is not loaded", key));
			return value;
		}

		public void OnLevelExit()
		{
			Dictionary<string, UnityEngine.Object>.Enumerator enumerator = m_AllLevelResources.GetEnumerator();
			while (enumerator.MoveNext())
			{
				Mod.Resource.UnloadAsset(enumerator.Current.Value);
			}
			m_AllLevelResources.Clear();
			m_ResourcePathList.Clear();
		}

		private void OnDestroy()
		{
			if (theResource != null)
			{
				theResource = null;
			}
		}

		public TextAsset GetLevelResourceTable()
		{
			return m_LevelResourceTable as TextAsset;
		}

		public TileMapScripTable GetTileMapTable()
		{
			return m_Tilemap;
		}

		public WorldConfigureScriptTable GetWorldConfigTable()
		{
			return m_worldConfigureTable;
		}

		public OriginRebirthData GetOriginRebirthData()
		{
			return m_originRebirthData;
		}

		public void SetLevelResourceTable(TextAsset resourceTable)
		{
			m_LevelResourceTable = resourceTable;
		}

		public void SetTileMapTable(TileMapScripTable tilemap)
		{
			m_Tilemap = tilemap;
		}

		public void SetWorldConfigTable(WorldConfigureScriptTable worldConfig)
		{
			m_worldConfigureTable = worldConfig;
		}

		public void SetOriginRebirthData(OriginRebirthData originData)
		{
			m_originRebirthData = originData;
		}
	}
}
