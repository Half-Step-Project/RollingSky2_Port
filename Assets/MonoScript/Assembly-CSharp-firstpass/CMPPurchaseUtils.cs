using System;
using UnityEngine;

public class CMPPurchaseUtils : MonoBehaviour
{
	public struct CMPPurchaseData
	{
		public string url;

		public string userID;

		public string platformType;

		public string productIdJsonArray;

		public string gameID;

		public string token;
	}

	public static CMPPurchaseUtils Instance;

	public CMPPurchaseData purchaseData;

	public static event Action<string> productListReceivedEvent;

	public static event Action<string> purchaseCompleteEvent;

	public static event Action<string> restoreCompleteEvent;

	public static event Action<string> requestNonConsumableProductStatusCompleteEvent;

	public static void Initialize()
	{
		if (Instance == null)
		{
			GameObject gameObject = GameObject.Find("PurchaseUtil");
			if (gameObject == null)
			{
				gameObject = new GameObject("PurchaseUtil");
			}
			if (gameObject != null)
			{
				gameObject.AddComponent<CMPPurchaseUtils>();
			}
		}
	}

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}

	private void Init()
	{
		purchaseData = default(CMPPurchaseData);
		purchaseData.url = "";
		purchaseData.userID = "";
		purchaseData.productIdJsonArray = "";
		purchaseData.platformType = "0";
		purchaseData.gameID = "";
		purchaseData.token = "";
		setDebug(false);
		initPurchase(purchaseData.url, purchaseData.userID, purchaseData.platformType, purchaseData.productIdJsonArray, purchaseData.gameID, purchaseData.token);
	}

	public void setDebug(bool value)
	{
	}

	public void initPurchase(string url, string userID, string platformType, string productIdJsonArray, string gameID, string token)
	{
	}

	public void setNonConsumableProducts(string noconsume_productIdJsonArray)
	{
	}

	public void requestProducts(string productIdJsonArray)
	{
	}

	public void buyProduct(string userID, string platformType, string productID)
	{
	}

	public void remedyTransaction()
	{
	}

	public void restoreCompletedTransactions(string strJson)
	{
	}

	public void requestNonConsumableProductStatusWithProductIDs(string strJson)
	{
	}

	public void onPayCallBack(string strJson)
	{
		CMPPurchaseUtils.purchaseCompleteEvent(strJson);
	}

	public void onGetProductInfos(string strJson)
	{
		CMPPurchaseUtils.productListReceivedEvent(strJson);
	}

	public void onRestoreProductsWithResult(string strJson)
	{
		CMPPurchaseUtils.restoreCompleteEvent(strJson);
	}

	public void onRequestNonConsumableProductStatusWithResult(string strJson)
	{
		CMPPurchaseUtils.requestNonConsumableProductStatusCompleteEvent(strJson);
	}
}
