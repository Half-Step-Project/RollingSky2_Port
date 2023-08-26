using RS2;
using UnityEngine;
using UnityEngine.UI;

public class MoneyShopItemController : UILoopItem
{
	private ShopItemData itemData;

	public Image goodsIcon;

	public Text goodsNumTxt;

	public Text buyBtnTxt;

	public GameObject buyBtn;

	public override void Data(object data)
	{
		itemData = data as ShopItemData;
		if (itemData == null)
		{
			return;
		}
		string path = GameCommon.ICON_PATH + itemData.iconId;
		goodsIcon.sprite = ResourcesManager.Load<Sprite>(path);
		goodsIcon.SetNativeSize();
		buyBtnTxt.text = itemData.price;
		GoodsTeam_goodsTeamTable goodsTeamTableById = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).GetGoodsTeamTableById(itemData.goodsdTeamid);
		if (goodsTeamTableById != null)
		{
			string[] array = goodsTeamTableById.GoodsCount.Split(':');
			int num = 1;
			if (array.Length != 0)
			{
				num = int.Parse(array[0]);
				goodsNumTxt.text = "x" + itemData.count * num;
			}
		}
		EventTriggerListener.Get(buyBtn).onClick = OnBuyBtnClickHandle;
	}

	private void OnBuyBtnClickHandle(GameObject obj)
	{
	}

	public override object GetData()
	{
		return itemData;
	}

	public override void SetSelected(bool selected)
	{
	}
}
