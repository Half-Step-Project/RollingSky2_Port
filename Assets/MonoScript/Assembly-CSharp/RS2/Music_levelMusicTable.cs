namespace RS2
{
	public sealed class Music_levelMusicTable : DataRowBase
	{
		public const string DataTableName = "Music_levelMusicTable";

		public int Id { get; private set; }

		public string Notes { get; private set; }

		public string ResourceName { get; private set; }

		public int Priority { get; private set; }

		public float Volume { get; private set; }

		public float Length { get; private set; }

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
			Length = ReadFloat();
		}
	}
}
