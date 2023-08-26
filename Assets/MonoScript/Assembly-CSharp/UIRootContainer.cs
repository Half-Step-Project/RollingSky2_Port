using System.Collections.Generic;
using UnityEngine;

public sealed class UIRootContainer : MonoBehaviour
{
	[SerializeField]
	private List<Canvas> uiLayerList;

	public List<Canvas> UiLayerList
	{
		get
		{
			return uiLayerList;
		}
	}

	public Canvas GetUILayerContainer(int layer)
	{
		if (uiLayerList != null && uiLayerList.Count > layer)
		{
			return uiLayerList[layer];
		}
		return null;
	}
}
