namespace RS2
{
	public sealed class PlayerStarLevel_table : DataRowBase
	{
		public const string DataTableName = "PlayerStarLevel_table";

		public int Id { get; private set; }

		public string Note { get; private set; }

		public int StarUpNeedLevel { get; private set; }

		public int StarUpNeedGoodsId { get; private set; }

		public string StarUpNeedGoodsCount { get; private set; }

		public int PlayerTitle { get; private set; }

		public int PlayerTitleName { get; private set; }

		public int UnlockInstrumentId { get; private set; }

		public int PlayerIcon { get; private set; }

		public string PlayerStageAvatar { get; private set; }

		public int PlayerLevelAvatar { get; private set; }

		public int PlayerStarLevelIcon { get; private set; }

		public override int GetId()
		{
			return Id;
		}

		protected override void ParseRow()
		{
			Id = ReadInt();
			Note = ReadLocalString();
			StarUpNeedLevel = ReadInt();
			StarUpNeedGoodsId = ReadInt();
			StarUpNeedGoodsCount = ReadLocalString();
			PlayerTitle = ReadInt();
			PlayerTitleName = ReadInt();
			UnlockInstrumentId = ReadInt();
			PlayerIcon = ReadInt();
			PlayerStageAvatar = ReadLocalString();
			PlayerLevelAvatar = ReadInt();
			PlayerStarLevelIcon = ReadInt();
		}
	}
}
