using System;
using Foundation;
using RisingWin.Library;
using UnityEngine;

namespace RS2
{
	public sealed class LaunchProcedure : BaseProcedure
	{
		private sealed class BuiltinInfo
		{
			public string VersionUrl { get; set; }
		}

		private const string DictionaryTexPath = "Builtin/Dictionary";

		private Font _defaultFont;

		protected override void OnEnter(IFsm<ProcedureMod> procedureOwner)
		{
			base.OnEnter(procedureOwner);
			PlayerPrefsManager.DoInitializationIfNeeded();
			InputAdaptor instance = MonoBehaviorSingleton<InputAdaptor>.Instance;
			InitLanguage();
			InitDictionary();
			InitQuality();
			InitVariant();
			InitSound();
			InitRemoteResourceFolder();
		}

		protected override void OnTick(IFsm<ProcedureMod> procedureOwner, float elapseSeconds, float realElapseSeconds)
		{
			base.OnTick(procedureOwner, elapseSeconds, realElapseSeconds);
			ChangeState(procedureOwner, Mod.Core.EditorMode ? typeof(PreloadProcedure) : typeof(UpdateProcedure));
		}

		protected override void OnLeave(IFsm<ProcedureMod> procedureOwner, bool isShutdown)
		{
			Mod.Resource.UnloadUnusedAssets();
			base.OnLeave(procedureOwner, isShutdown);
		}

		private void InitLanguage()
		{
			if (Mod.Core.EditorMode && Mod.Core.EditorLanguage != SystemLanguage.Unknown)
			{
				return;
			}
			int @int = PlayerPrefsAdapter.GetInt(PlayerLocalDatakey.SETTING_LANGUAGE);
			if (@int == 0)
			{
				SystemLanguage systemLanguage = Mod.Localization.Language;
				string @string = Mod.Setting.GetString("Setting.Language");
				if (!string.IsNullOrEmpty(@string))
				{
					try
					{
						systemLanguage = (SystemLanguage)Enum.Parse(typeof(SystemLanguage), @string);
					}
					catch
					{
					}
				}
				if (systemLanguage != SystemLanguage.English && systemLanguage != SystemLanguage.ChineseSimplified)
				{
					systemLanguage = SystemLanguage.English;
					Mod.Setting.SetString("Setting.Language", systemLanguage.ToString());
					Mod.Setting.Save();
				}
				Mod.Localization.Language = systemLanguage;
			}
			else
			{
				SystemLanguage language = (SystemLanguage)@int;
				Mod.Localization.Language = language;
			}
		}

		private void InitDictionary()
		{
			TextAsset textAsset = null;
			try
			{
				textAsset = Resources.Load<TextAsset>("Builtin/Dictionary");
				if (textAsset == null)
				{
					throw new Exception("Default dictionary can not be found.");
				}
				if (string.IsNullOrEmpty(textAsset.text))
				{
					throw new Exception("Default dictionary is empty.");
				}
				Mod.Localization.Parse(textAsset.text);
			}
			finally
			{
				if (textAsset != null)
				{
					Resources.UnloadAsset(textAsset);
				}
			}
		}

		private void InitQuality()
		{
			DeviceManager.Instance.Init();
			int @int = Mod.Setting.GetInt("Setting.QualityLevel", -1);
			if (@int == -1)
			{
				Mod.Setting.SetInt("Setting.QualityLevel", (int)DeviceManager.Instance.QualityLevel);
			}
			else
			{
				DeviceManager.Instance.SetGameQuality((DeviceQuality)@int);
			}
		}

		private void InitVariant()
		{
			if (!Mod.Core.EditorMode)
			{
				SystemLanguage language = Mod.Localization.Language;
				string variant = ((language != SystemLanguage.Chinese && (uint)(language - 40) > 1u) ? "english" : "chinese");
				Mod.Resource.Variant = variant;
			}
		}

		private void InitSound()
		{
			if (Mod.Setting.GetInt("Setting.SoundMuted", -1) == -1)
			{
				Mod.Setting.SetInt("Setting.SoundMuted", 1);
			}
			if (Mod.Setting.GetInt("Setting.MusicMuted", -1) == -1)
			{
				Mod.Setting.SetInt("Setting.MusicMuted", 1);
			}
		}

		private void InitRemoteResourceFolder()
		{
			if (!Mod.Core.EditorMode)
			{
				string remoteResourceFolder = "";
				Mod.Resource.SetRemoteResourceFolder(remoteResourceFolder);
			}
		}
	}
}
