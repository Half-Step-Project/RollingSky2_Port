namespace RS2
{
	public sealed class Levels_levelTable : DataRowBase
	{
		public const string DataTableName = "Levels_levelTable";

		public int Id { get; private set; }

		public string Notes { get; private set; }

		public string LevelScriptable { get; private set; }

		public string LevelName { get; private set; }

		public int Isopen { get; private set; }

		public int Diamonds { get; private set; }

		public int Crowns { get; private set; }

		public int Crowns_type { get; private set; }

		public int Title_id { get; private set; }

		public string TargetIds { get; private set; }

		public int MaxTargetNum { get; private set; }

		public int LockState { get; private set; }

		public string UnLockIds { get; private set; }

		public int TryPercent { get; private set; }

		public int DefaultMusicPercent { get; private set; }

		public int MusicAnimationID { get; private set; }

		public string OriginRebirthDiscountid { get; private set; }

		public int BuyOutRebirthCost { get; private set; }

		public int MaxDieCountForShowPromote { get; private set; }

		public int MusicId { get; private set; }

		public int NextEnterLevelType { get; private set; }

		public int NextLevelId { get; private set; }

		public int LevelGrade { get; private set; }

		public int IsShow { get; private set; }

		public int GoldAwardBase { get; private set; }

		public override int GetId()
		{
			return Id;
		}

		protected override void ParseRow()
		{
			Id = ReadInt();
			Notes = ReadLocalString();
			LevelScriptable = ReadLocalString();
			LevelName = ReadLocalString();
			Isopen = ReadInt();
			Diamonds = ReadInt();
			Crowns = ReadInt();
			Crowns_type = ReadInt();
			Title_id = ReadInt();
			TargetIds = ReadLocalString();
			MaxTargetNum = ReadInt();
			LockState = ReadInt();
			UnLockIds = ReadLocalString();
			TryPercent = ReadInt();
			DefaultMusicPercent = ReadInt();
			MusicAnimationID = ReadInt();
			OriginRebirthDiscountid = ReadLocalString();
			BuyOutRebirthCost = ReadInt();
			MaxDieCountForShowPromote = ReadInt();
			MusicId = ReadInt();
			NextEnterLevelType = ReadInt();
			NextLevelId = ReadInt();
			LevelGrade = ReadInt();
			IsShow = ReadInt();
			GoldAwardBase = ReadInt();
		}
	}
}
