using Foundation;

namespace RS2
{
	public sealed class ClickGameStartButtonEventArgs : EventArgs<ClickGameStartButtonEventArgs>
	{
		public ClickGameStartButtonEventArgs Initialize()
		{
			return this;
		}

		protected override void OnRecycle()
		{
		}
	}
}
