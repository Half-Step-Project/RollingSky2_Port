using Foundation;

namespace RS2
{
	public sealed class EnergyEffectFinishedEventArgs : EventArgs<EnergyEffectFinishedEventArgs>
	{
		public EnergyEffectFinishedEventArgs Initialize()
		{
			return this;
		}

		protected override void OnRecycle()
		{
		}
	}
}
