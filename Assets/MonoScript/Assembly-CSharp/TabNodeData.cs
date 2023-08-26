using UnityEngine;

public class TabNodeData
{
	public int m_index;

	public GameObject m_tabButton;

	public NodeView m_nodeView;

	public GameObject m_defaultObject;

	public TabNodeData(int index, GameObject tabButton, NodeView nodeView, GameObject defaultPanel = null)
	{
		m_index = index;
		m_tabButton = tabButton;
		m_defaultObject = defaultPanel;
		m_nodeView = nodeView;
	}
}
