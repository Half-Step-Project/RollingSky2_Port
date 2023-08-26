using Foundation;

namespace RS2
{
	public sealed class ToClickGameStartEventArgs : EventArgs<ToClickGameStartEventArgs>
	{
		public ToClickGameStartEventArgs Initialize()
		{
			return this;
		}

		protected override void OnRecycle()
		{
		}
	}
}
