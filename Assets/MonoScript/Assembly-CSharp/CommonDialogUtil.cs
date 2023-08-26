using System;
using Foundation;
using RS2;

public class CommonDialogUtil
{
	public enum ShowType
	{
		YesNo,
		Yes
	}

	public enum ShowStyle
	{
		IN_MENU,
		IN_GAME
	}

	private CommonDialogForm m_CommonDialogForm;

	private static CommonDialogUtil m_Instance;

	private string m_Message = "";

	private ShowType m_ShowType;

	private string m_YesStr;

	private string m_NoStr;

	private System.Action m_OnYes;

	private System.Action m_OnNo;

	private ShowStyle m_Style;

	public bool onShow;

	public static CommonDialogUtil Instance
	{
		get
		{
			if (m_Instance == null)
			{
				m_Instance = new CommonDialogUtil();
			}
			return m_Instance;
		}
	}

	private CommonDialogUtil()
	{
	}

	public void ShowDialog(string message, ShowType showType, string yesStr, string noStr, System.Action onYes, System.Action onNo, ShowStyle style = ShowStyle.IN_MENU)
	{
		m_Message = message;
		m_ShowType = showType;
		m_YesStr = yesStr;
		m_NoStr = noStr;
		m_OnYes = onYes;
		m_OnNo = onNo;
		m_Style = style;
		Mod.Event.Subscribe(EventArgs<UIMod.OpenSuccessEventArgs>.EventId, OnOpenUIFormSuccess);
		onShow = true;
		Mod.UI.OpenUIForm(UIFormId.CommonDialogForm);
	}

	public bool IsOpenDialog()
	{
		onShow = true;
		return Mod.UI.UIFormIsOpen(UIFormId.CommonDialogForm);
	}

	public void CloseDialog()
	{
		onShow = false;
		Mod.UI.CloseUIForm(UIFormId.CommonDialogForm);
		Mod.Event.Unsubscribe(EventArgs<UIMod.OpenSuccessEventArgs>.EventId, OnOpenUIFormSuccess);
		Clear();
	}

	private void OnOpenUIFormSuccess(object sender, Foundation.EventArgs e)
	{
		UIMod.OpenSuccessEventArgs openSuccessEventArgs = (UIMod.OpenSuccessEventArgs)e;
		m_CommonDialogForm = openSuccessEventArgs.UIForm.Logic as CommonDialogForm;
		if (m_CommonDialogForm != null)
		{
			m_CommonDialogForm.GetController().BindData(m_Message, m_ShowType, m_YesStr, m_NoStr, m_OnYes, m_OnNo, m_Style);
		}
	}

	private void Clear()
	{
		m_Message = "";
		m_ShowType = ShowType.YesNo;
		m_YesStr = "";
		m_NoStr = "";
		m_OnYes = null;
		m_OnNo = null;
		m_Style = ShowStyle.IN_MENU;
	}
}
