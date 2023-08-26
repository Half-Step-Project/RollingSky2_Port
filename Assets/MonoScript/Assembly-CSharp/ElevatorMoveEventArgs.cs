using Foundation;
using UnityEngine;
using User.TileMap;

public sealed class ElevatorMoveEventArgs : EventArgs<ElevatorMoveEventArgs>
{
	public const int InValidId = -1;

	public float ValidDistance { get; private set; }

	public Point TriggerPoint { get; private set; }

	public Transform GridTrans { get; private set; }

	public bool IfUp { get; private set; }

	public Vector3 BeginPos { get; private set; }

	public int ValidIndex { get; private set; }

	public void Initialize(float validDis, Point triggerPos, Transform gridTrans, bool ifUp, Vector3 beginPos, int validIndex = -1)
	{
		ValidDistance = validDis;
		TriggerPoint = triggerPos;
		GridTrans = gridTrans;
		IfUp = ifUp;
		BeginPos = beginPos;
		ValidIndex = validIndex;
	}

	protected override void OnRecycle()
	{
		ValidIndex = -1;
	}
}
