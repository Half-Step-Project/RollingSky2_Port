using Foundation;

namespace RS2
{
	public sealed class BuyOutRebirthUnLockEventArgs : EventArgs<BuyOutRebirthUnLockEventArgs>
	{
		public int LevelId { get; private set; }

		public BuyOutRebirthUnLockEventArgs Initialize(int levelId)
		{
			LevelId = levelId;
			return this;
		}

		protected override void OnRecycle()
		{
		}
	}
}
