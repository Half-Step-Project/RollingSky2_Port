using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace RS2
{
	public class LevelEnterForm : UGUIForm
	{
		public GameObject m_closeBtn;

		public Text m_chapterName;

		public Text m_chapterSubName;

		public GameObject m_levelContainerList;

		private int m_levelSeriesId = -1;

		private int m_preLevelSeriesId = -3;

		private List<int> m_ChaterList = new List<int>();

		private List<LevelMetaTableData> m_levelMetaDataList = new List<LevelMetaTableData>();

		public List<LevelItemController> m_levelDic = new List<LevelItemController>();

		public float m_CellWidth = 360f;

		public float m_CellScale = 0.7f;

		private LevelMetaTableData m_currentMetaData;

		private RectTransform m_currentContent;

		private Vector3 m_currentBackPos = Vector3.zero;

		private LevelItemController currentLevel;

		public GameObject m_LockContainer;

		public GameObject m_KeyUnlockBtn;

		public GameObject m_AdUnlockBtn;

		public Text m_keyNumTxt;

		public Text m_AdDescTxt;

		public GameObject m_UnLockContainer;

		public GameObject m_PlayBtn;

		private int m_lastLevelId = -1;

		public SetUIGrey m_UIGray;

		public DOTweenAnimation m_tweenAnimation;

		public Vector3 m_sourcePos;

		public GameObject m_freeEnterLevelBtn;

		public Text m_resumeAdTime;

		private PlayerLocalLevelData m_CurrentlevelLogicData;

		private int m_totalFrame;

		private static bool m_isNeedDifficultTutorial;

		private const int LevelEnterPluginAdId = 6;

		private int m_changePageTotalCount;

		private float m_pluginAdTime;

		private List<object> loadedAsserts = new List<object>();

		public static bool NeedDifficultTutorial
		{
			get
			{
				return m_isNeedDifficultTutorial;
			}
		}

		public float CellWidth
		{
			get
			{
				return m_CellWidth;
			}
		}

		public float CellScale
		{
			get
			{
				return m_CellScale;
			}
		}

		public void SetAdTextByState(bool isHadAd)
		{
			if (m_AdDescTxt != null)
			{
				if (isHadAd)
				{
					m_AdDescTxt.gameObject.SetActive(true);
					m_AdDescTxt.text = Mod.Localization.GetInfoById(18);
				}
				else
				{
					m_AdDescTxt.gameObject.SetActive(true);
					m_AdDescTxt.text = Mod.Localization.GetInfoById(211);
				}
			}
		}

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			m_currentContent = m_levelContainerList.transform.Find("viewport/content") as RectTransform;
			SetAdTextByState(true);
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			Mod.Event.Fire(this, UIFormOpenEvent.Make(UIFormId.LevelEnterForm));
			m_currentMetaData = (LevelMetaTableData)userData;
			m_levelSeriesId = m_currentMetaData.SeriesId;
			if (m_levelSeriesId <= 0)
			{
				Mod.UI.CloseUIForm(UIFormId.LevelEnterForm);
				return;
			}
			if (m_preLevelSeriesId != m_levelSeriesId)
			{
				InitMetaData();
			}
			m_preLevelSeriesId = m_levelSeriesId;
			m_isNeedDifficultTutorial = IsNeedDifficultTutorial();
			if (m_isNeedDifficultTutorial)
			{
				StartCoroutine(DealTutorial());
			}
			if (currentLevel != null)
			{
				currentLevel.m_downloadFlag.gameObject.SetActive(false);
			}
			SetLevelData();
			AddEventListener();
			if (m_currentMetaData.LevelId > 0)
			{
				SwitchLevel(m_currentMetaData.LevelId);
			}
			else
			{
				SwitchLevel(FindDefaultLevelId());
			}
			TweenIn();
		}

		private void TweenIn()
		{
			if (!(m_tweenAnimation != null))
			{
				return;
			}
			List<Tween> tweens = m_tweenAnimation.GetTweens();
			for (int i = 0; i < tweens.Count; i++)
			{
				if (i != 0)
				{
					continue;
				}
				tweens[i].OnComplete(delegate
				{
					if (currentLevel != null)
					{
						currentLevel.StartTutorial();
					}
				});
			}
			m_tweenAnimation.DORestart();
			m_tweenAnimation.DOPlayForward();
		}

		private int FindDefaultLevelId()
		{
			int num = -1;
			bool flag = false;
			List<LevelMetaTableData> list = m_levelMetaDataList.FindAll((LevelMetaTableData x) => x.IsShowInSelect);
			for (int i = 0; i < list.Count; i++)
			{
				flag = MonoSingleton<GameTools>.Instacne.IsPreparing(list[i].LevelId);
				if (PlayerDataModule.Instance.GetLevelMaxProgress(list[i].LevelId) < 100 && !flag)
				{
					num = list[i].LevelId;
					break;
				}
			}
			if (num == -1)
			{
				num = list[0].LevelId;
			}
			return num;
		}

		private IEnumerator DealTutorial()
		{
			yield return new WaitForEndOfFrame();
			DealRealTutorial();
			m_isNeedDifficultTutorial = false;
		}

		protected override void OnClose(object userData)
		{
			base.OnClose(userData);
			RemoveEventListener();
			StopAllCoroutines();
			ClearLeaveFormPluginAdData();
			m_lastLevelId = -1;
			m_tweenAnimation.transform.localPosition = m_sourcePos;
			Mod.Event.Fire(this, UIFormCloseEvent.Make(UIFormId.LevelEnterForm));
		}

		protected override void OnUnload()
		{
			base.OnUnload();
			Release();
			for (int i = 0; i < loadedAsserts.Count; i++)
			{
				Mod.Resource.UnloadAsset(loadedAsserts[i]);
			}
			loadedAsserts.Clear();
		}

		private void Release()
		{
			m_preLevelSeriesId = -3;
			m_levelSeriesId = -1;
			List<LevelItemController>.Enumerator enumerator = m_levelDic.GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.Release();
			}
		}

		public void SwitchLevel(int currentLevelId)
		{
			if (m_lastLevelId != currentLevelId)
			{
				m_lastLevelId = currentLevelId;
				ShowContentByLevelId(currentLevelId);
				m_changePageTotalCount++;
			}
		}

		private void ShowLockContentByState(int levelId)
		{
			if (MonoSingleton<GameTools>.Instacne.IsPreparing(levelId))
			{
				m_LockContainer.SetActive(false);
				m_UnLockContainer.SetActive(false);
				return;
			}
			m_CurrentlevelLogicData = Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).GetPlayerLevelData(levelId);
			if (m_CurrentlevelLogicData.LockState <= 0)
			{
				m_LockContainer.SetActive(false);
				m_UnLockContainer.SetActive(true);
				return;
			}
			m_LockContainer.SetActive(true);
			m_UnLockContainer.SetActive(false);
			if (m_CurrentlevelLogicData.IsCanFreeEnterLevel())
			{
				m_AdUnlockBtn.SetActive(false);
				m_freeEnterLevelBtn.SetActive(true);
				m_resumeAdTime.gameObject.SetActive(true);
				RefreshFreeEnterLevelTime();
			}
			else
			{
				m_AdUnlockBtn.SetActive(true);
				m_freeEnterLevelBtn.SetActive(false);
				m_resumeAdTime.gameObject.SetActive(false);
			}
		}

		private void RefreshFreeEnterLevelTime()
		{
			if (m_CurrentlevelLogicData != null)
			{
				if (m_CurrentlevelLogicData.IsCanFreeEnterLevel())
				{
					int num = (int)(m_CurrentlevelLogicData.FreeEnterLevelTimeStamp - PlayerDataModule.Instance.ServerTime) / 1000;
					m_resumeAdTime.text = string.Format(Mod.Localization.GetInfoById(197), MonoSingleton<GameTools>.Instacne.TimeFormat_HH_MM_SS(num));
				}
				else
				{
					m_AdUnlockBtn.SetActive(true);
					m_freeEnterLevelBtn.SetActive(false);
					m_resumeAdTime.gameObject.SetActive(false);
				}
			}
		}

		private void ShowContentByLevelId(int levelId)
		{
			List<LevelItemController>.Enumerator enumerator = m_levelDic.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.MetaData.LevelId == levelId)
				{
					currentLevel = enumerator.Current;
				}
				else
				{
					enumerator.Current.SetItemSelected(false);
				}
			}
			if (currentLevel != null)
			{
				currentLevel.SetItemSelected(true);
				ShowChapterName(currentLevel.MetaData.ChapterId);
				ShowLockContentByState(levelId);
			}
		}

		public void SetKeyUnLockNum(int unlockKeyNum)
		{
			m_keyNumTxt.text = string.Format("{0}", unlockKeyNum);
		}

		public void OnBeginDrag()
		{
			bool flag = currentLevel != null;
		}

		public void OnCenterStart(int levelId)
		{
		}

		public void OnCenterEnd()
		{
		}

		private void ShowChapterName(int chapterId)
		{
			LevelChapters_table levelChapters_table = Mod.DataTable.Get<LevelChapters_table>()[chapterId];
			if (levelChapters_table != null)
			{
				m_chapterName.text = Mod.Localization.GetInfoById(levelChapters_table.Name);
				m_chapterSubName.text = Mod.Localization.GetInfoById(levelChapters_table.SubName);
			}
		}

		private void SetLevelData()
		{
			List<LevelMetaTableData> list = m_levelMetaDataList.FindAll((LevelMetaTableData x) => x.IsShowInSelect);
			LevelMetaTableData levelMetaTableData = null;
			for (int i = 0; i < list.Count && i < m_levelDic.Count; i++)
			{
				LevelItemController levelItemController = m_levelDic[i];
				levelMetaTableData = list[i];
				levelItemController.gameObject.name = levelMetaTableData.LevelId.ToString();
				levelItemController.SetData(levelMetaTableData, this);
			}
		}

		private void InitMetaData()
		{
			m_levelMetaDataList = PlayerDataModule.Instance.GloableLevelLableData.FindAll((LevelMetaTableData x) => x.SeriesId == m_levelSeriesId);
		}

		private void AddEventListener()
		{
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(m_closeBtn);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(OnClickCloseButton));
			EventTriggerListener eventTriggerListener2 = EventTriggerListener.Get(m_AdUnlockBtn);
			eventTriggerListener2.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener2.onClick, new EventTriggerListener.VoidDelegate(AdUnLockBtnClickHandler));
			EventTriggerListener eventTriggerListener3 = EventTriggerListener.Get(m_KeyUnlockBtn);
			eventTriggerListener3.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener3.onClick, new EventTriggerListener.VoidDelegate(KeyUnLockBtnClickHandler));
			EventTriggerListener eventTriggerListener4 = EventTriggerListener.Get(m_PlayBtn);
			eventTriggerListener4.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener4.onClick, new EventTriggerListener.VoidDelegate(PlayClickHandler));
			EventTriggerListener eventTriggerListener5 = EventTriggerListener.Get(m_freeEnterLevelBtn);
			eventTriggerListener5.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener5.onClick, new EventTriggerListener.VoidDelegate(FreeEnterLevel));
			Mod.Event.Subscribe(EventArgs<LevelUnLockEventArgs>.EventId, OnLevelUnLockChange);
		}

		private void OnLevelUnLockChange(object sender, Foundation.EventArgs e)
		{
			LevelUnLockEventArgs levelUnLockEventArgs = e as LevelUnLockEventArgs;
			if (levelUnLockEventArgs != null && currentLevel != null && levelUnLockEventArgs.LevelId == currentLevel.GetLevelId())
			{
				ShowLockContentByState(levelUnLockEventArgs.LevelId);
			}
		}

		private void PlayClickHandler(GameObject go)
		{
			if (currentLevel != null)
			{
				currentLevel.EnterLevelHandler();
			}
		}

		private void FreeEnterLevel(GameObject obj)
		{
			if (currentLevel != null)
			{
				currentLevel.EnterLevelHandler();
			}
		}

		private void AdUnLockBtnClickHandler(GameObject go)
		{
			if (currentLevel != null)
			{
				currentLevel.AdUnLockkHandler();
			}
		}

		private void KeyUnLockBtnClickHandler(GameObject go)
		{
			if (currentLevel != null)
			{
				currentLevel.KeyUnLockHandler();
			}
		}

		private void RemoveEventListener()
		{
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(m_closeBtn);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(OnClickCloseButton));
			EventTriggerListener eventTriggerListener2 = EventTriggerListener.Get(m_AdUnlockBtn);
			eventTriggerListener2.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener2.onClick, new EventTriggerListener.VoidDelegate(AdUnLockBtnClickHandler));
			EventTriggerListener eventTriggerListener3 = EventTriggerListener.Get(m_KeyUnlockBtn);
			eventTriggerListener3.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener3.onClick, new EventTriggerListener.VoidDelegate(KeyUnLockBtnClickHandler));
			EventTriggerListener eventTriggerListener4 = EventTriggerListener.Get(m_PlayBtn);
			eventTriggerListener4.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener4.onClick, new EventTriggerListener.VoidDelegate(PlayClickHandler));
			EventTriggerListener eventTriggerListener5 = EventTriggerListener.Get(m_freeEnterLevelBtn);
			eventTriggerListener5.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener5.onClick, new EventTriggerListener.VoidDelegate(FreeEnterLevel));
			Mod.Event.Unsubscribe(EventArgs<LevelUnLockEventArgs>.EventId, OnLevelUnLockChange);
		}

		private void OnClickCloseButton(GameObject obj)
		{
			if (TutorialManager.Instance.GetCurrentStageId() != TutorialStageId.STAGE_FIRST_LEVEL)
			{
				Mod.UI.CloseUIForm(UIFormId.LevelEnterForm);
			}
		}

		protected override void OnTick(float elapseSeconds, float realElapseSeconds)
		{
			base.OnTick(elapseSeconds, realElapseSeconds);
			m_pluginAdTime += elapseSeconds;
			if (m_pluginAdTime >= 60f)
			{
				m_pluginAdTime = 0f;
				m_changePageTotalCount = 0;
			}
			if (++m_totalFrame % 30 == 0)
			{
				RefreshFreeEnterLevelTime();
			}
		}

		private bool IsNeedDifficultTutorial()
		{
			return false;
		}

		private void DealRealTutorial()
		{
			CommonTutorialData commonTutorialData = new CommonTutorialData(-1);
			commonTutorialData.AddStep(new CommonTutorialStepData
			{
				stepAction = delegate
				{
					TutorialManager.Instance.SetTutorialStageFlag(TutorialStageId.STAGE_LEVEL_DIFFICULT);
				}
			});
			Mod.UI.CloseUIForm(UIFormId.CommonTutorialForm);
			BaseTutorialStep step = new CommonClickTutorialStep(commonTutorialData);
			TutorialManager.Instance.GetStage(TutorialStageId.STAGE_LEVEL_DIFFICULT).AddStep(step);
			TutorialManager.Instance.GetStage(TutorialStageId.STAGE_LEVEL_DIFFICULT).Execute();
		}

		private bool LevelIsCanUnlockByChengJiu(int levelId, LevelDifficulty degree)
		{
			PlayerDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule);
			dataModule.GetPlayerLevelData(levelId);
			string[] array = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).GetLevelTableById(levelId).UnLockIds.Split('|');
			LevelUnLock_table levelUnLock_table = null;
			List<bool> list = new List<bool>();
			int num = -1;
			for (int i = 0; i < array.Length; i++)
			{
				levelUnLock_table = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).GetLevelUnLockById(int.Parse(array[i]));
				if (levelUnLock_table == null)
				{
					continue;
				}
				LevelLockType lockType = (LevelLockType)levelUnLock_table.LockType;
				if (lockType != LevelLockType.CHENGJIU)
				{
					continue;
				}
				num = levelUnLock_table.LockTypeId;
				int unLockNum = levelUnLock_table.UnLockNum;
				List<int> list2 = MonoSingleton<GameTools>.Instacne.StringToIntList(levelUnLock_table.LinkLevel);
				switch (num)
				{
				case 1:
				{
					int num3 = 0;
					for (int k = 0; k < list2.Count; k++)
					{
						num3 += dataModule.GetLevelMaxDiamond(list2[k]);
					}
					switch (degree)
					{
					case LevelDifficulty.NORMAL:
						if (num3 >= unLockNum)
						{
							return true;
						}
						break;
					case LevelDifficulty.Difficulty:
						if (num3 >= unLockNum)
						{
							if (levelUnLock_table.Relation == 1)
							{
								return true;
							}
							if (levelUnLock_table.Relation == 2)
							{
								list.Add(true);
							}
							break;
						}
						return false;
					}
					break;
				}
				case 3:
					if (degree == LevelDifficulty.Difficulty)
					{
						int num4 = 0;
						for (int l = 0; l < list2.Count; l++)
						{
							num4 += dataModule.GetLevelMaxProgress(list2[l]);
						}
						if (num4 < unLockNum)
						{
							return false;
						}
						if (levelUnLock_table.Relation == 1)
						{
							return true;
						}
						if (levelUnLock_table.Relation == 2)
						{
							list.Add(true);
						}
					}
					break;
				case 2:
					if (degree == LevelDifficulty.Difficulty)
					{
						int num2 = 0;
						for (int j = 0; j < list2.Count; j++)
						{
							num2 += dataModule.GetLevelMaxCrown(list2[j]);
						}
						if (num2 < unLockNum)
						{
							return false;
						}
						if (levelUnLock_table.Relation == 1)
						{
							return true;
						}
						if (levelUnLock_table.Relation == 2)
						{
							list.Add(true);
						}
					}
					break;
				}
			}
			bool flag = true;
			for (int m = 0; m < list.Count; m++)
			{
				flag &= list[m];
			}
			if (list.Count > 0 && flag)
			{
				return true;
			}
			return false;
		}

		private bool DealLeaveformPluginAd()
		{
			int id = MonoSingleton<GameTools>.Instacne.ComputerScreenPluginAdId(6);
			ScreenPluginsAd_table screenPluginsAd_table = Mod.DataTable.Get<ScreenPluginsAd_table>()[id];
			bool flag = false;
			if (screenPluginsAd_table != null)
			{
				flag |= m_changePageTotalCount >= screenPluginsAd_table.TriggerNum;
				if (flag)
				{
					flag &= PlayerDataModule.Instance.PluginAdController.IsScreenAdRead();
					if (flag)
					{
						PluginAdData pluginAdData = new PluginAdData();
						pluginAdData.PluginId = 6;
						pluginAdData.EndHandler = delegate
						{
							ClearLeaveFormPluginAdData();
						};
						Mod.UI.OpenUIForm(UIFormId.ScreenPluginsForm, pluginAdData);
					}
				}
			}
			return flag;
		}

		private void ClearLeaveFormPluginAdData()
		{
			m_changePageTotalCount = 0;
			m_pluginAdTime = 0f;
		}
	}
}
