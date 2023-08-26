using Foundation;

namespace RS2
{
	public sealed class EnterLevelProcedureEventArgs : EventArgs<EnterLevelProcedureEventArgs>
	{
		public EnterLevelProcedureEventArgs Initialize()
		{
			return this;
		}

		protected override void OnRecycle()
		{
		}
	}
}
