using System;
using UnityEngine;

namespace RS2
{
	[Serializable]
	public class ThemeFog
	{
		public bool m_isFog;

		public Color m_fogColor;

		public float m_fogStartDistance;

		public float m_fogEndDistance;

		public ThemeFog()
		{
			m_isFog = false;
			m_fogColor = Color.white;
			m_fogStartDistance = 0f;
			m_fogEndDistance = 0f;
		}
	}
}
