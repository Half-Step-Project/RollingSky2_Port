using Foundation;

namespace RS2
{
	public sealed class BuySuccessEventArgs : EventArgs<BuySuccessEventArgs>
	{
		public int ShopItemId { get; private set; }

		public BuySuccessEventArgs Initialize(int shopItemId)
		{
			ShopItemId = shopItemId;
			return this;
		}

		protected override void OnRecycle()
		{
			ShopItemId = -1;
		}
	}
}
