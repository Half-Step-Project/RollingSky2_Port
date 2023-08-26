using UnityEngine;
using UnityEngine.UI;

public class RoleShopViewPetCellController : UILoopItem
{
	public Image roleIcon;

	public GameObject roleSelect;

	private ShopItemData gamePetData;

	public override void Data(object data)
	{
		gamePetData = data as ShopItemData;
		if (gamePetData != null)
		{
			string path = GameCommon.SPRITES_PATH + gamePetData.iconId;
			roleIcon.sprite = ResourcesManager.Load<Sprite>(path);
		}
	}

	public override object GetData()
	{
		return gamePetData;
	}

	public override void SetSelected(bool selected)
	{
		roleSelect.SetActive(selected);
	}
}
