using Foundation;

namespace RS2
{
	public class SuperMusicTrigger : BaseTriggerBox
	{
		public override void TriggerEnter(BaseRole ball)
		{
			Mod.Event.Fire(this, Mod.Reference.Acquire<GamePlaySuperMusicEventArgs>().Initialize());
		}
	}
}
