using System;

[Serializable]
internal class RD_JumpDistanceQTETile_DATA
{
	public RD_ElementTransform_DATA m_state1Transform;

	public RD_ElementTransform_DATA m_state2Transform;

	public RD_ElementTransform_DATA m_modelTransform;

	public RD_ElementTransform_DATA m_effectTransform;

	public RD_ElementParticle_DATA[] m_effect;

	public float m_jumpMat;
}
