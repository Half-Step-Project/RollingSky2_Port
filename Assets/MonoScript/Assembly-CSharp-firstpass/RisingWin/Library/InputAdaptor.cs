using UnityEngine;

namespace RisingWin.Library
{
	public class InputAdaptor : MonoBehaviorSingleton<InputAdaptor>
	{
		private GeneralInput input;

		public override void Awake()
		{
			base.Awake();
			input = new JoystickInput();
		}

		public void SetDirectionButtonAsAxis(bool isDirectionButtonAsAxis)
		{
		}

		public void Update()
		{
			input.Update(Time.unscaledDeltaTime);
		}

		public bool IsOnlyOnePlayer()
		{
			return 1 == GetControllerCount();
		}

		public bool AreControllersReady(int playerCount)
		{
			return input.AreControllersReady(playerCount);
		}

		public int GetControllerCount()
		{
			return input.GetControllerCount();
		}

		public void ResetAxis()
		{
			input.ResetAxis();
		}

		public float GetAxis(AxisType axis, int index = 0, AxisButtonTriggerType buttonTriggerType = AxisButtonTriggerType.DISCONTINUOUS)
		{
			return input.GetAxis(axis, index, buttonTriggerType);
		}

		public bool GetKey(KeyCode joystickButtonCode, int index = 0)
		{
			return input.GetKey(joystickButtonCode, index);
		}

		public bool GetKeyUp(KeyCode joystickButtonCode, int index = 0)
		{
			return input.GetKeyUp(joystickButtonCode, index);
		}

		public bool GetKeyDown(KeyCode joystickButtonCode, int index = 0)
		{
			return input.GetKeyDown(joystickButtonCode, index);
		}

		public void Vibrate(float time = 0.3f, int index = 0)
		{
			input.Vibrate(time, index);
		}

		public void VibrateAll(float time = 0.3f)
		{
			input.VibrateAll(time);
		}

		public bool AnyKeyDown()
		{
			return input.AnyKeyDown();
		}

		public float GetAcceleration(AxisType pAxis, int pIndex = 0)
		{
			return input.GetAcceleration(pAxis, pIndex);
		}
	}
}
