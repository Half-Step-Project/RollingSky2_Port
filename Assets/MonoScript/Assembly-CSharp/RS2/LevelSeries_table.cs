namespace RS2
{
	public sealed class LevelSeries_table : DataRowBase
	{
		public const string DataTableName = "LevelSeries_table";

		public int Id { get; private set; }

		public string Notes { get; private set; }

		public int Name { get; private set; }

		public string LevelChapters { get; private set; }

		public int MuisicId { get; private set; }

		public string LevelBgColor { get; private set; }

		public int UnlockStar { get; private set; }

		public int UnlockLevel { get; private set; }

		public override int GetId()
		{
			return Id;
		}

		protected override void ParseRow()
		{
			Id = ReadInt();
			Notes = ReadLocalString();
			Name = ReadInt();
			LevelChapters = ReadLocalString();
			MuisicId = ReadInt();
			LevelBgColor = ReadLocalString();
			UnlockStar = ReadInt();
			UnlockLevel = ReadInt();
		}
	}
}
