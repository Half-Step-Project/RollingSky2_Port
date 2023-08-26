using Foundation;

namespace RS2
{
	public sealed class GainedDropEventArgs : EventArgs<GainedDropEventArgs>
	{
		public DropData m_dropData;

		public GainedDropEventArgs Initialize(DropData dropData)
		{
			m_dropData = dropData;
			return this;
		}

		protected override void OnRecycle()
		{
		}
	}
}
