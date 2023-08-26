using System;
using UnityEngine;

[Serializable]
internal class RD_RelativeDisplacementMotionTriggerBox_DATA
{
	public RD_ElementTransform_DATA m_modelTransform;

	public RD_ElementAnimator_DATA m_animator;

	public RelativeDisplacementMotionTriggerBox.ElementState m_state;

	public Vector3 m_railwayInverse;

	public RD_ElementParticle_DATA[] particles;
}
