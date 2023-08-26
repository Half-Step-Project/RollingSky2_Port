using System;

[Serializable]
internal class RD_Railway_DATA
{
	public bool IfRebirthOnVehicle;

	public Railway.RailwayState CurrentState;

	public Railway.RailwayMoveDir moveDir;

	public float moveDirScaler;

	public RD_RailwayMover_DATA railwayMoverData;

	public RD_ElementTransform_DATA transData;
}
