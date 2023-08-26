namespace RS2
{
	public sealed class CommonData_commonDataTable : DataRowBase
	{
		public const string DataTableName = "CommonData_commonDataTable";

		public int Id { get; private set; }

		public string Note { get; private set; }

		public int Type { get; private set; }

		public string Num { get; private set; }

		public override int GetId()
		{
			return Id;
		}

		protected override void ParseRow()
		{
			Id = ReadInt();
			Note = ReadLocalString();
			Type = ReadInt();
			Num = ReadLocalString();
		}
	}
}
