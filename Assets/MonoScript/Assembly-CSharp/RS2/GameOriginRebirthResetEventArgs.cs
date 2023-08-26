using Foundation;

namespace RS2
{
	public sealed class GameOriginRebirthResetEventArgs : EventArgs<GameOriginRebirthResetEventArgs>
	{
		public bool CheckFreeBuff { get; set; }

		public static GameOriginRebirthResetEventArgs Make(bool checkFreeBuff = false)
		{
			GameOriginRebirthResetEventArgs gameOriginRebirthResetEventArgs = Mod.Reference.Acquire<GameOriginRebirthResetEventArgs>();
			gameOriginRebirthResetEventArgs.CheckFreeBuff = checkFreeBuff;
			return gameOriginRebirthResetEventArgs;
		}

		public void Initialize()
		{
		}

		protected override void OnRecycle()
		{
		}
	}
}
