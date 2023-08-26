using System.Collections;
using Foundation;
using RS2;
using UnityEngine;

public class SetRenderDepthGroup : MonoBehaviour
{
	public int offsetOrder;

	[HideInInspector]
	public Canvas targetCanvas;

	[HideInInspector]
	public ParticleSystem[] particles;

	private UGUIForm targetUGUIForm;

	private int cacheTargetCanvasOrder = -1;

	private void Awake()
	{
		particles = GetComponentsInChildren<ParticleSystem>(true);
	}

	public void SetTargetCanvas(Canvas canvas)
	{
		if (!(canvas == null))
		{
			targetCanvas = canvas;
			targetUGUIForm = targetCanvas.GetComponent<UGUIForm>();
			if (targetUGUIForm != null)
			{
				targetUGUIForm.DepthChangedEvent += OnTargetDepthChanged;
			}
			SetRenderOrder();
		}
	}

	private void OnDestroy()
	{
		if (targetUGUIForm != null)
		{
			targetUGUIForm.DepthChangedEvent -= OnTargetDepthChanged;
		}
	}

	private void OnTargetDepthChanged()
	{
		if (base.gameObject.activeInHierarchy)
		{
			Log.Info("SetRenderDepthGroup OnTargetDepthChanged");
			SetRenderOrder();
		}
	}

	private IEnumerator ToSetTargetCanvas()
	{
		yield return new WaitForEndOfFrame();
		ParticleSystem[] array = particles;
		for (int i = 0; i < array.Length; i++)
		{
			Renderer component = array[i].GetComponent<Renderer>();
			component.sortingOrder = component.sortingOrder + targetCanvas.sortingOrder + offsetOrder;
		}
	}

	private void SetRenderOrder()
	{
		if (cacheTargetCanvasOrder != targetCanvas.sortingOrder)
		{
			cacheTargetCanvasOrder = targetCanvas.sortingOrder;
			StartCoroutine(ToSetTargetCanvas());
		}
	}
}
