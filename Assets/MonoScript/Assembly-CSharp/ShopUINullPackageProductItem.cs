using Foundation;
using RS2;
using UnityEngine.UI;

public class ShopUINullPackageProductItem : UIProductItem
{
	public Text m_text;

	public override void OnInit(object data)
	{
		OnRefresh();
	}

	public override void OnOpen()
	{
		OnRefresh();
	}

	public override void OnClose()
	{
	}

	public override void OnRelease()
	{
	}

	public override void OnRefresh()
	{
		OnRefreshLanguage();
	}

	private void OnRefreshLanguage()
	{
		m_text.text = Mod.Localization.GetInfoById(102);
	}
}
