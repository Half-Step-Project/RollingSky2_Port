using Foundation;

namespace RS2
{
	public sealed class OpenTutorialVideoEventArgs : EventArgs<OpenTutorialVideoEventArgs>
	{
		public OpenTutorialVideoEventArgs Initialize()
		{
			return this;
		}

		protected override void OnRecycle()
		{
		}
	}
}
