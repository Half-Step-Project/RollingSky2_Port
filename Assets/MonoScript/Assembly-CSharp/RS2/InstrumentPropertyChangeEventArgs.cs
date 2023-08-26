using System.Collections.Generic;
using Foundation;

namespace RS2
{
	public class InstrumentPropertyChangeEventArgs : EventArgs<InstrumentPropertyChangeEventArgs>
	{
		private List<int> m_InstrmentIds = new List<int>();

		public List<int> InstrumentIds
		{
			get
			{
				return m_InstrmentIds;
			}
		}

		public InstrumentPropertyType ChangeType { get; private set; }

		public InstrumentPropertyChangeEventArgs Initialize(List<int> ids, InstrumentPropertyType type)
		{
			m_InstrmentIds.Clear();
			for (int i = 0; i < ids.Count; i++)
			{
				m_InstrmentIds.Add(ids[i]);
			}
			ChangeType = type;
			return this;
		}

		protected override void OnRecycle()
		{
			m_InstrmentIds.Clear();
			ChangeType = InstrumentPropertyType.NONE;
		}
	}
}
