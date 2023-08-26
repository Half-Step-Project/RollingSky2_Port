using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace RisingWin.Library
{
	public class JoystickListenerManager : MonoBehaviorSingleton<JoystickListenerManager>
	{
		private const float AXIS_THRESHOLD = 0.3f;

		private List<IJoystickListener> listeners = new List<IJoystickListener>();

		private IJoystickListener currentListener;

		private DirectionFastForwardProcessor horizontalFastForwardProcessor = new DirectionFastForwardProcessor();

		private DirectionFastForwardProcessor verticalFastForwardProcessor = new DirectionFastForwardProcessor();

		private Vector2 direction;

		private Vector2 previousDirection;

		private readonly KeyCode[] MONITORED_KEY_CODE = new KeyCode[7]
		{
			KeyCode.JoystickButton0,
			KeyCode.JoystickButton1,
			KeyCode.JoystickButton2,
			KeyCode.JoystickButton3,
			KeyCode.JoystickButton4,
			KeyCode.JoystickButton5,
			KeyCode.Escape
		};

		public void PushListener(IJoystickListener pListener)
		{
			listeners.Add(pListener);
			ChangeListener();
		}

		private void ChangeListener()
		{
			currentListener = GetLastListener();
		}

		public void PopListener(IJoystickListener pListener)
		{
			if (listeners.Contains(pListener))
			{
				listeners.Remove(pListener);
			}
			currentListener = GetLastListener();
		}

		private IJoystickListener GetLastListener()
		{
			if (listeners.Count == 0)
			{
				return null;
			}
			return listeners[listeners.Count - 1];
		}

		public void Clear()
		{
			listeners.Clear();
		}

		private void Update()
		{
			if (currentListener != null)
			{
				OnDirectionProcess();
				OnButtonProcess();
			}
		}

		private void OnDirectionProcess()
		{
			direction.x = MonoBehaviorSingleton<InputAdaptor>.Instance.GetAxis(AxisType.HORIZONTAL);
			if (Mathf.Abs(previousDirection.x) < 0.3f && Mathf.Abs(direction.x) >= 0.3f)
			{
				currentListener.OnDirectionTrigger(JoystickDirection.HORIZONTAL, direction.x);
			}
			direction.y = MonoBehaviorSingleton<InputAdaptor>.Instance.GetAxis(AxisType.VERTICAL);
			if (Mathf.Abs(previousDirection.y) < 0.3f && Mathf.Abs(direction.y) >= 0.3f)
			{
				currentListener.OnDirectionTrigger(JoystickDirection.VERTICAL, direction.y);
			}
			horizontalFastForwardProcessor.OnProcess(direction.x, previousDirection.x, Time.deltaTime, _003COnDirectionProcess_003Eb__14_0);
			verticalFastForwardProcessor.OnProcess(direction.y, previousDirection.y, Time.deltaTime, _003COnDirectionProcess_003Eb__14_1);
			previousDirection = direction;
		}

		private void OnButtonProcess()
		{
			for (int i = 0; i < MONITORED_KEY_CODE.Length; i++)
			{
				KeyCode keyCode = MONITORED_KEY_CODE[i];
				if (MonoBehaviorSingleton<InputAdaptor>.Instance.GetKeyDown(keyCode))
				{
					JoystickButton pButton = ToJoystickButton(keyCode);
					currentListener.OnButtonTrigger(pButton);
				}
			}
		}

		private JoystickButton ToJoystickButton(KeyCode pKeyCode)
		{
			switch (pKeyCode)
			{
			case KeyCode.JoystickButton0:
				return JoystickButton.CONFIRM;
			case KeyCode.JoystickButton1:
				return JoystickButton.CANCEL;
			case KeyCode.JoystickButton2:
				return JoystickButton.MENU;
			case KeyCode.JoystickButton3:
				return JoystickButton.JUMP;
			case KeyCode.JoystickButton4:
				return JoystickButton.SWITCH_LEFT;
			case KeyCode.JoystickButton5:
				return JoystickButton.SWITCH_RIGHT;
			case KeyCode.Escape:
				return JoystickButton.ESCAPE;
			default:
				throw new ArgumentException("Not support KeyCode:" + pKeyCode);
			}
		}

		[CompilerGenerated]
		private void _003COnDirectionProcess_003Eb__14_0(float pValue)
		{
			currentListener.OnConsequentDirectionTrigger(JoystickDirection.HORIZONTAL, pValue);
		}

		[CompilerGenerated]
		private void _003COnDirectionProcess_003Eb__14_1(float pValue)
		{
			currentListener.OnConsequentDirectionTrigger(JoystickDirection.VERTICAL, pValue);
		}
	}
}
