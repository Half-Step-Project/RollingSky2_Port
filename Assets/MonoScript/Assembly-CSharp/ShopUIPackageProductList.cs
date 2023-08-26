using System.Collections.Generic;
using UnityEngine;

public class ShopUIPackageProductList : MonoBehaviour
{
	private ShopUIPackageProductListData m_data;

	public ShopUIPackageTitleItem m_titlePrefab;

	public ShopUIPackageProductItem m_productPrefab;

	public ShopUINullPackageProductItem m_nullProductPrefab;

	public RectTransform m_rectTransform;

	public int m_firstSpacing = 10;

	public int m_spacing = 5;

	public int m_segmentationSpacing = 15;

	public int m_xSpacing;

	public int m_ySpacing;

	public int m_columnCount = 3;

	private List<UITitleItem> m_titles = new List<UITitleItem>();

	private List<UIProductItem> m_products = new List<UIProductItem>();

	public void OnInit(ShopUIPackageProductListData userData)
	{
		m_data = userData;
		if (m_titlePrefab == null || m_productPrefab == null)
		{
			return;
		}
		float num = 0f;
		RectTransform component = m_titlePrefab.gameObject.GetComponent<RectTransform>();
		RectTransform component2 = m_productPrefab.gameObject.GetComponent<RectTransform>();
		float height = component.rect.height;
		float height2 = component2.rect.height;
		float width = component2.rect.width;
		float num2 = height / 2f;
		float num3 = height2 / 2f;
		float num4 = width / 2f;
		float num5 = height2;
		float num6 = m_firstSpacing;
		foreach (KeyValuePair<ShopType, ShopResponseData> responseData in m_data.m_responseDatas)
		{
			if (responseData.Value != null && responseData.Value.shopItemList.Count != 0)
			{
				num6 += height;
				num6 += (float)m_spacing;
				int num7 = Mathf.CeilToInt((float)responseData.Value.shopItemList.Count * 1f / (float)m_columnCount);
				num6 += (float)num7 * (height2 + (float)m_ySpacing);
				num6 += (float)m_segmentationSpacing;
			}
		}
		num6 += num5;
		m_rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, num6);
		float num8 = component2.rect.width * (float)m_columnCount + (float)(m_xSpacing * (m_columnCount - 1));
		num -= (float)m_firstSpacing;
		bool flag = MonoSingleton<GameTools>.Instacne.IsOpenBuffFunction();
		foreach (KeyValuePair<ShopType, ShopResponseData> responseData2 in m_data.m_responseDatas)
		{
			if ((!flag && responseData2.Key == ShopType.ABILITYPACKAGE) || responseData2.Value == null || responseData2.Value.shopItemList.Count == 0)
			{
				continue;
			}
			GameObject obj = Object.Instantiate(m_titlePrefab.gameObject);
			obj.SetActive(true);
			num -= num2;
			obj.transform.SetParent(m_rectTransform);
			obj.transform.localScale = new Vector3(1f, 1f, 1f);
			obj.transform.localPosition = new Vector3(0f, num, 0f);
			num -= num2;
			num -= (float)m_spacing;
			ShopUIPackageTitleItem component3 = obj.GetComponent<ShopUIPackageTitleItem>();
			component3.OnInit(responseData2.Key);
			m_titles.Add(component3);
			int num9 = Mathf.CeilToInt((float)responseData2.Value.shopItemList.Count * 1f / (float)m_columnCount);
			float num10 = 0f - num8 / 2f + num4;
			int num11 = 0;
			for (int i = 0; i < num9; i++)
			{
				num -= num3;
				for (int j = 0; j < m_columnCount; j++)
				{
					if (num11 < responseData2.Value.shopItemList.Count)
					{
						GameObject obj2 = Object.Instantiate(m_productPrefab.gameObject);
						obj2.SetActive(true);
						obj2.transform.SetParent(m_rectTransform);
						obj2.transform.localScale = new Vector3(1f, 1f);
						obj2.transform.localPosition = new Vector3(num10 + (width + (float)m_xSpacing) * (float)j, num, 0f);
						ShopUIPackageProductItem component4 = obj2.GetComponent<ShopUIPackageProductItem>();
						component4.OnInit(responseData2.Value.shopItemList[num11]);
						m_products.Add(component4);
					}
					else
					{
						GameObject obj3 = Object.Instantiate(m_nullProductPrefab.gameObject);
						obj3.SetActive(true);
						obj3.transform.SetParent(m_rectTransform);
						obj3.transform.localScale = new Vector3(1f, 1f);
						obj3.transform.localPosition = new Vector3(num10 + (width + (float)m_xSpacing) * (float)j, num, 0f);
						ShopUINullPackageProductItem component5 = obj3.GetComponent<ShopUINullPackageProductItem>();
						component5.OnInit(null);
						m_products.Add(component5);
					}
					num11++;
				}
				num -= num3;
				num -= (float)m_ySpacing;
			}
			num -= (float)m_segmentationSpacing;
		}
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
}
