using Foundation;

namespace RS2
{
	public sealed class IosPayEventArgs : EventArgs<IosPayEventArgs>
	{
		public string JsonInfo { get; private set; }

		public IosPayEventArgs Initialize(string jsonInfo)
		{
			JsonInfo = jsonInfo;
			return this;
		}

		protected override void OnRecycle()
		{
			JsonInfo = string.Empty;
		}
	}
}
