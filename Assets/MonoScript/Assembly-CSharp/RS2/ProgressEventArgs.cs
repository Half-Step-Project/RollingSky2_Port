using Foundation;

namespace RS2
{
	public sealed class ProgressEventArgs : EventArgs<ProgressEventArgs>
	{
		public int m_progress;

		public ProgressEventArgs Initialize(int progress)
		{
			m_progress = progress;
			return this;
		}

		protected override void OnRecycle()
		{
			m_progress = 0;
		}
	}
}
