using Foundation;

namespace RS2
{
	public sealed class PropsRemoveEventArgs : EventArgs<PropsRemoveEventArgs>
	{
		public PropsName m_propsName;

		public PropsRemoveEventArgs Initialize(PropsName propsName)
		{
			m_propsName = propsName;
			return this;
		}

		protected override void OnRecycle()
		{
		}
	}
}
