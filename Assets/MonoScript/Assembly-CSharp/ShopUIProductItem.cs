using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIProductItem : UIProductItem
{
	public Text m_productNameText;

	public Image m_productIconImage;

	public Text m_productNumberText;

	public Button m_buyButton;

	public Text m_buyButtonText;

	public Image m_moneyImage;

	public GameObject m_hotGameObject;

	private ShopUIProductItemData m_productData;

	private List<object> m_loadedAsserts = new List<object>();

	private bool m_isInit;

	private ShopDataModule GetShopDataModule
	{
		get
		{
			return Singleton<DataModuleManager>.Instance.GetDataModule<ShopDataModule>(DataNames.ShopDataModule);
		}
	}

	private void SetProductNumber(string productNumber)
	{
		m_productNumberText.text = "X" + productNumber;
	}

	private void SetBuyNumber(string buyNumber)
	{
		m_buyButtonText.text = buyNumber;
	}

	private void SetProductName(string productName)
	{
		if (m_productNameText != null)
		{
			m_productNameText.text = productName;
		}
	}

	private void SetHotActive(bool active)
	{
		if (m_hotGameObject != null)
		{
			m_hotGameObject.gameObject.SetActive(active);
		}
	}

	private bool IsCanClickBuyButton()
	{
		return GetShopDataModule.IsCanShowShopForm(m_productData.m_shopTable.Id);
	}

	private void SetCanClickBuyButton(bool interactable)
	{
		m_buyButton.interactable = interactable;
	}

	public override void OnInit(object userData)
	{
		m_isInit = true;
		m_productData = (ShopUIProductItemData)userData;
		OnRefresh();
		string _moneyIconName = m_productData.m_moneyGoodsTable.IconId.ToString();
		Mod.Resource.LoadAsset(AssetUtility.GetGameIconAsset(_moneyIconName), new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
		{
			if (!m_isInit)
			{
				OnRelease();
			}
			else if (m_moneyImage != null && asset != null)
			{
				m_moneyImage.sprite = MonoSingleton<GameTools>.Instacne.CreateSpriteByTexture2D(asset as Texture2D);
				m_moneyImage.SetNativeSize();
				m_loadedAsserts.Add(asset);
			}
		}, delegate(string assetName, string errorMessage, object data2)
		{
			Log.Error(string.Format("Can not load sprite '{0}' from '{1}' with error message '{2}'.", _moneyIconName, assetName, errorMessage));
		}));
		string _goodIconName = m_productData.m_shopTable.IconId.ToString();
		Mod.Resource.LoadAsset(AssetUtility.GetGameIconAsset(_goodIconName), new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
		{
			if (!m_isInit)
			{
				OnRelease();
			}
			else if (m_productIconImage != null && asset != null)
			{
				m_productIconImage.sprite = MonoSingleton<GameTools>.Instacne.CreateSpriteByTexture2D(asset as Texture2D);
				m_productIconImage.SetNativeSize();
				m_loadedAsserts.Add(asset);
			}
		}, delegate(string assetName, string errorMessage, object data2)
		{
			Log.Error(string.Format("Can not load sprite '{0}' from '{1}' with error message '{2}'.", _goodIconName, assetName, errorMessage));
		}));
		m_buyButton.onClick.AddListener(OnClickBuyButton);
	}

	public override void OnOpen()
	{
		OnRefresh();
		InfocUtils.Report_rollingsky2_games_neigou(m_productData.m_shopTable.Id, 1);
	}

	public override void OnClose()
	{
	}

	public override void OnRelease()
	{
		m_buyButton.onClick.RemoveListener(OnClickBuyButton);
		m_isInit = false;
		for (int i = 0; i < m_loadedAsserts.Count; i++)
		{
			Mod.Resource.UnloadAsset(m_loadedAsserts[i]);
		}
		m_loadedAsserts.Clear();
	}

	public override void OnRefresh()
	{
		SetProductNumber((int.Parse(m_productData.m_productGoodsTeamTable.GoodsCount) * m_productData.m_shopTable.Count).ToString());
		SetBuyNumber(MonoSingleton<GameTools>.Instacne.GetProductRealPrice(m_productData.m_shopTable.Id));
		string productName = ((m_productData.m_shopTable.Name != -1) ? Mod.Localization.GetInfoById(m_productData.m_shopTable.Name) : string.Empty);
		SetProductName(productName);
		SetHotActive(m_productData.m_shopTable.Hot != 0);
		SetCanClickBuyButton(IsCanClickBuyButton());
	}

	private void OnClickBuyButton()
	{
		if (!IsCanClickBuyButton())
		{
			return;
		}
		Mod.Sound.PlayUISound(10000);
		CommonAlertData commonAlertData = new CommonAlertData();
		commonAlertData.showType = CommonAlertData.AlertShopType.BUY_SHOPITEM;
		commonAlertData.shopItemId = m_productData.m_shopTable.Id;
		commonAlertData.lableContent = Mod.Localization.GetInfoById(21);
		commonAlertData.callBackFunc = delegate
		{
			bool num = IsCanBuy();
			InfocUtils.Report_rollingsky2_games_neigou(m_productData.m_shopTable.Id, 2);
			if (num)
			{
				OnBuy();
				InfocUtils.Report_rollingsky2_games_neigou(m_productData.m_shopTable.Id, 3);
				Mod.UI.CloseUIForm(UIFormId.CommonAlertForm);
				SetCanClickBuyButton(IsCanClickBuyButton());
			}
			else
			{
				InfocUtils.Report_rollingsky2_games_neigou(m_productData.m_shopTable.Id, 4);
			}
		};
		Mod.UI.OpenUIForm(UIFormId.CommonAlertForm, commonAlertData);
	}

	private bool IsCanBuy()
	{
		if (!(Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).GetPlayGoodsNum(m_productData.m_moneyGoodsTable.Id) >= (double)int.Parse(m_productData.m_shopTable.Price)))
		{
			return false;
		}
		return true;
	}

	private void OnBuy()
	{
		PlayerDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule);
		dataModule.ChangePlayerGoodsNum(m_productData.m_moneyGoodsTable.Id, -int.Parse(m_productData.m_shopTable.Price));
		int num = m_productData.m_shopTable.Count * int.Parse(m_productData.m_productGoodsTeamTable.GoodsCount);
		dataModule.ChangePlayerGoodsNum(m_productData.m_productGoodsTable.Id, num);
		Singleton<DataModuleManager>.Instance.GetDataModule<ShopDataModule>(DataNames.ShopDataModule).AddBuyCountByID(m_productData.m_shopTable.Id);
		BroadResultByGoodsID(m_productData.m_productGoodsTable.Id, num);
	}

	private string GetLackingText()
	{
		string result = string.Empty;
		if (m_productData.m_moneyGoodsTable.Id == 6)
		{
			result = Mod.Localization.GetInfoById(36);
		}
		else if (m_productData.m_moneyGoodsTable.Id == 3)
		{
			result = Mod.Localization.GetInfoById(37);
		}
		return result;
	}

	private MoneyShopForm.MoneyType GetMoneyShopTypeID()
	{
		MoneyShopForm.MoneyType result = MoneyShopForm.MoneyType.Money;
		if (m_productData.m_moneyGoodsTable.Id == 6)
		{
			result = MoneyShopForm.MoneyType.Money;
		}
		else if (m_productData.m_moneyGoodsTable.Id == 3)
		{
			result = MoneyShopForm.MoneyType.Key;
		}
		return result;
	}

	private void BroadResulByGoodsTeamID(int goodsTeamId, int goodsTeamNum)
	{
		GetGoodsData getGoodsData = new GetGoodsData();
		getGoodsData.Buy = true;
		getGoodsData.GoodsTeam = true;
		getGoodsData.GoodsTeamId = goodsTeamId;
		getGoodsData.GoodsTeamNum = goodsTeamNum;
		Mod.UI.OpenUIForm(UIFormId.GetGoodsForm, getGoodsData);
	}

	private void BroadResultByGoodsID(int goodsId, int goodsNum)
	{
		GetGoodsData getGoodsData = new GetGoodsData();
		getGoodsData.Buy = true;
		getGoodsData.GoodsId = goodsId;
		getGoodsData.GoodsNum = goodsNum;
		Mod.UI.OpenUIForm(UIFormId.GetGoodsForm, getGoodsData);
	}
}
