using DG.Tweening;
using Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace RS2
{
	public class RuntimeForm : UGUIForm
	{
		public string[] progressPecentTexts = new string[101]
		{
			"0%", "1%", "2%", "3%", "4%", "5%", "6%", "7%", "8%", "9%",
			"10%", "11%", "12%", "13%", "14%", "15%", "16%", "17%", "18%", "19%",
			"20%", "21%", "22%", "23%", "24%", "25%", "26%", "27%", "28%", "29%",
			"30%", "31%", "32%", "33%", "34%", "35%", "36%", "37%", "38%", "39%",
			"40%", "41%", "42%", "43%", "44%", "45%", "46%", "47%", "48%", "49%",
			"50%", "51%", "52%", "53%", "54%", "55%", "56%", "57%", "58%", "59%",
			"60%", "61%", "62%", "63%", "64%", "65%", "66%", "67%", "68%", "69%",
			"70%", "71%", "72%", "73%", "74%", "75%", "76%", "77%", "78%", "79%",
			"80%", "81%", "82%", "83%", "84%", "85%", "86%", "87%", "88%", "89%",
			"90%", "91%", "92%", "93%", "94%", "95%", "96%", "97%", "98%", "99%",
			"100%"
		};

		public GameObject pauseBtn;

		public Text progressTxt;

		private InsideGameDataModule insideGameModule;

		private ResultPageDataModule resultPageData;

		private string m_prefabPath;

		public GameObject goodsContent;

		public Text diamondTotalNumTxt;

		public Text crownTotalNumTxt;

		public Text tryAlertTxt;

		public GameObject m_content;

		private int mMaxDiamondNum;

		private int mMaxCrownNum;

		private bool mIsCollectFinished;

		private bool mIsTutorialLevel;

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			bool active = MonoSingleton<GameTools>.Instacne.IsCanOperateBackToMenu();
			pauseBtn.SetActive(active);
			if (DeviceManager.Instance.IsNeedSpecialAdapte())
			{
				MonoSingleton<GameTools>.Instacne.AdapteSpecialScreen(goodsContent.transform as RectTransform);
			}
			tryAlertTxt.text = Mod.Localization.GetInfoById(54);
			tryAlertTxt.gameObject.SetActive(false);
			insideGameModule = Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule);
			resultPageData = Singleton<DataModuleManager>.Instance.GetDataModule<ResultPageDataModule>(DataNames.ResultPageDataModule);
			progressTxt.text = "";
			m_content.SetActive(false);
			goodsContent.SetActive(false);
			if (DeviceManager.Instance.IsNeedSpecialAdapte() && goodsContent != null)
			{
				MonoSingleton<GameTools>.Instacne.AdapteSpecialScreen(goodsContent.GetComponent<RectTransform>(), -45);
			}
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			goodsContent.SetActive(false);
			AddEventHandler();
			mMaxDiamondNum = resultPageData.MaxDiamondNum;
			mMaxCrownNum = resultPageData.MaxCrownNum;
			diamondTotalNumTxt.text = insideGameModule.CurrentDiamondCollectCount.ToString() + "/" + mMaxDiamondNum;
			crownTotalNumTxt.text = insideGameModule.CurrentCrownCollectCount.ToString() + "/" + mMaxCrownNum;
			int curLevelId = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).CurLevelId;
			mIsTutorialLevel = curLevelId == 10000;
		}

		protected override void OnClose(object userData)
		{
			base.OnClose(userData);
			progressTxt.text = "";
			RemoveEventHanler();
			goodsContent.SetActive(false);
			diamondTotalNumTxt.text = "";
			crownTotalNumTxt.text = "";
			tryAlertTxt.gameObject.SetActive(false);
			m_content.SetActive(false);
		}

		private void AddEventHandler()
		{
			EventTriggerListener.Get(pauseBtn).onClick = PauseBtnClickHandler;
			Mod.Event.Subscribe(EventArgs<ProgressEventArgs>.EventId, SetProgressText);
			Mod.Event.Subscribe(EventArgs<GainedDropEventArgs>.EventId, HandlerGainedDrop);
			Mod.Event.Subscribe(EventArgs<TryLevelEndEventArgs>.EventId, StarAlertTryEnd);
			Mod.Event.Subscribe(EventArgs<UIFormOpenEvent>.EventId, OnUIFormOpen);
			Mod.Event.Subscribe(EventArgs<UIFormCloseEvent>.EventId, OnUIFormClose);
			Mod.Event.Subscribe(EventArgs<ClickGameStartButtonEventArgs>.EventId, OnHandleClickGameStartButtonEventArgs);
		}

		private void OnUIFormOpen(object sender, EventArgs e)
		{
			UIFormOpenEvent uIFormOpenEvent = e as UIFormOpenEvent;
			if (uIFormOpenEvent != null && uIFormOpenEvent.UIFormId == UIFormId.PauseForm)
			{
				pauseBtn.SetActive(false);
			}
		}

		private void OnUIFormClose(object sender, EventArgs e)
		{
			UIFormCloseEvent uIFormCloseEvent = e as UIFormCloseEvent;
			if (uIFormCloseEvent != null && uIFormCloseEvent.UIFormId == UIFormId.PauseForm)
			{
				pauseBtn.SetActive(true);
			}
		}

		private void PauseBtnClickHandler(GameObject obj)
		{
			GameController.GAMESTATE m_gameState = GameController.Instance.M_gameState;
			if (m_gameState != 0 && m_gameState != GameController.GAMESTATE.Reset && m_gameState != GameController.GAMESTATE.RebirthReset && m_gameState != GameController.GAMESTATE.Null && m_gameState != GameController.GAMESTATE.End && m_gameState != GameController.GAMESTATE.Pause)
			{
				Mod.Event.Fire(this, Mod.Reference.Acquire<GamePauseEventArgs>());
			}
		}

		private void RemoveEventHanler()
		{
			EventTriggerListener.Get(pauseBtn).onClick = null;
			Mod.Event.Unsubscribe(EventArgs<ProgressEventArgs>.EventId, SetProgressText);
			Mod.Event.Unsubscribe(EventArgs<GainedDropEventArgs>.EventId, HandlerGainedDrop);
			Mod.Event.Unsubscribe(EventArgs<TryLevelEndEventArgs>.EventId, StarAlertTryEnd);
			Mod.Event.Unsubscribe(EventArgs<UIFormOpenEvent>.EventId, OnUIFormOpen);
			Mod.Event.Unsubscribe(EventArgs<UIFormCloseEvent>.EventId, OnUIFormClose);
			Mod.Event.Unsubscribe(EventArgs<ClickGameStartButtonEventArgs>.EventId, OnHandleClickGameStartButtonEventArgs);
		}

		private void HandlerGainedDrop(object sender, EventArgs e)
		{
			GainedDropEventArgs gainedDropEventArgs = e as GainedDropEventArgs;
			if (gainedDropEventArgs != null)
			{
				switch (gainedDropEventArgs.m_dropData.m_type)
				{
				case DropType.DIAMOND:
					diamondTotalNumTxt.text = insideGameModule.CurrentDiamondCollectCount.ToString() + "/" + mMaxDiamondNum;
					break;
				case DropType.CROWN:
					crownTotalNumTxt.text = insideGameModule.CurrentCrownCollectCount.ToString() + "/" + mMaxCrownNum;
					break;
				}
			}
		}

		private void OnHandleClickGameStartButtonEventArgs(object sender, EventArgs e)
		{
			if (e is ClickGameStartButtonEventArgs)
			{
				mIsCollectFinished = insideGameModule.IsCollectFinished();
			}
		}

		private void SetProgressText(object sender, EventArgs e)
		{
			if (!mIsTutorialLevel && e is ProgressEventArgs)
			{
				SetProgressText(insideGameModule.ProgressPercentage);
				m_content.SetActive(true);
				if (!mIsCollectFinished)
				{
					goodsContent.SetActive(true);
				}
			}
		}

		private void SetProgressText(int progress)
		{
			if (progress <= 0)
			{
				progress = 0;
			}
			if (progress >= 100)
			{
				progress = 100;
			}
			progressTxt.text = progressPecentTexts[progress];
		}

		private void SetDiamondsNum(object sender, EventArgs e)
		{
			DiamondsChangeEventArgs diamondsChangeEventArgs = e as DiamondsChangeEventArgs;
			if (diamondsChangeEventArgs != null)
			{
				diamondTotalNumTxt.text = diamondsChangeEventArgs.DiamondsNum.ToString();
			}
		}

		private void SetCrownsNum(object sender, EventArgs e)
		{
			if (!goodsContent.activeSelf)
			{
				goodsContent.SetActive(true);
			}
			CrownsChangeEventArgs crownsChangeEventArgs = e as CrownsChangeEventArgs;
			if (crownsChangeEventArgs != null)
			{
				crownTotalNumTxt.text = crownsChangeEventArgs.CrownsNum.ToString();
			}
		}

		private void StarAlertTryEnd(object sender, EventArgs e)
		{
			if (e is TryLevelEndEventArgs)
			{
				tryAlertTxt.gameObject.SetActive(true);
				tryAlertTxt.transform.DOLocalMoveY(100f, 2.5f).OnComplete(delegate
				{
					tryAlertTxt.gameObject.SetActive(false);
					tryAlertTxt.transform.localPosition = Vector3.zero;
				});
			}
		}

		protected override void OnPause()
		{
			base.OnPause();
			base.gameObject.SetActive(false);
		}

		protected override void OnResume()
		{
			base.OnResume();
			base.gameObject.SetActive(true);
		}
	}
}
