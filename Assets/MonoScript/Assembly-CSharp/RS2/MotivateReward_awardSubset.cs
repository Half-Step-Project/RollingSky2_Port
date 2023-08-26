namespace RS2
{
	public sealed class MotivateReward_awardSubset : DataRowBase
	{
		public const string DataTableName = "MotivateReward_awardSubset";

		public int Id { get; private set; }

		public int SupersetId { get; private set; }

		public int NextId { get; private set; }

		public int NextMin { get; private set; }

		public int NextMax { get; private set; }

		public int Quality { get; private set; }

		public override int GetId()
		{
			return Id;
		}

		protected override void ParseRow()
		{
			Id = ReadInt();
			SupersetId = ReadInt();
			NextId = ReadInt();
			NextMin = ReadInt();
			NextMax = ReadInt();
			Quality = ReadInt();
		}
	}
}
