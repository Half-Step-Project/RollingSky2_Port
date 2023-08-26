using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace User.LightingMap
{
	[Serializable]
	public class LightingMapSubstanceData
	{
		public int m_lightmapIndex;

		public Vector4 m_lightmapScaleOffset;

		public int m_realtimeLightmapIndex;

		public Vector4 m_realtimeLightmapScaleOffset;

		public LightProbeUsage m_lightProbeUsage;

		public ReflectionProbeUsage m_reflectionProbeUsage;
	}
}
