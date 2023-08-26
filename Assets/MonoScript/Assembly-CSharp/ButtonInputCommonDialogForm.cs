using System.Collections.Generic;
using UnityEngine;

public class ButtonInputCommonDialogForm : MonoBehaviour
{
	private bool isPress;

	private void Start()
	{
		isPress = false;
	}

	private void Update()
	{
		List<RectTransform> buttonBackgroundRects = base.gameObject.GetComponent<AutoSetButtonIcon>().m_ButtonBackgroundRects;
		buttonBackgroundRects[0].anchoredPosition = new Vector2(buttonBackgroundRects[0].sizeDelta.x / 2f + 25f, buttonBackgroundRects[0].anchoredPosition.y);
		buttonBackgroundRects[1].anchoredPosition = new Vector2((0f - buttonBackgroundRects[1].sizeDelta.x) / 2f - 25f, buttonBackgroundRects[1].anchoredPosition.y);
		if (InputService.KeyDown_A)
		{
			isPress = true;
		}
		else if (InputService.KeyDown_B)
		{
			isPress = true;
		}
		if (!Input.anyKeyDown && isPress)
		{
			isPress = false;
		}
	}
}
