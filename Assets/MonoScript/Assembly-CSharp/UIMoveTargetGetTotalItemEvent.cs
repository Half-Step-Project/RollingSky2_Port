using Foundation;

public class UIMoveTargetGetTotalItemEvent : EventArgs<UIMoveTargetGetTotalItemEvent>
{
	public int GoodId { get; set; }

	public static UIMoveTargetGetTotalItemEvent Make(int goodId)
	{
		UIMoveTargetGetTotalItemEvent uIMoveTargetGetTotalItemEvent = Mod.Reference.Acquire<UIMoveTargetGetTotalItemEvent>();
		uIMoveTargetGetTotalItemEvent.GoodId = goodId;
		return uIMoveTargetGetTotalItemEvent;
	}

	protected override void OnRecycle()
	{
	}
}
