using System.Collections.Generic;
using DG.Tweening;
using My.Core;
using RS2;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ResultLevelGoalItem : UILoopItem
{
	private LevelGoalItemData itemData;

	private Text targetDec;

	private Text rewardNumTxt;

	private Image goodsIcon;

	private Image goldIcon;

	private Button refreshBtn;

	private List<GameObject> startsList;

	private Text doubleTxt;

	private float startY;

	private int awardNum;

	private CanvasGroup canvasCroup;

	private GameObject Effect_JS_star01;

	private Vector3 Effect_JS_star01_localPosition;

	private int rewardCount;

	public override void Data(object data)
	{
		Init();
		itemData = data as LevelGoalItemData;
		rewardCount = 0;
		UpdateContent();
	}

	private void Init()
	{
		if (targetDec == null)
		{
			targetDec = base.transform.Find("targetDec").GetComponent<Text>();
		}
		if (rewardNumTxt == null)
		{
			rewardNumTxt = base.transform.Find("rewareContent/rewardNumTxt").GetComponent<Text>();
		}
		if (goodsIcon == null)
		{
			goodsIcon = base.transform.Find("rewareContent/goodsIcon").GetComponent<Image>();
		}
		if (goldIcon == null)
		{
			goldIcon = base.transform.Find("rewareContent/goldIcon").GetComponent<Image>();
		}
		if (refreshBtn == null)
		{
			refreshBtn = base.transform.Find("refreshBtn").GetComponent<Button>();
			refreshBtn.gameObject.SetActive(false);
		}
		if (doubleTxt == null)
		{
			doubleTxt = base.transform.Find("doubleTxt").GetComponent<Text>();
			doubleTxt.text = "";
			startY = doubleTxt.transform.localPosition.y;
		}
		if (Effect_JS_star01 == null)
		{
			Effect_JS_star01 = base.transform.Find("Effect_JS_star01").gameObject;
			Effect_JS_star01.gameObject.SetActive(false);
			Effect_JS_star01_localPosition = Effect_JS_star01.transform.localPosition;
		}
		EventTriggerListener.Get(refreshBtn.gameObject).onClick = RefreshBtnHandle;
		if (!My.Core.Singleton<ADHelper>.Instance.RewardeVideoCanShow(ADScene.LevelTargetRefresh))
		{
			refreshBtn.interactable = false;
		}
		else
		{
			refreshBtn.interactable = true;
		}
		if (canvasCroup == null)
		{
			canvasCroup = base.gameObject.GetComponent<CanvasGroup>();
		}
	}

	private void UpdateContent()
	{
		GameDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule);
		Award_awardTable award_awardTable = null;
		targetDec.text = itemData.targetDesc;
		int i = 0;
		for (int count = itemData.awardList.Count; i < count; i++)
		{
			award_awardTable = dataModule.GetAwardTableById(itemData.awardList[i]);
			if (award_awardTable != null)
			{
				if (award_awardTable.GoodsID == 3)
				{
					goldIcon.gameObject.SetActive(true);
					goodsIcon.gameObject.SetActive(false);
					rewardNumTxt.text = "x" + award_awardTable.Count;
					rewardCount += award_awardTable.Count;
				}
				else
				{
					goldIcon.gameObject.SetActive(false);
					goodsIcon.gameObject.SetActive(true);
					rewardNumTxt.text = "x" + award_awardTable.Count;
				}
				awardNum = award_awardTable.Count;
			}
		}
	}

	private void RefreshBtnHandle(GameObject obj)
	{
		MonoSingleton<GameTools>.Instacne.PlayerVideoAd(ADScene.LevelTargetRefresh, OnAddSuccess);
	}

	private void OnAddSuccess(ADScene adScene)
	{
		RefreshTarget();
	}

	public void RefreshTarget()
	{
		PlayerDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule);
		int targetId = itemData.targetId;
		dataModule.PlayerLevelTargetData.RefreshTargets(false, targetId);
	}

	public void SetRefreshBtnVisiable(bool visiable)
	{
		refreshBtn.gameObject.SetActive(visiable);
	}

	public void PlayAwardEffect(int num, Vector3 targetPosition, int index, UnityAction<int> finishCallBack)
	{
		if (num > 1)
		{
			doubleTxt.text = "x" + num;
		}
		else
		{
			doubleTxt.text = "";
		}
		doubleTxt.transform.DOLocalMoveY(startY + 30f, 1.5f).OnComplete(delegate
		{
			doubleTxt.transform.localPosition = new Vector3(doubleTxt.transform.localPosition.x, startY, doubleTxt.transform.localPosition.z);
			doubleTxt.text = "";
			PlayGoldsEffect(targetPosition, index, finishCallBack, num);
		});
		doubleTxt.DOFade(0f, 1.5f).OnComplete(delegate
		{
			doubleTxt.color = new Color(0f, 1f, 0f, 1f);
			doubleTxt.text = "";
		});
	}

	private void PlayGoldsEffect(Vector3 targetPos, int index, UnityAction<int> finishCallBack, int num)
	{
		Effect_JS_star01.SetActive(true);
		Effect_JS_star01.transform.DOMove(targetPos, 1f).SetDelay((float)index * 0.5f).OnComplete(delegate
		{
			Effect_JS_star01.transform.localPosition = Effect_JS_star01_localPosition;
			Effect_JS_star01.SetActive(false);
			if (finishCallBack != null)
			{
				finishCallBack(num * rewardCount);
			}
		});
	}

	private void PlayRereshTargetEffect(UnityAction action)
	{
		Sequence sequence = DOTween.Sequence();
		canvasCroup.alpha = 1f;
		sequence.Append(canvasCroup.DOFade(0f, 1.8f).OnComplete(delegate
		{
			RefreshTarget();
		})).Append(canvasCroup.DOFade(1f, 1f).OnComplete(delegate
		{
			canvasCroup.alpha = 1f;
			if (action != null)
			{
				action();
			}
		}));
		sequence.PlayForward();
	}

	public override object GetData()
	{
		return itemData;
	}

	public override void SetSelected(bool selected)
	{
	}

	public int GetTargetId()
	{
		return itemData.targetId;
	}

	public override void OnRelease()
	{
	}
}
