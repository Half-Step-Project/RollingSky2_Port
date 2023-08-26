using Foundation;
using RS2;
using UnityEngine;

public class ButtonInputHomeForm : MonoBehaviour
{
	public GameObject[] buttons;

	private bool isPress;

	private HomeForm homeForm;

	private int index;

	private void Start()
	{
		index = 0;
		isPress = false;
		homeForm = GetComponent<HomeForm>();
	}

	private void Update()
	{
		if (InputService.keyDown_Option && !CommonDialogUtil.Instance.IsOpenDialog())
		{
			CommonDialogUtil.Instance.ShowDialog("Whether to quit the game?", CommonDialogUtil.ShowType.YesNo, "Yes", "No", Mod.Shutdown, delegate
			{
				CommonDialogUtil.Instance.CloseDialog();
			});
		}
		if (MenuForm.State != MenuFormState.SelectLevel)
		{
			return;
		}
		if (!isPress)
		{
			if (InputService.KeyDown_L)
			{
				isPress = true;
				LeftMoveHandle();
			}
			else if (InputService.KeyDown_R)
			{
				isPress = true;
				RightMoveHandle();
			}
		}
		else if (!Input.anyKeyDown)
		{
			isPress = false;
		}
	}

	private void LeftMoveHandle()
	{
		homeForm.LeftMoveHandle();
	}

	private void RightMoveHandle()
	{
		homeForm.RightMoveHandle();
	}

	private void CloseCupInfo()
	{
		homeForm.OnClickCupTips(null);
	}
}
