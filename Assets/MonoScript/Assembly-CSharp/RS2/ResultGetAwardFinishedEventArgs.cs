using Foundation;

namespace RS2
{
	public sealed class ResultGetAwardFinishedEventArgs : EventArgs<ResultGetAwardFinishedEventArgs>
	{
		public ResultGetAwardFinishedEventArgs Initialize()
		{
			return this;
		}

		protected override void OnRecycle()
		{
		}
	}
}
