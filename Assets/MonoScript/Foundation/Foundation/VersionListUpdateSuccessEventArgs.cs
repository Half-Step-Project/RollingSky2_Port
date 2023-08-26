namespace Foundation
{
	public sealed class VersionListUpdateSuccessEventArgs : EventArgs<VersionListUpdateSuccessEventArgs>
	{
		public string Path { get; private set; }

		public string Uri { get; private set; }

		public static VersionListUpdateSuccessEventArgs Make(string path, string uri)
		{
			VersionListUpdateSuccessEventArgs versionListUpdateSuccessEventArgs = Mod.Reference.Acquire<VersionListUpdateSuccessEventArgs>();
			versionListUpdateSuccessEventArgs.Path = path;
			versionListUpdateSuccessEventArgs.Uri = uri;
			return versionListUpdateSuccessEventArgs;
		}

		protected override void OnRecycle()
		{
		}
	}
}
