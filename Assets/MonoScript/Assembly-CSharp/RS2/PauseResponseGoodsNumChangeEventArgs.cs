using Foundation;

namespace RS2
{
	public sealed class PauseResponseGoodsNumChangeEventArgs : EventArgs<PauseResponseGoodsNumChangeEventArgs>
	{
		public bool IsPause { get; private set; }

		public PauseResponseGoodsNumChangeEventArgs Initialize(bool isPause)
		{
			IsPause = isPause;
			return this;
		}

		protected override void OnRecycle()
		{
		}
	}
}
