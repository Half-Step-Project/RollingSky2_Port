using Foundation;

namespace RS2
{
	public class DropDieForwordTrigger : BaseTriggerBox
	{
		public override void TriggerEnter(BaseRole ball)
		{
			Mod.Event.FireNow(this, Mod.Reference.Acquire<RoleDropDieMoveEventArgs>().Initialize(false));
		}
	}
}
