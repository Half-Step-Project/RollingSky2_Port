using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.UI;

public class ShopUISingleResponseProductItem : UIProductItem
{
	private ShopItemData m_productData;

	[Header("商品信息：")]
	public Text m_productNameText;

	public Image m_productIconImage;

	public Text m_productNumberText;

	[Header("资源按钮：")]
	public AssetButtonHandler m_assetButtonHandler;

	[Header("购买按钮：")]
	public Button m_buyButton;

	public Text m_buyButtonText;

	[Header("热购：")]
	public GameObject m_hotGameObject;

	[Header("打折：")]
	public GameObject m_discountGameObject;

	public Text m_discountTxt;

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
		return GetShopDataModule.IsCanShowShopForm(m_productData.id);
	}

	private void SetCanClickBuyButton(bool interactable)
	{
		m_buyButton.interactable = interactable;
	}

	public override void OnInit(object userData)
	{
		m_isInit = true;
		m_productData = (ShopItemData)userData;
		string _goodIconName = m_productData.iconId.ToString();
		Dictionary<int, int> dictionary = MonoSingleton<GameTools>.Instacne.DealGoodsTeamById(m_productData.goodsdTeamid, m_productData.count);
		Goods_goodsTable assetData = null;
		using (Dictionary<int, int>.Enumerator enumerator = dictionary.GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				KeyValuePair<int, int> current = enumerator.Current;
				assetData = Mod.DataTable.Get<Goods_goodsTable>().Get(current.Key);
			}
		}
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
		if (m_assetButtonHandler != null)
		{
			m_assetButtonHandler.SetAssetData(assetData);
		}
	}

	public override void OnOpen()
	{
		OnRefresh();
		InfocUtils.Report_rollingsky2_games_neigou(m_productData.itemId, 1);
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
		SetProductNumber(m_productData.count.ToString());
		SetBuyNumber(m_productData.price);
		string productName = ((m_productData.name != -1) ? Mod.Localization.GetInfoById(m_productData.name) : string.Empty);
		SetProductName(productName);
		SetHotActive(m_productData.hot != 0);
		SetCanClickBuyButton(IsCanClickBuyButton());
	}

	private void OnClickBuyButton()
	{
		MonoSingleton<GameTools>.Instacne.CommonBuyOperate(m_productData.itemId);
		PlayerDataModule.Instance.PluginAdController.CommonBuyRecord.Add(UIFormId.ShopForm);
	}
}
