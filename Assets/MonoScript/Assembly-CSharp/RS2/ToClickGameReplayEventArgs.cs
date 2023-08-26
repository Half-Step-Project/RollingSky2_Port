using Foundation;

namespace RS2
{
	public sealed class ToClickGameReplayEventArgs : EventArgs<ToClickGameReplayEventArgs>
	{
		public ToClickGameReplayEventArgs Initialize()
		{
			return this;
		}

		protected override void OnRecycle()
		{
		}
	}
}
