using System;

namespace RS2
{
	[Serializable]
	public class Theme
	{
		public int m_index;

		public float m_timeTaken;

		public ThemeLight m_themeLight;

		public ThemeFog m_themeFog;

		public ThemeCamera m_themeCamera;

		public ThemeSkyBox m_themeSkyBox;

		public Theme()
		{
			m_index = 0;
			m_timeTaken = 2f;
			m_themeLight = new ThemeLight();
			m_themeFog = new ThemeFog();
			m_themeCamera = new ThemeCamera();
			m_themeSkyBox = new ThemeSkyBox();
		}
	}
}
