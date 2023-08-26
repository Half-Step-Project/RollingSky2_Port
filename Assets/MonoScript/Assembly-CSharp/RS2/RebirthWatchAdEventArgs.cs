using Foundation;

namespace RS2
{
	public sealed class RebirthWatchAdEventArgs : EventArgs<RebirthWatchAdEventArgs>
	{
		public object AdCallBackData { get; private set; }

		public RebirthWatchAdEventArgs Initialize(object adCallBackData)
		{
			AdCallBackData = adCallBackData;
			return this;
		}

		protected override void OnRecycle()
		{
			AdCallBackData = null;
		}
	}
}
