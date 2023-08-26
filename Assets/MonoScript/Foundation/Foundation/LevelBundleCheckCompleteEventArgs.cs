namespace Foundation
{
	public sealed class LevelBundleCheckCompleteEventArgs : EventArgs<LevelBundleCheckCompleteEventArgs>
	{
		public int Level { get; private set; }

		public bool NeedUpdate { get; private set; }

		public int UpdateCount { get; private set; }

		public int UpdateTotalLength { get; private set; }

		public static LevelBundleCheckCompleteEventArgs Make(int level, bool needUpdate, int updateCount, int updateTotalLength)
		{
			LevelBundleCheckCompleteEventArgs levelBundleCheckCompleteEventArgs = Mod.Reference.Acquire<LevelBundleCheckCompleteEventArgs>();
			levelBundleCheckCompleteEventArgs.Level = level;
			levelBundleCheckCompleteEventArgs.NeedUpdate = needUpdate;
			levelBundleCheckCompleteEventArgs.UpdateCount = updateCount;
			levelBundleCheckCompleteEventArgs.UpdateTotalLength = updateTotalLength;
			return levelBundleCheckCompleteEventArgs;
		}

		protected override void OnRecycle()
		{
		}
	}
}
