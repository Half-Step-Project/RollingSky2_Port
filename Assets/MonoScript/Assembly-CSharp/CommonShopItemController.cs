using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.UI;

public class CommonShopItemController : UILoopItem
{
	private ShopItemData itemData;

	public Image goodsIcon;

	public Text goodsNumTxt;

	public Text buyBtnTxt;

	public GameObject buyBtn;

	public Image advIcon;

	public Image goldIcon;

	private float price;

	public override void Data(object data)
	{
		itemData = data as ShopItemData;
		if (itemData == null)
		{
			return;
		}
		string spriteName = itemData.iconId.ToString();
		Mod.Resource.LoadAsset(AssetUtility.GetGameIconAsset(spriteName), new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
		{
			goodsIcon.sprite = MonoSingleton<GameTools>.Instacne.CreateSpriteByTexture2D(asset as Texture2D);
			Mod.Resource.UnloadAsset(asset);
			goodsIcon.SetNativeSize();
		}, delegate(string assetName, string errorMessage, object data2)
		{
			Log.Error(string.Format("Can not load sprite '{0}' from '{1}' with error message '{2}'.", spriteName, assetName, errorMessage));
		}));
		GoodsTeam_goodsTeamTable goodsTeamTableById = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).GetGoodsTeamTableById(itemData.goodsdTeamid);
		if (goodsTeamTableById != null)
		{
			string[] array = goodsTeamTableById.GoodsCount.Split(':');
			int num = 1;
			if (array.Length != 0)
			{
				num = int.Parse(array[0]);
				goodsNumTxt.text = "x" + itemData.count * num;
			}
		}
		switch (itemData.buyType)
		{
		case 3:
			advIcon.gameObject.SetActive(true);
			goldIcon.gameObject.SetActive(false);
			buyBtnTxt.text = "";
			break;
		case 4:
			goldIcon.gameObject.SetActive(true);
			advIcon.gameObject.SetActive(false);
			buyBtnTxt.text = "x" + itemData.price;
			break;
		case 1:
			goldIcon.gameObject.SetActive(false);
			advIcon.gameObject.SetActive(false);
			buyBtnTxt.text = itemData.price;
			break;
		}
		buyBtnTxt.text = GetProductPriceByProductId();
		EventTriggerListener.Get(buyBtn).onClick = OnBuyBtnClickHandle;
	}

	private string GetProductPriceByProductId()
	{
		return "";
	}

	private string GetProductId()
	{
		return "";
	}

	private void OnBuyBtnClickHandle(GameObject obj)
	{
		switch (itemData.buyType)
		{
		case 1:
		{
			string productId = GetProductId();
			Log.Error("productId:" + productId);
			break;
		}
		case 2:
			if (Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).GetPlayGoodsNum(3) < (double)price)
			{
				CommonDialogUtil.Instance.ShowDialog("Gold is Not enough!", CommonDialogUtil.ShowType.Yes, "Yes", "No", delegate
				{
					CommonDialogUtil.Instance.CloseDialog();
				}, delegate
				{
					CommonDialogUtil.Instance.CloseDialog();
				});
			}
			break;
		case 3:
			break;
		}
	}

	private void SendShopBuyMessage()
	{
		ShopBuyResquestData shopBuyResquestData = new ShopBuyResquestData();
		shopBuyResquestData.shopItemIds.Add(itemData.itemId);
		NetMessageData netMessageData = ObjectPool<NetMessageData>.Instance.Get();
		netMessageData.messageId = NetMessageId.POST_SHOPBUY;
		netMessageData.content = JsonUtility.ToJson(shopBuyResquestData);
		netMessageData.succesHandler = delegate(NetMessageResultData content)
		{
			Log.Error("RequestShopBuyData:" + content);
			if (content.code == 0)
			{
				ShopBuyResponseData shopBuyResponseData = JsonUtility.FromJson<ShopBuyResponseData>(content.data);
				if (shopBuyResponseData != null && shopBuyResponseData.shopBuyResultData != null)
				{
					int i = 0;
					for (int count = shopBuyResponseData.shopBuyResultData.Count; i < count; i++)
					{
						Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).ChangePlayerGoodsNum(shopBuyResponseData.shopBuyResultData[i].goodsId, shopBuyResponseData.shopBuyResultData[i].num, AssertChangeType.SHOP_KEY, NetWorkSynType.NONE);
					}
				}
			}
		};
		netMessageData.errorHandler = delegate(string content)
		{
			Log.Error("RequestShopBuyData:" + content);
		};
		MonoSingleton<NetWorkManager>.Instacne.Send(netMessageData);
	}

	public override object GetData()
	{
		return itemData;
	}

	public override void SetSelected(bool selected)
	{
	}
}
