namespace RS2
{
	public sealed class MotivateReward_award : DataRowBase
	{
		public const string DataTableName = "MotivateReward_award";

		public int Id { get; private set; }

		public int SubsetId { get; private set; }

		public int GoodsId { get; private set; }

		public int Min { get; private set; }

		public int Max { get; private set; }

		public int Weight { get; private set; }

		public override int GetId()
		{
			return Id;
		}

		protected override void ParseRow()
		{
			Id = ReadInt();
			SubsetId = ReadInt();
			GoodsId = ReadInt();
			Min = ReadInt();
			Max = ReadInt();
			Weight = ReadInt();
		}
	}
}
