using Foundation;

namespace RS2
{
	public sealed class PointLightChangeEventArgs : EventArgs<PointLightChangeEventArgs>
	{
		public bool IfOpen { get; private set; }

		public PointLightChangeEventArgs Initialize(bool ifOpen)
		{
			IfOpen = ifOpen;
			return this;
		}

		protected override void OnRecycle()
		{
			IfOpen = false;
		}
	}
}
