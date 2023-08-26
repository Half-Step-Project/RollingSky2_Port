using UnityEngine;

namespace RS2
{
	public class FreeCollideData
	{
		public float ValidDistance;

		public Vector3 TriggerPos;

		public Transform GridTrans;

		public FreeCollideData(float validDistance, Vector3 triggerPos, Transform gridTrans)
		{
			ValidDistance = validDistance;
			TriggerPos = triggerPos;
			GridTrans = gridTrans;
		}
	}
}
