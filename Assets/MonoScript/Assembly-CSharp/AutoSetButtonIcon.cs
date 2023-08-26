using System.Collections.Generic;
using System.Linq;
using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AutoSetButtonIcon : MonoBehaviour
{
	public float m_ControllerWidthMagnification = 1.3f;

	public float m_WidthMagnification = 1.5f;

	public List<RectTransform> m_ButtonBackgroundRects;

	public List<CustomText> m_ButtonTexts;

	public List<Image> m_Icons;

	public bool m_AsWide = true;

	public TextAnchor m_TextAnchor = TextAnchor.MiddleCenter;

	public UnityEvent OnButtonSetEnd;

	public float IconPosX = 20f;

	private List<RectTransform> m_TextRects;

	private List<RectTransform> m_IconRects;

	private bool m_nowControllerType;

	private void Start()
	{
		m_nowControllerType = InputService.UseController;
		Mod.Event.Subscribe(EventArgs<ChangeLanguageArgs>.EventId, SetButton);
		Mod.Event.Subscribe(EventArgs<ChangeControllerArgs>.EventId, ChangeControllerType);
		Init();
		SetButton(null, null);
	}

	private void ChangeControllerType(object sender, EventArgs args)
	{
		if (m_nowControllerType != InputService.UseController)
		{
			m_nowControllerType = InputService.UseController;
			SetButton(null, null);
		}
	}

	private void OnDestroy()
	{
		Mod.Event.Unsubscribe(EventArgs<ChangeLanguageArgs>.EventId, SetButton);
		Mod.Event.Unsubscribe(EventArgs<ChangeControllerArgs>.EventId, ChangeControllerType);
	}

	private void Init()
	{
		if (m_ButtonTexts.Count > 0)
		{
			m_TextRects = new List<RectTransform>();
			foreach (CustomText buttonText in m_ButtonTexts)
			{
				m_TextRects.Add(buttonText.GetComponent<RectTransform>());
			}
		}
		if (m_Icons.Count <= 0)
		{
			return;
		}
		m_IconRects = new List<RectTransform>();
		foreach (Image icon in m_Icons)
		{
			m_IconRects.Add(icon.GetComponent<RectTransform>());
		}
	}

	public void SetButton(object sender, EventArgs args)
	{
		Init();
		if (m_ButtonTexts.Count > 0)
		{
			float num = SetAllTextWidth().Max();
			SetAllTextHeight().Max();
			float x = m_IconRects[0].sizeDelta.x;
			if (m_nowControllerType)
			{
				float num2 = (x + num) * m_ControllerWidthMagnification;
				float num3 = (num2 - (x + num + IconPosX * 2f)) / 2f;
				foreach (RectTransform buttonBackgroundRect in m_ButtonBackgroundRects)
				{
					buttonBackgroundRect.sizeDelta = new Vector2(num2 + 65f, 130f);
				}
				foreach (RectTransform iconRect in m_IconRects)
				{
					iconRect.gameObject.SetActive(true);
					iconRect.sizeDelta = new Vector2(80f, 80f);
					iconRect.anchoredPosition = new Vector2(IconPosX, iconRect.anchoredPosition.y);
				}
				foreach (RectTransform textRect in m_TextRects)
				{
					textRect.GetComponent<CustomText>().alignment = m_TextAnchor;
					textRect.GetComponent<CustomText>().fontSize = 60;
					textRect.anchorMin = new Vector2(0f, 0.5f);
					textRect.anchorMax = new Vector2(0f, 0.5f);
					textRect.pivot = new Vector2(0f, 0.5f);
					textRect.sizeDelta = new Vector2(num * 1.1f, 130f);
					textRect.anchoredPosition = new Vector2(IconPosX * 2f + x + (num3 - IconPosX), textRect.anchoredPosition.y);
				}
			}
			else
			{
				foreach (Image icon in m_Icons)
				{
					icon.gameObject.SetActive(false);
				}
				float num2 = num * m_WidthMagnification;
				float num3 = (num2 - num) / 2f;
				foreach (RectTransform buttonBackgroundRect2 in m_ButtonBackgroundRects)
				{
					buttonBackgroundRect2.sizeDelta = new Vector2(num2, 130f);
				}
				foreach (RectTransform textRect2 in m_TextRects)
				{
					textRect2.GetComponent<CustomText>().alignment = m_TextAnchor;
					textRect2.GetComponent<CustomText>().fontSize = 60;
					textRect2.anchorMin = new Vector2(0f, 0.5f);
					textRect2.anchorMax = new Vector2(0f, 0.5f);
					textRect2.pivot = new Vector2(0f, 0.5f);
					textRect2.sizeDelta = new Vector2(num, 130f);
					textRect2.anchoredPosition = new Vector2(num3, textRect2.anchoredPosition.y);
				}
			}
			if (OnButtonSetEnd != null)
			{
				OnButtonSetEnd.Invoke();
			}
		}
		else
		{
			Debug.LogError(string.Format("Error for AutoSetButtonIcon : {0} Is not set TextList", base.name));
		}
	}

	private float[] SetAllTextWidth()
	{
		float[] array = new float[m_ButtonTexts.Count];
		for (int i = 0; i < m_ButtonTexts.Count; i++)
		{
			array[i] = m_ButtonTexts[i].preferredWidth;
		}
		return array;
	}

	private float[] SetAllTextHeight()
	{
		float[] array = new float[m_ButtonTexts.Count];
		for (int i = 0; i < m_ButtonTexts.Count; i++)
		{
			array[i] = m_ButtonTexts[i].preferredHeight;
		}
		return array;
	}
}
