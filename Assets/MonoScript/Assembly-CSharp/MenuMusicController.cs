using System.Collections;
using Foundation;
using RS2;
using UnityEngine;

public class MenuMusicController : Singleton<MenuMusicController>, IOriginRebirth
{
	public static readonly string MusicGroupName = "Music";

	public bool IfEnable = true;

	public int CurrentPlayedLevelId = -1;

	public int CurrentPlayedMusicId = -1;

	private float cachedGameTime;

	private bool IsMusicOn
	{
		get
		{
			return Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).IsMusicPlayOn();
		}
	}

	public bool IsRecordOriginRebirth
	{
		get
		{
			return true;
		}
	}

	public void Init()
	{
		SetMute(!IsMusicOn);
	}

	public void PlayMenuMusic(int musicId, float skipTime)
	{
		if (IfEnable)
		{
			Mod.Sound.PlayMusic(musicId, MusicGroupName, skipTime);
		}
	}

	public void StopMenuMusic()
	{
		if (IfEnable)
		{
			Mod.Sound.StopMusic();
		}
	}

	public void PauseMenuMusic()
	{
		if (IfEnable)
		{
			Mod.Sound.PauseMusic();
		}
	}

	public void ResumeMenuMusic()
	{
		if (IfEnable)
		{
			Mod.Sound.ResumeMusic();
		}
	}

	public void SetMute(bool ifMute)
	{
		Mod.Sound.Mute(MusicGroupName, ifMute);
	}

	public void SetVolume(float volume)
	{
		Mod.Sound.SetVolume(MusicGroupName, volume);
	}

	private void ChangeGameVolumeTo(float volume)
	{
		Mod.Sound.ChangeToVolume(MusicGroupName, volume);
	}

	public bool IfSoundOn()
	{
		return Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).IsSoundPlayOn();
	}

	public void PlayGameMusic(int levelId, float time = 0f)
	{
		if (IsMusicOn)
		{
			CurrentPlayedLevelId = levelId;
			Levels_levelTable levels_levelTable = Mod.DataTable.Get<Levels_levelTable>()[levelId];
			if (levels_levelTable != null)
			{
				int musicId = levels_levelTable.MusicId;
				int? num = Mod.Sound.PlayMusic(musicId, MusicGroupName, time, false);
				CurrentPlayedMusicId = (num.HasValue ? num.Value : (-1));
			}
			ChangeGameVolumeTo(1f);
		}
		Mod.Event.Subscribe(EventArgs<BackMusicFadeOutEventArgs>.EventId, OnBackMusicFadeOut);
		Log.Info("bc ==> Play Game Music:" + levelId);
	}

	public void StopGameMusic()
	{
		if (IsMusicOn && CurrentPlayedMusicId >= 0)
		{
			Mod.Sound.StopMusic();
			ChangeGameVolumeTo(1f);
		}
		Log.Info("bc ==> Stop Game Music");
		CoroutineManager.DestroyCoroutine(CoroutineManagerType.AUDIO);
		Mod.Event.Unsubscribe(EventArgs<BackMusicFadeOutEventArgs>.EventId, OnBackMusicFadeOut);
	}

	private void OnBackMusicFadeOut(object sender, EventArgs e)
	{
		BackMusicFadeOutEventArgs backMusicFadeOutEventArgs = e as BackMusicFadeOutEventArgs;
		if (backMusicFadeOutEventArgs != null && IsMusicOn && CurrentPlayedMusicId >= 0)
		{
			CoroutineManager.DestroyCoroutine(CoroutineManagerType.AUDIO);
			CoroutineManager.CreateCoroutine(CoroutineManagerType.AUDIO, FadeMusic(backMusicFadeOutEventArgs.DelayTime, backMusicFadeOutEventArgs.FadeOutTime, backMusicFadeOutEventArgs.BeginVolume, backMusicFadeOutEventArgs.EndVolume));
		}
	}

	private IEnumerator FadeMusic(float delayTime, float fadeTime, float beginVol, float endVol)
	{
		float fadeTimeCounter = 0f;
		yield return new WaitForSeconds(delayTime);
		Log.Info("bc ==> Begin Fade Music");
		do
		{
			fadeTimeCounter += Time.deltaTime;
			ChangeGameVolumeTo(Mathf.Lerp(beginVol, endVol, fadeTimeCounter / fadeTime));
			yield return null;
		}
		while (fadeTimeCounter <= fadeTime);
		ChangeGameVolumeTo(endVol);
	}

	public void PauseGameMusic()
	{
		if (IsMusicOn)
		{
			Mod.Sound.PauseMusic();
		}
	}

	public void ResumeGameMusic()
	{
		if (IsMusicOn)
		{
			Mod.Sound.ResumeMusic();
		}
	}

	public void ResumeGameMusic(float time)
	{
		if (IsMusicOn)
		{
			Levels_levelTable levels_levelTable = Mod.DataTable.Get<Levels_levelTable>()[CurrentPlayedLevelId];
			if (levels_levelTable != null)
			{
				int musicId = levels_levelTable.MusicId;
				int? num = Mod.Sound.PlayMusic(musicId, MusicGroupName, time, false);
				CurrentPlayedMusicId = (num.HasValue ? num.Value : (-1));
			}
			ChangeGameVolumeTo(1f);
		}
	}

	private void GetAudioResourceByLevel(int levelId)
	{
	}

	public float GetTime()
	{
		return Mod.Sound.GetMusicTime();
	}

	public object GetOriginRebirthData(object obj = null)
	{
		return JsonUtility.ToJson(new RD_OpAudioPlayerMix_Data
		{
			m_musicTime = GetTime()
		});
	}

	public void SetOriginRebirthData(object dataInfo)
	{
		RD_OpAudioPlayerMix_Data rD_OpAudioPlayerMix_Data = JsonUtility.FromJson<RD_OpAudioPlayerMix_Data>(dataInfo as string);
		cachedGameTime = rD_OpAudioPlayerMix_Data.m_musicTime;
	}

	public void StartRunByOriginRebirthData(object dataInfo)
	{
		PlayGameMusic(CurrentPlayedLevelId, cachedGameTime);
	}

	public byte[] GetOriginRebirthBsonData(object obj = null)
	{
		return Bson.ToBson(new RD_OpAudioPlayerMix_Data
		{
			m_musicTime = GetTime()
		});
	}

	public void SetOriginRebirthBsonData(byte[] dataInfo)
	{
		RD_OpAudioPlayerMix_Data rD_OpAudioPlayerMix_Data = Bson.ToObject<RD_OpAudioPlayerMix_Data>(dataInfo);
		cachedGameTime = rD_OpAudioPlayerMix_Data.m_musicTime;
	}

	public void StartRunByOriginRebirthBsonData(byte[] dataInfo)
	{
		PlayGameMusic(CurrentPlayedLevelId, cachedGameTime);
	}
}
