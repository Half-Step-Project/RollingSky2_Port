using System;

namespace User.TileMap
{
	[Serializable]
	public struct ChildInfo
	{
		public int m_childNum;

		public bool IfHaveChild
		{
			get
			{
				return m_childNum > 0;
			}
		}

		public void Reset()
		{
			m_childNum = 0;
		}
	}
}
