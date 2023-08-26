using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;

namespace Foundation
{
	[DisallowMultipleComponent]
	[AddComponentMenu("Framework/Sound")]
	public sealed class SoundMod : ModBase
	{
		public sealed class PlayDependencyEventArgs : EventArgs<PlayDependencyEventArgs>
		{
			public int SoundId { get; private set; }

			public string AssetName { get; private set; }

			public string GroupName { get; private set; }

			public PlaySoundParams PlaySoundParams { get; private set; }

			public string DependencyAssetName { get; private set; }

			public int LoadedCount { get; private set; }

			public int TotalCount { get; private set; }

			public object UserData { get; private set; }

			public static PlayDependencyEventArgs Make(int soundId, string assetName, string groupName, PlaySoundParams playSoundParams, string dependencyAssetName, int loadedCount, int totalCount, object userData)
			{
				PlayDependencyEventArgs playDependencyEventArgs = Mod.Reference.Acquire<PlayDependencyEventArgs>();
				playDependencyEventArgs.SoundId = soundId;
				playDependencyEventArgs.AssetName = assetName;
				playDependencyEventArgs.GroupName = groupName;
				playDependencyEventArgs.PlaySoundParams = playSoundParams;
				playDependencyEventArgs.DependencyAssetName = dependencyAssetName;
				playDependencyEventArgs.LoadedCount = loadedCount;
				playDependencyEventArgs.TotalCount = totalCount;
				playDependencyEventArgs.UserData = userData;
				return playDependencyEventArgs;
			}

			protected override void OnRecycle()
			{
				PlaySoundParams = null;
				UserData = null;
			}
		}

		public sealed class PlayFailureEventArgs : EventArgs<PlayFailureEventArgs>
		{
			public int SoundId { get; private set; }

			public string AssetName { get; private set; }

			public string GroupName { get; private set; }

			public PlaySoundParams PlaySoundParams { get; private set; }

			public string Message { get; private set; }

			public object UserData { get; private set; }

			public static PlayFailureEventArgs Make(int soundId, string assetName, string groupName, PlaySoundParams playSoundParams, string message, object userData)
			{
				PlayFailureEventArgs playFailureEventArgs = Mod.Reference.Acquire<PlayFailureEventArgs>();
				playFailureEventArgs.SoundId = soundId;
				playFailureEventArgs.AssetName = assetName;
				playFailureEventArgs.GroupName = groupName;
				playFailureEventArgs.PlaySoundParams = playSoundParams;
				playFailureEventArgs.Message = message;
				playFailureEventArgs.UserData = userData;
				return playFailureEventArgs;
			}

			protected override void OnRecycle()
			{
				PlaySoundParams = null;
				UserData = null;
			}
		}

		public sealed class PlaySuccessEventArgs : EventArgs<PlaySuccessEventArgs>
		{
			public int SoundId { get; private set; }

			public string AssetName { get; private set; }

			public SoundAgent SoundAgent { get; private set; }

			public IHearable Hearable { get; private set; }

			public float Duration { get; private set; }

			public object UserData { get; private set; }

			public static PlaySuccessEventArgs Make(int soundId, string assetName, SoundAgent soundAgent, float duration, IHearable hearable, object userData)
			{
				PlaySuccessEventArgs playSuccessEventArgs = Mod.Reference.Acquire<PlaySuccessEventArgs>();
				playSuccessEventArgs.SoundId = soundId;
				playSuccessEventArgs.AssetName = assetName;
				playSuccessEventArgs.SoundAgent = soundAgent;
				playSuccessEventArgs.Duration = duration;
				playSuccessEventArgs.Hearable = hearable;
				playSuccessEventArgs.UserData = userData;
				return playSuccessEventArgs;
			}

			protected override void OnRecycle()
			{
				SoundAgent = null;
				Hearable = null;
				UserData = null;
			}
		}

		public sealed class PlayUpdateEventArgs : EventArgs<PlayUpdateEventArgs>
		{
			public int SoundId { get; private set; }

