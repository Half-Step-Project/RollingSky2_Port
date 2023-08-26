using Foundation;

namespace RS2
{
	public class NoConsumePowerTimeChanged : EventArgs<NoConsumePowerTimeChanged>
	{
		private long m_changeMilisec;

		public long ChangeMilisec
		{
			get
			{
				return m_changeMilisec;
			}
		}

		public NoConsumePowerTimeChanged Initialize(long changeMilisec)
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
