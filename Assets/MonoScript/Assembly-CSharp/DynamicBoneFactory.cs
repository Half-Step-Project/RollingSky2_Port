using System.Collections.Generic;
using Foundation;
using UnityEngine;

public class DynamicBoneFactory : IElementRebirth
{
	public enum DynamicBoneType
	{
		Initialize,
		StartBall,
		Cutscene,
		OpenRightWind,
		OpenLeftWind,
		CloseWind,
		ResetBall,
		RebirthResetBall,
		ChangeToWin
	}

	public BaseRole m_baseRole;

	public RoleDynamicBoneData m_roleDynamicBoneData;

	protected Dictionary<string, DynamicBone> m_dynamicBones = new Dictionary<string, DynamicBone>();

	public DynamicBoneType CurrentDynamicBoneType { get; private set; }

	public bool IfRebirthRecord
	{
		get
		{
			return false;
		}
	}

	public DynamicBoneFactory(BaseRole baseRole, RoleDynamicBoneData data)
	{
		m_baseRole = baseRole;
		m_roleDynamicBoneData = data;
	}

	public void OnInitialize()
	{
		DynamicBone[] componentsInChildren = m_baseRole.gameObject.GetComponentsInChildren<DynamicBone>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].m_Root != null)
			{
				DynamicBone value = null;
				if (!m_dynamicBones.TryGetValue(componentsInChildren[i].m_Root.gameObject.name, out value))
				{
					m_dynamicBones[componentsInChildren[i].m_Root.gameObject.name] = componentsInChildren[i];
				}
			}
		}
	}

	public void SwitchDynamicBoneType(DynamicBoneType dynamicBoneType)
	{
		if (m_roleDynamicBoneData == null || !(m_baseRole != null))
		{
			return;
		}
		for (int i = 0; i < m_roleDynamicBoneData.m_dynamicBoneDataForState.Count; i++)
		{
			if (m_roleDynamicBoneData.m_dynamicBoneDataForState[i].m_dynamicBoneType != dynamicBoneType)
			{
				continue;
			}
			for (int j = 0; j < m_roleDynamicBoneData.m_dynamicBoneDataForState[i].m_indexs.Count; j++)
			{
				int num = m_roleDynamicBoneData.m_dynamicBoneDataForState[i].m_indexs[j];
				if (m_roleDynamicBoneData.m_dynamicBoneDatas.Count <= num)
				{
					continue;
				}
				DynamicBone.DynamicBoneData dynamicBoneData = m_roleDynamicBoneData.m_dynamicBoneDatas[num];
				DynamicBone value = null;
				if (m_dynamicBones.TryGetValue(dynamicBoneData.m_RootName, out value) && value != null)
				{
					CurrentDynamicBoneType = dynamicBoneType;
					if (dynamicBoneType == DynamicBoneType.RebirthResetBall || dynamicBoneType == DynamicBoneType.ResetBall || dynamicBoneType == DynamicBoneType.Initialize)
					{
						value.SetDynamicBoneData(dynamicBoneData, true);
					}
					else
					{
						value.SetDynamicBoneData(dynamicBoneData);
					}
				}
			}
			break;
		}
	}

	public void DestroyLocal()
	{
	}

	public void RebirthReadData(object rd_data)
	{
	}

	public object RebirthWriteData()
	{
		return JsonUtility.ToJson(new RD_DynamicBoneFactory_DATA());
	}

	public void RebirthStartGame(object rd_data)
	{
	}

	public void RebirthReadByteData(byte[] rd_data)
	{
	}

	public byte[] RebirthWriteByteData()
	{
		return Bson.ToBson(new RD_DynamicBoneFactory_DATA());
	}

	public void RebirthStartByteGame(byte[] rd_data)
	{
	}
}
