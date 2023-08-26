using Foundation;
using RS2;
using UnityEngine;

public class ButtonInputManager : MonoBehaviour
{
	public GameObject[] buttons;

	public bool horizon;

	public GameObject[] backInfo;

	private bool isPress;

	private MenuForm menuForm;

	private void Start()
	{
		isPress = false;
		menuForm = GetComponent<MenuForm>();
	}

	private void OnEnable()
	{
		Mod.Event.Subscribe(EventArgs<ChangeInputArgs>.EventId, ChangeInputInfo);
	}

	private void OnDisable()
	{
		Mod.Event.Unsubscribe(EventArgs<ChangeInputArgs>.EventId, ChangeInputInfo);
	}

	private void Update()
	{
		if (CommonDialogUtil.Instance.onShow || MenuForm.isLoading)
		{
			return;
		}
		if (MenuForm.State == MenuFormState.SelectLevel)
		{
			if (InputService.KeyDown_B)
			{
				isPress = true;
				BackMenu();
			}
		}
		else if (MenuForm.State == MenuFormState.Education)
		{
			if (InputService.KeyDown_A)
			{
				isPress = true;
				OnButtonEnter();
			}
			if (InputService.KeyDown_Language)
			{
				isPress = true;
				OnLanguageButtonEnter();
			}
		}
		if (!Input.anyKeyDown && isPress)
		{
			isPress = false;
		}
	}

	public void OnButtonEnter()
	{
		menuForm.Temp1(null);
	}

	public void OnLanguageButtonEnter()
	{
		menuForm.Temp4(null);
	}

	private void BackMenu()
	{
		menuForm.Temp3(null);
	}

	private void ChangeInputInfo(object sender, EventArgs e)
	{
		ChangeInputArgs changeInputArgs = e as ChangeInputArgs;
		for (int i = 0; i < backInfo.Length; i++)
		{
			if (changeInputArgs.inputState == (InputSystem.InputState)i)
			{
				backInfo[i].SetActive(true);
			}
			else
			{
				backInfo[i].SetActive(false);
			}
		}
	}

	private void NSInputInfo()
	{
		backInfo[0].SetActive(false);
		backInfo[1].SetActive(false);
		backInfo[2].SetActive(true);
	}
}
