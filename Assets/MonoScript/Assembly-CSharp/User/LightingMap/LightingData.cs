using System;
using System.Collections.Generic;
using UnityEngine;

namespace User.LightingMap
{
	[Serializable]
	public class LightingData : ScriptableObject
	{
		public LightmapsMode m_lightMapsMode;

		public List<LightingTexturesData> m_lightingTexturesData = new List<LightingTexturesData>();
	}
}
