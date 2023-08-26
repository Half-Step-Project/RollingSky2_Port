using System.Collections;
using DG.Tweening;
using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIMoveTarget : MonoBehaviour
{
	public int id;

	public Text text;

	public Image image;

	public Image imageFade;

	public GameObject getEffect;

	public Image totalImage;

	public Animation shineAnim;

	public Graphic addOne;

	public Text deltaCount;

	private double start;

	private double end;

	private int max = -1;

	private int goodsId;

	private int preX = -1;

	private Tweener increaseTweener;

	private UnityAction finishedCallback;

	private const float FADE_TIME = 0.6f;

	private const float DELAY_TIME = 1f;

	private void Awake()
	{
		if ((bool)getEffect)
		{
			getEffect.SetActive(false);
		}
		if ((bool)totalImage)
		{
			totalImage.gameObject.SetActive(false);
		}
		if ((bool)imageFade)
		{
			imageFade.gameObject.SetActive(false);
		}
		if ((bool)addOne)
		{
			Color color = addOne.color;
			addOne.color = new Color(color.r, color.g, color.b, 0f);
		}
		if ((bool)deltaCount)
		{
			Color color2 = deltaCount.color;
			deltaCount.color = new Color(color2.r, color2.g, color2.b, 0f);
		}
		Mod.Event.Subscribe(EventArgs<UIMoveTargetTogglePauseIncreaseEvent>.EventId, OnTogglePauseIncrease);
	}

	private void OnTogglePauseIncrease(object sender, EventArgs e)
	{
		if (increaseTweener != null)
		{
			increaseTweener.TogglePause();
		}
	}

	private void OnDestroy()
	{
		StopAllCoroutines();
		CancelInvoke();
		Mod.Event.Unsubscribe(EventArgs<UIMoveTargetTogglePauseIncreaseEvent>.EventId, OnTogglePauseIncrease);
	}

	public void SetData(double start, double end, int max, Sprite totalImageSprite = null, int goodsId = 0)
	{
		this.start = start;
		this.end = end;
		this.max = max;
		if ((bool)totalImage && (bool)totalImageSprite)
		{
			totalImage.sprite = totalImageSprite;
		}
		this.goodsId = goodsId;
	}

	public void ShowGetEffect()
	{
		if (!(getEffect == null))
		{
			getEffect.SetActive(false);
			getEffect.SetActive(true);
		}
	}

	private void IncreaseSetter(double x)
	{
		if ((bool)text)
		{
			if (max != -1)
			{
				text.text = string.Format("{0}/{1}", (int)(x % (double)max), max);
			}
			else
			{
				text.text = MonoSingleton<GameTools>.Instacne.DoubleToFormatString(x);
			}
		}
		if ((bool)image && max != -1)
		{
			image.fillAmount = (float)(x % (double)max / (double)max);
		}
		if ((bool)imageFade && max != -1)
		{
			imageFade.fillAmount = (float)(x % (double)max / (double)max);
		}
		if (!totalImage || max == -1 || goodsId == 0 || (int)x <= 0 || preX == (int)x || (int)x % max != 0)
		{
			return;
		}
		preX = (int)x;
		Mod.Event.Fire(this, UIMoveTargetTogglePauseIncreaseEvent.Make());
		GetGoodsData getGoodsData = new GetGoodsData();
		getGoodsData.GoodsId = goodsId;
		getGoodsData.GoodsNum = 1.0;
		getGoodsData.CallBackFunc = delegate
		{
			if ((bool)shineAnim)
			{
				shineAnim.Play();
				Mod.Sound.PlayUISound(20023);
				Invoke("ResumeIncreaseAfterShine", 1f);
				if ((bool)addOne)
				{
					addOne.DOFade(1f, 0.5f).OnComplete(delegate
					{
						Mod.Event.Fire(this, UIMoveTargetGetTotalItemEvent.Make(goodsId));
						addOne.DOFade(0f, 0.5f);
					});
				}
				else
				{
					Mod.Event.Fire(this, UIMoveTargetGetTotalItemEvent.Make(goodsId));
				}
			}
			else
			{
				Mod.Event.Fire(this, UIMoveTargetTogglePauseIncreaseEvent.Make());
			}
		};
		Mod.UI.OpenUIForm(UIFormId.GetGoodsForm, getGoodsData);
	}

	private void ResumeIncreaseAfterShine()
	{
		if (end == (double)max)
		{
			StartCoroutine(DelayIncreaseFinishedCallback());
			if (increaseTweener != null)
			{
				increaseTweener.Kill();
			}
		}
		else
		{
			Mod.Event.Fire(this, UIMoveTargetTogglePauseIncreaseEvent.Make());
		}
	}

	public void StartIncrease(float time = 1f, UnityAction finishedCallback = null)
	{
		this.finishedCallback = finishedCallback;
		DOTween.defaultEaseType = Ease.Linear;
		if (goodsId != 0 && max != -1)
		{
			double num = (end - start) / (double)time;
			increaseTweener = DOTween.To(IncreaseGetter, IncreaseSetter, end, (float)num).SetSpeedBased(true).OnComplete(IncreaseFinishedCallback);
			if ((bool)imageFade)
			{
				imageFade.gameObject.SetActive(true);
				Color color = imageFade.color;
				imageFade.color = new Color(color.r, color.g, color.b, 0f);
				imageFade.DOFade(1f, 0.6f);
			}
		}
		else
		{
			increaseTweener = DOTween.To(IncreaseGetter, IncreaseSetter, end, time).OnComplete(IncreaseFinishedCallback);
		}
		if ((bool)deltaCount)
		{
			deltaCount.DOFade(1f, 0.5f).OnComplete(delegate
			{
				deltaCount.DOFade(0f, 0.5f);
			});
			deltaCount.text = "+" + MonoSingleton<GameTools>.Instacne.DoubleToFormatString(end - start).ToString();
		}
	}

	private void IncreaseFinishedCallback()
	{
		if ((bool)imageFade)
		{
			imageFade.DOFade(0f, 0.6f);
		}
		if (end != (double)max)
		{
			StartCoroutine(DelayIncreaseFinishedCallback());
		}
	}

	private IEnumerator DelayIncreaseFinishedCallback()
	{
		Mod.Event.Fire(this, UIMoveTargetFinishedEvent.Make(goodsId));
		yield return new WaitForSeconds(1f);
		if (finishedCallback != null)
		{
			finishedCallback();
		}
	}

	private double IncreaseGetter()
	{
		return start;
	}
}
