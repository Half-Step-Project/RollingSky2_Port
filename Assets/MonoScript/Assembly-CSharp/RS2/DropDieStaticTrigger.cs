using Foundation;

namespace RS2
{
	public class DropDieStaticTrigger : BaseTriggerBox
	{
		public override void TriggerEnter(BaseRole ball)
		{
			Mod.Event.FireNow(this, Mod.Reference.Acquire<RoleDropDieMoveEventArgs>().Initialize(true));
		}

		public override void RebirthReadByteData(byte[] rd_data)
		{
		}

		public override byte[] RebirthWriteByteData()
		{
			return null;
		}

		public override void RebirthStartByteGame(byte[] rd_data)
		{
		}
	}
}
