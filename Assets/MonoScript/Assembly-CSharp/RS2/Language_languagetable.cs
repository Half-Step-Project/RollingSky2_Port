namespace RS2
{
	public sealed class Language_languagetable : DataRowBase
	{
		public const string DataTableName = "Language_languagetable";

		public int Id { get; private set; }

		public string Notes { get; private set; }

		public string English { get; private set; }

		public string Spanish { get; private set; }

		public string ChineseSimplified { get; private set; }

		public string ChineseTraditional { get; private set; }

		public string Japanese { get; private set; }

		public string French { get; private set; }

		public string German { get; private set; }

		public string Italian { get; private set; }

		public string Dutch { get; private set; }

		public string Russian { get; private set; }

		public string Korean { get; private set; }

		public override int GetId()
		{
			return Id;
		}

		protected override void ParseRow()
		{
			Id = ReadInt();
			Notes = ReadLocalString();
			English = ReadLocalString();
			Spanish = ReadLocalString();
			ChineseSimplified = ReadLocalString();
			ChineseTraditional = ReadLocalString();
			Japanese = ReadLocalString();
			French = ReadLocalString();
			German = ReadLocalString();
			Italian = ReadLocalString();
			Dutch = ReadLocalString();
			Russian = ReadLocalString();
			Korean = ReadLocalString();
		}
	}
}
