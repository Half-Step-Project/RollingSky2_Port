using UnityEngine;

public class ShopUITabList : MonoBehaviour
{
	public ShopUITabItem[] m_tabItems;

	public GameObject[] m_objects;

	public int m_currentIndex;

	private void SetOnClickTabButtonEvent()
	{
		for (int i = 0; i < m_tabItems.Length; i++)
		{
			if (m_tabItems != null)
			{
				int _index = i;
				m_tabItems[i].m_index = _index;
				EventTriggerListener.Get(m_tabItems[i].gameObject).onClick = delegate
				{
					OnClickTabButton(m_tabItems[_index]);
				};
			}
		}
	}

	private void SetSelectIndex(int index)
	{
		if (index >= m_tabItems.Length || (!(m_tabItems[index] != null) && !(m_objects[index] != null)))
		{
			return;
		}
		for (int i = 0; i < m_tabItems.Length; i++)
		{
			if (m_objects[i] != null && m_tabItems[i] != null)
			{
				if (i == index)
				{
					m_tabItems[i].OnSelection(true);
					m_objects[i].SetActive(true);
					m_currentIndex = index;
				}
				else
				{
					m_tabItems[i].OnSelection(false);
					m_objects[i].SetActive(false);
					m_currentIndex = index;
				}
			}
		}
	}

	private void OnClickTabButton(ShopUITabItem item)
	{
		SetSelectIndex(item.m_index);
	}

	public void OnInit()
	{
		SetOnClickTabButtonEvent();
	}

	public void OnOpen(ShopUITabItemData tabData)
	{
		for (int i = 0; i < m_tabItems.Length; i++)
		{
			m_tabItems[i].OnOpen();
		}
		SetSelectIndex(tabData.m_selectIndex);
	}

	public void OnClose()
	{
	}

	public void OnRelease()
	{
	}
}
