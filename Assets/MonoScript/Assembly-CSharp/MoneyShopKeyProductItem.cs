using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.UI;

public class MoneyShopKeyProductItem : UILoopItem
{
	public Image m_goodsIcon;

	public Text m_goodsNumTxt;

	public Button m_buyBtn;

	public Image m_buyIcon;

	public Text m_buyBtnTxt;

	public GameObject m_goodsSale;

	public GameObject m_hotObject;

	public GameObject[] m_ten = new GameObject[10];

	public GameObject[] m_ge = new GameObject[10];

	private ShopItemData m_itemData;

	private List<object> m_loadedAsserts = new List<object>();

	private bool m_isInit;

	public override void Data(object data)
	{
		m_isInit = true;
		base.Data(data);
		m_itemData = (ShopItemData)data;
		m_buyBtnTxt.text = "x" + m_itemData.price;
		m_goodsNumTxt.text = m_itemData.showInfo;
		ShowDiscount(m_itemData.discount);
		ShowHot(m_itemData.hot);
		string _goodsIconName = m_itemData.iconId.ToString();
		Mod.Resource.LoadAsset(AssetUtility.GetGameIconAsset(_goodsIconName), new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
		{
			if (!m_isInit)
			{
				OnRelease();
			}
			else if (m_goodsIcon != null && asset != null)
			{
				m_goodsIcon.sprite = MonoSingleton<GameTools>.Instacne.CreateSpriteByTexture2D(asset as Texture2D);
				m_loadedAsserts.Add(asset);
			}
		}, delegate(string assetName, string errorMessage, object data2)
		{
			Log.Error(string.Format("Can not load sprite '{0}' from '{1}' with error message '{2}'.", _goodsIconName, assetName, errorMessage));
		}));
		Goods_goodsTable goods_goodsTable = Mod.DataTable.Get<Goods_goodsTable>().Get(6);
		string _moneyIconName = goods_goodsTable.IconId.ToString();
		Mod.Resource.LoadAsset(AssetUtility.GetGameIconAsset(_moneyIconName), new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
		{
			if (!m_isInit)
			{
				OnRelease();
			}
			else if (m_buyIcon != null && asset != null)
			{
				m_buyIcon.sprite = MonoSingleton<GameTools>.Instacne.CreateSpriteByTexture2D(asset as Texture2D);
				m_loadedAsserts.Add(asset);
			}
		}, delegate(string assetName, string errorMessage, object data2)
		{
			Log.Error(string.Format("Can not load sprite '{0}' from '{1}' with error message '{2}'.", _moneyIconName, assetName, errorMessage));
		}));
		EventTriggerListener.Get(m_buyBtn.gameObject).onClick = OnClickBuyButton;
	}

	public override object GetData()
	{
		return base.GetData();
	}

	public override void SetSelected(bool selected)
	{
		base.SetSelected(selected);
	}

	public override void OnRelease()
	{
		m_isInit = false;
		for (int i = 0; i < m_loadedAsserts.Count; i++)
		{
			Mod.Resource.UnloadAsset(m_loadedAsserts[i]);
		}
		m_loadedAsserts.Clear();
	}

	private void OnClickBuyButton(GameObject obj)
	{
		if (IsCanBuy())
		{
			CommonAlertData commonAlertData = new CommonAlertData();
			commonAlertData.showType = CommonAlertData.AlertShopType.BUY_SHOPITEM;
			commonAlertData.shopItemId = m_itemData.itemId;
			commonAlertData.lableContent = Mod.Localization.GetInfoById(21);
			commonAlertData.callBackFunc = delegate
			{
				Mod.UI.CloseUIForm(UIFormId.CommonAlertForm);
				OnBuy();
			};
			Mod.UI.OpenUIForm(UIFormId.CommonAlertForm, commonAlertData);
		}
	}

	private bool IsCanBuy()
	{
		return Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).GetPlayGoodsNum(6) >= (double)int.Parse(m_itemData.price);
	}

	private void OnBuy()
	{
		PlayerDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule);
		dataModule.ChangePlayerGoodsNum(6, -int.Parse(m_itemData.price), AssertChangeType.SHOP_KEY);
		GoodsTeam_goodsTeamTable goodsTeam_goodsTeamTable = Mod.DataTable.Get<GoodsTeam_goodsTeamTable>().Get(m_itemData.goodsdTeamid);
		int num = int.Parse(goodsTeam_goodsTeamTable.GoodsIds);
		int num2 = int.Parse(goodsTeam_goodsTeamTable.GoodsCount) * m_itemData.count;
		Mod.DataTable.Get<Goods_goodsTable>().Get(num);
		dataModule.ChangePlayerGoodsNum(num, num2, AssertChangeType.SHOP_KEY);
		TipBuySceccuss(goodsTeam_goodsTeamTable.Id, m_itemData.count);
		InfocUtils.Report_rollingsky2_games_currency(3, 1, 2, 0, m_itemData.itemId);
	}

	private void TipBuySceccuss(int goodsID, int number)
	{
		MonoSingleton<GameTools>.Instacne.ShowBuyResult(true, goodsID, number);
	}

	private void ShowDiscount(int discount)
	{
		if (discount > 0)
		{
			m_goodsSale.SetActive(true);
			ShowDiscountNum(discount / 10, discount % 10);
		}
		else
		{
			m_goodsSale.SetActive(false);
		}
	}

	private void ShowDiscountNum(int ten, int ge)
	{
		for (int i = 0; i < 10; i++)
		{
			if (ten == 0)
			{
				m_ten[i].SetActive(false);
			}
			else if (i == ten)
			{
				m_ten[i].SetActive(true);
			}
			else
			{
				m_ten[i].SetActive(false);
			}
			if (i == ge)
			{
				m_ge[i].SetActive(true);
			}
			else
			{
				m_ge[i].SetActive(false);
			}
		}
	}

	private void ShowHot(int active)
	{
		if ((bool)m_hotObject)
		{
			m_hotObject.SetActive(active == 1);
		}
	}
}
