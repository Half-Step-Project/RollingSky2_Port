using Foundation;
using RS2;
using UnityEngine.UI;

public class ShopUIPackageTitleItem : UITitleItem
{
	public Text m_name;

	public Text m_message;

	public Image m_background0;

	public Image m_background1;

	private ShopType m_shopType;

	public override void OnInit(object TitleData)
	{
		m_shopType = (ShopType)TitleData;
	}

	public override void OnOpen()
	{
		SetData(m_shopType);
	}

	private void SetData(ShopType shopType)
	{
		m_background0.gameObject.SetActive(false);
		m_background1.gameObject.SetActive(false);
		switch (shopType)
		{
		case ShopType.BASICPACKAGE:
			m_background0.gameObject.SetActive(true);
			SetData(Mod.Localization.GetInfoById(88), "");
			break;
		case ShopType.ABILITYPACKAGE:
			m_background1.gameObject.SetActive(true);
			SetData(Mod.Localization.GetInfoById(89), "");
			break;
		}
	}

	public override void OnClose()
	{
	}

	public override void OnRelease()
	{
	}

	public override void OnRefresh()
	{
		SetData(m_shopType);
	}

	private void SetData(string name, string message)
	{
		if (m_name != null)
		{
			m_name.text = name;
		}
		if (m_message != null)
		{
			m_message.text = message;
		}
	}
}
