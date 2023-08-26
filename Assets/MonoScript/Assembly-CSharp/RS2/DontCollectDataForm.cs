using System;
using Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace RS2
{
	public class DontCollectDataForm : UGUIForm
	{
		public Text titleTxt;

		public Text targetContentTxt;

		public GameObject closeBtn;

		public GameObject restartBtn;

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			AddEventListener();
		}

		protected override void OnClose(object userData)
		{
			base.OnClose(userData);
			RemoveEventListener();
		}

		private void AddEventListener()
		{
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(closeBtn);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(CloseHandler));
			EventTriggerListener eventTriggerListener2 = EventTriggerListener.Get(restartBtn);
			eventTriggerListener2.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener2.onClick, new EventTriggerListener.VoidDelegate(RestartHandler));
		}

		private void RemoveEventListener()
		{
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(closeBtn);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(CloseHandler));
			EventTriggerListener eventTriggerListener2 = EventTriggerListener.Get(restartBtn);
			eventTriggerListener2.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener2.onClick, new EventTriggerListener.VoidDelegate(RestartHandler));
		}

		private void CloseHandler(GameObject obj)
		{
			Mod.UI.CloseUIForm(UIFormId.DontCollectDataForm);
		}

		private void RestartHandler(GameObject obj)
		{
			CMPGDPRUtils.Instance.setGDPRAgreedAdStayInformed(false);
			Mod.UI.CloseUIForm(UIFormId.DontCollectDataForm);
		}
	}
}
