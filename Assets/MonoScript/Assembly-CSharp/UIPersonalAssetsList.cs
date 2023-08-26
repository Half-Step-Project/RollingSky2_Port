using System;
using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.UI;

public class UIPersonalAssetsList : MonoBehaviour
{
	public enum ParentType
	{
		None,
		Home,
		Result,
		Shop,
		OriginRebirth,
		UsingAssert,
		ExchangeStore
	}

	private ParentType parentType;

	public Image m_heartIcon;

	public Text m_heartText;

	public Text recoverTimeTxt;

	public GameObject m_powerBuyButton;

	public Text m_keyText;

	public GameObject m_keyBuyButton;

	public Text m_rebirthText;

	public Text m_shalouText;

	public Image m_TimePower;

	public Text m_timePowerTxt;

	private bool m_isUpdatePowerTime;

	public GameObject m_KeyFreeFlag;

	private long m_keyAdStamp;

	private int m_framCount;

	public Text goldCount;

	[Header("buffer专用")]
	public BufferContainer m_bufferContainer;

	[Header("引导专用")]
	public GameObject m_bufferContainerForTutorial;

	public RectTransform m_bufferContainerCenterForTutorial;

	[Header("其中的按钮是否可以点击")]
	[Label]
	private bool m_isCanClickButtons = true;

	public Text m_CoolPlayTxt;

	private MonoTimer m_timer;

	private uint recoverTimerId;

	public bool needPauseResponseGoodsNumChange;

	private bool pauseResponseGoodsNumChange;

	public bool IsCanClickButtons
	{
		get
		{
			return m_isCanClickButtons;
		}
		set
		{
			m_isCanClickButtons = value;
			if ((bool)m_bufferContainer)
			{
				m_bufferContainer.m_isCanClickButtons = m_isCanClickButtons;
			}
		}
	}

