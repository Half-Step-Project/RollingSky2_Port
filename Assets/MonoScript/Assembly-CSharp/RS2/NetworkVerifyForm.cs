using Foundation;

namespace RS2
{
	public class NetworkVerifyForm : UGUIForm
	{
		public static void Verify(Action handler)
		{
			if (handler != null)
			{
				handler();
			}
		}
	}
}
