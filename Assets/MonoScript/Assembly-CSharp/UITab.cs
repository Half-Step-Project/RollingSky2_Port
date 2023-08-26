using UnityEngine;
using UnityEngine.UI;

public sealed class UITab : MonoBehaviour
{
	public delegate void OnTabRange(GameObject[] objs, int index, bool isSooth);

	public OnTabRange m_onTabRange;

	public Image m_backGround;

	public GameObject[] m_tabNodes;

	private int m_currentIndex;

	public bool m_isNeedStart = true;

	public int m_startIndex;

	public bool m_isPlayTweener;

	public int CurrentIndex
	{
		get
		{
			return m_currentIndex;
		}
	}

	private void Start()
	{
		for (int i = 0; i < m_tabNodes.Length; i++)
		{
			int _index = i;
			if (m_tabNodes[i] == null)
			{
				Debug.LogError("Tab node = null ---->tab Name :" + base.gameObject.name + ", index :" + i);
				break;
			}
			EventTriggerListener.Get(m_tabNodes[i]).onClick = delegate(GameObject x)
			{
				OnClickNode(x, _index);
			};
		}
		if (m_isNeedStart)
		{
			SetTabIndex(m_startIndex, false);
		}
	}

	private void OnClickNode(GameObject obj, int index)
	{
		if (m_currentIndex != index)
		{
			SetTabIndex(index);
		}
	}

	private void TweenPlay(int index, bool isSmooth)
	{
	}

	public Rect GetTabSize()
	{
		if (m_backGround != null)
		{
			return m_backGround.rectTransform.rect;
		}
		return Rect.zero;
	}

	public void SetTabIndex(int index, bool isSmooth = true)
	{
		if (index < m_tabNodes.Length && m_tabNodes[index] != null)
		{
			if (m_onTabRange != null)
			{
				m_onTabRange(m_tabNodes, index, isSmooth);
			}
			if (m_isPlayTweener)
			{
				TweenPlay(index, isSmooth);
			}
			m_currentIndex = index;
		}
	}

	public static UITab Builder(GameObject obj, Image background, GameObject[] tabNodes)
	{
		if (obj == null || background == null || tabNodes == null)
		{
			return null;
		}
		UITab uITab = obj.GetComponent<UITab>();
		if (uITab == null)
		{
			uITab = obj.AddComponent<UITab>();
		}
		uITab.m_backGround = background;
		uITab.m_tabNodes = tabNodes;
		return uITab;
	}
}
