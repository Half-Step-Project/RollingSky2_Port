using System;

namespace RS2
{
	[Serializable]
	internal class RD_CraneJumpTile_DATA
	{
		public RD_ElementTransform_DATA m_jumpObject;

		public RD_ElementTransform_DATA m_jumpModelObject;

		public RD_ElementParticle_DATA m_jumpEffect;

		public RD_ElementTransform_DATA m_colliderTriggerBoxObject;

		public string m_rebirthData;

		public byte[] m_rebirthBytesData;

		public RD_ElementTransform_DATA colider;
	}
}
