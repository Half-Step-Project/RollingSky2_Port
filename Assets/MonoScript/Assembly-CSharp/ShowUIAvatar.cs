using UnityEngine;

public class ShowUIAvatar : Singleton<ShowUIAvatar>
{
	private GameObject stage;

	public void ShowAvatar(GameObject childAvatar, int layer, bool isCanRotate, Vector3 cameraPos)
	{
		RemoveAvatar();
		if (stage == null)
		{
			stage = Object.Instantiate(ResourcesManager.Load<GameObject>("Prefab/UI/Stage"));
		}
		Transform parent = stage.transform.Find("stageContainer");
		childAvatar.transform.SetParent(parent);
		childAvatar.transform.localPosition = Vector3.zero;
		childAvatar.transform.localRotation = Quaternion.identity;
		childAvatar.transform.localScale = Vector3.one;
		Camera component = stage.transform.Find("Camera").GetComponent<Camera>();
		component.cullingMask = 1 << layer;
		if (component != null)
		{
			NGUIToolsSetLayer(childAvatar, layer);
			component.transform.localPosition = cameraPos;
		}
		if (isCanRotate)
		{
			childAvatar.AddComponent<SpinWithMouse>();
		}
	}

	public void RemoveAvatar()
	{
		if (stage == null)
		{
			stage = Object.Instantiate(ResourcesManager.Load<GameObject>("Prefab/UI/Stage"));
		}
		Transform transform = stage.transform.Find("stageContainer");
		if (transform.childCount > 0)
		{
			GameObject gameObject = transform.GetChild(0).gameObject;
			if (gameObject != null)
			{
				NGUIToolDestroy(gameObject.gameObject);
			}
		}
	}

	public static void NGUIToolDestroy(Object obj)
	{
		if (!(obj != null))
		{
			return;
		}
		if (Application.isPlaying)
		{
			if (obj is GameObject)
			{
				(obj as GameObject).transform.parent = null;
			}
			Object.Destroy(obj);
		}
		else
		{
			Object.DestroyImmediate(obj);
		}
	}

	public static void NGUIToolsSetLayer(GameObject go, int layer)
	{
		go.layer = layer;
		Transform transform = go.transform;
		int i = 0;
		for (int childCount = transform.childCount; i < childCount; i++)
		{
			NGUIToolsSetLayer(transform.GetChild(i).gameObject, layer);
		}
	}
}
