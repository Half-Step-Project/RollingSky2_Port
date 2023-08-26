using UnityEngine;

public class CameraEffectMaterialHelper : MonoBehaviour
{
	public GameObject TargetGameObject;

	public Material MyMaterial;

	public float Scale = 1f;

	private bool m_DebugMode;

	private float m_Aspect = (float)Screen.height / (float)Screen.width;

	private const string m_ScreenPositionPropertyName = "_ScreenPosition";

	private void Start()
	{
		m_Aspect = (float)Screen.height / (float)Screen.width;
		if (TargetGameObject == null)
		{
			TargetGameObject = base.gameObject;
		}
		Vector3 position = TargetGameObject.transform.position;
		Vector3 vector = Camera.main.WorldToScreenPoint(position);
		float x = vector.x / (float)Screen.width;
		float y = vector.y / (float)Screen.height;
		if (MyMaterial == null)
		{
			CameraEffectAdapter[] array = Resources.FindObjectsOfTypeAll<CameraEffectAdapter>();
			int num = array.Length;
			if (num > 1 || num == 0)
			{
				base.enabled = false;
				Debug.LogErrorFormat("Unexpected cameraEffectAdapters.Length({0}).Disable CameraEffectMaterialHelper Immediately!", num);
			}
			else
			{
				Material sharedMaterial = array[0].gameObject.GetComponent<MeshRenderer>().sharedMaterial;
				if ((bool)sharedMaterial && sharedMaterial.HasProperty("_ScreenPosition"))
				{
					MyMaterial = sharedMaterial;
				}
				else
				{
					Debug.LogErrorFormat("Get valid material failed! SharedMaterial({0}) HasProperty({1})(false)", (sharedMaterial == null) ? "null" : sharedMaterial.name, "_ScreenPosition");
				}
			}
		}
		if (MyMaterial != null)
		{
			float num2 = 1f / Mathf.Max(float.Epsilon, Scale);
			Vector4 value = new Vector4(x, y, num2, num2 * m_Aspect);
			MyMaterial.SetVector("_ScreenPosition", value);
		}
	}

	private void Update()
	{
		if (TargetGameObject == null)
		{
			TargetGameObject = base.gameObject;
		}
		Vector3 position = TargetGameObject.transform.position;
		Vector3 vector = Camera.main.WorldToScreenPoint(position);
		float x = vector.x / (float)Screen.width;
		float y = vector.y / (float)Screen.height;
		if (MyMaterial != null)
		{
			float num = 1f / Mathf.Max(float.Epsilon, Scale);
			Vector4 value = new Vector4(x, y, num, num * m_Aspect);
			MyMaterial.SetVector("_ScreenPosition", value);
		}
	}

	[ContextMenu("DebugMode")]
	private void SetDebugMode()
	{
		m_DebugMode = !m_DebugMode;
		Debug.LogWarningFormat("DebugMode({0}).", m_DebugMode);
	}
}
