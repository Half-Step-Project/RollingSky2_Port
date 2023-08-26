using Foundation;
using UnityEngine;

namespace RS2
{
	public class InputSystem : MonoBehaviour
	{
		public enum InputState
		{
			Keyboard,
			Joystick,
			Switch
		}

		protected static InputSystem _instance;

		private bool _isJoystick;

		public InputState inputState;

		public static InputSystem Instance
		{
			get
			{
				if (null == _instance)
				{
					_instance = Object.FindObjectOfType<InputSystem>();
					if (null == _instance)
					{
						GameObject obj = new GameObject("(singleton)" + typeof(InputSystem).Name.ToString());
						_instance = obj.AddComponent<InputSystem>();
						Object.DontDestroyOnLoad(obj);
					}
					else
					{
						Object.DontDestroyOnLoad(_instance.gameObject);
					}
				}
				return _instance;
			}
		}

		private bool isJoystick
		{
			get
			{
				return _isJoystick;
			}
			set
			{
				_isJoystick = value;
				InputService.UseController = _isJoystick;
			}
		}

		private void Update()
		{
			if (!Input.anyKey && InputService.GetAnyAxis())
			{
				isJoystick = true;
			}
			if (!isJoystick && Input.anyKey)
			{
				for (int i = 0; i < 19; i++)
				{
					if (Input.GetKey("joystick button " + i))
					{
						isJoystick = true;
						break;
					}
				}
			}
			if (isJoystick)
			{
				SetInputState(InputState.Joystick);
			}
			else if (Input.anyKey || Input.GetMouseButton(0) || Input.GetMouseButton(1))
			{
				SetInputState(InputState.Keyboard);
			}
		}

		public void SetInputState(InputState state)
		{
			if (inputState != state)
			{
				inputState = state;
				NotifyChangedInputState();
			}
		}

		private void NotifyChangedInputState()
		{
			Mod.Event.Fire(this, Mod.Reference.Acquire<ChangeInputArgs>().Initialize(inputState));
		}

		private static float Absolute(float value)
		{
			if (!(value < 0f))
			{
				return value;
			}
			return 0f - value;
		}
	}
}
