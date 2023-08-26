using System;
using System.Collections.Generic;
using RS2;
using UnityEngine;
using UnityEngine.UI;

public class CommonAlertItemMessionController : CommonAlertItemController
{
	public CommonAlertData.AlertShopType m_showType = CommonAlertData.AlertShopType.MESSION;

	private CommonAlertData m_AlertData;

	public GameObject adBtn;

	public GameObject adBtnDisable;

	public Text rewardContentTxt;

	public Text adTxt;

	public Text lableContent;

	public GameObject btnContent;

	private List<object> loadedAsserts = new List<object>();

	private uint adTimerId;

	public override void Init(CommonAlertData alertData)
	{
		AddEventHandler();
		m_AlertData = alertData;
		adTxt.text = alertData.adContent;
		rewardContentTxt.text = alertData.alertContent;
		lableContent.text = alertData.lableContent;
		SetAdButtonState(m_AlertData.isADBtnShow);
		adTimerId = TimerHeap.AddTimer((uint)(GameCommon.COMMON_AD_REFRESHTIME * 1000f), (uint)(GameCommon.COMMON_AD_REFRESHTIME * 1000f), delegate
		{
			SetAdButtonState(m_AlertData.isADBtnShow);
		});
	}

	private void AddEventHandler()
	{
		EventTriggerListener eventTriggerListener = EventTriggerListener.Get(adBtn);
		eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(AdBtnClickHandler));
		EventTriggerListener eventTriggerListener2 = EventTriggerListener.Get(btnContent);
		eventTriggerListener2.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener2.onClick, new EventTriggerListener.VoidDelegate(ContentBtnClickHandler));
	}

	private void RemoveEventHandler()
	{
		EventTriggerListener eventTriggerListener = EventTriggerListener.Get(adBtn);
		eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(AdBtnClickHandler));
		EventTriggerListener eventTriggerListener2 = EventTriggerListener.Get(btnContent);
		eventTriggerListener2.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener2.onClick, new EventTriggerListener.VoidDelegate(ContentBtnClickHandler));
	}

	private void AdBtnClickHandler(GameObject obj)
	{
		if (!MonoSingleton<GameTools>.Instacne.CanShowAd(ADScene.NONE) || m_AlertData == null)
		{
			return;
		}
		InfocUtils.Report_rollingsky2_games_ads(30, 0, 1, 0, 3, 0);
		MonoSingleton<GameTools>.Instacne.PlayerVideoAd(m_AlertData.adScene, delegate
		{
			m_AlertData.adCallBackFunc();
		}, null, null, delegate
		{
			if (m_AlertData.startADCallBackFunc != null)
			{
				m_AlertData.startADCallBackFunc();
			}
		});
	}

	private void ContentBtnClickHandler(GameObject obj)
	{
		if (m_AlertData != null && m_AlertData.callBackFunc != null)
		{
			m_AlertData.callBackFunc();
		}
	}

	public override void Release()
	{
		TimerHeap.DelTimer(adTimerId);
	}

	public override void Reset()
	{
		RemoveEventHandler();
	}

	public override CommonAlertData.AlertShopType GetAlertType()
	{
		return m_showType;
	}

	private void SetAdButtonState(bool needAD)
	{
		if (!needAD)
		{
			adBtn.SetActive(false);
			adBtnDisable.SetActive(false);
		}
		else if (MonoSingleton<GameTools>.Instacne.CanShowAd(ADScene.NONE))
		{
			adBtn.SetActive(true);
			adBtnDisable.SetActive(false);
			TimerHeap.DelTimer(adTimerId);
			adTxt.text = m_AlertData.adContent;
		}
		else
		{
			adBtn.SetActive(false);
			adBtnDisable.SetActive(true);
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate(adTxt.transform.parent as RectTransform);
	}
}
