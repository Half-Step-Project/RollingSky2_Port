using System;
using System.Collections;
using Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace RS2
{
	public class PauseForm : UGUIForm
	{
		public GameObject levelsListBtn;

		public GameObject resumeBtn;

		public Text timeTxt;

		public string m_prefabPath;

		public Image back;

		public Text pauseTxt;

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			Vector4 border = new Vector4(2f, 2f, 2f, 2f);
			back.sprite = ViewTools.CreateNewSpriteFromSource(back.sprite, border);
			back.type = Image.Type.Sliced;
			timeTxt.text = "";
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			resumeBtn.SetActive(true);
			timeTxt.text = "";
			pauseTxt.text = Mod.Localization.GetInfoById(73);
			bool active = MonoSingleton<GameTools>.Instacne.IsCanOperateBackToMenu();
			levelsListBtn.SetActive(active);
			AddEventHandler();
			Mod.Event.Fire(this, UIFormOpenEvent.Make(UIFormId.PauseForm));
			levelsListBtn.transform.Find("Text").GetComponent<CustomText>().supportRichText = false;
			string text = levelsListBtn.transform.Find("Text").GetComponent<CustomText>().text.Replace("<color=#000000BE>", "");
			text = text.Replace("</color>", "");
			levelsListBtn.transform.Find("Text").GetComponent<CustomText>().text = text;
			resumeBtn.transform.Find("Text").GetComponent<CustomText>().supportRichText = false;
			text = resumeBtn.transform.Find("Text").GetComponent<CustomText>().text.Replace("<color=#000000BE>", "");
			text = text.Replace("</color>", "");
			resumeBtn.transform.Find("Text").GetComponent<CustomText>().text = text;
			float[] array = new float[2]
			{
				levelsListBtn.transform.Find("Text").GetComponent<CustomText>().preferredWidth,
				resumeBtn.transform.Find("Text").GetComponent<CustomText>().preferredWidth
			};
		}

		protected override void OnClose(object userData)
		{
			base.OnClose(userData);
			RemoveEventHandler();
			timeTxt.text = "";
			StopAllCoroutines();
			Mod.Event.Fire(this, UIFormCloseEvent.Make(UIFormId.PauseForm));
		}

		private void RefreshOpenForm()
		{
			StopAllCoroutines();
			timeTxt.text = "";
			resumeBtn.SetActive(true);
			bool active = MonoSingleton<GameTools>.Instacne.IsCanOperateBackToMenu();
			levelsListBtn.SetActive(active);
		}

		private void AddEventHandler()
		{
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(levelsListBtn);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(LevelBtnClickHandler));
			EventTriggerListener eventTriggerListener2 = EventTriggerListener.Get(resumeBtn);
			eventTriggerListener2.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener2.onClick, new EventTriggerListener.VoidDelegate(ResumeBtnClickHandler));
			Mod.Event.Subscribe(EventArgs<UIMod.OpenSuccessEventArgs>.EventId, OnOpenCommonDialogFormSuccess);
		}

		private void RemoveEventHandler()
		{
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(levelsListBtn);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(LevelBtnClickHandler));
			EventTriggerListener eventTriggerListener2 = EventTriggerListener.Get(resumeBtn);
			eventTriggerListener2.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener2.onClick, new EventTriggerListener.VoidDelegate(ResumeBtnClickHandler));
			Mod.Event.Unsubscribe(EventArgs<UIMod.OpenSuccessEventArgs>.EventId, OnOpenCommonDialogFormSuccess);
		}

		private void ResumeBtnClickHandler(GameObject obj)
		{
			if (resumeBtn != null)
			{
				resumeBtn.SetActive(false);
			}
			if (levelsListBtn != null)
			{
				levelsListBtn.SetActive(false);
			}
			if (base.gameObject != null && base.gameObject.activeSelf)
			{
				StartCoroutine(CountTime());
			}
			if (pauseTxt != null)
			{
				pauseTxt.text = "";
			}
		}

		private IEnumerator CountTime()
		{
			float totalTime = 3f;
			while (totalTime > 0f)
			{
				totalTime -= Time.unscaledDeltaTime;
				if (totalTime < 0f)
				{
					totalTime = 0f;
				}
				timeTxt.text = totalTime.ToString("0");
				yield return null;
			}
			Mod.Event.FireNow(this, Mod.Reference.Acquire<GameResumeEventArgs>());
			Mod.UI.CloseUIForm(UIFormId.PauseForm);
		}

		private void LevelBtnClickHandler(GameObject obj)
		{
			Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule);
			Mod.UI.CloseUIForm(UIFormId.PauseForm);
			HomeForm.CurrentSeriesId = PlayerDataModule.Instance.LastEndterLevelData.SeriesId;
			Mod.Event.Fire(this, Mod.Reference.Acquire<GameExitEventArgs>());
		}

		private void OnOpenCommonDialogFormSuccess(object sender, Foundation.EventArgs e)
		{
			UIMod.OpenSuccessEventArgs openSuccessEventArgs = (UIMod.OpenSuccessEventArgs)e;
			if (openSuccessEventArgs != null && (bool)(openSuccessEventArgs.UIForm.Logic as CommonDialogForm))
			{
				RefreshOpenForm();
			}
		}

		public void ExitGame()
		{
			LevelBtnClickHandler(null);
		}

		public void ContinueGame()
		{
			ResumeBtnClickHandler(null);
		}

		private RectTransform SetNSButton(RectTransform buttonRoot, float maxWidth)
		{
			RectTransform component = buttonRoot.GetComponentInChildren<CustomText>().GetComponent<RectTransform>();
			float x = buttonRoot.transform.Find("InputInfo/Joystick").GetComponent<RectTransform>().sizeDelta.x;
			float num = (maxWidth + x) * 1.8f;
			float num2 = (num - (maxWidth + x)) / 3f;
			component.GetComponent<RectTransform>().sizeDelta = new Vector2(maxWidth, component.GetComponent<RectTransform>().sizeDelta.y);
			buttonRoot.sizeDelta = new Vector2(num, buttonRoot.sizeDelta.y);
			component.sizeDelta = new Vector2(maxWidth, 108f);
			component.anchorMin = new Vector2(0f, 0.5f);
			component.anchorMax = new Vector2(0f, 0.5f);
			component.pivot = new Vector2(0f, 0.5f);
			component.GetComponent<CustomText>().alignment = TextAnchor.MiddleLeft;
			component.anchoredPosition = new Vector2(x + num2 * 2f, component.anchoredPosition.y);
			buttonRoot.transform.Find("InputInfo/Joystick").GetComponent<RectTransform>().anchoredPosition = new Vector2(num2, 0f);
			return buttonRoot;
		}
	}
}
