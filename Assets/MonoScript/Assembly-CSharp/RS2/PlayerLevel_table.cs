namespace RS2
{
	public sealed class PlayerLevel_table : DataRowBase
	{
		public const string DataTableName = "PlayerLevel_table";

		public int Id { get; private set; }

		public string Note { get; private set; }

		public int LevelUpConsumeGoodsID { get; private set; }

		public string LevelUpConsumeGoodsNum { get; private set; }

		public string LevelUpMission { get; private set; }

		public string RewardId { get; private set; }

		public override int GetId()
		{
			return Id;
		}

		protected override void ParseRow()
		{
			Id = ReadInt();
			Note = ReadLocalString();
			LevelUpConsumeGoodsID = ReadInt();
			LevelUpConsumeGoodsNum = ReadLocalString();
			LevelUpMission = ReadLocalString();
			RewardId = ReadLocalString();
		}
	}
}
