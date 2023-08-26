using System.Collections;
using System.Collections.Generic;
using RS2;
using UnityEngine;

public class AutoRenderDepthGroup : MonoBehaviour
{
	public Canvas targetCanvas;

	public int offsetOrder;

	public SpriteMask particleSystemMask;

	private UGUIForm targetUGUIForm;

	private int cacheTargetCanvasOrder = -1;

	private Renderer[] renders;

	private Dictionary<int, int> originalOrder = new Dictionary<int, int>();

	private void Awake()
	{
		Init();
	}

	private void Init()
	{
		if (!(targetCanvas == null))
		{
			targetUGUIForm = targetCanvas.GetComponent<UGUIForm>();
			if (targetUGUIForm != null)
			{
				targetUGUIForm.DepthChangedEvent += OnTargetDepthChanged;
			}
			renders = GetComponentsInChildren<Renderer>(true);
			originalOrder.Clear();
			for (int i = 0; i < renders.Length; i++)
			{
				originalOrder.Add(renders[i].GetInstanceID(), renders[i].sortingOrder);
			}
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
			Renderer[] array = renders;
			foreach (Renderer renderer in array)
			{
				renderer.sortingOrder = originalOrder[renderer.GetInstanceID()];
			}
			int num = int.MaxValue;
			int num2 = int.MaxValue;
			int num3 = int.MinValue;
			int num4 = int.MinValue;
			array = renders;
			foreach (Renderer obj in array)
			{
				obj.sortingOrder = obj.sortingOrder + targetCanvas.sortingOrder + offsetOrder;
				num2 = Mathf.Min(obj.sortingOrder, num2);
				num4 = Mathf.Max(obj.sortingOrder, num4);
				num = Mathf.Min(obj.sortingLayerID, num);
				num3 = Mathf.Max(obj.sortingLayerID, num3);
			}
			if ((bool)particleSystemMask)
			{
				particleSystemMask.frontSortingLayerID = num3;
				particleSystemMask.frontSortingOrder = num4;
				particleSystemMask.backSortingLayerID = num;
				particleSystemMask.backSortingOrder = Mathf.Max(0, num2 - 1);
			}
		}
	}

	private void OnTargetDepthChanged()
	{
		SetRenderOrder();
	}
}
