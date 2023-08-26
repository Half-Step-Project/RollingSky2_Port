using System;
using System.Collections.Generic;
using UnityEngine;

namespace Foundation
{
	public sealed class EditorResLoader : EditorLoaderBase
	{
		private struct SceneLoadInfo
		{
			public AsyncOperation AsyncOperation { get; private set; }

			public string AssetName { get; private set; }

			public DateTime StartTime { get; private set; }

			public SceneLoadCallbacks LoadSceneCallbacks { get; private set; }

			public object UserData { get; private set; }

			public SceneLoadInfo(AsyncOperation asyncOperation, string assetName, DateTime startTime, SceneLoadCallbacks loadSceneCallbacks, object userData)
			{
				this = default(SceneLoadInfo);
				AsyncOperation = asyncOperation;
				AssetName = assetName;
				StartTime = startTime;
				LoadSceneCallbacks = loadSceneCallbacks;
				UserData = userData;
			}
		}

		private struct SceneUnloadInfo
		{
			public AsyncOperation AsyncOperation { get; private set; }

			public string SceneAssetName { get; private set; }

			public SceneUnloadCallbacks UnloadSceneCallbacks { get; private set; }

			public object UserData { get; private set; }

			public SceneUnloadInfo(AsyncOperation asyncOperation, string assetName, SceneUnloadCallbacks unloadSceneCallbacks, object userData)
			{
				this = default(SceneUnloadInfo);
				AsyncOperation = asyncOperation;
				SceneAssetName = assetName;
				UnloadSceneCallbacks = unloadSceneCallbacks;
				UserData = userData;
			}
		}

		private readonly LinkedList<SceneLoadInfo> _loadSceneInfos = new LinkedList<SceneLoadInfo>();

		private readonly LinkedList<SceneUnloadInfo> _unloadSceneInfos = new LinkedList<SceneUnloadInfo>();

		protected override void LoadAsset(string assetName, AssetLoadCallbacks loadCallbacks, object userData)
		{
		}

		protected override void LoadScene(string assetName, SceneLoadCallbacks loadCallbacks, object userData)
		{
		}

		protected override void UnloadScene(string assetName, SceneUnloadCallbacks unloadCallbacks, object userData)
		{
		}

		protected override void Tick(float elapseSeconds, float realElapseSeconds)
		{
		}
	}
}
