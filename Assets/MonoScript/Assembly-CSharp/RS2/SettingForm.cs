using System;
using System.Collections.Generic;
using Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace RS2
{
	public sealed class SettingForm : UGUIForm
	{
		[SerializeField]
		private Toggle m_MusicMuteToggle;

		[SerializeField]
		private Slider m_MusicVolumeSlider;

		[SerializeField]
		private Toggle m_SoundMuteToggle;

		[SerializeField]
		private Slider m_SoundVolumeSlider;

		[SerializeField]
		private Toggle m_UISoundMuteToggle;

		[SerializeField]
		private Slider m_UISoundVolumeSlider;

		[SerializeField]
		private CanvasGroup m_LanguageTipsCanvasGroup;

		[SerializeField]
		private Toggle m_EnglishToggle;

		[SerializeField]
		private Toggle m_ChineseSimplifiedToggle;

		[SerializeField]
		private Toggle m_ChineseTraditionalToggle;

		[SerializeField]
		private Toggle m_KoreanToggle;

		private SystemLanguage m_SelectedLanguage;

		private List<UnityEngine.Object> loadedAsserts = new List<UnityEngine.Object>();

		public void OnMusicMuteChanged(bool isOn)
		{
			Mod.Sound.Mute("Music", !isOn);
			m_MusicVolumeSlider.gameObject.SetActive(isOn);
		}

		public void OnMusicVolumeChanged(float volume)
		{
			Mod.Sound.SetVolume("Music", volume);
		}

		public void OnSoundMuteChanged(bool isOn)
		{
			Mod.Sound.Mute("Sound", !isOn);
			m_SoundVolumeSlider.gameObject.SetActive(isOn);
		}

		public void OnSoundVolumeChanged(float volume)
		{
			Mod.Sound.SetVolume("Sound", volume);
		}

		public void OnUISoundMuteChanged(bool isOn)
		{
			Mod.Sound.Mute("UISound", !isOn);
			m_UISoundVolumeSlider.gameObject.SetActive(isOn);
		}

		public void OnUISoundVolumeChanged(float volume)
		{
			Mod.Sound.SetVolume("UISound", volume);
		}

		public void OnEnglishSelected(bool isOn)
		{
			if (isOn)
			{
				m_SelectedLanguage = SystemLanguage.English;
				RefreshLanguageTips();
			}
		}

		public void OnChineseSimplifiedSelected(bool isOn)
		{
			if (isOn)
			{
				m_SelectedLanguage = SystemLanguage.ChineseSimplified;
				RefreshLanguageTips();
			}
		}

		public void OnChineseTraditionalSelected(bool isOn)
		{
			if (isOn)
			{
				m_SelectedLanguage = SystemLanguage.ChineseTraditional;
				RefreshLanguageTips();
			}
		}

		public void OnKoreanSelected(bool isOn)
		{
			if (isOn)
			{
				m_SelectedLanguage = SystemLanguage.Korean;
				RefreshLanguageTips();
			}
		}

		public void OnSubmitButtonClick()
		{
			if (m_SelectedLanguage == Mod.Localization.Language)
			{
				Close();
				return;
			}
			Mod.Setting.SetString("Setting.Language", m_SelectedLanguage.ToString());
			Mod.Setting.Save();
			Mod.Sound.StopMusic();
			Mod.Shutdown();
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			m_MusicMuteToggle.isOn = !Mod.Sound.IsMuted("Music");
			m_MusicVolumeSlider.value = Mod.Sound.GetVolume("Music");
			m_SoundMuteToggle.isOn = !Mod.Sound.IsMuted("Sound");
			m_SoundVolumeSlider.value = Mod.Sound.GetVolume("Sound");
			m_UISoundMuteToggle.isOn = !Mod.Sound.IsMuted("UISound");
			m_UISoundVolumeSlider.value = Mod.Sound.GetVolume("UISound");
			m_SelectedLanguage = Mod.Localization.Language;
			switch (m_SelectedLanguage)
			{
			case SystemLanguage.English:
				m_EnglishToggle.isOn = true;
				break;
			case SystemLanguage.ChineseSimplified:
				m_ChineseSimplifiedToggle.isOn = true;
				break;
			case SystemLanguage.ChineseTraditional:
				m_ChineseTraditionalToggle.isOn = true;
				break;
			case SystemLanguage.Korean:
				m_KoreanToggle.isOn = true;
				break;
			}
		}

		protected override void OnTick(float elapseSeconds, float realElapseSeconds)
		{
			base.OnTick(elapseSeconds, realElapseSeconds);
			if (m_LanguageTipsCanvasGroup.gameObject.activeSelf)
			{
				m_LanguageTipsCanvasGroup.alpha = 0.5f + 0.5f * Mathf.Sin((float)Math.PI * Time.time);
			}
		}

		private void RefreshLanguageTips()
		{
			m_LanguageTipsCanvasGroup.gameObject.SetActive(m_SelectedLanguage != Mod.Localization.Language);
		}

		protected override void OnUnload()
		{
			base.OnUnload();
			for (int i = 0; i < loadedAsserts.Count; i++)
			{
				Mod.Resource.UnloadAsset(loadedAsserts[i]);
			}
		}
	}
}
