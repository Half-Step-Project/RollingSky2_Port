using UnityEngine;

public class RoleProperty : ScriptableObject
{
	public int RoleIndex;

	public string RoleDes;

	public float SpeedForward;

	public float RotationSpeed;

	public float SlideSpeed;

	public float TrailFadeSpeed;

	public RuntimeAnimatorController[] m_runtimeAnimatorControllers;

	public RoleDynamicBoneData m_roleDynamicBoneData = new RoleDynamicBoneData();
}
