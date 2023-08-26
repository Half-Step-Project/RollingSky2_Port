using Foundation;

namespace RS2
{
	public sealed class BackgroundChangeEventArgs : EventArgs<BackgroundChangeEventArgs>
	{
		public BackgroundChangeEventArgs Initialize()
		{
			return this;
		}

		protected override void OnRecycle()
		{
		}
	}
}
