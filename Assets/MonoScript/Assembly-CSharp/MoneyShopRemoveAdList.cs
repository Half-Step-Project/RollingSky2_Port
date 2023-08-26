using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;

public class MoneyShopRemoveAdList : MonoBehaviour
{
	public ShopResponseData m_data;

	public ShopUINormalProductItem m_normalProductPrefab;

	public ShopUISingleResponseProductItem m_singleResponseProductPrefab;

	public ShopUIStackResponseProductItem m_stackResponseProductPrefab;

	public UIPersonalAssetsList mPersonalAssets;

	public RectTransform m_rectTransform;

	private List<UITitleItem> m_titles = new List<UITitleItem>();

	private List<UIProductItem> m_products = new List<UIProductItem>();

	private bool m_isCloseUI;

	private bool m_isOpen;

	public void OnInit(ShopResponseData data)
	{
		m_data = data;
		for (int i = 0; i < m_data.shopItemList.Count; i++)
		{
			Dictionary<int, int> dictionary = MonoSingleton<GameTools>.Instacne.DealGoodsTeamById(m_data.shopItemList[i].goodsdTeamid);
			if (dictionary.Count == 1)
			{
				GameObject obj = Object.Instantiate(m_singleResponseProductPrefab.gameObject);
				obj.SetActive(true);
				obj.transform.SetParent(m_rectTransform);
				obj.transform.localScale = new Vector3(1f, 1f);
				obj.transform.localPosition = new Vector3(0f, 0f, 0f);
				UIProductItem component = obj.GetComponent<ShopUISingleResponseProductItem>();
				component.OnInit(m_data.shopItemList[i]);
				m_products.Add(component);
			}
			else if (dictionary.Count > 1)
			{
				GameObject obj2 = Object.Instantiate(m_stackResponseProductPrefab.gameObject);
				obj2.SetActive(true);
				obj2.transform.SetParent(m_rectTransform);
				obj2.transform.localScale = new Vector3(1f, 1f);
				obj2.transform.localPosition = new Vector3(0f, 0f, 0f);
				UIProductItem component2 = obj2.GetComponent<ShopUIStackResponseProductItem>();
				component2.OnInit(m_data.shopItemList[i]);
				m_products.Add(component2);
			}
		}
		if ((bool)mPersonalAssets)
		{
			if (DeviceManager.Instance.IsNeedSpecialAdapte())
			{
				MonoSingleton<GameTools>.Instacne.AdapteSpecialScreen(mPersonalAssets.transform as RectTransform);
			}
			mPersonalAssets.OnInit();
		}
	}

	public void OnOpen()
	{
		m_isCloseUI = false;
		for (int i = 0; i < m_titles.Count; i++)
		{
			m_titles[i].OnOpen();
		}
		for (int j = 0; j < m_products.Count; j++)
		{
			m_products[j].OnOpen();
		}
		if (Mod.UI.UIFormIsOpen(UIFormId.LevelEnterForm))
		{
			m_isCloseUI = true;
			return;
		}
		m_isOpen = true;
		if ((bool)mPersonalAssets)
		{
			mPersonalAssets.OnOpen(UIPersonalAssetsList.ParentType.Shop);
		}
		PlayerDataModule.Instance.PlayerRecordData.AddShopRemoveAdFormOpenCount();
		InfocUtils.Report_rollingsky2_games_pageshow(11, 0, 1);
	}

	public void OnTick(float elapseSeconds, float realElapseSeconds)
	{
		if ((bool)mPersonalAssets)
		{
			mPersonalAssets.OnUpdate();
		}
		if (m_isCloseUI)
		{
			m_isCloseUI = false;
		}
	}

	public void OnClose()
	{
		if ((bool)mPersonalAssets)
		{
			mPersonalAssets.OnClose();
		}
		for (int i = 0; i < m_titles.Count; i++)
		{
			m_titles[i].OnClose();
		}
		for (int j = 0; j < m_products.Count; j++)
		{
			m_products[j].OnClose();
		}
		if (m_isOpen)
		{
			Mod.Event.Fire(this, Mod.Reference.Acquire<RemoveAdListCloseEventArgs>());
		}
		m_isOpen = false;
	}

	public void OnRelease()
	{
		if ((bool)mPersonalAssets)
		{
			mPersonalAssets.OnRelease();
		}
		for (int i = 0; i < m_titles.Count; i++)
		{
			m_titles[i].OnRelease();
			Object.Destroy(m_titles[i].gameObject);
		}
		for (int j = 0; j < m_products.Count; j++)
		{
			m_products[j].OnRelease();
			Object.Destroy(m_products[j].gameObject);
		}
		m_titles.Clear();
		m_products.Clear();
	}

	public void OnRefresh()
	{
		for (int i = 0; i < m_titles.Count; i++)
		{
			m_titles[i].OnRefresh();
		}
		for (int j = 0; j < m_products.Count; j++)
		{
			m_products[j].OnRefresh();
		}
	}

	public void BuyPackageSuccessHandler(object sender, BuySuccessEventArgs e)
	{
		if (e != null && e.ShopItemId == 1009)
		{
			InfocUtils.Report_rollingsky2_games_pageshow(11, 15, 2);
		}
	}
}
