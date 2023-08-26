namespace RS2
{
	public sealed class MotivateReward_randomAwardTable : DataRowBase
	{
		public const string DataTableName = "MotivateReward_randomAwardTable";

		public int Id { get; private set; }

		public int LevelId { get; private set; }

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
			LevelId = ReadInt();
			GoodsId = ReadInt();
			Count = ReadInt();
			Weight = ReadInt();
		}
	}
}
