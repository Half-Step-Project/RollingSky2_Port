using System.Collections.Generic;
using RisingWin.Library;
using UnityEngine;

public class JoystickInput : GeneralInput
{
	private List<JoystickController> listJoystickController = new List<JoystickController>();

	private static readonly KeyCode[,] keyCode = new KeyCode[4, 6]
	{
		{
			KeyCode.JoystickButton0,
			KeyCode.JoystickButton1,
			KeyCode.JoystickButton2,
			KeyCode.JoystickButton3,
			KeyCode.JoystickButton4,
			KeyCode.JoystickButton5
		},
		{
			KeyCode.Joystick1Button0,
			KeyCode.Joystick1Button1,
			KeyCode.Joystick1Button2,
			KeyCode.Joystick1Button3,
			KeyCode.Joystick1Button4,
			KeyCode.Joystick1Button5
		},
		{
			KeyCode.Joystick2Button0,
			KeyCode.Joystick2Button1,
			KeyCode.Joystick2Button2,
			KeyCode.Joystick2Button3,
			KeyCode.Joystick2Button4,
			KeyCode.Joystick2Button5
		},
		{
			KeyCode.Joystick3Button0,
			KeyCode.Joystick3Button1,
			KeyCode.Joystick3Button2,
			KeyCode.Joystick3Button3,
			KeyCode.Joystick3Button4,
			KeyCode.Joystick3Button5
		}
	};

	public JoystickInput()
	{
		Init(maxPlayer);
	}

	public void Init(int maxPlayer)
	{
		base.maxPlayer = maxPlayer;
		for (int i = 0; i < maxPlayer; i++)
		{
			JoystickController joystickController = new JoystickController();
			joystickController.Init(i, keyCode[i, 0], keyCode[i, 1], keyCode[i, 2], keyCode[i, 3], keyCode[i, 4], keyCode[i, 5]);
			listJoystickController.Add(joystickController);
		}
	}

	public override float GetAxis(AxisType axis, int index, AxisButtonTriggerType buttonTriggerType)
	{
		if (1 == base.GetControllerCount())
		{
			return Input.GetAxis(AxisName.GetAxisName(axis));
		}
		return listJoystickController[index].GetAxis(axis);
	}

	public override bool GetKey(KeyCode joystickButtonCode, int index)
	{
		return listJoystickController[index].GetKey(joystickButtonCode);
	}

	public override bool GetKeyUp(KeyCode joystickButtonCode, int index)
	{
		return listJoystickController[index].GetKeyUp(joystickButtonCode);
	}

	public override bool GetKeyDown(KeyCode joystickButtonCode, int index)
	{
		return listJoystickController[index].GetKeyDown(joystickButtonCode);
	}

	public override bool AnyKeyDown()
	{
		for (int i = 0; i < maxPlayer; i++)
		{
			if (listJoystickController[i].AnyKeyDown())
			{
				return true;
			}
		}
		return false;
	}
}
