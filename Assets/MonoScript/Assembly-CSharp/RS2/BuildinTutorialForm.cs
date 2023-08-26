using System;
using System.Collections;
using Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace RS2
{
	public class BuildinTutorialForm : MonoBehaviour
	{
		public GameObject block;

		public Transform targetParent;

		public Transform fingerRoot;

		public GameObject back;

		public GameObject tutorialFiger;

		public Text tutorialTxt;

		public RectTransform content;

		public Transform eventZone;

		private CommonTutorialData toturialData;

		private CommonTutorialStepData currentStep;

		public static BuildinTutorialForm Form;

		private bool m_isEnd;

		private static GameObject m_tutorialForm;

		public Vector2 selftPos = Vector2.zero;

		private float selfScale = 1f;

		public void OnInit(object userData)
		{
			content.gameObject.SetActive(false);
		}

		public static bool Init()
		{
			if (m_tutorialForm == null)
			{
				GameObject gameObject = Resources.Load<GameObject>("Builtin/UI/BuildinTutorialForm");
				if (gameObject == null)
				{
					Log.Error("The BuildinTutorialForm asset load failed.");
					return false;
				}
				m_tutorialForm = UnityEngine.Object.Instantiate(gameObject);
				Transform obj = m_tutorialForm.transform;
				obj.position = Vector3.zero;
				obj.localScale = Vector3.one;
				obj.rotation = Quaternion.identity;
				m_tutorialForm.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
				m_tutorialForm.SetActive(false);
			}
			if (Form == null)
			{
				Form = m_tutorialForm.GetComponent<BuildinTutorialForm>();
				Form.OnOpen();
			}
			return true;
		}

		public void OnOpen()
		{
			selftPos = new Vector2((int)((float)Screen.width * 0.5f), (int)((float)Screen.height * 0.5f));
			float num = 720f;
			float num2 = 1280f;
			float num5 = num / num2;
			float num3 = (float)Screen.width * 1f;
			float num4 = (float)Screen.height * 1f;
			selfScale = Mathf.Min(num3 / num, num4 / num2);
			AddEventListener();
		}

		public void StartTutorial(CommonTutorialData data)
		{
			m_isEnd = false;
			toturialData = data;
			base.gameObject.SetActive(true);
			MonoSingleton<GameTools>.Instacne.EnableInput();
			if (toturialData.SetCount() > 0)
			{
				ExecuteTutorialStep();
			}
		}

		public void OnClose(object userData)
		{
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
			if (currentStep == null || !currentStep.disableBackClick)
			{
				if (currentStep != null && currentStep.finishAction != null)
				{
					currentStep.finishAction();
				}
				ExecuteTutorialStep();
			}
		}

		private bool ExecuteTutorialStep()
		{
			bool result = false;
			currentStep = toturialData.GetStep();
			Vector3 zero = Vector3.zero;
			if (currentStep == null)
			{
				result = true;
				EndTutorial();
			}
			else
			{
				switch (currentStep.stepType)
				{
				case TutorialStepType.NONE:
					EndTutorial();
					break;
				case TutorialStepType.ONLY_CONTENT:
					content.gameObject.SetActive(true);
					tutorialTxt.text = currentStep.tutorialContent;
					tutorialFiger.gameObject.SetActive(false);
					break;
				case TutorialStepType.CONTENT_AND_FINGER:
					content.gameObject.SetActive(true);
					tutorialTxt.text = currentStep.tutorialContent;
					if (currentStep.useFingerLocalPos)
					{
						tutorialFiger.transform.localPosition = currentStep.fingerLocalPos;
					}
					else if (!currentStep.useViewportAdjustPos)
					{
						Vector3 position2 = ComputerPos(new Vector3(currentStep.position.x, currentStep.position.y, 0f));
						(tutorialFiger.transform as RectTransform).position = position2;
						tutorialFiger.transform.localPosition = new Vector3(tutorialFiger.transform.localPosition.x, tutorialFiger.transform.localPosition.y, 0f);
					}
					else
					{
						Vector3 sourcePos3 = new Vector3(currentStep.position.x, currentStep.position.y, 0f);
						tutorialFiger.transform.localPosition = ComputerViewportAdjustPos(sourcePos3);
					}
					zero = fingerRoot.localPosition;
					fingerRoot.localPosition = new Vector3(zero.x + currentStep.posOffset.x, zero.y + currentStep.posOffset.y, 0f);
					if (currentStep.changeRect)
					{
						(tutorialFiger.transform as RectTransform).sizeDelta = new Vector2(currentStep.position.width, currentStep.position.height);
					}
					if (currentStep.delayShowFinger > 0f)
					{
						tutorialFiger.gameObject.SetActive(false);
						StartCoroutine(DelayShowFinger(currentStep.delayShowFinger));
					}
					else
					{
						tutorialFiger.gameObject.SetActive(true);
					}
					break;
				case TutorialStepType.ONLY_FINGER:
					content.gameObject.SetActive(false);
					if (currentStep.useFingerLocalPos)
					{
						tutorialFiger.transform.localPosition = currentStep.fingerLocalPos;
					}
					else if (!currentStep.useViewportAdjustPos)
					{
						Vector3 position = ComputerPos(new Vector3(currentStep.position.x, currentStep.position.y, 0f));
						(tutorialFiger.transform as RectTransform).position = position;
						tutorialFiger.transform.localPosition = new Vector3(tutorialFiger.transform.localPosition.x, tutorialFiger.transform.localPosition.y, 0f);
					}
					else
					{
						Vector3 sourcePos2 = new Vector3(currentStep.position.x, currentStep.position.y, 0f);
						tutorialFiger.transform.localPosition = ComputerViewportAdjustPos(sourcePos2);
					}
					zero = fingerRoot.localPosition;
					fingerRoot.localPosition = new Vector3(zero.x + currentStep.posOffset.x, zero.y + currentStep.posOffset.y, 0f);
					if (currentStep.changeRect)
					{
						(tutorialFiger.transform as RectTransform).sizeDelta = new Vector2(currentStep.position.width, currentStep.position.height);
					}
					if (currentStep.delayShowFinger > 0f)
					{
						tutorialFiger.gameObject.SetActive(false);
						StartCoroutine(DelayShowFinger(currentStep.delayShowFinger));
					}
					else
					{
						tutorialFiger.gameObject.SetActive(true);
					}
					break;
				}
				if (currentStep.target != null)
				{
					Vector2 sizeDelta = (currentStep.target as RectTransform).sizeDelta;
					sizeDelta = new Vector2(sizeDelta.x * currentStep.target.localScale.x, sizeDelta.y * currentStep.target.localScale.y);
					Vector2 sizeDelta2 = (eventZone as RectTransform).sizeDelta;
					Vector3 localScale = new Vector3(sizeDelta.x / sizeDelta2.x, sizeDelta.y / sizeDelta2.y, 1f) * 1.2f;
					eventZone.localScale = localScale;
					Transform targetOriginalParent = currentStep.target.parent;
					Vector3 sourcePos = currentStep.target.position;
					Vector3 sourceScale = currentStep.target.localScale;
					currentStep.target.SetParent(targetParent);
					if (!currentStep.useViewportAdjustPos)
					{
						currentStep.target.position = ComputerPos(sourcePos);
					}
					else
					{
						currentStep.target.localPosition = ComputerViewportAdjustPos(sourcePos);
					}
					currentStep.target.localScale = sourceScale;
					currentStep.finishAction = delegate
					{
						currentStep.target.SetParent(targetOriginalParent);
						currentStep.target.transform.localScale = sourceScale;
						currentStep.target.position = sourcePos;
						currentStep.target.gameObject.SetActive(currentStep.finishTargetActive);
					};
				}
				block.SetActive(currentStep.needBlock);
				block.GetComponent<Button>().enabled = currentStep.blockBtnEnable;
				back.SetActive(currentStep.needBack);
			}
			return result;
		}

		private Vector3 ComputerPos(Vector3 soucePos)
		{
			float num = Mod.UI.GetUIGroup("Forth").transform.localScale.x / selfScale;
			return new Vector3(soucePos.x / num + selftPos.x, soucePos.y / num + selftPos.y, 0f);
		}

		private Vector3 ComputerViewportAdjustPos(Vector3 sourcePos)
		{
			Vector3 vector = Mod.UI.UICamera.WorldToViewportPoint(sourcePos);
			Vector2 sizeDelta = GetComponent<RectTransform>().sizeDelta;
			return new Vector3(sizeDelta.x * (vector.x - 0.5f), sizeDelta.y * (vector.y - 0.5f), 0f);
		}

		private Vector3 ComputerScale(Vector3 souceScale)
		{
			float x = Mod.UI.GetUIGroup("Forth").transform.localScale.x;
			return new Vector3(souceScale.x / x * selfScale, souceScale.y / x * selfScale, 1f);
		}

		private IEnumerator DelayShowFinger(float delayTime)
		{
			yield return new WaitForSeconds(delayTime);
			tutorialFiger.gameObject.SetActive(true);
		}

		private void EndTutorial()
		{
			m_isEnd = true;
			base.gameObject.SetActive(false);
			currentStep = null;
			if (toturialData.endAction != null)
			{
				toturialData.endAction();
			}
		}
	}
}
