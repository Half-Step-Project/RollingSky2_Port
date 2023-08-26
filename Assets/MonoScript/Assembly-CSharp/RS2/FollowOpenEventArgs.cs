using Foundation;

namespace RS2
{
	public sealed class FollowOpenEventArgs : EventArgs<FollowOpenEventArgs>
	{
		public FollowData m_followData;

		public FollowOpenEventArgs Initialize(FollowData followData)
		{
			m_followData = followData.Clone();
			return this;
		}

		protected override void OnRecycle()
		{
			m_followData = null;
		}
	}
}
