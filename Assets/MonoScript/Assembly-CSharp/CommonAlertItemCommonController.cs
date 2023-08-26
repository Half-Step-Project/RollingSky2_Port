using System;
using UnityEngine;
using UnityEngine.UI;

public class CommonAlertItemCommonController : CommonAlertItemController
{
	private CommonAlertData m_AlertData;

	public CommonAlertData.AlertShopType m_showType;

	public Text m_alertTxt;

	public GameObject m_okBtn;

	public GameObject m_cancelBtn;

	public Text m_buttonTxt;

	public Text m_cancelbuttonTxt;

	public override void Init(CommonAlertData alertData)
	{
		AddEventHandler();
		m_AlertData = alertData;
		m_alertTxt.text = alertData.alertContent;
		m_buttonTxt.text = (string.IsNullOrEmpty(alertData.lableContent) ? "Ok" : alertData.lableContent);
		m_cancelbuttonTxt.text = (string.IsNullOrEmpty(alertData.cancelButtonText) ? "Cancel" : alertData.cancelButtonText);
		if (m_cancelBtn != null)
		{
			m_cancelBtn.SetActive(alertData.needCancelButton);
		}
	}

	private void AddEventHandler()
	{
		EventTriggerListener eventTriggerListener = EventTriggerListener.Get(m_okBtn);
		eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(OkBtnClickHandler));
		if (m_cancelBtn != null)
		{
			EventTriggerListener eventTriggerListener2 = EventTriggerListener.Get(m_cancelBtn);
			eventTriggerListener2.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener2.onClick, new EventTriggerListener.VoidDelegate(CancelBtnClickHandler));
		}
	}

	private void RemoveEventHandler()
	{
		EventTriggerListener eventTriggerListener = EventTriggerListener.Get(m_okBtn);
		eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(OkBtnClickHandler));
		if (m_cancelBtn != null)
		{
			EventTriggerListener eventTriggerListener2 = EventTriggerListener.Get(m_cancelBtn);
			eventTriggerListener2.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener2.onClick, new EventTriggerListener.VoidDelegate(CancelBtnClickHandler));
		}
	}

	private void OkBtnClickHandler(GameObject obj)
	{
		if (m_AlertData != null && m_AlertData.callBackFunc != null)
		{
			m_AlertData.callBackFunc();
		}
	}

	private void CancelBtnClickHandler(GameObject obj)
	{
		if (m_AlertData != null && m_AlertData.cancelCallBackFunc != null)
		{
			m_AlertData.cancelCallBackFunc();
		}
	}

	public override void Release()
	{
	}

	public override void Reset()
	{
		RemoveEventHandler();
	}

	public override CommonAlertData.AlertShopType GetAlertType()
	{
		return m_showType;
	}
}
