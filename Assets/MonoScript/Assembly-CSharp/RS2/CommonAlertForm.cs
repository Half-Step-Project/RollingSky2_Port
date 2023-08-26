using System;
using System.Collections.Generic;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class CommonAlertForm : UGUIForm
	{
		public List<CommonAlertItemController> m_itemController = new List<CommonAlertItemController>();

		public GameObject closeBtn;

		private CommonAlertData alertData;

		private CommonAlertItemController m_currentItem;

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			alertData = (CommonAlertData)userData;
			for (int i = 0; i < m_itemController.Count; i++)
			{
				if (m_itemController[i].GetAlertType().Equals(alertData.showType))
				{
					m_itemController[i].gameObject.SetActive(true);
					m_itemController[i].Init(alertData);
					m_currentItem = m_itemController[i];
				}
				else
				{
					m_itemController[i].gameObject.SetActive(false);
				}
			}
			AddEventHandler();
		}

		protected override void OnClose(object userData)
		{
			base.OnClose(userData);
			RemoveEventHandler();
			if (m_currentItem != null)
			{
				m_currentItem.Reset();
			}
			m_currentItem = null;
			alertData = null;
		}

		private void AddEventHandler()
		{
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(closeBtn);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(CloseBtnClickHandler));
		}

		private void RemoveEventHandler()
		{
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(closeBtn);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(CloseBtnClickHandler));
		}

		private void CloseBtnClickHandler(GameObject obj)
		{
			if (alertData.closeCallBackFunc != null)
			{
				alertData.closeCallBackFunc();
			}
			Mod.UI.CloseUIForm(UIFormId.CommonAlertForm);
		}

		protected override void OnCover()
		{
			base.OnCover();
			if (alertData.isCoverHide)
			{
				base.gameObject.SetActive(false);
			}
		}

		protected override void OnReveal()
		{
			base.OnReveal();
			base.gameObject.SetActive(true);
		}

		protected override void OnUnload()
		{
			base.OnUnload();
			for (int i = 0; i < m_itemController.Count; i++)
			{
				m_itemController[i].Release();
			}
		}
	}
}
