using System;
using System.Collections.Generic;
using Foundation;
using LitJson;
using UnityEngine;

namespace RS2
{
	public sealed class IAPForIOS : IIAP
	{
		private const string PURCHASE_URL = "https://gdcasapi.cmcm.com/globalpay/iosPay?protocolVer=";

		private const string GAME_ID = "1018";

		private Shops_shopTable[] allProduct;

		private string allProductStr;

		private Dictionary<string, string> purchaseEvent = new Dictionary<string, string>();

		public void Initialize()
		{
			int num = 0;
			try
			{
				allProduct = Mod.DataTable.Get<Shops_shopTable>().Records;
				num = 1;
				List<string> list = new List<string>();
				string text = "";
				num = 2;
				for (int i = 0; i < allProduct.Length; i++)
				{
					if (allProduct[i].BuyType == 1)
					{
						text = allProduct[i].Product_id_ios;
						list.Add(text);
					}
				}
				num = 3;
				IJsonHandler handler = Json.Handler;
				num = 4;
				Json.Handler = LitJsonHandler.Default;
				num = 5;
				allProductStr = Json.ToJson(list.ToArray());
				num = 6;
				Json.Handler = handler;
				num = 7;
				CMPPurchaseUtils.Instance.initPurchase("https://gdcasapi.cmcm.com/globalpay/iosPay?protocolVer=", "", "", allProductStr, "1018", "");
				num = 8;
				CMPPurchaseUtils.purchaseCompleteEvent += OnIAPCallback;
				CMPPurchaseUtils.restoreCompleteEvent += OnIAPRestoreCallback;
				num = 9;
				CMPPurchaseUtils.productListReceivedEvent += RefreshProductInfoCallback;
			}
			catch (Exception exception)
			{
				Log.Error(string.Format("IAPForIOS.Initialize error at {0}", num));
				Log.Exception(exception);
			}
		}

		public void RefreshProductInfo()
		{
			CMPPurchaseUtils.Instance.requestProducts(allProductStr);
		}

		public void PurchaseProduct(string productID)
		{
			Log.Info(string.Format("IAP PurchaseProduct : {0}", productID));
			CMPPurchaseUtils.Instance.buyProduct("", "", productID);
		}

		public void RestoreProduct(string[] productIDs)
		{
			string text = "";
			JsonData jsonData = new JsonData();
			jsonData.SetJsonType(JsonType.Object);
			JsonData jsonData2 = new JsonData();
			jsonData2.SetJsonType(JsonType.Array);
			JsonData jsonData3 = new JsonData();
			jsonData3.SetJsonType(JsonType.Array);
			jsonData["Auto-Renewable Subscriptions"] = jsonData2;
			for (int i = 0; i < productIDs.Length; i++)
			{
				jsonData3.Add(productIDs[i]);
			}
			jsonData["Non-Consumable"] = jsonData3;
			text = jsonData.ToJson();
			CMPPurchaseUtils.Instance.restoreCompletedTransactions(text);
			Log.Info(string.Format("IAP RestoreProduct : {0}", text));
		}

		public void ReValidateOrder(string productID)
		{
		}

		public void OnIAPCallback(string json)
		{
			Log.Info(string.Format("IOS OnIAPCallback:{0}", json));
			if (Mod.UI.UIFormIsOpen(UIFormId.ShopLoadingForm))
			{
				Mod.UI.CloseUIForm(UIFormId.ShopLoadingForm);
			}
			OnOperateIOSRepairOrder(json);
		}

		public void OnIAPRestoreCallback(string json)
		{
			Log.Info(string.Format("IOS OnIAPRestoreCallback:{0}", json));
			if (Mod.UI.UIFormIsOpen(UIFormId.ShopLoadingForm))
			{
				Mod.UI.CloseUIForm(UIFormId.ShopLoadingForm);
			}
			OnOperateIOSRestore(json);
		}

		public void RefreshProductInfoCallback(string json)
		{
			Log.Info(string.Format("IOS RefreshProductInfoCallback:{0}", json));
			if (!string.IsNullOrEmpty(json))
			{
				json = string.Format("{{\"productinInfoList\":{0}}}", json);
				EncodeConfig.setConfig(PlayerLocalDatakey.GAMESHOPPRICEDATA, json);
				MonoSingleton<PluginManager>.Instacne.RefreshProductionInfo();
				Mod.Event.FireNow(this, Mod.Reference.Acquire<PayRefreshProductInfoEventArgs>().Initialize(json));
			}
		}