			public string AssetName { get; private set; }

			public string GroupName { get; private set; }

			public PlaySoundParams PlaySoundParams { get; private set; }

			public float Progress { get; private set; }

			public object UserData { get; private set; }

			public static PlayUpdateEventArgs Make(int soundId, string assetName, string groupName, PlaySoundParams playSoundParams, float progress, object userData)
			{
				PlayUpdateEventArgs playUpdateEventArgs = Mod.Reference.Acquire<PlayUpdateEventArgs>();
				playUpdateEventArgs.SoundId = soundId;
				playUpdateEventArgs.AssetName = assetName;
				playUpdateEventArgs.GroupName = groupName;
				playUpdateEventArgs.PlaySoundParams = playSoundParams;
				playUpdateEventArgs.Progress = progress;
				playUpdateEventArgs.UserData = userData;
				return playUpdateEventArgs;
			}

			protected override void OnRecycle()
			{
				PlaySoundParams = null;
				UserData = null;
			}
		}

		private sealed class LoadData
		{
			[CompilerGenerated]
			private readonly int _003CId_003Ek__BackingField;

			[CompilerGenerated]
			private readonly SoundGroup _003CGroup_003Ek__BackingField;

			[CompilerGenerated]
			private readonly PlaySoundParams _003CParams_003Ek__BackingField;

			[CompilerGenerated]
			private readonly IHearable _003CHearable_003Ek__BackingField;

			[CompilerGenerated]
			private readonly object _003CUserData_003Ek__BackingField;

			public int Id
			{
				[CompilerGenerated]
				get
				{
					return _003CId_003Ek__BackingField;
				}
			}

			public SoundGroup Group
			{
				[CompilerGenerated]
				get
				{
					return _003CGroup_003Ek__BackingField;
				}
			}

			public PlaySoundParams Params
			{
				[CompilerGenerated]
				get
				{
					return _003CParams_003Ek__BackingField;
				}
			}

			public IHearable Hearable
			{
				[CompilerGenerated]
				get
				{
					return _003CHearable_003Ek__BackingField;
				}
			}

			public object UserData
			{
				[CompilerGenerated]
				get
				{
					return _003CUserData_003Ek__BackingField;
				}
			}

			public LoadData(int soundId, IHearable hearable, SoundGroup group, PlaySoundParams @params, object userData)
			{
				_003CId_003Ek__BackingField = soundId;
				_003CHearable_003Ek__BackingField = hearable;
				_003CGroup_003Ek__BackingField = group;
				_003CParams_003Ek__BackingField = @params;
				_003CUserData_003Ek__BackingField = userData;
			}
		}

		[Serializable]
		private sealed class GroupInfo
		{
			[SerializeField]
			private string _name;

			[SerializeField]
			private bool _avoidReplaced;

			[SerializeField]
			private bool _mute;

			[Range(0f, 1f)]
			[SerializeField]
			private float _volume;

			[SerializeField]
			private int _agentCount;

			public string Name
			{
				get
				{
					return _name;
				}
			}

			public bool AvoidReplaced
			{
				get
				{
					return _avoidReplaced;
				}
			}

			public bool Mute
			{
				get
				{
					return _mute;
				}
			}

			public float Volume
			{
				get
				{
					return _volume;
				}
			}

			public int AgentCount
			{
				get
				{
					return _agentCount;
				}
			}

			public GroupInfo()
			{
				_mute = false;
				_volume = 1f;
				_agentCount = 1;
			}
		}

		[SerializeField]
		private AudioListener _audioListener;

		[SerializeField]
		private AudioMixer _audioMixer;

		[SerializeField]
		private GroupInfo[] _groupInfos = new GroupInfo[0];

		public const int InvalidSoundId = -1;

		private readonly Dictionary<string, SoundGroup> _soundGroups = new Dictionary<string, SoundGroup>();

		private readonly List<int> _loadingSounds = new List<int>();

		private readonly HashSet<int> _unloadingSounds = new HashSet<int>();

		private AssetLoadCallbacks _loadCallbacks;

