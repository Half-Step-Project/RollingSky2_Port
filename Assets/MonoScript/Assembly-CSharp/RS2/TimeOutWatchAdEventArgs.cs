using Foundation;

namespace RS2
{
	public sealed class TimeOutWatchAdEventArgs : EventArgs<TimeOutWatchAdEventArgs>
	{
		public object AdCallBackData { get; private set; }

		public void Initialize(object adCallBackData)
		{
			AdCallBackData = adCallBackData;
		}

		protected override void OnRecycle()
		{
			AdCallBackData = null;
		}
	}
}
