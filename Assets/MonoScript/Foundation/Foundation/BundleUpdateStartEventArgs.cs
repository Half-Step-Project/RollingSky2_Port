namespace Foundation
{
	public sealed class BundleUpdateStartEventArgs : EventArgs<BundleUpdateStartEventArgs>
	{
		public string Name { get; private set; }

		public string Path { get; private set; }

		public string Uri { get; private set; }

		public int ZipLength { get; private set; }

		public int RetryCount { get; private set; }

		public static BundleUpdateStartEventArgs Make(string name, string path, string uri, int zipLength, int retryCount)
		{
			BundleUpdateStartEventArgs bundleUpdateStartEventArgs = Mod.Reference.Acquire<BundleUpdateStartEventArgs>();
			bundleUpdateStartEventArgs.Name = name;
			bundleUpdateStartEventArgs.Path = path;
			bundleUpdateStartEventArgs.Uri = uri;
			bundleUpdateStartEventArgs.ZipLength = zipLength;
			bundleUpdateStartEventArgs.RetryCount = retryCount;
			return bundleUpdateStartEventArgs;
		}

		protected override void OnRecycle()
		{
		}
	}
}
