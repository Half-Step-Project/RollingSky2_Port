using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Foundation
{
	[ExecuteInEditMode]
	public sealed class SoundGroup : MonoBehaviour
	{
		[SerializeField]
		private AudioMixerGroup _mixerGroup;

		private List<SoundAgent> _agents;

		private bool _mute;

		private float _volume;

		public string Name { get; private set; }

		public int AgentCount
		{
			get
			{
				return _agents.Count;
			}
		}

		public bool AvoidReplacedBySamePriority { get; set; }

		public AudioMixerGroup MixerGroup
		{
			get
			{
				return _mixerGroup;
			}
		}

		public bool Mute
		{
			get
			{
				return _mute;
			}
			set
			{
				if (value == _mute)
				{
					return;
				}
				_mute = value;
				foreach (SoundAgent agent in _agents)
				{
					agent.MuteInGroup = _mute;
				}
			}
		}

		public float Volume
		{
			get
			{
				return _volume;
			}
			set
			{
				if (Math.Abs(value - _volume) <= float.Epsilon)
				{
					return;
				}
				_volume = value;
				foreach (SoundAgent agent in _agents)
				{
					agent.VolumeInGroup = _volume;
				}
			}
		}

		internal void Init(string name, AudioMixerGroup mixerGroup, int agentCount)
		{
			if (string.IsNullOrEmpty(name))
			{
				Log.Error("Sound group name is invalid.");
				return;
			}
			if (agentCount < 1)
			{
				Log.Error("Agent count needs least one.");
				return;
			}
			Name = name;
			_agents = new List<SoundAgent>(agentCount);
			for (int i = 0; i < agentCount; i++)
			{
				GameObject gameObject = new GameObject("Sound Agent");
				gameObject.transform.parent = base.transform;
				gameObject.hideFlags |= HideFlags.DontSave;
				gameObject.hideFlags |= HideFlags.NotEditable;
				SoundAgent orAddComponent = gameObject.GetOrAddComponent<SoundAgent>();
				orAddComponent.hideFlags |= HideFlags.DontSave;
				orAddComponent.hideFlags |= HideFlags.NotEditable;
				orAddComponent.Init(this);
				_agents.Add(orAddComponent);
			}
			_mixerGroup = mixerGroup;
		}

		internal SoundAgent GetSoundAgent(int id)
		{
			for (int i = 0; i < _agents.Count; i++)
			{
				SoundAgent soundAgent = _agents[i];
				if (soundAgent.Id == id)
				{
					return soundAgent;
				}
			}
			return null;
		}

		internal SoundAgent PlaySound(int id, object asset, PlaySoundParams playParams)
		{
			SoundAgent soundAgent = null;
			for (int i = 0; i < _agents.Count; i++)
			{
				SoundAgent soundAgent2 = _agents[i];
				if (!soundAgent2.IsPlaying)
				{
					soundAgent = soundAgent2;
					break;
				}
				if (soundAgent2.Priority < playParams.Priority)
				{
					if (soundAgent == null || soundAgent2.Priority < soundAgent.Priority)
					{
						soundAgent = soundAgent2;
					}
				}
				else if ((AvoidReplacedBySamePriority || soundAgent2.Priority == playParams.Priority) && (soundAgent == null || soundAgent2.SetAssetTime < soundAgent.SetAssetTime))
				{
					soundAgent = soundAgent2;
				}
			}
			if (soundAgent == null)
			{
				return null;
			}
			soundAgent.Clip = asset;
			soundAgent.Id = id;
			soundAgent.Time = playParams.Time;
			soundAgent.MuteInGroup = playParams.MuteInGroup;
			soundAgent.Loop = playParams.Loop;
			soundAgent.Priority = playParams.Priority;
			soundAgent.VolumeInGroup = playParams.VolumeInGroup;
			soundAgent.Pitch = playParams.Pitch;
			soundAgent.PanStereo = playParams.PanStereo;
			soundAgent.SpatialBlend = playParams.SpatialBlend;
			soundAgent.MaxDistance = playParams.MaxDistance;
			soundAgent.Play(playParams.FadeInSeconds);
			return soundAgent;
		}

		internal bool StopSound(int id, float fadeOutSeconds, out float time)
		{
			foreach (SoundAgent agent in _agents)
			{
				if (agent.Id == id)
				{
					agent.Stop(fadeOutSeconds);
					time = agent.Time;
					return true;
				}
			}
			time = 0f;
			return false;
		}

		internal bool PauseSound(int id, float fadeOutSeconds, out float time)
		{
			foreach (SoundAgent agent in _agents)
			{
				if (agent.Id == id)
				{
					agent.Pause(fadeOutSeconds);
					time = agent.Time;
					return true;
				}
			}
			time = 0f;
			return false;
		}

		internal bool ResumeSound(int id, float fadeInSeconds)
		{
			foreach (SoundAgent agent in _agents)
			{
				if (agent.Id == id)
				{
					agent.Resume(fadeInSeconds);
					return true;
				}
			}
			return false;
		}

		public void StopLoadedSounds()
		{
			foreach (SoundAgent agent in _agents)
			{
				if (agent.IsPlaying)
				{
					agent.Stop();
				}
			}
		}

		public void StopLoadedSounds(float fadeOutSeconds)
		{
			foreach (SoundAgent agent in _agents)
			{
				if (agent.IsPlaying)
				{
					agent.Stop(fadeOutSeconds);
				}
			}
		}
	}
}
