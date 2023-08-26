using UnityEngine;

namespace RS2
{
	public class FreeMoveData
	{
		public float ValidDistance;

		public Vector3 TriggerPos;

		public Transform GridTrans;

		public bool IfTriggerControl;

		public float BeginDistance;

		public float SpeedScaler;

		public float MoveScaler;

		public FreeMoveData(float validDistance, Vector3 triggerPos, Transform gridTrans, bool ifTriggerControl = false, float beginDistance = 0f, float speedScaler = 0f, float moveScaler = 0f)
		{
			ValidDistance = validDistance;
			TriggerPos = triggerPos;
			GridTrans = gridTrans;
			IfTriggerControl = ifTriggerControl;
			BeginDistance = beginDistance;
			SpeedScaler = speedScaler;
			MoveScaler = moveScaler;
		}
	}
}
