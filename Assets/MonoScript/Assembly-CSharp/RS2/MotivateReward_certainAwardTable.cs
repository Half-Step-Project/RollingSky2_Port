namespace RS2
{
	public sealed class MotivateReward_certainAwardTable : DataRowBase
	{
		public const string DataTableName = "MotivateReward_certainAwardTable";

		public int Id { get; private set; }

		public int AwardGoodsTeamId { get; private set; }

		public int IsCompleteActive { get; private set; }

		public override int GetId()
		{
			return Id;
		}

		protected override void ParseRow()
		{
			Id = ReadInt();
			AwardGoodsTeamId = ReadInt();
			IsCompleteActive = ReadInt();
		}
	}
}
