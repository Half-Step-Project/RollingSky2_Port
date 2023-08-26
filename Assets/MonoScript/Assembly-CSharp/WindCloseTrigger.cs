using Foundation;
using RS2;

public class WindCloseTrigger : BaseTriggerBox
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
		base.TriggerEnter(ball);
		Mod.Event.FireNow(this, Mod.Reference.Acquire<WindCloseEventArgs>().Initialize());
	}

	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
	}
}
