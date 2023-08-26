using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Foundation
{
	[AddComponentMenu("Framework/Config")]
	[DisallowMultipleComponent]
	public sealed class ConfigMod : ModBase
	{
		private struct Value : IEquatable<Value>
		{
			[CompilerGenerated]
			private readonly bool _003CBoolValue_003Ek__BackingField;

			[CompilerGenerated]
			private readonly int _003CIntValue_003Ek__BackingField;

			[CompilerGenerated]
			private readonly float _003CFloatValue_003Ek__BackingField;

			[CompilerGenerated]
			private readonly string _003CStringValue_003Ek__BackingField;

			public bool BoolValue
			{
				[CompilerGenerated]
				get
				{
					return _003CBoolValue_003Ek__BackingField;
				}
			}

			public int IntValue
			{
				[CompilerGenerated]
				get
				{
					return _003CIntValue_003Ek__BackingField;
				}
			}

			public float FloatValue
			{
				[CompilerGenerated]
				get
				{
					return _003CFloatValue_003Ek__BackingField;
				}
			}

			public string StringValue
			{
				[CompilerGenerated]
				get
				{
					return _003CStringValue_003Ek__BackingField;
				}
			}

			public Value(bool boolValue, int intValue, float floatValue, string stringValue)
			{
				_003CBoolValue_003Ek__BackingField = boolValue;
				_003CIntValue_003Ek__BackingField = intValue;
				_003CFloatValue_003Ek__BackingField = floatValue;
				_003CStringValue_003Ek__BackingField = stringValue;
			}

			public bool Equals(Value other)
			{
				if (BoolValue == other.BoolValue && IntValue == other.IntValue && FloatValue.Equals(other.FloatValue))
				{
					return StringValue == other.StringValue;
				}
				return false;
			}

			public override bool Equals(object obj)
			{
				if (obj is Value)
				{
					return Equals((Value)obj);
				}
				return false;
			}

			public override int GetHashCode()
			{
				return BoolValue.GetHashCode() ^ IntValue.GetHashCode() ^ FloatValue.GetHashCode() ^ StringValue.GetHashCode();
			}
		}

		private sealed class ValueComparer : IEqualityComparer<Value>
		{
			[CompilerGenerated]
			private static readonly ValueComparer _003CDefault_003Ek__BackingField = new ValueComparer();

			public static ValueComparer Default
			{
				[CompilerGenerated]
				get
				{
					return _003CDefault_003Ek__BackingField;
				}
			}

			public bool Equals(Value x, Value y)
			{
				return x.Equals(y);
			}

			public int GetHashCode(Value obj)
			{
				return obj.GetHashCode();
			}
		}

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
				UserData = null;
			}
		}

		private const int ColumnCount = 4;

		private static readonly string[] _columnSplit = new string[1] { "\t" };

		private readonly Dictionary<string, Value> _data = new Dictionary<string, Value>();

		private AssetLoadCallbacks _loadCallbacks;

		public int Count
		{
			get
			{
				return _data.Count;
			}
		}

		public void Load(string assetName, object userData = null)
		{
			Mod.Resource.LoadAsset(assetName, _loadCallbacks, userData);
		}

		public bool Parse(string text)
		{
			bool result = true;
			try
			{
				string[] array = TextUtility.SplitToLines(text);
				for (int i = 0; i < array.Length; i++)
				{
					string text2 = array[i];
					if (string.IsNullOrEmpty(text2))
					{
						continue;
					}
					text2 = text2.Trim();
					if (text2.Length > 0 && text2[0] != '#')
					{
						string[] array2 = text2.Split(_columnSplit, StringSplitOptions.None);
						if (array2.Length != 4)
						{
							Log.Warning("Config fields is not '" + 4 + "'.");
							return false;
						}
						string key = array2[1];
						string text3 = array2[3];
						bool result2;
						bool.TryParse(text3, out result2);
						int result3;
						int.TryParse(text3, out result3);
						float result4;
						float.TryParse(text3, out result4);
						result = Add(key, result2, result3, result4, text3);
					}
				}
				return result;
			}
			catch (Exception ex)
			{
				Log.Warning("Can not parse config with exception " + ex.Message + "\n" + ex.StackTrace + ".");
				return false;
			}
		}

		public bool Add(string key, bool boolValue, int intValue, float floatValue, string stringValue)
		{
			if (_data.ContainsKey(key))
			{
				Log.Warning("Can not add config with key '" + key + "' which is duplicate.");
				return false;
			}
			_data.Add(key, new Value(boolValue, intValue, floatValue, stringValue));
			return true;
		}

		public bool Contains(string key)
		{
			return _data.ContainsKey(key);
		}

		public bool Remove(string key)
		{
			return _data.Remove(key);
		}

		public void RemoveAll()
		{
			_data.Clear();
		}

		public bool GetBool(string key, bool defaultValue = false)
		{
			Value value;
			if (!TryGetValue(key, out value))
			{
				Log.Warning("Config key '" + key + "' is not exist.");
				return defaultValue;
			}
			return value.BoolValue;
		}

		public int GetInt(string key, int defaultValue = 0)
		{
			Value value;
			if (!TryGetValue(key, out value))
			{
				Log.Warning("Config key '" + key + "' is not exist.");
				return defaultValue;
			}
			return value.IntValue;
		}

		public float GetFloat(string key, float defaultValue = 0f)
		{
			Value value;
			if (!TryGetValue(key, out value))
			{
				Log.Warning("Config key '" + key + "' is not exist.");
				return defaultValue;
			}
			return value.FloatValue;
		}

		public string GetString(string key, string defaultValue = null)
		{
			Value value;
			if (!TryGetValue(key, out value))
			{
				Log.Warning("Config key '" + key + "' is not exist.");
				return defaultValue;
			}
			return value.StringValue;
		}

		protected override void Awake()
		{
			Mod.Config = this;
		}

		internal override void OnTick(float elapseSeconds, float realElapseSeconds)
		{
		}

		private bool TryGetValue(string key, out Value value)
		{
			value = default(Value);
			if (!string.IsNullOrEmpty(key))
			{
				return _data.TryGetValue(key, out value);
			}
			return false;
		}

		private void OnLoadSuccess(string assetName, UnityEngine.Object asset, float duration, object userData)
		{
			string text = ((TextAsset)asset).text;
			Mod.Resource.UnloadAsset(asset);
			if (!Parse(text))
			{
				OnLoadFailure(assetName, "Config asset '" + assetName + "' parse failure.", userData);
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
