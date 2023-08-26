using Foundation;

namespace RS2
{
	public sealed class DiamondsChangeEventArgs : EventArgs<DiamondsChangeEventArgs>
	{
		private int m_diamondsNum;

		public int DiamondsNum
		{
			get
			{
				return m_diamondsNum;
			}
		}

		public DiamondsChangeEventArgs Initialize(int diamondsNum)
		{
			m_diamondsNum = diamondsNum;
			return this;
		}

		protected override void OnRecycle()
		{
			m_diamondsNum = 0;
		}
	}
}
