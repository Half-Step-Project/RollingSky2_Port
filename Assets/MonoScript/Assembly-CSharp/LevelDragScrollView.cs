using System.Collections.Generic;
using RS2;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelDragScrollView : MonoBehaviour, IDragHandler, IEventSystemHandler, IEndDragHandler, IBeginDragHandler
{
	public RectTransform levelsContainer;

	public UGUICenterOnChild centerOnChild;

	public RectTransform effectDrageZone;

	private GameObject currentCenterObj;

	private float dragLength;

	private bool isHadDragOperate;

	private bool isCenter;

	public LevelEnterForm m_mainForm;

	private Dictionary<int, LevelItemController> chidlDic = new Dictionary<int, LevelItemController>();

	public float m_scallMinThreshold = 0.7f;

	private float m_alphaMinThreshlod = 0.3f;

	public float m_cellStep = 360f;

	private float autoMoveDistance;

	private int childCount;

	private GameObject m_preCenteObj;

	private bool isInEffectZone = true;

	private void Start()
	{
		centerOnChild.onCenterCallBack = OnItemCenteredHandler;
		centerOnChild.onCenterFinshed = OnItemCenteredEndHandler;
	}

	private void OnDisable()
	{
		m_preCenteObj = null;
	}

	private void OnDragIng(Vector2 delta)
	{
		float x = delta.x;
		dragLength += delta.x;
		isCenter = false;
		float x2 = levelsContainer.localPosition.x;
		int childCount2 = levelsContainer.transform.childCount;
		float num = 0f;
		num += autoMoveDistance;
		x2 += x;
		float num2 = -1f * levelsContainer.sizeDelta.x;
		num2 -= autoMoveDistance;
		if (x2 > num)
		{
			x2 = num;
			return;
		}
		if (x2 < num2)
		{
			x2 = num2;
			return;
		}
		Mathf.Abs(x2 - levelsContainer.localPosition.x);
		if (x2 < 0f && x2 > num2 + autoMoveDistance && !isHadDragOperate)
		{
			m_mainForm.OnBeginDrag();
			isHadDragOperate = true;
		}
		levelsContainer.transform.localPosition = new Vector3(x2, levelsContainer.transform.localPosition.y, 0f);
		if (chidlDic.Count <= 1)
		{
			return;
		}
		int num3 = (int)(Mathf.Abs(x2) / m_cellStep);
		float num4 = Mathf.Abs(delta.x) / 720f;
		float num5 = 1f;
		float num6 = 1f;
		if (delta.x > 0f)
		{
			num3++;
			if (num3 >= childCount)
			{
				num3 = childCount - 1;
			}
			LevelItemController levelItemController = chidlDic[num3];
			num5 = levelItemController.transform.localScale.x - num4;
			num5 = ((num5 < m_scallMinThreshold) ? m_scallMinThreshold : num5);
			num6 = levelItemController.GetContentAlpha() - num4;
			num6 = ((num6 < m_alphaMinThreshlod) ? m_alphaMinThreshlod : num6);
			levelItemController.ChangeAlpha(num6);
			levelItemController.transform.localScale = new Vector3(num5, num5, 1f);
			num3--;
			if (num3 >= 0)
			{
				LevelItemController levelItemController2 = chidlDic[num3];
				num5 = levelItemController2.transform.localScale.x + num4;
				num5 = ((num5 > 1f) ? 1f : num5);
				num6 = levelItemController2.GetContentAlpha() + num4;
				num6 = ((num6 > 1f) ? 1f : num6);
				levelItemController2.ChangeAlpha(num6);
				levelItemController2.transform.localScale = new Vector3(num5, num5, 1f);
			}
		}
		else if (delta.x < 0f)
		{
			LevelItemController levelItemController3 = chidlDic[num3];
			num5 = levelItemController3.transform.localScale.x - num4;
			num5 = ((num5 < m_scallMinThreshold) ? m_scallMinThreshold : num5);
			levelItemController3.transform.localScale = new Vector3(num5, num5, 1f);
			num6 = levelItemController3.GetContentAlpha() - num4;
			num6 = ((num6 < m_alphaMinThreshlod) ? m_alphaMinThreshlod : num6);
			levelItemController3.ChangeAlpha(num6);
			num3++;
			if (num3 < childCount)
			{
				LevelItemController levelItemController4 = chidlDic[num3];
				num5 = levelItemController4.transform.localScale.x + num4;
				num5 = ((num5 > 1f) ? 1f : num5);
				levelItemController4.transform.localScale = new Vector3(num5, num5, 1f);
				num6 = levelItemController4.GetContentAlpha() + num4;
				num6 = ((num6 > 1f) ? 1f : num6);
				levelItemController4.ChangeAlpha(num6);
			}
		}
	}

	private int RoundToInt(float f)
	{
		if (f < 0f)
		{
			return 0;
		}
		int num = (int)f;
		if (!(f - (float)num >= 0.5f))
		{
			return num;
		}
		return num + 1;
	}

	private void OnItemCenteredHandler(GameObject obj)
	{
		if (!(m_preCenteObj != obj))
		{
			return;
		}
		m_preCenteObj = obj;
		LevelItemController component = obj.GetComponent<LevelItemController>();
		if (!(component != null))
		{
			return;
		}
		m_mainForm.OnCenterStart(component.GetLevelId());
		int result = 0;
		if (int.TryParse(obj.name, out result))
		{
			int num = result + 1;
			if (num >= childCount)
			{
				num = childCount - 1;
			}
			if (num != result && chidlDic.ContainsKey(num))
			{
				chidlDic[num].transform.localScale = new Vector3(m_scallMinThreshold, m_scallMinThreshold, 1f);
			}
			int num2 = result - 1;
			if (num2 < 0)
			{
				num2 = 0;
			}
			if (num2 != result && chidlDic.ContainsKey(num2))
			{
				chidlDic[num2].transform.localScale = new Vector3(m_scallMinThreshold, m_scallMinThreshold, 1f);
			}
		}
	}

	private void OnItemCenteredEndHandler(GameObject obj)
	{
		m_mainForm.OnCenterEnd();
	}

	private int GetIndex(Transform trans)
	{
		int num = levelsContainer.transform.childCount;
		for (int i = 0; i < num; i++)
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
		dragLength = 0f;
		isHadDragOperate = false;
		m_preCenteObj = null;
		isInEffectZone = true;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		isInEffectZone = MonoSingleton<GameTools>.Instacne.IsClickInItemZone(effectDrageZone);
		if (!isInEffectZone)
		{
			return;
		}
		chidlDic.Clear();
		if (chidlDic.Count <= 0)
		{
			Transform transform = null;
			LevelItemController levelItemController = null;
			for (int i = 0; i < levelsContainer.childCount; i++)
			{
				transform = levelsContainer.GetChild(i);
				levelItemController = transform.GetComponent<LevelItemController>();
				chidlDic.Add(int.Parse(transform.name), levelItemController);
			}
			childCount = chidlDic.Count;
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (isInEffectZone && (eventData.delta.x >= 0.001f || eventData.delta.x <= -0.001f))
		{
			OnDragIng(eventData.delta);
		}
	}

	public void Release()
	{
		chidlDic.Clear();
	}
}
