using Foundation;

namespace RS2
{
	public sealed class WindOpenEventArgs : EventArgs<WindOpenEventArgs>
	{
		public WindOpenTrigger.WindData m_windData;

		public WindOpenEventArgs Initialize(WindOpenTrigger.WindData windData)
		{
			m_windData = windData;
			return this;
		}

		protected override void OnRecycle()
		{
		}
	}
}
