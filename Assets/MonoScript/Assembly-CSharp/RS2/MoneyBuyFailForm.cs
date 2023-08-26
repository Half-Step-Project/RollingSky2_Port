using Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace RS2
{
	public class MoneyBuyFailForm : UGUIForm
	{
		public enum FailType
		{
			UnNet,
			Fail
		}

		public Text m_alertTxt;

		public GameObject m_okBtn;

		public Text m_buttonTxt;

		public GameObject m_closeBtn;

		private FailType m_failType = FailType.Fail;

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			m_failType = (FailType)userData;
			EventTriggerListener.Get(m_okBtn).onClick = OkBtnClickHandler;
			EventTriggerListener.Get(m_closeBtn).onClick = CloseBtnClickHandler;
		}

		protected override void OnOpen(object userData)
		{
			switch (m_failType)
			{
			case FailType.UnNet:
				m_alertTxt.text = Mod.Localization.GetInfoById(40);
				m_buttonTxt.text = Mod.Localization.GetInfoById(41);
				break;
			case FailType.Fail:
				m_alertTxt.text = Mod.Localization.GetInfoById(56);
				m_buttonTxt.text = Mod.Localization.GetInfoById(41);
				break;
			}
			base.OnOpen(userData);
		}

		protected override void OnClose(object userData)
		{
			base.OnClose(userData);
		}

		private void OkBtnClickHandler(GameObject obj)
		{
			Mod.UI.CloseUIForm(UIFormId.MoneyBuyFailForm);
		}

		private void CloseBtnClickHandler(GameObject obj)
		{
			Mod.UI.CloseUIForm(UIFormId.MoneyBuyFailForm);
		}
	}
}
