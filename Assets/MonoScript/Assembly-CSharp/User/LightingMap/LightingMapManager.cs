using System.Collections.Generic;
using UnityEngine;

namespace User.LightingMap
{
	public class LightingMapManager
	{
		public static LightingMapSubstanceGroupData GetSubstanceGroupDataByGameOject(GameObject targetObj, bool chindren = true)
		{
			LightingMapSubstanceGroupData lightingMapSubstanceGroupData = new LightingMapSubstanceGroupData();
			if (targetObj != null)
			{
				lightingMapSubstanceGroupData.m_lightingMapSubstanceDatas = new List<LightingMapSubstanceData>();
				if (chindren)
				{
					Renderer[] componentsInChildren = targetObj.GetComponentsInChildren<Renderer>();
					for (int i = 0; i < componentsInChildren.Length; i++)
					{
						if (componentsInChildren[i] != null)
						{
							LightingMapSubstanceData lightingMapSubstanceData = new LightingMapSubstanceData();
							lightingMapSubstanceData.m_lightmapIndex = componentsInChildren[i].lightmapIndex;
							lightingMapSubstanceData.m_lightmapScaleOffset = componentsInChildren[i].lightmapScaleOffset;
							lightingMapSubstanceData.m_realtimeLightmapIndex = componentsInChildren[i].realtimeLightmapIndex;
							lightingMapSubstanceData.m_realtimeLightmapScaleOffset = componentsInChildren[i].realtimeLightmapScaleOffset;
							lightingMapSubstanceData.m_lightProbeUsage = componentsInChildren[i].lightProbeUsage;
							lightingMapSubstanceData.m_reflectionProbeUsage = componentsInChildren[i].reflectionProbeUsage;
							lightingMapSubstanceGroupData.m_lightingMapSubstanceDatas.Add(lightingMapSubstanceData);
						}
					}
				}
				else
				{
					Renderer component = targetObj.GetComponent<Renderer>();
					if (component != null)
					{
						LightingMapSubstanceData lightingMapSubstanceData2 = new LightingMapSubstanceData();
						lightingMapSubstanceData2.m_lightmapIndex = component.lightmapIndex;
						lightingMapSubstanceData2.m_lightmapScaleOffset = component.lightmapScaleOffset;
						lightingMapSubstanceData2.m_realtimeLightmapIndex = component.realtimeLightmapIndex;
						lightingMapSubstanceData2.m_realtimeLightmapScaleOffset = component.realtimeLightmapScaleOffset;
						lightingMapSubstanceData2.m_lightProbeUsage = component.lightProbeUsage;
						lightingMapSubstanceData2.m_reflectionProbeUsage = component.reflectionProbeUsage;
						lightingMapSubstanceGroupData.m_lightingMapSubstanceDatas.Add(lightingMapSubstanceData2);
					}
				}
			}
			return lightingMapSubstanceGroupData;
		}

		public static void SetSubstanceGroupDataByGameOject(GameObject targetObj, LightingMapSubstanceGroupData groupData)
		{
			if (!(targetObj != null))
			{
				return;
			}
			Renderer[] componentsInChildren = targetObj.GetComponentsInChildren<Renderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i] != null && groupData.m_lightingMapSubstanceDatas.Count > i)
				{
					componentsInChildren[i].lightmapIndex = groupData.m_lightingMapSubstanceDatas[i].m_lightmapIndex;
					componentsInChildren[i].lightmapScaleOffset = groupData.m_lightingMapSubstanceDatas[i].m_lightmapScaleOffset;
					componentsInChildren[i].realtimeLightmapIndex = groupData.m_lightingMapSubstanceDatas[i].m_realtimeLightmapIndex;
					componentsInChildren[i].realtimeLightmapScaleOffset = groupData.m_lightingMapSubstanceDatas[i].m_realtimeLightmapScaleOffset;
					componentsInChildren[i].lightProbeUsage = groupData.m_lightingMapSubstanceDatas[i].m_lightProbeUsage;
					componentsInChildren[i].reflectionProbeUsage = groupData.m_lightingMapSubstanceDatas[i].m_reflectionProbeUsage;
				}
			}
		}

		public static void SetLightMapData(LightmapData lightMapData)
		{
			SetLightMapData(new LightmapData[1] { lightMapData });
		}

		public static void SetLightMapData(LightingData lightData)
		{
			if (lightData != null)
			{
				LightmapSettings.lightmapsMode = lightData.m_lightMapsMode;
				SetLightMapData(GetLightmapDatas(lightData));
			}
		}

		public static void SetLightMapData(LightmapData[] lightMapDatas)
		{
			bool flag = true;
			for (int i = 0; i < lightMapDatas.Length; i++)
			{
				if (lightMapDatas == null)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				LightmapSettings.lightmaps = lightMapDatas;
			}
			else
			{
				Debug.LogError("lightMapDatas 中含有空元素，烘培赋值失败！！！");
			}
		}

		public static LightmapData GetLightMapData(Texture2D lightmapFar, Texture2D lightmapNear)
		{
			return new LightmapData
			{
				lightmapColor = lightmapFar,
				lightmapDir = lightmapNear
			};
		}

		public static LightmapData[] GetLightmapDatas(LightingData data)
		{
			LightmapData[] array = new LightmapData[data.m_lightingTexturesData.Count];
			for (int i = 0; i < data.m_lightingTexturesData.Count; i++)
			{
				array[i] = GetLightMapData(data.m_lightingTexturesData[i].m_lightmapFar, data.m_lightingTexturesData[i].m_lightmapNear);
			}
			return array;
		}
	}
}
