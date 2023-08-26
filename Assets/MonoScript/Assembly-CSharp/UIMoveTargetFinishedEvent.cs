using Foundation;

public class UIMoveTargetFinishedEvent : EventArgs<UIMoveTargetFinishedEvent>
{
	public int GoodId { get; set; }

	public static UIMoveTargetFinishedEvent Make(int goodId)
	{
		UIMoveTargetFinishedEvent uIMoveTargetFinishedEvent = Mod.Reference.Acquire<UIMoveTargetFinishedEvent>();
		uIMoveTargetFinishedEvent.GoodId = goodId;
		return uIMoveTargetFinishedEvent;
	}

	protected override void OnRecycle()
	{
	}
}
