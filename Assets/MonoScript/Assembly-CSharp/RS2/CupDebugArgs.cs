using Foundation;

namespace RS2
{
	public class CupDebugArgs : EventArgs<CupDebugArgs>
	{
		public int cupState;

		public CupDebugArgs Initialize(int cupState)
		{
			this.cupState = cupState;
			return this;
		}

		protected override void OnRecycle()
		{
		}
	}
}
