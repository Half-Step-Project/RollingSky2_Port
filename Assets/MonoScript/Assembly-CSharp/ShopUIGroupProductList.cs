using System.Collections.Generic;
using UnityEngine;

public class ShopUIGroupProductList : MonoBehaviour
{
	public ShopUIGroupProductListData m_groupData;

	public ShopUIGroupTitleItem m_titlePrefab;

	public ShopUINormalProductItem m_normalProductPrefab;

	public ShopUISingleResponseProductItem m_singleResponseProductPrefab;

	public ShopUIStackResponseProductItem m_stackResponseProductPrefab;

	public GameObject m_nullPrefab;

	public RectTransform m_rectTransform;

	private List<UITitleItem> m_titles = new List<UITitleItem>();

	private List<UIProductItem> m_products = new List<UIProductItem>();

	private List<GameObject> m_nullObjects = new List<GameObject>();

	public void OnInit(ShopUIGroupProductListData userData)
	{
		m_groupData = userData;
		bool flag = MonoSingleton<GameTools>.Instacne.IsOpenBuffFunction();
		foreach (KeyValuePair<ShopType, ShopResponseData> responseData in userData.m_responseDatas)
		{
			if (!flag && responseData.Key == ShopType.ABILITYPACKAGE)
			{
				continue;
			}
			GameObject obj = Object.Instantiate(m_titlePrefab.gameObject);
			obj.SetActive(true);
			obj.transform.SetParent(m_rectTransform);
			obj.transform.localScale = new Vector3(1f, 1f, 1f);
			obj.transform.localPosition = new Vector3(0f, 0f, 0f);
			ShopUIGroupTitleItem component = obj.GetComponent<ShopUIGroupTitleItem>();
			component.OnInit(responseData.Key);
			m_titles.Add(component);
			for (int i = 0; i < responseData.Value.shopItemList.Count; i++)
			{
				Dictionary<int, int> dictionary = MonoSingleton<GameTools>.Instacne.DealGoodsTeamById(responseData.Value.shopItemList[i].goodsdTeamid);
				if (dictionary.Count == 1)
				{
					GameObject obj2 = Object.Instantiate(m_singleResponseProductPrefab.gameObject);
					obj2.SetActive(true);
					obj2.transform.SetParent(m_rectTransform);
					obj2.transform.localScale = new Vector3(1f, 1f);
					obj2.transform.localPosition = new Vector3(0f, 0f, 0f);
					UIProductItem component2 = obj2.GetComponent<ShopUISingleResponseProductItem>();
					component2.OnInit(responseData.Value.shopItemList[i]);
					m_products.Add(component2);
				}
				else if (dictionary.Count > 1)
				{
					GameObject obj3 = Object.Instantiate(m_stackResponseProductPrefab.gameObject);
					obj3.SetActive(true);
					obj3.transform.SetParent(m_rectTransform);
					obj3.transform.localScale = new Vector3(1f, 1f);
					obj3.transform.localPosition = new Vector3(0f, 0f, 0f);
					UIProductItem component3 = obj3.GetComponent<ShopUIStackResponseProductItem>();
					component3.OnInit(responseData.Value.shopItemList[i]);
					m_products.Add(component3);
				}
			}
		}
		foreach (KeyValuePair<ShopType, List<ShopUIProductItemData>> normalData in userData.m_normalDatas)
		{
			if (normalData.Value != null && normalData.Value.Count != 0)
			{
				GameObject obj4 = Object.Instantiate(m_titlePrefab.gameObject);
				obj4.SetActive(true);
				obj4.transform.SetParent(m_rectTransform);
				obj4.transform.localScale = new Vector3(1f, 1f, 1f);
				obj4.transform.localPosition = new Vector3(0f, 0f, 0f);
				ShopUIGroupTitleItem component4 = obj4.GetComponent<ShopUIGroupTitleItem>();
				component4.OnInit(normalData.Key);
				m_titles.Add(component4);
				for (int j = 0; j < normalData.Value.Count; j++)
				{
					GameObject obj5 = Object.Instantiate(m_normalProductPrefab.gameObject);
					obj5.SetActive(true);
					obj5.transform.SetParent(m_rectTransform);
					obj5.transform.localScale = new Vector3(1f, 1f);
					obj5.transform.localPosition = new Vector3(0f, 0f, 0f);
					ShopUINormalProductItem component5 = obj5.GetComponent<ShopUINormalProductItem>();
					component5.OnInit(normalData.Value[j]);
					m_products.Add(component5);
				}
			}
		}
		GameObject gameObject = Object.Instantiate(m_nullPrefab.gameObject);
		gameObject.SetActive(true);
		gameObject.transform.SetParent(m_rectTransform);
		gameObject.transform.localScale = new Vector3(1f, 1f);
		m_nullObjects.Add(gameObject);
	}

	public void OnOpen()
	{
		for (int i = 0; i < m_titles.Count; i++)
		{
			m_titles[i].OnOpen();
		}
		for (int j = 0; j < m_products.Count; j++)
		{
			m_products[j].OnOpen();
		}
	}

	public void OnClose()
	{
		for (int i = 0; i < m_titles.Count; i++)
		{
			m_titles[i].OnClose();
		}
		for (int j = 0; j < m_products.Count; j++)
		{
			m_products[j].OnClose();
		}
	}

	public void OnRelease()
	{
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
		for (int k = 0; k < m_nullObjects.Count; k++)
		{
			Object.Destroy(m_nullObjects[k].gameObject);
		}
		m_titles.Clear();
		m_products.Clear();
		m_nullObjects.Clear();
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
}
