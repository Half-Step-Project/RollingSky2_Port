using System.Collections.Generic;
using UnityEngine;

namespace RS2
{
	public class WorldConfigureScriptTable : ScriptableObject
	{
		public WorldStartInfo m_worldStartInfo;

		public int m_defaultThemeIndex;

		public List<Theme> m_themes = new List<Theme>();
	}
}
