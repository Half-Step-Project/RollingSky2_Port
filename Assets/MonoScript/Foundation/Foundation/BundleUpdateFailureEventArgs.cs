namespace Foundation
{
	public sealed class BundleUpdateFailureEventArgs : EventArgs<BundleUpdateFailureEventArgs>
	{
		public string Name { get; private set; }

		public string Uri { get; private set; }

		public int RetryCount { get; private set; }

		public int TotalRetryCount { get; private set; }

		public string Message { get; private set; }

		public static BundleUpdateFailureEventArgs Make(string name, string uri, int retryCount, int totalRetryCount, string message)
		{
			BundleUpdateFailureEventArgs bundleUpdateFailureEventArgs = Mod.Reference.Acquire<BundleUpdateFailureEventArgs>();
			bundleUpdateFailureEventArgs.Name = name;
			bundleUpdateFailureEventArgs.Uri = uri;
			bundleUpdateFailureEventArgs.RetryCount = retryCount;
			bundleUpdateFailureEventArgs.TotalRetryCount = totalRetryCount;
			bundleUpdateFailureEventArgs.Message = message;
			return bundleUpdateFailureEventArgs;
		}

		protected override void OnRecycle()
		{
		}
	}
}
