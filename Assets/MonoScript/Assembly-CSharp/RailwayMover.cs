using UnityEngine;

public class RailwayMover
{
	public enum MoveState
	{
		Normal,
		SpeedUp,
		SpeedDown,
		StateLength
	}

	public enum MoveDir
	{
		Forward,
		Backward
	}

	public MoveState CurrentState;

	public MoveDir CurrentDir;

	public MoveData currentMoveInfo;

	public RailwayMover(MoveData moveData, MoveDir moveDir)
	{
		currentMoveInfo = moveData;
		CurrentDir = moveDir;
	}

	public void ResetMoveData(MoveData moveData, MoveDir moveDir)
	{
		currentMoveInfo = moveData;
		CurrentDir = moveDir;
	}

	public void ReverseData()
	{
		CurrentDir = ((CurrentDir == MoveDir.Forward) ? MoveDir.Backward : MoveDir.Forward);
		currentMoveInfo.ReverseDir();
	}

	public MoveState MoveForward(Vector3 curPos, float deltaTime, ref Vector3 nextPos)
	{
		if (currentMoveInfo.GetNextPos(curPos, deltaTime, ref nextPos))
		{
			if (!(currentMoveInfo.Acceleration > 0f))
			{
				return MoveState.SpeedDown;
			}
			return MoveState.SpeedUp;
		}
		return MoveState.Normal;
	}
}
