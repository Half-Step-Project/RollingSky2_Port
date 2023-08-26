using Foundation;

namespace RS2
{
	public sealed class InstantPropsEventArgs : EventArgs<InstantPropsEventArgs>
	{
		public InstantPropsEventArgs Initialize()
		{
			return this;
		}

		protected override void OnRecycle()
		{
		}
	}
}
