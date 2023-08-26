using Foundation;

namespace RS2
{
	public sealed class GameOverEventArgs : EventArgs<GameOverEventArgs>
	{
		public GameOverEventArgs Initialize()
		{
			return this;
		}

		protected override void OnRecycle()
		{
		}
	}
}
