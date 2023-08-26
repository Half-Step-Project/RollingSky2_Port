using Foundation;
using UnityEngine;

namespace RS2
{
	public sealed class GameFailEventArgs : EventArgs<GameFailEventArgs>
	{
		public enum GameFailType
		{
			Drop,
			Crash,
			Special,
			TryLevel
		}

		public GameFailType FailType { get; private set; }

		public Vector3 DiePosition { get; private set; }

		public void Initialize(GameFailType failType, Vector3 diePos)
		{
			FailType = failType;
			DiePosition = diePos;
		}

		protected override void OnRecycle()
		{
			FailType = GameFailType.Drop;
			DiePosition = Vector3.zero;
		}
	}
}
