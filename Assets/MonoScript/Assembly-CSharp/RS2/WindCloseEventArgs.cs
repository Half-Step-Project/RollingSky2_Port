using Foundation;

namespace RS2
{
	public sealed class WindCloseEventArgs : EventArgs<WindCloseEventArgs>
	{
		public WindCloseEventArgs Initialize()
		{
			return this;
		}

		protected override void OnRecycle()
		{
		}
	}
}
