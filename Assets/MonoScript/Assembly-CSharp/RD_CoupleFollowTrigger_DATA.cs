using System;
using UnityEngine;

[Serializable]
public class RD_CoupleFollowTrigger_DATA
{
	public RD_ElementTransform_DATA transformData;

	public int state;

	public int cachedLength;

	public Vector3 cachedPos;

	public RD_ElementAnimator_DATA coupleAnimatorData;

	public bool ifShowCouple;
}
