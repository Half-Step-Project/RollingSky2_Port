namespace Foundation
{
	public sealed class BundleUpdateChangedEventArgs : EventArgs<BundleUpdateChangedEventArgs>
	{
		public string Name { get; private set; }

		public string Path { get; private set; }

		public string Uri { get; private set; }

		public int Length { get; private set; }

		public int ZipLength { get; private set; }

		public static BundleUpdateChangedEventArgs Make(string name, string path, string uri, int length, int zipLength)
		{
			BundleUpdateChangedEventArgs bundleUpdateChangedEventArgs = Mod.Reference.Acquire<BundleUpdateChangedEventArgs>();
			bundleUpdateChangedEventArgs.Name = name;
			bundleUpdateChangedEventArgs.Path = path;
			bundleUpdateChangedEventArgs.Uri = uri;
			bundleUpdateChangedEventArgs.Length = length;
			bundleUpdateChangedEventArgs.ZipLength = zipLength;
			return bundleUpdateChangedEventArgs;
		}

		protected override void OnRecycle()
		{
		}
	}
}
