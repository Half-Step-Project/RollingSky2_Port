namespace RS2
{
	public sealed class UISound_uiSoundTable : DataRowBase
	{
		public const string DataTableName = "UISound_uiSoundTable";

		public int Id { get; private set; }

		public string Notes { get; private set; }

		public string ResourceName { get; private set; }

		public int Priority { get; private set; }

		public float Volume { get; private set; }

		public override int GetId()
		{
			return Id;
		}

		protected override void ParseRow()
		{
			Id = ReadInt();
			Notes = ReadLocalString();
			ResourceName = ReadLocalString();
			Priority = ReadInt();
			Volume = ReadFloat();
		}
	}
}
