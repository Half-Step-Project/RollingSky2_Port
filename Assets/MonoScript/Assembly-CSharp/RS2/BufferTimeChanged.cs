using Foundation;

namespace RS2
{
	public class BufferTimeChanged : EventArgs<BufferTimeChanged>
	{
		private long m_changeMilisec;

		public long ChangeMilisec
		{
			get
			{
				return m_changeMilisec;
			}
		}

		public BufferTimeChanged Initialize(long changeMilisec)
		{
			m_changeMilisec = changeMilisec;
			return this;
		}

		protected override void OnRecycle()
		{
			m_changeMilisec = 0L;
		}
	}
}
