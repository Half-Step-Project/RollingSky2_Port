namespace RS2
{
	public sealed class PlayerStarAbility_table : DataRowBase
	{
		public const string DataTableName = "PlayerStarAbility_table";

		public int Id { get; private set; }

		public string Note { get; private set; }

		public int AblilityNum { get; private set; }

		public int Desc { get; private set; }

		public int MinStarLevel { get; private set; }

		public override int GetId()
		{
			return Id;
		}

		protected override void ParseRow()
		{
			Id = ReadInt();
			Note = ReadLocalString();
			AblilityNum = ReadInt();
			Desc = ReadInt();
			MinStarLevel = ReadInt();
		}
	}
}
