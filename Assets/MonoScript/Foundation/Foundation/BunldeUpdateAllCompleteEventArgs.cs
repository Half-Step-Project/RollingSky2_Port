namespace Foundation
{
	public sealed class BunldeUpdateAllCompleteEventArgs : EventArgs<BunldeUpdateAllCompleteEventArgs>
	{
		public static BunldeUpdateAllCompleteEventArgs Make()
		{
			return Mod.Reference.Acquire<BunldeUpdateAllCompleteEventArgs>();
		}

		protected override void OnRecycle()
		{
		}
	}
}
