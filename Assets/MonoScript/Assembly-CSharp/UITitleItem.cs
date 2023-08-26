using UnityEngine;

public abstract class UITitleItem : MonoBehaviour
{
	public abstract void OnInit(object TitleData);

	public abstract void OnOpen();

	public abstract void OnClose();

	public abstract void OnRelease();

	public abstract void OnRefresh();
}
