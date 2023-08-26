using System.Collections.Generic;
using Foundation;
using RisingWin.Library;
using UnityEngine;

public class InputController : MonoBehaviour, IOriginRebirth
{
	public enum DragState
	{
		None,
		DragDown,
		DragUp
	}

	private class FloatBuffer
	{
		private int index;

		private List<float> bufferList;

		public float Average
		{
			get
			{
				float num = 0f;
				for (int i = 0; i < bufferList.Count; i++)
				{
					num += bufferList[i];
				}
				return num / (float)bufferList.Count;
			}
		}

		public float Value
		{
			get
			{
				return bufferList[index];
			}
		}

		public bool IsEmpty { get; private set; }

		public FloatBuffer(int num)
		{
			if (num <= 0)
			{
				Debug.LogError("num should bigger than 0!");
			}
			bufferList = new List<float>(num);
			for (int i = 0; i < num; i++)
			{
				bufferList.Add(0f);
			}
			index = bufferList.Count - 1;
			IsEmpty = true;
		}

		public void Reset(float f)
		{
			if (bufferList != null)
			{
				for (int i = 0; i < bufferList.Count; i++)
				{
					bufferList[i] = f;
				}
				index = bufferList.Count - 1;
				IsEmpty = false;
			}
		}

		public void Add(float f)
		{
			index = (index + 1) % bufferList.Count;
			bufferList[index] = f;
			IsEmpty = false;
		}

		public void Clear()
		{
			Reset(0f);
			IsEmpty = true;
		}
	}

	public bool HasFingerInput;

	public static bool hasSixAsix = false;

	public static float ctrlSensitivity = 0.9f;

	public static float sixAsixSensitivity = 1f;

	public static float mouseSensitivity = 2f;

	public static float accelerate = 2f;

	public static float zPos;

	private float lastPos;

	public static float InputPercent = 0f;

	public static float InputDeltaMovement = 0f;

	private static bool IfInputEnable = true;

	private static bool IfInputPause = false;

	public static readonly float InputMargin = 0.001f;

	public static readonly float OffsetMargin = 1f;

	public const float DEFAULT_NORMALIZE_SPEED = 2f;

	public const float DEFAULT_SENSITIVITY = 1.2f;

	public static float NormalizeSpeed = 2f;

	public static float Sensitivity = 1.2f;

	public static float controllerSensitivity = 2f;

	private float screenWidth;

	private float screenHeight;

	public float inputOffset;

	public float inputPosition;

	private float offsetNormalize;

	public static InputController instance;

	private FloatBuffer lastPosition;

	public static Vector2 FreeOffset;

	public float MoveRangeX = 0.005f;

	public float MoveRangeY = 0.005f;

	public float MoveLimitX = 0.5f;

	public float MoveLimitY = 0.5f;

	private Vector2 freeBeginPos;

	private Vector2 freeCurPos;

	private FloatBuffer freeLastPosX;

	private FloatBuffer freeLastPosY;

	private DragState dragState;

	private Vector2 beginDragPos = Vector2.zero;

	private Vector2 endDragPos = Vector2.zero;

	private bool ifUpdateDrag;

	public Vector3 InputPosition
	{
		get
		{
			Vector3 mousePosition = Input.mousePosition;
			if (ifPosValid(mousePosition))
			{
				return mousePosition;
			}
			if (Input.touchCount > 0)
			{
				mousePosition = Input.touches[0].position;
				if (ifPosValid(mousePosition))
				{
					return mousePosition;
				}
			}
			Log.Warning("Input Invalid: " + Input.mousePosition);
			return Vector3.zero;
		}
	}

	public bool IsRecordOriginRebirth
	{
		get
		{
			return true;
		}
	}

	private bool ifPosValid(Vector3 pos)
	{
		if (pos.x <= screenWidth * 2f && pos.x >= (0f - screenWidth) * 2f && pos.y <= screenHeight * 2f && pos.y >= (0f - screenHeight) * 2f)
		{
			return true;
		}
		return false;
	}

	private void Awake()
	{
		instance = this;
	}

	private void OnDestroy()
	{
		if (instance != null)
		{
			instance = null;
		}
	}

	private void Start()
	{
		lastPosition = new FloatBuffer(3);
		freeLastPosX = new FloatBuffer(3);
		freeLastPosY = new FloatBuffer(3);
		screenWidth = Screen.width;
		screenHeight = Screen.height;
	}

