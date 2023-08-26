using Foundation;

namespace RS2
{
	public class EnterLevelArgs : EventArgs<EnterLevelArgs>
	{
		public EnterLevelArgs Initialize()
		{
			return this;
		}

		protected override void OnRecycle()
		{
		}
	}
}
