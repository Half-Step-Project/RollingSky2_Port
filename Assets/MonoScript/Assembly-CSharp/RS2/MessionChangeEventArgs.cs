using System.Collections.Generic;
using Foundation;

namespace RS2
{
	public class MessionChangeEventArgs : EventArgs<MessionChangeEventArgs>
	{
		private List<int> m_changeMession = new List<int>();

		public List<int> ChangeMessions
		{
			get
			{
				return m_changeMession;
			}
		}

		public MessionChangeEventArgs Initialize(List<int> messions)
		{
			m_changeMession.Clear();
			for (int i = 0; i < messions.Count; i++)
			{
				ChangeMessions.Add(messions[i]);
			}
			return this;
		}

		protected override void OnRecycle()
		{
			m_changeMession.Clear();
		}
	}
}
