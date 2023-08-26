using Foundation;

namespace RS2
{
	public class CoupleDetachTrigger : BaseTriggerBox
	{
		public override void TriggerEnter(BaseRole ball)
		{
			Mod.Event.Fire(this, Mod.Reference.Acquire<CoupleDetachEventArgs>().Initialize());
		}
	}
}