		private void OnOperateIOSRepairOrder(string json)
		{
			int num = 0;
			try
			{
				if (string.IsNullOrEmpty(json))
				{
					return;
				}
				num = 1;
				PayCallBackData payCallBackData = JsonUtility.FromJson<PayCallBackData>(json);
				num = 2;
				if (payCallBackData == null)
				{
					return;
				}
				num = 3;
				Shops_shopTable shops_shopTable = null;
				Shops_shopTable[] records = Mod.DataTable.Get<Shops_shopTable>().Records;
				num = 4;
				for (int i = 0; i < records.Length; i++)
				{
					if (records[i].Product_id_ios.Equals(payCallBackData.productId))
					{
						shops_shopTable = records[i];
						break;
					}
				}
				num = 5;
				TipMoneyGoods(payCallBackData, shops_shopTable);
				num = 6;
				purchaseEvent.Clear();
				if (shops_shopTable == null)
				{
					InfocUtils.Report_rollingsky2_games_neigou(0, 4);
					return;
				}
				num = 7;
				if (payCallBackData.status == 100)
				{
					num = 8;
					MonoSingleton<GameTools>.Instacne.AddGoodsByShopTable(shops_shopTable);
					InfocUtils.Report_rollingsky2_games_neigou(shops_shopTable.Id, 3);
					num = 9;
					purchaseEvent.Add("af_currency", "USD");
					purchaseEvent.Add("af_revenue", shops_shopTable.Price);
					purchaseEvent.Add("af_quantity", "1");
					AppsFlyer.trackRichEvent("af_purchase", purchaseEvent);
				}
				else if (payCallBackData.status == 101)
				{
					num = 10;
					MonoSingleton<GameTools>.Instacne.AddGoodsByShopTable(shops_shopTable);
					InfocUtils.Report_rollingsky2_games_neigou(shops_shopTable.Id, 5);
					num = 11;
					purchaseEvent.Add("af_currency", "USD");
					purchaseEvent.Add("af_revenue", shops_shopTable.Price);
					purchaseEvent.Add("af_quantity", "1");
					AppsFlyer.trackRichEvent("af_purchase", purchaseEvent);
				}
				else
				{
					num = 12;
					InfocUtils.Report_rollingsky2_games_neigou(shops_shopTable.Id, 4);
					Log.Info(string.Format("IOS OnOperateIOSRepairOrder: callBackData.status = {0}", payCallBackData.status));
					num = 13;
				}
			}
			catch (Exception ex)
			{
				Log.Error(string.Format("OnOperateIOSRepairOrder Exception with errorcode : {0}  json : {1}", num, json));
				throw ex;
			}
		}

		private void OnOperateIOSRestore(string json)
		{
			Mod.UI.CloseUIForm(UIFormId.ShopLoadingForm);
			try
			{
				if (string.IsNullOrEmpty(json))
				{
					return;
				}
				CommonAlertData commonAlertData = new CommonAlertData();
				commonAlertData.showType = CommonAlertData.AlertShopType.COMMON;
				commonAlertData.callBackFunc = delegate
				{
					Mod.UI.CloseUIForm(UIFormId.CommonAlertForm);
				};
				RestoreCallBackData restoreCallBackData = JsonUtility.FromJson<RestoreCallBackData>(json);
				if (restoreCallBackData.status.Equals("300"))
				{
					commonAlertData.alertContent = Mod.Localization.GetInfoById(172);
					Shops_shopTable[] records = Mod.DataTable.Get<Shops_shopTable>().Records;
					Shops_shopTable shops_shopTable = null;
					int num = 0;
					int num2 = 0;
					foreach (RestoreResultData item in restoreCallBackData.result)
					{
						shops_shopTable = null;
						for (int i = 0; i < records.Length; i++)
						{
							if (records[i].Product_id_ios.Equals(item.productId))
							{
								shops_shopTable = records[i];
								break;
							}
						}
						if (shops_shopTable == null)
						{
							continue;
						}
						num = shops_shopTable.GoodsTeamid;
						num2 = shops_shopTable.Count;
						Dictionary<int, int>.Enumerator enumerator2 = MonoSingleton<GameTools>.Instacne.DealGoodsTeamById(num, num2).GetEnumerator();
						while (enumerator2.MoveNext())
						{
							int key = enumerator2.Current.Key;
							int value = enumerator2.Current.Value;
							Goods_goodsTable goods_goodsTable = Mod.DataTable.Get<Goods_goodsTable>().Get(key);
							if (goods_goodsTable != null && goods_goodsTable.CanRecover == 1)
							{
								PlayerDataModule.Instance.ChangePlayerGoodsNum(key, value, AssertChangeType.SHOP_REALMONEY);
							}
						}
					}
				}
				else if (restoreCallBackData.status.Equals("301"))
				{
					commonAlertData.alertContent = Mod.Localization.GetInfoById(173);
				}
				else
				{
					commonAlertData.alertContent = Mod.Localization.GetInfoById(174);
				}
				Mod.UI.OpenUIForm(UIFormId.CommonAlertForm, commonAlertData);
			}
			catch (Exception ex)
			{
				Log.Error(string.Format("OnOperateIOSRestore Exception {0}", json));
				throw ex;
			}
		}

		private void TipMoneyGoods(PayCallBackData callBackData, Shops_shopTable shopTable)
		{
			if (shopTable != null)
			{
				if (shopTable.BuyType != 1)
				{
					return;
				}
				if (callBackData.status < 200)
				{
					int goodsTeamid = shopTable.GoodsTeamid;
					int count = shopTable.Count;
					Dictionary<int, int> dictionary = MonoSingleton<GameTools>.Instacne.DealGoodsTeamById(goodsTeamid, count);
					int value = 0;
					dictionary.TryGetValue(6, out value);
					if (shopTable.NeedShowPayResult == 1)
					{
						MonoSingleton<GameTools>.Instacne.ShowBuyResult(true, goodsTeamid, count);
					}
					PlayerDataModule.Instance.DealBuyResult(true, shopTable.Id);
				}
				else
				{
					MonoSingleton<GameTools>.Instacne.ShowBuyResult(false);
				}
			}
			else
			{
				MonoSingleton<GameTools>.Instacne.ShowBuyResult(false);
			}
		}
	}
}
