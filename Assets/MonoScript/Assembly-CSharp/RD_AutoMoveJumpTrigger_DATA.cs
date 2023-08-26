using System;

[Serializable]
internal class RD_AutoMoveJumpTrigger_DATA
{
	public RD_ElementTransform_DATA tileObject;

	public bool[] colliders;

	public AutoMoveJumpTrigger.AutoMoveState currentState;

	public int currentPartIndex;

	public int currentPointIndex;

	public int partCount;
}
