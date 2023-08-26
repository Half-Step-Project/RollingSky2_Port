using System.Collections.Generic;
using RS2;
using UnityEngine;

public class BackgroundManager
{
	public static Dictionary<int, BackgroundData> BackDataDic = new Dictionary<int, BackgroundData>
	{
		{
			1,
			new BackgroundData(1, "Assets/_RS2Art/Res/Prefab/Backgrounds/Back_XiaoWangZi_Sky.prefab", "", typeof(BackgroundElement), null)
		},
		{
			2,
			new BackgroundData(2, "Assets/_RS2Art/Res/Prefab/Backgrounds/Back_Desert_Sky.prefab", "", typeof(DesertBackground), null)
		},
		{
			3,
			new BackgroundData(3, "Assets/_RS2Art/Res/Prefab/Backgrounds/Back_Fate_Sky.prefab", "", typeof(DesertBackground), null)
		},
		{
			4,
			new BackgroundData(4, "Assets/_RS2Art/Res/Prefab/Backgrounds/Back_BaiRiMeng_Sky.prefab", "", typeof(HomelandBack), null)
		},
		{
			5,
			new BackgroundData(5, "Assets/_RS2Art/Res/Prefab/Backgrounds/Back_Thief_Sky.prefab", "", typeof(HomelandBack), null)
		},
		{
			6,
			new BackgroundData(6, "Assets/_RS2Art/Res/Prefab/Backgrounds/Back_Tutorial_Sky.prefab", "", typeof(HomelandBack), null)
		},
		{
			7,
			new BackgroundData(7, "Assets/_RS2Art/Res/Prefab/Backgrounds/Back_Rainbow_Sky.prefab", "", typeof(HomelandBack), null)
		},
		{
			8,
			new BackgroundData(8, "Assets/_RS2Art/Res/Prefab/Backgrounds/Back_Normal_Desert.prefab", "", typeof(NormalBackground), null)
		},
		{
			9,
			new BackgroundData(9, "Assets/_RS2Art/Res/Prefab/Backgrounds/Back_Thief02_Sky.prefab", "", typeof(NormalBackground), null)
		}
	};

	public static BaseBackgroundElement CreateBackground(int id)
	{
		BackgroundData value = null;
		BaseBackgroundElement baseBackgroundElement = null;
		if (BackDataDic.TryGetValue(id, out value))
		{
			string backPrefabPath = value.BackPrefabPath;
			GameObject gameObject = Object.Instantiate(LevelResources.theResource.GetLevelResource(backPrefabPath) as GameObject);
			baseBackgroundElement = gameObject.GetComponent<BaseBackgroundElement>();
			if (baseBackgroundElement == null)
			{
				baseBackgroundElement = gameObject.AddComponent(value.BackClassType) as BaseBackgroundElement;
				baseBackgroundElement.CurrentBackData = value;
			}
		}
		else
		{
			Debug.Log("the BackId " + id + " can't be found!");
		}
		return baseBackgroundElement;
	}
}
