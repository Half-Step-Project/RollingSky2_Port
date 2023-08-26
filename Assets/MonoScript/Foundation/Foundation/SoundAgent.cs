using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace Foundation
{
	public sealed class SoundAgent : MonoBehaviour
	{
		private SoundGroup _group;

		private bool _muteInGroup;

		private float _volumeInGroup;

		private Transform _cachedTransform;

		private AudioSource _audioSource;

		private IHearable _hearable;

		private float _volumeWhenPause;

		private float _fadingTargetVolume;

		private bool _isFadingVolume;

		public int Id { get; internal set; }

		public SoundGroup Group
		{
			get
			{
				return _group;
			}
		}

		internal IHearable Hearable
		{
			get
			{
				return _hearable;
			}
			set
			{
				_hearable = value;
				if (_hearable != null)
				{
					UpdatePosition();
				}
				else
				{
					Recycle();
				}
			}
		}

		public bool IsPlaying
		{
			get
			{
				return _audioSource.isPlaying;
			}
		}

		public bool IsPaused { get; private set; }

		internal DateTime SetAssetTime { get; private set; }

		public float Time
		{
			get
			{
				return _audioSource.time;
			}
			set
			{
				_audioSource.time = value;
			}
		}

		internal bool Mute
		{
			get
			{
				return _audioSource.mute;
			}
			set
			{
				_audioSource.mute = value;
			}
		}

		internal bool Loop
		{
			get
			{
				return _audioSource.loop;
			}
			set
			{
				_audioSource.loop = value;
			}
		}

		internal int Priority
		{
			get
			{
				return 128 - _audioSource.priority;
			}
			set
			{
				_audioSource.priority = 128 - value;
			}
		}

		public float Volume
		{
			get
			{
				return _audioSource.volume;
			}
			set
			{
				_audioSource.volume = value;
			}
		}

		public float Pitch
		{
			get
			{
				return _audioSource.pitch;
			}
			set
			{
				_audioSource.pitch = value;
			}
		}

		public float PanStereo
		{
			get
			{
				return _audioSource.panStereo;
			}
			set
			{
				_audioSource.panStereo = value;
			}
		}

		public float SpatialBlend
		{
			get
			{
				return _audioSource.spatialBlend;
			}
			set
			{
				_audioSource.spatialBlend = value;
			}
		}

		public float MaxDistance
		{
			get
			{
				return _audioSource.maxDistance;
			}
			set
			{
				_audioSource.maxDistance = value;
			}
		}

		internal AudioMixerGroup MixerGroup
		{
			get
			{
				return _audioSource.outputAudioMixerGroup;
			}
			set
			{
				_audioSource.outputAudioMixerGroup = value;
			}
		}

		internal bool MuteInGroup
		{
			get
			{
				return _muteInGroup;
			}
			set
			{
				_muteInGroup = value;
				if (_audioSource != null)
				{
					_audioSource.mute = _group.Mute || _muteInGroup;
				}
			}
		}

		internal float VolumeInGroup
		{
			get
			{
				return _volumeInGroup;
			}
			set
			{
				if (!(Math.Abs(value - _volumeInGroup) <= float.Epsilon))
				{
					_volumeInGroup = value;
					if (_audioSource != null)
					{
						_audioSource.volume = _group.Volume * _volumeInGroup;
					}
				}
			}
		}

		internal object Clip
		{
			get
			{
				return _audioSource.clip;
			}
			set
			{
				AudioClip audioClip = value as AudioClip;
				if (!(audioClip == null))
				{
					Recycle();
					SetAssetTime = DateTime.Now;
					_audioSource.clip = audioClip;
				}
			}
		}

		internal void Init(SoundGroup group)
		{
			_audioSource = base.gameObject.GetOrAddComponent<AudioSource>();
			_audioSource.playOnAwake = false;
			_audioSource.rolloffMode = AudioRolloffMode.Custom;
			_cachedTransform = base.transform;
			Recycle();
			_group = group;
		}

		private void Recycle()
		{
			if (_audioSource.clip != null)
			{
				Mod.Resource.UnloadAsset(_audioSource.clip);
				_audioSource.clip = null;
			}
			SetAssetTime = DateTime.MinValue;
			Time = 0f;
			Mute = false;
			Loop = false;
			Priority = 0;
			Volume = 1f;
			Pitch = 1f;
			PanStereo = 0f;
			SpatialBlend = 0f;
			MaxDistance = 100f;
			_cachedTransform.localPosition = Vector3.zero;
			_hearable = null;
			_volumeWhenPause = 0f;
			_isFadingVolume = false;
		}

		internal void Play(float fadeInSeconds = 0f)
		{
			StopFadingVolume();
			_audioSource.Play();
			if (fadeInSeconds > 0f)
			{
				StartCoroutine(FadeToVolume(0f, _fadingTargetVolume, fadeInSeconds));
			}
		}

		internal void Stop(float fadeOutSeconds = 0f)
		{
			StopFadingVolume();
			if (fadeOutSeconds > 0f && base.gameObject.activeInHierarchy)
			{
				StartCoroutine(StopCo(fadeOutSeconds));
			}
			else
			{
				_audioSource.Stop();
			}
		}

		internal void Pause(float fadeOutSeconds = 0f)
		{
			StopFadingVolume();
			_volumeWhenPause = _audioSource.volume;
			IsPaused = true;
			if (fadeOutSeconds > 0f && base.gameObject.activeInHierarchy)
			{
				StartCoroutine(PauseCo(fadeOutSeconds));
			}
			else
			{
				_audioSource.Pause();
			}
		}

		internal void Resume(float fadeInSeconds = 0f)
		{
			StopFadingVolume();
			IsPaused = false;
			_audioSource.UnPause();
			if (fadeInSeconds > 0f)
			{
				StartCoroutine(FadeToVolume(_fadingTargetVolume, _volumeWhenPause, fadeInSeconds));
			}
			else
			{
				_audioSource.volume = _volumeWhenPause;
			}
		}

		private void Update()
		{
			if (!IsPaused && !IsPlaying && _audioSource.clip != null)
			{
				Recycle();
			}
			else if (_hearable != null)
			{
				UpdatePosition();
			}
		}

		private void UpdatePosition()
		{
			if (_hearable.ActiveAndEnabled)
			{
				_cachedTransform.position = _hearable.Position;
			}
			else
			{
				Recycle();
			}
		}

		private IEnumerator PauseCo(float fadeOutSeconds)
		{
			yield return FadeToVolume(_fadingTargetVolume, 0f, fadeOutSeconds);
			_audioSource.Pause();
		}

		private IEnumerator StopCo(float fadeOutSeconds)
		{
			yield return FadeToVolume(_fadingTargetVolume, 0f, fadeOutSeconds);
			_audioSource.Stop();
		}

		private IEnumerator FadeToVolume(float volume, float toVolume, float duration)
		{
			_isFadingVolume = true;
			float time = 0f;
			while (time < duration)
			{
				time += UnityEngine.Time.deltaTime;
				_audioSource.volume = Mathf.Lerp(volume, toVolume, time / duration);
				yield return null;
			}
			_audioSource.volume = toVolume;
			_isFadingVolume = false;
		}

		private void StopFadingVolume()
		{
			if (_isFadingVolume)
			{
				_isFadingVolume = false;
				_audioSource.volume = _fadingTargetVolume;
			}
			else
			{
				_fadingTargetVolume = _audioSource.volume;
			}
			StopAllCoroutines();
		}
	}
}
