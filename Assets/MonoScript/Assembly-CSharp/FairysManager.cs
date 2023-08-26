using System.Collections.Generic;
using RS2;
using UnityEngine;

public class FairysManager
{
	public static Dictionary<int, FairyData> mDatas = new Dictionary<int, FairyData>
	{
		{
			1,
			new FairyData
			{
				mID = 1,
				mType = typeof(FairyOther),
				mPath = "Fairys_1"
			}
		},
		{
			2,
			new FairyData
			{
				mID = 2,
				mType = typeof(FairyOther),
				mPath = "Fairys_2"
			}
		},
		{
			3,
			new FairyData
			{
				mID = 3,
				mType = typeof(FairyOther),
				mPath = "Fairys_3"
			}
		},
		{
			4,
			new FairyData
			{
				mID = 4,
				mType = typeof(FairyOther),
				mPath = "Fairys_4"
			}
		},
		{
			5,
			new FairyData
			{
				mID = 5,
				mType = typeof(FairyOther),
				mPath = "Fairys_5"
			}
		},
		{
			6,
			new FairyData
			{
				mID = 6,
				mType = typeof(FairyOther),
				mPath = "Fairys_6"
			}
		},
		{
			7,
			new FairyData
			{
				mID = 7,
				mType = typeof(FairyOther),
				mPath = "Fairys_7"
			}
		},
		{
			8,
			new FairyData
			{
				mID = 8,
				mType = typeof(FairyOther),
				mPath = "Fairys_8"
			}
		},
		{
			9,
			new FairyData
			{
				mID = 9,
				mType = typeof(FairyOther),
				mPath = "Fairys_9"
			}
		},
		{
			10,
			new FairyData
			{
				mID = 10,
				mType = typeof(FairyOther),
				mPath = "Fairys_9"
			}
		},
		{
			11,
			new FairyData
			{
				mID = 11,
				mType = typeof(FairyOther),
				mPath = "Fairys_9"
			}
		},
		{
			12,
			new FairyData
			{
				mID = 12,
				mType = typeof(FairyOther),
				mPath = "Fairys_9"
			}
		},
		{
			13,
			new FairyData
			{
				mID = 13,
				mType = typeof(FairyOther),
				mPath = "Fairys_9"
			}
		}
	};

	public static BaseFairy CreateFairy(int id)
	{
		BaseFairy baseFairy = null;
		FairyData value = null;
		if (mDatas.TryGetValue(id, out value) && value != null)
		{
			string fairyPrefabAsset = AssetUtility.GetFairyPrefabAsset(value.mPath);
			GameObject gameObject = LevelResources.theResource.GetLevelResource(fairyPrefabAsset) as GameObject;
			if (gameObject != null)
			{
				baseFairy = Object.Instantiate(gameObject).AddComponent(value.mType) as BaseFairy;
				baseFairy.mData = value;
			}
		}
		return baseFairy;
	}

	public static string LoadFairyPath(int id)
	{
		string result = string.Empty;
		FairyData value = null;
		if (mDatas.TryGetValue(id, out value) && value != null)
		{
			result = AssetUtility.GetFairyPrefabAsset(value.mPath);
		}
		return result;
	}
}
