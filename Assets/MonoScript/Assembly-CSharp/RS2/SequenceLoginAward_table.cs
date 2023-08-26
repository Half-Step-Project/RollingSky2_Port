namespace RS2
{
	public sealed class SequenceLoginAward_table : DataRowBase
	{
		public const string DataTableName = "SequenceLoginAward_table";

		public int Id { get; private set; }

		public string Notes { get; private set; }

		public int GoodTeamIds { get; private set; }

		public override int GetId()
		{
			return Id;
		}

		protected override void ParseRow()
		{
			Id = ReadInt();
			Notes = ReadLocalString();
			GoodTeamIds = ReadInt();
		}
	}
}
