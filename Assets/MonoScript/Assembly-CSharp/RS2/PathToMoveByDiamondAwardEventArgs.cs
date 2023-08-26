using Foundation;

namespace RS2
{
	public class PathToMoveByDiamondAwardEventArgs : EventArgs<PathToMoveByDiamondAwardEventArgs>
	{
		public int m_groupID;

		public PathToMoveByDiamondAwardEventArgs Initialize(int groupID)
		{
			m_groupID = groupID;
			return this;
		}

		protected override void OnRecycle()
		{
		}
	}
}
