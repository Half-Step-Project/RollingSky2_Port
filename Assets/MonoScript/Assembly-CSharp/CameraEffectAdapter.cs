using Foundation;
using UnityEngine;

public class CameraEffectAdapter : MonoBehaviour
{
	public Camera TargetCamera;

	public int SortingOrder;

	protected void Start()
	{
		if (TargetCamera == null)
		{
			TargetCamera = Camera.main;
			Log.Warning("CameraEffectAdapter: TargetCamera == null, use Camera.main.", base.gameObject);
		}
		float num = (float)Screen.width / (float)Screen.height;
		float fieldOfView = TargetCamera.fieldOfView;
		float magnitude = (TargetCamera.transform.position - base.transform.position).magnitude;
		float num2 = Mathf.Tan(MathUtils.ToRadians(fieldOfView * 0.5f)) * magnitude * 2f;
		float x = num * num2;
		base.transform.localScale = new Vector3(x, num2, 1f);
		MeshRenderer component = base.gameObject.GetComponent<MeshRenderer>();
		component.sortingLayerName = "CameraEffect";
		component.sortingOrder = SortingOrder;
	}
}
