using Foundation;

namespace RS2
{
	public sealed class FollowCloseEventArgs : EventArgs<FollowCloseEventArgs>
	{
		public FollowCloseEventArgs Initialize()
		{
			return this;
		}

		protected override void OnRecycle()
		{
		}
	}
}
