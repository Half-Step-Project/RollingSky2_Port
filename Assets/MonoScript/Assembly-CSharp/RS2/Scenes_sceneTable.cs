namespace RS2
{
	public sealed class Scenes_sceneTable : DataRowBase
	{
		public const string DataTableName = "Scenes_sceneTable";

		public int Id { get; private set; }

		public string Notes { get; private set; }

		public string AssetName { get; private set; }

		public int IsGameLevel { get; private set; }

		public int IsBuiltin { get; private set; }

		public int IsInVersion { get; private set; }

		public override int GetId()
		{
			return Id;
		}

		protected override void ParseRow()
		{
			Id = ReadInt();
			Notes = ReadLocalString();
			AssetName = ReadLocalString();
			IsGameLevel = ReadInt();
			IsBuiltin = ReadInt();
			IsInVersion = ReadInt();
		}
	}
}
