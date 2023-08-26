using Foundation;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.Rendering;

public sealed class DeviceManager
{
	private static DeviceManager instance;

	private int memoryNum;

	private int cpuCoreCount;

	private string processorType;

	private Resolution currentResolution;

	private GraphicsDeviceType graphicsDeviceType;

	private int graphicsMemorySize;

	private PostProcessingBehaviour m_PostProcessingBehaviour;

	private bool isInited;

	private DeviceQuality quality = DeviceQuality.MID;

	private const string fastQualityName = "RollingSkyFast";

	private const string lowQualityName = "RollingSkyLow";

	private const string normalQualityName = "RollingSkyNormal";

	private const string midQualityName = "RollingSkyMid";

	private const string higQualityName = "RollingSkyHight";

	private int[] qualityLevelIdxs = new int[5];

	public static DeviceManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new DeviceManager();
			}
			return instance;
		}
	}

	public DeviceQuality QualityLevel
	{
		get
		{
			return quality;
		}
	}

	public void Init()
	{
		if (isInited)
		{
			return;
		}
		memoryNum = SystemInfo.systemMemorySize;
		cpuCoreCount = SystemInfo.processorCount;
		processorType = SystemInfo.processorType;
		currentResolution = Screen.currentResolution;
		graphicsDeviceType = SystemInfo.graphicsDeviceType;
		graphicsMemorySize = SystemInfo.graphicsMemorySize;
		string[] names = QualitySettings.names;
		int num = names.Length;
		for (int i = 0; i < num; i++)
		{
			switch (names[i])
			{
			case "RollingSkyFast":
				qualityLevelIdxs[0] = i;
				break;
			case "RollingSkyLow":
				qualityLevelIdxs[1] = i;
				break;
			case "RollingSkyNormal":
				qualityLevelIdxs[2] = i;
				break;
			case "RollingSkyMid":
				qualityLevelIdxs[3] = i;
				break;
			case "RollingSkyHight":
				qualityLevelIdxs[4] = i;
				break;
			}
		}
		quality = GetDeviceDefaultQuality();
		ResetQualityParams();
		isInited = true;
	}

	public DeviceQuality GetQuality()
	{
		return quality;
	}

	public void SetGameQuality(DeviceQuality quality)
	{
		if (!isInited)
		{
			Init();
		}
		QualitySettings.SetQualityLevel(qualityLevelIdxs[(int)quality], false);
		this.quality = quality;
		ResetQualityParams();
	}

	public bool IsiPhoneX()
	{
		return false;
	}

	public bool IsNeedSpecialAdapte()
	{
		return IsHasNotch();
	}

	private bool IsHasNotch()
	{
		bool result = false;
		if ((float)Screen.height / (float)Screen.width >= 2f)
		{
			result = true;
		}
		return result;
	}

	public bool IsLowEndQualityLevel(int qualityLevel)
	{
		return qualityLevel <= 1;
	}

	public void RegisterToDeviceManager(PostProcessingBehaviour postProcessingBehaviour)
	{
		m_PostProcessingBehaviour = postProcessingBehaviour;
		ResetQualityParams();
	}

	public void UnregisterToDeviceManager()
	{
		m_PostProcessingBehaviour = null;
	}

	private void ResetQualityParams()
	{
		switch (quality)
		{
		case DeviceQuality.FAST:
		case DeviceQuality.LOW:
			Shader.globalMaximumLOD = 100;
			break;
		case DeviceQuality.NORMAL:
		case DeviceQuality.MID:
			Shader.globalMaximumLOD = 200;
			break;
		case DeviceQuality.HIGTH:
			Shader.globalMaximumLOD = 300;
			break;
		default:
			Log.Error(string.Format("Unexpected device quality({0}).Setup Shader.globalMaximumLOD failed.", quality));
			break;
		}
		if ((bool)m_PostProcessingBehaviour)
		{
			m_PostProcessingBehaviour.enabled = quality > DeviceQuality.NORMAL;
		}
		DeviceQuality deviceDefaultQuality = GetDeviceDefaultQuality();
		switch (deviceDefaultQuality)
		{
		case DeviceQuality.FAST:
		case DeviceQuality.LOW:
			Mod.Core.FrameRate = 30;
			break;
		case DeviceQuality.NORMAL:
		case DeviceQuality.MID:
			Mod.Core.FrameRate = 60;
			break;
		case DeviceQuality.HIGTH:
			Mod.Core.FrameRate = 60;
			break;
		default:
			Log.Error(string.Format("Unexpected default device quality({0}).Setup FrameRate failed.", deviceDefaultQuality));
			break;
		}
	}

	private DeviceQuality GetDeviceDefaultQuality()
	{
		return DeviceQuality.NORMAL;
	}
}
