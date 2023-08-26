using Foundation;

namespace RS2
{
	public class CoupleDetachEventArgs : EventArgs<CoupleDetachEventArgs>
	{
		public CoupleDetachEventArgs Initialize()
		{
			return this;
		}

		protected override void OnRecycle()
		{
		}
	}
}
