using DG.Tweening;
using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TipStartController : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
{
	public Text m_tipContent;

	public Image m_upMask;

	public Image m_Cool;

	public GameObject m_Arrow;

	private ITutorialHandOperate m_tutorialManager;

	public Animation m_arrowAnimation;

	private bool m_isPressDown;

	public float m_MaskFade = 1f;

	private GameObject m_tutorialUI;

	public bool PressDown
	{
		get
		{
			return m_isPressDown;
		}
	}

	public void Init(ITutorialHandOperate tutorialManager)
	{
		m_tutorialManager = tutorialManager;
		m_Arrow.SetActive(false);
		TutorialVideoController[] array = Resources.FindObjectsOfTypeAll<TutorialVideoController>();
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].gameObject.name == "Tutorial")
			{
				m_tutorialUI = array[i].gameObject;
				break;
			}
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		m_isPressDown = true;
		if (m_tutorialManager != null)
		{
			m_tutorialManager.OnHanderDown();
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		m_isPressDown = false;
		if (m_tutorialManager != null)
		{
			m_tutorialManager.OnHandUp();
		}
	}

	public void Open()
	{
		Mod.Event.Subscribe(EventArgs<GuideUiEventArgs>.EventId, GuideUIEvent);
	}

	public void Reset()
	{
		Mod.Event.Unsubscribe(EventArgs<GuideUiEventArgs>.EventId, GuideUIEvent);
	}

	private void ShowMask()
	{
		m_upMask.DOFade(m_MaskFade, 0.3f);
	}

	private void HideMask()
	{
		m_upMask.DOFade(0f, 0.3f);
	}

	private void ShowCool()
	{
		m_Cool.DOFade(1f, 0.5f);
		m_Cool.transform.DOScale(1.6f, 0.5f);
	}

	private void HideCool()
	{
		m_Cool.DOFade(0f, 0.5f);
		m_Cool.transform.DOScale(0.1f, 0.5f);
	}

	private void ShowLeftArrow()
	{
		m_arrowAnimation.Stop();
		m_arrowAnimation["anim_XS_Arrow"].time = 0f;
		m_arrowAnimation.Play();
		m_Arrow.transform.localScale = Vector3.one;
		m_Arrow.SetActive(true);
	}

	private void ShowRightArrow()
	{
		m_arrowAnimation.Stop();
		m_arrowAnimation["anim_XS_Arrow"].time = 0f;
		m_arrowAnimation.Play();
		m_Arrow.transform.localScale = new Vector3(-1f, 1f, 1f);
		m_Arrow.SetActive(true);
	}

	private void HideArrow()
	{
		m_Arrow.SetActive(false);
	}

	private void GuideUIEvent(object sender, EventArgs e)
	{
		GuideUiEventArgs guideUiEventArgs = e as GuideUiEventArgs;
		if (guideUiEventArgs == null)
		{
			return;
		}
		switch (guideUiEventArgs.GuideUiIndex)
		{
		case 1:
			ShowMask();
			HideCool();
			ShowLeftArrow();
			m_tipContent.text = Mod.Localization.GetInfoById(110);
			break;
		case 2:
			ShowMask();
			HideCool();
			ShowRightArrow();
			m_tipContent.text = Mod.Localization.GetInfoById(111);
			break;
		case 3:
			ShowMask();
			ShowCool();
			HideArrow();
			m_tipContent.text = "";
			break;
		case 6:
			ShowMask();
			HideCool();
			ShowLeftArrow();
			m_tipContent.text = Mod.Localization.GetInfoById(112);
			break;
		case 7:
			ShowMask();
			HideCool();
			ShowRightArrow();
			m_tipContent.text = Mod.Localization.GetInfoById(113);
			break;
		case 4:
			if (m_tutorialManager != null)
			{
				m_tutorialManager.EndHand();
			}
			break;
		case 5:
			HideMask();
			HideCool();
			HideArrow();
			m_tipContent.text = "";
			break;
		case 8:
			HideMask();
			HideCool();
			HideArrow();
			m_tipContent.text = "";
			if (!RecordOriginRebirthManager.m_isCanRecord)
			{
				if (m_tutorialUI != null)
				{
					m_tutorialUI.SetActive(true);
				}
				if (GameController.Instance.M_gameState != GameController.GAMESTATE.Pause)
				{
					Mod.Event.Fire(this, Mod.Reference.Acquire<GamePauseEventArgs>().Initialize(false));
				}
				else
				{
					Mod.UI.CloseUIForm(UIFormId.PauseForm);
				}
			}
			break;
		}
	}
}
