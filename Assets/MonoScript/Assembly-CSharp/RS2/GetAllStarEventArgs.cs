using Foundation;

namespace RS2
{
	public sealed class GetAllStarEventArgs : EventArgs<GetAllStarEventArgs>
	{
		public int LevelSeriesId { get; set; }

		public GetAllStarEventArgs Initialize(int id)
		{
			LevelSeriesId = id;
			return this;
		}

		protected override void OnRecycle()
		{
		}
	}
}
