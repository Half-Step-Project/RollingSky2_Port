using UnityEngine;

namespace RS2
{
	public class ButtonInputResultForm : MonoBehaviour
	{
		private ResultForm resultForm;

		private bool isPress;

		private void Start()
		{
			isPress = false;
			resultForm = GetComponent<ResultForm>();
		}

		private void Update()
		{
			if (!CommonDialogUtil.Instance.onShow)
			{
				if (InputService.KeyDown_B)
				{
					isPress = true;
					resultForm.OnMenuBackBtnClick();
				}
				else if (InputService.KeyDown_A)
				{
					isPress = true;
					resultForm.OnReplayTwoBtnClick();
				}
				if (!Input.anyKeyDown && isPress)
				{
					isPress = false;
				}
			}
		}
	}
}
