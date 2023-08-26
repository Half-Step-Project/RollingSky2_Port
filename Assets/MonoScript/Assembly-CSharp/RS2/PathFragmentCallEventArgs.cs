using Foundation;

namespace RS2
{
	public sealed class PathFragmentCallEventArgs : EventArgs<PathFragmentCallEventArgs>
	{
		public PathFragmentCallEventArgs Initialize()
		{
			return this;
		}

		protected override void OnRecycle()
		{
		}
	}
}
