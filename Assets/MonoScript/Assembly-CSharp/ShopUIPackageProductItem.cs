using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIPackageProductItem : UIProductItem
{
	public Text m_productText;

	public Image m_productIconImage;

	public Button m_buyBtn;

	public Text m_buyBtnTxt;

	public GameObject m_hotGameObject;

	public Image m_interactableImage;

	public Image[] m_nodeObjects;

	public Text[] m_nodeTexts;

	private ShopItemData m_data;

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
				m_productIconImage.SetNativeSize();
				m_loadedAsserts.Add(asset);
			}
		}, delegate(string assetName, string errorMessage, object data2)
		{
			Log.Error(string.Format("Can not load sprite '{0}' from '{1}' with error message '{2}'.", _iconName, assetName, errorMessage));
		}));
		Dictionary<int, int> dictionary = MonoSingleton<GameTools>.Instacne.DealGoodsTeamById(m_data.goodsdTeamid, m_data.count);
		Debug.Log(m_data.itemId + "=" + dictionary.Count);
		int num = 0;
		foreach (KeyValuePair<int, int> item in dictionary)
		{
			Goods_goodsTable goods_goodsTable = Mod.DataTable.Get<Goods_goodsTable>().Get(item.Key);
			Image _image = m_nodeObjects[num];
			_image.gameObject.SetActive(true);
			string _goodsIconName = goods_goodsTable.IconId.ToString();
			Mod.Resource.LoadAsset(AssetUtility.GetGameIconAsset(_goodsIconName), new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
			{
				if (!m_isInit)
				{
					OnRelease();
				}
				else if (asset != null && _image != null)
				{
					_image.sprite = MonoSingleton<GameTools>.Instacne.CreateSpriteByTexture2D(asset as Texture2D);
					_image.SetNativeSize();
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
		SetBuyButtonColor((ShopType)m_data.type);
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
		m_buyBtn.onClick.RemoveListener(OnClickBuyButton);
		m_isInit = false;
		for (int i = 0; i < m_loadedAsserts.Count; i++)
		{
			Mod.Resource.UnloadAsset(m_loadedAsserts[i]);
		}
		m_loadedAsserts.Clear();
	}

	public override void OnRefresh()
	{
		OnRefreshLanguage();
		SetCanClickBuyButton(GetShopDataModule.IsCanShowShopForm(m_data.itemId));
	}

	private void OnRefreshLanguage()
	{
		m_productText.text = ((m_data.name > 0) ? Mod.Localization.GetInfoById(m_data.name) : "~the paoduct is unName");
		SetHotActive(m_data.hot != 0);
	}

	private void OnClickBuyButton()
	{
		Mod.UI.OpenUIForm(UIFormId.GiftPackageBuyForm, m_data.itemId);
	}

	private void SetCanClickBuyButton(bool interactable)
	{
		if (interactable)
		{
			m_buyBtnTxt.text = m_data.price;
			m_buyBtnTxt.color = new Color32(0, 0, 0, byte.MaxValue);
		}
		else
		{
			m_buyBtnTxt.text = Mod.Localization.GetInfoById(103);
			m_buyBtnTxt.color = new Color32(0, 0, 0, 120);
		}
		m_buyBtn.interactable = interactable;
		if (m_interactableImage != null)
		{
			m_interactableImage.gameObject.SetActive(!interactable);
		}
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
}
