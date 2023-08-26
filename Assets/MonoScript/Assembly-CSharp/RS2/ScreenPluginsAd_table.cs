namespace RS2
{
	public sealed class ScreenPluginsAd_table : DataRowBase
	{
		public const string DataTableName = "ScreenPluginsAd_table";

		public int Id { get; private set; }

		public string Note { get; private set; }

		public int BeforeImage { get; private set; }

		public int BeforeText { get; private set; }

		public int AfterImage { get; private set; }

		public int AfterText { get; private set; }

		public int TriggerType { get; private set; }

		public int TriggerNum { get; private set; }

		public int IshadShopItem { get; private set; }

		public int AwardId { get; private set; }

		public int AutoPlayAdTime { get; private set; }

		public int AutoCloseAdTime { get; private set; }

		public override int GetId()
		{
			return Id;
		}

		protected override void ParseRow()
		{
			Id = ReadInt();
			Note = ReadLocalString();
			BeforeImage = ReadInt();
			BeforeText = ReadInt();
			AfterImage = ReadInt();
			AfterText = ReadInt();
			TriggerType = ReadInt();
			TriggerNum = ReadInt();
			IshadShopItem = ReadInt();
			AwardId = ReadInt();
			AutoPlayAdTime = ReadInt();
			AutoCloseAdTime = ReadInt();
		}
	}
}
