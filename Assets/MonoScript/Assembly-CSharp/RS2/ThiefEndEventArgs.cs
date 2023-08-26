using Foundation;

namespace RS2
{
	public sealed class ThiefEndEventArgs : EventArgs<ThiefEndEventArgs>
	{
		public static readonly int eObjRole = 0;

		public static readonly int eObjCouple = 1;

		public static readonly int eObjChest = 2;

		public static readonly int eObjCamera = 3;

		public int ObjType { get; private set; }

		public float CoupleWaitTime { get; private set; }

		public float RoleWaitTime { get; private set; }

		public float ChestWaitTime { get; private set; }

		public float CameraWaitTime { get; private set; }

		public ThiefEndEventArgs Initialize(int objType, float roleWaitTime, float coupleWaitTime, float chestWaitTime, float cameraWaitTime)
		{
			ObjType = objType;
			RoleWaitTime = roleWaitTime;
			CoupleWaitTime = coupleWaitTime;
			ChestWaitTime = chestWaitTime;
			CameraWaitTime = cameraWaitTime;
			return this;
		}

		protected override void OnRecycle()
		{
			RoleWaitTime = 0f;
			CoupleWaitTime = 0f;
			ChestWaitTime = 0f;
			CameraWaitTime = 0f;
		}
	}
}
