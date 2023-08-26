using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILoopList : MonoBehaviour
{
	private enum Direction
	{
		Horizontal,
		Vertical
	}

	public delegate void OnSelectedEvent(UILoopItem item);

	[SerializeField]
	private float topPadding;

	[SerializeField]
	private float bottomPadding;

	[SerializeField]
	private RectTransform m_Cell;

	[SerializeField]
	private Vector2 m_Page;

	[SerializeField]
	private Direction direction;

	[SerializeField]
	[Range(4f, 10f)]
	private int m_BufferNo;

	[SerializeField]
	private float cellGapX;

	[SerializeField]
	private float cellGapY;

	public OnSelectedEvent onSelectedEvent;

	private List<RectTransform> m_InstantiateItems = new List<RectTransform>();

	private List<RectTransform> m_oldItems = new List<RectTransform>();

	private IList m_Datas;

	private float m_PrevPos;

	private int m_CurrentIndex;

	private Vector2 m_InstantiateSize = Vector2.zero;

	private ScrollRect m_ScrollRect;

	private RectTransform m_Rect;

	private bool isFirst = true;

	private object selectedObject;

	[NonSerialized]
	public UILoopItem selectedItem;

	private List<Button> addClickEventList = new List<Button>();

	public Vector2 CellRect
	{
		get
		{
			if (!(m_Cell != null))
			{
				return new Vector2(100f, 100f);
			}
			return new Vector2(cellGapX + m_Cell.sizeDelta.x, cellGapY + m_Cell.sizeDelta.y);
		}
	}

	public float CellScale
	{
		get
		{
			if (direction != 0)
			{
				return CellRect.y;
			}
			return CellRect.x;
		}
	}

	public float DirectionPos
	{
		get
		{
			if (direction != 0)
			{
				return m_Rect.anchoredPosition.y;
			}
			return m_Rect.anchoredPosition.x;
		}
	}

	public Vector2 InstantiateSize
	{
		get
		{
			if (m_InstantiateSize == Vector2.zero)
			{
				float x;
				float y;
				if (direction == Direction.Horizontal)
				{
					x = m_Page.x;
					y = m_Page.y + (float)m_BufferNo;
				}
				else
				{
					x = m_Page.x + (float)m_BufferNo;
					y = m_Page.y;
				}
				m_InstantiateSize = new Vector2(x, y);
			}
			return m_InstantiateSize;
		}
	}

	public int PageCount
	{
		get
		{
			return (int)m_Page.x * (int)m_Page.y;
		}
	}

	public int PageScale
	{
		get
		{
			if (direction != 0)
			{
				return (int)m_Page.y;
			}
			return (int)m_Page.x;
		}
	}

	public int InstantiateCount
	{
		get
		{
			return (int)InstantiateSize.x * (int)InstantiateSize.y;
		}
	}

	public float MaxPrevPos
	{
		get
		{
			Vector2 rectByNum = getRectByNum(m_Datas.Count);
			float num = ((direction != 0) ? (rectByNum.x - m_Page.x) : (rectByNum.y - m_Page.y));
			return num * CellScale;
		}
	}

	public float scale
	{
		get
		{
			if (direction != 0)
			{
				return -1f;
			}
			return 1f;
		}
	}

	protected void Awake()
	{
		m_ScrollRect = GetComponentInParent<ScrollRect>();
		m_ScrollRect.horizontal = direction == Direction.Horizontal;
		m_ScrollRect.vertical = direction == Direction.Vertical;
		m_Rect = GetComponent<RectTransform>();
		if (m_Cell.transform.parent != null)
		{
			m_Cell.gameObject.SetActive(false);
		}
		m_Rect.anchorMax = Vector2.up;
		m_Rect.anchorMin = Vector2.up;
		m_Rect.pivot = Vector2.up;
		m_Rect.anchoredPosition = new Vector2(0f, 0f);
	}

	public void Data(object data)
	{
		Reset();
		m_Datas = data as IList;
		if (m_Datas.Count > PageCount)
		{
			setBound(getRectByNum(m_Datas.Count));
		}
		else
		{
			setBound(m_Page);
		}
		if (m_Datas.Count > InstantiateCount)
		{
			while (m_InstantiateItems.Count < InstantiateCount)
			{
				createItem(m_InstantiateItems.Count);
			}
		}
		else
		{
			while (m_InstantiateItems.Count > m_Datas.Count)
			{
				removeItem(m_InstantiateItems.Count - 1);
			}
			while (m_InstantiateItems.Count < m_Datas.Count)
			{
				createItem(m_InstantiateItems.Count);
			}
		}
		if (m_Datas.Count > 0)
		{
			int num = Mathf.Min(m_InstantiateItems.Count, m_Datas.Count);
			for (int i = 0; i < num; i++)
			{
				updateItem(i, m_InstantiateItems[i].gameObject);
			}
		}
	}

	private void Reset()
	{
		for (int i = 0; i < m_InstantiateItems.Count; i++)
		{
			m_InstantiateItems[i].gameObject.SetActive(false);
			m_oldItems.Add(m_InstantiateItems[i]);
		}
		m_InstantiateItems.Clear();
		m_PrevPos = 0f;
		m_CurrentIndex = 0;
		selectedObject = null;
		selectedItem = null;
		GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
	}

	public void SetIndexToBottom(int itemIndex)
	{
		GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, CellRect.y * (float)itemIndex - m_ScrollRect.GetComponent<RectTransform>().sizeDelta.y + CellRect.y * 2f + topPadding + bottomPadding);
	}

	private void createItem(int index)
	{
		RectTransform rectTransform = null;
		if (m_oldItems.Count > 0)
		{
			rectTransform = m_oldItems[0];
			m_oldItems.Remove(rectTransform);
		}
		else
		{
			rectTransform = UnityEngine.Object.Instantiate(m_Cell);
			rectTransform.SetParent(base.transform, false);
			rectTransform.anchorMax = Vector2.up;
			rectTransform.anchorMin = Vector2.up;
			rectTransform.pivot = Vector2.up;
		}
		rectTransform.name = "item" + index;
		rectTransform.anchoredPosition = getPosByIndex(index);
		m_InstantiateItems.Add(rectTransform);
		rectTransform.gameObject.SetActive(true);
	}

	private void removeItem(int index)
	{
		RectTransform rectTransform = m_InstantiateItems[index];
		m_InstantiateItems.Remove(rectTransform);
		rectTransform.gameObject.SetActive(false);
		m_oldItems.Add(rectTransform);
	}

	private Vector2 getRectByNum(int num)
	{
		if (direction != 0)
		{
			return new Vector2(Mathf.CeilToInt((float)num / m_Page.y), m_Page.y);
		}
		return new Vector2(m_Page.x, Mathf.CeilToInt((float)num / m_Page.x));
	}

	private void setBound(Vector2 bound)
	{
		m_Rect.sizeDelta = new Vector2(bound.y * CellRect.x, bound.x * CellRect.y + bottomPadding + topPadding);
	}

	private void Update()
	{
		if (isFirst)
		{
			isFirst = false;
			return;
		}
		while (scale * DirectionPos - m_PrevPos < (0f - CellScale) * 2f)
		{
			if (m_PrevPos <= 0f - MaxPrevPos)
			{
				return;
			}
			m_PrevPos -= CellScale;
			List<RectTransform> range = m_InstantiateItems.GetRange(0, PageScale);
			m_InstantiateItems.RemoveRange(0, PageScale);
			m_InstantiateItems.AddRange(range);
			for (int i = 0; i < range.Count; i++)
			{
				moveItemToIndex(m_CurrentIndex * PageScale + m_InstantiateItems.Count + i, range[i]);
			}
			m_CurrentIndex++;
		}
		while (scale * DirectionPos - m_PrevPos > 0f - CellScale && Mathf.RoundToInt(m_PrevPos) < 0)
		{
			m_PrevPos += CellScale;
			m_CurrentIndex--;
			if (m_CurrentIndex < 0)
			{
				break;
			}
			List<RectTransform> range2 = m_InstantiateItems.GetRange(m_InstantiateItems.Count - PageScale, PageScale);
			m_InstantiateItems.RemoveRange(m_InstantiateItems.Count - PageScale, PageScale);
			m_InstantiateItems.InsertRange(0, range2);
			for (int j = 0; j < range2.Count; j++)
			{
				moveItemToIndex(m_CurrentIndex * PageScale + j, range2[j]);
			}
		}
	}

	private void moveItemToIndex(int index, RectTransform item)
	{
		item.anchoredPosition = getPosByIndex(index);
		updateItem(index, item.gameObject);
	}

	private Vector2 getPosByIndex(int index)
	{
		if (direction != 0)
		{
			return new Vector2((float)index % InstantiateSize.y * CellRect.x, (0f - Mathf.Floor((float)index / InstantiateSize.y)) * CellRect.y - topPadding);
		}
		return new Vector2(Mathf.Floor((float)index / InstantiateSize.x) * CellRect.x, (0f - (float)index % InstantiateSize.x) * CellRect.y);
	}

	private void updateItem(int index, GameObject item)
	{
		item.SetActive(index < m_Datas.Count);
		if (!item.activeSelf)
		{
			return;
		}
		UILoopItem component = item.GetComponent<UILoopItem>();
		component.UpdateItem(index, item);
		component.Data(m_Datas[index]);
		if (selectedObject == m_Datas[index])
		{
			component.SetSelected(true);
		}
		else
		{
			component.SetSelected(false);
		}
		if (!(component.GetComponent<Button>() != null) || onSelectedEvent == null || addClickEventList.IndexOf(component.GetComponent<Button>()) >= 0)
		{
			return;
		}
		addClickEventList.Add(component.GetComponent<Button>());
		component.GetComponent<Button>().onClick.AddListener(delegate
		{
			if (onSelectedEvent != null)
			{
				if (selectedItem != null && selectedItem != item.GetComponent<UILoopItem>())
				{
					selectedItem.SetSelected(false);
				}
				selectedItem = item.GetComponent<UILoopItem>();
				selectedObject = selectedItem.GetData();
				selectedItem.SetSelected(true);
				onSelectedEvent(selectedItem);
			}
		});
	}

	public object GetSelectedData()
	{
		return selectedObject;
	}

	public void SetSelectedIndex(int selectedIndex)
	{
		if (m_Datas.Count <= 0 || m_Datas.Count <= selectedIndex)
		{
			return;
		}
		selectedObject = m_Datas[selectedIndex];
		if (selectedItem != null)
		{
			selectedItem.SetSelected(false);
		}
		for (int i = 0; i < m_InstantiateItems.Count; i++)
		{
			if (selectedObject == m_InstantiateItems[i].GetComponent<UILoopItem>().GetData())
			{
				m_InstantiateItems[i].GetComponent<UILoopItem>().SetSelected(true);
				selectedItem = m_InstantiateItems[i].GetComponent<UILoopItem>();
			}
		}
	}
}
