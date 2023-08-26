using Foundation;

namespace RS2
{
	public sealed class PropsTriggerEventArgs : EventArgs<PropsTriggerEventArgs>
	{
		public PropsName m_propsName;

		public PropsTriggerEventArgs Initialize(PropsName propsName)
		{
			m_propsName = propsName;
			return this;
		}

		protected override void OnRecycle()
		{
		}
	}
}