	public void InputUpdate()
	{
		NewInputUpdate();
	}

	private void HorizonInputUpdate()
	{
		offsetNormalize = NormalizeSpeed * ((float)Screen.width / 10f);
		if (Input.GetMouseButtonDown(0))
		{
			HasFingerInput = true;
			inputPosition = InputPosition.x;
			if (lastPosition.IsEmpty)
			{
				inputOffset = inputPosition - (float)Screen.width / 2f;
			}
			else
			{
				inputOffset = inputPosition - lastPosition.Value + inputOffset;
			}
			lastPosition.Clear();
			lastPosition.Reset(inputPosition);
			InputDeltaMovement = 0f;
		}
		if (Input.GetMouseButton(0))
		{
			inputPosition = InputPosition.x;
			InputDeltaMovement = Mathf.Abs(inputPosition - lastPosition.Average);
			if (InputDeltaMovement >= InputMargin)
			{
				if (Mathf.Abs(inputOffset) <= OffsetMargin)
				{
					inputOffset = 0f;
				}
				else
				{
					float num = offsetNormalize * Time.smoothDeltaTime;
					if (inputOffset < 0f)
					{
						inputOffset += num;
					}
					else
					{
						inputOffset -= num;
					}
				}
			}
			lastPosition.Add(inputPosition);
			float num2 = (inputPosition - inputOffset) / (float)Screen.width;
			InputPercent = ((2f * num2 - 1f) * (1f + Sensitivity) + 1f) / 2f * 5f - 2.5f;
		}
		if (Input.GetMouseButtonUp(0))
		{
			inputPosition = InputPosition.x;
			HasFingerInput = false;
			lastPosition.Clear();
			InputDeltaMovement = 0f;
		}
	}

	private void NewInputUpdate()
	{
		if (Input.GetMouseButtonDown(0))
		{
			HasFingerInput = true;
			inputPosition = Input.mousePosition.x;
			lastPosition.Clear();
			lastPosition.Reset(inputPosition);
		}
		if (Input.GetMouseButton(0))
		{
			float num = (Input.mousePosition.x - inputPosition) / (float)(Screen.width / 2) * 5f * mouseSensitivity;
			InputPercent = lastPos + num;
			zPos = InputPercent;
		}
		if (Input.GetMouseButtonUp(0))
		{
			lastPos = InputPercent;
			float num2 = 5f;
			lastPos = ((lastPos > num2) ? num2 : lastPos);
			lastPos = ((lastPos < 0f - num2) ? (0f - num2) : lastPos);
			HasFingerInput = false;
			lastPosition.Clear();
		}
	}

	private void FreeInputUpdate()
	{
		if (Input.GetMouseButtonDown(0))
		{
			freeBeginPos = InputPosition;
			freeLastPosX.Clear();
			freeLastPosY.Clear();
			freeLastPosX.Reset(freeBeginPos.x);
			freeLastPosY.Reset(freeBeginPos.y);
			FreeOffset = Vector2.zero;
		}
		if (Input.GetMouseButton(0))
		{
			freeCurPos = InputPosition;
			FreeOffset.x = freeCurPos.x - freeLastPosX.Average;
			FreeOffset.y = freeCurPos.y - freeLastPosY.Average;
			freeLastPosX.Add(freeCurPos.x);
			freeLastPosY.Add(freeCurPos.y);
		}
		if (Input.GetMouseButtonUp(0))
		{
			freeLastPosX.Clear();
			freeLastPosY.Clear();
			FreeOffset = Vector2.zero;
		}
		FreeOffset.x = Mathf.Max(0f - MoveLimitX, Mathf.Min(FreeOffset.x * MoveRangeX, MoveLimitX));
		FreeOffset.y = Mathf.Max(0f - MoveLimitY, Mathf.Min(FreeOffset.y * MoveRangeY, MoveLimitY));
	}

	private void HorizonJoystickInputUpdate()
	{
		if (!HasFingerInput)
		{
			if (InputService.GetKey_Left)
			{
				inputPosition = -1f;
			}
			else if (InputService.GetKey_Right)
			{
				inputPosition = 1f;
			}
			else if (InputService.GetKey_Right || InputService.GetKey_Left)
			{
				inputPosition = 0f;
			}
			else
			{
				inputPosition = MonoBehaviorSingleton<InputAdaptor>.Instance.GetAxis(AxisType.HORIZONTAL);
			}
			InputPercent = inputPosition * 5f * ctrlSensitivity;
			lastPos = InputPercent;
		}
	}

