using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.UI;

public class ShopUITabItem : MonoBehaviour
{
	public Image m_normalImage;

	public Image m_highlightedImage;

	public Text m_name;

	[HideInInspector]
	public int m_index;

	public void OnSelection(bool isSelect)
	{
		if (isSelect)
		{
			if (m_normalImage != null)
			{
				m_normalImage.gameObject.SetActive(true);
			}
			if (m_highlightedImage != null)
			{
				m_highlightedImage.gameObject.SetActive(false);
			}
		}
		else
		{
			if (m_normalImage != null)
			{
				m_normalImage.gameObject.SetActive(false);
			}
			if (m_highlightedImage != null)
			{
				m_highlightedImage.gameObject.SetActive(true);
			}
		}
	}

	public void OnOpen()
	{
		OnRefreshLanuage();
	}

	public void OnRefreshLanuage()
	{
		string text = string.Empty;
		switch (m_index)
		{
		case 0:
			text = Mod.Localization.GetInfoById(10014);
			break;
		case 1:
			text = Mod.Localization.GetInfoById(10015);
			break;
		}
		if (m_name != null)
		{
			m_name.text = text;
		}
	}
}
