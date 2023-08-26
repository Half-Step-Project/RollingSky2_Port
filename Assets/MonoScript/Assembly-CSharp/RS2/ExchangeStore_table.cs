namespace RS2
{
	public sealed class ExchangeStore_table : DataRowBase
	{
		public const string DataTableName = "ExchangeStore_table";

		public int Id { get; private set; }

		public string Note { get; private set; }

		public int Type { get; private set; }

		public int ShowInfoID { get; private set; }

		public int IconID { get; private set; }

		public int Price { get; private set; }

		public int Coin_time { get; private set; }

		public int Ad_get { get; private set; }

		public int Ad_cd { get; private set; }

		public override int GetId()
		{
			return Id;
		}

		protected override void ParseRow()
		{
			Id = ReadInt();
			Note = ReadLocalString();
			Type = ReadInt();
			ShowInfoID = ReadInt();
			IconID = ReadInt();
			Price = ReadInt();
			Coin_time = ReadInt();
			Ad_get = ReadInt();
			Ad_cd = ReadInt();
		}
	}
}
