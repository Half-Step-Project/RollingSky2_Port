public class EnableInputTrigger : BaseTriggerBox
{
	public override bool IfRebirthRecord
	{
		get
		{
			return false;
		}
	}

	public override void TriggerEnter(BaseRole ball)
	{
		InputController.instance.EnableInput(true);
	}
}
