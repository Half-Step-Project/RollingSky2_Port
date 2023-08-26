namespace ZBDD.Data
{
	public class SettingDataDynamic
	{
		public enum WindowMode
		{
			Window,
			Fullscreen
		}

		public enum Language
		{
			None,
			English,
			TraditionalChinese,
			SimplifiedChinese,
			Persian
		}

		public enum InputState
		{
			Keyboard,
			Joystick,
			VirtualJoystick,
			Joystick_Switch
		}

		public static SettingDataDynamic Instance;

		private const string DEFALUT_RESOLUTION = "1280 x 720 @ 60Hz";

		public WindowMode windowMode = WindowMode.Fullscreen;

		public string resolution = "1280 x 720 @ 60Hz";

		public float musicVolume = 1f;

		public float soundVolume = 1f;

		public Language language;

		public InputState inputState;

		public static void Init()
		{
			Instance = new SettingDataDynamic();
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
		}
	}
}
