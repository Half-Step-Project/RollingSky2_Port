using Foundation;

namespace RS2
{
	public sealed class TileFreeMoveCallEventArgs : EventArgs<TileFreeMoveCallEventArgs>
	{
		public FreeMoveData TileMoveData { get; private set; }

		public TileFreeMoveCallEventArgs Initialize(FreeMoveData moveData)
		{
			TileMoveData = moveData;
			return this;
		}

		protected override void OnRecycle()
		{
			TileMoveData = null;
		}
	}
}
