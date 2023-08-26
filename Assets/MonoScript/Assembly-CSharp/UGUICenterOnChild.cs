using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UGUICenterOnChild : MonoBehaviour, IEndDragHandler, IEventSystemHandler, IBeginDragHandler
{
	public enum CenterMode
	{
		centerOnChild,
		centerOnItem
	}

	public Action<GameObject> onCenterCallBack;

	public Action<GameObject> onCenterFinshed;

	public CenterMode centerMode;

	public float speed = 10f;

	private ScrollRect scroll;

	private Transform content;

	private Vector3 targetPos;

	private bool centering;

	public GameObject centeredObject { get; private set; }

	private void Awake()
	{
		scroll = GetComponentInParent<ScrollRect>();
		content = scroll.content;
	}

	private void OnDisable()
	{
		centering = false;
	}

	private Vector3 CalPickingPoint()
	{
		Vector3 zero = Vector3.zero;
		Vector2 vector = (scroll.transform as RectTransform).rect.size / 2f;
		Vector2[] array = new Vector2[4]
		{
			new Vector2(0f - vector.x, vector.y),
			new Vector2(vector.x, vector.y),
			new Vector2(vector.x, 0f - vector.y),
			new Vector2(0f - vector.x, 0f - vector.y)
		};
		if (centerMode == CenterMode.centerOnChild)
		{
			for (int i = 0; i < 4; i++)
			{
				array[i] = scroll.transform.TransformPoint(array[i]);
			}
			zero = (array[0] + array[2]) * 0.5f;
		}
		else
		{
			Vector2 vector2 = (content.GetChild(0) as RectTransform).rect.size * 0.5f;
			vector2.y *= -1f;
			zero = scroll.transform.TransformPoint(array[0] + vector2);
			Debug.Log(zero);
		}
		return zero;
	}

	public void reCenter()
	{
		Vector3 vector = CalPickingPoint();
		float num = float.MaxValue;
		Transform target = null;
		int i = 0;
		for (int childCount = content.childCount; i < childCount; i++)
		{
			Transform child = content.GetChild(i);
			if (child.gameObject.activeInHierarchy)
			{
				float num2 = Vector2.SqrMagnitude(child.position - vector);
				if (num2 <= num)
				{
					num = num2;
					target = child;
				}
			}
		}
		CenterOn(target, vector);
	}

	private void CenterOn(Transform target, Vector3 centerPos)
	{
		if (target != null && scroll != null)
		{
			centeredObject = target.gameObject;
			Vector3 vector = content.parent.InverseTransformPoint(target.position);
			Vector3 vector2 = content.parent.InverseTransformPoint(centerPos) - vector;
			if (!scroll.horizontal)
			{
				vector2.x = 0f;
			}
			if (!scroll.vertical)
			{
				vector2.y = 0f;
			}
			vector2.z = 0f;
			targetPos = content.localPosition + vector2;
			centering = true;
			if (onCenterCallBack != null)
			{
				onCenterCallBack(target.gameObject);
			}
		}
	}

	public void CenterOn(Transform target)
	{
		CenterOn(target, CalPickingPoint());
	}

	private void Update()
	{
		if (!centering)
		{
			return;
		}
		Vector2 vector = (Vector2)content.localPosition;
		content.localPosition = Vector2.Lerp(content.localPosition, targetPos, speed * Time.deltaTime);
		if (Vector2.Distance(content.localPosition, targetPos) < 0.01f)
		{
			content.localPosition = targetPos;
			centering = false;
			if (onCenterFinshed != null)
			{
				onCenterFinshed(centeredObject);
			}
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		scroll.StopMovement();
		reCenter();
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		centering = false;
		targetPos = Vector3.zero;
	}
}
