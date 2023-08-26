using UnityEngine;
using UnityEngine.EventSystems;

namespace Foundation
{
	public sealed class UIInput : BaseInput
	{
		public bool EnableInput { get; set; }

		public override string compositionString
		{
			get
			{
				if (!EnableInput)
				{
					return string.Empty;
				}
				return Input.compositionString;
			}
		}

		public override IMECompositionMode imeCompositionMode
		{
			get
			{
				if (!EnableInput)
				{
					return IMECompositionMode.Off;
				}
				return Input.imeCompositionMode;
			}
			set
			{
				if (EnableInput)
				{
					Input.imeCompositionMode = value;
				}
			}
		}

		public override Vector2 compositionCursorPos
		{
			get
			{
				if (!EnableInput)
				{
					return Vector2.zero;
				}
				return Input.compositionCursorPos;
			}
			set
			{
				Input.compositionCursorPos = value;
			}
		}

		public override bool mousePresent
		{
			get
			{
				if (EnableInput)
				{
					return Input.mousePresent;
				}
				return false;
			}
		}

		public override Vector2 mousePosition
		{
			get
			{
				if (!EnableInput)
				{
					return Vector2.zero;
				}
				return Input.mousePosition;
			}
		}

		public override Vector2 mouseScrollDelta
		{
			get
			{
				if (!EnableInput)
				{
					return Vector2.zero;
				}
				return Input.mouseScrollDelta;
			}
		}

		public override int touchCount
		{
			get
			{
				if (!EnableInput)
				{
					return 0;
				}
				return Input.touchCount;
			}
		}

		public UIInput()
		{
			EnableInput = true;
		}

		public override bool GetMouseButtonDown(int button)
		{
			if (EnableInput)
			{
				return Input.GetMouseButtonDown(button);
			}
			return false;
		}

		public override bool GetMouseButtonUp(int button)
		{
			if (EnableInput)
			{
				return Input.GetMouseButtonUp(button);
			}
			return false;
		}

		public override bool GetMouseButton(int button)
		{
			if (EnableInput)
			{
				return Input.GetMouseButton(button);
			}
			return false;
		}

		public override Touch GetTouch(int index)
		{
			if (!EnableInput)
			{
				return default(Touch);
			}
			return Input.GetTouch(index);
		}

		public override float GetAxisRaw(string axisName)
		{
			if (!EnableInput)
			{
				return 0f;
			}
			return Input.GetAxisRaw(axisName);
		}

		public override bool GetButtonDown(string buttonName)
		{
			if (EnableInput)
			{
				return Input.GetButtonDown(buttonName);
			}
			return false;
		}
	}
}
