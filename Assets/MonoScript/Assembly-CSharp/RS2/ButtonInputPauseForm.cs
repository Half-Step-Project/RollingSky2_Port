using UnityEngine;

namespace RS2
{
	public class ButtonInputPauseForm : MonoBehaviour
	{
		private enum UIstate
		{
			Select,
			Continue,
			Exit
		}

		private PauseForm pauseForm;

		private bool isPress;

		private UIstate state;

		private void Start()
		{
			isPress = false;
			pauseForm = GetComponent<PauseForm>();
		}

		private void OnEnable()
		{
			state = UIstate.Select;
		}

		private void Update()
		{
			if (CommonDialogUtil.Instance.onShow)
			{
				return;
			}
			if (InputService.KeyDown_B)
			{
				if (state != 0)
				{
					return;
				}
				isPress = true;
				state = UIstate.Exit;
				pauseForm.ExitGame();
			}
			else if (InputService.KeyDown_A)
			{
				if (state != 0)
				{
					return;
				}
				isPress = true;
				state = UIstate.Continue;
				pauseForm.ContinueGame();
			}
			if (!Input.anyKeyDown && isPress)
			{
				isPress = false;
			}
		}
	}
}
