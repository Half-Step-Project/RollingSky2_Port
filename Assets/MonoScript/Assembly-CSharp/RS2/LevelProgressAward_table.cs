namespace RS2
{
	public sealed class LevelProgressAward_table : DataRowBase
	{
		public const string DataTableName = "LevelProgressAward_table";

		public int Id { get; private set; }

		public string Notes { get; private set; }

		public string AwardIds { get; private set; }

		public override int GetId()
		{
			return Id;
		}

		protected override void ParseRow()
		{
			Id = ReadInt();
			Notes = ReadLocalString();
			AwardIds = ReadLocalString();
		}
	}
}
