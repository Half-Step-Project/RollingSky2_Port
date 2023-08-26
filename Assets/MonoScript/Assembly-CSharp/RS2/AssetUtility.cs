using System;
using System.Collections.Generic;
using Foundation;
using UnityEngine;

namespace RS2
{
	public static class AssetUtility
	{
		private static Dictionary<string, string> _configPaths = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		private static Dictionary<string, string> _dataTablePaths = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		private static Dictionary<string, string> _dictionaryPaths = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		private static Dictionary<string, string> _fontPaths = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		private static Dictionary<string, string> _scenePaths = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		private static Dictionary<string, string> _musicPaths = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		private static Dictionary<string, string> _soundPaths = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		private static Dictionary<string, string> _entityPaths = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		private static Dictionary<string, string> _uiformPaths = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		private static Dictionary<string, string> _uispritePaths = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		private static Dictionary<string, string> _uiLanguagesSpritePaths = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		private static Dictionary<string, string> _uiSoundPaths = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		private static Dictionary<string, string> _gameIconPaths = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		private static Dictionary<string, string> _gameUIItemPaths = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		private static Dictionary<string, string> _resBrushPaths = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		private static Dictionary<string, string> _tablePaths = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		public static string GetConfigAsset(string assetName)
		{
			string value;
			if (_configPaths.TryGetValue(assetName, out value))
			{
				return value;
			}
			return string.Format("Assets/GamePlay/_Resources/Configs/{0}.txt", assetName);
		}

		public static string GetDataTableAsset(string assetName)
		{
			string value;
			if (_dataTablePaths.TryGetValue(assetName, out value))
			{
				return value;
			}
			return string.Format("Assets/GamePlay/_Resources/DataTables/{0}.bytes", assetName);
		}

		public static string GetDictionaryAsset(string assetName)
		{
			string value;
			if (_dictionaryPaths.TryGetValue(assetName, out value))
			{
				return value;
			}
			return string.Format("Assets/GamePlay/_Resources/Localization/{0}/{1}.txt", Mod.Localization.Language.ToString(), assetName);
		}

		public static string GetFontAsset(string assetName)
		{
			string value;
			if (_fontPaths.TryGetValue(assetName, out value))
			{
				return value;
			}
			return string.Format("Assets/GamePlay/_Resources/Localization/{0}/{1}.ttf", Mod.Localization.Language.ToString(), assetName);
		}

		public static string GetSceneAsset(string assetName)
		{
			string value;
			if (_scenePaths.TryGetValue(assetName, out value))
			{
				return value;
			}
			return string.Format("Assets/_RS2Art/Scene/{0}.unity", assetName);
		}

		public static string GetMusicAsset(string assetName)
		{
			string value;
			if (_musicPaths.TryGetValue(assetName, out value))
			{
				return value;
			}
			Debug.Log("assetName : " + assetName);
			return string.Format("Assets/_RS2Art/Res/Base/Music/{0}.mp3", assetName);
		}

		public static string GetSoundAsset(string assetName)
		{
			string value;
			if (_soundPaths.TryGetValue(assetName, out value))
			{
				return value;
			}
			return string.Format("Assets/GamePlay/_Resources/Sounds/{0}.wav", assetName);
		}

		public static string GetEntityAsset(string assetName)
		{
			string value;
			if (_entityPaths.TryGetValue(assetName, out value))
			{
				return value;
			}
			return string.Format("Assets/GamePlay/_Resources/Entities/{0}.prefab", assetName);
		}

		public static string GetUIFormAsset(string assetName)
		{
			string value;
			if (_uiformPaths.TryGetValue(assetName, out value))
			{
				return value;
			}
			return string.Format("Assets/_RS2Art/UI/UIForms/{0}.prefab", assetName);
		}

		public static string GetUISpriteAsset(string assetName)
		{
			string value;
			if (_uispritePaths.TryGetValue(assetName, out value))
			{
				return value;
			}
			return string.Format("Assets/_RS2Art/UI/UISprites/{0}.png", assetName);
		}

