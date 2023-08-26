using Foundation;

namespace RS2
{
	public sealed class MenuViewOpenItemEventArgs : EventArgs<MenuViewOpenItemEventArgs>
	{
		public int Index { get; private set; }

		public MenuViewOpenItemEventArgs Initialize(int index)
		{
			Index = index;
			return this;
		}

		protected override void OnRecycle()
		{
		}
	}
}
