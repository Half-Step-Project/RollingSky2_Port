namespace RS2
{
	public sealed class LuckyTurntable_table : DataRowBase
	{
		public const string DataTableName = "LuckyTurntable_table";

		public int Id { get; private set; }

		public string Note { get; private set; }

		public int GoodsId { get; private set; }

		public int GoodsCount { get; private set; }

		public int GoodsTime { get; private set; }

		public int WeightNess { get; private set; }

		public int Desc { get; private set; }

		public override int GetId()
		{
			return Id;
		}

		protected override void ParseRow()
		{
			Id = ReadInt();
			Note = ReadLocalString();
			GoodsId = ReadInt();
			GoodsCount = ReadInt();
			GoodsTime = ReadInt();
			WeightNess = ReadInt();
			Desc = ReadInt();
		}
	}
}
