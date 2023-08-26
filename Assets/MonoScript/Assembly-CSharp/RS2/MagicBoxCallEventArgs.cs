using Foundation;

namespace RS2
{
	public sealed class MagicBoxCallEventArgs : EventArgs<MagicBoxCallEventArgs>
	{
		public MagicBoxData MagicBoxInfo { get; private set; }

		public MagicBoxCallEventArgs Initialize(MagicBoxData magicBoxData)
		{
			MagicBoxInfo = magicBoxData;
			return this;
		}

		protected override void OnRecycle()
		{
			MagicBoxInfo = null;
		}
	}
}
