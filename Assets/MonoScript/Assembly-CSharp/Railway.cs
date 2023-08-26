using System;
using System.Collections.Generic;
using Foundation;
using UnityEngine;

public sealed class Railway : MonoBehaviour, IOriginRebirth
{
	public enum RailwayState
	{
		Static,
		CutScene,
		Move
	}

	public enum RailwayMoveDir
	{
		Forward,
		Backward
	}

	public static readonly string NodeRoleStart = "RoleStartPoint";

	public static readonly string NodeRoleRestart = "RoleRestartPoint";

	public static readonly string NodeCoupleStart = "CoupleStartPoint";

	public static Railway theRailway;

	public RailwayState CurrentState;

	private RailwayMover MoveController;

	private System.Action currentUpdate;

	private Dictionary<int, System.Action> moveForwardList = new Dictionary<int, System.Action>();

	[HideInInspector]
	public Vector3 ForwardMovement = Vector3.zero;

	public float SpeedForward;

	private RailwayMoveDir moveDir;

	private float moveDirScaler;

	public Vector3 StartWorldOrigin;

	public Vector3 StartWorldRight;

	public Vector3 StartRailwayPos;

	public Vector3 StartRoleLocalPos;

	public Vector3 RestartRoleLocalPos;

	public Vector3 StartCoupleWorldPos;

	[HideInInspector]
	public Vector3 WorldOrigin;

	[HideInInspector]
	public Vector3 WorldRight;

	public bool IfMoveForward
	{
		get
		{
			return moveDir == RailwayMoveDir.Forward;
		}
	}

	public bool IfRebirthOnVehicle { get; private set; }

	public bool IsRecordOriginRebirth
	{
		get
		{
			return true;
		}
	}

