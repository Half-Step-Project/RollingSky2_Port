using System;
using Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace RS2
{
	public class CommonHelpForm : UGUIForm
	{
		public GameObject back;

		public Text titleTxt;

		public Text contentTxt;

		public GameObject closeBtn;

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			AddEventListener();
			if (userData != null)
			{
				CommonHelpData commonHelpData = userData as CommonHelpData;
				if (commonHelpData != null)
				{
					titleTxt.text = commonHelpData.Title;
					contentTxt.text = commonHelpData.Content.Replace("\\n", "\n");
				}
			}
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
			EventTriggerListener eventTriggerListener2 = EventTriggerListener.Get(back);
			eventTriggerListener2.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener2.onClick, new EventTriggerListener.VoidDelegate(CloseHandler));
		}

		private void RemoveEventListener()
		{
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(closeBtn);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(CloseHandler));
			EventTriggerListener eventTriggerListener2 = EventTriggerListener.Get(back);
			eventTriggerListener2.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener2.onClick, new EventTriggerListener.VoidDelegate(CloseHandler));
		}

		private void CloseHandler(GameObject obj)
		{
			Mod.UI.CloseUIForm(UIFormId.CommonHelpForm);
		}
	}
}
