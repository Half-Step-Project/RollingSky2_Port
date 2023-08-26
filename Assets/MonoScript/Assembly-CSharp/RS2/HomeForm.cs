using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Foundation;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RS2
{
	public class HomeForm : UGUIForm, ITabSelect
	{
		public GameObject nextLevelTutorialRoot;

		public RectTransform levelsContainer;

		public GameObject viewport;

		public GameObject levelItem;

		public GameObject m_leftDirection;

		public HomeHintRoot m_homeHintRoot;

		public GameObject topLeftContainer;

		public DOTweenAnimation m_TopContainer;

		public DOTweenAnimation m_LeftContainer;

		public DOTweenAnimation m_RightContainer;

		public Transform m_LeftTarget;

		public Transform m_RightTarget;

		public GameObject m_rightDirection;

		public GameObject m_newLevelLockAlertEffect;

		public GameObject m_UnLockEffect;

		public Animator m_newLevelLockAlertAnimator;

		public UIPersonalAssetsList m_PlayerAssetList;

		public GameObject m_levelDebuEnter;

		public GameObject cup0Tips;

		public GameObject cup1Tips;

		public GameObject luckyTurntableEntry;

		private LevelSeriesDragScrollView m_LevelDragScrollView;

		private Vector3 scrowLocalPos;

		private static int m_seriesId = -1;

		private LevelSeriesController m_preItemController;

		private bool m_isPlayingEffect;

		private AdPlayState m_AdState = AdPlayState.NONE;

		private int m_currentSeriesId = -1;

		private int m_currentIndexId = -1;

		private GameObject m_levelBgContainer;

		private const int DEFAULT_LEVEL_BG_RENDERTEXTURE_WIDTH = 512;

		private const int MAX_LEVEL_BG_RENDERTEXTURE_WIDTH = 1024;

		private RenderTexture m_levelBgRenderTexture;

		private bool m_isPlayingTweenAniation;

		private List<LevelSeriesController> m_HideList;

		private IEnumerator m_coroutine;

		private Camera m_bgCamera;

		private List<LevelSeriesController> itemControllerList = new List<LevelSeriesController>();

		private LevelSeriesController m_showBgController;

		private const int LEVELITEM_BUFFER_COUNT = 5;

		private List<object> loadedAsserts = new List<object>();

		private LevelSeriesController m_CurrentItemController;

		private int totalLevelNum;

		private static float m_cellW = -1f;

		private PlayerLuckTurnTableLocalData m_luckData;

		public RedPointBehaviour mRootRedPoint;

		public RedPointBehaviour mExchangeStoreRedPoint;

		public RedPointBehaviour m_ShopRedPoint;

		public RedPointBehaviour m_luckyTurntableEntryRedPoint;

		public RedPointBehaviour mGiftRedPoint;

		private const int HomeFormPluginAdId = 5;

		private int m_changePageTotalCount;

		private float m_pluginAdTime;

		public static int QUIT_LEVEL_COUNT = 0;

		private const int QuitLevelAdId = 13;

		public static int OPEN_COUNT = 1;

		private VideoAwardRoot videoAwardRoot;

		public static HomeForm Form = null;

		public RectTransform upgradeBtn;

		public PlayerInfoInspector playerInfoInspector;

		public bool CanEnterLevel
		{
			get
			{
				if (!m_isPlayingEffect && !m_LevelDragScrollView.isDrag)
				{
					return !PlayingTweenAniation;
				}
				return false;
			}
		}

		public Camera BgCamera
		{
			get
			{
				return m_bgCamera;
			}
		}

		public bool PlayingTweenAniation
		{
			get
			{
				return m_isPlayingTweenAniation;
			}
			set
			{
				m_isPlayingTweenAniation = value;
			}
		}

		public static int CurrentSeriesId
		{
			get
			{
				return m_seriesId;
			}
			set
			{
				m_seriesId = value;
			}
		}

		public static float CellH
		{
			get
			{
				if (m_cellW <= 0f)
				{
					float num = 720f;
					float num2 = 1280f;
					float num3 = num / num2;
					float num4 = (float)Screen.width * 1f;
					float num5 = (float)Screen.height * 1f;
					float num6 = 1f;
					float num7 = num4 / num5;
					float num8 = 605f * num2 / (num * 1024f);
					if (num7 > num3)
					{
						num6 = num4 * num2 / (num * num5);
						num6 *= num8;
						m_cellW = 720f * num6 * 1.02f;
					}
					else
					{
						num6 = num * num5 / (num4 * num2);
						num6 *= num8;
						m_cellW = 720f * num6;
					}
				}
				return m_cellW;
			}
		}

		public HomeFormState State { get; private set; }

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			Init();
			if (CurrentSeriesId < 1)
			{
				CurrentSeriesId = GetFirstSeriesId();
			}
			m_luckData = PlayerDataModule.Instance.LuckTurnLocalData;
			m_luckData.Init();
			itemControllerList.Clear();
			InitLevelInfoById(CurrentSeriesId);
			InitTweenParam();
		}

		private void DealWithNextLevelTutorial()
		{
			if (TutorialManager.Instance.GetCurrentStageId() == TutorialStageId.STAGE_NEXT_LEVEL)
			{
				CommonTutorialData commonTutorialData = new CommonTutorialData(-1);
				CommonTutorialStepData commonTutorialStepData = new CommonTutorialStepData();
				commonTutorialStepData.showContent = true;
				commonTutorialStepData.needBlock = false;
				commonTutorialStepData.changeRect = false;
				commonTutorialStepData.stepType = TutorialStepType.ONLY_CONTENT;
				commonTutorialStepData.tutorialContent = Mod.Localization.GetInfoById(328);
				commonTutorialStepData.finishAction = delegate
				{
					nextLevelTutorialRoot.SetActive(true);
				};
				commonTutorialData.AddStep(commonTutorialStepData);
				BuildinTutorialForm.Form.StartTutorial(commonTutorialData);
			}
			else
			{
				nextLevelTutorialRoot.SetActive(false);
			}
		}

		protected override void OnOpen(object userData)
		{
			Form = this;
			base.OnOpen(userData);
			Mod.UI.OpenUIForm(UIFormId.GetAwardForm);
			m_levelDebuEnter.SetActive(false);
			m_PlayerAssetList.OnOpen(UIPersonalAssetsList.ParentType.Home);
			if (CurrentSeriesId < 1)
			{
				int firstSeriesId = GetFirstSeriesId();
				GoToSeries(firstSeriesId);
			}
			else
			{
				GoToSeries(CurrentSeriesId);
			}
			m_currentIndexId = GetOrderIndexBySeriesId(CurrentSeriesId) + 1;
			AddEventListener();
			if (!MenuForm.isSelectLevel)
			{
				SetState(HomeFormState.Education);
			}
			else
			{
				SetState(HomeFormState.SelectLevel);
			}
			if (TutorialManager.Instance.GetCurrentStageId() == TutorialStageId.STAGE_INSTRUMENT_CALL)
			{
				TutorialForInstrumentCall();
			}
			else if (TutorialManager.Instance.GetCurrentStageId() == TutorialStageId.STAGE_UPGRADE)
			{
				TutorialForUpgrade();
			}
			else if (TutorialManager.Instance.GetCurrentStageId() == TutorialStageId.STAGE_TAPTAP)
			{
				TutorialForTapTap();
			}
		}

		private void TutorialForInstrumentCall()
		{
			MonoSingleton<GameTools>.Instacne.DisableInputForAWhile();
			EducationDisplayDirector.Instance._Slots.PlayAnim("In");
			EducationDisplayDirector.Instance._Sprite.PlayAnim("Magic");
			Invoke("InvokeAfterAnim", 2.5f);
		}

		private void InvokeAfterAnim()
		{
			MonoSingleton<GameTools>.Instacne.EnableInput();
			CommonTutorialData commonTutorialData = new CommonTutorialData(-1);
			CommonTutorialStepData commonTutorialStepData = new CommonTutorialStepData();
			commonTutorialStepData.stepType = TutorialStepType.ONLY_CONTENT;
			commonTutorialStepData.tutorialContent = Mod.Localization.GetInfoById(324);
			commonTutorialStepData.needBlock = true;
			commonTutorialStepData.blockBtnEnable = false;
			commonTutorialStepData.finishAction = delegate
			{
				PlayerDataModule.Instance.StartProductGoods();
				TutorialManager.Instance.EndCurrentStage();
				TutorialForUpgrade();
			};
			commonTutorialData.AddStep(commonTutorialStepData);
			BuildinTutorialForm.Form.StartTutorial(commonTutorialData);
		}

		private void TutorialForUpgrade()
		{
			if (TutorialManager.Instance.GetCurrentStageId() == TutorialStageId.STAGE_UPGRADE)
			{
				MonoSingleton<GameTools>.Instacne.DisableInputForAWhile();
				CommonTutorialData commonTutorialData = new CommonTutorialData(-1);
				CommonTutorialStepData commonTutorialStepData = new CommonTutorialStepData();
				commonTutorialStepData.stepType = TutorialStepType.ONLY_FINGER;
				Slot firstSlot = EducationDisplayDirector.Instance._Slots.GetSlot(0);
				Vector3 vector = Camera.main.WorldToViewportPoint(firstSlot.transform.position);
				Rect rect = GetComponent<RectTransform>().rect;
				Vector2 vector2 = new Vector2(rect.width, rect.height);
				commonTutorialStepData.useFingerLocalPos = true;
				commonTutorialStepData.fingerLocalPos = new Vector3(vector2.x * (vector.x - 0.5f), vector2.y * (vector.y - 0.5f), 0f);
				commonTutorialStepData.stepAction = delegate
				{
					firstSlot.MI.DoMouseUpAsButton();
				};
				commonTutorialStepData.disableBackClick = true;
				for (int i = 0; i < 5; i++)
				{
					commonTutorialData.AddStep(commonTutorialStepData);
				}
				commonTutorialData.endAction = delegate
				{
					MonoSingleton<GameTools>.Instacne.DisableInputForAWhile();
					StartCoroutine(DoTutorialUpgrade());
				};
				BuildinTutorialForm.Form.StartTutorial(commonTutorialData);
			}
		}

		private IEnumerator DoTutorialUpgrade()
		{
			while (!PlayerDataModule.Instance.IsPlayerUpLevelNeedGoodsEnough())
			{
				yield return null;
			}
			CommonTutorialData commonTutorialData = new CommonTutorialData(-1);
			CommonTutorialStepData commonTutorialStepData = new CommonTutorialStepData();
			commonTutorialStepData.stepType = TutorialStepType.CONTENT_AND_FINGER;
			commonTutorialStepData.tutorialContent = Mod.Localization.GetInfoById(325);
			commonTutorialStepData.position = new Rect(upgradeBtn.position.x, upgradeBtn.position.y, upgradeBtn.sizeDelta.x, upgradeBtn.sizeDelta.y);
			commonTutorialStepData.target = upgradeBtn;
			commonTutorialStepData.useViewportAdjustPos = true;
			commonTutorialStepData.needBlock = true;
			commonTutorialStepData.stepAction = delegate
			{
				playerInfoInspector.OnUpgradeClick();
			};
			commonTutorialData.endAction = delegate
			{
				TutorialManager.Instance.EndCurrentStage();
				TutorialForTapTap();
			};
			commonTutorialData.AddStep(commonTutorialStepData);
			BuildinTutorialForm.Form.StartTutorial(commonTutorialData);
		}

		private void TutorialForTapTap()
		{
			if (TutorialManager.Instance.GetCurrentStageId() == TutorialStageId.STAGE_TAPTAP)
			{
				MonoSingleton<GameTools>.Instacne.DisableInputForAWhile();
				CommonTutorialData commonTutorialData = new CommonTutorialData(-1);
				CommonTutorialStepData commonTutorialStepData = new CommonTutorialStepData();
				commonTutorialStepData.stepType = TutorialStepType.CONTENT_AND_FINGER;
				commonTutorialStepData.tutorialContent = Mod.Localization.GetInfoById(326);
				RectTransform component = MenuForm.Form.m_upgrade.GetComponent<RectTransform>();
				commonTutorialStepData.position = new Rect(component.position.x, component.position.y, component.sizeDelta.x, component.sizeDelta.y);
				commonTutorialStepData.target = component;
				commonTutorialStepData.useViewportAdjustPos = true;
				commonTutorialStepData.needBlock = true;
				commonTutorialStepData.stepAction = delegate
				{
					MenuForm.Form.OnUpgradeBtnClick();
				};
				commonTutorialData.AddStep(commonTutorialStepData);
				commonTutorialStepData = new CommonTutorialStepData();
				commonTutorialStepData.stepType = TutorialStepType.ONLY_CONTENT;
				commonTutorialStepData.tutorialContent = Mod.Localization.GetInfoById(327);
				commonTutorialStepData.blockBtnEnable = false;
				commonTutorialStepData.needBlock = true;
				commonTutorialStepData.finishAction = delegate
				{
					TutorialManager.Instance.EndCurrentStage();
				};
				commonTutorialData.AddStep(commonTutorialStepData);
				BuildinTutorialForm.Form.StartTutorial(commonTutorialData);
			}
		}

		protected override void OnClose(object userData)
		{
			base.OnClose(userData);
			RemoveEventListener();
			for (int i = 0; i < itemControllerList.Count; i++)
			{
				itemControllerList[i].Reset();
			}
			m_PlayerAssetList.OnClose();
			m_PlayerAssetList.OnClose();
			m_isPlayingEffect = false;
			m_UnLockEffect.SetActive(false);
			m_AdState = AdPlayState.NONE;
			m_currentIndexId = -1;
			m_isPlayingTweenAniation = false;
			if (m_HideList != null)
			{
				m_HideList.Clear();
			}
			ClearLeaveFormPluginAdData();
			RenderTextureManager.Release(ref m_levelBgRenderTexture);
			Form = null;
		}

		private void InitLevelInfoById(int seriesId)
		{
			LevelOrder_levelOrderTable[] records = Mod.DataTable.Get<LevelOrder_levelOrderTable>().Records;
			LevelSeriesController levelSeriesController = null;
			LevelSeries_table levelTableItem = null;
			int num = GetOrderIndexBySeriesId(seriesId) - 2;
			if (num < 0)
			{
				num = 0;
			}
			int num2 = num + 5;
			if (num2 > records.Length)
			{
				num2 = records.Length;
			}
			if (num2 - num < 5)
			{
				num = num2 - 5;
			}
			List<string> idlist = new List<string>();
			for (int i = num; i < num2; i++)
			{
				idlist.Add(records[i].LevelSeriesId.ToString());
			}
			List<LevelSeriesController> list = itemControllerList.FindAll((LevelSeriesController x) => !idlist.Contains(x.gameObject.name));
			for (int j = 0; j < list.Count; j++)
			{
				LevelSeriesController levelSeriesController2 = list[j];
				itemControllerList.Remove(levelSeriesController2);
				levelSeriesController2.Release();
				levelSeriesController2.transform.SetParent(null);
				levelSeriesController2.gameObject.SetActive(false);
				UnityEngine.Object.Destroy(levelSeriesController2.gameObject);
				levelSeriesController2 = null;
			}
			for (int k = num; k < num2; k++)
			{
				levelTableItem = Mod.DataTable.Get<LevelSeries_table>()[records[k].LevelSeriesId];
				if (!itemControllerList.Exists((LevelSeriesController x) => x.gameObject.name.Equals(levelTableItem.Id.ToString())))
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(levelItem);
					gameObject.SetActive(true);
					levelSeriesController = gameObject.GetComponent<LevelSeriesController>();
					levelSeriesController.SetData(levelTableItem, this, records[k].Id);
					AddChild(gameObject.transform, levelsContainer.transform, k);
					gameObject.gameObject.SetActive(false);
					gameObject.name = records[k].LevelSeriesId.ToString();
					itemControllerList.Add(levelSeriesController);
				}
			}
			itemControllerList.Sort((LevelSeriesController x, LevelSeriesController y) => x.Index - y.Index);
		}

		public void NavigateToSpecialSeries(int seriesId, UnityAction finishAction = null)
		{
			InitLevelInfoById(seriesId);
			if (m_CurrentItemController != null)
			{
				m_CurrentItemController.SetBgShow(false);
				m_CurrentItemController.gameObject.SetActive(false);
				m_CurrentItemController.OnHide();
			}
			m_preItemController = null;
			if (m_HideList != null)
			{
				m_HideList.Clear();
			}
			m_currentIndexId = GetOrderIndexBySeriesId(seriesId) + 1;
			GoToSeries(seriesId, MoveDirection.NONE, finishAction);
		}

		private bool OnRootRedPointHandler()
		{
			bool flag = false;
			int num = 0;
			if ((bool)mExchangeStoreRedPoint && mExchangeStoreRedPoint.mCurrentState)
			{
				flag = true;
				num++;
			}
			if (m_ShopRedPoint != null && m_ShopRedPoint.mCurrentState)
			{
				flag = true;
				num++;
			}
			if (flag)
			{
				mRootRedPoint.SetRedPointTxt(num.ToString());
			}
			if ((bool)mGiftRedPoint)
			{
				flag |= mGiftRedPoint.mCurrentState;
			}
			return flag;
		}

		private bool OnExchangeStoreRedPointHandler()
		{
			return Singleton<DataModuleManager>.Instance.GetDataModule<ExchangeStoreDataModule>(DataNames.ExchangeStoreDataModule).IsNeedRedPoint();
		}

		private bool OnGiftRedPointHandler()
		{
			int nextGiftId = PlayerDataModule.Instance.PlayerGiftPackageData.GetNextGiftId();
			Shops_shopTable shops_shopTable = Mod.DataTable.Get<Shops_shopTable>()[nextGiftId];
			int totalAdCount = PlayerDataModule.Instance.PlayerGiftPackageData.totalAdCount;
			if (shops_shopTable.AdFreeCount != -1)
			{
				return totalAdCount >= shops_shopTable.AdFreeCount;
			}
			return false;
		}

		private int GetFirstSeriesId()
		{
			return Mod.DataTable.Get<LevelOrder_levelOrderTable>().Records[0].LevelSeriesId;
		}

		private int GetLastSerieslId()
		{
			LevelOrder_levelOrderTable[] records = Mod.DataTable.Get<LevelOrder_levelOrderTable>().Records;
			return records[records.Length - 1].LevelSeriesId;
		}

		private int MaxSeriesNum()
		{
			return Mod.DataTable.Get<LevelOrder_levelOrderTable>().Records.Length - 1;
		}

		private void Init()
		{
			m_levelBgContainer = UnityEngine.Object.Instantiate(ResourcesManager.Load<GameObject>("LevelBgContainer"));
			m_levelBgContainer.transform.SetParent(base.transform);
			m_levelBgContainer.transform.localPosition = new Vector3(0f, 0f, m_levelBgContainer.transform.localPosition.z * 0.1f);
			m_bgCamera = EducationDisplayDirector.Instance._Camera.transform.GetComponentInChildren<Camera>();
			totalLevelNum = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).GetLevelNum();
			scrowLocalPos = levelsContainer.transform.localPosition;
			if (DeviceManager.Instance.IsNeedSpecialAdapte())
			{
				MonoSingleton<GameTools>.Instacne.AdapteSpecialScreen(topLeftContainer.transform as RectTransform);
				MonoSingleton<GameTools>.Instacne.AdapteSpecialScreen(m_TopContainer.transform as RectTransform);
			}
			m_LevelDragScrollView = viewport.GetComponent<LevelSeriesDragScrollView>();
			m_PlayerAssetList.OnInit();
			Camera componentInChildren = m_levelBgContainer.GetComponentInChildren<Camera>();
			DeviceQuality quality = DeviceManager.Instance.GetQuality();
			int num;
			switch (quality)
			{
			case DeviceQuality.FAST:
			case DeviceQuality.LOW:
			case DeviceQuality.NORMAL:
				num = Mathf.Min(512, Screen.width);
				break;
			case DeviceQuality.MID:
			case DeviceQuality.HIGTH:
				num = Mathf.Min(1024, Screen.width);
				break;
			default:
				num = Mathf.Min(512, Screen.width);
				Debug.LogErrorFormat("Unexpected deviceQuality({0})!levelSelectRenderTexureWidth use ({1}).", quality, num);
				break;
			}
			int num2 = num;
			num = 1280;
			num2 = 720;
			m_levelBgRenderTexture = RenderTextureManager.Get(num, num2);
			componentInChildren.targetTexture = m_levelBgRenderTexture;
			GameObject obj = new GameObject();
			obj.name = "SelectLevelBackground";
			obj.transform.SetParent(base.gameObject.transform, false);
			obj.transform.SetAsFirstSibling();
			obj.AddComponent<RawImage>().texture = m_levelBgRenderTexture;
			RectTransform component = obj.GetComponent<RectTransform>();
			component.offsetMax = new Vector2(0f, 0f);
			component.offsetMin = new Vector2(0f, 0f);
			component.anchorMax = new Vector2(1f, 1f);
			component.anchorMin = new Vector2(0f, 0f);
			component.pivot = new Vector2(0.5f, 0.5f);
		}

		private void AddChild(Transform child, Transform parent, int index)
		{
			child.transform.SetParent(parent.transform);
			child.transform.SetSiblingIndex(index);
			child.transform.localPosition = new Vector3(0f, 0f, 0f);
			RectTransform obj = child.transform as RectTransform;
			obj.anchoredPosition3D = new Vector3(0f, 0f, 0f);
			obj.offsetMin = new Vector2(obj.offsetMin.x, 0f);
			obj.offsetMax = new Vector2(obj.offsetMax.x, 0f);
			child.transform.localRotation = Quaternion.identity;
			child.transform.localScale = Vector3.one;
		}

		private GameObject FindCennterSeriesObj()
		{
			if (levelsContainer != null)
			{
				int index = Mathf.RoundToInt(Mathf.Abs(levelsContainer.localPosition.y) / CellH);
				return levelsContainer.transform.GetChild(index).gameObject;
			}
			return null;
		}

		public void SetDirectionShowState()
		{
			if (State == HomeFormState.SelectLevel)
			{
				m_rightDirection.transform.Find("NSICON").gameObject.SetActive(false);
				m_leftDirection.transform.Find("NSICON").gameObject.SetActive(false);
				int num = GetOrderIndexBySeriesId(CurrentSeriesId) + 1;
				int num2 = 1;
				int num3 = MaxSeriesNum();
				if (num == num2)
				{
					m_rightDirection.SetActive(true);
					m_leftDirection.SetActive(false);
				}
				else if (num == num3)
				{
					m_rightDirection.SetActive(false);
					m_leftDirection.SetActive(true);
				}
				else
				{
					m_rightDirection.SetActive(true);
					m_leftDirection.SetActive(true);
				}
				OnChangeController(null, null);
			}
		}

		private void GoToSeries(int seriesId, MoveDirection direction = MoveDirection.NONE, UnityAction switchFinisAction = null)
		{
			if (seriesId < 0)
			{
				return;
			}
			GetOrderIndexBySeriesId(seriesId);
			LevelSeriesController levelSeriesController = null;
			for (int i = 0; i < itemControllerList.Count; i++)
			{
				if (itemControllerList[i].name.Equals(seriesId.ToString()))
				{
					levelSeriesController = itemControllerList[i];
					break;
				}
			}
			m_CurrentItemController = levelSeriesController;
			if (levelSeriesController != null)
			{
				if (levelSeriesController.BgLoaded)
				{
					SwitchSeries(levelSeriesController, direction, switchFinisAction);
				}
				else
				{
					WaitSeriesBgLoaded(levelSeriesController, direction, switchFinisAction);
				}
			}
		}

		private void WaitSeriesBgLoaded(LevelSeriesController itemController, MoveDirection direction, UnityAction switchFinisAction = null)
		{
			MonoTimer timer = new MonoTimer(0.02f, true);
			timer.Elapsed += delegate
			{
				if (itemController.BgLoaded)
				{
					SwitchSeries(itemController, direction, switchFinisAction);
					timer.Stop();
				}
			};
			timer.FireElapsedOnStop = false;
			timer.Start();
		}

		private void SwitchSeries(LevelSeriesController itemController, MoveDirection direction, UnityAction switchFinisAction = null)
		{
			if (m_preItemController != null && m_preItemController != itemController)
			{
				m_preItemController.HideCurrentLevel(direction);
				if (m_HideList == null)
				{
					m_HideList = new List<LevelSeriesController>();
				}
				m_HideList.Add(m_preItemController);
				m_showBgController = m_preItemController;
				StopMusic();
			}
			itemController.ShowCurrentLevel(direction);
			CurrentSeriesId = itemController.GetSeriesId();
			m_preItemController = itemController;
			SetDirectionShowState();
			if (State == HomeFormState.SelectLevel)
			{
				m_coroutine = PlayMusic(CurrentSeriesId);
				StartCoroutine(m_coroutine);
			}
			itemController.OnShow();
			if (switchFinisAction != null)
			{
				switchFinisAction();
			}
		}

		public void ShowPreIemBg()
		{
			if (m_showBgController != null)
			{
				m_showBgController.SetBgShow(true);
			}
		}

		public void HidePreIemBg()
		{
			if (m_HideList != null && m_HideList.Count > 0)
			{
				for (int i = 0; i < m_HideList.Count; i++)
				{
					m_HideList[i].SetBgShow(false);
				}
			}
			m_HideList.Clear();
		}

		private int GetOrderIndexBySeriesId(int levelId)
		{
			LevelOrder_levelOrderTable[] records = Mod.DataTable.Get<LevelOrder_levelOrderTable>().Records;
			for (int i = 0; i < records.Length; i++)
			{
				if (records[i].LevelSeriesId == levelId)
				{
					return records[i].Id - 1;
				}
			}
			return 0;
		}

		public void PlayHadNewUnLockEffect()
		{
			m_newLevelLockAlertAnimator.enabled = true;
		}

		public void StopHadNewUnLockEffect()
		{
			m_newLevelLockAlertAnimator.enabled = false;
			m_newLevelLockAlertEffect.SetActive(false);
		}

		private void AddEventListener()
		{
			EventTriggerListener.Get(viewport).onClick = ItemBtnHandler;
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(m_leftDirection);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(LeftDirctionHandler));
			EventTriggerListener eventTriggerListener2 = EventTriggerListener.Get(m_rightDirection);
			eventTriggerListener2.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener2.onClick, new EventTriggerListener.VoidDelegate(RightDirctionHandler));
			EventTriggerListener eventTriggerListener3 = EventTriggerListener.Get(m_levelDebuEnter);
			eventTriggerListener3.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener3.onClick, new EventTriggerListener.VoidDelegate(LevelDebuEnterHandler));
			EventTriggerListener eventTriggerListener4 = EventTriggerListener.Get(cup0Tips);
			eventTriggerListener4.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener4.onClick, new EventTriggerListener.VoidDelegate(OnClickCupTips));
			EventTriggerListener eventTriggerListener5 = EventTriggerListener.Get(cup1Tips);
			eventTriggerListener5.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener5.onClick, new EventTriggerListener.VoidDelegate(OnClickCupTips));
			EventTriggerListener eventTriggerListener6 = EventTriggerListener.Get(luckyTurntableEntry);
			eventTriggerListener6.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener6.onClick, new EventTriggerListener.VoidDelegate(OnClickLuckyTurntableEntry));
			Mod.Event.Subscribe(EventArgs<UIFormOpenEvent>.EventId, OnUIFormOpened);
			Mod.Event.Subscribe(EventArgs<UIFormCloseEvent>.EventId, OnUIFormClosed);
			Mod.Event.Subscribe(EventArgs<InstrumentPropertyChangeEventArgs>.EventId, OnInstrumentPropertyChangeHandler);
			Mod.Event.Subscribe(EventArgs<ChangeControllerArgs>.EventId, OnChangeController);
			if ((bool)mRootRedPoint)
			{
				RedPointBehaviour redPointBehaviour = mRootRedPoint;
				redPointBehaviour.mIsNeedHandler = (RedPointBehaviour.IsNeedHandler)Delegate.Combine(redPointBehaviour.mIsNeedHandler, new RedPointBehaviour.IsNeedHandler(OnRootRedPointHandler));
			}
			if ((bool)mExchangeStoreRedPoint)
			{
				RedPointBehaviour redPointBehaviour2 = mExchangeStoreRedPoint;
				redPointBehaviour2.mIsNeedHandler = (RedPointBehaviour.IsNeedHandler)Delegate.Combine(redPointBehaviour2.mIsNeedHandler, new RedPointBehaviour.IsNeedHandler(OnExchangeStoreRedPointHandler));
			}
			if (m_luckyTurntableEntryRedPoint != null)
			{
				RedPointBehaviour luckyTurntableEntryRedPoint = m_luckyTurntableEntryRedPoint;
				luckyTurntableEntryRedPoint.mIsNeedHandler = (RedPointBehaviour.IsNeedHandler)Delegate.Combine(luckyTurntableEntryRedPoint.mIsNeedHandler, new RedPointBehaviour.IsNeedHandler(OnLuckTurnRedPointHandler));
			}
			if (m_ShopRedPoint != null)
			{
				RedPointBehaviour shopRedPoint = m_ShopRedPoint;
				shopRedPoint.mIsNeedHandler = (RedPointBehaviour.IsNeedHandler)Delegate.Combine(shopRedPoint.mIsNeedHandler, new RedPointBehaviour.IsNeedHandler(OnShopRedPointHandler));
			}
			if ((bool)mGiftRedPoint)
			{
				RedPointBehaviour redPointBehaviour3 = mGiftRedPoint;
				redPointBehaviour3.mIsNeedHandler = (RedPointBehaviour.IsNeedHandler)Delegate.Combine(redPointBehaviour3.mIsNeedHandler, new RedPointBehaviour.IsNeedHandler(OnGiftRedPointHandler));
			}
		}

		private bool OnShopRedPointHandler()
		{
			DateTime start = TimeTools.StringToDatetime(PlayerDataModule.Instance.GetAppLaunchTime());
			DateTime now = DateTime.Now;
			if (PlayerDataModule.Instance.IsAppFirstLaunch() && OPEN_COUNT < 2)
			{
				return true;
			}
			if (!TimeTools.IsSameDay(start, now) && OPEN_COUNT < 2)
			{
				return true;
			}
			return false;
		}

		private bool OnLuckTurnRedPointHandler()
		{
			return m_luckData.FreeCount >= 1;
		}

		private void LevelDebuEnterHandler(GameObject go)
		{
			Mod.UI.OpenUIForm(UIFormId.LevelEnterDebugForm);
		}

		public void LeftMoveHandle()
		{
			if (PlayingTweenAniation || m_isPlayingEffect || !IsPreLoadedItmeFinished())
			{
				return;
			}
			int num = GetOrderIndexBySeriesId(CurrentSeriesId) + 1;
			num--;
			if (num < 1)
			{
				num = 1;
			}
			if (m_currentIndexId == num)
			{
				return;
			}
			m_currentIndexId = num;
			int levelSeriesId = Mod.DataTable.Get<LevelOrder_levelOrderTable>()[num].LevelSeriesId;
			int num2 = num - 2;
			if (num2 < 1)
			{
				num2 = 1;
			}
			LevelOrder_levelOrderTable[] records = Mod.DataTable.Get<LevelOrder_levelOrderTable>().Records;
			int startSeriesId = records[num2 - 1].LevelSeriesId;
			if (!itemControllerList.Exists((LevelSeriesController x) => x.gameObject.name.Equals(startSeriesId.ToString())) && !itemControllerList[0].name.Equals(startSeriesId.ToString()))
			{
				LevelSeriesController levelSeriesController = itemControllerList.RemoveLast();
				if (levelSeriesController != null)
				{
					levelSeriesController.Release();
					LevelSeries_table data = Mod.DataTable.Get<LevelSeries_table>()[startSeriesId];
					levelSeriesController.SetData(data, this, num2);
					levelSeriesController.gameObject.SetActive(false);
					levelSeriesController.gameObject.name = startSeriesId.ToString();
					itemControllerList.Insert(0, levelSeriesController);
				}
			}
			GoToSeries(levelSeriesId, MoveDirection.LEFT);
			m_changePageTotalCount++;
		}

		private bool IsPreLoadedItmeFinished()
		{
			bool flag = true;
			for (int i = 0; i < itemControllerList.Count; i++)
			{
				flag &= itemControllerList[i].BgLoaded;
			}
			return flag;
		}

		private void LeftDirctionHandler(GameObject go)
		{
			LeftMoveHandle();
		}

		public void RightMoveHandle()
		{
			if (PlayingTweenAniation || m_isPlayingEffect)
			{
				return;
			}
			if (TutorialManager.Instance.GetCurrentStageId() == TutorialStageId.STAGE_NEXT_LEVEL)
			{
				TutorialManager.Instance.EndCurrentStage();
				nextLevelTutorialRoot.SetActive(false);
			}
			if (!IsPreLoadedItmeFinished())
			{
				return;
			}
			int num = GetOrderIndexBySeriesId(CurrentSeriesId) + 1;
			num++;
			int num2 = MaxSeriesNum();
			if (num > num2)
			{
				num = num2;
			}
			if (m_currentIndexId == num)
			{
				return;
			}
			m_currentIndexId = num;
			int levelSeriesId = Mod.DataTable.Get<LevelOrder_levelOrderTable>()[num].LevelSeriesId;
			int num3 = num - 2;
			if (num3 < 1)
			{
				num3 = 1;
			}
			int num4 = num3 + 5 - 1;
			LevelOrder_levelOrderTable[] records = Mod.DataTable.Get<LevelOrder_levelOrderTable>().Records;
			if (num4 > records.Length)
			{
				num4 = records.Length;
			}
			int endSeriesId = records[num4 - 1].LevelSeriesId;
			if (!itemControllerList.Exists((LevelSeriesController x) => x.gameObject.name.Equals(endSeriesId.ToString())) && !itemControllerList[4].name.Equals(endSeriesId.ToString()))
			{
				LevelSeriesController levelSeriesController = itemControllerList[0];
				itemControllerList.RemoveAt(0);
				if (levelSeriesController != null)
				{
					levelSeriesController.Release();
					LevelSeries_table data = Mod.DataTable.Get<LevelSeries_table>()[endSeriesId];
					levelSeriesController.SetData(data, this, endSeriesId);
					levelSeriesController.gameObject.SetActive(false);
					levelSeriesController.gameObject.name = endSeriesId.ToString();
					itemControllerList.Add(levelSeriesController);
				}
			}
			GoToSeries(levelSeriesId, MoveDirection.RIGHT);
			m_changePageTotalCount++;
		}

		private void RightDirctionHandler(GameObject go)
		{
			RightMoveHandle();
		}

		private void RemoveEventListener()
		{
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(m_leftDirection);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(LeftDirctionHandler));
			EventTriggerListener eventTriggerListener2 = EventTriggerListener.Get(m_rightDirection);
			eventTriggerListener2.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener2.onClick, new EventTriggerListener.VoidDelegate(RightDirctionHandler));
			EventTriggerListener eventTriggerListener3 = EventTriggerListener.Get(m_levelDebuEnter);
			eventTriggerListener3.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener3.onClick, new EventTriggerListener.VoidDelegate(LevelDebuEnterHandler));
			EventTriggerListener eventTriggerListener4 = EventTriggerListener.Get(cup0Tips);
			eventTriggerListener4.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener4.onClick, new EventTriggerListener.VoidDelegate(OnClickCupTips));
			EventTriggerListener eventTriggerListener5 = EventTriggerListener.Get(cup1Tips);
			eventTriggerListener5.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener5.onClick, new EventTriggerListener.VoidDelegate(OnClickCupTips));
			EventTriggerListener eventTriggerListener6 = EventTriggerListener.Get(luckyTurntableEntry);
			eventTriggerListener6.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener6.onClick, new EventTriggerListener.VoidDelegate(OnClickLuckyTurntableEntry));
			Mod.Event.Unsubscribe(EventArgs<UIFormOpenEvent>.EventId, OnUIFormOpened);
			Mod.Event.Unsubscribe(EventArgs<UIFormCloseEvent>.EventId, OnUIFormClosed);
			Mod.Event.Unsubscribe(EventArgs<InstrumentPropertyChangeEventArgs>.EventId, OnInstrumentPropertyChangeHandler);
			Mod.Event.Unsubscribe(EventArgs<ChangeControllerArgs>.EventId, OnChangeController);
			if ((bool)mRootRedPoint)
			{
				RedPointBehaviour redPointBehaviour = mRootRedPoint;
				redPointBehaviour.mIsNeedHandler = (RedPointBehaviour.IsNeedHandler)Delegate.Remove(redPointBehaviour.mIsNeedHandler, new RedPointBehaviour.IsNeedHandler(OnRootRedPointHandler));
			}
			if ((bool)mExchangeStoreRedPoint)
			{
				RedPointBehaviour redPointBehaviour2 = mExchangeStoreRedPoint;
				redPointBehaviour2.mIsNeedHandler = (RedPointBehaviour.IsNeedHandler)Delegate.Remove(redPointBehaviour2.mIsNeedHandler, new RedPointBehaviour.IsNeedHandler(OnExchangeStoreRedPointHandler));
			}
			if (m_luckyTurntableEntryRedPoint != null)
			{
				RedPointBehaviour luckyTurntableEntryRedPoint = m_luckyTurntableEntryRedPoint;
				luckyTurntableEntryRedPoint.mIsNeedHandler = (RedPointBehaviour.IsNeedHandler)Delegate.Remove(luckyTurntableEntryRedPoint.mIsNeedHandler, new RedPointBehaviour.IsNeedHandler(OnLuckTurnRedPointHandler));
			}
			if (m_ShopRedPoint != null)
			{
				RedPointBehaviour shopRedPoint = m_ShopRedPoint;
				shopRedPoint.mIsNeedHandler = (RedPointBehaviour.IsNeedHandler)Delegate.Remove(shopRedPoint.mIsNeedHandler, new RedPointBehaviour.IsNeedHandler(OnShopRedPointHandler));
			}
			if ((bool)mGiftRedPoint)
			{
				RedPointBehaviour redPointBehaviour3 = mGiftRedPoint;
				redPointBehaviour3.mIsNeedHandler = (RedPointBehaviour.IsNeedHandler)Delegate.Remove(redPointBehaviour3.mIsNeedHandler, new RedPointBehaviour.IsNeedHandler(OnGiftRedPointHandler));
			}
		}

		private void OnChangeController(object sender, Foundation.EventArgs args)
		{
			m_rightDirection.transform.Find("NSICON").gameObject.SetActive(InputService.UseController);
			m_leftDirection.transform.Find("NSICON").gameObject.SetActive(InputService.UseController);
		}

		private void OnClickLuckyTurntableEntry(GameObject go)
		{
			Mod.UI.OpenUIForm(UIFormId.LuckyTurntableForm);
		}

		public void UnLockEnterLevel(int levelId)
		{
			StartCoroutine(DeltaEnterLeve(1.5f, levelId));
		}

		private void OpenLevelLockForm(int levelId, LevelSeriesController itemController)
		{
			LevelLockData levelLockData = new LevelLockData();
			levelLockData.currentLevelId = levelId;
			levelLockData.m_isHadDowloaded = itemController.BundleReady;
			levelLockData.callBack = delegate(LevelLockData.EnterLevelType type)
			{
				switch (type)
				{
				case LevelLockData.EnterLevelType.TRY:
					if (!itemController.BundleReady)
					{
						itemController.TryDownloadLevel();
					}
					else
					{
						PlayerLocalLevelData playerLevelData = PlayerDataModule.Instance.GetPlayerLevelData(levelId);
						PlayerDataModule.Instance.SetLevelTicketNum(levelId, playerLevelData.GetTicketNum() - 1);
						EnterLevelById(levelId);
					}
					break;
				case LevelLockData.EnterLevelType.LOCKED:
					UnLockEnterLevel(levelId);
					break;
				}
			};
			Mod.UI.OpenUIForm(UIFormId.LevelLockForm, levelLockData);
		}

		private void ItemBtnHandler(GameObject go)
		{
			if (TutorialManager.Instance.IsTutorialStageFinish(TutorialStageId.STAGE_NEXT_LEVEL) && !PlayingTweenAniation && !m_isPlayingEffect && !m_LevelDragScrollView.isDrag && !PlayingTweenAniation)
			{
				Vector3 validScreenPos = MonoSingleton<GameTools>.Instacne.GetValidScreenPos(Input.mousePosition);
				Vector3 vector = Mod.UI.UICamera.ScreenToViewportPoint(validScreenPos);
				RaycastHit hitInfo;
				if (!(vector.y < 0f) && !(vector.x < 0f) && Physics.Raycast(m_bgCamera.ScreenPointToRay(validScreenPos), out hitInfo, 1000f, LayerMask.GetMask("LevelSelect")) && CurrentSeriesId != 7)
				{
					m_CurrentItemController.EnterLevel();
				}
			}
		}

		public void ShopBtnHander(GameObject go)
		{
			UGUIForm uIForm = Mod.UI.GetUIForm(UIFormId.ShopForm);
			if (uIForm != null && uIForm is IActiveForm)
			{
				((IActiveForm)uIForm).ShowForm();
				OPEN_COUNT++;
			}
		}

		public void ExchangeStoreBtnHander(GameObject go)
		{
			Mod.UI.OpenUIForm(UIFormId.ExchangeStoreForm);
		}

		public void SettingBtnHander(GameObject go)
		{
			Mod.UI.OpenUIForm(UIFormId.GameSettingForm);
		}

		private IEnumerator DeltaEnterLeve(float deltaTime, int levelId)
		{
			m_isPlayingEffect = true;
			yield return new WaitForSeconds(deltaTime);
			m_UnLockEffect.SetActive(true);
			yield return new WaitForSeconds(1f);
			m_isPlayingEffect = false;
			LevelSeriesController leveItemControllerById = GetLeveItemControllerById(levelId);
			if (!leveItemControllerById.BundleReady)
			{
				leveItemControllerById.TryDownloadLevel();
			}
			else
			{
				EnterLevelById(levelId);
			}
		}

		public void EnterLevelById(int levelId)
		{
			Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).CurLevelId = levelId;
			MenuProcedure obj = (MenuProcedure)Mod.Procedure.Current;
			int sceneIdByLevelId = MonoSingleton<GameTools>.Instacne.GetSceneIdByLevelId(levelId);
			obj.WillToScene(sceneIdByLevelId);
			Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).CurrentEnterLevelType = LevelEnterType.MENU;
		}

		public void SetLevelBgContainer(Transform levelBg)
		{
			if (m_levelBgContainer != null)
			{
				levelBg.SetParent(m_levelBgContainer.transform);
				levelBg.localPosition = Vector3.zero;
			}
		}

		protected override void OnUnload()
		{
			base.OnUnload();
			for (int i = 0; i < itemControllerList.Count; i++)
			{
				itemControllerList[i].Release();
			}
			for (int j = 0; j < loadedAsserts.Count; j++)
			{
				Mod.Resource.UnloadAsset(loadedAsserts[j]);
			}
			loadedAsserts.Clear();
		}

		private LevelSeriesController GetLeveItemControllerById(int levelId)
		{
			for (int i = 0; i < itemControllerList.Count; i++)
			{
				if (itemControllerList[i].GetSeriesId() == levelId)
				{
					return itemControllerList[i];
				}
			}
			return null;
		}

		protected override void OnTick(float elapseSeconds, float realElapseSeconds)
		{
			base.OnTick(elapseSeconds, realElapseSeconds);
			m_PlayerAssetList.OnUpdate();
			m_pluginAdTime += elapseSeconds;
			if (m_pluginAdTime >= 60f)
			{
				ClearLeaveFormPluginAdData();
			}
		}

		public void ChangeBgColorById(bool tween, int seriesId)
		{
			Color32 seriesBgColor = MonoSingleton<GameTools>.Instacne.GetSeriesBgColor(seriesId);
			foreach (Renderer item in EducationDisplayDirector.Instance.m_MaskRenders)
			{
				if (tween)
				{
					DOTween.To(() => MaterialTool.GetMaterialColor(item, "_TintColor"), delegate(Color x)
					{
						MaterialTool.SetMaterialColor(item, "_TintColor", x);
					}, seriesBgColor, 1f);
				}
				else
				{
					MaterialTool.SetMaterialColor(item, "_TintColor", seriesBgColor);
				}
			}
		}

		public bool isLoadComplete()
		{
			bool flag = true;
			for (int i = 0; i < itemControllerList.Count; i++)
			{
				flag &= itemControllerList[i].BgLoaded;
			}
			return flag;
		}

		private IEnumerator PlayMusic(int seriesID)
		{
			if (m_currentSeriesId == seriesID)
			{
				yield break;
			}
			m_currentSeriesId = seriesID;
			int muisicId = Mod.DataTable.Get<LevelSeries_table>()[m_currentSeriesId].MuisicId;
			int num = 100;
			Singleton<MenuMusicController>.Instance.PlayMenuMusic(muisicId, 0f);
			if (num >= 100)
			{
				yield break;
			}
			Music_levelMusicTable music_levelMusicTable = Mod.DataTable.Get<Music_levelMusicTable>().Get((Music_levelMusicTable x) => x.Id == seriesID);
			float time = music_levelMusicTable.Length * (float)num * 0.01f;
			float addTime = 0f;
			while (true)
			{
				addTime += Time.deltaTime;
				if (addTime > time)
				{
					Singleton<MenuMusicController>.Instance.StopMenuMusic();
					yield return new WaitForSeconds(1f);
					Singleton<MenuMusicController>.Instance.PlayMenuMusic(m_currentSeriesId, 0f);
					addTime = 0f;
				}
				yield return null;
			}
		}

		private void StopMusic()
		{
			Singleton<MenuMusicController>.Instance.StopMenuMusic();
			if (m_coroutine != null)
			{
				StopCoroutine(m_coroutine);
			}
		}

		private void OnUIFormOpened(object sender, Foundation.EventArgs args)
		{
			if ((args as UIFormOpenEvent).UIFormId == UIFormId.LevelEnterForm && videoAwardRoot != null)
			{
				videoAwardRoot.gameObject.SetActive(false);
			}
		}

		private void OnUIFormClosed(object sender, Foundation.EventArgs args)
		{
			if ((args as UIFormCloseEvent).UIFormId == UIFormId.LevelEnterForm && videoAwardRoot != null)
			{
				videoAwardRoot.gameObject.SetActive(true);
				videoAwardRoot.videoAwardItem.StartWork();
			}
		}

		private void OnInstrumentPropertyChangeHandler(object sender, Foundation.EventArgs args)
		{
			InstrumentPropertyChangeEventArgs instrumentPropertyChangeEventArgs = args as InstrumentPropertyChangeEventArgs;
			if (instrumentPropertyChangeEventArgs == null || instrumentPropertyChangeEventArgs.InstrumentIds == null || instrumentPropertyChangeEventArgs.InstrumentIds.Count <= 0 || instrumentPropertyChangeEventArgs.ChangeType != InstrumentPropertyType.LOCK_STATE || Mod.UI.UIFormIsOpen(UIFormId.InstrumentUnlockForm))
			{
				return;
			}
			List<int> list = new List<int> { 2 };
			if (instrumentPropertyChangeEventArgs.InstrumentIds.Count > 0)
			{
				int num = instrumentPropertyChangeEventArgs.InstrumentIds[0];
				PlayerLocalInstrumentData instrumentDataById = PlayerDataModule.Instance.GetInstrumentDataById(num);
				if (instrumentDataById == null || (int)instrumentDataById.LockState > 0 || list.Contains(num))
				{
					return;
				}
			}
			InstrumentUnlockForm.Data data = new InstrumentUnlockForm.Data();
			data.ids = new List<int>(instrumentPropertyChangeEventArgs.InstrumentIds);
			data.type = instrumentPropertyChangeEventArgs.ChangeType;
			Mod.UI.OpenUIForm(UIFormId.InstrumentUnlockForm, data);
		}

		private void AdEventChangeHandler(object sender, Foundation.EventArgs args)
		{
			AdPlayEventArgs adPlayEventArgs = args as AdPlayEventArgs;
			if (adPlayEventArgs != null)
			{
				m_AdState = (AdPlayState)adPlayEventArgs.AdState;
				if (m_AdState == AdPlayState.FAILED || m_AdState == AdPlayState.NOAD || m_AdState == AdPlayState.SUCCESS)
				{
					m_coroutine = PlayMusic(CurrentSeriesId);
					StartCoroutine(m_coroutine);
				}
				else if (m_AdState == AdPlayState.START)
				{
					StopMusic();
				}
			}
		}

		public void OnSelect(bool isSelect)
		{
			HideTutorialForm(!isSelect);
			if (m_homeHintRoot != null)
			{
				m_homeHintRoot.OnSelectRefresh(isSelect);
			}
		}

		private void HideTutorialForm(bool isHide)
		{
			if (Mod.UI.UIFormIsOpen(UIFormId.CommonTutorialForm))
			{
				UGUIForm uIForm = Mod.UI.GetUIForm(UIFormId.CommonTutorialForm);
				if (!(uIForm == null) && uIForm.gameObject.activeInHierarchy != !isHide)
				{
					uIForm.gameObject.SetActive(!isHide);
				}
			}
		}

		private void InitTweenParam()
		{
			m_TopContainer.endValueV3 = new Vector3(0f, m_TopContainer.transform.localPosition.y + 350f, 0f);
			m_LeftContainer.endValueV3 = new Vector3(m_LeftTarget.localPosition.x, m_LeftContainer.transform.localPosition.y, 0f);
			m_RightContainer.endValueV3 = new Vector3(m_RightTarget.localPosition.x, m_RightContainer.transform.localPosition.y, 0f);
			m_TopContainer.CreateTween();
			m_LeftContainer.CreateTween();
			m_RightContainer.CreateTween();
		}

		public void SetState(HomeFormState state)
		{
			State = state;
			switch (state)
			{
			case HomeFormState.Education:
				m_levelBgContainer.SetActive(false);
				levelsContainer.gameObject.SetActive(false);
				m_rightDirection.SetActive(false);
				m_leftDirection.SetActive(false);
				if ((bool)videoAwardRoot)
				{
					videoAwardRoot.gameObject.SetActive(true);
				}
				m_TopContainer.DOPlayBackwards();
				m_LeftContainer.DOPlayBackwards();
				m_RightContainer.DOPlayBackwards();
				StopCoroutine("DelayOpenLevelsContainer");
				if (m_coroutine != null)
				{
					StopCoroutine(m_coroutine);
					m_currentSeriesId = -1;
				}
				m_CurrentItemController.OnHide();
				Singleton<MenuMusicController>.Instance.PlayMenuMusic(54, 0f);
				MenuForm.isSelectLevel = false;
				break;
			case HomeFormState.SelectLevel:
				StartCoroutine("DelayOpenLevelsContainer");
				m_levelBgContainer.SetActive(true);
				if ((bool)videoAwardRoot)
				{
					videoAwardRoot.gameObject.SetActive(false);
				}
				m_TopContainer.DOPlayForward();
				m_LeftContainer.DOPlayForward();
				m_RightContainer.DOPlayForward();
				m_CurrentItemController.OnShow();
				m_coroutine = PlayMusic(CurrentSeriesId);
				StartCoroutine(m_coroutine);
				DealWithNextLevelTutorial();
				MenuForm.isSelectLevel = true;
				break;
			}
		}

		private IEnumerator DelayOpenLevelsContainer()
		{
			yield return new WaitForSeconds(1.2f);
			levelsContainer.gameObject.SetActive(true);
			SetDirectionShowState();
		}

		public void GoToLevel(int level)
		{
			int seriesID = GetSeriesID(level);
			if (seriesID != -1)
			{
				NavigateToSpecialSeries(seriesID, delegate
				{
					m_CurrentItemController.m_LevelListController.SwitchLevel(level);
					EducationDisplayDirector.Instance.OnSelectLevel();
				});
			}
		}

		private int GetSeriesID(int level)
		{
			return PlayerDataModule.Instance.GetPlayerLevelData(level).GetLevelSeriesId();
		}

		private bool DealLeaveformPluginAd()
		{
			int id = MonoSingleton<GameTools>.Instacne.ComputerScreenPluginAdId(5);
			ScreenPluginsAd_table screenPluginsAd_table = Mod.DataTable.Get<ScreenPluginsAd_table>()[id];
			bool flag = true;
			if (screenPluginsAd_table != null)
			{
				flag &= m_changePageTotalCount >= screenPluginsAd_table.TriggerNum;
				if (flag)
				{
					flag &= PlayerDataModule.Instance.PluginAdController.IsScreenAdRead();
					if (flag)
					{
						PluginAdData pluginAdData = new PluginAdData();
						pluginAdData.PluginId = 5;
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

		private bool DealQuitLevelPluginAd()
		{
			int id = MonoSingleton<GameTools>.Instacne.ComputerScreenPluginAdId(13);
			ScreenPluginsAd_table screenPluginsAd_table = Mod.DataTable.Get<ScreenPluginsAd_table>()[id];
			bool flag = true;
			if (screenPluginsAd_table != null)
			{
				flag &= QUIT_LEVEL_COUNT >= screenPluginsAd_table.TriggerNum;
				if (flag)
				{
					flag &= PlayerDataModule.Instance.PluginAdController.IsScreenAdRead();
					if (flag)
					{
						PluginAdData pluginAdData = new PluginAdData();
						pluginAdData.PluginId = 13;
						pluginAdData.EndHandler = delegate
						{
							ClearQuitLevelPluginAdData();
						};
						Mod.UI.OpenUIForm(UIFormId.ScreenPluginsForm, pluginAdData);
					}
				}
			}
			return flag;
		}

		private void ClearQuitLevelPluginAdData()
		{
			QUIT_LEVEL_COUNT = 0;
		}

		public RenderTexture GetLevelBgRenderTexture()
		{
			return m_levelBgRenderTexture;
		}

		public void OnClickCupTips(GameObject go)
		{
			HideCupTips();
		}

		private void HideCupTips()
		{
			cup0Tips.SetActive(false);
			cup1Tips.SetActive(false);
		}
	}
}
