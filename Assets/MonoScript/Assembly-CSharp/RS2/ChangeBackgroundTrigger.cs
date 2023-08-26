using Foundation;

namespace RS2
{
	public class ChangeBackgroundTrigger : BaseTriggerBox
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
			Mod.Event.FireNow(this, Mod.Reference.Acquire<BackgroundChangeEventArgs>().Initialize());
		}
	}
}
