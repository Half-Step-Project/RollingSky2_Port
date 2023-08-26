using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIStackResponseProductItem : UIProductItem
{
	private ShopItemData m_data;

	[Header("礼包信息：")]
	public Text m_productText;

	public Image m_productIconImage;

	[Header("购买按钮：")]
	public Button m_buyBtn;

	public Text m_buyBtnTxt;

	[Header("热购：")]
	public GameObject m_hotGameObject;

	[Header("去广告：")]
	public AssetButtonHandler m_removeAdHandler;

	[Header("打折：")]
	public GameObject m_discountGameObject;

	public Text m_discountTxt;

	[Header("礼包包含商品信息，顺序影响显示：")]
	public Image[] m_nodeObjects;

	public Text[] m_nodeTexts;

	public AssetButtonHandler[] m_nodeHandlers;

	private List<object> m_loadedAsserts = new List<object>();

	private bool m_isInit;

	private ShopDataModule GetShopDataModule
	{
		get
		{
			return Singleton<DataModuleManager>.Instance.GetDataModule<ShopDataModule>(DataNames.ShopDataModule);
		}
	}

	public override void OnInit(object data)
	{
		m_isInit = true;
		m_data = (ShopItemData)data;
		m_buyBtn.onClick.AddListener(OnClickBuyButton);
		SetCancelAdActive(false);
		string _iconName = m_data.iconId.ToString();
		Mod.Resource.LoadAsset(AssetUtility.GetGameIconAsset(_iconName), new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
		{
			if (!m_isInit)
			{
				OnRelease();
			}
			else if (m_productIconImage != null && asset != null)
			{
				m_productIconImage.sprite = MonoSingleton<GameTools>.Instacne.CreateSpriteByTexture2D(asset as Texture2D);
				m_loadedAsserts.Add(asset);
			}
		}, delegate(string assetName, string errorMessage, object data2)
		{
			Log.Error(string.Format("Can not load sprite '{0}' from '{1}' with error message '{2}'.", _iconName, assetName, errorMessage));
		}));
		Dictionary<int, int> dictionary = MonoSingleton<GameTools>.Instacne.DealGoodsTeamById(m_data.goodsdTeamid, m_data.count);
		int num = 0;
		foreach (KeyValuePair<int, int> item in dictionary)
		{
			Goods_goodsTable goods_goodsTable = Mod.DataTable.Get<Goods_goodsTable>().Get(item.Key);
			if (dictionary.Count > 1 && item.Key == GameCommon.REMOVE_AD)
			{
				SetCancelAdActive(true);
				m_removeAdHandler.SetAssetData(goods_goodsTable);
				continue;
			}
			Image _image = m_nodeObjects[num];
			_image.gameObject.SetActive(true);
			string _goodsIconName = goods_goodsTable.IconId.ToString();
			m_nodeHandlers[num].SetAssetData(goods_goodsTable);
			Mod.Resource.LoadAsset(AssetUtility.GetGameIconAsset(_goodsIconName), new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
			{
				if (!m_isInit)
				{
					OnRelease();
				}
				else if (asset != null && _image != null)
				{
					_image.sprite = MonoSingleton<GameTools>.Instacne.CreateSpriteByTexture2D(asset as Texture2D);
					m_loadedAsserts.Add(asset);
				}
			}, delegate(string assetName, string errorMessage, object data2)
			{
				Log.Error(string.Format("Can not load sprite '{0}' from '{1}' with error message '{2}'.", _goodsIconName, assetName, errorMessage));
			}));
			m_nodeTexts[num].text = "X" + item.Value;
			num++;
		}
		for (int i = num; i < m_nodeObjects.Length; i++)
		{
			m_nodeObjects[i].gameObject.SetActive(false);
		}
		SetDiscount(m_data.discount);
	}

	public override void OnOpen()
	{
		OnRefresh();
		InfocUtils.Report_rollingsky2_games_neigou(m_data.itemId, 1);
	}

	public override void OnClose()
	{
	}

	public override void OnRelease()
	{
		m_isInit = false;
		m_buyBtn.onClick.RemoveListener(OnClickBuyButton);
		for (int i = 0; i < m_loadedAsserts.Count; i++)
		{
			Mod.Resource.UnloadAsset(m_loadedAsserts[i]);
		}
		m_loadedAsserts.Clear();
	}

	public override void OnRefresh()
	{
		OnRefreshLanguage();
		OnRefreshRemoveAdButton();
		SetCanClickBuyButton(GetShopDataModule.IsCanShowShopForm(m_data.itemId));
	}

	private void OnRefreshRemoveAdButton()
	{
		if (PlayerDataModule.Instance.GetPlayGoodsNum(GameCommon.REMOVE_AD) > 0.0)
		{
			SetCancelAdActive(false);
		}
	}

	private void OnRefreshLanguage()
	{
		m_productText.text = ((m_data.name > 0) ? Mod.Localization.GetInfoById(m_data.name) : "~the paoduct is unName");
		SetHotActive(m_data.hot != 0);
	}

	private void OnClickBuyButton()
	{
		MonoSingleton<GameTools>.Instacne.CommonBuyOperate(m_data.itemId);
		PlayerDataModule.Instance.PluginAdController.CommonBuyRecord.Add(UIFormId.ShopForm);
	}

	private void SetCanClickBuyButton(bool interactable)
	{
		if (interactable)
		{
			m_buyBtnTxt.text = m_data.price;
		}
		else
		{
			m_buyBtnTxt.text = Mod.Localization.GetInfoById(103);
		}
		m_buyBtn.interactable = interactable;
	}

	private void SetHotActive(bool active)
	{
		if (m_hotGameObject != null)
		{
			m_hotGameObject.gameObject.SetActive(active);
		}
	}

	private void SetBuyButtonColor(ShopType shopType)
	{
		if (!(m_buyBtn == null))
		{
			ColorBlock colors = m_buyBtn.colors;
			switch (shopType)
			{
			case ShopType.BASICPACKAGE:
				colors.normalColor = new Color32(141, 189, 212, byte.MaxValue);
				colors.highlightedColor = new Color32(141, 189, 212, byte.MaxValue);
				colors.pressedColor = new Color32(121, 169, 192, byte.MaxValue);
				break;
			case ShopType.ABILITYPACKAGE:
				colors.normalColor = new Color32(212, 176, 141, byte.MaxValue);
				colors.highlightedColor = new Color32(212, 176, 141, byte.MaxValue);
				colors.pressedColor = new Color32(192, 156, 121, byte.MaxValue);
				break;
			}
			if (m_buyBtn.targetGraphic != null)
			{
				m_buyBtn.targetGraphic.color = Color.white;
			}
			m_buyBtn.colors = colors;
		}
	}

	private void SetCancelAdActive(bool active)
	{
		if (m_removeAdHandler != null)
		{
			m_removeAdHandler.gameObject.SetActive(active);
		}
	}

	private void SetDiscount(bool active, string DiscountValue)
	{
		if (m_discountGameObject != null)
		{
			m_discountGameObject.SetActive(active);
		}
		if (m_discountTxt != null)
		{
			m_discountTxt.text = DiscountValue;
		}
	}

	private void SetDiscount(int discount)
	{
		if (discount <= 0)
		{
			SetDiscount(false, string.Empty);
			return;
		}
		string discountValue = string.Format("{0}%", Mathf.Abs(discount - 100));
		SetDiscount(true, discountValue);
	}
}
