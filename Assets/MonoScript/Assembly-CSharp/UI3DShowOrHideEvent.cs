using Foundation;

public class UI3DShowOrHideEvent : EventArgs<UI3DShowOrHideEvent>
{
	public bool IsShow { get; set; }

	public static UI3DShowOrHideEvent Make(bool isShow)
	{
		UI3DShowOrHideEvent uI3DShowOrHideEvent = Mod.Reference.Acquire<UI3DShowOrHideEvent>();
		uI3DShowOrHideEvent.IsShow = isShow;
		return uI3DShowOrHideEvent;
	}

	protected override void OnRecycle()
	{
	}
}
