using Foundation;

namespace RS2
{
	public sealed class EnergyEffectAllFinishedEventArgs : EventArgs<EnergyEffectAllFinishedEventArgs>
	{
		public EnergyEffectAllFinishedEventArgs Initialize()
		{
			return this;
		}

		protected override void OnRecycle()
		{
		}
	}
}
