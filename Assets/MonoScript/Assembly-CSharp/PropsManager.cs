using System.Collections.Generic;
using RS2;
using UnityEngine;

public sealed class PropsManager
{
	public static Dictionary<PropsName, PropData> m_propDatas = new Dictionary<PropsName, PropData>
	{
		{
			PropsName.SHIELD,
			new PropData(PropsName.SHIELD, "PropShield", typeof(PropShield))
		},
		{
			PropsName.REBIRTH,
			new PropData(PropsName.REBIRTH, "PropRebirth", typeof(PropRebirth))
		},
		{
			PropsName.PATHGUIDE,
			new PropData(PropsName.PATHGUIDE, "PropPathGuide", typeof(PropPathGuide))
		}
	};

	public static BaseProp CreateBaseProp(PropsName propsName)
	{
		return CreateBaseProp(GetPropData(propsName));
	}

	public static BaseProp CreateBaseProp(PropData propData)
	{
		BaseProp baseProp = null;
		if (propData != null)
		{
			string propPrefabAsset = AssetUtility.GetPropPrefabAsset(propData.m_path);
			GameObject gameObject = LevelResources.theResource.GetLevelResource(propPrefabAsset) as GameObject;
			if (gameObject != null)
			{
				baseProp = Object.Instantiate(gameObject).AddComponent(propData.m_classType) as BaseProp;
				baseProp.m_propData = propData;
			}
		}
		return baseProp;
	}

	public static PropData GetPropData(PropsName propsName)
	{
		PropData value = null;
		m_propDatas.TryGetValue(propsName, out value);
		return value;
	}
}
