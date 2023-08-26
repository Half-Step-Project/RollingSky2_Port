using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.Events;

public class ResultMoveItem : MonoBehaviour
{
	public const int Max_COUNT = 20;

	public const float MOVE_TIME = 0.75f;

	public const float INTERVAL_MIN = 0f;

	public const float INTERVAL_MAX = 0.5f;

	public const float MOVE_TO_START_TIME = 0.2f;

	private const float START_INTERVAL_MIN = 0f;

	public const float START_INTERVAL_MAX = 0.4f;

	public int moveTargetId;

	public AutoReleaseImage itemTemplate;

	private List<AutoReleaseImage> items = new List<AutoReleaseImage>();

	private List<Vector3> startPositions = new List<Vector3>();

	private UnityAction moveFinishedCallback;

	private List<float> intervals = new List<float>();

	private float intervalMin = 0.5f;

	private float intervalMax;

	private List<float> startIntervals = new List<float>();

	private float startIntervalMin = 0.4f;

	private float startIntervalMax;

	private UIMoveTarget moveTarget;

	private float randomStartPosRange;

	private string animTrigger;

	private Vector2 itemSize = Vector2.zero;

	private int goodsId;

	public bool IsMoving { get; set; }

	public void Init()
	{
		IsMoving = false;
		itemTemplate.gameObject.SetActive(false);
		items.Clear();
		for (int i = 0; i < 20; i++)
		{
			AutoReleaseImage autoReleaseImage = Object.Instantiate(itemTemplate);
			autoReleaseImage.transform.SetParent(base.transform, false);
			autoReleaseImage.gameObject.SetActive(false);
			items.Insert(0, autoReleaseImage);
		}
	}

	public float StartMove(int goodsId, UIMoveTarget moveTarget, int count, Vector2 itemSize, string animTrigger = "gold", UnityAction moveFinishedCallback = null, float randomStartPosRange = 150f)
	{
		this.moveTarget = moveTarget;
		this.moveFinishedCallback = moveFinishedCallback;
		this.randomStartPosRange = randomStartPosRange;
		this.animTrigger = animTrigger;
		this.itemSize = itemSize;
		this.goodsId = goodsId;
		if (count % 4 != 0)
		{
			count += 4 - count % 4;
		}
		count = Mathf.Min(count, 20);
		int num = count / 4;
		startPositions.Clear();
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < num; j++)
			{
				AutoReleaseImage autoReleaseImage = items[i * num + j];
				autoReleaseImage.gameObject.SetActive(true);
				if (goodsId != -1)
				{
					autoReleaseImage.SetImageByGoodsId(goodsId);
				}
				autoReleaseImage.gameObject.SetActive(false);
				if (randomStartPosRange > 0f)
				{
					Vector3 localPosition = autoReleaseImage.transform.localPosition;
					float x = 0f;
					float y = 0f;
					switch (i)
					{
					case 0:
						x = Random.Range(0f - randomStartPosRange, 0f);
						y = Random.Range(0f - randomStartPosRange, 0f);
						break;
					case 1:
						x = Random.Range(0f - randomStartPosRange, 0f);
						y = Random.Range(0f, randomStartPosRange);
						break;
					case 2:
						x = Random.Range(0f, randomStartPosRange);
						y = Random.Range(0f, randomStartPosRange);
						break;
					case 3:
						x = Random.Range(0f, randomStartPosRange);
						y = Random.Range(0f - randomStartPosRange, 0f);
						break;
					}
					startPositions.Add(new Vector3(x, y, localPosition.z));
				}
			}
		}
		if (randomStartPosRange > 0f)
		{
			startIntervals.Clear();
			for (int k = 0; k < count; k++)
			{
				float num2 = Random.Range(0f, 0.4f);
				startIntervalMin = Mathf.Min(startIntervalMin, num2);
				startIntervalMax = Mathf.Max(startIntervalMax, num2);
				startIntervals.Add(num2);
			}
		}
		else
		{
			intervals.Clear();
			for (int l = 0; l < count; l++)
			{
				float num3 = Random.Range(0f, 0.5f);
				intervalMin = Mathf.Min(intervalMin, num3);
				intervalMax = Mathf.Max(intervalMax, num3);
				intervals.Add(num3);
			}
		}
		IsMoving = true;
		Invoke("MoveFinished", TotalTime());
		return TotalTime();
	}

	private float TotalTime()
	{
		if (randomStartPosRange > 0f)
		{
			return startIntervalMax + 0.2f + 0.75f;
		}
		return intervalMax + 0.75f;
	}

	private IEnumerator MoveTargetStartIncrease()
	{
		float num = ((!(randomStartPosRange > 0f)) ? (intervalMin + 0.75f) : (startIntervalMin + 0.2f + 0.75f));
		float totalTime = TotalTime() - num;
		yield return new WaitForSeconds(num);
		moveTarget.StartIncrease(totalTime);
	}

	private void MoveItems(int count)
	{
		for (int i = 0; i < count; i++)
		{
			StartCoroutine(MoveOneItem(i));
		}
	}

	private void SetItemSize(AutoReleaseImage item)
	{
		if (!(itemSize == Vector2.zero))
		{
			item.GetComponent<RectTransform>().sizeDelta = itemSize;
		}
	}

	private void SetAnimTrigger(AutoReleaseImage item)
	{
		if (!string.IsNullOrEmpty(animTrigger))
		{
			Animator component = item.GetComponent<Animator>();
			if ((bool)component)
			{
				component.SetTrigger(animTrigger);
			}
		}
	}

	private IEnumerator MoveOneItem(int index)
	{
		AutoReleaseImage item = items[index];
		item.gameObject.SetActive(true);
		item.transform.localScale = Vector3.one;
		item.transform.localPosition = Vector3.zero;
		SetAnimTrigger(item);
		SetItemSize(item);
		if (randomStartPosRange > 0f)
		{
			yield return new WaitForSeconds(startIntervals[index]);
			item.transform.DOLocalMove(startPositions[index], 0.2f).SetEase(Ease.OutCubic).OnComplete(delegate
			{
				MoveToTarget(item);
			});
			if (goodsId == 3)
			{
				Mod.Sound.PlayUISound(20017);
			}
			else if (goodsId == 6)
			{
				Mod.Sound.PlayUISound(20024);
			}
		}
		else
		{
			yield return new WaitForSeconds(intervals[index]);
			MoveToTarget(item);
		}
	}

	private void MoveToTarget(AutoReleaseImage item)
	{
		item.transform.DOMove(moveTarget.transform.position, 0.75f).SetEase(Ease.InCubic).OnComplete(delegate
		{
			item.gameObject.SetActive(false);
			moveTarget.ShowGetEffect();
			if (goodsId == 3)
			{
				Mod.Sound.PlayUISound(20018);
			}
			else if (goodsId == 6)
			{
				Mod.Sound.PlayUISound(20025);
			}
		});
		item.transform.DOScale(0.2f, 0.75f).SetEase(Ease.InExpo);
	}

	private void MoveFinished()
	{
		IsMoving = false;
		if (moveFinishedCallback != null)
		{
			moveFinishedCallback();
		}
	}

	private void OnDestroy()
	{
		StopAllCoroutines();
		CancelInvoke();
	}
}
