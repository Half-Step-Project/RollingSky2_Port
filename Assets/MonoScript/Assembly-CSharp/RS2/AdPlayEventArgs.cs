using Foundation;

namespace RS2
{
	public class AdPlayEventArgs : EventArgs<AdPlayEventArgs>
	{
		public int AdState { get; private set; }

		public int AdScene { get; private set; }

		public AdPlayEventArgs Initialize(int adState, int adScene)
		{
			AdState = adState;
			AdScene = adScene;
			return this;
		}

		protected override void OnRecycle()
		{
			AdState = -1;
			AdScene = -1;
		}
	}
}
