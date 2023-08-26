using System;
using System.Collections.Generic;

[Serializable]
public class RoleDynamicBoneData
{
	public List<DynamicBone.DynamicBoneData> m_dynamicBoneDatas = new List<DynamicBone.DynamicBoneData>();

	public List<RoleStateData> m_dynamicBoneDataForState = new List<RoleStateData>();

	public void AddDynamicBoneData(DynamicBone.DynamicBoneData data)
	{
		m_dynamicBoneDatas.Add(data);
	}
}
