namespace RS2
{
	public sealed class Instrument_table : DataRowBase
	{
		public const string DataTableName = "Instrument_table";

		public int Id { get; private set; }

		public string Note { get; private set; }

		public int DefaultSlot { get; private set; }

		public int Name { get; private set; }

		public int ProductGoods { get; private set; }

		public string AssetName { get; private set; }

		public string SoundID { get; private set; }

		public int PlayProductAdd { get; private set; }

		public int IconId { get; private set; }

		public int UnLockPlayerStarLevel { get; private set; }

		public int UnLockPlayerLevel { get; private set; }

		public int AdDirectUpLevel { get; private set; }

		public override int GetId()
		{
			return Id;
		}

		protected override void ParseRow()
		{
			Id = ReadInt();
			Note = ReadLocalString();
			DefaultSlot = ReadInt();
			Name = ReadInt();
			ProductGoods = ReadInt();
			AssetName = ReadLocalString();
			SoundID = ReadLocalString();
			PlayProductAdd = ReadInt();
			IconId = ReadInt();
			UnLockPlayerStarLevel = ReadInt();
			UnLockPlayerLevel = ReadInt();
			AdDirectUpLevel = ReadInt();
		}
	}
}
