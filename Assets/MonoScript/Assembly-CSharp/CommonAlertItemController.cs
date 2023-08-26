using UnityEngine;

public abstract class CommonAlertItemController : MonoBehaviour
{
	public abstract void Init(CommonAlertData alertData);

	public abstract void Reset();

	public abstract void Release();

	public abstract CommonAlertData.AlertShopType GetAlertType();
}
