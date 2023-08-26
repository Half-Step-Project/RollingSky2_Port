using System;
using System.Collections;
using DG.Tweening;
using Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace RS2
{
	public class NewFeatureForm : UGUIForm
	{
		public class Data
		{
			public System.Action closeCallback;
		}

		public GameObject back;

		public Text desc;

		public Text click;

		public GameObject effectRoot;

		private Data data;

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			desc.text = Mod.Localization.GetInfoById(79);
			click.text = Mod.Localization.GetInfoById(78);
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			base.gameObject.SetActive(true);
			data = (Data)userData;
			click.transform.DOScale(1.1f, 0.4f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
			StartCoroutine(ShowEffect());
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(back);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(BackClickHandler));
		}

		protected override void OnClose(object userData)
		{
			base.OnClose(userData);
			click.transform.DOKill();
			effectRoot.SetActive(false);
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(back);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(BackClickHandler));
			if (data != null && data.closeCallback != null)
			{
				data.closeCallback();
			}
		}

		private IEnumerator ShowEffect()
		{
			yield return new WaitForSeconds(0.5f);
			effectRoot.SetActive(true);
		}

		private void BackClickHandler(GameObject obj)
		{
			Mod.UI.CloseUIForm(UIFormId.NewFeatureForm);
		}
	}
}
