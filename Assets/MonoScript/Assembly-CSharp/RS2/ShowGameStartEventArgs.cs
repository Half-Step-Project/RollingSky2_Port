using Foundation;

namespace RS2
{
	public sealed class ShowGameStartEventArgs : EventArgs<ShowGameStartEventArgs>
	{
		public bool CheckFreeBuff { get; set; }

		public bool ShowRebirth { get; set; }

		public ShowGameStartEventArgs Initialize(bool checkFreeBuff, bool showRebirth)
		{
			CheckFreeBuff = checkFreeBuff;
			ShowRebirth = showRebirth;
			return this;
		}

		protected override void OnRecycle()
		{
		}
	}
}
