namespace RS2
{
	public sealed class MotivateReward_awardSuperset : DataRowBase
	{
		public const string DataTableName = "MotivateReward_awardSuperset";

		public int Id { get; private set; }

		public int LevelCount { get; private set; }

		public int SetId { get; private set; }

		public override int GetId()
		{
			return Id;
		}

		protected override void ParseRow()
		{
			Id = ReadInt();
			LevelCount = ReadInt();
			SetId = ReadInt();
		}
	}
}
