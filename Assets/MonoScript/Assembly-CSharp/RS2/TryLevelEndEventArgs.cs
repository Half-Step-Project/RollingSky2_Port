using Foundation;

namespace RS2
{
	public sealed class TryLevelEndEventArgs : EventArgs<TryLevelEndEventArgs>
	{
		public TryLevelEndEventArgs Initialize()
		{
			return this;
		}

		protected override void OnRecycle()
		{
		}
	}
}
