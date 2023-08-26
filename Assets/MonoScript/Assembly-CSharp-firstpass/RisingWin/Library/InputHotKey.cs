using UnityEngine;
using UnityEngine.Events;

namespace RisingWin.Library
{
	public class InputHotKey : MonoBehaviour
	{
		public static bool isEnable = true;

		private static int[] hotKeyLayerCount = new int[GeneralInput.buttonsKeyCode.Length];

		public KeyCode key;

		[SerializeField]
		private UnityEvent @event;

		private int indexLayer;

		private void OnEnable()
		{
			indexLayer = (int)GeneralInput.buttonsKeyCode[(int)key]++;
		}

		private void OnDisable()
		{
			hotKeyLayerCount[(int)key]--;
		}

		private void Update()
		{
			if (!isEnable)
			{
				return;
			}
			for (int i = 0; i < GeneralInput.buttonsKeyCode.Length; i++)
			{
				if (MonoBehaviorSingleton<InputAdaptor>.Instance.GetKeyDown(GeneralInput.buttonsKeyCode[i]))
				{
					OnKeyDown();
				}
			}
		}

		private void OnKeyDown()
		{
			if (indexLayer >= hotKeyLayerCount[(int)key])
			{
				@event.Invoke();
			}
		}
	}
}
