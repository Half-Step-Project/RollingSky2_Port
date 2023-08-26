using UnityEngine;

public abstract class UIProductItem : MonoBehaviour
{
	public abstract void OnInit(object data);

	public abstract void OnOpen();

	public abstract void OnClose();

	public abstract void OnRelease();

	public abstract void OnRefresh();
}
