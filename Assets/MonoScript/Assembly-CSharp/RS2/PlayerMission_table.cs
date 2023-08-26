namespace RS2
{
	public sealed class PlayerMission_table : DataRowBase
	{
		public const string DataTableName = "PlayerMission_table";

		public int Id { get; private set; }

		public string Notes { get; private set; }

		public int Desc { get; private set; }

		public int Type { get; private set; }

		public int TypeId { get; private set; }

		public int TargetNum { get; private set; }

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
			TypeId = ReadInt();
			TargetNum = ReadInt();
		}
	}
}
