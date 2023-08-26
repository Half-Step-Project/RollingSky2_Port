namespace RS2
{
	public sealed class LevelsResolution_table : DataRowBase
	{
		public const string DataTableName = "LevelsResolution_table";

		public int Id { get; private set; }

		public string Notes { get; private set; }

		public string Type_1 { get; private set; }

		public string Type_2 { get; private set; }

		public string Type_3 { get; private set; }

		public string Type_4 { get; private set; }

		public override int GetId()
		{
			return Id;
		}

		protected override void ParseRow()
		{
			Id = ReadInt();
			Notes = ReadLocalString();
			Type_1 = ReadLocalString();
			Type_2 = ReadLocalString();
			Type_3 = ReadLocalString();
			Type_4 = ReadLocalString();
		}
	}
}
