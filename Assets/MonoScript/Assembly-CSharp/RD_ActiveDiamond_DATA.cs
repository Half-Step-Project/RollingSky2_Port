using System;

[Serializable]
internal class RD_ActiveDiamond_DATA
{
	public RD_ElementTransform_DATA modle;

	public RD_ElementTransform_DATA triggerPoint;

	public RD_ElementTransform_DATA cacheParent;

	public int startMove;

	public int isFinished;

	public RD_ElementParticle_DATA particle;

	public RD_BezierMove_Data m_bezierMover;
}
