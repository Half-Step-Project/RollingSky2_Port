using Foundation;

namespace RS2
{
	public static class SoundExtension
	{
		private const float FadeVolumeDuration = 1f;

		private static int? _musicSerialId;

		private static string[] levelMusicName = new string[12]
		{
			"Fantasia_Jazz", "Fantasia_Waltz", "Fate_Fate", "Home_Homeland", "Home_Rainbow", "Pharaohs_Escape", "Pharaohs_Pharaohs", "Samurai", "StarryDream_Mysterious", "StarryDream_StarryDream",
			"Thief_Thief", "Thief_Thief_2"
		};

		public static int? PlayMusic(this SoundMod sound, int musicId, object userData = null, float time = 0f, bool ifLoop = true)
		{
			sound.StopMusic();
			Music_levelMusicTable music_levelMusicTable = Mod.DataTable.Get<Music_levelMusicTable>()[musicId];
			if (music_levelMusicTable == null)
			{
				Log.Warning(string.Format("Can not load music '{0}' from data table.", musicId.ToString()));
				return null;
			}
			string groupName = (string)userData;
			SoundGroup soundGroup = Mod.Sound.GetSoundGroup(groupName);
			PlaySoundParams playParams = new PlaySoundParams
			{
				Priority = 64,
				Loop = ifLoop,
				VolumeInGroup = 1f,
				FadeInSeconds = 1f,
				SpatialBlend = 0f,
				Time = time,
				MuteInGroup = soundGroup.Mute
			};
			string text = "";
			text = string.Format("Assets/_RS2Art/Res/Base/Music/{0}.mp3", music_levelMusicTable.ResourceName);
			for (int i = 0; i < levelMusicName.Length; i++)
			{
				if (music_levelMusicTable.ResourceName == levelMusicName[i])
				{
					text = string.Format("Assets/_RS2Art/Res/Base/Music/{0}.wav", music_levelMusicTable.ResourceName);
					break;
				}
			}
			_musicSerialId = sound.PlaySound(text, "Music", playParams, null, userData);
			return _musicSerialId;
		}

		public static void StopMusic(this SoundMod sound)
		{
			if (_musicSerialId.HasValue)
			{
				sound.StopSound(_musicSerialId.Value, 1f);
				_musicSerialId = null;
			}
		}

		public static void PauseMusic(this SoundMod sound)
		{
			if (_musicSerialId.HasValue)
			{
				sound.PauseSound(_musicSerialId.Value);
			}
		}

		public static void ResumeMusic(this SoundMod sound)
		{
			if (_musicSerialId.HasValue)
			{
				sound.ResumeSound(_musicSerialId.Value);
			}
		}

		public static float GetMusicTime(this SoundMod sound)
		{
			if (!_musicSerialId.HasValue)
			{
				return 0f;
			}
			return sound.SoundTime(_musicSerialId.Value, "Music");
		}

		public static int? PlaySound(this SoundMod sound, int soundId, BaseElement bindingElement = null, object userData = null)
		{
			PlaySoundParams playParams = new PlaySoundParams
			{
				Priority = 64,
				Loop = false,
				VolumeInGroup = 1f,
				SpatialBlend = 0f
			};
			return sound.PlaySound("Assets/_RS2Art/Art/Levels/LevelOther/Music/Audio/Sound_Crown.mp3", "Sound", playParams, bindingElement, userData);
		}

		public static int? PlayUISound(this SoundMod sound, int uiSoundId, object userData = null)
		{
			if (!Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).IsSoundPlayOn())
			{
				return -1;
			}
			UISound_uiSoundTable uISound_uiSoundTable = Mod.DataTable.Get<UISound_uiSoundTable>()[uiSoundId];
			if (uISound_uiSoundTable != null)
			{
				PlaySoundParams playParams = new PlaySoundParams
				{
					Priority = uISound_uiSoundTable.Priority,
					Loop = false,
					VolumeInGroup = uISound_uiSoundTable.Volume,
					SpatialBlend = 0f
				};
				string uISoundAsset = AssetUtility.GetUISoundAsset(uISound_uiSoundTable.ResourceName);
				return sound.PlaySound(uISoundAsset, "Sound", playParams, userData);
			}
			return -1;
		}

		public static int? PlayUISound(this SoundMod sound, int uiSoundId, SoundGroupName group, float time = 0f, object userData = null)
		{
			if (!Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).IsSoundPlayOn())
			{
				return -1;
			}
			UISound_uiSoundTable uISound_uiSoundTable = Mod.DataTable.Get<UISound_uiSoundTable>()[uiSoundId];
			if (uISound_uiSoundTable != null)
			{
				PlaySoundParams playParams = new PlaySoundParams
				{
					Priority = uISound_uiSoundTable.Priority,
					Loop = false,
					VolumeInGroup = uISound_uiSoundTable.Volume,
					SpatialBlend = 0f,
					Time = time
				};
				string uISoundAsset = AssetUtility.GetUISoundAsset(uISound_uiSoundTable.ResourceName);
				return sound.PlaySound(uISoundAsset, group.ToString(), playParams, userData);
			}
			return -1;
		}

		public static bool IsMuted(this SoundMod sound, string soundGroupName)
		{
			if (string.IsNullOrEmpty(soundGroupName))
			{
				Log.Warning("Sound group is invalid.");
				return true;
			}
			SoundGroup soundGroup = sound.GetSoundGroup(soundGroupName);
			if (soundGroup == null)
			{
				Log.Warning(string.Format("Sound group '{0}' is invalid.", soundGroupName));
				return true;
			}
			return soundGroup.Mute;
		}

		public static void Mute(this SoundMod sound, string soundGroupName, bool mute)
		{
			if (string.IsNullOrEmpty(soundGroupName))
			{
				Log.Warning("Sound group is invalid.");
				return;
			}
			SoundGroup soundGroup = sound.GetSoundGroup(soundGroupName);
			if (soundGroup == null)
			{
				Log.Warning(string.Format("Sound group '{0}' is invalid.", soundGroupName));
				return;
			}
			soundGroup.Mute = mute;
			Mod.Setting.Save();
		}

		public static float GetVolume(this SoundMod sound, string soundGroupName)
		{
			if (string.IsNullOrEmpty(soundGroupName))
			{
				Log.Warning("Sound group is invalid.");
				return 0f;
			}
			SoundGroup soundGroup = sound.GetSoundGroup(soundGroupName);
			if (soundGroup == null)
			{
				Log.Warning(string.Format("Sound group '{0}' is invalid.", soundGroupName));
				return 0f;
			}
			return soundGroup.Volume;
		}

		public static void SetVolume(this SoundMod sound, string soundGroupName, float volume)
		{
			Mod.Setting.Save();
		}

		public static void ChangeToVolume(this SoundMod sound, string soundGroupName, float volume)
		{
			SoundGroup soundGroup = sound.GetSoundGroup(soundGroupName);
			if (soundGroup == null)
			{
				Log.Warning("Sound group '{0}' is invalid.", soundGroupName);
			}
			else
			{
				soundGroup.Volume = volume;
			}
		}
	}
}
