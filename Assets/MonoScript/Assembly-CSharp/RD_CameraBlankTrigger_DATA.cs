using System;
using RS2;
using UnityExpansion;

[Serializable]
internal class RD_CameraBlankTrigger_DATA
{
	public CameraBlankTrigger.TriggerState currentState;

	public RD_ElementTransform_DATA modelPart;

	public MyVector3 localCollidePos;
}