	private void Awake()
	{
		theRailway = this;
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void OnDestroy()
	{
		if (theRailway != null)
		{
			theRailway = null;
		}
	}

	private void UpdateStatic()
	{
	}

	private void UpdateMove()
	{
		if (InputService.KeyDown(KeyCode.A))
		{
			ChangeMoveDirTo(IfMoveForward ? RailwayMoveDir.Backward : RailwayMoveDir.Forward);
			MoveController.ReverseData();
		}
		Vector3 nextPos = Vector3.zero;
		MoveController.MoveForward(base.transform.position, Time.deltaTime, ref nextPos);
		base.transform.position = nextPos;
	}

	public void Initialize()
	{
		moveForwardList.Clear();
		moveForwardList.Add(0, UpdateStatic);
		moveForwardList.Add(2, UpdateMove);
		ChangeStateTo(RailwayState.Static);
		SetWorldInfo(StartWorldOrigin, StartWorldRight);
		ForwardMovement = Vector3.zero;
		ChangeMoveDirTo(RailwayMoveDir.Forward);
		base.transform.eulerAngles = Vector3.zero;
		StartRailwayPos = base.transform.position;
		StartRoleLocalPos = base.transform.Find(NodeRoleStart).localPosition;
		RestartRoleLocalPos = base.transform.Find(NodeRoleRestart).localPosition;
		Transform transform = base.transform.Find(NodeCoupleStart);
		if ((bool)transform)
		{
			StartCoupleWorldPos = transform.position;
		}
		MoveData moveData = new MoveData(SpeedForward, SpeedForward, StartRailwayPos, StartRailwayPos, base.transform.forward);
		if (MoveController == null)
		{
			MoveController = new RailwayMover(moveData, RailwayMover.MoveDir.Forward);
		}
		else
		{
			MoveController.ResetMoveData(moveData, RailwayMover.MoveDir.Forward);
		}
		IfRebirthOnVehicle = false;
	}

	public void DevInitialize()
	{
		ForwardMovement = Vector3.zero;
		ChangeMoveDirTo(RailwayMoveDir.Forward);
		base.transform.eulerAngles = Vector3.zero;
		StartRailwayPos = base.transform.position;
		StartRoleLocalPos = base.transform.Find(NodeRoleStart).localPosition;
		RestartRoleLocalPos = base.transform.Find(NodeRoleRestart).localPosition;
		Transform transform = base.transform.Find(NodeCoupleStart);
		if ((bool)transform)
		{
			StartCoupleWorldPos = transform.position;
		}
	}

	public void ResetRailway()
	{
		ChangeStateTo(RailwayState.Static);
		SetWorldInfo(StartWorldOrigin, StartWorldRight);
		ForwardMovement = Vector3.zero;
		ChangeMoveDirTo(RailwayMoveDir.Forward);
		base.transform.eulerAngles = Vector3.zero;
		base.transform.position = StartRailwayPos;
		MoveData moveData = new MoveData(SpeedForward, SpeedForward, StartRailwayPos, StartRailwayPos, base.transform.forward);
		MoveController.ResetMoveData(moveData, RailwayMover.MoveDir.Forward);
		IfRebirthOnVehicle = false;
	}

	public void UpdateRailway()
	{
		currentUpdate();
	}

	public void ResetBySavePointInfo(RebirthBoxData savePoint)
	{
		ResetRailway();
		base.transform.position = savePoint.RailwayPos;
		IfRebirthOnVehicle = savePoint.m_rebirthVehicleData != null && savePoint.m_rebirthVehicleData.m_vehicleGridId != -1;
	}

	public void StartRailway()
	{
		ChangeStateTo(RailwayState.Move);
	}

	private void ChangeStateTo(RailwayState state)
	{
		if (CurrentState != state)
		{
			CurrentState = state;
			currentUpdate = moveForwardList[(int)CurrentState];
		}
	}

	public void SetWorldInfo(Vector3 worldOrigin, Vector3 worldRight)
	{
		WorldOrigin = worldOrigin;
		WorldRight = worldRight;
	}

	public Vector3 GetWorldOrigin()
	{
		return WorldOrigin;
	}

	public Vector3 GetWorldRight()
	{
		return WorldRight;
	}

	public float GetRightOffset(Vector3 pos)
	{
		return Vector3.Dot(pos - GetWorldOrigin(), GetWorldRight());
	}

	public Vector3 GetRoleStartPos()
	{
		return base.transform.Find(NodeRoleStart).position;
	}

	public RailwayMoveDir GetMoveDir()
	{
		return moveDir;
	}

	public bool ChangeMoveDirTo(RailwayMoveDir dir)
	{
		moveDirScaler = ((dir == RailwayMoveDir.Forward) ? 1f : (-1f));
		if (moveDir == dir)
		{
			return false;
		}
		moveDir = dir;
		MoveController.ReverseData();
		return true;
	}

	public void ResetMoveData(MoveData moveData, RailwayMover.MoveDir dir)
	{
		ChangeMoveDirTo((dir != 0) ? RailwayMoveDir.Backward : RailwayMoveDir.Forward);
		MoveController.ResetMoveData(moveData, dir);
	}

	public void ResetMoveData(MoveData moveData, RailwayMoveDir dir)
	{
		ChangeMoveDirTo(dir);
		MoveController.ResetMoveData(moveData, (dir != 0) ? RailwayMover.MoveDir.Backward : RailwayMover.MoveDir.Forward);
	}

	public object GetOriginRebirthData(object obj = null)
	{
		return JsonUtility.ToJson(new RD_Railway_DATA
		{
			IfRebirthOnVehicle = IfRebirthOnVehicle,
			CurrentState = CurrentState,
			moveDir = moveDir,
			moveDirScaler = moveDirScaler,
			railwayMoverData = MoveController.GetRailwayMoverData(),
			transData = base.transform.GetTransData()
		});
	}

	public void SetOriginRebirthData(object dataInfo)
	{
		RD_Railway_DATA rD_Railway_DATA = JsonUtility.FromJson<RD_Railway_DATA>(dataInfo as string);
		IfRebirthOnVehicle = rD_Railway_DATA.IfRebirthOnVehicle;
		ChangeStateTo(rD_Railway_DATA.CurrentState);
		moveDir = rD_Railway_DATA.moveDir;
		moveDirScaler = rD_Railway_DATA.moveDirScaler;
		MoveController.SetRailwayMoverData(rD_Railway_DATA.railwayMoverData);
		base.transform.SetTransData(rD_Railway_DATA.transData);
	}

	public void StartRunByOriginRebirthData(object dataInfo)
	{
	}

	public byte[] GetOriginRebirthBsonData(object obj = null)
	{
		return Bson.ToBson(new RD_Railway_DATA
		{
			IfRebirthOnVehicle = IfRebirthOnVehicle,
			CurrentState = CurrentState,
			moveDir = moveDir,
			moveDirScaler = moveDirScaler,
			railwayMoverData = MoveController.GetRailwayMoverData(),
			transData = base.transform.GetTransData()
		});
	}

	public void SetOriginRebirthBsonData(byte[] dataInfo)
	{
		RD_Railway_DATA rD_Railway_DATA = Bson.ToObject<RD_Railway_DATA>(dataInfo);
		IfRebirthOnVehicle = rD_Railway_DATA.IfRebirthOnVehicle;
		ChangeStateTo(rD_Railway_DATA.CurrentState);
		moveDir = rD_Railway_DATA.moveDir;
		moveDirScaler = rD_Railway_DATA.moveDirScaler;
		MoveController.SetRailwayMoverData(rD_Railway_DATA.railwayMoverData);
		base.transform.SetTransData(rD_Railway_DATA.transData);
	}

	public void StartRunByOriginRebirthBsonData(byte[] dataInfo)
	{
	}
}
