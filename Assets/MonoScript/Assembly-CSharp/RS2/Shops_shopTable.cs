namespace RS2
{
	public sealed class Shops_shopTable : DataRowBase
	{
		public const string DataTableName = "Shops_shopTable";

		public int Id { get; private set; }

		public string Note { get; private set; }

		public int Name { get; private set; }

		public int Sort { get; private set; }

		public int Type { get; private set; }

		public int GoodsTeamid { get; private set; }

		public int Count { get; private set; }

		public int BuyType { get; private set; }

		public string Price { get; private set; }

		public string OnTime { get; private set; }

		public string OutTime { get; private set; }

		public int IsResetForDay { get; private set; }

		public int MaxBuyCount { get; private set; }

		public string Product_id_android { get; private set; }

		public string Product_id_ios { get; private set; }

		public int Sence { get; private set; }

		public int IconId { get; private set; }

		public string ShowInfo { get; private set; }

		public int Discount { get; private set; }

		public int Hot { get; private set; }

		public int LimitCount { get; private set; }

		public int NeedShowPayResult { get; private set; }

		public int Shopsort { get; private set; }

		public int CanRecover { get; private set; }

		public int BuyCd { get; private set; }

		public int AdFreeCount { get; private set; }

		public override int GetId()
		{
			return Id;
		}

		protected override void ParseRow()
		{
			Id = ReadInt();
			Note = ReadLocalString();
			Name = ReadInt();
			Sort = ReadInt();
			Type = ReadInt();
			GoodsTeamid = ReadInt();
			Count = ReadInt();
			BuyType = ReadInt();
			Price = ReadLocalString();
			OnTime = ReadLocalString();
			OutTime = ReadLocalString();
			IsResetForDay = ReadInt();
			MaxBuyCount = ReadInt();
			Product_id_android = ReadLocalString();
			Product_id_ios = ReadLocalString();
			Sence = ReadInt();
			IconId = ReadInt();
			ShowInfo = ReadLocalString();
			Discount = ReadInt();
			Hot = ReadInt();
			LimitCount = ReadInt();
			NeedShowPayResult = ReadInt();
			Shopsort = ReadInt();
			CanRecover = ReadInt();
			BuyCd = ReadInt();
			AdFreeCount = ReadInt();
		}
	}
}
