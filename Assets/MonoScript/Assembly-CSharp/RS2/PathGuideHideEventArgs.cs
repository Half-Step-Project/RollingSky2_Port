using Foundation;

namespace RS2
{
	public sealed class PathGuideHideEventArgs : EventArgs<PathGuideHideEventArgs>
	{
		public PathGuideHideEventArgs Initialize()
		{
			return this;
		}

		protected override void OnRecycle()
		{
		}
	}
}
