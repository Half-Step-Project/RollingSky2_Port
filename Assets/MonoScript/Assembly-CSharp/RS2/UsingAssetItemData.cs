namespace RS2
{
	public class UsingAssetItemData
	{
		private int m_goodsId;

		private int m_productShopID = 2001;

		public int ProductShopID
		{
			get
			{
				return m_productShopID;
			}
		}

		public int GoodsId
		{
			get
			{
				return m_goodsId;
			}
		}

		public UsingAssetItemData(int goodsId, int productShopId = -1)
		{
			m_goodsId = goodsId;
			m_productShopID = productShopId;
		}
	}
}
