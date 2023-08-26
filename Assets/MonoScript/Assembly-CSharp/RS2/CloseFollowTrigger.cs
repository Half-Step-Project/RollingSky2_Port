using Foundation;

namespace RS2
{
	public class CloseFollowTrigger : BaseTriggerBox
	{
		public override bool IfRebirthRecord
		{
			get
			{
				return false;
			}
		}

		public override void Initialize()
		{
			base.Initialize();
		}

		public override void TriggerEnter(BaseRole ball)
		{
			Mod.Event.FireNow(this, Mod.Reference.Acquire<FollowCloseEventArgs>().Initialize());
		}
	}
}
