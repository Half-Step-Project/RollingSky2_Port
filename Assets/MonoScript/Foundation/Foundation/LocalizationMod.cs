using System;
using System.Collections.Generic;
using System.Security;
using Mono.Xml;
using UnityEngine;

namespace Foundation
{
	[AddComponentMenu("Framework/Localization")]
	[DisallowMultipleComponent]
	public sealed class LocalizationMod : ModBase
	{
		public sealed class LoadDependencyEventArgs : EventArgs<LoadDependencyEventArgs>
		{
			public string AssetName { get; private set; }

			public string DependencyAssetName { get; private set; }

			public int LoadedCount { get; private set; }

			public int TotalCount { get; private set; }

			public object UserData { get; private set; }

			public static LoadDependencyEventArgs Make(string assetName, string dependencyAssetName, int loadedCount, int totalCount, object userData)
			{
				LoadDependencyEventArgs loadDependencyEventArgs = Mod.Reference.Acquire<LoadDependencyEventArgs>();
				loadDependencyEventArgs.AssetName = assetName;
				loadDependencyEventArgs.DependencyAssetName = dependencyAssetName;
				loadDependencyEventArgs.LoadedCount = loadedCount;
				loadDependencyEventArgs.TotalCount = totalCount;
				loadDependencyEventArgs.UserData = userData;
				return loadDependencyEventArgs;
			}

			protected override void OnRecycle()
			{
				UserData = null;
			}
		}

		public sealed class LoadFailureEventArgs : EventArgs<LoadFailureEventArgs>
		{
			public string AssetName { get; private set; }

			public string Message { get; private set; }

			public object UserData { get; private set; }

			public static LoadFailureEventArgs Make(string assetName, string message, object userData)
			{
				LoadFailureEventArgs loadFailureEventArgs = Mod.Reference.Acquire<LoadFailureEventArgs>();
				loadFailureEventArgs.AssetName = assetName;
				loadFailureEventArgs.Message = message;
				loadFailureEventArgs.UserData = userData;
				return loadFailureEventArgs;
			}

			protected override void OnRecycle()
			{
				UserData = null;
			}
		}

		public sealed class LoadSuccessEventArgs : EventArgs<LoadSuccessEventArgs>
		{
			public string AssetName { get; private set; }

			public float Duration { get; private set; }

			public object UserData { get; private set; }

			public static LoadSuccessEventArgs Make(string assetName, float duration, object userData)
			{
				LoadSuccessEventArgs loadSuccessEventArgs = Mod.Reference.Acquire<LoadSuccessEventArgs>();
				loadSuccessEventArgs.AssetName = assetName;
				loadSuccessEventArgs.Duration = duration;
				loadSuccessEventArgs.UserData = userData;
				return loadSuccessEventArgs;
			}

			protected override void OnRecycle()
			{
				UserData = null;
			}
		}

		public sealed class LoadUpdateEventArgs : EventArgs<LoadUpdateEventArgs>
		{
			public string AssetName { get; private set; }

			public float Progress { get; private set; }

			public object UserData { get; private set; }

			public static LoadUpdateEventArgs Make(string assetName, float progress, object userData)
			{
				LoadUpdateEventArgs loadUpdateEventArgs = Mod.Reference.Acquire<LoadUpdateEventArgs>();
				loadUpdateEventArgs.AssetName = assetName;
				loadUpdateEventArgs.Progress = progress;
				loadUpdateEventArgs.UserData = userData;
				return loadUpdateEventArgs;
			}

			protected override void OnRecycle()
			{
				AssetName = null;
				Progress = 0f;
				UserData = null;
			}
		}

		private readonly Dictionary<string, string> _dictionaries = new Dictionary<string, string>();

		private AssetLoadCallbacks _loadCallbacks;

		private readonly SystemLanguage[] _supportedLanguage = new SystemLanguage[12]
		{
			SystemLanguage.English,
			SystemLanguage.Chinese,
			SystemLanguage.ChineseSimplified,
			SystemLanguage.ChineseTraditional,
			SystemLanguage.Dutch,
			SystemLanguage.French,
			SystemLanguage.German,
			SystemLanguage.Italian,
			SystemLanguage.Japanese,
			SystemLanguage.Russian,
			SystemLanguage.Spanish,
			SystemLanguage.Korean
		};

		public SystemLanguage Language { get; set; }

		public string FontName { get; private set; }

		public int Count
		{
			get
			{
				return _dictionaries.Count;
			}
		}

		public SystemLanguage OsLanguage
		{
			get
			{
				return Application.systemLanguage;
			}
		}

		public void Load(string assetName, object userData)
		{
			Mod.Resource.LoadAsset(assetName, _loadCallbacks, userData);
		}

