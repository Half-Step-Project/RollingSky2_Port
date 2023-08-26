using System;
using UnityEngine;

[Serializable]
public class RoleBodyPart
{
	public Transform RoleRoot;

	public Transform RoleCenter;

	public Transform RoleHead;

	public Transform RoleLeftFoot;

	public Transform RoleRightFoot;

	public Transform RoleLeftHand;

	public Transform RoleRightHand;

	public Transform RoleWaist;

	public Transform RoleLeftShoulder;

	public Transform RoleRightShoulder;

	public RoleBodyPart()
	{
	}

	public RoleBodyPart(Transform root, Transform center, Transform head, Transform leftFoot, Transform rightFoot, Transform leftShoulder, Transform rightShoulder, Transform leftHand, Transform rightHand, Transform waist)
	{
		RoleRoot = root;
		RoleCenter = center;
		RoleHead = head;
		RoleLeftFoot = leftFoot;
		RoleRightFoot = rightFoot;
		RoleLeftHand = leftHand;
		RoleRightHand = rightHand;
		RoleWaist = waist;
		RoleLeftShoulder = leftShoulder;
		RoleRightShoulder = rightShoulder;
	}
}
