using System;

namespace RS2
{
	[Serializable]
	public class RD_CraneTile_DATA
	{
		public CraneTile.State m_state;

		public float m_currentTime;

		public RD_ElementTransform_DATA m_baseCenterObject;

		public RD_ElementTransform_DATA m_ropeObject;

		public RD_ElementTransform_DATA m_boardObject;

		public RD_ElementTransform_DATA m_colliderTriggerBoxObject;

		public RD_ElementAnim_DATA m_triggerAnimation;

		public int m_isFinished;

		public int m_isTriggerControllerA;

		public RD_ElementParticle_DATA m_A;

		public RD_ElementParticle_DATA m_B;
	}
}
