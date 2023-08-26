using Foundation;

namespace RS2
{
	public sealed class GamePauseEventArgs : EventArgs<GamePauseEventArgs>
	{
		private bool m_isShowPauseForm = true;

		public bool IsShowPauseForm
		{
			get
			{
				return m_isShowPauseForm;
			}
			private set
			{
				m_isShowPauseForm = value;
			}
		}

		public GamePauseEventArgs Initialize(bool isShowPauseFomr = true)
		{
			IsShowPauseForm = isShowPauseFomr;
			return this;
		}

		protected override void OnRecycle()
		{
			m_isShowPauseForm = true;
		}
	}
}
