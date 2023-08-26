using UnityEngine;
using User.TileMap;

namespace RS2
{
	public class MagicBoxData
	{
		public float ValidDistance;

		public Point TriggerPoint;

		public Transform GridTrans;

		public float RotateAngle;

		public float RotateSpeed;

		public MagicBoxData(float validDistance, Point triggerPoint, Transform gridTrans, float rotateAngle, float rotateSpeed)
		{
			ValidDistance = validDistance;
			TriggerPoint = triggerPoint;
			GridTrans = gridTrans;
			RotateAngle = rotateAngle;
			RotateSpeed = rotateSpeed;
		}
	}
}
