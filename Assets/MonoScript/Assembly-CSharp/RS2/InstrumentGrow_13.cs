namespace RS2
{
	public sealed class InstrumentGrow_13 : DataRowBase
	{
		public const string DataTableName = "InstrumentGrow_13";

		public int Id { get; private set; }

		public string Node { get; private set; }

		public string StarLevel_production { get; private set; }

		public string StarLevel_consume { get; private set; }

		public override int GetId()
		{
			return Id;
		}

		protected override void ParseRow()
		{
			Id = ReadInt();
			Node = ReadLocalString();
			StarLevel_production = ReadLocalString();
			StarLevel_consume = ReadLocalString();
		}
	}
}
