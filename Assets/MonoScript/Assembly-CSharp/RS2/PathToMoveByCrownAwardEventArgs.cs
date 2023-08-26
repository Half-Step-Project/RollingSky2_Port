using Foundation;

namespace RS2
{
	public class PathToMoveByCrownAwardEventArgs : EventArgs<PathToMoveByCrownAwardEventArgs>
	{
		public int m_groupID;

		public PathToMoveByCrownAwardEventArgs Initialize(int groupID)
		{
			m_groupID = groupID;
			return this;
		}

		protected override void OnRecycle()
		{
		}
	}
}
