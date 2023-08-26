using UnityEngine;

public class RenderDepth : MonoBehaviour
{
	public int order;

	private void Start()
	{
		Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].sortingOrder = order;
		}
	}
}
