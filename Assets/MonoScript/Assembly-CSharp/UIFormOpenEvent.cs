using Foundation;
using RS2;

public class UIFormOpenEvent : EventArgs<UIFormOpenEvent>
{
	public UIFormId UIFormId { get; set; }

	public static UIFormOpenEvent Make(UIFormId uiFormId)
	{
		UIFormOpenEvent uIFormOpenEvent = Mod.Reference.Acquire<UIFormOpenEvent>();
		uIFormOpenEvent.UIFormId = uiFormId;
		return uIFormOpenEvent;
	}

	protected override void OnRecycle()
	{
	}
}
