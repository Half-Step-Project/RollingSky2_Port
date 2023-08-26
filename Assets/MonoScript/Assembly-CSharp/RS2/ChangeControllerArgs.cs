using Foundation;

namespace RS2
{
	public class ChangeControllerArgs : EventArgs<ChangeControllerArgs>
	{
		public ChangeControllerArgs Initialize()
		{
			return this;
		}

		protected override void OnRecycle()
		{
		}
	}
}
