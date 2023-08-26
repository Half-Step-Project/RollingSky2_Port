using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Foundation
{
	[DisallowMultipleComponent]
	[AddComponentMenu("Framework/Scene")]
	public sealed class SceneMod : ModBase
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
				AssetName = null;
				DependencyAssetName = null;
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
				AssetName = null;
				Message = null;
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
				AssetName = null;
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
				UserData = null;
			}
		}

		public sealed class UnloadFailureEventArgs : EventArgs<UnloadFailureEventArgs>
		{
			public string AssetName { get; private set; }

			public string Message { get; private set; }

			public object UserData { get; private set; }

			public static UnloadFailureEventArgs Make(string assetName, string message, object userData)
			{
				UnloadFailureEventArgs unloadFailureEventArgs = Mod.Reference.Acquire<UnloadFailureEventArgs>();
				unloadFailureEventArgs.AssetName = assetName;
				unloadFailureEventArgs.Message = message;
				unloadFailureEventArgs.UserData = userData;
				return unloadFailureEventArgs;
			}

			protected override void OnRecycle()
			{
				AssetName = null;
				UserData = null;
			}
		}

		public sealed class UnloadSuccessEventArgs : EventArgs<UnloadSuccessEventArgs>
		{
			public string AssetName { get; private set; }

			public object UserData { get; private set; }

			public static UnloadSuccessEventArgs Make(string assetName, object userData)
			{
				UnloadSuccessEventArgs unloadSuccessEventArgs = Mod.Reference.Acquire<UnloadSuccessEventArgs>();
				unloadSuccessEventArgs.AssetName = assetName;
				unloadSuccessEventArgs.UserData = userData;
				return unloadSuccessEventArgs;
			}

			protected override void OnRecycle()
			{
				AssetName = null;
				UserData = null;
			}
		}

		private readonly List<string> _loadedAssetNames = new List<string>();

		private readonly List<string> _loadingAssetNames = new List<string>();

		private readonly List<string> _unloadingAssetNames = new List<string>();

		private SceneLoadCallbacks _loadCallbacks;

		private SceneUnloadCallbacks _unloadCallbacks;

		private Scene _modLevel;

		private Camera _mainCamera;

		public Camera MainCamera
		{
			get
			{
				return _mainCamera;
			}
		}

		public string[] LoadedAssetNames
		{
			get
			{
				return _loadedAssetNames.ToArray();
			}
		}

		public string[] LoadingAssetNames
		{
			get
			{
				return _loadingAssetNames.ToArray();
			}
		}

		public string[] UnloadingAssetNames
		{
			get
			{
				return _unloadingAssetNames.ToArray();
			}
		}

		public string GetName(string assetName)
		{
			if (string.IsNullOrEmpty(assetName))
			{
				Log.Error("Scene asset name is invalid.");
				return null;
			}
			int num = assetName.LastIndexOf('/');
			if (num + 1 >= assetName.Length)
			{
				Log.Error("Scene asset name '" + assetName + "' is invalid.");
				return null;
			}
			string text = assetName.Substring(num + 1);
			num = text.LastIndexOf(".unity", StringComparison.Ordinal);
			if (num > 0)
			{
				text = text.Substring(0, num);
			}
			return text;
		}

		public bool IsLoaded(string assetName)
		{
			if (string.IsNullOrEmpty(assetName))
			{
				Log.Warning("Scene asset name is invalid.");
				return false;
			}
			return _loadedAssetNames.Contains(assetName);
		}

		public bool IsLoading(string assetName)
		{
			if (string.IsNullOrEmpty(assetName))
			{
				Log.Warning("Scene asset name is invalid.");
				return false;
			}
			return _loadingAssetNames.Contains(assetName);
		}

		public bool IsUnloading(string assetName)
		{
			if (string.IsNullOrEmpty(assetName))
			{
				Log.Warning("Scene asset name is invalid.");
				return false;
			}
			return _unloadingAssetNames.Contains(assetName);
		}

		public void LoadScene(string assetName, object userData)
		{
			if (string.IsNullOrEmpty(assetName))
			{
				Log.Warning("Scene asset name is invalid.");
				return;
			}
			if (IsUnloading(assetName))
			{
				Log.Warning("Scene asset '" + assetName + "' is being unloaded.");
				return;
			}
			if (IsLoading(assetName))
			{
				Log.Warning("Scene asset '" + assetName + "' is being loaded.");
				return;
			}
			if (IsLoaded(assetName))
			{
				Log.Warning("Scene asset '" + assetName + "' is already loaded.");
				return;
			}
			_loadingAssetNames.Add(assetName);
			Mod.Resource.LoadScene(assetName, _loadCallbacks, userData);
		}

		public void UnloadScene(string assetName, object userData = null)
		{
			if (string.IsNullOrEmpty(assetName))
			{
				Log.Warning("Scene asset name is invalid.");
				return;
			}
			if (IsUnloading(assetName))
			{
				Log.Warning("Scene asset '" + assetName + "' is being unloaded.");
				return;
			}
			if (IsLoading(assetName))
			{
				Log.Warning("Scene asset '" + assetName + "' is being loaded.");
				return;
			}
			if (!IsLoaded(assetName))
			{
				Log.Warning("Scene asset '" + assetName + "' is not loaded yet.");
				return;
			}
			_unloadingAssetNames.Add(assetName);
			Mod.Resource.UnloadScene(assetName, _unloadCallbacks, userData);
		}

		protected override void Awake()
		{
			Mod.Scene = this;
		}

		internal override void OnInit()
		{
			base.OnInit();
			_loadCallbacks = new SceneLoadCallbacks(OnLoadSuccess, OnLoadFailure, OnLoadUpdate, OnLoadDependencyAsset);
			_unloadCallbacks = new SceneUnloadCallbacks(OnUnloadSuccess, OnUnloadFailure);
			_modLevel = SceneManager.GetSceneAt(0);
			if (!_modLevel.IsValid())
			{
				Log.Error("Framework scene is invalid.");
			}
		}

		internal override void OnTick(float elapseSeconds, float realElapseSeconds)
		{
		}

		internal override void OnExit()
		{
			base.OnExit();
			string[] array = _loadedAssetNames.ToArray();
			string[] array2 = array;
			foreach (string assetName in array2)
			{
				if (!IsUnloading(assetName))
				{
					UnloadScene(assetName);
				}
			}
			_loadedAssetNames.Clear();
			_loadingAssetNames.Clear();
			_unloadingAssetNames.Clear();
		}

		private void OnLoadSuccess(string assetName, float duration, object userData)
		{
			if (SceneManager.GetActiveScene() == _modLevel)
			{
				Scene sceneByName = SceneManager.GetSceneByName(GetName(assetName));
				if (!sceneByName.IsValid())
				{
					OnLoadFailure(assetName, "Loaded scene '" + assetName + "' is invalid.", userData);
					return;
				}
				SceneManager.SetActiveScene(sceneByName);
			}
			_loadingAssetNames.Remove(assetName);
			_loadedAssetNames.Add(assetName);
			_mainCamera = Camera.main;
			Mod.ObjectPool.Unload();
			Mod.Resource.UnloadUnusedAssets();
			LoadSuccessEventArgs args = LoadSuccessEventArgs.Make(assetName, duration, userData);
			Mod.Event.Fire(this, args);
		}

		private void OnLoadFailure(string assetName, string message, object userData)
		{
			_loadingAssetNames.Remove(assetName);
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

		private void OnUnloadSuccess(string assetName, object userData)
		{
			_unloadingAssetNames.Remove(assetName);
			_loadedAssetNames.Remove(assetName);
			Mod.ObjectPool.Unload();
			Mod.Resource.UnloadUnusedAssets();
			UnloadSuccessEventArgs args = UnloadSuccessEventArgs.Make(assetName, userData);
			Mod.Event.Fire(this, args);
		}

		private void OnUnloadFailure(string assetName, string message, object userData)
		{
			_unloadingAssetNames.Remove(assetName);
			UnloadFailureEventArgs args = UnloadFailureEventArgs.Make(assetName, message, userData);
			Mod.Event.Fire(this, args);
		}
	}
}
