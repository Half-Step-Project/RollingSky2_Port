using Foundation;

namespace RS2
{
	public class ChangeBackgroundEventArgs : EventArgs<ChangeBackgroundEventArgs>
	{
		private const int DefaultIndex = -1;

		public int BackItemIndex = -1;

		public ChangeBackgroundEventArgs Initialize(int index = -1)
		{
			BackItemIndex = index;
			return this;
		}

		protected override void OnRecycle()
		{
			BackItemIndex = -1;
		}
	}
}
