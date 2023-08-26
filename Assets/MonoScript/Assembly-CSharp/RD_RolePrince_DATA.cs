using System;
using RS2;
using UnityEngine;

[Serializable]
internal class RD_RolePrince_DATA
{
	public bool IfWinBeforeFinish;

	public bool IfRebirthOnVehicle;

	public float RoleMoveLimit;

	public BaseRole.BallState CurrentState;

	public bool ifDropDieStatic;

	public RD_ElementTransform_DATA roleRootTransData;

	public RD_ElementTransform_DATA roleModelTransData;

	public RD_ElementTransform_DATA roleRoleTransData;

	public BaseRole.AnimType m_currentAnimType;

	public RD_ElementAnim_DATA roleAnimData;

	public int m_runtimeAnimatorControllerIndex;

	public int m_roleSkinIndex;

	public RD_JumpInfo_Data jumpInfo;

	public BaseRole.JumpType jumpType;

	public bool ifDroping;

	public float dropBeginY;

	public Vector3 dropBeginPos;

	public Vector3 dropEndPos;

	public Vector3 dropDir;

	public float dropDistance;

	public float dropHeightY;

	public float dropDeltaY;

	public bool ifHavePathTrigger;

	public int pathTriggerIndex;

	public int pathPointIndex;

	public bool ifHavePathFixedTrigger;

	public int pathFixedTriggerIndex;

	public int pathFixedPointIndex;

	public bool ifHavePathLerpTrigger;

	public int pathLerpTriggerIndex;

	public int pathLerpPointIndex;

	public Vector3 rolePosForLerp;

	public int LastTileId = -1;

	public int CurrentTileId = -1;

	public int currentGrid = -1;

	public bool IfHaveTreasureChest;

	public bool treasureElementShow;

	public bool IfShowfaceCover;

	public RD_ElementAnim_DATA faceCoverAnimData;

	public int currentMoodType;

	public int dancerUuid = -1;

	public bool ifDanceTogether;

	public bool m_isHaveWind;

	public WindOpenTrigger.WindData m_windData;

	public SwingingRipeData m_swingingRipeData;

	public RD_Vehicle_DATA CurrentVehicle;

	public float SlideSpeed;

	public int CurrentWorldRow;

	public int LastRow;
}
