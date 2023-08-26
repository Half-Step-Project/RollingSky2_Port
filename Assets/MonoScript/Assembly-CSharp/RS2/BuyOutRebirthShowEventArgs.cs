using Foundation;

namespace RS2
{
	public sealed class BuyOutRebirthShowEventArgs : EventArgs<BuyOutRebirthShowEventArgs>
	{
		public bool IsShow { get; private set; }

		public BuyOutRebirthShowEventArgs Initialize(bool isShow)
		{
			IsShow = isShow;
			return this;
		}

		protected override void OnRecycle()
		{
		}
	}
}
