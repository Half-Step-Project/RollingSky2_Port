using Foundation;

namespace RS2
{
	public sealed class PropsRemoveAllEventArgs : EventArgs<PropsRemoveAllEventArgs>
	{
		public PropsRemoveAllEventArgs Initialize()
		{
			return this;
		}

		protected override void OnRecycle()
		{
		}
	}
}