		private int _nextId;

		private AudioListener _defaultListener;

		public AudioMixer AudioMixer
		{
			get
			{
				return _audioMixer;
			}
		}

		public AudioListener AudioListener
		{
			get
			{
				if (_audioListener == null)
				{
					_audioListener = UnityEngine.Object.FindObjectOfType<AudioListener>();
					if (_audioListener == null)
					{
						_defaultListener.enabled = true;
						_audioListener = _defaultListener;
					}
				}
				return _audioListener;
			}
			set
			{
				if (_defaultListener != null)
				{
					_defaultListener.enabled = false;
				}
				_audioListener = value;
			}
		}

		public int SoundGroupCount
		{
			get
			{
				return _soundGroups.Count;
			}
		}

		public SoundGroup[] SoundGroups
		{
			get
			{
				int num = 0;
				SoundGroup[] array = new SoundGroup[_soundGroups.Count];
				foreach (KeyValuePair<string, SoundGroup> soundGroup in _soundGroups)
				{
					array[num++] = soundGroup.Value;
				}
				return array;
			}
		}

		public int[] LoadingSoundIds
		{
			get
			{
				return _loadingSounds.ToArray();
			}
		}

		public float SoundTime(int soundId, string groupName = null)
		{
			SoundGroup value;
			if (!string.IsNullOrEmpty(groupName) && _soundGroups.TryGetValue(groupName, out value) && value != null)
			{
				SoundAgent soundAgent = value.GetSoundAgent(soundId);
				if (soundAgent != null)
				{
					return soundAgent.Time;
				}
			}
			foreach (KeyValuePair<string, SoundGroup> soundGroup in _soundGroups)
			{
				SoundAgent soundAgent2 = soundGroup.Value.GetSoundAgent(soundId);
				if (soundAgent2 != null)
				{
					return soundAgent2.Time;
				}
			}
			return 0f;
		}

		public bool HasSoundGroup(string groupName)
		{
			if (string.IsNullOrEmpty(groupName))
			{
				Log.Warning("Sound group name is invalid.");
				return false;
			}
			return _soundGroups.ContainsKey(groupName);
		}

		public SoundGroup GetSoundGroup(string groupName)
		{
			if (string.IsNullOrEmpty(groupName))
			{
				Log.Warning("Sound group name is invalid.");
				return null;
			}
			SoundGroup value;
			if (!_soundGroups.TryGetValue(groupName, out value))
			{
				return null;
			}
			return value;
		}

		public bool IsLoadingSound(int soundId)
		{
			return _loadingSounds.Contains(soundId);
		}

		public int PlaySound(string assetName, string groupName, object userData)
		{
			return PlaySound(assetName, groupName, null, null, userData);
		}

		public int PlaySound(string assetName, string groupName, PlaySoundParams playParams, object userData)
		{
			return PlaySound(assetName, groupName, playParams, null, userData);
		}

		public int PlaySound(string assetName, string groupName, PlaySoundParams playParams, IHearable hearable, object userData)
		{
			if (playParams == null)
			{
				playParams = new PlaySoundParams();
			}
			int num = _nextId++;
			string text = null;
			SoundGroup soundGroup = GetSoundGroup(groupName);
			if (soundGroup == null)
			{
				text = "Sound group '" + groupName + "' is not exist.";
			}
			else if (soundGroup.AgentCount <= 0)
			{
				text = "Sound group '" + groupName + "' is have no sound agent.";
			}
			if (text != null)
			{
				Log.Error(text);
				return -1;
			}
			_loadingSounds.Add(num);
			Mod.Resource.LoadAsset(assetName, _loadCallbacks, new LoadData(num, hearable, soundGroup, playParams, userData));
			return num;
		}

		public float StopSound(int soundId, float fadeOutSeconds = 0f)
		{
			if (IsLoadingSound(soundId))
			{
				_unloadingSounds.Add(soundId);
				return 0f;
			}
			foreach (KeyValuePair<string, SoundGroup> soundGroup in _soundGroups)
			{
				float time;
				if (soundGroup.Value.StopSound(soundId, fadeOutSeconds, out time))
				{
					return time;
				}
			}
			return 0f;
		}

