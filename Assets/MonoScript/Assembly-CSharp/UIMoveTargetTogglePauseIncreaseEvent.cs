using Foundation;

public class UIMoveTargetTogglePauseIncreaseEvent : EventArgs<UIMoveTargetTogglePauseIncreaseEvent>
{
	public static UIMoveTargetTogglePauseIncreaseEvent Make()
	{
		return Mod.Reference.Acquire<UIMoveTargetTogglePauseIncreaseEvent>();
	}

	protected override void OnRecycle()
	{
	}
}
