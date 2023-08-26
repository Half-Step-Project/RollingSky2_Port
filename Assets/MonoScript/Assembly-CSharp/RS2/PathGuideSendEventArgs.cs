using Foundation;

namespace RS2
{
	public class PathGuideSendEventArgs : EventArgs<PathGuideSendEventArgs>
	{
		public GroupsSendData Data;

		public PathGuideSendEventArgs Initialize(GroupsSendData data)
		{
			Data = data;
			return this;
		}

		protected override void OnRecycle()
		{
		}
	}
}
