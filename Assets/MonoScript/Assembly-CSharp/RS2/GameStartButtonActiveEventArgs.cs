using Foundation;

namespace RS2
{
	public sealed class GameStartButtonActiveEventArgs : EventArgs<GameStartButtonActiveEventArgs>
	{
		public bool mActive;

		public GameStartButtonActiveEventArgs Initialize(bool active)
		{
			mActive = active;
			return this;
		}

		protected override void OnRecycle()
		{
		}
	}
}
