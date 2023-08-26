using System.Collections;
using DG.Tweening;
using Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace RS2
{
	public class ResultForm : UGUIForm
	{
		private enum RecoreType
		{
			Grade,
			NewRecord,
			Congrates,
			Perfect
		}

		public static int maxCrownNum = 3;

		public GameObject resMenuBackBtn;

		public Text percentNumLabel;

		public Image percentImage;

		public Text diamondLabel;

		public Text levelNameLabel;

		public GameObject replayBtn;

		public GameObject rePlayTwoBtn;

		public GameObject goodsContent;

		public Text buyOutRebirthTips;

		public Text m_CrownTxt;

		public UIPersonalAssetsList m_PlayerAssetList;

		public ResultMotivateRoot motivateRoot;

		public ResultGetGoldRoot getGoldRoot;

		public ResultMiniBag miniBag;

		public GameObject levelButtonRoot;

		public Transform levelButtonRootPosStart;

		public Transform levelButtonRootPosEnd;

		public Animator resultAnim;

		public ResultUIAnimHandler animHandler;

		public GameObject centerPart;

		public GameObject singleAwardBtn;

		public Transform awardBtnRoot;

		public Transform awardBtnRootPosStart;

		public Transform awardBtnRootPosEnd;

		public ResultGetGoods getGoods;

		public UIMoveTarget goldMoveTarget;

		public GameObject m_QuitLevelBtn;

		private const int LeaveFormPluginAdId = 2;

		private static int m_leaveFormTotalTime = 0;

		private bool getGoldIncreasing;

		private ResultGetGoods.ShowData getGoodsShowData;

		private PlayerLocalLevelData levelData;

		private double goldStart;

		private double goldBase;

		private double goldLevelProgress;

		private double goldLevelFinished;

		private double goldTotal;

		private bool getGoldIncreaseMulti;

		private bool progressIncreasing;

		private Tweener increaseTweener;

		private const float INCREASE_TIME = 0.7f;

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			Init();
			GetAward();
			centerPart.SetActive(true);
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			Begin();
			m_PlayerAssetList.OnOpen(UIPersonalAssetsList.ParentType.Result);
			if (TutorialManager.Instance.GetCurrentStageId() == TutorialStageId.STAGE_FIRST_LEVEL)
			{
				MonoSingleton<GameTools>.Instacne.DisableInputForAWhile(5000u);
			}
			PlayerDataModule.Instance.PlayerAllLocalData.CheckNeedSave();
			levelData = motivateRoot.ShowMotivateReward();
			resultAnim.speed = 1f;
			resultAnim.SetTrigger("centerIn");
			ShowResult();
		}

		public void OnClickBack()
		{
			if (getGoldIncreasing)
			{
				getGoldRoot.SkipIncrease();
			}
			if (progressIncreasing)
			{
				SkipIncreaseProgress();
			}
			if (getGoldIncreaseMulti)
			{
				getGoldRoot.SkipIncreaseMulti();
			}
			resultAnim.speed = 5f;
		}

		private void GetMoveTarget()
		{
			if (getGoodsShowData == null)
			{
				return;
			}
			foreach (ResultGetGoods.GoodsData goodsData in getGoodsShowData.goodsDatas)
			{
				ResultMiniBagItem itemByGoodsId = miniBag.GetItemByGoodsId(goodsData.goodsId);
				if (!(itemByGoodsId == null))
				{
					UIMoveTarget component = itemByGoodsId.GetComponent<UIMoveTarget>();
					if (!(component == null))
					{
						goodsData.moveTarget = component;
					}
				}
			}
		}

		public void OnCenterInFinished()
		{
			IncreaseProgress();
			getGoodsShowData = motivateRoot.GetReward();
			GetMoveTarget();
			DealTutorialReplay();
			ShowResultGetGold();
		}

		public void OnGetGoldStartIncrease()
		{
			getGoldRoot.StartIncrease();
			getGoldIncreasing = true;
		}

		public void OnGetGoldInFinished()
		{
		}

		public void OnGetGoldMoveFinished()
		{
			Invoke("MoveAwardBtn", 0.2f);
		}

		public void OnGetGoldMoveBoxFinished()
		{
		}

		private void ShowResultGetGold()
		{
			ResultGetGoldRoot.ShowData showData = new ResultGetGoldRoot.ShowData();
			showData.increaseFinishedCallback = delegate
			{
				getGoldIncreasing = false;
				resultAnim.speed = 1f;
				if (levelData.isShowGiftBox)
				{
					resultAnim.SetTrigger("getGoldMoveBox");
					motivateRoot.MoveBox();
				}
				else
				{
					resultAnim.SetTrigger("getGoldMove");
				}
			};
			GetGoldData();
			showData.baseCount = goldBase;
			showData.batonCount = PlayerDataModule.Instance.GetBatonProductAdd();
			showData.levelCount = (goldLevelFinished - 1.0) * 100.0;
			showData.totalCount = goldTotal;
			Singleton<DataModuleManager>.Instance.GetDataModule<ResultPageDataModule>(DataNames.ResultPageDataModule);
			getGoldRoot.Show(showData);
		}

		private void GetGoldData()
		{
			GetGoldLevelProgressFactor();
			GetGoldBase();
			GetGoldTotal();
		}

		private void GetGoldTotal()
		{
			goldStart = PlayerDataModule.Instance.GetPlayGoodsNum(3);
			goldTotal = (int)(goldBase * goldLevelFinished * (double)(100 + PlayerDataModule.Instance.GetBatonProductAdd()) / 100.0);
		}

		private void GetGoldBase()
		{
			int curLevelId = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).CurLevelId;
			Levels_levelTable levelTableById = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).GetLevelTableById(curLevelId);
			goldBase = (int)((double)levelTableById.GoldAwardBase * goldLevelProgress / 100.0);
		}

		private void GetGoldLevelProgressFactor()
		{
			ResultPageDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<ResultPageDataModule>(DataNames.ResultPageDataModule);
			goldLevelProgress = ((dataModule.Progress <= 20) ? 1 : dataModule.Progress);
			goldLevelFinished = ((dataModule.Progress < 100) ? 1 : 2);
		}

		private void MoveAwardBtn()
		{
			awardBtnRoot.gameObject.SetActive(true);
			awardBtnRoot.position = awardBtnRootPosStart.position;
			awardBtnRoot.DOMove(awardBtnRootPosEnd.position, 0.2f).OnComplete(delegate
			{
				Invoke("ShowSingleAwardBtn", 0.8f);
			});
		}

		private void ShowSingleAwardBtn()
		{
			singleAwardBtn.SetActive(true);
			singleAwardBtn.GetComponent<DOTweenAnimation>().DOPlay();
		}

		private void MoveLevelBtn()
		{
			levelButtonRoot.SetActive(true);
			levelButtonRoot.transform.position = levelButtonRootPosStart.position;
			levelButtonRoot.transform.DOMove(levelButtonRootPosEnd.position, 0.2f).OnComplete(delegate
			{
				DealTutorial();
			});
		}

		public void OnClickAdButton()
		{
			InfocUtils.Report_rollingsky2_games_ads(34, 0, 1, 0, 3, 0);
			MonoSingleton<GameTools>.Instacne.PlayVideoAdAndDisableInput(ADScene.ResultView, delegate(ADScene adScen)
			{
				OnAdSuccess(adScen);
				MonoSingleton<GameTools>.Instacne.EnableInput();
				InfocUtils.Report_rollingsky2_games_ads(34, 0, 1, 0, 4, 0);
			});
		}

		private void OnAdSuccess(ADScene adScen = ADScene.NONE)
		{
			awardBtnRoot.gameObject.SetActive(false);
			GetAward(3);
		}

		public void OnClickNoAdButton()
		{
			awardBtnRoot.gameObject.SetActive(false);
			GetAward();
		}

		private int GetGoldMoveCount()
		{
			return Mathf.CeilToInt((float)Singleton<DataModuleManager>.Instance.GetDataModule<ResultPageDataModule>(DataNames.ResultPageDataModule).Progress / 5f);
		}

		private void GetGoldRootStartMove(int multi = 1)
		{
			double num = goldTotal * (double)multi;
			goldMoveTarget.SetData(goldStart, goldStart + num, -1);
			getGoldRoot.Hide();
			MoveLevelBtn();
		}

		private void GetAward(int multi = 1)
		{
			if (multi > 1)
			{
				getGoldRoot.StartIncreaseMulti(multi, delegate
				{
					getGoldIncreaseMulti = false;
					GetGoldRootStartMove(multi);
				});
				getGoldIncreaseMulti = true;
			}
			else
			{
				GetGoldRootStartMove(multi);
			}
		}

		private void MoveToBag(int multi = 1)
		{
			double getGoldCount = goldTotal * (double)multi;
			getGoods.MoveToBag(multi, delegate
			{
				miniBag.Hide(delegate
				{
					MoveLevelBtn();
				});
				getGoods.gameObject.SetActive(false);
				foreach (ResultGetGoods.GoodsData goodsData in getGoodsShowData.goodsDatas)
				{
					PlayerDataModule.Instance.ChangePlayerGoodsNumPieces(goodsData.goodsId, goodsData.DeltaCount() * (double)multi);
				}
				PlayerDataModule.Instance.ChangePlayerGoodsNum(3, getGoldCount);
			});
		}

		protected override void OnClose(object userData)
		{
			base.OnClose(userData);
			CancelInvoke();
			m_PlayerAssetList.OnClose();
			Reset();
			StopAllCoroutines();
			m_CrownTxt.text = "";
			diamondLabel.text = "";
			percentNumLabel.text = "";
			percentImage.fillAmount = 0f;
		}

		public void Init()
		{
			if (DeviceManager.Instance.IsNeedSpecialAdapte())
			{
				MonoSingleton<GameTools>.Instacne.AdapteSpecialScreen(goodsContent.transform as RectTransform);
			}
			getGoldRoot.gameObject.SetActive(false);
			getGoldRoot.moveItem.Init();
			levelButtonRoot.SetActive(false);
			awardBtnRoot.gameObject.SetActive(false);
			singleAwardBtn.SetActive(false);
			miniBag.Init();
			centerPart.SetActive(false);
			getGoods.gameObject.SetActive(false);
		}

		public void Begin()
		{
			ResultPageDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<ResultPageDataModule>(DataNames.ResultPageDataModule);
			int curLevelId = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).CurLevelId;
			Levels_levelTable levelTableById = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).GetLevelTableById(curLevelId);
			if (levelTableById != null)
			{
				maxCrownNum = levelTableById.Crowns;
			}
			else
			{
				maxCrownNum = 3;
			}
			AddEventListener();
			InsideGameDataModule dataModule2 = Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule);
			PlayerDataModule dataModule3 = Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule);
			if (dataModule3.GetPlayerLevelData(curLevelId).IsPerfect == 0 && dataModule2.GainedCrowns >= dataModule.MaxCrownNum && dataModule2.GainedDiamonds >= dataModule.MaxDiamondNum)
			{
				dataModule3.SetLevelIsPerfect(curLevelId, 1);
			}
			bool active = true;
			rePlayTwoBtn.SetActive(active);
			if (curLevelId == 1)
			{
				dataModule.ResetShowTutorial(dataModule.Progress);
			}
			motivateRoot.gameObject.SetActive(true);
			ShowBuyOutRebirthTips();
			replayBtn.SetActive(false);
		}

		public void Reset()
		{
			RemoveEventListener();
		}

		private void AddEventListener()
		{
			Mod.Event.Subscribe(EventArgs<ResultGetAwardFinishedEventArgs>.EventId, OnGetAward);
			Mod.Event.Subscribe(EventArgs<ToClickGameReplayEventArgs>.EventId, OnClickReplay);
		}

		private void RemoveEventListener()
		{
			Mod.Event.Unsubscribe(EventArgs<ResultGetAwardFinishedEventArgs>.EventId, OnGetAward);
			Mod.Event.Unsubscribe(EventArgs<ToClickGameReplayEventArgs>.EventId, OnClickReplay);
		}

		private void OnGetAward(object sender, EventArgs e)
		{
			getGoodsShowData.showItemsFinishedCallback = delegate
			{
				motivateRoot.HideAwardBox();
				getGoldRoot.Hide();
			};
			getGoodsShowData.firstShowItemMoveFinishedCallback = delegate
			{
				resultAnim.SetTrigger("shake");
			};
			getGoodsShowData.openFinishedCallback = GetGoodsFinished;
			getGoodsShowData.goldData = new ResultGetGoods.GoodsData(3, goldStart, goldTotal);
			getGoodsShowData.goldData.moveTarget = goldMoveTarget;
			getGoods.Show(getGoodsShowData);
		}

		private void GetGoodsFinished()
		{
			Invoke("DoGetGoodsFinished", 0.2f);
		}

		private void DoGetGoodsFinished()
		{
			MoveAwardBtn();
		}

		private void OnClickReplay(object sender, EventArgs e)
		{
			OnReplayTwoBtnClick();
		}

		public void OnMenuBackBtnClick()
		{
			int currentStageId = TutorialManager.Instance.GetCurrentStageId();
			if (Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).CurLevelId != GameCommon.FIRST_LEVEL || (currentStageId != TutorialStageId.STAGE_FIRST_LEVEL && currentStageId != TutorialStageId.STAGE_FIRST_LEVEL_BUFF))
			{
				BackToMenu();
			}
		}

		private void BackToMenu()
		{
			Mod.UI.CloseUIForm(UIFormId.ResultForm);
			int curLevelId = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).CurLevelId;
			if (TutorialManager.Instance.GetCurrentStageId() == TutorialStageId.STAGE_FIRST_LEVEL)
			{
				HomeForm.CurrentSeriesId = GameCommon.SECOND_SERIES;
			}
			else
			{
				Levels_levelTable levels_levelTable = Mod.DataTable.Get<Levels_levelTable>()[curLevelId];
				ResultPageDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<ResultPageDataModule>(DataNames.ResultPageDataModule);
				if (!MonoSingleton<GameTools>.Instacne.CheckSeriesLock(levels_levelTable.NextLevelId) && dataModule.Progress >= 100)
				{
					HomeForm.CurrentSeriesId = levels_levelTable.NextLevelId;
				}
				else
				{
					HomeForm.CurrentSeriesId = PlayerDataModule.Instance.LastEndterLevelData.SeriesId;
				}
			}
			Mod.Event.Fire(this, Mod.Reference.Acquire<GameExitEventArgs>());
			InfocUtils.Report_rollingsky2_games_pageshow(1, 2, 2, curLevelId);
		}

		public void OnReplayBtnClick()
		{
			int currentStageId = TutorialManager.Instance.GetCurrentStageId();
			if (currentStageId != TutorialStageId.STAGE_FIRST_LEVEL && currentStageId != TutorialStageId.STAGE_FIRST_LEVEL_BUFF)
			{
				if (Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule).BuyOutRebirthIndex >= 1)
				{
					ReplayLevel(2, true);
				}
				else
				{
					ReplayLevel();
				}
			}
		}

		private bool IsPowerEnough()
		{
			int curLevelId = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).CurLevelId;
			bool flag = PlayerDataModule.Instance.PlayerRecordData.IsInNoConsumePowerTime();
			if (Singleton<DataModuleManager>.Instance.GetDataModule<ResultPageDataModule>(DataNames.ResultPageDataModule).RemainBalls > 0 || flag)
			{
				return true;
			}
			return false;
		}

		private void ReplayLevel(int enterType = 1, bool isFirst = false)
		{
			Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule).TheBoardFinished();
			int curLevelId = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).CurLevelId;
			switch (enterType)
			{
			case 1:
				EnterLevel();
				break;
			case 2:
				EnterLevelFromRebirthPoint(isFirst);
				break;
			}
			InfocUtils.Report_rollingsky2_games_pageshow(1, 1, 2, curLevelId);
		}

		private void EnterLevel()
		{
			Mod.UI.CloseUIForm(UIFormId.ResultForm);
			Mod.Event.Fire(this, Mod.Reference.Acquire<GameResetEventArgs>());
			Mod.UI.OpenUIForm(UIFormId.TutorialGameStartForm);
			Mod.UI.OpenUIForm(UIFormId.RuntimeForm);
			if (TutorialManager.Instance.GetCurrentStageId() != TutorialStageId.STAGE_TUTORIAL_LEVEL)
			{
				new LevelStartForm.ShowData().showRebirth = true;
			}
			Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).CurrentEnterLevelType = LevelEnterType.RESULT;
			InsideGameDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule);
			int curLevelId = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).CurLevelId;
			if (TutorialManager.Instance.GetCurrentStageId() == TutorialStageId.STAGE_FIRST_LEVEL_BUFF && curLevelId == GameCommon.FIRST_LEVEL)
			{
				PlayerDataModule.Instance.GetFreeGoodsForTutorial();
				dataModule.m_theBoardRebirthCount = 0;
				GetGoodsData getGoodsData = new GetGoodsData();
				getGoodsData.GoodsTeamId = GameCommon.TUTORIAL_BUFF_GOODSTEAMID;
				getGoodsData.GoodsTeamNum = 1;
				getGoodsData.GoodsTeam = true;
				getGoodsData.CallBackFunc = delegate
				{
					Mod.Event.Fire(this, Mod.Reference.Acquire<ToClickGameStartEventArgs>().Initialize());
				};
				Mod.UI.OpenUIForm(UIFormId.GetGoodsForm, getGoodsData, true);
				TutorialManager.Instance.EndCurrentStage();
			}
		}

		private void EnterLevelFromRebirthPoint(bool isFirst = false)
		{
			InsideGameDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule);
			dataModule.SetBuyOutRebirthData(isFirst);
			if (isFirst)
			{
				dataModule.ResetFirstBuyOutRebirth();
			}
			Mod.Event.Fire(this, GameOriginRebirthResetEventArgs.Make(true));
			Mod.UI.CloseUIForm(UIFormId.ResultForm);
		}

		private void DoReplayTwoBtnClick()
		{
			int curLevelId = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).CurLevelId;
			Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).GetPlayerLevelData(curLevelId);
			Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule);
			ReplayLevel(2);
		}

		public void OnReplayTwoBtnClick()
		{
			InsideGameDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule);
			ResultPageDataModule dataModule2 = Singleton<DataModuleManager>.Instance.GetDataModule<ResultPageDataModule>(DataNames.ResultPageDataModule);
			if (dataModule.BuyOutRebirthIndex <= 1 || dataModule2.Progress >= 100)
			{
				OnReplayBtnClick();
				return;
			}
			int curLevelId = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).CurLevelId;
			if (TutorialManager.Instance.GetCurrentStageId() != TutorialStageId.STAGE_FIRST_LEVEL_BUFF || curLevelId != GameCommon.FIRST_LEVEL)
			{
				DoReplayTwoBtnClick();
			}
		}

		private void ShowBuyOutRebirthTips()
		{
			InsideGameDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule);
			buyOutRebirthTips.text = string.Format("({0}%)", dataModule.BuyOutRebirthProgress);
		}

		private IEnumerator PlayResultSound(float deltyTime)
		{
			yield return new WaitForSeconds(deltyTime);
			int uiSoundId = 20001;
			Mod.Sound.PlayUISound(uiSoundId);
		}

		private void IncreaseProgress()
		{
			int progress = Singleton<DataModuleManager>.Instance.GetDataModule<ResultPageDataModule>(DataNames.ResultPageDataModule).Progress;
			int preMaxProgress = Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule).PreMaxProgress;
			if (progress > preMaxProgress)
			{
				percentNumLabel.text = string.Format("{0}%", preMaxProgress);
				percentImage.GetComponent<IncreaseSlider>().StartIncrease((float)preMaxProgress / 100f, (float)progress / 100f, 0.7f, 0.1f);
				string.Format("{0}%", progress);
				increaseTweener = DOTween.To(delegate(float x)
				{
					percentNumLabel.text = string.Format("{0}%", (int)x);
				}, preMaxProgress, progress, 0.7f).OnComplete(delegate
				{
					SetFinalProgress();
					progressIncreasing = false;
				});
				progressIncreasing = true;
				Mod.Sound.PlayUISound(20020);
			}
			else
			{
				SetFinalProgress(true);
				progressIncreasing = false;
			}
		}

		private void SkipIncreaseProgress()
		{
			SetFinalProgress();
			if (increaseTweener != null)
			{
				increaseTweener.Kill();
			}
			progressIncreasing = false;
		}

		private void SetFinalProgress(bool delay = false)
		{
			int progress = Singleton<DataModuleManager>.Instance.GetDataModule<ResultPageDataModule>(DataNames.ResultPageDataModule).Progress;
			percentNumLabel.text = string.Format("{0}%", progress);
			percentImage.fillAmount = (float)progress / 100f;
			resultAnim.speed = 1f;
			if (delay)
			{
				Invoke("PlayGetGoldIn", 0.3f);
			}
			else
			{
				PlayGetGoldIn();
			}
		}

		private void PlayGetGoldIn()
		{
			resultAnim.SetTrigger("getGoldIn");
			if (Singleton<DataModuleManager>.Instance.GetDataModule<ResultPageDataModule>(DataNames.ResultPageDataModule).Progress >= 100 && !PlayerDataModule.Instance.PlayerLocalLevelSeriesData.IsInMotivateAwardCD())
			{
				motivateRoot.ShowMaxEffect();
			}
		}

		private void ShowResult()
		{
			ResultPageDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<ResultPageDataModule>(DataNames.ResultPageDataModule);
			levelNameLabel.text = dataModule.LevelTitle;
			int progress = dataModule.Progress;
			int preMaxProgress = Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule).PreMaxProgress;
			percentNumLabel.text = string.Format("{0}%", dataModule.Progress);
			percentImage.fillAmount = (float)progress / 100f;
			InsideGameDataModule dataModule2 = Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule);
			int currentCrownCollectCount = dataModule2.CurrentCrownCollectCount;
			diamondLabel.text = string.Format("{0}/{1}", dataModule2.CurrentDiamondCollectCount, dataModule.MaxDiamondNum);
			m_CrownTxt.text = string.Format("{0}/{1}", dataModule2.CurrentCrownCollectCount, maxCrownNum);
		}

		protected override void OnTick(float elapseSeconds, float realElapseSeconds)
		{
			base.OnTick(elapseSeconds, realElapseSeconds);
			m_PlayerAssetList.OnUpdate();
		}

		public void DealTutorialReplay()
		{
			Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule);
			int curLevelId = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).CurLevelId;
			if (TutorialManager.Instance.GetCurrentStageId() != TutorialStageId.STAGE_FIRST_LEVEL_BUFF || curLevelId != GameCommon.FIRST_LEVEL)
			{
				return;
			}
			if (PlayerDataModule.Instance.GetLevelMaxProgress(GameCommon.FIRST_LEVEL) >= 100)
			{
				TutorialManager.Instance.EndCurrentStage();
				return;
			}
			CommonTutorialData commonTutorialData = new CommonTutorialData(-1);
			CommonTutorialStepData commonTutorialStepData = new CommonTutorialStepData();
			RectTransform rectTransform = rePlayTwoBtn.transform as RectTransform;
			commonTutorialStepData.showContent = false;
			commonTutorialStepData.needBlock = true;
			commonTutorialStepData.position = new Rect(rectTransform.position.x, rectTransform.position.y, rectTransform.sizeDelta.x, rectTransform.sizeDelta.y);
			commonTutorialStepData.changeRect = true;
			commonTutorialStepData.target = rectTransform;
			commonTutorialStepData.stepAction = delegate
			{
				DoReplayTwoBtnClick();
			};
			commonTutorialData.AddStep(commonTutorialStepData);
			BaseTutorialStep step = new CommonClickTutorialStep(commonTutorialData);
			TutorialManager.Instance.GetCurrentStage().AddStep(step);
			TutorialManager.Instance.GetCurrentStage().Execute();
		}

		public void DealTutorial()
		{
			if (TutorialManager.Instance.GetCurrentStageId() == TutorialStageId.STAGE_HOME_MENU)
			{
				CommonTutorialData commonTutorialData = new CommonTutorialData(-1);
				CommonTutorialStepData commonTutorialStepData = new CommonTutorialStepData();
				RectTransform rectTransform = m_QuitLevelBtn.transform as RectTransform;
				commonTutorialStepData.position = new Rect(rectTransform.position.x, rectTransform.position.y, rectTransform.sizeDelta.x, rectTransform.sizeDelta.y);
				commonTutorialStepData.posOffset = new Vector2(0f, 0f);
				commonTutorialStepData.showContent = true;
				commonTutorialStepData.needBlock = true;
				commonTutorialStepData.changeRect = false;
				commonTutorialStepData.target = rectTransform;
				commonTutorialStepData.stepType = TutorialStepType.ONLY_FINGER;
				commonTutorialStepData.useViewportAdjustPos = true;
				commonTutorialStepData.stepAction = delegate
				{
					TutorialManager.Instance.EndCurrentStage();
					StartCoroutine(DelayClose(0.1f));
				};
				commonTutorialData.AddStep(commonTutorialStepData);
				BuildinTutorialForm.Form.StartTutorial(commonTutorialData);
			}
		}

		private IEnumerator DelayClose(float delayTime)
		{
			yield return new WaitForSeconds(delayTime);
			OnMenuBackBtnClick();
		}

		private bool DealLeaveformPluginAd()
		{
			m_leaveFormTotalTime++;
			int id = MonoSingleton<GameTools>.Instacne.ComputerScreenPluginAdId(2);
			ScreenPluginsAd_table screenPluginsAd_table = Mod.DataTable.Get<ScreenPluginsAd_table>()[id];
			bool flag = false;
			if (screenPluginsAd_table != null)
			{
				flag |= m_leaveFormTotalTime >= screenPluginsAd_table.TriggerNum;
				if (flag)
				{
					flag &= PlayerDataModule.Instance.PluginAdController.IsScreenAdRead();
					if (flag)
					{
						PluginAdData pluginAdData = new PluginAdData();
						pluginAdData.PluginId = 2;
						pluginAdData.EndHandler = delegate
						{
							ClearLeaveFormPluginAdData();
							BackToMenu();
						};
						Mod.UI.OpenUIForm(UIFormId.ScreenPluginsForm, pluginAdData);
					}
				}
			}
			return flag;
		}

		private void ClearLeaveFormPluginAdData()
		{
			m_leaveFormTotalTime = 0;
		}

		private RectTransform SetNSButton(RectTransform buttonRoot, float maxWidth)
		{
			RectTransform component = buttonRoot.GetComponentInChildren<CustomText>().GetComponent<RectTransform>();
			float x = buttonRoot.transform.Find("InputInfo/Joystick").GetComponent<RectTransform>().sizeDelta.x;
			float num = (maxWidth + x) * 1.8f;
			Debug.Log(num);
			float num2 = (num - (maxWidth + x)) / 3f;
			component.GetComponent<RectTransform>().sizeDelta = new Vector2(maxWidth, component.GetComponent<RectTransform>().sizeDelta.y);
			buttonRoot.sizeDelta = new Vector2(num, buttonRoot.sizeDelta.y);
			component.sizeDelta = new Vector2(maxWidth, 108f);
			component.anchorMin = new Vector2(0f, 0.5f);
			component.anchorMax = new Vector2(0f, 0.5f);
			component.pivot = new Vector2(0f, 0.5f);
			component.GetComponent<CustomText>().alignment = TextAnchor.MiddleLeft;
			component.anchoredPosition = new Vector2(x + num2 * 2f, component.anchoredPosition.y);
			buttonRoot.transform.Find("InputInfo/Joystick").GetComponent<RectTransform>().anchoredPosition = new Vector2(num2, 0f);
			return buttonRoot;
		}
	}
}
