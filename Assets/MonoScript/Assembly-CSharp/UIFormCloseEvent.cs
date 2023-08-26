using Foundation;
using RS2;

public class UIFormCloseEvent : EventArgs<UIFormCloseEvent>
{
	public UIFormId UIFormId { get; set; }

	public static UIFormCloseEvent Make(UIFormId uiFormId)
	{
		UIFormCloseEvent uIFormCloseEvent = Mod.Reference.Acquire<UIFormCloseEvent>();
		uIFormCloseEvent.UIFormId = uiFormId;
		return uIFormCloseEvent;
	}

	protected override void OnRecycle()
	{
	}
}
