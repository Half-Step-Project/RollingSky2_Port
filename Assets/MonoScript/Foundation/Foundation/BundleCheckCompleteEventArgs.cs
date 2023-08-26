namespace Foundation
{
	public sealed class BundleCheckCompleteEventArgs : EventArgs<BundleCheckCompleteEventArgs>
	{
		public int RemovedCount { get; private set; }

		public int UpdateCount { get; private set; }

		public int UpdateTotalLength { get; private set; }

		public static BundleCheckCompleteEventArgs Make(int removedCount, int updateCount, int updateTotalLength)
		{
			BundleCheckCompleteEventArgs bundleCheckCompleteEventArgs = Mod.Reference.Acquire<BundleCheckCompleteEventArgs>();
			bundleCheckCompleteEventArgs.RemovedCount = removedCount;
			bundleCheckCompleteEventArgs.UpdateCount = updateCount;
			bundleCheckCompleteEventArgs.UpdateTotalLength = updateTotalLength;
			return bundleCheckCompleteEventArgs;
		}

		protected override void OnRecycle()
		{
		}
	}
}