		public bool Parse(string text)
		{
			try
			{
				string text2 = Enum.GetName(typeof(SystemLanguage), Language);
				SecurityParser securityParser = new SecurityParser();
				securityParser.LoadXml(text);
				SecurityElement securityElement = securityParser.ToXml();
				if (securityElement.Tag != "Dictionaries")
				{
					return false;
				}
				for (int i = 0; i < securityElement.Children.Count; i++)
				{
					SecurityElement securityElement2 = securityElement.Children[i] as SecurityElement;
					if ((securityElement2 != null && securityElement2.Tag != "Dictionary") || securityElement2 == null)
					{
						continue;
					}
					string text3 = securityElement2.Attribute("Language");
					if (text3 != text2)
					{
						continue;
					}
					FontName = securityElement2.Attribute("Font");
					if (string.IsNullOrEmpty(FontName))
					{
						Log.Warning("Default font name is invalid.");
					}
					for (int j = 0; j < securityElement2.Children.Count; j++)
					{
						SecurityElement securityElement3 = securityElement2.Children[j] as SecurityElement;
						if ((securityElement3 == null || !(securityElement3.Tag != "String")) && securityElement3 != null)
						{
							string text4 = securityElement3.Attribute("Key");
							string value = securityElement3.Attribute("Value");
							if (!Add(text4, value))
							{
								Log.Warning("Can not add raw string with key '" + text4 + "' which may be invalid or duplicate.");
								return false;
							}
						}
					}
				}
				return true;
			}
			catch (Exception ex)
			{
				Log.Warning("Can not parse dictionary '" + text + "' with exception '" + ex.Message + "\n" + ex.StackTrace + "'.");
				return false;
			}
		}

		public bool Add(string key, string value)
		{
			if (string.IsNullOrEmpty(key))
			{
				Log.Warning("The key is invalid.");
				return false;
			}
			if (Contains(key))
			{
				return false;
			}
			_dictionaries.Add(key, value ?? string.Empty);
			return true;
		}

		public string Get(string key, params object[] args)
		{
			if (string.IsNullOrEmpty(key))
			{
				Log.Error("Key is invalid.");
				return null;
			}
			string value;
			if (!_dictionaries.TryGetValue(key, out value))
			{
				return "<NoKey>" + key;
			}
			try
			{
				return string.Format(value, args);
			}
			catch (Exception ex)
			{
				string text = "<Error>" + key + "," + value;
				if (args != null)
				{
					foreach (object obj in args)
					{
						text = text + "," + obj;
					}
				}
				return text + "," + ex.Message;
			}
		}

		public string Get(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				Log.Error("Key is invalid.");
				return null;
			}
			string value;
			if (!_dictionaries.TryGetValue(key, out value))
			{
				return "<NoKey>" + key;
			}
			return value;
		}

		public bool Contains(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				Log.Error("Key is invalid.");
				return false;
			}
			return _dictionaries.ContainsKey(key);
		}

		public bool Remove(string key)
		{
			if (Contains(key))
			{
				return _dictionaries.Remove(key);
			}
			return false;
		}

		private bool LanguageIsSupported(SystemLanguage language)
		{
			bool result = false;
			for (int i = 0; i < _supportedLanguage.Length; i++)
			{
				if (language == _supportedLanguage[i])
				{
					result = true;
					break;
				}
			}
			return result;
		}

		public void RemoveAll()
		{
			_dictionaries.Clear();
		}

		protected override void Awake()
		{
			Mod.Localization = this;
		}

		internal override void OnInit()
		{
			base.OnInit();
			_loadCallbacks = new AssetLoadCallbacks(OnLoadSuccess, OnLoadFailure, OnLoadUpdate, OnLoadDependencyAsset);
			Language = (Mod.Core.EditorMode ? Mod.Core.EditorLanguage : OsLanguage);
			if (!LanguageIsSupported(Language))
			{
				Language = SystemLanguage.English;
			}
		}

		internal override void OnTick(float elapseSeconds, float realElapseSeconds)
		{
		}

		private void OnLoadSuccess(string assetName, object asset, float duration, object userData)
		{
			string text = ((TextAsset)asset).text;
			Mod.Resource.UnloadAsset((UnityEngine.Object)asset);
			if (text == null)
			{
				OnLoadFailure(assetName, "Dictionary asset '" + assetName + "' is invalid.", userData);
				return;
			}
			if (!Parse(text))
			{
				OnLoadFailure(assetName, "Dictionary asset '" + assetName + "' parse failure.", userData);
				return;
			}
			LoadSuccessEventArgs args = LoadSuccessEventArgs.Make(assetName, duration, userData);
			Mod.Event.Fire(this, args);
		}

		private void OnLoadFailure(string assetName, string message, object userData)
		{
			LoadFailureEventArgs args = LoadFailureEventArgs.Make(assetName, message, userData);
			Mod.Event.Fire(this, args);
		}

		private void OnLoadUpdate(string assetName, float progress, object userData)
		{
			LoadUpdateEventArgs args = LoadUpdateEventArgs.Make(assetName, progress, userData);
			Mod.Event.Fire(this, args);
		}

		private void OnLoadDependencyAsset(string assetName, string dependencyAssetName, int loadedCount, int totalCount, object userData)
		{
			LoadDependencyEventArgs args = LoadDependencyEventArgs.Make(assetName, dependencyAssetName, loadedCount, totalCount, userData);
			Mod.Event.Fire(this, args);
		}
	}
}
