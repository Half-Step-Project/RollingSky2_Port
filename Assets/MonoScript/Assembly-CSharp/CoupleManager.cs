using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;

public class CoupleManager
{
	public static Dictionary<int, CoupleData> CoupleDataDic = new Dictionary<int, CoupleData>
	{
		{
			1,
			new CoupleData(1, "CoupleThief", typeof(CoupleThief), null)
		},
		{
			2,
			new CoupleData(2, "CoupleThief2", typeof(CoupleThief), null)
		}
	};

	public static BaseCouple CreateCouple(int id)
	{
		BaseCouple baseCouple = null;
		CoupleData value = null;
		if (CoupleDataDic.TryGetValue(id, out value))
		{
			string couplePrefabAsset = AssetUtility.GetCouplePrefabAsset(value.CouplePrefabPath);
			GameObject gameObject = LevelResources.theResource.GetLevelResource(couplePrefabAsset) as GameObject;
			if (gameObject != null)
			{
				baseCouple = Object.Instantiate(gameObject).AddComponent(value.CoupleClassType) as BaseCouple;
				baseCouple.CurrentCoupleData = value;
			}
			if ((bool)baseCouple)
			{
				baseCouple.PreInitialize();
			}
			else
			{
				Log.Error("Remember to modify the file in:GamePlay._Resources.LevelResourceTables.");
			}
		}
		return baseCouple;
	}
}
