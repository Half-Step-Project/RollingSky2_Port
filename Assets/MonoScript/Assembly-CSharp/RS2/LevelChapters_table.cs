namespace RS2
{
	public sealed class LevelChapters_table : DataRowBase
	{
		public const string DataTableName = "LevelChapters_table";

		public int Id { get; private set; }

		public string Notes { get; private set; }

		public int Name { get; private set; }

		public int SubName { get; private set; }

		public string Sections { get; private set; }

		public override int GetId()
		{
			return Id;
		}

		protected override void ParseRow()
		{
			Id = ReadInt();
			Notes = ReadLocalString();
			Name = ReadInt();
			SubName = ReadInt();
			Sections = ReadLocalString();
		}
	}
}
