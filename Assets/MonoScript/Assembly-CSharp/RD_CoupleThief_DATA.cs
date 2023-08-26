using System;

[Serializable]
internal class RD_CoupleThief_DATA
{
	public RD_ElementTransform_DATA transformData;

	public bool IfHaveTreasureChest;

	public bool ifSlowPath;

	public bool treasureElementShow;

	public bool IfDizziness;

	public float dizzinessCounter;

	public float newRoundCounter;

	public RD_ElementAnimator_DATA coupleAnimator;

	public int pathTriggerUuid;

	public RD_BezierMove_Data currentBezierMoverData;
}
