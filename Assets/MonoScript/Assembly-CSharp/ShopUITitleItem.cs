using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.UI;

public class ShopUITitleItem : UITitleItem
{
	public Text m_name;

	public Text m_message;

	private ShopType m_shopType;

	public override void OnInit(object TitleData)
	{
		m_shopType = (ShopType)TitleData;
		Debug.Log(m_shopType);
	}

	public override void OnOpen()
	{
		Debug.Log(m_shopType);
		SetData(m_shopType);
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

	private void SetData(ShopType shopType)
	{
		int num = 0;
		int num2 = 0;
		switch (shopType)
		{
		case ShopType.POWER:
			num = 90;
			break;
		case ShopType.REBIRTH:
			num = 91;
			break;
		}
		string text = string.Empty;
		string message = string.Empty;
		if (num != 0)
		{
			text = Mod.Localization.GetInfoById(num);
		}
		if (num2 != 0)
		{
			message = Mod.Localization.GetInfoById(num2);
		}
		SetData(text, message);
	}

	private string GetGoodNameByGoodID(int goodID)
	{
		string result = string.Empty;
		int num = 0;
		switch (goodID)
		{
		case 1:
			num = 24;
			break;
		case 2:
			num = 26;
			break;
		case 3:
			num = 28;
			break;
		case 4:
			num = 30;
			break;
		case 6:
			num = 34;
			break;
		}
		if (num != 0)
		{
			result = Mod.Localization.GetInfoById(num);
		}
		return result;
	}

	private string GetGoodMessageByGoodID(int goodID)
	{
		string result = string.Empty;
		int num = 0;
		switch (goodID)
		{
		case 1:
			num = 25;
			break;
		case 2:
			num = 27;
			break;
		case 3:
			num = 29;
			break;
		case 4:
			num = 31;
			break;
		case 6:
			num = 35;
			break;
		}
		if (num != 0)
		{
			result = Mod.Localization.GetInfoById(num);
		}
		return result;
	}
}
