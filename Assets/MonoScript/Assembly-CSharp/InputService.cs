using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputService
{
	public enum ButtonMapping
	{
		None,
		Up,
		Down,
		Left,
		Right,
		A,
		B,
		X,
		Y,
		L,
		R,
		UpPress,
		DownPress,
		LeftPress,
		RightPress,
		Tab,
		SkillPage,
		BagPage,
		FacilityPage,
		MapPage,
		SettingPage,
		Weapon1,
		Weapon2,
		Weapon3,
		Weapon4,
		Attack,
		Detect,
		Sneak,
		FixFacility,
		RecycleFacility,
		ClearSave
	}

	private const float THRESHOLD = 0.2f;

	private const float UDRL_ARROW = 1f;

	private static bool Joystick;

	private static bool Keyboard;

	private static int playModeCount = 2;

	private static bool useController = false;

	private static readonly string[] keyboardTextArray = new string[31]
	{
		"", "W", "S", "A", "D", "Enter", "Esc", "I", "J", "Q",
		"E", "W", "S", "A", "D", "Tab", "K", "I", "B", "M",
		"O", "1", "2", "3", "4", "Space", "Enter", "Shift", "Enter", "Back",
		"I"
	};

	private static readonly string[] joystickTextArray = new string[31]
	{
		"", "Up", "Down", "Left", "Right", "A", "B", "X", "Y", "L",
		"R", "Up", "Down", "Left", "Right", "LT/RT", "Down", "Up", "Left", "Right",
		"Button8", "", "", "", "", "X", "A", "B", "A", "B",
		"X"
	};

	private static GameObject GobjVirtualJoystick;

	public static bool UseController
	{
		get
		{
			return useController;
		}
		set
		{
			useController = value;
			Mod.Event.Fire(null, Mod.Reference.Acquire<ChangeControllerArgs>().Initialize());
		}
	}

	public static bool KeyDown_Up
	{
		get
		{
			Keyboard = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
			UseController = false;
			if (!Joystick)
			{
				return Keyboard;
			}
			return true;
		}
	}

	public static bool KeyDown_Down
	{
		get
		{
			Keyboard = Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow);
			UseController = false;
			if (!Joystick)
			{
				return Keyboard;
			}
			return true;
		}
	}

	public static bool KeyDown_Left
	{
		get
		{
			Keyboard = Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow);
			UseController = false;
			if (!Joystick)
			{
				return Keyboard;
			}
			return true;
		}
	}

	public static bool KeyDown_Right
	{
		get
		{
			Keyboard = Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow);
			if (!Joystick)
			{
				return Keyboard;
			}
			return true;
		}
	}

	public static bool GetKey_Left
	{
		get
		{
			Keyboard = Input.GetKey(KeyCode.LeftArrow);
			UseController = false;
			if (!Joystick)
			{
				return Keyboard;
			}
			return true;
		}
	}

	public static bool KeyUp_Left
	{
		get
		{
			Keyboard = Input.GetKeyUp(KeyCode.LeftArrow);
			UseController = false;
			if (!Joystick)
			{
				return Keyboard;
			}
			return true;
		}
	}

	public static bool GetKey_Right
	{
		get
		{
			Keyboard = Input.GetKey(KeyCode.RightArrow);
			UseController = false;
			if (!Joystick)
			{
				return Keyboard;
			}
			return true;
		}
	}

	public static bool KeyUp_Right
	{
		get
		{
			Keyboard = Input.GetKeyUp(KeyCode.RightArrow);
			UseController = false;
			if (!Joystick)
			{
				return Keyboard;
			}
			return true;
		}
	}

	public static bool KeyDown_A
	{
		get
		{
			Keyboard = Input.GetKeyDown(KeyCode.Return);
			UseController = false;
			if (!Joystick)
			{
				return Keyboard;
			}
			return true;
		}
	}

	public static bool KeyDown_B
	{
		get
		{
			Keyboard = Input.GetKeyDown(KeyCode.Backspace);
			UseController = false;
			if (!Joystick)
			{
				return Keyboard;
			}
			return true;
		}
	}

	public static bool KeyDown_X
	{
		get
		{
			Keyboard = Input.GetKeyDown(KeyCode.Space);
			UseController = false;
			if (!Joystick)
			{
				return Keyboard;
			}
			return true;
		}
	}

	public static bool GetKey_x
	{
		get
		{
			Keyboard = Input.GetKey(KeyCode.Space);
			UseController = false;
			if (!Joystick)
			{
				return Keyboard;
			}
			return true;
		}
	}

	public static bool KeyDown_Y
	{
		get
		{
			Keyboard = Input.GetKeyDown(KeyCode.I);
			UseController = false;
			if (!Joystick)
			{
				return Keyboard;
			}
			return true;
		}
	}

	public static bool KeyDown_L
	{
		get
		{
			Keyboard = Input.GetKeyDown(KeyCode.Q);
			UseController = false;
			if (!Joystick)
			{
				return Keyboard;
			}
			return true;
		}
	}

	public static bool KeyDown_R
	{
		get
		{
			Keyboard = Input.GetKeyDown(KeyCode.E);
			UseController = false;
			if (!Joystick)
			{
				return Keyboard;
			}
			return true;
		}
	}

	public static bool KeyDown_Invincible
	{
		get
		{
			Keyboard = Input.GetKeyDown(KeyCode.F2);
			UseController = false;
			if (!Joystick)
			{
				return Keyboard;
			}
			return true;
		}
	}

	public static bool keyDown_Option
	{
		get
		{
			Keyboard = Input.GetKeyDown(KeyCode.Escape);
			UseController = false;
			if (!Joystick)
			{
				return Keyboard;
			}
			return true;
		}
	}

	public static bool KeyDown_Language
	{
		get
		{
			Keyboard = Input.GetKeyDown(KeyCode.Tab);
			UseController = false;
			if (!Joystick)
			{
				return Keyboard;
			}
			return true;
		}
	}

	public static bool KeyDown(KeyCode keyCode)
	{
		if (Input.GetKeyDown(keyCode))
		{
			UseController = false;
		}
		return Input.GetKeyDown(keyCode);
	}

	public static bool GetAnyAxis()
	{
		if (Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f)
		{
			return true;
		}
		return false;
	}

	public static void CreateVirtualJoystick()
	{
		if (!(GobjVirtualJoystick != null))
		{
			GobjVirtualJoystick.transform.SetParent(null);
			SceneManager.MoveGameObjectToScene(GobjVirtualJoystick, SceneManager.GetSceneByName("GamePlay"));
		}
	}

	public static void ShowVirtualJoystick()
	{
		if (GobjVirtualJoystick != null)
		{
			GobjVirtualJoystick.SetActive(true);
		}
	}

	public static void HideVirtualJoystick()
	{
		if (GobjVirtualJoystick != null)
		{
			GobjVirtualJoystick.SetActive(false);
		}
	}

	public static void RemoveVirtualJoystick()
	{
		if (GobjVirtualJoystick != null)
		{
			GobjVirtualJoystick = null;
		}
	}

	public static string GetKeyboardText(ButtonMapping pButton)
	{
		return keyboardTextArray[(int)pButton];
	}

	public static string GetJoystickText(ButtonMapping pButton)
	{
		return joystickTextArray[(int)pButton];
	}
}
