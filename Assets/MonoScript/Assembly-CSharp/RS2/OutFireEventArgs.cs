using Foundation;

namespace RS2
{
	public class OutFireEventArgs : EventArgs<OutFireEventArgs>
	{
		public GroupSendData mData;

		public OutFireEventArgs Initialize(GroupSendData data)
		{
			mData = data;
			return this;
		}

		protected override void OnRecycle()
		{
		}
	}
}
