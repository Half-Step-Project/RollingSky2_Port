using System;
using UnityEngine;

namespace RS2
{
	[Serializable]
	public class ThemeSkyBox
	{
		public bool m_isSkyBox;

		public Material m_SkyBoxMaterial;

		public bool m_BlendLerp;

		public Color m_skyColor;

		public Color m_EquatorColor;

		public Color m_GroundColor;

		public float m_ReflectionIntensity;

		public Cubemap m_Cubemap;

		public ThemeSkyBox()
		{
			m_isSkyBox = false;
			m_SkyBoxMaterial = null;
			m_skyColor = Color.white;
			m_EquatorColor = Color.white;
			m_GroundColor = Color.white;
			m_ReflectionIntensity = 0f;
			m_Cubemap = null;
		}
	}
}
