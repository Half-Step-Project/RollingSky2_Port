namespace RS2
{
	public sealed class Goods_goodsTable : DataRowBase
	{
		public const string DataTableName = "Goods_goodsTable";

		public int Id { get; private set; }

		public string Note { get; private set; }

		public int Name { get; private set; }

		public int FunctionType { get; private set; }

		public int FunctionNum { get; private set; }

		public int IconId { get; private set; }

		public int Desc { get; private set; }

		public int CanRecover { get; private set; }

		public int PartsId { get; private set; }

		public int PartsNum { get; private set; }

		public int FullGoodsId { get; private set; }

		public override int GetId()
		{
			return Id;
		}

		protected override void ParseRow()
		{
			Id = ReadInt();
			Note = ReadLocalString();
			Name = ReadInt();
			FunctionType = ReadInt();
			FunctionNum = ReadInt();
			IconId = ReadInt();
			Desc = ReadInt();
			CanRecover = ReadInt();
			PartsId = ReadInt();
			PartsNum = ReadInt();
			FullGoodsId = ReadInt();
		}
	}
}
