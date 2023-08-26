using System;
using UnityEngine;

namespace RS2
{
	[Serializable]
	public class ThemeLight
	{
		public bool m_isLight;

		public Color m_lightColor;

		public float m_intensity;

		public float m_shadowStrength;

		public Vector3 m_rotation;

		public bool m_ifLightOn;

		public ThemeLight()
		{
			m_isLight = false;
			m_lightColor = Color.white;
			m_intensity = 0f;
			m_shadowStrength = 0f;
			m_rotation = Vector3.zero;
			m_ifLightOn = false;
		}
	}
}
