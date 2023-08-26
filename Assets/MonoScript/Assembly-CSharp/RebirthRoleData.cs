using UnityEngine;

public class RebirthRoleData
{
	public Vector3 RoleLocalPos;

	public BaseRole.BallState m_roleState;

	public int m_currentPathToMoveIndex;

	public PathToMoveByRoleTrigger.PathToMoveByRoleTriggerData m_pathToMovebyRoleData;

	public PathToMoveFixedByRoleTrigger.PathToMoveFixedByRoleTriggerData m_pathToMoveFixedByRoleTriggerData;

	public BaseRole.AnimType m_roleAnimType;

	public bool m_trailActive;

	public int m_runtimeAnimatorControllerIndex;

	public int m_roleSkinIndex;

	public float m_roleMoveLimit;
}
