using System;
using System.Collections.Generic;
using Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace RS2
{
	public class OriginRebirthForm : UGUIForm
	{
		public class OriginRebirthFormData
		{
			public bool m_isGuideUnlimitedRebirth;
		}

		public enum RebirthState
		{
			NormalRebirth,
			FreeRebirth,
			UnlimitedRebirth,
			AdRebirth
		}

		public Text m_timeProgressText;

		public Text m_levelProgressText;

		public Text m_heartRebirthCountText;

		public Text m_heartRebirthButtonText;

		public Text m_needHeartRebirthCountText;

		public GameObject m_haveInfo;

		public Image m_rebirthIcon;

		public Button m_heartRebirthButton;

		public Image m_unlimitedRebirthIcon;

		public Button m_freeRebirthButton;

		public Text m_freeRebirthText;

		public Button m_unlimitedRebirthButton;

		public Text m_unlimitedRebirthText;

		public Button m_adButton;

		public SetUIGrey m_adGrey;

		public Button m_closeButton;

		public Image m_timeSlider;

		public UIPersonalAssetsList m_personalAssetsList;

		private bool m_isPause;

		private float m_currentTime;

		private float m_continueTime = GameCommon.originRebirthContinueTime;

		private float m_continueShowTime = GameCommon.originRebirthContinueShowTime;

		private float m_laveTime = 10f;

		private float m_progress;

		private readonly int m_keyToResurrectionHeartProductID = 10006;

		public ParticleSystem m_buttonClickParticleSystem;

		private float m_currentRemoveAdTime;

		private float m_waitCloseTimeMax = 0.4f;

		private float m_waitCloseTime;

		private bool m_isWaitClose;

		private bool m_isCanClickHeartRebirthButton;

		private int m_needHeartRebirthCount;

		private int m_needkeyCount = 1;

		private int m_keyToHeartRebirth = 5;

		private bool m_isMoneyShopBuy;

		private bool m_isUpdateShowDisount = true;

		private float RemoveAdTime
		{
			get
			{
				return GameCommon.COMMON_AD_REFRESHTIME * 60f;
			}
		}

		private int HeartRebirthCount
		{
			get
			{
				return (int)Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).GetPlayGoodsNum(2);
			}
		}

		private InsideGameDataModule GetInsideGameDataModule
		{
			get
			{
				return Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule);
			}
		}

		private GameDataModule GetGameDataModule
		{
			get
			{
				return Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule);
			}
		}

		private PlayerDataModule GetPlayerDataModule
		{
			get
			{
				return Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule);
			}
		}

		private long KeyNumber
		{
			get
			{
				return (long)Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).GetPlayGoodsNum(6);
			}
		}

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			if (m_personalAssetsList != null)
			{
				m_personalAssetsList.OnInit();
				if (DeviceManager.Instance.IsNeedSpecialAdapte())
				{
					MonoSingleton<GameTools>.Instacne.AdapteSpecialScreen(m_personalAssetsList.transform as RectTransform);
				}
			}
		}

		private bool IsUnlimitedRebirthLevel()
		{
			bool flag = PlayerDataModule.Instance.GetLevelMaxProgress(GetGameDataModule.CurLevelId) >= 100;
			if (false)
			{
				return !flag;
			}
			return false;
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			OriginRebirthFormData originRebirthFormData = userData as OriginRebirthFormData;
			m_isPause = false;
			m_isMoneyShopBuy = false;
			m_currentRemoveAdTime = 0f;
			if (m_personalAssetsList != null)
			{
				m_personalAssetsList.OnOpen(UIPersonalAssetsList.ParentType.OriginRebirth);
			}
			RefreshRebirthButton();
			LoadLocalTable();
			RefreshLanguage();
			m_isPause = false;
			AddEventListener();
			ResetWindow(true);
			m_isUpdateShowDisount = false;
			m_isWaitClose = false;
			m_isCanClickHeartRebirthButton = true;
			if (originRebirthFormData != null && originRebirthFormData.m_isGuideUnlimitedRebirth)
			{
				m_isPause = true;
				CommonTutorialData commonTutorialData = new CommonTutorialData(4);
				CommonTutorialStepData commonTutorialStepData = new CommonTutorialStepData();
				RectTransform component = m_unlimitedRebirthButton.gameObject.GetComponent<RectTransform>();
				commonTutorialStepData.position = new Rect(component.position.x, component.position.y, component.sizeDelta.x, component.sizeDelta.y);
				commonTutorialStepData.changeRect = true;
				commonTutorialStepData.needBlock = true;
				commonTutorialStepData.target = m_unlimitedRebirthButton.transform;
				commonTutorialStepData.stepAction = delegate
				{
					if (Mod.UI.UIFormIsOpen(UIFormId.OriginRebirthForm))
					{
						OnClickHeartRebirthButtonBySelect(m_unlimitedRebirthButton.gameObject);
						OnWaitClose();
					}
				};
				commonTutorialData.AddStep(commonTutorialStepData);
				Mod.UI.OpenUIForm(UIFormId.CommonTutorialForm, commonTutorialData);
				m_isUpdateShowDisount = false;
			}
			RebirthState rebirthState = GetRebirthState();
			if (rebirthState == RebirthState.UnlimitedRebirth)
			{
				m_isUpdateShowDisount = false;
			}
			Mod.Event.Subscribe(EventArgs<BuyOutRebirthShowEventArgs>.EventId, OnBuyOutRebirthShow);
			Mod.Event.Subscribe(EventArgs<UIMod.OpenSuccessEventArgs>.EventId, OnOpenCommonDialogFormSuccess);
			Mod.Event.Subscribe(EventArgs<UIMod.CloseCompleteEventArgs>.EventId, OnCloseCommonDialogFormSuccess);
			Mod.Event.Subscribe(EventArgs<GameGoodsNumChangeEventArgs>.EventId, OnPlayerAssetChange);
			Mod.Event.Subscribe(EventArgs<UIMod.CloseCompleteEventArgs>.EventId, OnCloseCommonMoneyShopSuccess);
			InfocUtils.Report_rollingsky2_games_pageshow(8, 0, 1);
			DealPause();
		}

		private void DealPause()
		{
			if (Mod.UI.GetUIForm(UIFormId.GetGoodsForm) != null)
			{
				m_isPause = true;
			}
		}

		private void RefreshRebirthButton()
		{
			RebirthState rebirthState = GetRebirthState();
			bool flag = Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).BufferIsEnable(GameCommon.ORIGIN_REBIRTH_FREE);
			int freeRebirthCount = GetInsideGameDataModule.FreeRebirthCount;
			switch (rebirthState)
			{
			case RebirthState.NormalRebirth:
				if (m_heartRebirthButton != null)
				{
					m_heartRebirthButton.gameObject.SetActive(true);
				}
				if (m_rebirthIcon != null)
				{
					m_rebirthIcon.gameObject.SetActive(true);
				}
				if (m_adButton != null)
				{
					m_adButton.gameObject.SetActive(false);
				}
				if (m_freeRebirthButton != null)
				{
					m_freeRebirthButton.gameObject.SetActive(false);
				}
				if (m_unlimitedRebirthButton != null)
				{
					m_unlimitedRebirthButton.gameObject.SetActive(false);
				}
				if (m_unlimitedRebirthIcon != null)
				{
					m_unlimitedRebirthIcon.gameObject.SetActive(false);
				}
				if (m_haveInfo != null)
				{
					m_haveInfo.SetActive(true);
				}
				m_needHeartRebirthCount = (flag ? (GetInsideGameDataModule.m_theBoardRebirthCount - freeRebirthCount + 1) : (GetInsideGameDataModule.m_theBoardRebirthCount + 1));
				if (m_needHeartRebirthCount > GameCommon.needHeartRebirthMaxNumber)
				{
					m_needHeartRebirthCount = GameCommon.needHeartRebirthMaxNumber;
				}
				break;
			case RebirthState.FreeRebirth:
				if (m_freeRebirthButton != null)
				{
					m_freeRebirthButton.gameObject.SetActive(true);
				}
				if (m_rebirthIcon != null)
				{
					m_rebirthIcon.gameObject.SetActive(true);
				}
				if (m_heartRebirthButton != null)
				{
					m_heartRebirthButton.gameObject.SetActive(false);
				}
				if (m_unlimitedRebirthButton != null)
				{
					m_unlimitedRebirthButton.gameObject.SetActive(false);
				}
				if (m_unlimitedRebirthIcon != null)
				{
					m_unlimitedRebirthIcon.gameObject.SetActive(false);
				}
				if (m_adButton != null)
				{
					m_adButton.gameObject.SetActive(false);
				}
				if (m_haveInfo != null)
				{
					m_haveInfo.SetActive(false);
				}
				m_needHeartRebirthCount = 0;
				break;
			case RebirthState.UnlimitedRebirth:
				if (m_unlimitedRebirthButton != null)
				{
					m_unlimitedRebirthButton.gameObject.SetActive(true);
				}
				if (m_unlimitedRebirthIcon != null)
				{
					m_unlimitedRebirthIcon.gameObject.SetActive(true);
				}
				if (m_freeRebirthButton != null)
				{
					m_freeRebirthButton.gameObject.SetActive(false);
				}
				if (m_rebirthIcon != null)
				{
					m_rebirthIcon.gameObject.SetActive(false);
				}
				if (m_heartRebirthButton != null)
				{
					m_heartRebirthButton.gameObject.SetActive(false);
				}
				if (m_adButton != null)
				{
					m_adButton.gameObject.SetActive(false);
				}
				if (m_haveInfo != null)
				{
					m_haveInfo.SetActive(false);
				}
				m_needHeartRebirthCount = 0;
				break;
			case RebirthState.AdRebirth:
				if (m_heartRebirthButton != null)
				{
					m_heartRebirthButton.gameObject.SetActive(true);
				}
				if (m_rebirthIcon != null)
				{
					m_rebirthIcon.gameObject.SetActive(true);
				}
				if (m_adButton != null)
				{
					m_adButton.gameObject.SetActive(true);
					bool flag2 = MonoSingleton<GameTools>.Instacne.CanShowAd(ADScene.ReBirthView);
					m_adButton.interactable = flag2;
					if (m_adGrey != null)
					{
						m_adGrey.SetGrey(!flag2);
					}
				}
				if (m_freeRebirthButton != null)
				{
					m_freeRebirthButton.gameObject.SetActive(false);
				}
				if (m_unlimitedRebirthButton != null)
				{
					m_unlimitedRebirthButton.gameObject.SetActive(false);
				}
				if (m_unlimitedRebirthIcon != null)
				{
					m_unlimitedRebirthIcon.gameObject.SetActive(false);
				}
				if (m_haveInfo != null)
				{
					m_haveInfo.SetActive(true);
				}
				m_needHeartRebirthCount = (flag ? (GetInsideGameDataModule.m_theBoardRebirthCount - freeRebirthCount + 1) : (GetInsideGameDataModule.m_theBoardRebirthCount + 1));
				if (m_needHeartRebirthCount > GameCommon.needHeartRebirthMaxNumber)
				{
					m_needHeartRebirthCount = GameCommon.needHeartRebirthMaxNumber;
				}
				break;
			}
		}

		protected override void OnTick(float elapseSeconds, float realElapseSeconds)
		{
			base.OnTick(elapseSeconds, realElapseSeconds);
			if (m_personalAssetsList != null)
			{
				m_personalAssetsList.OnUpdate();
			}
			if (m_isUpdateShowDisount)
			{
				int? shopID;
				if (GetInsideGameDataModule.IsCanShowOriginRebirthDiscount(out shopID))
				{
					if (shopID.HasValue && !Mod.UI.UIFormIsOpen(UIFormId.BuyOutRebirthForm))
					{
						ShowOriginRebirthDiscount(shopID.Value);
						m_isUpdateShowDisount = false;
					}
				}
				else
				{
					m_isUpdateShowDisount = false;
				}
			}
			CheckNeedPause();
			if (!m_isPause)
			{
				m_currentTime += elapseSeconds;
				if (m_currentTime > m_continueTime)
				{
					BackToResultForm();
					return;
				}
				m_laveTime = m_continueTime - m_currentTime;
				m_progress = m_laveTime / m_continueTime;
				m_timeSlider.fillAmount = m_progress;
				m_timeProgressText.text = Mathf.CeilToInt(m_laveTime * (m_continueShowTime / m_continueTime)).ToString();
			}
			if ((bool)m_adButton && !m_adButton.interactable && !m_isPause)
			{
				m_currentRemoveAdTime += elapseSeconds;
				if (m_currentRemoveAdTime >= RemoveAdTime)
				{
					bool flag = MonoSingleton<GameTools>.Instacne.CanShowAd(ADScene.ReBirthView);
					m_adButton.interactable = flag;
					m_adGrey.SetGrey(!flag);
					m_currentRemoveAdTime = 0f;
				}
			}
			if (m_isWaitClose)
			{
				m_waitCloseTime += Time.deltaTime;
				if (m_waitCloseTime >= m_waitCloseTimeMax)
				{
					OnWaitClose();
					m_isWaitClose = false;
				}
			}
		}

		protected override void OnClose(object userData)
		{
			base.OnClose(userData);
			if (m_personalAssetsList != null)
			{
				m_personalAssetsList.OnClose();
			}
			RemoveEventListener();
			Mod.Event.Unsubscribe(EventArgs<BuyOutRebirthShowEventArgs>.EventId, OnBuyOutRebirthShow);
			Mod.Event.Unsubscribe(EventArgs<UIMod.OpenSuccessEventArgs>.EventId, OnOpenCommonDialogFormSuccess);
			Mod.Event.Unsubscribe(EventArgs<UIMod.CloseCompleteEventArgs>.EventId, OnCloseCommonDialogFormSuccess);
			Mod.Event.Unsubscribe(EventArgs<GameGoodsNumChangeEventArgs>.EventId, OnPlayerAssetChange);
			Mod.Event.Unsubscribe(EventArgs<UIMod.CloseCompleteEventArgs>.EventId, OnCloseCommonMoneyShopSuccess);
		}

		private void CheckNeedPause()
		{
			if (Mod.UI.UIFormIsOpen(UIFormId.BuyOutRebirthForm))
			{
				m_isPause = true;
			}
		}

		private void OnBuyOutRebirthShow(object sender, Foundation.EventArgs e)
		{
			if (((BuyOutRebirthShowEventArgs)e).IsShow)
			{
				m_isPause = true;
			}
			else if (!Mod.UI.UIFormIsOpen(UIFormId.CommonAlertForm))
			{
				m_isPause = false;
			}
		}

		private void OnOpenCommonDialogFormSuccess(object sender, Foundation.EventArgs e)
		{
			UIMod.OpenSuccessEventArgs openSuccessEventArgs = (UIMod.OpenSuccessEventArgs)e;
			if (openSuccessEventArgs != null && (bool)(openSuccessEventArgs.UIForm.Logic as CommonDialogForm))
			{
				m_isPause = true;
			}
		}

		private void OnCloseCommonDialogFormSuccess(object sender, Foundation.EventArgs e)
		{
			UIMod.CloseCompleteEventArgs closeCompleteEventArgs = (UIMod.CloseCompleteEventArgs)e;
			if (closeCompleteEventArgs != null)
			{
				string assetName = closeCompleteEventArgs.AssetName;
				string uIFormAsset = AssetUtility.GetUIFormAsset(Mod.DataTable.Get<UIForms_uiformTable>().Get(7).AssetName);
				if (assetName.Equals(uIFormAsset) && !Mod.UI.UIFormIsOpen(UIFormId.CommonAlertForm) && Mod.UI.UIFormIsOpen(UIFormId.OriginRebirthForm))
				{
					m_isPause = false;
					ResetWindow(true);
				}
			}
		}

		private void OnCloseCommonMoneyShopSuccess(object sender, Foundation.EventArgs e)
		{
			UIMod.CloseCompleteEventArgs closeCompleteEventArgs = (UIMod.CloseCompleteEventArgs)e;
			if (closeCompleteEventArgs != null)
			{
				string assetName = closeCompleteEventArgs.AssetName;
				string uIFormAsset = AssetUtility.GetUIFormAsset(Mod.DataTable.Get<UIForms_uiformTable>().Get(10).AssetName);
				if (assetName.Equals(uIFormAsset) && Mod.UI.UIFormIsOpen(UIFormId.OriginRebirthForm) && !m_isMoneyShopBuy)
				{
					Mod.UI.CloseUIForm(UIFormId.CommonAlertForm);
					m_isPause = false;
					ResetWindow(true);
				}
			}
		}

		private void AddEventListener()
		{
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(m_heartRebirthButton.gameObject);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(OnClickHeartRebirthButtonBySelect));
			EventTriggerListener eventTriggerListener2 = EventTriggerListener.Get(m_freeRebirthButton.gameObject);
			eventTriggerListener2.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener2.onClick, new EventTriggerListener.VoidDelegate(OnClickHeartRebirthButtonBySelect));
			EventTriggerListener eventTriggerListener3 = EventTriggerListener.Get(m_unlimitedRebirthButton.gameObject);
			eventTriggerListener3.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener3.onClick, new EventTriggerListener.VoidDelegate(OnClickHeartRebirthButtonBySelect));
			EventTriggerListener eventTriggerListener4 = EventTriggerListener.Get(m_adButton.gameObject);
			eventTriggerListener4.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener4.onClick, new EventTriggerListener.VoidDelegate(OnClickAdButtonHandler));
			EventTriggerListener eventTriggerListener5 = EventTriggerListener.Get(m_closeButton.gameObject);
			eventTriggerListener5.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener5.onClick, new EventTriggerListener.VoidDelegate(OnClickCloseButton));
			Mod.Event.Subscribe(EventArgs<UIMod.CloseCompleteEventArgs>.EventId, CloseUIFormSuccess);
		}

		private void CloseUIFormSuccess(object sender, Foundation.EventArgs args)
		{
			UIMod.CloseCompleteEventArgs closeCompleteEventArgs = args as UIMod.CloseCompleteEventArgs;
			if (closeCompleteEventArgs != null && closeCompleteEventArgs.AssetName.Equals(UIExtension.GetUIFormAssetName(UIFormId.GetGoodsForm)))
			{
				m_isPause = false;
			}
		}

		private void RemoveEventListener()
		{
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(m_heartRebirthButton.gameObject);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(OnClickHeartRebirthButtonBySelect));
			EventTriggerListener eventTriggerListener2 = EventTriggerListener.Get(m_freeRebirthButton.gameObject);
			eventTriggerListener2.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener2.onClick, new EventTriggerListener.VoidDelegate(OnClickHeartRebirthButtonBySelect));
			EventTriggerListener eventTriggerListener3 = EventTriggerListener.Get(m_unlimitedRebirthButton.gameObject);
			eventTriggerListener3.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener3.onClick, new EventTriggerListener.VoidDelegate(OnClickHeartRebirthButtonBySelect));
			EventTriggerListener eventTriggerListener4 = EventTriggerListener.Get(m_adButton.gameObject);
			eventTriggerListener4.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener4.onClick, new EventTriggerListener.VoidDelegate(OnClickAdButtonHandler));
			EventTriggerListener eventTriggerListener5 = EventTriggerListener.Get(m_closeButton.gameObject);
			eventTriggerListener5.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener5.onClick, new EventTriggerListener.VoidDelegate(OnClickCloseButton));
			Mod.Event.Unsubscribe(EventArgs<UIMod.CloseCompleteEventArgs>.EventId, CloseUIFormSuccess);
		}

		private void OnClickCloseButton(GameObject obj)
		{
			BackToResultForm();
		}

		private void BackToMenuForm()
		{
			Mod.UI.CloseUIForm(UIFormId.OriginRebirthForm);
			Mod.Event.Fire(this, Mod.Reference.Acquire<GameExitEventArgs>());
		}

		private void BackToResultForm()
		{
			Mod.UI.CloseUIForm(UIFormId.OriginRebirthForm);
			Mod.Event.Fire(this, Mod.Reference.Acquire<GameOverEventArgs>());
		}

		private void OnClickHeartRebirthButtonBySelect(GameObject obj)
		{
			if (m_isCanClickHeartRebirthButton)
			{
				InfocUtils.Report_rollingsky2_games_pageshow(8, 9, 2);
				OnHeartRebirthButton();
				m_isCanClickHeartRebirthButton = false;
			}
		}

		private void OnHeartRebirthButton()
		{
			Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule).m_isOriginRebirth = true;
			RangeHeartRebirth(-m_needHeartRebirthCount);
			InfocUtils.Report_rollingsky2_games_currency(2, 2, 9);
			if (TutorialManager.Instance.GetCurrentStageId() != TutorialStageId.STAGE_TUTORIAL_LEVEL)
			{
				InfocUtils.Report_rollingsky2_games_currency(2, 2, 12, GetGameDataModule.CurLevelId);
			}
			m_buttonClickParticleSystem.Play(true);
			m_waitCloseTime = 0f;
			m_isWaitClose = true;
			m_isPause = true;
		}

		private void OnClickAdButtonHandler(GameObject obj)
		{
			if (m_adButton.interactable)
			{
				m_isPause = true;
				MonoSingleton<GameTools>.Instacne.PlayerVideoAd(ADScene.ReBirthView, OnAdSuccessCallBack, OnAdFailCallBack, OnAdNoAdCallBack, null, OnAdCloseCallBack);
				InfocUtils.Report_rollingsky2_games_ads(10, 0, 1, GetGameDataModule.CurLevelId, 3, 0);
			}
		}

		private void OnWaitClose()
		{
			Mod.Event.Fire(this, GameOriginRebirthResetEventArgs.Make());
			Mod.UI.CloseUIForm(UIFormId.OriginRebirthForm);
			Mod.UI.CloseUIForm(UIFormId.CommonAlertForm);
		}

		private void OnExchangeHeartButton()
		{
			Shops_shopTable _shopTable = Mod.DataTable.Get<Shops_shopTable>()[m_keyToResurrectionHeartProductID];
			CommonAlertData _data = new CommonAlertData();
			_data.showType = CommonAlertData.AlertShopType.BUY_SHOPITEM;
			_data.shopItemId = _shopTable.Id;
			_data.lableContent = Mod.Localization.GetInfoById(21);
			_data.alertContent = Mod.Localization.GetInfoById(58);
			int itemNum = m_needHeartRebirthCount - HeartRebirthCount;
			_data.isBuyBySinglePrice = true;
			_data.itemNum = itemNum;
			m_isPause = true;
			_data.closeCallBackFunc = delegate
			{
				if (Mod.UI.UIFormIsOpen(UIFormId.OriginRebirthForm))
				{
					m_isPause = false;
					ResetWindow(true);
				}
			};
			_data.callBackFunc = delegate
			{
				if (!Mod.UI.UIFormIsOpen(UIFormId.OriginRebirthForm))
				{
					Mod.UI.CloseUIForm(UIFormId.CommonAlertForm);
				}
				else
				{
					InfocUtils.Report_rollingsky2_games_neigou(_shopTable.Id, 2, Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).CurLevelId);
					Dictionary<int, int> dictionary = MonoSingleton<GameTools>.Instacne.DealGoodsTeamById(_shopTable.GoodsTeamid);
					int num = 0;
					int num2 = 0;
					foreach (KeyValuePair<int, int> item in dictionary)
					{
						num = item.Value;
					}
					num2 = int.Parse(_shopTable.Price);
					num2 = Mathf.RoundToInt(_data.itemNum * num2);
					num *= _data.itemNum;
					m_needkeyCount = num2;
					m_keyToHeartRebirth = num;
					if (KeyNumber >= m_needkeyCount)
					{
						m_isPause = false;
						Mod.UI.CloseUIForm(UIFormId.CommonAlertForm);
						OnBuy();
						ResetWindow(true);
						OnHeartRebirthButton();
						OnWaitClose();
						InfocUtils.Report_rollingsky2_games_neigou(_shopTable.Id, 3, Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).CurLevelId);
					}
					else
					{
						m_isMoneyShopBuy = false;
						ResetWindow(false);
						InfocUtils.Report_rollingsky2_games_neigou(_shopTable.Id, 4, Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).CurLevelId);
					}
				}
			};
			Mod.UI.OpenUIForm(UIFormId.CommonAlertForm, _data);
		}

		private void RefreshLanguage()
		{
			InsideGameDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule);
			if (dataModule.CurrentOriginRebirth != null)
			{
				OriginRebirthProgressData originRebirthProgressData = default(OriginRebirthProgressData);
				originRebirthProgressData = ((!RecordOriginRebirthManager.m_isBson) ? ProgressController.LoadData(dataModule.CurrentOriginRebirth.m_rowValue.m_sceneData.m_progressData) : ProgressController.LoadData(dataModule.CurrentOriginRebirth.m_rowValue.m_sceneData.ProgressData));
				if (m_levelProgressText != null)
				{
					m_levelProgressText.text = Mod.Localization.GetInfoById(49) + originRebirthProgressData.m_levelProgress + "%";
				}
				if (m_heartRebirthButtonText != null)
				{
					m_heartRebirthButtonText.text = Mod.Localization.GetInfoById(212);
				}
				if (m_unlimitedRebirthText != null)
				{
					m_unlimitedRebirthText.text = Mod.Localization.GetInfoById(101);
				}
				if (m_freeRebirthText != null)
				{
					m_freeRebirthText.text = Mod.Localization.GetInfoById(101);
				}
				if (m_needHeartRebirthCountText != null)
				{
					m_needHeartRebirthCountText.text = "X" + m_needHeartRebirthCount;
				}
				if (m_timeProgressText != null)
				{
					m_timeProgressText.text = Mathf.CeilToInt((m_continueTime - m_currentTime) * (m_continueShowTime / m_continueTime)).ToString();
				}
			}
		}

		private void RangeHeartRebirth(int num)
		{
			Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).ChangePlayerGoodsNum(2, num);
		}

		private void RangeKey(int num)
		{
			Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).ChangePlayerGoodsNum(6, num);
		}

		private void OnBuy()
		{
			RangeKey(-m_needkeyCount);
			RangeHeartRebirth(m_keyToHeartRebirth);
			InfocUtils.Report_rollingsky2_games_currency(2, 1, 9);
		}

		private void ResetWindow(bool resetData)
		{
			if (resetData)
			{
				m_currentTime = 0f;
			}
			RefreshRebirthButton();
			switch (GetRebirthState())
			{
			case RebirthState.NormalRebirth:
			case RebirthState.AdRebirth:
				if (m_needHeartRebirthCountText != null)
				{
					m_needHeartRebirthCountText.text = "X" + m_needHeartRebirthCount;
				}
				if (m_heartRebirthCountText != null)
				{
					m_heartRebirthCountText.text = MonoSingleton<GameTools>.Instacne.ProductsCountToString(HeartRebirthCount);
				}
				break;
			case RebirthState.FreeRebirth:
				if (m_heartRebirthCountText != null)
				{
					m_heartRebirthCountText.text = MonoSingleton<GameTools>.Instacne.ProductsCountToString(HeartRebirthCount);
				}
				break;
			case RebirthState.UnlimitedRebirth:
				if (m_heartRebirthCountText != null)
				{
					m_heartRebirthCountText.text = "1";
				}
				break;
			}
		}

		private void LoadLocalTable()
		{
			Shops_shopTable shops_shopTable = Mod.DataTable.Get<Shops_shopTable>()[m_keyToResurrectionHeartProductID];
			if (shops_shopTable != null)
			{
				Dictionary<int, int>.Enumerator enumerator = MonoSingleton<GameTools>.Instacne.DealGoodsTeamById(shops_shopTable.GoodsTeamid).GetEnumerator();
				while (enumerator.MoveNext())
				{
					m_keyToHeartRebirth = enumerator.Current.Value;
				}
				m_needkeyCount = int.Parse(shops_shopTable.Price);
			}
		}

		private RebirthState GetRebirthState()
		{
			RebirthState result = RebirthState.NormalRebirth;
			int num = 0;
			bool num2 = PlayerDataModule.Instance.BufferIsEnable(GameCommon.ORIGIN_REBIRTH_FREE) || GetInsideGameDataModule.HasUseRebirthItem;
			int num3 = GetInsideGameDataModule.FreeRebirthCount + num;
			if (num2 ? (GetInsideGameDataModule.m_theBoardRebirthCount - 1 < GetInsideGameDataModule.m_maxShowRebirthAdByTheBoardCount) : (GetInsideGameDataModule.m_theBoardRebirthCount < GetInsideGameDataModule.m_maxShowRebirthAdByTheBoardCount))
			{
				result = RebirthState.AdRebirth;
			}
			if (num2 && num3 > GetInsideGameDataModule.m_theBoardRebirthCount)
			{
				result = RebirthState.FreeRebirth;
			}
			if (PlayerDataModule.Instance.BufferIsEnable(GameCommon.ORIGIN_REBIRTH_FREE))
			{
				result = RebirthState.FreeRebirth;
			}
			if (IsUnlimitedRebirthLevel())
			{
				bool isHaveUnlimitedRebirth = GetInsideGameDataModule.IsHaveUnlimitedRebirth;
				if (false && isHaveUnlimitedRebirth)
				{
					result = RebirthState.UnlimitedRebirth;
				}
			}
			return result;
		}

		private void ShowOriginRebirthDiscount(int productID)
		{
			Shops_shopTable _shopTable = Mod.DataTable.Get<Shops_shopTable>().Get(productID);
			CommonAlertData commonAlertData = new CommonAlertData();
			commonAlertData.showType = CommonAlertData.AlertShopType.BUY_SHOPITEM;
			commonAlertData.shopItemId = _shopTable.Id;
			commonAlertData.lableContent = Mod.Localization.GetInfoById(21);
			commonAlertData.alertContent = Mod.Localization.GetInfoById(70);
			GetInsideGameDataModule.m_theLevelShowOriginRebirthDiscountCount++;
			m_isPause = true;
			commonAlertData.closeCallBackFunc = delegate
			{
				if (Mod.UI.UIFormIsOpen(UIFormId.OriginRebirthForm))
				{
					m_isPause = false;
					ResetWindow(true);
				}
			};
			commonAlertData.callBackFunc = delegate
			{
				if (!Mod.UI.UIFormIsOpen(UIFormId.OriginRebirthForm))
				{
					Mod.UI.CloseUIForm(UIFormId.CommonAlertForm);
				}
				else
				{
					InfocUtils.Report_rollingsky2_games_neigou(_shopTable.Id, 2, Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).CurLevelId);
					int num = int.Parse(_shopTable.Price);
					Dictionary<int, int> dictionary = MonoSingleton<GameTools>.Instacne.DealGoodsTeamById(_shopTable.GoodsTeamid);
					int num2 = 0;
					foreach (KeyValuePair<int, int> item in dictionary)
					{
						num2 = item.Value;
					}
					if (KeyNumber >= num)
					{
						m_isPause = false;
						Mod.UI.CloseUIForm(UIFormId.CommonAlertForm);
						RangeKey(-num);
						RangeHeartRebirth(num2);
						BroadResultByGoodsID(2, num2);
						if (_shopTable.Id == 10008)
						{
							InfocUtils.Report_rollingsky2_games_currency(2, 1, 8);
						}
						else if (_shopTable.Id == 10007)
						{
							InfocUtils.Report_rollingsky2_games_currency(2, 1, 7);
						}
						ResetWindow(true);
						InfocUtils.Report_rollingsky2_games_neigou(_shopTable.Id, 3, Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).CurLevelId);
					}
					else
					{
						ResetWindow(false);
						InfocUtils.Report_rollingsky2_games_neigou(_shopTable.Id, 4, Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).CurLevelId);
					}
				}
			};
			Mod.UI.OpenUIForm(UIFormId.CommonAlertForm, commonAlertData);
		}

		private void OnAdSuccessCallBack(ADScene adScene)
		{
			if (adScene == ADScene.ReBirthView && Mod.UI.UIFormIsOpen(UIFormId.OriginRebirthForm))
			{
				m_isPause = false;
				m_currentTime = 0f;
				ResetWindow(true);
				GetInsideGameDataModule.m_theBoardShowRebirthAdCount++;
				GetInsideGameDataModule.m_isAdRebirth = true;
				m_needHeartRebirthCount = 0;
				OnHeartRebirthButton();
				OnWaitClose();
				InfocUtils.Report_rollingsky2_games_ads(10, 0, 1, GetGameDataModule.CurLevelId, 4, 0);
			}
		}

		private void OnAdCloseCallBack(ADScene adScene)
		{
			if (adScene == ADScene.ReBirthView && Mod.UI.UIFormIsOpen(UIFormId.OriginRebirthForm))
			{
				m_isPause = false;
				m_currentTime = 0f;
				ResetWindow(true);
			}
		}

		private void OnAdNoAdCallBack(ADScene adScene)
		{
			if (adScene == ADScene.ReBirthView && Mod.UI.UIFormIsOpen(UIFormId.OriginRebirthForm))
			{
				MonoSingleton<GameTools>.Instacne.GetDefaultNoAdCallback()(adScene);
				m_isPause = false;
				m_currentTime = 0f;
				ResetWindow(true);
			}
		}

		private void OnAdFailCallBack(ADScene adScene)
		{
			if (adScene == ADScene.ReBirthView && Mod.UI.UIFormIsOpen(UIFormId.OriginRebirthForm))
			{
				MonoSingleton<GameTools>.Instacne.GetDefaultFailCallback()(adScene);
				m_isPause = false;
				m_currentTime = 0f;
				ResetWindow(true);
			}
		}

		private void OnPlayerAssetChange(object sender, Foundation.EventArgs e)
		{
			GameGoodsNumChangeEventArgs gameGoodsNumChangeEventArgs = e as GameGoodsNumChangeEventArgs;
			if (gameGoodsNumChangeEventArgs == null)
			{
				return;
			}
			int goodsId = gameGoodsNumChangeEventArgs.GoodsId;
			if (Mod.UI.UIFormIsOpen(UIFormId.MoneyShopForm))
			{
				MoneyShopForm moneyShopForm = Mod.UI.GetUIForm(UIFormId.MoneyShopForm) as MoneyShopForm;
				if (moneyShopForm != null && moneyShopForm.CurrentMoneyType == MoneyShopForm.MoneyType.Money && Mod.UI.UIFormIsOpen(UIFormId.OriginRebirthForm) && goodsId == 6)
				{
					m_isMoneyShopBuy = true;
					Mod.UI.CloseUIForm(UIFormId.MoneyShopForm);
				}
			}
		}

		private void BroadResultByGoodsID(int goodsId, int goodsNum)
		{
			m_isPause = true;
			GetGoodsData getGoodsData = new GetGoodsData();
			getGoodsData.Buy = true;
			getGoodsData.GoodsId = goodsId;
			getGoodsData.GoodsNum = goodsNum;
			getGoodsData.CallBackFunc = delegate
			{
				m_isPause = false;
			};
			Mod.UI.OpenUIForm(UIFormId.GetGoodsForm, getGoodsData);
		}

		private void BroadResulByGoodsTeamID(int goodsTeamId, int goodsTeamNum)
		{
			m_isPause = true;
			GetGoodsData getGoodsData = new GetGoodsData();
			getGoodsData.Buy = true;
			getGoodsData.GoodsTeam = true;
			getGoodsData.GoodsTeamId = goodsTeamId;
			getGoodsData.GoodsTeamNum = goodsTeamNum;
			Mod.UI.OpenUIForm(UIFormId.GetGoodsForm, getGoodsData);
		}
	}
}
