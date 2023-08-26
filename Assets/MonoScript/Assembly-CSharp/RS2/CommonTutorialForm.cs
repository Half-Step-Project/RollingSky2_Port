using System;
using System.Collections;
using Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace RS2
{
	public class CommonTutorialForm : UGUIForm
	{
		public GameObject block;

		public Transform targetParent;

		public Transform fingerRoot;

		public GameObject back;

		public GameObject tutorialFiger;

		public Text tutorialTxt;

		public RectTransform content;

		private CommonTutorialData toturialData;

		private CommonTutorialStepData currentStep;

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			content.gameObject.SetActive(false);
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			AddEventListener();
			if (userData != null)
			{
				toturialData = userData as CommonTutorialData;
				if (toturialData.SetCount() > 0)
				{
					ExecuteTutorialStep();
				}
			}
			MonoSingleton<GameTools>.Instacne.EnableInput();
		}

		protected override void OnClose(object userData)
		{
			base.OnClose(userData);
			RemoveEventListener();
		}

		private void AddEventListener()
		{
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(tutorialFiger);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(tutorialFigerHandler));
		}

		private void tutorialFigerHandler(GameObject go)
		{
			if (currentStep != null && currentStep.stepAction != null)
			{
				currentStep.stepAction();
				if (currentStep.finishAction != null)
				{
					currentStep.finishAction();
				}
				ExecuteTutorialStep();
			}
		}

		private void RemoveEventListener()
		{
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(tutorialFiger);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(tutorialFigerHandler));
		}

		public void OnClickClose()
		{
			ExecuteTutorialStep();
		}

		private bool ExecuteTutorialStep()
		{
			bool result = false;
			currentStep = toturialData.GetStep();
			if (currentStep == null)
			{
				result = true;
				switch (toturialData.TutorialId)
				{
				case 1:
					PlayerDataModule.Instance.PlayerLevelTargetData.SetFinishTutorial(true);
					break;
				case 2:
					PlayerDataModule.Instance.PlayerRecordData.IsFinishedUsingAssetTutorial = 1;
					break;
				case 3:
					PlayerDataModule.Instance.PlayerRecordData.IsbuffShowFinishTutorial = 1;
					break;
				}
				Mod.UI.CloseUIForm(UIFormId.CommonTutorialForm);
			}
			else
			{
				tutorialTxt.text = currentStep.tutorialContent;
				(tutorialFiger.transform as RectTransform).position = new Vector3(currentStep.position.x, currentStep.position.y, 0f);
				tutorialFiger.transform.localPosition = new Vector3(tutorialFiger.transform.localPosition.x, tutorialFiger.transform.localPosition.y, 0f);
				Vector3 localPosition = fingerRoot.localPosition;
				fingerRoot.localPosition = new Vector3(localPosition.x + currentStep.posOffset.x, localPosition.y + currentStep.posOffset.y, 0f);
				if (currentStep.changeRect)
				{
					(tutorialFiger.transform as RectTransform).sizeDelta = new Vector2(currentStep.position.width, currentStep.position.height);
				}
				if (currentStep.delayShowFinger > 0f)
				{
					tutorialFiger.gameObject.SetActive(false);
					StartCoroutine(DelayShowFinger(currentStep.delayShowFinger));
				}
				if (currentStep.showContent)
				{
					content.gameObject.SetActive(true);
					float num = currentStep.position.x - (currentStep.position.width + content.sizeDelta.x) * 0.5f;
					float num2 = (720f - content.sizeDelta.x) * -0.5f;
					float num3 = (720f - content.sizeDelta.x) * 0.5f;
					if (num < num2)
					{
						num = num2;
					}
					if (num > num3)
					{
						num = num3;
					}
					float y = currentStep.position.y + (currentStep.position.height + content.sizeDelta.y) * 0.5f;
					content.anchoredPosition = new Vector2(num, y);
				}
				else
				{
					content.gameObject.SetActive(false);
				}
				if (currentStep.target != null)
				{
					Transform targetOriginalParent = currentStep.target.parent;
					currentStep.target.SetParent(targetParent);
					currentStep.finishAction = delegate
					{
						currentStep.target.SetParent(targetOriginalParent);
						currentStep.target.gameObject.SetActive(currentStep.finishTargetActive);
					};
				}
				block.SetActive(currentStep.needBlock);
				back.SetActive(currentStep.needBack);
			}
			return result;
		}

		private IEnumerator DelayShowFinger(float delayTime)
		{
			yield return new WaitForSeconds(delayTime);
			tutorialFiger.gameObject.SetActive(true);
		}
	}
}
