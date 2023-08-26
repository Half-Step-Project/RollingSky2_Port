namespace RS2
{
	public interface IIAP
	{
		void Initialize();

		void RefreshProductInfo();

		void PurchaseProduct(string productId);

		void RestoreProduct(string[] productId);

		void ReValidateOrder(string productId);

		void OnIAPCallback(string json);

		void RefreshProductInfoCallback(string json);
	}
}
