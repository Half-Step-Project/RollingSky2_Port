using System;
using RisingWin.Library;
using UnityEngine;

public class CMPlaySDKUtils : MonoBehaviour
{
	public class VideoEventArgs : EventArgs
	{
		private string message;

		public string Message
		{
			get
			{
				return message;
			}
		}

		public VideoEventArgs(string message)
		{
			this.message = message;
		}
	}

	public static CMPlaySDKUtils Instance;

	public bool isLoadingViewShow;

	public event EventHandler<VideoEventArgs> OnVideoClosedCallback;

	public event EventHandler<VideoEventArgs> OnVideoShowFailCallback;

	public event EventHandler<VideoEventArgs> OnVideoShowCallback;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			Init();
		}
	}

	private void Init()
	{
	}

	private void OnApplicationPause(bool isPause)
	{
	}

	private void OnApplicationQuit()
	{
		Debug.Log("infoc______ OnApplicationQuit ");
	}

	public void showLoadingView(float red, float green, float blue, float alpha)
	{
		if (!isLoadingViewShow)
		{
			isLoadingViewShow = true;
		}
	}

	public void dismissLoadingView()
	{
		if (isLoadingViewShow)
		{
			isLoadingViewShow = false;
		}
	}

	public string getCurrentTime()
	{
		return "";
	}

	public int getNetworkStatus()
	{
		return 0;
	}

	public string getDeviceId()
	{
		return "";
	}

	public void InfocEvent(string eventName, params object[] args)
	{
	}

	public void InitCMPlaySDK()
	{
		PlayerPrefsAdapter.GetString("Translator_UsedLanguage");
	}

	public void PullCloudConfigData(string language)
	{
		string text = "en";
		switch (language)
		{
		case "English":
			text = "en";
			break;
		case "French":
			text = "fr";
			break;
		case "Spanish":
			text = "es";
			break;
		case "Portuguese":
			text = "pt";
			break;
		case "Italian":
			text = "it";
			break;
		case "German":
			text = "de";
			break;
		case "ChineseSimplified":
			text = "zh_cn";
			break;
		case "ChineseTraditional":
			text = "zh_tw";
			break;
		case "Japanese":
			text = "ja";
			break;
		case "Korean":
			text = "ko";
			break;
		case "Russian":
			text = "ru";
			break;
		case "Arabic":
			text = "ar";
			break;
		case "Indonesian":
			text = "id";
			break;
		case "Turkish":
			text = "tr";
			break;
		case "Norwegian":
			text = "nb";
			break;
		case "Finnish":
			text = "fi";
			break;
		case "Swedish":
			text = "sv";
			break;
		case "Vietnamese":
			text = "vi";
			break;
		case "Dutch":
			text = "nl";
			break;
		case "Malay":
			text = "ms";
			break;
		case "Thai":
			text = "th";
			break;
		case "Hindi":
			text = "hi";
			break;
		case "Polish":
			text = "pl";
			break;
		case "Czech":
			text = "cs";
			break;
		case "Slovak":
			text = "sk";
			break;
		case "Ukrainian":
			text = "uk";
			break;
		case "Croatian":
			text = "hr";
			break;
		case "Romanian":
			text = "ro";
			break;
		case "Hebrew":
			text = "he";
			break;
		default:
			text = language;
			break;
		}
		Debug.Log("PullCloudConfigData===:" + text);
	}

	public void SetUnityInit(bool isInit)
	{
	}

	public bool CanShowOpenScreen(int scenes, bool isNewPlayer)
	{
		return false;
	}

	public void ShowOpenScreen()
	{
	}

	public bool CanShowInsertScreen()
	{
		return false;
	}

	public void ShowInsertScreen()
	{
	}

	public bool CanShowResultCard()
	{
		return false;
	}

	public string GetResultCardData()
	{
		return "";
	}

	public void ClickResultCard(string jsonData)
	{
	}

	public bool CanShowSettingCardRedDot()
	{
		return false;
	}

	public bool CanShowSettingCard()
	{
		return false;
	}

	public string GetSettingCardData()
	{
		return "";
	}

	public void ClickSettingCard(string jsonData)
	{
	}

	public bool CanShowFamilyGamesRedDot()
	{
		return false;
	}

	public bool CanShowFamilyGamesCard()
	{
		return false;
	}

	public string GetFamilyGamesData()
	{
		return "";
	}

	public void ClickFamilyGameCard(string jsonData)
	{
	}

	public bool IsInnerPushAppInstalled(string jsonDataForShow)
	{
		return false;
	}

	public bool CanShowRewardedVideo(int scene, bool isClicked)
	{
		return false;
	}

	public bool ShowRewardedVideo(int scene)
	{
		return false;
	}

	public bool CanShowHitTopRewardedVideo(int scene, bool isClicked)
	{
		return false;
	}

	public bool ShowHitTopRewardedVideo(int scene)
	{
		return false;
	}

	public void ReportData(string tableName, string data)
	{
	}

	public void ReportEvent(int action)
	{
	}

	public int getIntValue(int func_type, string section, string key, int defValue)
	{
		return 0;
	}

	public long getLongValue(int func_type, string section, string key, long defValue)
	{
		return 0L;
	}

	public bool getBooleanValue(int func_type, string section, string key, bool defValue)
	{
		return false;
	}

	public double getDoubleValue(int func_type, string section, string key, double defValue)
	{
		return 0.0;
	}

	public string getStringValue(int func_type, string section, string key, string defValue)
	{
		return "";
	}

	public void LoadFeedback(string url, string appID)
	{
	}

	public void onFamilyGamesPushUpdate(string str)
	{
		Debug.Log("internal_push onFamilyGamesPushUpdate  msg received@@@@@@");
	}

	public void onSettingsPushUpdate(string str)
	{
		Debug.Log("internal_push onSettingsPushUpdate  msg received@@@@@@");
	}

	public void onVideoClosed(string str)
	{
		Debug.Log("internal_push onVideoClosed  msg received@@@@@@      isCompleteView:" + bool.Parse(str));
		if (this.OnVideoClosedCallback != null)
		{
			this.OnVideoClosedCallback(this, new VideoEventArgs(str));
		}
	}

	public void onLoadSuccess(string str)
	{
		Debug.Log("internal_push onLoadSuccess  msg received@@@@@@");
	}

	public void onLoadError(string str)
	{
		Debug.Log("internal_push onLoadError  msg received@@@@@@   errorMsg:" + str);
	}

	public void onVideoShow(string str)
	{
		Debug.Log("internal_push onVideoShow  msg received@@@@@@");
		if (this.OnVideoShowCallback != null)
		{
			this.OnVideoShowCallback(this, new VideoEventArgs(str));
		}
	}

	public void onVideoShowFail(string str)
	{
		Debug.Log("internal_push onVideoShowFail  msg received@@@@@@  errorInfo:" + str);
		if (this.OnVideoShowFailCallback != null)
		{
			this.OnVideoShowFailCallback(this, new VideoEventArgs(str));
		}
	}

	public string GetDevicePlatform()
	{
		return "";
	}

	public void PauseADDownload()
	{
	}

	public void ResumeADDownload()
	{
	}
}
