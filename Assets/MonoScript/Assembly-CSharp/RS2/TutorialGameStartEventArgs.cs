using Foundation;

namespace RS2
{
	public sealed class TutorialGameStartEventArgs : EventArgs<TutorialGameStartEventArgs>
	{
		public TutorialGameStartEventArgs Initialize()
		{
			return this;
		}

		protected override void OnRecycle()
		{
		}
	}
}
