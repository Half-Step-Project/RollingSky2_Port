namespace RS2
{
	public sealed class VideoAward_videoAwardTable : DataRowBase
	{
		public const string DataTableName = "VideoAward_videoAwardTable";

		public int Id { get; private set; }

		public int PlayerType { get; private set; }

		public int Cd { get; private set; }

		public int Limit { get; private set; }

		public int GoodsId { get; private set; }

		public int Count { get; private set; }

		public int Weight { get; private set; }

		public override int GetId()
		{
			return Id;
		}

		protected override void ParseRow()
		{
			Id = ReadInt();
			PlayerType = ReadInt();
			Cd = ReadInt();
			Limit = ReadInt();
			GoodsId = ReadInt();
			Count = ReadInt();
			Weight = ReadInt();
		}
	}
}
