using Foundation;

namespace RS2
{
	public sealed class BgMusicPlayEventArgs : EventArgs<BgMusicPlayEventArgs>
	{
		public float m_time;

		public BgMusicPlayEventArgs Initialize(float time)
		{
			m_time = time;
			return this;
		}

		protected override void OnRecycle()
		{
			m_time = 0f;
		}
	}
}
