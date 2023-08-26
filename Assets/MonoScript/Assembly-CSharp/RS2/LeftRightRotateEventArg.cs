using Foundation;

namespace RS2
{
	public class LeftRightRotateEventArg : EventArgs<LeftRightRotateEventArg>
	{
		public LeftRightRotateSendTrigger.Data m_data;

		public LeftRightRotateEventArg Initialize(LeftRightRotateSendTrigger.Data data)
		{
			m_data = data;
			return this;
		}

		protected override void OnRecycle()
		{
		}
	}
}
