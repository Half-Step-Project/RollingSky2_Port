using System.Collections.Generic;
using UnityEngine;

public class ShopUIProductList : MonoBehaviour
{
	private ShopUIProductListData m_data;

	public ShopUITitleItem m_titlePrefab;

	public ShopUIProductItem m_productPrefab;

	public ShopUINullProductItem m_nullProductPrefab;

	public RectTransform m_rectTransform;

	public int m_firstSpacing = 10;

	public int m_spacing = 5;

	public int m_segmentationSpacing = 15;

	public int m_xSpacing;

	public int m_ySpacing;

	public int m_columnCount = 3;

	private List<UITitleItem> m_titles = new List<UITitleItem>();

	private List<UIProductItem> m_products = new List<UIProductItem>();

	public void OnInit(ShopUIProductListData userData)
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
		foreach (KeyValuePair<ShopType, List<ShopUIProductItemData>> data in m_data.m_datas)
		{
			if (data.Value != null && data.Value.Count != 0)
			{
				num6 += height;
				num6 += (float)m_spacing;
				int num7 = Mathf.CeilToInt((float)data.Value.Count * 1f / (float)m_columnCount);
				num6 += (float)num7 * (height2 + (float)m_ySpacing);
				num6 += (float)m_segmentationSpacing;
			}
		}
		num6 += num5;
		m_rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, num6);
		float num8 = component2.rect.width * (float)m_columnCount + (float)(m_xSpacing * (m_columnCount - 1));
		num -= (float)m_firstSpacing;
		foreach (KeyValuePair<ShopType, List<ShopUIProductItemData>> data2 in m_data.m_datas)
		{
			if (data2.Value == null || data2.Value.Count == 0)
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
			ShopUITitleItem component3 = obj.GetComponent<ShopUITitleItem>();
			component3.OnInit(data2.Key);
			m_titles.Add(component3);
			int num9 = Mathf.CeilToInt((float)data2.Value.Count * 1f / (float)m_columnCount);
			float num10 = 0f - num8 / 2f + num4;
			int num11 = 0;
			for (int i = 0; i < num9; i++)
			{
				num -= num3;
				for (int j = 0; j < m_columnCount; j++)
				{
					if (num11 < data2.Value.Count)
					{
						GameObject obj2 = Object.Instantiate(m_productPrefab.gameObject);
						obj2.SetActive(true);
						obj2.transform.SetParent(m_rectTransform);
						obj2.transform.localScale = new Vector3(1f, 1f);
						obj2.transform.localPosition = new Vector3(num10 + (width + (float)m_xSpacing) * (float)j, num, 0f);
						ShopUIProductItem component4 = obj2.GetComponent<ShopUIProductItem>();
						component4.OnInit(data2.Value[num11]);
						m_products.Add(component4);
					}
					else
					{
						GameObject obj3 = Object.Instantiate(m_nullProductPrefab.gameObject);
						obj3.SetActive(true);
						obj3.transform.SetParent(m_rectTransform);
						obj3.transform.localScale = new Vector3(1f, 1f);
						obj3.transform.localPosition = new Vector3(num10 + (width + (float)m_xSpacing) * (float)j, num, 0f);
						ShopUINullProductItem component5 = obj3.GetComponent<ShopUINullProductItem>();
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
