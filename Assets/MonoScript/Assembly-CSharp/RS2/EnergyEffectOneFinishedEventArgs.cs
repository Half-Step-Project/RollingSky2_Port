using Foundation;

namespace RS2
{
	public sealed class EnergyEffectOneFinishedEventArgs : EventArgs<EnergyEffectOneFinishedEventArgs>
	{
		public EnergyEffectOneFinishedEventArgs Initialize()
		{
			return this;
		}

		protected override void OnRecycle()
		{
		}
	}
}
