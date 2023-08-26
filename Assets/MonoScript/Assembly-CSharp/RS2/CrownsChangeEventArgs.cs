using Foundation;

namespace RS2
{
	public sealed class CrownsChangeEventArgs : EventArgs<CrownsChangeEventArgs>
	{
		private int m_crownsNum;

		public int CrownsNum
		{
			get
			{
				return m_crownsNum;
			}
		}

		public CrownsChangeEventArgs Initialize(int crownsNum)
		{
			m_crownsNum = crownsNum;
			return this;
		}

		protected override void OnRecycle()
		{
			m_crownsNum = 0;
		}
	}
}
