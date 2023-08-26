using System;
using System.Collections.Generic;

namespace RS2
{
	[Serializable]
	public struct WorldStartInfo
	{
		public int m_roleIndex;

		public float m_StartAnimTime;

		public float m_EndAnimTime;

		public int m_BackgroundIndex;

		public List<int> m_preloadRoleIndexList;

		public int CoupleRoleIndex;

		public bool m_ifLerpEmission;

		public float m_delayEmissionTime;

		public float m_lerpEmissionTime;

		public float m_beginEmissionValue;

		public float m_endEmissionValue;

		public int m_levelDeltaLength;

		public int m_petID;

		public int m_showRow;

		public float m_fairyStartRunDelay;
	}
}
