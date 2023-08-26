using Foundation;
using RS2;
using UnityEngine.UI;

public class ShopUIGroupTitleItem : UITitleItem
{
	public Text m_name;

	public Text m_message;

	public Image m_background;

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
		switch (shopType)
		{
		case ShopType.POWER:
			SetData(Mod.Localization.GetInfoById(90), "");
			break;
		case ShopType.REBIRTH:
			SetData(Mod.Localization.GetInfoById(91), "");
			break;
		case ShopType.BASICPACKAGE:
			SetData(Mod.Localization.GetInfoById(88), "");
			break;
		case ShopType.ABILITYPACKAGE:
			SetData(Mod.Localization.GetInfoById(89), "");
			break;
		case ShopType.REMOVEAD:
			SetData(Mod.Localization.GetInfoById(20007), "");
			break;
		case ShopType.HOURGLASSPROP:
			SetData(Mod.Localization.GetInfoById(178), "");
			break;
		case ShopType.ABILITYPROP:
			SetData(Mod.Localization.GetInfoById(179), "");
			break;
		case ShopType.GuideLine:
			SetData(Mod.Localization.GetInfoById(205), "");
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