		public static string GetUILanguagesSpriteAsset(string assetName)
		{
			string value;
			if (_uiLanguagesSpritePaths.TryGetValue(assetName, out value))
			{
				return value;
			}
			return string.Format("Assets/_RS2Art/UI/UILanguages/{0}.png", assetName);
		}

		public static string GetUISoundAsset(string assetName)
		{
			string value;
			if (_uiSoundPaths.TryGetValue(assetName, out value))
			{
				return value;
			}
			return string.Format("Assets/_RS2Art/UI/UISounds/{0}.wav", assetName);
		}

		public static string GetGameIconAsset(int iconId)
		{
			return GetGameIconAsset(iconId.ToString());
		}

		public static string GetGameIconAsset(string assetName)
		{
			string value;
			if (_gameIconPaths.TryGetValue(assetName, out value))
			{
				return value;
			}
			return string.Format("Assets/_RS2Art/UI/UIIcons/{0}.png", assetName);
		}

		public static string GetPlayerAsset(string assetName)
		{
			return string.Format("Assets/_RS2Art/UI/Sprites/{0}.prefab", assetName);
		}

		public static string GetGameUIItemAsset(string assetName)
		{
			string value;
			if (_gameUIItemPaths.TryGetValue(assetName, out value))
			{
				return value;
			}
			return string.Format("Assets/_RS2Art/UI/UIItems/{0}.prefab", assetName);
		}

		public static string GetResBrushesAsset(string assetName)
		{
			string value;
			if (_resBrushPaths.TryGetValue(assetName, out value))
			{
				return value;
			}
			return string.Format("Assets/_RS2Art/Res/{0}.prefab", assetName);
		}

		public static string GetTableAsset(string assetName)
		{
			string value;
			if (_tablePaths.TryGetValue(assetName, out value))
			{
				return value;
			}
			return string.Format("Assets/_RS2Art/Res/{0}.asset", assetName);
		}

		public static string GetRolePrefabAsset(string assetName)
		{
			return string.Format("Assets/_RS2Art/Res/Prefab/Roles/{0}.prefab", assetName);
		}

		public static string GetRoleConfigAsset(string assetName)
		{
			return string.Format("Assets/_RS2Art/Res/Base/RoleConfigure/{0}.asset", assetName);
		}

		public static string GetCouplePrefabAsset(string assetName)
		{
			return string.Format("Assets/_RS2Art/Res/Prefab/Couples/{0}.prefab", assetName);
		}

		public static string GetWorldConfigAsset(string assetName)
		{
			return string.Format("Assets/_RS2Art/Res/Base/WorldConfigure/{0}.asset", assetName);
		}

		public static string GetPropPrefabAsset(string assetName)
		{
			return string.Format("Assets/_RS2Art/Res/Prefab/Props/{0}.prefab", assetName);
		}

		public static string GetFairyPrefabAsset(string assetName)
		{
			return string.Format("Assets/_RS2Art/Res/Prefab/Fairys/{0}.prefab", assetName);
		}

		public static string GetLevelDataAsset(string assetName)
		{
			return string.Format("Assets/_RS2Art/Res/Base/LevelData/{0}.asset", assetName);
		}

		public static string GetPetPrefabAsset(string assetName)
		{
			return string.Format("Assets/_RS2Art/Res/Prefab/Pets/{0}.prefab", assetName);
		}

		public static string GetBackgroundPrefab(string assetName)
		{
			return string.Format("Assets/_RS2Art/Res/Prefab/Backgrounds/{0}.prefab", assetName);
		}

		public static string GetShaderVariantsAsset(string assetName)
		{
			return string.Format("Assets/_RS2Art/Art/Shader/CustomShader/{0}.shadervariants", assetName);
		}

		public static string GetCustomShaderAsset(string assetName)
		{
			return string.Format("Assets/_RS2Art/Art/Shader/CustomShader/{0}.shader", assetName);
		}

		public static string GetUIMaterialAsset(string assetName)
		{
			return string.Format("Assets/_RS2Art/UI/UIMaterials/{0}.mat", assetName, assetName);
		}

		public static string GetInstrumentAsset(string assetName)
		{
			return string.Format("Assets/_RS2Art/UI/Instruments/{0}.prefab", assetName);
		}
	}
}
