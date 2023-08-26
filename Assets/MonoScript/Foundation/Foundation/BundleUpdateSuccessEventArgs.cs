namespace Foundation
{
	public sealed class BundleUpdateSuccessEventArgs : EventArgs<BundleUpdateSuccessEventArgs>
	{
		public string Name { get; private set; }

		public string Path { get; private set; }

		public string Uri { get; private set; }

		public int Length { get; private set; }

		public int ZipLength { get; private set; }

		public static BundleUpdateSuccessEventArgs Make(string name, string path, string uri, int length, int zipLength)
		{
			BundleUpdateSuccessEventArgs bundleUpdateSuccessEventArgs = Mod.Reference.Acquire<BundleUpdateSuccessEventArgs>();
			bundleUpdateSuccessEventArgs.Name = name;
			bundleUpdateSuccessEventArgs.Path = path;
			bundleUpdateSuccessEventArgs.Uri = uri;
			bundleUpdateSuccessEventArgs.Length = length;
			bundleUpdateSuccessEventArgs.ZipLength = zipLength;
			return bundleUpdateSuccessEventArgs;
		}

		protected override void OnRecycle()
		{
		}
	}
}
