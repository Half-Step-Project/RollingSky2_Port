using Foundation;
using UnityEngine;

namespace RS2
{
	public sealed class GameCameraCutEventArgs : EventArgs<GameCameraCutEventArgs>
	{
		public bool IfOpenBlank { get; private set; }

		public Vector3 CollidePosition { get; private set; }

		public GameCameraCutEventArgs Initialize(bool ifOpenBlank, Vector3 collidePos)
		{
			IfOpenBlank = ifOpenBlank;
			CollidePosition = collidePos;
			return this;
		}

		protected override void OnRecycle()
		{
			IfOpenBlank = false;
			CollidePosition = Vector3.zero;
		}
	}
}
