namespace RS2
{
	public sealed class GoodsTeam_goodsTeamTable : DataRowBase
	{
		public const string DataTableName = "GoodsTeam_goodsTeamTable";

		public int Id { get; private set; }

		public string Note { get; private set; }

		public int Name { get; private set; }

		public string GoodsIds { get; private set; }

		public string GoodsCount { get; private set; }

		public int IconId { get; private set; }

		public int Desc { get; private set; }

		public override int GetId()
		{
			return Id;
		}

		protected override void ParseRow()
		{
			Id = ReadInt();
			Note = ReadLocalString();
			Name = ReadInt();
			GoodsIds = ReadLocalString();
			GoodsCount = ReadLocalString();
			IconId = ReadInt();
			Desc = ReadInt();
		}
	}
}
