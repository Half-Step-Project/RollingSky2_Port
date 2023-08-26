using DG.Tweening;
using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ResultGetGoldRoot : MonoBehaviour
{
	public class ShowData
	{
		public UnityAction increaseFinishedCallback;

		public double baseCount;

		public double batonCount;

		public double levelCount;

		public double totalCount;
	}

	private const float BATON_COUNT_INCREASE_TIME = 0.6f;

	private const float LEVEL_COUNT_INCREASE_TIME = 0.9f;

	public Text goldCount;

	public Text baseCount;

	public Text batonCount;

	public Text levelCount;

	public ResultMoveItem moveItem;

	private Vector3 originnalPos;

	private ShowData showData;

	private Tweener increaseTweener;

	private Tweener increaseMultiTweener;

	private UnityAction increaseMultiFinishedCallback;

	private int multi;

	public bool IsShow { get; private set; }

	public void Show(ShowData showData)
	{
		IsShow = true;
		this.showData = showData;
		base.gameObject.SetActive(true);
		base.transform.localPosition = originnalPos;
		goldCount.text = showData.baseCount.ToString("0");
		baseCount.text = string.Format(Mod.Localization.GetInfoById(248), MonoSingleton<GameTools>.Instacne.DoubleToFormatString(showData.baseCount));
		batonCount.text = string.Format(Mod.Localization.GetInfoById(249), showData.batonCount);
		levelCount.text = string.Format(Mod.Localization.GetInfoById(250), showData.levelCount);
	}

	private void IncreaseSetter(double x)
	{
		goldCount.text = MonoSingleton<GameTools>.Instacne.DoubleToFormatString(x);
	}

	private Tweener DoIncrease(double start, double end, float time, TweenCallback completeCallback)
	{
		increaseTweener = DOTween.To(() => start, IncreaseSetter, end, time).OnComplete(completeCallback).SetEase(Ease.Linear);
		return increaseTweener;
	}

	public void StartIncrease()
	{
		float time = 0.6f;
		if (showData.levelCount > 0.0)
		{
			time = 0.9f;
		}
		DoIncrease(showData.baseCount, showData.totalCount, time, delegate
		{
			if (showData.increaseFinishedCallback != null)
			{
				showData.increaseFinishedCallback();
			}
		});
		Mod.Sound.PlayUISound(20019);
	}

	public void SkipIncrease()
	{
		if (increaseTweener != null)
		{
			increaseTweener.Kill();
		}
		goldCount.text = showData.totalCount.ToString();
		if (showData.increaseFinishedCallback != null)
		{
			showData.increaseFinishedCallback();
		}
	}

	public void StartIncreaseMulti(int multi, UnityAction increaseMultiFinishedCallback)
	{
		this.increaseMultiFinishedCallback = increaseMultiFinishedCallback;
		this.multi = multi;
		increaseMultiTweener = DOTween.To(() => showData.totalCount, delegate(double x)
		{
			goldCount.text = MonoSingleton<GameTools>.Instacne.DoubleToFormatString(x);
		}, showData.totalCount * (double)multi, 1f).OnComplete(delegate
		{
			if (increaseMultiFinishedCallback != null)
			{
				increaseMultiFinishedCallback();
			}
		});
		Mod.Sound.PlayUISound(20019);
	}

	public void SkipIncreaseMulti()
	{
		if (increaseMultiTweener != null)
		{
			increaseMultiTweener.Kill();
		}
		goldCount.text = (showData.totalCount * (double)multi).ToString();
		if (increaseMultiFinishedCallback != null)
		{
			increaseMultiFinishedCallback();
		}
	}

	public void Hide()
	{
		IsShow = false;
		base.transform.localPosition = new Vector3(originnalPos.x + 5000f, originnalPos.y, originnalPos.z);
		CancelInvoke();
	}

	private void Awake()
	{
		originnalPos = base.transform.localPosition;
	}

	private void OnDestroy()
	{
		CancelInvoke();
	}
}
