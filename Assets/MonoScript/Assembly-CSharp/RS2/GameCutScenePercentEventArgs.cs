using Foundation;

namespace RS2
{
	public sealed class GameCutScenePercentEventArgs : EventArgs<GameCutScenePercentEventArgs>
	{
		public float Percent { get; private set; }

		public GameCutScenePercentEventArgs Initialize(float percent)
		{
			Percent = percent;
			return this;
		}

		protected override void OnRecycle()
		{
			Percent = 0f;
		}
	}
}
