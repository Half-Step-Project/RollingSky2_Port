using RS2;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelSeriesDragScrollView : MonoBehaviour, IDragHandler, IEventSystemHandler, IEndDragHandler, IBeginDragHandler
{
	public ScrollRect scrollView;

	public RectTransform levelsContainer;

	private UGUICenterOnChild centerOnChild;

	public GameObject currentCenterObj;

	public GameObject currentShowObj;

	private float dragLength;

	private LevelSeriesController dragItemController;

	public bool isDrag;

	private bool isCenter;

	public HomeForm m_home;

	private Transform current;

	private Transform next;

	private Transform last;

	private Transform preCurrent;

	private Transform preNext;

	private Transform preLast;

	private float moveLeft;

	private float moveRight;

	private bool isEndDrag;

	private bool DragEnable
	{
		get
		{
			return m_home.State == HomeFormState.SelectLevel;
		}
	}

	private void Start()
	{
		FindScrollView();
	}

	private void FindScrollView()
	{
		scrollView = GetComponentInParent<ScrollRect>();
		levelsContainer = scrollView.content;
		centerOnChild = GetComponent<UGUICenterOnChild>();
		centerOnChild.onCenterCallBack = OnItemCenteredHandler;
		centerOnChild.speed = 12f;
		m_home = GetComponentInParent<HomeForm>();
	}

	private void OnDragIng(Vector2 delta)
	{
		if (!DragEnable)
		{
			return;
		}
		float x = delta.x;
		dragLength += delta.x;
		isCenter = false;
		float x2 = levelsContainer.localPosition.x;
		int childCount = levelsContainer.transform.childCount;
		int num = childCount - 1;
		x2 += x;
		float num2 = 0f;
		x2 += x;
		float num3 = HomeForm.CellH * (float)num * -1f;
		if (x2 > num2)
		{
			x2 = num2;
			return;
		}
		if (x2 < num3)
		{
			x2 = num3;
			return;
		}
		levelsContainer.transform.localPosition = new Vector3(x2, 0f, 0f);
		int num4 = Mathf.CeilToInt(Mathf.Abs(x2) / HomeForm.CellH);
		float num5 = Mathf.Abs(delta.x) / (float)Screen.width;
		if (delta.x > 0f)
		{
			current = levelsContainer.transform.GetChild(num4);
			if (!(current != null))
			{
				return;
			}
			dragItemController = current.GetComponent<LevelSeriesController>();
			num4--;
			if (num4 < 0)
			{
				num4 = 0;
				return;
			}
			last = levelsContainer.transform.GetChild(num4);
			if (last != null)
			{
				dragItemController = last.GetComponent<LevelSeriesController>();
			}
		}
		else
		{
			if (!(delta.x < 0f))
			{
				return;
			}
			num4 = (int)(Mathf.Abs(x2) / HomeForm.CellH);
			current = levelsContainer.transform.GetChild(num4);
			if (!(current != null))
			{
				return;
			}
			dragItemController = current.GetComponent<LevelSeriesController>();
			num4++;
			if (num4 < childCount)
			{
				next = levelsContainer.transform.GetChild(num4);
				if (next != null)
				{
					dragItemController = next.GetComponent<LevelSeriesController>();
				}
			}
		}
	}

	private void ShowUIStateByScaleThreshold(Transform target, bool flag)
	{
		if (target != null && target.GetComponent<LevelSeriesController>() != null && flag)
		{
			if (currentShowObj != target && currentShowObj != null)
			{
				currentShowObj.GetComponent<LevelSeriesController>();
			}
			currentShowObj = target.gameObject;
		}
	}

	private void OnItemCenteredHandler(GameObject obj)
	{
		if (isCenter)
		{
			return;
		}
		if (currentCenterObj != obj)
		{
			if (currentCenterObj != null)
			{
				currentCenterObj.GetComponent<LevelSeriesController>();
			}
			currentCenterObj = obj;
			if (currentCenterObj != null)
			{
				bool flag = currentCenterObj.GetComponent<LevelSeriesController>() != null;
			}
		}
		if (m_home != null)
		{
			m_home.SetDirectionShowState();
		}
		isCenter = true;
	}

	private void OnItemCenteredScaleHandler()
	{
		int childCount = levelsContainer.transform.childCount;
		if (childCount != 1 && currentCenterObj != null)
		{
			int index = GetIndex(currentCenterObj.transform);
			int num = 0;
			if (index > 0)
			{
				num = index - 1;
				Transform child = levelsContainer.transform.GetChild(num);
				dragItemController = child.GetComponent<LevelSeriesController>();
			}
			if (index < childCount - 1)
			{
				num = index + 1;
				Transform child2 = levelsContainer.transform.GetChild(num);
				dragItemController = child2.GetComponent<LevelSeriesController>();
			}
		}
	}

	private int GetIndex(Transform trans)
	{
		int childCount = levelsContainer.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			if (levelsContainer.transform.GetChild(i) == trans)
			{
				return i;
			}
		}
		return -1;
	}

	private GameObject FindCennterLevel()
	{
		if (levelsContainer != null)
		{
			int index = Mathf.RoundToInt(Mathf.Abs(levelsContainer.localPosition.x * -1f) / HomeForm.CellH);
			Transform child = levelsContainer.transform.GetChild(index);
			if (!(child == null))
			{
				return child.gameObject;
			}
			return null;
		}
		return null;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (DragEnable)
		{
			dragLength = 0f;
			isDrag = false;
			preCurrent = null;
			preNext = null;
			preLast = null;
			moveLeft = 0f;
			moveRight = 0f;
			isEndDrag = false;
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (DragEnable)
		{
			isDrag = true;
			if (currentCenterObj == null)
			{
				currentCenterObj = FindCennterLevel();
			}
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (DragEnable && (eventData.delta.x >= 0.001f || eventData.delta.x <= -0.001f))
		{
			NewDrageing(eventData.delta);
		}
	}

	private void EaseOutBackground(Transform target)
	{
		if (target != null)
		{
			LevelBackgroundBlur componentInChildren = target.GetComponentInChildren<LevelBackgroundBlur>();
			if (componentInChildren != null)
			{
				componentInChildren.Enable();
			}
		}
	}

	private void EaseInNextBackground(Transform target)
	{
		if (target != null)
		{
			LevelBackgroundBlur componentInChildren = target.GetComponentInChildren<LevelBackgroundBlur>();
			if (componentInChildren != null)
			{
				componentInChildren.Enable();
			}
		}
	}

	private void StopEase(Transform target)
	{
		if (target != null)
		{
			LevelBackgroundBlur componentInChildren = target.GetComponentInChildren<LevelBackgroundBlur>();
			if (componentInChildren != null)
			{
				componentInChildren.Disable();
			}
		}
	}

	private void NewDrageing(Vector2 delta)
	{
		if (isEndDrag)
		{
			return;
		}
		if (delta.x > 0f)
		{
			moveRight += delta.x;
			if (moveRight > 50f)
			{
				m_home.LeftMoveHandle();
				moveRight = 0f;
				isEndDrag = true;
			}
		}
		else if (delta.x < 0f)
		{
			moveLeft += delta.x;
			if (moveLeft < -50f)
			{
				m_home.RightMoveHandle();
				moveLeft = 0f;
				isEndDrag = true;
			}
		}
	}
}
