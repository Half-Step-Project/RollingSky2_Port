using System;
using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.UI;

public class InstrumentItem : MonoBehaviour
{
	public MusicInstrumentForm m_musicForm;

	public InstrumentData m_data;

	public Text m_name;

	public Text m_lvl;

	public Text m_output;

	public Text m_outputCount;

	public Text m_nextLvl;

	public Text m_nextLvlCount;

	public Text m_lvlUp;

	public Text m_lvlUpNum;

	public Text m_coin;

	public Image m_icon;

	public Image m_adicon;

	public Image m_noticon;

	public Image m_btnOn;

	public Image m_btnOff;

	public Button m_btn;

	public Button m_notBtn;

	public Text m_notLvl;

	public Button m_adBtn;

	public ParticleSystem particles;

	public GameObject m_back1;

	public GameObject m_back2;

	private int m_stopEffectPosY;

	private List<object> loadedAsserts = new List<object>();

	private bool ifLoadImage;

	public void SetData(InstrumentData data)
	{
		m_data = data;
		if (particles == null)
		{
			particles = GetComponentInChildren<ParticleSystem>();
			if ((bool)particles)
			{
				particles.Stop();
			}
		}
		m_stopEffectPosY = -50;
		ifLoadImage = false;
	}

	public void RefreshData()
	{
		base.gameObject.SetActive(m_data != null);
		if (m_data == null)
		{
			return;
		}
		m_lvl.text = "lv" + m_data.m_lv;
		m_name.text = m_data.m_desStr;
		m_outputCount.text = (m_data.m_ifLock ? "???" : (MonoSingleton<GameTools>.Instacne.DoubleToFormatString(m_data.m_output) + "/s"));
		m_output.text = string.Format(Mod.Localization.GetInfoById(254), m_data.m_ifLock ? "???" : (MonoSingleton<GameTools>.Instacne.DoubleToFormatString(m_data.m_output) + "/s"));
		m_nextLvlCount.text = (m_data.m_ifLock ? "???" : (MonoSingleton<GameTools>.Instacne.DoubleToFormatString(m_data.m_nextlvl) + "/s"));
		m_nextLvl.text = string.Format(Mod.Localization.GetInfoById(255), m_data.m_ifLock ? "???" : (MonoSingleton<GameTools>.Instacne.DoubleToFormatString(m_data.m_nextlvl) + "/s"));
		m_lvlUpNum.text = " x " + m_data.m_lvlup;
		m_coin.text = MonoSingleton<GameTools>.Instacne.DoubleToFormatString(m_data.m_coin);
		m_btn.interactable = m_data.m_ifBtnEnable;
		bool flag = m_data.m_ifLowest && m_data.m_lv < m_data.m_instrumentCanUpMaxLevel;
		m_btn.gameObject.SetActive(!m_data.m_ifLock && !flag);
		m_adBtn.gameObject.SetActive(flag);
		m_btnOn.gameObject.SetActive(m_data.m_ifBtnEnable);
		m_btnOff.gameObject.SetActive(!m_data.m_ifBtnEnable && !m_data.m_ifLock);
		m_notBtn.gameObject.SetActive(m_data.m_ifLock);
		if (m_btnOff.gameObject.activeSelf)
		{
			m_coin.text = string.Format("<color=#fe4949FF>{0}</color>", m_coin.text);
		}
		if (!ifLoadImage)
		{
			Mod.Resource.LoadAsset(AssetUtility.GetGameIconAsset(m_data.m_iconId), new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
			{
				if (m_icon != null)
				{
					m_icon.sprite = MonoSingleton<GameTools>.Instacne.CreateSpriteByTexture2D(asset as Texture2D);
					m_noticon.sprite = MonoSingleton<GameTools>.Instacne.CreateSpriteByTexture2D(asset as Texture2D);
					loadedAsserts.Add(asset);
					ifLoadImage = true;
				}
			}, delegate(string assetName, string errorMessage, object data2)
			{
				Log.Error(string.Format("Can not load item '{0}' from '{1}' with error message.", assetName, errorMessage));
			}));
		}
		if (m_data.m_unlckstarlevel > PlayerDataModule.Instance.GetPlayerStarLevel())
		{
			m_notLvl.text = string.Format(Mod.Localization.GetInfoById(275), m_data.m_unlckstarlevel, m_data.m_unlocklevel);
		}
		else
		{
			m_notLvl.text = string.Format(Mod.Localization.GetInfoById(283), m_data.m_unlocklevel);
		}
		if (m_data.m_id == 1)
		{
			m_output.text = string.Format(Mod.Localization.GetInfoById(280), m_data.m_ifLock ? "???" : ((long)m_data.m_output).ToString());
			m_nextLvl.text = string.Format(Mod.Localization.GetInfoById(280), m_data.m_ifLock ? "???" : ((long)m_data.m_output).ToString());
		}
		if (m_back1 != null && m_back2 != null)
		{
			bool flag2 = m_data.m_id % 2 == 0;
			m_back1.gameObject.SetActive(flag2);
			m_back2.gameObject.SetActive(!flag2);
		}
	}

	public void OnBtnClick()
	{
		UpInstrumentResultState upInstrumentResultState = PlayerDataModule.Instance.UpInstrumentLevel(m_data.m_id, m_data.m_lvlup);
		switch (upInstrumentResultState)
		{
		case UpInstrumentResultState.SUCCESS:
			if ((bool)particles)
			{
				particles.Play();
			}
			Mod.Sound.PlayUISound(20014);
			m_musicForm.Refresh();
			break;
		case UpInstrumentResultState.LEVEL_REACH_MAX:
			if (!Mod.UI.UIFormIsOpen(UIFormId.BroadCastForm))
			{
				BroadCastData broadCastData = new BroadCastData();
				broadCastData.Type = BroadCastType.INFO;
				broadCastData.Info = string.Format(Mod.Localization.GetInfoById(279), m_data.m_instrumentCanUpMaxLevelShow.ToString());
				MonoSingleton<BroadCastManager>.Instacne.BroadCast(broadCastData);
				MonoSingleton<GameTools>.Instacne.EnableInput();
			}
			break;
		default:
			Debug.LogError(string.Concat("Not success:", upInstrumentResultState, " with Coin:", PlayerDataModule.Instance.GetPlayGoodsNum(3), " use: ", m_data.m_coin));
			break;
		}
	}

	public void OnNotBtnClick()
	{
		if (!Mod.UI.UIFormIsOpen(UIFormId.BroadCastForm))
		{
			BroadCastData broadCastData = new BroadCastData();
			broadCastData.Type = BroadCastType.INFO;
			broadCastData.Info = Mod.Localization.GetInfoById(294);
			MonoSingleton<BroadCastManager>.Instacne.BroadCast(broadCastData);
			MonoSingleton<GameTools>.Instacne.EnableInput();
		}
	}

	public void OnAdBtnClick()
	{
		CommonAlertData commonAlertData = new CommonAlertData();
		commonAlertData.showType = CommonAlertData.AlertShopType.COMMON;
		commonAlertData.alertContent = string.Format(Mod.Localization.GetInfoById(299).Replace("\\n", "\n"), m_data.m_desStr, m_data.m_lv + 1);
		commonAlertData.lableContent = Mod.Localization.GetInfoById(288);
		commonAlertData.cancelButtonText = Mod.Localization.GetInfoById(87);
		commonAlertData.needCancelButton = true;
		commonAlertData.cancelCallBackFunc = delegate
		{
			Mod.UI.CloseUIForm(UIFormId.CommonAlertForm);
		};
		commonAlertData.callBackFunc = delegate
		{
			Mod.UI.CloseUIForm(UIFormId.CommonAlertForm);
			MonoSingleton<GameTools>.Instacne.DisableInputForAWhile();
			MonoSingleton<GameTools>.Instacne.PlayVideoAdAndDisableInput(ADScene.MainView, delegate(ADScene adScen)
			{
				OnAdSuccess(adScen);
			});
		};
		Mod.UI.OpenUIForm(UIFormId.CommonAlertForm, commonAlertData);
		DateTime now = DateTime.Now;
		now.AddSeconds(GameCommon.showAdInInstrument);
		PlayerDataModule.Instance.SetMusicInstrumentAdTime(now);
		m_musicForm.Refresh();
		InfocUtils.Report_rollingsky2_games_ads(31, 0, 1, 0, 3, 0);
	}

	private void OnAdSuccess(ADScene adScen = ADScene.NONE)
	{
		PlayerDataModule.Instance.ForceUpInstrumentLevel(m_data.m_id);
		m_musicForm.Refresh();
		MonoSingleton<GameTools>.Instacne.EnableInput();
		InfocUtils.Report_rollingsky2_games_ads(31, 0, 1, 0, 4, 0);
	}

	private void OnDestroy()
	{
		for (int i = 0; i < loadedAsserts.Count; i++)
		{
			Mod.Resource.UnloadAsset(loadedAsserts[i]);
		}
		loadedAsserts.Clear();
	}

	private void Update()
	{
		if (base.transform.localPosition.y >= (float)m_stopEffectPosY && (bool)particles && particles.isPlaying)
		{
			particles.Stop();
		}
	}
}
