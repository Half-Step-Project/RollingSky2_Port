using Foundation;

namespace RS2
{
	public sealed class GamePlaySuperMusicEventArgs : EventArgs<GamePlaySuperMusicEventArgs>
	{
		public GamePlaySuperMusicEventArgs Initialize()
		{
			return this;
		}

		protected override void OnRecycle()
		{
		}
	}
}
