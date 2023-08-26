using Foundation;

namespace RS2
{
	public class CutEventArg : EventArgs<CutEventArg>
	{
		public GroupSendData mData;

		public CutEventArg Initialize(GroupSendData data)
		{
			mData = data;
			return this;
		}

		protected override void OnRecycle()
		{
		}
	}
}
