using RisingWin.Library;
using UnityEngine;

public class JoystickController
{
	public int id;

	public KeyCode buttonA = KeyCode.JoystickButton0;

	public KeyCode buttonB = KeyCode.JoystickButton1;

	public KeyCode buttonY = KeyCode.JoystickButton2;

	public KeyCode buttonX = KeyCode.JoystickButton3;

	public KeyCode buttonL = KeyCode.JoystickButton4;

	public KeyCode buttonR = KeyCode.JoystickButton5;

	public KeyCode buttonUp = KeyCode.JoystickButton10;

	public KeyCode buttonDown = KeyCode.JoystickButton11;

	public KeyCode buttonLeft = KeyCode.JoystickButton12;

	public KeyCode buttonRight = KeyCode.JoystickButton13;

	public KeyCode buttonSetting = KeyCode.Escape;

	public void Init(int id, KeyCode buttonA, KeyCode buttonB, KeyCode buttonY, KeyCode buttonX, KeyCode buttonL, KeyCode buttonR)
	{
		this.id = id;
		this.buttonA = buttonA;
		this.buttonB = buttonB;
		this.buttonY = buttonY;
		this.buttonX = buttonX;
		this.buttonL = buttonL;
		this.buttonR = buttonR;
	}

	public void Vibrate(float time = 0.3f)
	{
		MonoBehaviorSingleton<InputAdaptor>.Instance.Vibrate();
	}

	public void StopVibration()
	{
	}

	public float GetAxis(AxisType axis)
	{
		return Input.GetAxis(AxisName.GetAxisName(axis, id));
	}

	public bool GetKey(KeyCode joystickButtonCode)
	{
		KeyCode realkeyCode = KeyCode.None;
		if (!ToJoystickButton(ref realkeyCode, joystickButtonCode))
		{
			return false;
		}
		return Input.GetKey(realkeyCode);
	}

	public bool GetKeyUp(KeyCode joystickButtonCode)
	{
		KeyCode realkeyCode = KeyCode.None;
		if (!ToJoystickButton(ref realkeyCode, joystickButtonCode))
		{
			return false;
		}
		return Input.GetKeyUp(realkeyCode);
	}

	public bool GetKeyDown(KeyCode joystickButtonCode)
	{
		KeyCode realkeyCode = KeyCode.None;
		if (!ToJoystickButton(ref realkeyCode, joystickButtonCode))
		{
			return false;
		}
		return Input.GetKeyDown(realkeyCode);
	}

	public bool AnyKeyDown()
	{
		KeyCode realkeyCode = KeyCode.None;
		if (!ToJoystickButton(ref realkeyCode, KeyCode.JoystickButton0))
		{
			return false;
		}
		return Input.GetKeyDown(realkeyCode);
	}

	public bool ToJoystickButton(ref KeyCode realkeyCode, KeyCode joystickButtonCode)
	{
		switch (joystickButtonCode)
		{
		case KeyCode.JoystickButton0:
			realkeyCode = buttonA;
			return true;
		case KeyCode.JoystickButton1:
			realkeyCode = buttonB;
			return true;
		case KeyCode.JoystickButton2:
			realkeyCode = buttonY;
			return true;
		case KeyCode.JoystickButton3:
			realkeyCode = buttonX;
			return true;
		case KeyCode.JoystickButton4:
			realkeyCode = buttonL;
			return true;
		case KeyCode.JoystickButton5:
			realkeyCode = buttonR;
			return true;
		case KeyCode.JoystickButton10:
			realkeyCode = buttonUp;
			return true;
		case KeyCode.JoystickButton11:
			realkeyCode = buttonDown;
			return true;
		case KeyCode.JoystickButton12:
			realkeyCode = buttonLeft;
			return true;
		case KeyCode.JoystickButton13:
			realkeyCode = buttonRight;
			return true;
		default:
			return false;
		}
	}
}
