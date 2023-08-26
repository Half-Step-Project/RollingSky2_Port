using UnityEngine;

namespace RisingWin.Library
{
	public class GeneralInput
	{
		public static KeyCode[] buttonsKeyCode = new KeyCode[8]
		{
			KeyCode.JoystickButton0,
			KeyCode.JoystickButton1,
			KeyCode.JoystickButton2,
			KeyCode.JoystickButton3,
			KeyCode.JoystickButton4,
			KeyCode.JoystickButton5,
			KeyCode.JoystickButton6,
			KeyCode.JoystickButton7
		};

		public static string[] buttonsName = new string[8] { "A", "B", "X", "Y", "L", "R", "Back", "Start" };

		public int maxPlayer = 1;

		public virtual void Update(float pDeltaTime)
		{
		}

		public virtual bool AreControllersReady(int playerCount)
		{
			return GetControllerCount() >= playerCount;
		}

		public virtual int GetControllerCount()
		{
			return maxPlayer;
		}

		public virtual void ResetAxis()
		{
			Input.ResetInputAxes();
		}

		public virtual float GetAxis(AxisType axis, int index, AxisButtonTriggerType buttonTriggerType)
		{
			return Input.GetAxis(AxisName.GetAxisName(axis));
		}

		public virtual bool GetKey(KeyCode joystickButtonCode, int index)
		{
			return Input.GetKey(joystickButtonCode);
		}

		public virtual bool GetKeyUp(KeyCode joystickButtonCode, int index)
		{
			return Input.GetKeyUp(joystickButtonCode);
		}

		public virtual bool GetKeyDown(KeyCode joystickButtonCode, int index)
		{
			return Input.GetKeyDown(joystickButtonCode);
		}

		public virtual void Vibrate(float time = 0.3f, int index = 0)
		{
			MonoBehaviorSingleton<InputAdaptor>.Instance.Vibrate();
		}

		public virtual void VibrateAll(float time = 0.3f)
		{
			MonoBehaviorSingleton<InputAdaptor>.Instance.Vibrate();
		}

		public virtual bool AnyKeyDown()
		{
			return Input.anyKeyDown;
		}

		public virtual float GetAcceleration(AxisType pAxis, int pIndex)
		{
			if (pIndex == 0)
			{
				switch (pAxis)
				{
				case AxisType.HORIZONTAL:
					return Input.acceleration.x;
				case AxisType.VERTICAL:
					return Input.acceleration.y;
				}
			}
			return 0f;
		}
	}
}
