using Foundation;

namespace RS2
{
	public class PayRefreshProductInfoEventArgs : EventArgs<PayRefreshProductInfoEventArgs>
	{
		public string JsonInfo { get; private set; }

		public PayRefreshProductInfoEventArgs Initialize(string jsonInfo)
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
