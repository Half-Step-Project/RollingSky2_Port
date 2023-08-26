using Foundation;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace RS2
{
	public sealed class CommonButton : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
	{
		private const float FadeTime = 0.3f;

		private const float OnHoverAlpha = 0.7f;

		private const float OnClickAlpha = 0.6f;

		[SerializeField]
		private UnityEvent m_OnHover;

		[SerializeField]
		private UnityEvent m_OnClick;

		private CanvasGroup m_CanvasGroup;

		private void Awake()
		{
			m_CanvasGroup = base.gameObject.GetOrAddComponent<CanvasGroup>();
		}

		private void OnDisable()
		{
			m_CanvasGroup.alpha = 1f;
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			if (eventData.button == PointerEventData.InputButton.Left)
			{
				StopAllCoroutines();
				StartCoroutine(m_CanvasGroup.FadeToAlpha(0.7f, 0.3f));
				m_OnHover.Invoke();
			}
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			if (eventData.button == PointerEventData.InputButton.Left)
			{
				StopAllCoroutines();
				StartCoroutine(m_CanvasGroup.FadeToAlpha(1f, 0.3f));
			}
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			if (eventData.button == PointerEventData.InputButton.Left)
			{
				m_CanvasGroup.alpha = 0.6f;
				m_OnClick.Invoke();
			}
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			if (eventData.button == PointerEventData.InputButton.Left)
			{
				m_CanvasGroup.alpha = 0.7f;
			}
		}
	}
}
