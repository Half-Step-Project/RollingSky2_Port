using Foundation;

namespace RS2
{
	public class SwitchEventArgs : EventArgs<SwitchEventArgs>
	{
		public SwitchSendTrigger.Data m_data;

		public SwitchEventArgs Initialize(SwitchSendTrigger.Data data)
		{
			m_data = data;
			return this;
		}

		protected override void OnRecycle()
		{
		}
	}
}
