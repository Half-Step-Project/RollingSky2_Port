using Foundation;

namespace RS2
{
	public sealed class ResultBlockButtonEventArgs : EventArgs<ResultBlockButtonEventArgs>
	{
		public bool IsBlock { get; set; }

		public ResultBlockButtonEventArgs Initialize(bool isBlock)
		{
			IsBlock = isBlock;
			return this;
		}

		protected override void OnRecycle()
		{
		}
	}
}
