using System;
using RS2;

[Serializable]
public class ShopItemData
{
	public int id;

	public int itemId;

	public string note;

	public int name;

	public int type;

	public int goodsdTeamid;

	public int count;

	public int buyType;

	public string price;

	public string product_id_android;

	public string product_id_ios;

	public int scene;

	public int iconId;

	public string showInfo;

	public int discount;

	public int hot;

	public int shopSort;

	public int buyCd;

	public ShopItemData()
	{
	}

	public ShopItemData(Shops_shopTable tableData)
	{
		Init(tableData);
	}

	public void Init(Shops_shopTable tableData)
	{
		id = tableData.Id;
		itemId = tableData.Id;
		name = tableData.Name;
		note = tableData.Note;
		type = tableData.Type;
		goodsdTeamid = tableData.GoodsTeamid;
		count = tableData.Count;
		buyType = tableData.BuyType;
		price = tableData.Price;
		product_id_android = tableData.Product_id_android;
		product_id_ios = tableData.Product_id_ios;
		scene = tableData.Sence;
		iconId = tableData.IconId;
		showInfo = tableData.ShowInfo;
		discount = tableData.Discount;
		hot = tableData.Hot;
		shopSort = tableData.Shopsort;
		buyCd = tableData.BuyCd;
	}
}