	private PlayerDataModule GetPlayerDataModule
	{
		get
		{
			return Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule);
		}
	}

	public void OnInit()
	{
		if ((bool)m_TimePower)
		{
			m_TimePower.gameObject.SetActive(false);
		}
		if ((bool)m_timePowerTxt)
		{
			m_timePowerTxt.gameObject.SetActive(false);
		}
		if (m_CoolPlayTxt != null)
		{
			m_CoolPlayTxt.gameObject.SetActive(false);
		}
	}

	public void OnOpen(ParentType parentType)
	{
		this.parentType = parentType;
		bool powerTimeShowState = GetPlayerDataModule.PlayerRecordData.IsInNoConsumePowerTime();
		SetPowerTimeShowState(powerTimeShowState);
		SetRecoverTime();
		SetData();
		if (m_bufferContainer != null)
		{
			m_bufferContainer.gameObject.SetActive(true);
			m_bufferContainer.OnOpen();
		}
		DealWithTutorial();
		if ((bool)m_powerBuyButton)
		{
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(m_powerBuyButton);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(OnClickPowerBuyButton));
		}
		if ((bool)m_keyBuyButton)
		{
			EventTriggerListener eventTriggerListener2 = EventTriggerListener.Get(m_keyBuyButton);
			eventTriggerListener2.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener2.onClick, new EventTriggerListener.VoidDelegate(OnClickKeyBuyButton));
		}
		Mod.Event.Subscribe(EventArgs<GameGoodsNumChangeEventArgs>.EventId, OnPlayerAssetChange);
		Mod.Event.Subscribe(EventArgs<PauseResponseGoodsNumChangeEventArgs>.EventId, OnPauseResponseGoodsNumChange);
		Mod.Event.Subscribe(EventArgs<BufferTimeChanged>.EventId, OnBufferTimeChanged);
		Mod.Event.Subscribe(EventArgs<NoConsumePowerTimeChanged>.EventId, OnNoConsumePowerTimeChangeHandler);
		m_keyAdStamp = PlayerDataModule.Instance.GetAdKeyTimeStamp();
		if ((bool)m_KeyFreeFlag)
		{
			m_KeyFreeFlag.SetActive(false);
		}
		if (m_CoolPlayTxt != null)
		{
			EventTriggerListener eventTriggerListener3 = EventTriggerListener.Get(m_CoolPlayTxt.gameObject);
			eventTriggerListener3.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener3.onClick, new EventTriggerListener.VoidDelegate(OnCoolPlayClickHandler));
		}
		if (!PlayerDataModule.Instance.CoolPlayPagageData.IsEnable() || !(m_CoolPlayTxt != null))
		{
			return;
		}
		m_CoolPlayTxt.gameObject.SetActive(true);
		m_timer = new MonoTimer(1f, true);
		m_timer.Elapsed += delegate
		{
			int num = (int)((float)PlayerDataModule.Instance.CoolPlayPagageData.LeftTime() * 0.001f);
			if (num <= 0)
			{
				m_CoolPlayTxt.gameObject.SetActive(false);
				m_timer.Stop();
			}
			else
			{
				m_CoolPlayTxt.text = MonoSingleton<GameTools>.Instacne.TimeFormat_HH_MM_SS(num);
			}
		};
		m_timer.FireElapsedOnStop = false;
		m_timer.Start();
	}

	private void OnCoolPlayClickHandler(GameObject go)
	{
		new CoolPlayData
		{
			Type = CoolPlayData.OpenType.INFO,
			ShopId = GameCommon.COOLPLAY_PACKAGE,
			CallBack = null
		};
	}

	private UIBuffItem.ItemState GetItemStateByBufferID(int id)
	{
		bool num = PlayerDataModule.Instance.BufferIsEnable(id);
		bool flag = PlayerDataModule.Instance.BufferIsEnableForever(id);
		bool flag2 = PlayerDataModule.Instance.BufferIsEnableByTime(id);
		UIBuffItem.ItemState result = UIBuffItem.ItemState.Close;
		if (num)
		{
			if (flag)
			{
				result = UIBuffItem.ItemState.Permanent;
			}
			else if (flag2)
			{
				result = UIBuffItem.ItemState.TimeLimit;
			}
		}
		return result;
	}

	private void OnNoConsumePowerTimeChangeHandler(object sender, Foundation.EventArgs args)
	{
		if (args is NoConsumePowerTimeChanged)
		{
			SetPowerTimeShowState(true);
		}
	}

	public void OnClose()
	{
		m_isUpdatePowerTime = false;
		if ((bool)m_powerBuyButton)
		{
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(m_powerBuyButton);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(OnClickPowerBuyButton));
		}
		if ((bool)m_keyBuyButton)
		{
			EventTriggerListener eventTriggerListener2 = EventTriggerListener.Get(m_keyBuyButton);
			eventTriggerListener2.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener2.onClick, new EventTriggerListener.VoidDelegate(OnClickKeyBuyButton));
		}
		if ((bool)m_bufferContainer)
		{
			m_bufferContainer.OnClose();
		}
		Mod.Event.Unsubscribe(EventArgs<GameGoodsNumChangeEventArgs>.EventId, OnPlayerAssetChange);
		Mod.Event.Unsubscribe(EventArgs<PauseResponseGoodsNumChangeEventArgs>.EventId, OnPauseResponseGoodsNumChange);
		Mod.Event.Unsubscribe(EventArgs<BufferTimeChanged>.EventId, OnBufferTimeChanged);
		Mod.Event.Unsubscribe(EventArgs<NoConsumePowerTimeChanged>.EventId, OnNoConsumePowerTimeChangeHandler);
		TimerHeap.DelTimer(recoverTimerId);
		if (m_CoolPlayTxt != null)
		{
			EventTriggerListener eventTriggerListener3 = EventTriggerListener.Get(m_CoolPlayTxt.gameObject);
			eventTriggerListener3.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener3.onClick, new EventTriggerListener.VoidDelegate(OnCoolPlayClickHandler));
		}
		if (m_timer != null)
		{
			m_timer.Stop();
		}
		m_timer = null;
	}

	public void OnUpdate()
	{
		m_framCount++;
		if (m_isUpdatePowerTime)
		{
			if (GetPlayerDataModule.PlayerRecordData.IsInNoConsumePowerTime())
			{
				ShowPowerLeftTime();
			}
			else
			{
				SetPowerTimeShowState(false);
			}
		}
		if (m_bufferContainer != null && m_bufferContainer.isActiveAndEnabled)
		{
			m_bufferContainer.OnTick(Time.deltaTime, Time.unscaledDeltaTime);
		}
		if (m_framCount % 30 == 0 && (bool)m_KeyFreeFlag)
		{
			m_keyAdStamp = PlayerDataModule.Instance.GetAdKeyTimeStamp();
			if (m_keyAdStamp < PlayerDataModule.Instance.ServerTime)
			{
				m_KeyFreeFlag.SetActive(true);
			}
			else
			{
				m_KeyFreeFlag.SetActive(false);
			}
		}
	}

	public void OnRelease()
	{
		m_framCount = 0;
	}

	private void SetData()
	{
		if ((bool)m_heartText)
		{
			m_heartText.text = MonoSingleton<GameTools>.Instacne.ProductsCountToString(GetPlayerDataModule.GetPlayGoodsNum(1));
		}
		if ((bool)m_keyText)
		{
			m_keyText.text = MonoSingleton<GameTools>.Instacne.ProductsCountToString(GetPlayerDataModule.GetPlayGoodsNum(6));
		}
		if ((bool)m_shalouText)
		{
			m_shalouText.text = MonoSingleton<GameTools>.Instacne.ProductsCountToString(GetPlayerDataModule.GetPlayGoodsNum(11));
		}
		if ((bool)m_rebirthText)
		{
			m_rebirthText.text = MonoSingleton<GameTools>.Instacne.ProductsCountToString(GetPlayerDataModule.GetPlayGoodsNum(2));
		}
		if ((bool)goldCount)
		{
			goldCount.text = MonoSingleton<GameTools>.Instacne.DoubleToFormatString(GetPlayerDataModule.GetPlayGoodsNum(3));
		}
	}

	private void ShowPowerLeftTime()
	{
		if ((bool)m_timePowerTxt)
		{
			long totalSeconds = GetPlayerDataModule.PlayerRecordData.LeftNoConsumePowerTime() / 1000;
			if ((bool)m_timePowerTxt)
			{
				m_timePowerTxt.text = MonoSingleton<GameTools>.Instacne.CommonTimeFormat(totalSeconds);
			}
		}
	}

	private void SetPowerTimeShowState(bool isShow)
	{
		if (isShow)
		{
			if ((bool)m_heartIcon)
			{
				m_heartIcon.gameObject.SetActive(false);
			}
			if ((bool)m_heartText)
			{
				m_heartText.gameObject.SetActive(false);
			}
			if ((bool)recoverTimeTxt)
			{
				recoverTimeTxt.gameObject.SetActive(false);
			}
			if ((bool)m_TimePower)
			{
				m_TimePower.gameObject.SetActive(true);
			}
			if ((bool)m_timePowerTxt)
			{
				m_timePowerTxt.gameObject.SetActive(true);
			}
			ShowPowerLeftTime();
			m_isUpdatePowerTime = true;
		}
		else
		{
			if ((bool)m_heartIcon)
			{
				m_heartIcon.gameObject.SetActive(true);
			}
			if ((bool)m_heartText)
			{
				m_heartText.gameObject.SetActive(true);
			}
			if ((bool)m_TimePower)
			{
				m_TimePower.gameObject.SetActive(false);
			}
			if ((bool)m_timePowerTxt)
			{
				m_timePowerTxt.gameObject.SetActive(false);
			}
			m_isUpdatePowerTime = false;
		}
	}

	private void SetRecoverTime()
	{
		if (recoverTimeTxt == null)
		{
			return;
		}
		PlayerDataModule playerDataModule = GetPlayerDataModule;
		bool num2 = playerDataModule.PlayerRecordData.IsInNoConsumePowerTime();
		double num = playerDataModule.GetPlayGoodsNum(1);
		if (num2 || num >= (double)GameCommon.startPowerRecoverThreshold)
		{
			if ((bool)recoverTimeTxt)
			{
				recoverTimeTxt.gameObject.SetActive(false);
			}
			return;
		}
		if ((bool)recoverTimeTxt)
		{
			recoverTimeTxt.gameObject.SetActive(true);
		}
		recoverTimerId = TimerHeap.AddTimer(0u, 1000u, delegate
		{
			float num3 = (float)playerDataModule.PowerCdGoing() * 0.001f;
			if ((bool)recoverTimeTxt)
			{
				recoverTimeTxt.text = MonoSingleton<GameTools>.Instacne.CommonTimeFormat((long)num3, true);
			}
			num = playerDataModule.GetPlayGoodsNum(1);
			if (num >= (double)GameCommon.startPowerRecoverThreshold)
			{
				if ((bool)recoverTimeTxt)
				{
					recoverTimeTxt.text = "";
				}
				TimerHeap.DelTimer(recoverTimerId);
			}
		});
	}

	private void OnClickKeyBuyButton(GameObject obj)
	{
		bool isCanClickButton = m_isCanClickButtons;
	}

	private void OnClickPowerBuyButton(GameObject obj)
	{
		bool isCanClickButton = m_isCanClickButtons;
	}

	private void OnBufferTimeChanged(object sender, Foundation.EventArgs e)
	{
		if (TutorialManager.Instance.IsTutorialStageFinish(TutorialStageId.STAGE_FIRST_LEVEL) && TutorialManager.Instance.IsTutorialStageFinish(TutorialStageId.STAGE_FIRST_LEVEL_BUFF) && TutorialManager.Instance.GetStage(TutorialStageId.STAGE_BUFFER) != null && !TutorialManager.Instance.IsTutorialStageFinish(TutorialStageId.STAGE_BUFFER) && Mod.UI.UIFormIsOpen(UIFormId.CommonTutorialForm))
		{
			TutorialManager.Instance.SetTutorialStageFlag(TutorialStageId.STAGE_BUFFER);
			Mod.UI.CloseUIForm(UIFormId.CommonTutorialForm);
		}
	}

	private void OnPauseResponseGoodsNumChange(object sender, Foundation.EventArgs e)
	{
		if (needPauseResponseGoodsNumChange)
		{
			PauseResponseGoodsNumChangeEventArgs pauseResponseGoodsNumChangeEventArgs = e as PauseResponseGoodsNumChangeEventArgs;
			if (pauseResponseGoodsNumChangeEventArgs != null)
			{
				pauseResponseGoodsNumChange = pauseResponseGoodsNumChangeEventArgs.IsPause;
			}
		}
	}

	private void OnPlayerAssetChange(object sender, Foundation.EventArgs e)
	{
		if (pauseResponseGoodsNumChange)
		{
			return;
		}
		GameGoodsNumChangeEventArgs gameGoodsNumChangeEventArgs = e as GameGoodsNumChangeEventArgs;
		if (gameGoodsNumChangeEventArgs == null)
		{
			return;
		}
		int goodsId = gameGoodsNumChangeEventArgs.GoodsId;
		switch (goodsId)
		{
		case 1:
			if ((bool)m_heartText)
			{
				m_heartText.text = MonoSingleton<GameTools>.Instacne.ProductsCountToString(GetPlayerDataModule.GetPlayGoodsNum(goodsId));
			}
			break;
		case 6:
			if ((bool)m_keyText)
			{
				m_keyText.text = MonoSingleton<GameTools>.Instacne.ProductsCountToString(GetPlayerDataModule.GetPlayGoodsNum(goodsId));
			}
			break;
		case 11:
			if ((bool)m_shalouText)
			{
				m_shalouText.text = MonoSingleton<GameTools>.Instacne.ProductsCountToString(GetPlayerDataModule.GetPlayGoodsNum(goodsId));
			}
			break;
		case 2:
			if ((bool)m_rebirthText)
			{
				m_rebirthText.text = MonoSingleton<GameTools>.Instacne.ProductsCountToString(GetPlayerDataModule.GetPlayGoodsNum(goodsId));
			}
			break;
		}
		if ((goodsId == GameCommon.START_FREE_SHIELD || goodsId == GameCommon.EVERY_DAY_GIVE_POWER || goodsId == GameCommon.ORIGIN_REBIRTH_FREE) && TutorialManager.Instance.IsTutorialStageFinish(TutorialStageId.STAGE_FIRST_LEVEL) && TutorialManager.Instance.IsTutorialStageFinish(TutorialStageId.STAGE_FIRST_LEVEL_BUFF) && TutorialManager.Instance.GetStage(TutorialStageId.STAGE_BUFFER) != null && !TutorialManager.Instance.IsTutorialStageFinish(TutorialStageId.STAGE_BUFFER) && Mod.UI.UIFormIsOpen(UIFormId.CommonTutorialForm))
		{
			TutorialManager.Instance.SetTutorialStageFlag(TutorialStageId.STAGE_BUFFER);
			Mod.UI.GetUIForm(UIFormId.CommonTutorialForm).Close();
			Mod.UI.CloseUIForm(UIFormId.CommonTutorialForm);
		}
	}

	private void DealWithTutorial()
	{
		if (parentType != ParentType.Home || MonoSingleton<GameTools>.Instacne.NeedShowRemoveAd() || !TutorialManager.Instance.IsTutorialStageFinish(TutorialStageId.STAGE_FIRST_LEVEL_BUFF) || !TutorialManager.Instance.IsTutorialStageFinish(TutorialStageId.STAGE_FIRST_LEVEL) || TutorialManager.Instance.GetStage(TutorialStageId.STAGE_BUFFER) == null || TutorialManager.Instance.IsTutorialStageFinish(TutorialStageId.STAGE_BUFFER))
		{
			return;
		}
		TutorialManager.Instance.SwitchStage(TutorialStageId.STAGE_BUFFER);
		CommonTutorialData commonTutorialData = new CommonTutorialData(-1);
		CommonTutorialStepData commonTutorialStepData = new CommonTutorialStepData();
		RectTransform rectTransform = (m_bufferContainerForTutorial ? (m_bufferContainerForTutorial.transform as RectTransform) : null);
		commonTutorialStepData.showContent = false;
		commonTutorialStepData.needBlock = false;
		commonTutorialStepData.needBack = false;
		if (rectTransform != null)
		{
			commonTutorialStepData.position = new Rect(rectTransform.position.x, rectTransform.position.y, rectTransform.sizeDelta.x, rectTransform.sizeDelta.y);
		}
		if (m_bufferContainerCenterForTutorial != null)
		{
			commonTutorialStepData.position = new Rect(m_bufferContainerCenterForTutorial.position.x, m_bufferContainerCenterForTutorial.position.y, m_bufferContainerCenterForTutorial.sizeDelta.x, m_bufferContainerCenterForTutorial.sizeDelta.y);
		}
		commonTutorialStepData.changeRect = true;
		commonTutorialStepData.finishTargetActive = false;
		commonTutorialStepData.target = (m_bufferContainerForTutorial ? m_bufferContainerForTutorial.transform : null);
		commonTutorialStepData.stepAction = delegate
		{
			NewFeatureForm.Data userData = new NewFeatureForm.Data
			{
				closeCallback = delegate
				{
					Mod.UI.OpenUIForm(UIFormId.BufferShowForm);
					TutorialManager.Instance.EndCurrentStage();
				}
			};
			Mod.UI.OpenUIForm(UIFormId.NewFeatureForm, userData);
		};
		commonTutorialData.AddStep(commonTutorialStepData);
		BaseTutorialStep step = new CommonClickTutorialStep(commonTutorialData);
		if ((bool)m_bufferContainerForTutorial)
		{
			m_bufferContainerForTutorial.SetActive(true);
		}
		TutorialManager.Instance.GetCurrentStage().AddStep(step);
		TutorialManager.Instance.GetCurrentStage().Execute();
	}

	private void OnDestroy()
	{
		TimerHeap.DelTimer(recoverTimerId);
	}
}
