using System;

[Serializable]
internal class RD_AnimatorEnemy_DATA
{
	public RD_ElementAnimator_DATA m_animator;

	public int m_isCanDistance;

	public RD_ElementParticle_DATA[] particles;

	public BaseElement.CommonState commonState;
}
