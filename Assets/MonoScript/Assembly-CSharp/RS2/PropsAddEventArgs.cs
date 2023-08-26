using Foundation;

namespace RS2
{
	public sealed class PropsAddEventArgs : EventArgs<PropsAddEventArgs>
	{
		public PropsName m_propsName;

		public PropsAddEventArgs Initialize(PropsName propsName)
		{
			m_propsName = propsName;
			return this;
		}

		protected override void OnRecycle()
		{
		}
	}
}
