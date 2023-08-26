using System;

[Serializable]
internal class RD_PathToMoveByUnpredictableDiamondAwardTrigger_DATA
{
	public RD_ElementTransform_DATA m_modelObject;

	public RD_ElementTransform_DATA m_awardColliderObject;

	public RD_ElementTransform_DATA m_nextColliderObject;

	public RD_ElementAnim_DATA m_awardShowAnimation;

	public RD_ElementAnim_DATA m_awardHideAnimation;

	public RD_ElementTransform_DATA m_effectObject;

	public RD_ElementParticle_DATA[] m_particles;

	public int m_currentPointIndex;

	public int m_isTriggerNext;

	public RD_BezierMove_Data m_bezierMover;
}
