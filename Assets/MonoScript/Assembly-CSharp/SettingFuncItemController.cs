using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.UI;

public class SettingFuncItemController : MonoBehaviour
{
	private SettingFuncType funcType;

	private bool isInited;

	private Text contentTxt;

	private GameSettingForm settingForm;

	private Button m_button;

	public GameSettingForm SettingForm
	{
		set
		{
			settingForm = value;
		}
	}

	public void SetFuncType(SettingFuncType type)
	{
		funcType = type;
		Init();
		SetSaveSetting();
		AddEventListener();
	}

	private void SetSaveSetting()
	{
		switch (funcType)
		{
		case SettingFuncType.FeedBack:
			contentTxt.text = Mod.Localization.GetInfoById(8);
			break;
		case SettingFuncType.Quality:
		{
			DeviceQuality int2 = (DeviceQuality)Mod.Setting.GetInt("Setting.QualityLevel");
			CurrentQualityText(int2);
			break;
		}
		case SettingFuncType.Music:
		{
			int int3 = Mod.Setting.GetInt("Setting.MusicMuted");
			contentTxt.text = ((int3 == 1) ? Mod.Localization.GetInfoById(4) : Mod.Localization.GetInfoById(5));
			break;
		}
		case SettingFuncType.Sound:
		{
			int @int = Mod.Setting.GetInt("Setting.SoundMuted");
			contentTxt.text = ((@int == 1) ? Mod.Localization.GetInfoById(6) : Mod.Localization.GetInfoById(7));
			break;
		}
		case SettingFuncType.About:
			contentTxt.text = Mod.Localization.GetInfoById(167).Replace("\\n", "\n");
			break;
		case SettingFuncType.TermsOfService:
			contentTxt.text = Mod.Localization.GetInfoById(168).Replace("\\n", "\n");
			break;
		case SettingFuncType.PrivocyPolicy:
			contentTxt.text = Mod.Localization.GetInfoById(169).Replace("\\n", "\n");
			break;
		case SettingFuncType.AdChoices:
			contentTxt.text = Mod.Localization.GetInfoById(170).Replace("\\n", "\n");
			break;
		case SettingFuncType.DataManageMent:
			contentTxt.text = "Data Management";
			break;
		case SettingFuncType.DeleteMyData:
			contentTxt.text = "Delete data\nfrom cloud";
			break;
		case SettingFuncType.NotCollectMyData:
			contentTxt.text = "Opt out of interest-based ads";
			break;
		case SettingFuncType.RecoverBuy:
			contentTxt.text = Mod.Localization.GetInfoById(171);
			break;
		case SettingFuncType.QQ:
			contentTxt.text = Mod.Localization.GetInfoById(195);
			break;
		case SettingFuncType.Facebook:
			contentTxt.text = "Facebook";
			break;
		}
	}

	private void CurrentQualityText(DeviceQuality quality)
	{
		switch (quality)
		{
		case DeviceQuality.FAST:
			contentTxt.text = Mod.Localization.GetInfoById(176);
			break;
		case DeviceQuality.LOW:
			contentTxt.text = Mod.Localization.GetInfoById(11);
			break;
		case DeviceQuality.NORMAL:
			contentTxt.text = Mod.Localization.GetInfoById(175);
			break;
		case DeviceQuality.MID:
			contentTxt.text = Mod.Localization.GetInfoById(10);
			break;
		case DeviceQuality.HIGTH:
			contentTxt.text = Mod.Localization.GetInfoById(9);
			break;
		}
	}

	private void Init()
	{
		if (!isInited)
		{
			contentTxt = base.transform.Find("contentTxt").GetComponent<Text>();
			m_button = base.transform.GetComponent<Button>();
			isInited = true;
		}
	}

	private void AddEventListener()
	{
		if (m_button != null)
		{
			m_button.onClick.AddListener(delegate
			{
				OnClickHandler();
			});
		}
	}

	private void OnDownHandler(GameObject go)
	{
	}

	private void OnUpHandler(GameObject go)
	{
	}

	private void OnClickHandler(GameObject go = null)
	{
		switch (funcType)
		{
		case SettingFuncType.FeedBack:
			NativeUtils.Instance.OpenFeedbackView();
			break;
		case SettingFuncType.Quality:
		{
			int int3 = Mod.Setting.GetInt("Setting.QualityLevel");
			int3++;
			int3 %= 5;
			DeviceQuality deviceQuality = (DeviceQuality)int3;
			CurrentQualityText(deviceQuality);
			DeviceManager.Instance.SetGameQuality(deviceQuality);
			Mod.Setting.SetInt("Setting.QualityLevel", int3);
			break;
		}
		case SettingFuncType.Music:
		{
			int int2 = Mod.Setting.GetInt("Setting.MusicMuted");
			int2 = ((int2 != 1) ? 1 : 0);
			contentTxt.text = ((int2 == 1) ? Mod.Localization.GetInfoById(4) : Mod.Localization.GetInfoById(5));
			bool mute = int2 != 1;
			Singleton<MenuMusicController>.Instance.SetMute(mute);
			Mod.Setting.SetInt("Setting.MusicMuted", int2);
			break;
		}
		case SettingFuncType.Sound:
		{
			int @int = Mod.Setting.GetInt("Setting.SoundMuted");
			@int = ((@int != 1) ? 1 : 0);
			contentTxt.text = ((@int == 1) ? Mod.Localization.GetInfoById(6) : Mod.Localization.GetInfoById(7));
			Mod.Setting.SetInt("Setting.SoundMuted", @int);
			break;
		}
		case SettingFuncType.About:
			settingForm.ShowSecondPage();
			break;
		case SettingFuncType.TermsOfService:
			Application.OpenURL("http://www.cmcm.com/protocol/site/tos.html");
			break;
		case SettingFuncType.PrivocyPolicy:
			Application.OpenURL("http://www.cmcm.com/protocol/site/privacy.html");
			break;
		case SettingFuncType.AdChoices:
			Application.OpenURL("http://www.cmcm.com/protocol/site/ad-choice.html");
			break;
		case SettingFuncType.DataManageMent:
			settingForm.ShowThirdPage();
			break;
		case SettingFuncType.DeleteMyData:
			InfocUtils.Report_rollingsky2_games_pageshow(4, 41, 2);
			Application.OpenURL("http://www.cmcm.com/en-us/opt-out/index.html?pkg=com.kscheetah.rs2");
			break;
		case SettingFuncType.NotCollectMyData:
			InfocUtils.Report_rollingsky2_games_pageshow(4, 42, 2);
			Mod.UI.OpenUIForm(UIFormId.DontCollectDataForm);
			break;
		case SettingFuncType.RecoverBuy:
			MonoSingleton<GameTools>.Instacne.CommonRestoreBuy();
			break;
		case SettingFuncType.Facebook:
			Application.OpenURL("https://www.facebook.com/rollingsky2/");
			break;
		case SettingFuncType.QQ:
			break;
		}
	}

	private void RemoveEventListener()
	{
		if (m_button != null)
		{
			m_button.onClick.RemoveAllListeners();
		}
	}

	public void Clear()
	{
		RemoveEventListener();
	}

	public void SetShowAvaiable(bool isShow)
	{
		base.gameObject.SetActive(isShow);
	}
}
