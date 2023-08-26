namespace RS2
{
	public sealed class LevelSections_table : DataRowBase
	{
		public const string DataTableName = "LevelSections_table";

		public int Id { get; private set; }

		public string Notes { get; private set; }

		public int Name { get; private set; }

		public string Levels { get; private set; }

		public override int GetId()
		{
			return Id;
		}

		protected override void ParseRow()
		{
			Id = ReadInt();
			Notes = ReadLocalString();
			Name = ReadInt();
			Levels = ReadLocalString();
		}
	}
}
