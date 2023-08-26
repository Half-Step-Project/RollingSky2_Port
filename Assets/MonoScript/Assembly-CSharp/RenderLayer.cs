using UnityEngine;

public class RenderLayer : MonoBehaviour
{
	public string layerName;

	private void Start()
	{
		Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].sortingLayerName = layerName;
		}
	}
}
