using Foundation;
using UnityEngine;

public class UI3DCamera : MonoBehaviour
{
	private Camera camera3D;

	private void Awake()
	{
		camera3D = GetComponent<Camera>();
		Mod.Event.Subscribe(EventArgs<UI3DShowOrHideEvent>.EventId, OnUI3DShowOrHide);
	}

	private void OnDestroy()
	{
		Mod.Event.Unsubscribe(EventArgs<UI3DShowOrHideEvent>.EventId, OnUI3DShowOrHide);
	}

	private void OnUI3DShowOrHide(object sender, EventArgs args)
	{
		UI3DShowOrHideEvent uI3DShowOrHideEvent = args as UI3DShowOrHideEvent;
		if (uI3DShowOrHideEvent != null)
		{
			camera3D.enabled = uI3DShowOrHideEvent.IsShow;
		}
	}
}
