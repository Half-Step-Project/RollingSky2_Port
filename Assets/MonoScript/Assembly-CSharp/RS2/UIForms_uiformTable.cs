namespace RS2
{
	public sealed class UIForms_uiformTable : DataRowBase
	{
		public const string DataTableName = "UIForms_uiformTable";

		public int Id { get; private set; }

		public string Notes { get; private set; }

		public string AssetName { get; private set; }

		public string UiGroupName { get; private set; }

		public int AllowMultiInstance { get; private set; }

		public int PauseCoveredUIForm { get; private set; }

		public override int GetId()
		{
			return Id;
		}

		protected override void ParseRow()
		{
			Id = ReadInt();
			Notes = ReadLocalString();
			AssetName = ReadLocalString();
			UiGroupName = ReadLocalString();
			AllowMultiInstance = ReadInt();
			PauseCoveredUIForm = ReadInt();
		}
	}
}
