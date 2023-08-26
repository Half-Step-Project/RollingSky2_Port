using Foundation;

namespace RS2
{
	public sealed class AnimFreeCollideCallEventArgs : EventArgs<AnimFreeCollideCallEventArgs>
	{
		public FreeCollideData FreeCollideInfo { get; private set; }

		public AnimFreeCollideCallEventArgs Initialize(FreeCollideData collideData)
		{
			FreeCollideInfo = collideData;
			return this;
		}

		protected override void OnRecycle()
		{
			FreeCollideInfo = null;
		}
	}
}
