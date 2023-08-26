using System.Collections.Generic;

public class ShopUIGroupProductListData
{
	public Dictionary<ShopType, ShopResponseData> m_responseDatas = new Dictionary<ShopType, ShopResponseData>();

	public Dictionary<ShopType, List<ShopUIProductItemData>> m_normalDatas = new Dictionary<ShopType, List<ShopUIProductItemData>>();
}
