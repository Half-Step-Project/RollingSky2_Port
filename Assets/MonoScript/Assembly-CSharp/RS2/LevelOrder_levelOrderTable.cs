namespace RS2
{
	public sealed class LevelOrder_levelOrderTable : DataRowBase
	{
		public const string DataTableName = "LevelOrder_levelOrderTable";

		public int Id { get; private set; }

		public string Notes { get; private set; }

		public int LevelSeriesId { get; private set; }

		public int IsHadContent { get; private set; }

		public override int GetId()
		{
			return Id;
		}

		protected override void ParseRow()
		{
			Id = ReadInt();
			Notes = ReadLocalString();
			LevelSeriesId = ReadInt();
			IsHadContent = ReadInt();
		}
	}
}
