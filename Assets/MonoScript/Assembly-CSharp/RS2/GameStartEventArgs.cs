using Foundation;

namespace RS2
{
	public sealed class GameStartEventArgs : EventArgs<GameStartEventArgs>
	{
		public enum GameStartType
		{
			Normal,
			ForceRun
		}

		public GameStartType StartType { get; private set; }

		public void Initialize(GameStartType startType)
		{
			StartType = startType;
		}

		protected override void OnRecycle()
		{
			StartType = GameStartType.Normal;
		}
	}
}
