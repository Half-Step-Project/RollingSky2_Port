namespace RS2
{
	public sealed class OriginRebirth_Discount : DataRowBase
	{
		public const string DataTableName = "OriginRebirth_Discount";

		public int Id { get; private set; }

		public int Min { get; private set; }

		public int Max { get; private set; }

		public float Count { get; private set; }

		public int ShopID { get; private set; }

		public override int GetId()
		{
			return Id;
		}

		protected override void ParseRow()
		{
			Id = ReadInt();
			Min = ReadInt();
			Max = ReadInt();
			Count = ReadFloat();
			ShopID = ReadInt();
		}
	}
}
