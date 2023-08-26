public class CommonAlertItemUpdateConfirmController : CommonAlertItemController
{
	public override void Init(CommonAlertData alertData)
	{
	}

	public override void Reset()
	{
	}

	public override void Release()
	{
	}

	public override CommonAlertData.AlertShopType GetAlertType()
	{
		return CommonAlertData.AlertShopType.AD;
	}
}