		public void StopLoadedSounds(float fadeOutSeconds = 0f)
		{
			foreach (KeyValuePair<string, SoundGroup> soundGroup in _soundGroups)
			{
				soundGroup.Value.StopLoadedSounds(fadeOutSeconds);
			}
		}

		public void StopLoadingSounds()
		{
			for (int i = 0; i < _loadingSounds.Count; i++)
			{
				_unloadingSounds.Add(_loadingSounds[i]);
			}
		}

		public float PauseSound(int soundId, float fadeOutSeconds = 0f)
		{
			foreach (KeyValuePair<string, SoundGroup> soundGroup in _soundGroups)
			{
				float time;
				if (soundGroup.Value.PauseSound(soundId, fadeOutSeconds, out time))
				{
					return time;
				}
			}
			Log.Warning("Can not find sound '" + soundId + "'.");
			return 0f;
		}

		public void ResumeSound(int soundId, float fadeInSeconds = 0f)
		{
			foreach (KeyValuePair<string, SoundGroup> soundGroup in _soundGroups)
			{
				if (soundGroup.Value.ResumeSound(soundId, fadeInSeconds))
				{
					return;
				}
			}
			Log.Warning("Can not find sound '" + soundId + "'.");
		}

		protected override void Awake()
		{
			Mod.Sound = this;
		}

		internal override void OnInit()
		{
			base.OnInit();
			_loadCallbacks = new AssetLoadCallbacks(OnLoadSuccess, OnLoadFailure, OnLoadUpdate, OnLoadDependencyAsset);
			_defaultListener = base.gameObject.GetOrAddComponent<AudioListener>();
			_defaultListener.hideFlags = HideFlags.DontSave;
			for (int i = 0; i < _groupInfos.Length; i++)
			{
				GroupInfo groupInfo = _groupInfos[i];
				if (string.IsNullOrEmpty(groupInfo.Name))
				{
					Log.Warning("Sound group name is invalid.");
					continue;
				}
				GameObject gameObject = new GameObject("Sound Group - " + groupInfo.Name);
				gameObject.transform.parent = base.transform;
				gameObject.hideFlags |= HideFlags.DontSave;
				gameObject.hideFlags |= HideFlags.NotEditable;
				SoundGroup orAddComponent = gameObject.GetOrAddComponent<SoundGroup>();
				orAddComponent.hideFlags |= HideFlags.DontSave;
				orAddComponent.hideFlags |= HideFlags.NotEditable;
				AudioMixerGroup mixerGroup = null;
				if (_audioMixer != null)
				{
					AudioMixerGroup[] array = _audioMixer.FindMatchingGroups("Master/" + groupInfo.Name);
					mixerGroup = ((array.Length != 0) ? array[0] : _audioMixer.FindMatchingGroups("Master")[0]);
				}
				if (!AddSoundGroup(orAddComponent, groupInfo.Name, groupInfo.AvoidReplaced, groupInfo.Mute, groupInfo.Volume, groupInfo.AgentCount, mixerGroup))
				{
					Log.Warning("Add sound group '" + groupInfo.Name + "' failure.");
				}
			}
			Mod.Event.Subscribe(EventArgs<SceneMod.LoadSuccessEventArgs>.EventId, OnSceneLoadSuccess);
		}

		internal override void OnTick(float elapseSeconds, float realElapseSeconds)
		{
		}

		internal override void OnExit()
		{
			Mod.Event.Unsubscribe(EventArgs<SceneMod.LoadSuccessEventArgs>.EventId, OnSceneLoadSuccess);
			StopLoadedSounds();
			_soundGroups.Clear();
			_loadingSounds.Clear();
			_unloadingSounds.Clear();
			int childCount = base.transform.childCount;
			GameObject[] array = new GameObject[childCount];
			for (int i = 0; i < childCount; i++)
			{
				array[i] = base.transform.GetChild(i).gameObject;
			}
			for (int j = 0; j < array.Length; j++)
			{
				if (Application.isEditor)
				{
					UnityEngine.Object.DestroyImmediate(array[j]);
				}
				else
				{
					UnityEngine.Object.Destroy(array[j]);
				}
			}
		}

