using System;
using UnityEngine;

namespace RS2
{
	[Serializable]
	public class ThemeCamera
	{
		public bool m_isCameraColor;

		public Color m_backGround;

		public ThemeCamera()
		{
			m_isCameraColor = false;
			m_backGround = Color.white;
		}
	}
}
