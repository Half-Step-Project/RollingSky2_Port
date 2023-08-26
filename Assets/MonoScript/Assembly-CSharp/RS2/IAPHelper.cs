using My.Core;

namespace RS2
{
	public sealed class IAPHelper : My.Core.Singleton<IAPHelper>
	{
		private IIAP iap;

		public IIAP IAP
		{
			get
			{
				return iap;
			}
		}

		public void Initialize()
		{
		}

		public void RefreshProductInfo()
		{
			if (iap != null)
			{
				iap.RefreshProductInfo();
			}
		}

		public void PurchaseProduct(string productId)
		{
			if (iap != null)
			{
				iap.PurchaseProduct(productId);
			}
		}

		public void RestoreProduct(string[] productIds)
		{
			if (iap != null)
			{
				iap.RestoreProduct(productIds);
			}
		}

		public void ReValidateOrder(string productId)
		{
			if (iap != null)
			{
				iap.ReValidateOrder(productId);
			}
		}
	}
}
