using Foundation;

namespace RS2
{
	public sealed class LevelUnLockEventArgs : EventArgs<LevelUnLockEventArgs>
	{
		public int LevelId { get; private set; }

		public LevelUnLockEventArgs Initialize(int levelId)
		{
			LevelId = levelId;
			return this;
		}

		protected override void OnRecycle()
		{
			LevelId = 0;
		}
	}
}
