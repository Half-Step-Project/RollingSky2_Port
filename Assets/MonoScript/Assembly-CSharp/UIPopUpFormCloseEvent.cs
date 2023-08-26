using Foundation;
using RS2;

public class UIPopUpFormCloseEvent : EventArgs<UIPopUpFormCloseEvent>
{
	public UIFormId UIFormId { get; set; }

	public static UIPopUpFormCloseEvent Make(UIFormId uiFormId)
	{
		UIPopUpFormCloseEvent uIPopUpFormCloseEvent = Mod.Reference.Acquire<UIPopUpFormCloseEvent>();
		uIPopUpFormCloseEvent.UIFormId = uiFormId;
		return uIPopUpFormCloseEvent;
	}

	protected override void OnRecycle()
	{
	}
}
