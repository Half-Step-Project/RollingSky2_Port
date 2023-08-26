using System.Collections.Generic;
using RisingWin.Library;
using RS2;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonInputLanguageForm : MonoBehaviour
{
	public LanguageForm m_languageForm;

	public List<Button> m_Btns_1;

	public List<Button> m_Btns_2;

	public List<Button> m_Btns_3;

	public List<Button> m_Btns_4;

	private Vector2 nowButtonPosition;

	private float delayTime;

	private void Start()
	{
		EventSystem.current.SetSelectedGameObject(null);
		m_languageForm = base.gameObject.GetComponent<LanguageForm>();
	}

	private void OnEnable()
	{
		SetNowButtonPosition();
	}

	public void OnButtonDown(Button button)
	{
		ButtonTool.Select(button);
		SetNowButtonPosition();
		UseVector2SelectTargetButton(ChangeNowButtonPosition(new Vector2(nowButtonPosition.x, nowButtonPosition.y)));
		if (delayTime <= 0f)
		{
			delayTime = 0.2f;
			ButtonTool.CurrentSelectedButton.onClick.Invoke();
		}
	}

	private void SetNowButtonPosition()
	{
		Button currentSelectedButton = ButtonTool.CurrentSelectedButton;
		if (!(currentSelectedButton != null))
		{
			return;
		}
		for (int i = 0; i < m_Btns_1.Count; i++)
		{
			if (m_Btns_1[i].Equals(currentSelectedButton))
			{
				nowButtonPosition = new Vector2(0f, i);
			}
		}
		for (int j = 0; j < m_Btns_2.Count; j++)
		{
			if (m_Btns_2[j].Equals(currentSelectedButton))
			{
				nowButtonPosition = new Vector2(1f, j);
			}
		}
		for (int k = 0; k < m_Btns_3.Count; k++)
		{
			if (m_Btns_3[k].Equals(currentSelectedButton))
			{
				nowButtonPosition = new Vector2(2f, k);
			}
		}
		for (int l = 0; l < m_Btns_4.Count; l++)
		{
			if (m_Btns_4[l].Equals(currentSelectedButton))
			{
				nowButtonPosition = new Vector2(3f, l);
			}
		}
	}

	private Vector2 ChangeNowButtonPosition(Vector2 v2)
	{
		if (v2.x < 0f)
		{
			v2.x = 0f;
		}
		if (v2.x > 3f)
		{
			v2.x = 3f;
		}
		if (v2.x == 0f)
		{
			if (v2.y < 0f)
			{
				v2.y = 0f;
			}
			if (v2.y >= (float)m_Btns_1.Count)
			{
				v2.y = m_Btns_1.Count - 1;
			}
		}
		if (v2.x == 1f)
		{
			if (v2.y < 0f)
			{
				v2.y = 0f;
			}
			if (v2.y >= (float)m_Btns_2.Count)
			{
				v2.y = m_Btns_2.Count - 1;
			}
		}
		if (v2.x == 2f)
		{
			if (v2.y < 0f)
			{
				v2.y = 0f;
			}
			if (v2.y >= (float)m_Btns_3.Count)
			{
				v2.y = m_Btns_3.Count - 1;
			}
		}
		if (v2.x == 3f)
		{
			if (v2.y < 0f)
			{
				v2.y = 0f;
			}
			if (v2.y >= (float)m_Btns_4.Count)
			{
				v2.y = m_Btns_4.Count - 1;
			}
		}
		nowButtonPosition = v2;
		return nowButtonPosition;
	}

	private void UseVector2SelectTargetButton(Vector2 v2)
	{
		Button button = ((v2.x == 0f) ? m_Btns_1[(int)v2.y] : ((v2.x == 1f) ? m_Btns_2[(int)v2.y] : ((v2.x != 2f) ? m_Btns_4[(int)v2.y] : m_Btns_3[(int)v2.y])));
		ButtonTool.Select(button);
	}

	private void Update()
	{
		if (delayTime <= 0f)
		{
			if (InputService.KeyDown_Up)
			{
				Debug.Log("KeyDown_Up");
				UseVector2SelectTargetButton(ChangeNowButtonPosition(new Vector2(nowButtonPosition.x, nowButtonPosition.y - 1f)));
				ButtonTool.CurrentSelectedButton.onClick.Invoke();
				delayTime = 0.2f;
			}
			if (InputService.KeyDown_Down)
			{
				Debug.Log("KeyDown_Down");
				UseVector2SelectTargetButton(ChangeNowButtonPosition(new Vector2(nowButtonPosition.x, nowButtonPosition.y + 1f)));
				ButtonTool.CurrentSelectedButton.onClick.Invoke();
				delayTime = 0.2f;
			}
			if (InputService.KeyDown_Left)
			{
				Debug.Log("KeyDown_Left");
				UseVector2SelectTargetButton(ChangeNowButtonPosition(new Vector2(nowButtonPosition.x - 1f, nowButtonPosition.y)));
				ButtonTool.CurrentSelectedButton.onClick.Invoke();
				delayTime = 0.2f;
			}
			if (InputService.KeyDown_Right)
			{
				Debug.Log("KeyDown_Right");
				UseVector2SelectTargetButton(ChangeNowButtonPosition(new Vector2(nowButtonPosition.x + 1f, nowButtonPosition.y)));
				ButtonTool.CurrentSelectedButton.onClick.Invoke();
				delayTime = 0.2f;
			}
		}
		else
		{
			delayTime -= Time.deltaTime;
		}
		if (InputService.KeyDown_B || InputService.KeyDown(KeyCode.Escape))
		{
			m_languageForm.CloseLanguageForm();
		}
	}
}
