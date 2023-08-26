using Foundation;

namespace RS2
{
	public class PointLightTrigger : BaseTriggerBox
	{
		public bool m_open;

		public override bool IfRebirthRecord
		{
			get
			{
				return false;
			}
		}

		public override void TriggerEnter(BaseRole ball)
		{
			Mod.Event.Fire(this, Mod.Reference.Acquire<PointLightChangeEventArgs>().Initialize(m_open));
		}
	}
}