		private bool AddSoundGroup(SoundGroup group, string groupName, bool avoidReplaced, bool mute, float volume, int agentCount, AudioMixerGroup mixerGroup)
		{
			if (group == null)
			{
				Log.Warning("Sound group is invalid.");
				return false;
			}
			if (string.IsNullOrEmpty(groupName))
			{
				Log.Warning("Sound group name is invalid.");
				return false;
			}
			if (agentCount < 1)
			{
				Log.Warning("Sound agent count less then 1");
				return false;
			}
			if (HasSoundGroup(groupName))
			{
				return false;
			}
			group.Init(groupName, mixerGroup, agentCount);
			group.AvoidReplacedBySamePriority = avoidReplaced;
			group.Mute = mute;
			group.Volume = volume;
			_soundGroups.Add(groupName, group);
			return true;
		}

		private void OnLoadSuccess(string assetName, object asset, float duration, object userData)
		{
			LoadData loadData = (LoadData)userData;
			_loadingSounds.Remove(loadData.Id);
			if (_unloadingSounds.Remove(loadData.Id))
			{
				Log.Info("Release sound '" + loadData.Id + "' on loading success.");
				Mod.Resource.UnloadAsset((UnityEngine.Object)asset);
				return;
			}
			SoundAgent soundAgent = loadData.Group.PlaySound(loadData.Id, asset, loadData.Params);
			if (soundAgent != null)
			{
				if (loadData.Hearable != null)
				{
					soundAgent.Hearable = loadData.Hearable;
				}
				PlaySuccessEventArgs args = PlaySuccessEventArgs.Make(loadData.Id, assetName, soundAgent, duration, loadData.Hearable, loadData.UserData);
				Mod.Event.Fire(this, args);
			}
			else
			{
				Mod.Resource.UnloadAsset((UnityEngine.Object)asset);
				string message = "Sound group '" + loadData.Group.Name + "' play sound '" + assetName + "' failure.";
				PlayFailureEventArgs args2 = PlayFailureEventArgs.Make(loadData.Id, assetName, loadData.Group.Name, loadData.Params, message, loadData.UserData);
				Mod.Event.Fire(this, args2);
			}
		}

		private void OnLoadFailure(string assetName, string message, object userData)
		{
			LoadData loadData = (LoadData)userData;
			_loadingSounds.Remove(loadData.Id);
			_unloadingSounds.Remove(loadData.Id);
			PlayFailureEventArgs args = PlayFailureEventArgs.Make(loadData.Id, assetName, loadData.Group.Name, loadData.Params, message, loadData.UserData);
			Mod.Event.Fire(this, args);
		}

		private void OnLoadUpdate(string assetName, float progress, object userData)
		{
			LoadData loadData = (LoadData)userData;
			PlayUpdateEventArgs args = PlayUpdateEventArgs.Make(loadData.Id, assetName, loadData.Group.Name, loadData.Params, progress, loadData.UserData);
			Mod.Event.Fire(this, args);
		}

		private void OnLoadDependencyAsset(string assetName, string dependencyAssetName, int loadedCount, int totalCount, object userData)
		{
			LoadData loadData = (LoadData)userData;
			PlayDependencyEventArgs args = PlayDependencyEventArgs.Make(loadData.Id, assetName, loadData.Group.Name, loadData.Params, dependencyAssetName, loadedCount, totalCount, loadData.UserData);
			Mod.Event.Fire(this, args);
		}

		private void OnSceneLoadSuccess(object sender, EventArgs args)
		{
			AudioListener audioListener = UnityEngine.Object.FindObjectOfType<AudioListener>();
			if (audioListener != null)
			{
				AudioListener = audioListener;
			}
		}
	}
}
