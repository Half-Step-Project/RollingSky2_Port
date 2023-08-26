namespace RS2
{
	public sealed class LevelTarget_levelTargetTable : DataRowBase
	{
		public const string DataTableName = "LevelTarget_levelTargetTable";

		public int Id { get; private set; }

		public string Notes { get; private set; }

		public int Desc { get; private set; }

		public int Type { get; private set; }

		public int TargetNum { get; private set; }

		public string AwardIds { get; private set; }

		public int StartLevel { get; private set; }

		public override int GetId()
		{
			return Id;
		}

		protected override void ParseRow()
		{
			Id = ReadInt();
			Notes = ReadLocalString();
			Desc = ReadInt();
			Type = ReadInt();
			TargetNum = ReadInt();
			AwardIds = ReadLocalString();
			StartLevel = ReadInt();
		}
	}
}
