using System;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class PlayerLevelUpgradeForm : UGUIForm
	{
		public GameObject closeBtn;

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			AddEventHandler();
		}

		protected override void OnClose(object userData)
		{
			base.OnClose(userData);
			RemoveEventHandler();
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
			Mod.UI.CloseUIForm(UIFormId.CommonAlertForm);
		}

		protected override void OnCover()
		{
			base.OnCover();
			base.gameObject.SetActive(false);
		}

		protected override void OnReveal()
		{
			base.OnReveal();
			base.gameObject.SetActive(true);
		}

		protected override void OnUnload()
		{
			base.OnUnload();
		}
	}
}
