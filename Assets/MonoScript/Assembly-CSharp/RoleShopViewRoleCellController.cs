using UnityEngine;
using UnityEngine.UI;

public class RoleShopViewRoleCellController : UILoopItem
{
	public Image roleIcon;

	public GameObject roleSelect;

	private ShopItemData gameRoleData;

	public override void Data(object data)
	{
		gameRoleData = data as ShopItemData;
		if (gameRoleData != null)
		{
			string path = GameCommon.SPRITES_PATH + gameRoleData.iconId;
			roleIcon.sprite = ResourcesManager.Load<Sprite>(path);
		}
	}

	public override object GetData()
	{
		return gameRoleData;
	}

	public override void SetSelected(bool selected)
	{
		roleSelect.SetActive(selected);
	}
}
