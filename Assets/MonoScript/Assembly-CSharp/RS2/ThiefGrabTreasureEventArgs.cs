using Foundation;

namespace RS2
{
	public sealed class ThiefGrabTreasureEventArgs : EventArgs<ThiefGrabTreasureEventArgs>
	{
		public bool IfGrab;

		protected override void OnRecycle()
		{
			IfGrab = true;
		}

		public ThiefGrabTreasureEventArgs Initialize(bool ifGrab)
		{
			IfGrab = ifGrab;
			return this;
		}
	}
}
