using RS2;
using UnityEngine;

public class ButtonInputLevelListController : MonoBehaviour
{
	public LevelItem[] buttons;

	public LevelSeriesController levelSeriesController;

	public bool horizon;

	private bool isPress;

	private int index;

	private void Start()
	{
		isPress = false;
	}

	private void Update()
	{
		if (CommonDialogUtil.Instance.onShow || MenuForm.State != MenuFormState.SelectLevel)
		{
			return;
		}
		if (!isPress)
		{
			if (InputService.KeyDown_Up)
			{
				isPress = true;
				ButtonSelect(-1);
			}
			else if (InputService.KeyDown_Down)
			{
				isPress = true;
				ButtonSelect(1);
			}
			else if (InputService.KeyDown_A)
			{
				isPress = true;
				levelSeriesController.EnterLevel();
			}
		}
		else if (!Input.anyKeyDown && !InputService.GetAnyAxis())
		{
			isPress = false;
		}
	}

	private void ButtonSelect(int value)
	{
		for (int i = 0; i < buttons.Length; i++)
		{
			if (buttons[i].m_isSelected)
			{
				index = i;
				break;
			}
		}
		index += value;
		index = Mathf.Clamp(index, 0, buttons.Length - 1);
		buttons[index].BtnSelect();
	}
}
