using System.Collections;
using RS2;
using UnityEngine;

public class AutoRenderDepth : MonoBehaviour
{
	public Canvas targetCanvas;

	public int offsetOrder;

	private UGUIForm targetUGUIForm;

	private int cacheTargetCanvasOrder;

	private Renderer[] _renders;

	private void Awake()
	{
		if (!(targetCanvas == null))
		{
			targetUGUIForm = targetCanvas.GetComponent<UGUIForm>();
			if (targetUGUIForm != null)
			{
				targetUGUIForm.DepthChangedEvent += OnTargetDepthChanged;
			}
			_renders = GetComponents<Renderer>();
		}
	}

	private void OnDestroy()
	{
		if (targetUGUIForm != null)
		{
			targetUGUIForm.DepthChangedEvent -= OnTargetDepthChanged;
		}
	}

	private void OnEnable()
	{
		StartCoroutine(ToSetRenderOrder());
	}

	private IEnumerator ToSetRenderOrder()
	{
		yield return new WaitForEndOfFrame();
		SetRenderOrder();
	}

	private void SetRenderOrder()
	{
		if (!(targetCanvas == null) && cacheTargetCanvasOrder != targetCanvas.sortingOrder)
		{
			cacheTargetCanvasOrder = targetCanvas.sortingOrder;
			Renderer[] renders = _renders;
			for (int i = 0; i < renders.Length; i++)
			{
				renders[i].sortingOrder = targetCanvas.sortingOrder + offsetOrder;
			}
		}
	}

	private void OnTargetDepthChanged()
	{
		SetRenderOrder();
	}
}
