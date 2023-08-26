namespace RS2
{
	public sealed class Award_awardTable : DataRowBase
	{
		public const string DataTableName = "Award_awardTable";

		public int Id { get; private set; }

		public string Notes { get; private set; }

		public int GoodsID { get; private set; }

		public int Count { get; private set; }

		public override int GetId()
		{
			return Id;
		}

		protected override void ParseRow()
		{
			Id = ReadInt();
			Notes = ReadLocalString();
			GoodsID = ReadInt();
			Count = ReadInt();
		}
	}
}
