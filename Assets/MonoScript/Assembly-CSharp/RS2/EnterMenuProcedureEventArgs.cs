using Foundation;

namespace RS2
{
	public sealed class EnterMenuProcedureEventArgs : EventArgs<EnterMenuProcedureEventArgs>
	{
		public EnterMenuProcedureEventArgs Initialize()
		{
			return this;
		}

		protected override void OnRecycle()
		{
		}
	}
}
