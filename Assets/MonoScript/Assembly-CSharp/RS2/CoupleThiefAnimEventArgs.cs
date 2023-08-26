using Foundation;

namespace RS2
{
	public sealed class CoupleThiefAnimEventArgs : EventArgs<CoupleThiefAnimEventArgs>
	{
		public CoupleAnimReceiverType Receiver { get; private set; }

		public int AnimType { get; private set; }

		public CoupleThiefAnimEventArgs Initialize(int animType, CoupleAnimReceiverType receiver)
		{
			AnimType = animType;
			Receiver = receiver;
			return this;
		}

		protected override void OnRecycle()
		{
			AnimType = 0;
			Receiver = CoupleAnimReceiverType.eDefault;
		}
	}
}
