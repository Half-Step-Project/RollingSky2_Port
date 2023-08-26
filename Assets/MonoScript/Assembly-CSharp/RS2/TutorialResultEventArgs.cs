using Foundation;

namespace RS2
{
	public sealed class TutorialResultEventArgs : EventArgs<TutorialResultEventArgs>
	{
		public TutorialResultEventArgs Initialize()
		{
			return this;
		}

		protected override void OnRecycle()
		{
		}
	}
}