	public void Reset()
	{
		InputPercent = 0f;
		FreeOffset = Vector2.zero;
		ResetInputParam();
		ResetInputPrivateParam();
		IfInputEnable = true;
		IfInputPause = false;
		HasFingerInput = false;
		lastPos = 0f;
	}

	private void ResetInputPrivateParam()
	{
		inputOffset = 0f;
		zPos = 0f;
		lastPos = 0f;
		lastPosition.Clear();
		freeLastPosX.Clear();
		freeLastPosY.Clear();
	}

	public void ResetInputParam(float normSpeed = 2f, float sens = 1.2f)
	{
		NormalizeSpeed = normSpeed;
		Sensitivity = sens;
	}

	public void EnableInput(bool ifEnable)
	{
		if (ifEnable)
		{
			InputPercent = 0f;
			ResetInputPrivateParam();
		}
		IfInputEnable = ifEnable;
	}

	public void PauseInput(bool ifPause)
	{
		if (!ifPause)
		{
			InputPercent = 0f;
			ResetInputPrivateParam();
		}
		IfInputPause = ifPause;
	}

	public bool IfEnableInput()
	{
		return IfInputEnable;
	}

	public bool IfPauseInput()
	{
		return IfInputPause;
	}

	public void BeginCheckDrag()
	{
		ResetEasyDrag();
		ifUpdateDrag = true;
	}

	public void EndCheckDrag()
	{
		ResetEasyDrag();
		ifUpdateDrag = false;
	}

	public void UpdateEasyDrag()
	{
		if (!ifUpdateDrag)
		{
			return;
		}
		switch (dragState)
		{
		case DragState.None:
			if (Input.GetMouseButtonDown(0))
			{
				dragState = DragState.DragDown;
				beginDragPos = InputPosition;
			}
			break;
		case DragState.DragDown:
			if (Input.GetMouseButtonUp(0))
			{
				dragState = DragState.DragUp;
				endDragPos = InputPosition;
			}
			break;
		case DragState.DragUp:
			break;
		}
	}

	public void ResetEasyDragPerframe()
	{
		if (dragState == DragState.DragUp)
		{
			ResetEasyDrag();
		}
	}

	public void ResetEasyDrag()
	{
		dragState = DragState.None;
		beginDragPos = Vector2.zero;
		endDragPos = Vector2.zero;
	}

	public bool IfEasyDragFinished(out Vector2 data)
	{
		data = endDragPos - beginDragPos;
		return dragState == DragState.DragUp;
	}

	public object GetOriginRebirthData(object obj = null)
	{
		return JsonUtility.ToJson(new RD_Input_DATA
		{
			Sensitivity = Sensitivity,
			NormalizeSpeed = NormalizeSpeed,
			IfInputDisable = !IfInputEnable
		});
	}

	public void SetOriginRebirthData(object dataInfo)
	{
		RD_Input_DATA rD_Input_DATA = JsonUtility.FromJson<RD_Input_DATA>(dataInfo as string);
		IfInputEnable = !rD_Input_DATA.IfInputDisable;
		Sensitivity = rD_Input_DATA.Sensitivity;
		NormalizeSpeed = rD_Input_DATA.NormalizeSpeed;
	}

	public void StartRunByOriginRebirthData(object dataInfo)
	{
	}

	public byte[] GetOriginRebirthBsonData(object obj = null)
	{
		byte[] array = new byte[0];
		return Bson.ToBson(new RD_Input_DATA
		{
			Sensitivity = Sensitivity,
			NormalizeSpeed = NormalizeSpeed,
			IfInputDisable = !IfInputEnable
		});
	}

	public void SetOriginRebirthBsonData(byte[] dataInfo)
	{
		RD_Input_DATA rD_Input_DATA = Bson.ToObject<RD_Input_DATA>(dataInfo);
		IfInputEnable = !rD_Input_DATA.IfInputDisable;
		Sensitivity = rD_Input_DATA.Sensitivity;
		NormalizeSpeed = rD_Input_DATA.NormalizeSpeed;
	}

	public void StartRunByOriginRebirthBsonData(byte[] dataInfo)
	{
	}
}
