namespace RS2
{
	public sealed class MotivateReward_needEnergy : DataRowBase
	{
		public const string DataTableName = "MotivateReward_needEnergy";

		public int Id { get; private set; }

		public int Energy { get; private set; }

		public int Rare { get; private set; }

		public override int GetId()
		{
			return Id;
		}

		protected override void ParseRow()
		{
			Id = ReadInt();
			Energy = ReadInt();
			Rare = ReadInt();
		}
	}
}
