namespace RS2
{
	public sealed class InstrumentBatonOffLineBaseProduct_table : DataRowBase
	{
		public const string DataTableName = "InstrumentBatonOffLineBaseProduct_table";

		public int Id { get; private set; }

		public string Node { get; private set; }

		public string OfflineBaseNum { get; private set; }

		public override int GetId()
		{
			return Id;
		}

		protected override void ParseRow()
		{
			Id = ReadInt();
			Node = ReadLocalString();
			OfflineBaseNum = ReadLocalString();
		}
	}
}
