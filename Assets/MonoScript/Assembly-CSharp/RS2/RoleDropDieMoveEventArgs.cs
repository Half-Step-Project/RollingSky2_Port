using Foundation;

namespace RS2
{
	public class RoleDropDieMoveEventArgs : EventArgs<RoleDropDieMoveEventArgs>
	{
		public bool IfDropDieStatic;

		public RoleDropDieMoveEventArgs Initialize(bool ifDropDieStatic)
		{
			IfDropDieStatic = ifDropDieStatic;
			return this;
		}

		protected override void OnRecycle()
		{
			IfDropDieStatic = false;
		}
	}
}
