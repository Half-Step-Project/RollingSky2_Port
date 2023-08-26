namespace Foundation
{
	public sealed class VersionListUpdateFailureEventArgs : EventArgs<VersionListUpdateFailureEventArgs>
	{
		public string Uri { get; private set; }

		public string Message { get; private set; }

		public static VersionListUpdateFailureEventArgs Make(string uri, string message)
		{
			VersionListUpdateFailureEventArgs versionListUpdateFailureEventArgs = Mod.Reference.Acquire<VersionListUpdateFailureEventArgs>();
			versionListUpdateFailureEventArgs.Uri = uri;
			versionListUpdateFailureEventArgs.Message = message;
			return versionListUpdateFailureEventArgs;
		}

		protected override void OnRecycle()
		{
		}
	}
}
