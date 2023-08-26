using System;
using Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace RS2
{
	public class BuyOutRebirthIntroductionForm : UGUIForm
	{
		public Text m_title;

		public GameObject m_closeButton;

		public Text m_message;

		public Text m_alreadyMessage;

		public GameObject m_okButton;

		public Text m_okButtonLabel;

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(m_closeButton);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(OnClickCloseButton));
			EventTriggerListener eventTriggerListener2 = EventTriggerListener.Get(m_okButton);
			eventTriggerListener2.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener2.onClick, new EventTriggerListener.VoidDelegate(OnClickOkButton));
			m_message.text = Mod.Localization.GetInfoById(76).Replace("\\n", "\n");
			m_alreadyMessage.text = Mod.Localization.GetInfoById(126);
			m_okButtonLabel.text = Mod.Localization.GetInfoById(41);
		}

		protected override void OnClose(object userData)
		{
			base.OnClose(userData);
		}

		protected override void OnReveal()
		{
			base.OnReveal();
		}

		private void OnClickCloseButton(GameObject obj)
		{
			Mod.UI.CloseUIForm(UIFormId.BuyOutRebirthIntroductionForm);
		}

		private void OnClickOkButton(GameObject obj)
		{
			Mod.UI.CloseUIForm(UIFormId.BuyOutRebirthIntroductionForm);
		}
	}
}
