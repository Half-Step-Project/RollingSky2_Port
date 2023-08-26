using Foundation;
using RS2;
using UnityEngine.UI;

public class ShopUINullProductItem : UIProductItem
{
	public Text m_text;

	public override void OnInit(object data)
	{
		OnRelease();
	}

	public override void OnOpen()
	{
	}

	public override void OnClose()
	{
	}

	public override void OnRefresh()
	{
	}

	public override void OnRelease()
	{
		OnRefreshLanguage();
	}

	private void OnRefreshLanguage()
	{
		m_text.text = Mod.Localization.GetInfoById(102);
	}
}
