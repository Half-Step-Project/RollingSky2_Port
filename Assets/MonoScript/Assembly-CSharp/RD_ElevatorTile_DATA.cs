using System;
using UnityExpansion;

[Serializable]
internal class RD_ElevatorTile_DATA
{
	public bool IfMoveUp;

	public MyVector3 collidePos;

	public BaseElement.CommonState commonState;

	public RD_ElementTransform_DATA trans;
}
