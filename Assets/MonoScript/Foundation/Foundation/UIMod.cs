using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Foundation
{
	[AddComponentMenu("Framework/UI")]
	[DisallowMultipleComponent]
	public sealed class UIMod : ModBase
	{
		public sealed class CloseCompleteEventArgs : EventArgs<CloseCompleteEventArgs>
		{
			public int UIFormId { get; private set; }

			public string AssetName { get; private set; }

			public UIGroup Group { get; private set; }

			public object UserData { get; private set; }

			protected override void OnRecycle()
			{
				UIFormId = -1;
				AssetName = null;
				Group = null;
				UserData = null;
			}

			public static CloseCompleteEventArgs Make(int uiFormId, string assetName, UIGroup group, object userData)
			{
				CloseCompleteEventArgs closeCompleteEventArgs = Mod.Reference.Acquire<CloseCompleteEventArgs>();
				closeCompleteEventArgs.UIFormId = uiFormId;
				closeCompleteEventArgs.AssetName = assetName;
				closeCompleteEventArgs.Group = group;
				closeCompleteEventArgs.UserData = userData;
				return closeCompleteEventArgs;
			}
		}

		public sealed class OpenDependencyEventArgs : EventArgs<OpenDependencyEventArgs>
		{
			public int UIFormId { get; private set; }

			public string AssetName { get; private set; }

			public string GroupName { get; private set; }

			public bool PauseCoveredUIForm { get; private set; }

			public string DependencyAssetName { get; private set; }

			public int LoadedCount { get; private set; }

			public int TotalCount { get; private set; }

			public object UserData { get; private set; }

			protected override void OnRecycle()
			{
				UIFormId = -1;
				AssetName = null;
				GroupName = null;
				PauseCoveredUIForm = false;
				DependencyAssetName = null;
				LoadedCount = 0;
				TotalCount = 0;
				UserData = null;
			}

			public static OpenDependencyEventArgs Make(int uiFormId, string assetName, string groupName, bool pauseCoveredUIForm, string dependencyAssetName, int loadedCount, int totalCount, object userData)
			{
				OpenDependencyEventArgs openDependencyEventArgs = Mod.Reference.Acquire<OpenDependencyEventArgs>();
				openDependencyEventArgs.UIFormId = uiFormId;
				openDependencyEventArgs.AssetName = assetName;
				openDependencyEventArgs.GroupName = groupName;
				openDependencyEventArgs.PauseCoveredUIForm = pauseCoveredUIForm;
				openDependencyEventArgs.DependencyAssetName = dependencyAssetName;
				openDependencyEventArgs.LoadedCount = loadedCount;
				openDependencyEventArgs.TotalCount = totalCount;
				openDependencyEventArgs.UserData = userData;
				return openDependencyEventArgs;
			}
		}

		public sealed class OpenFailureEventArgs : EventArgs<OpenFailureEventArgs>
		{
			public int UIFormId { get; private set; }

			public string AssetName { get; private set; }

			public string GroupName { get; private set; }

			public bool PauseCoveredUIForm { get; private set; }

			public string Message { get; private set; }

			public object UserData { get; private set; }

			protected override void OnRecycle()
			{
				UIFormId = -1;
				AssetName = null;
				GroupName = null;
				PauseCoveredUIForm = false;
				Message = null;
				UserData = null;
			}

			public static OpenFailureEventArgs Make(int uiFormId, string assetName, string groupName, bool pauseCoveredUIForm, string message, object userData)
			{
				OpenFailureEventArgs openFailureEventArgs = Mod.Reference.Acquire<OpenFailureEventArgs>();
				openFailureEventArgs.UIFormId = uiFormId;
				openFailureEventArgs.AssetName = assetName;
				openFailureEventArgs.GroupName = groupName;
				openFailureEventArgs.PauseCoveredUIForm = pauseCoveredUIForm;
				openFailureEventArgs.Message = message;
				openFailureEventArgs.UserData = userData;
				return openFailureEventArgs;
			}
		}

		public sealed class OpenSuccessEventArgs : EventArgs<OpenSuccessEventArgs>
		{
			public UIForm UIForm { get; private set; }

			public float Duration { get; private set; }

			public object UserData { get; private set; }

			protected override void OnRecycle()
			{
				UIForm = null;
				Duration = 0f;
				UserData = null;
			}

			public static OpenSuccessEventArgs Make(UIForm uiForm, float duration, object userData)
			{
				OpenSuccessEventArgs openSuccessEventArgs = Mod.Reference.Acquire<OpenSuccessEventArgs>();
				openSuccessEventArgs.UIForm = uiForm;
				openSuccessEventArgs.Duration = duration;
				openSuccessEventArgs.UserData = userData;
				return openSuccessEventArgs;
			}
		}

		public sealed class OpenUpdateEventArgs : EventArgs<OpenUpdateEventArgs>
		{
			public int UIFormId { get; private set; }

			public string AssetName { get; private set; }

			public string GroupName { get; private set; }

			public bool PauseCoveredUIForm { get; private set; }

			public float Progress { get; private set; }

			public object UserData { get; private set; }

			protected override void OnRecycle()
			{
				UIFormId = -1;
				AssetName = null;
				GroupName = null;
				PauseCoveredUIForm = false;
				Progress = 0f;
				UserData = null;
			}

			public static OpenUpdateEventArgs Make(int uiFormId, string assetName, string groupName, bool pauseCoveredUIForm, float progress, object userData)
			{
				OpenUpdateEventArgs openUpdateEventArgs = Mod.Reference.Acquire<OpenUpdateEventArgs>();
				openUpdateEventArgs.UIFormId = uiFormId;
				openUpdateEventArgs.AssetName = assetName;
				openUpdateEventArgs.GroupName = groupName;
				openUpdateEventArgs.PauseCoveredUIForm = pauseCoveredUIForm;
				openUpdateEventArgs.Progress = progress;
				openUpdateEventArgs.UserData = userData;
				return openUpdateEventArgs;
			}
		}

		private sealed class UIFormObject : SharedObject
		{
			private GameObject _asset;

			public UIFormObject(string name, GameObject asset, GameObject uiFormGo)
				: base(name, uiFormGo)
			{
				if (asset == null)
				{
					Log.Error("UIForm asset is invalid.");
				}
				else if (uiFormGo == null)
				{
					Log.Error("UIForm instance is invalid.");
				}
				else
				{
					_asset = asset;
				}
			}

			protected internal override void OnUnload(bool force = false)
			{
				GameObject gameObject = base.Target as GameObject;
				if (gameObject != null)
				{
					UIForm component = gameObject.GetComponent<UIForm>();
					if (component != null)
					{
						component.OnUnload();
					}
					UnityEngine.Object.Destroy(gameObject);
				}
				base.OnUnload(force);
				Mod.Resource.UnloadAsset(_asset);
				_asset = null;
			}
		}

		[Serializable]
		private sealed class UIGroupInfo
		{
			[SerializeField]
			private string _name;

			[SerializeField]
			private int _depth;

			public string Name
			{
				get
				{
					return _name;
				}
			}

			public int Depth
			{
				get
				{
					return _depth;
				}
			}

			public UIGroupInfo()
			{
				_depth = 0;
			}
		}

		private sealed class UserDataEx
		{
			[CompilerGenerated]
			private readonly int _003CUIFormId_003Ek__BackingField;

			[CompilerGenerated]
			private readonly UIGroup _003CUIGroup_003Ek__BackingField;

			[CompilerGenerated]
			private readonly bool _003CPauseCovered_003Ek__BackingField;

			[CompilerGenerated]
			private readonly bool _003CHidden_003Ek__BackingField;

			[CompilerGenerated]
			private readonly object _003CUserData_003Ek__BackingField;

			public int UIFormId
			{
				[CompilerGenerated]
				get
				{
					return _003CUIFormId_003Ek__BackingField;
				}
			}

			public UIGroup UIGroup
			{
				[CompilerGenerated]
				get
				{
					return _003CUIGroup_003Ek__BackingField;
				}
			}

			public bool PauseCovered
			{
				[CompilerGenerated]
				get
				{
					return _003CPauseCovered_003Ek__BackingField;
				}
			}

			public bool Hidden
			{
				[CompilerGenerated]
				get
				{
					return _003CHidden_003Ek__BackingField;
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

			public UserDataEx(int uiFormId, UIGroup uiGroup, bool pauseCovered, bool hidden, object userData)
			{
				_003CUIFormId_003Ek__BackingField = uiFormId;
				_003CUIGroup_003Ek__BackingField = uiGroup;
				_003CPauseCovered_003Ek__BackingField = pauseCovered;
				_003CHidden_003Ek__BackingField = hidden;
				_003CUserData_003Ek__BackingField = userData;
			}
		}

		private sealed class LoadingFormData
		{
			[CompilerGenerated]
			private readonly int _003CUIFormId_003Ek__BackingField;

			[CompilerGenerated]
			private readonly string _003CAssertName_003Ek__BackingField;

			public int UIFormId
			{
				[CompilerGenerated]
				get
				{
					return _003CUIFormId_003Ek__BackingField;
				}
			}

			public string AssertName
			{
				[CompilerGenerated]
				get
				{
					return _003CAssertName_003Ek__BackingField;
				}
			}

			public LoadingFormData(int uiFormId, string assertName)
			{
				_003CUIFormId_003Ek__BackingField = uiFormId;
				_003CAssertName_003Ek__BackingField = assertName;
			}

			public override int GetHashCode()
			{
				return UIFormId;
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass40_0
		{
			public int uiFormId;

			internal bool _003CCloseUIForm_003Eb__0(LoadingFormData x)
			{
				return x.UIFormId == uiFormId;
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass54_0
		{
			public UserDataEx info;

			internal bool _003COnLoadSuccess_003Eb__0(LoadingFormData x)
			{
				return x.UIFormId == info.UIFormId;
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass55_0
		{
			public UserDataEx info;

			internal bool _003COnLoadFailure_003Eb__0(LoadingFormData x)
			{
				return x.UIFormId == info.UIFormId;
			}
		}

		[SerializeField]
		private Camera _uiCamera;

		[SerializeField]
		private UIGroupInfo[] _groupInfos = new UIGroupInfo[0];

		private readonly Dictionary<string, UIGroup> _groups = new Dictionary<string, UIGroup>();

		private readonly HashSet<LoadingFormData> _loadingUIForms = new HashSet<LoadingFormData>();

		private readonly HashSet<int> _unloadingUIForms = new HashSet<int>();

		private readonly LinkedList<UIForm> _recycleQueue = new LinkedList<UIForm>();

		private AssetLoadCallbacks _loadCallbacks;

		private ObjectPool<UIFormObject> _uiFormPool;

		private int _nextUIFormId;

		public const int InvalidUIFormId = -1;

		public Camera UICamera
		{
			get
			{
				return _uiCamera;
			}
			set
			{
				_uiCamera = value;
			}
		}

		public int UIGroupCount
		{
			get
			{
				return _groups.Count;
			}
		}

		public UIGroup[] UIGroups
		{
			get
			{
				int num = 0;
				UIGroup[] array = new UIGroup[_groups.Count];
				foreach (KeyValuePair<string, UIGroup> group in _groups)
				{
					array[num++] = group.Value;
				}
				return array;
			}
		}

		public UIForm[] LoadedUIForms
		{
			get
			{
				List<UIForm> list = new List<UIForm>();
				foreach (KeyValuePair<string, UIGroup> group in _groups)
				{
					list.AddRange(group.Value.UIForms);
				}
				return list.ToArray();
			}
		}

		public int[] LoadingUIFormIds
		{
			get
			{
				int num = 0;
				int[] array = new int[_loadingUIForms.Count];
				foreach (LoadingFormData loadingUIForm in _loadingUIForms)
				{
					array[num++] = loadingUIForm.UIFormId;
				}
				return array;
			}
		}

		public bool HasUIGroup(string groupName)
		{
			if (string.IsNullOrEmpty(groupName))
			{
				Log.Warning("UIGroup name is invalid.");
				return false;
			}
			return _groups.ContainsKey(groupName);
		}

		public UIGroup GetUIGroup(string groupName)
		{
			if (string.IsNullOrEmpty(groupName))
			{
				Log.Warning("UIGroup name is invalid.");
				return null;
			}
			UIGroup value;
			if (!_groups.TryGetValue(groupName, out value))
			{
				return null;
			}
			return value;
		}

		public bool HasUIForm(int uiFormId)
		{
			foreach (KeyValuePair<string, UIGroup> group in _groups)
			{
				if (group.Value.HasUIForm(uiFormId))
				{
					return true;
				}
			}
			return false;
		}

		public bool HasUIForm(string assetName)
		{
			if (string.IsNullOrEmpty(assetName))
			{
				Log.Warning("UIForm asset name is invalid.");
				return false;
			}
			foreach (KeyValuePair<string, UIGroup> group in _groups)
			{
				if (group.Value.HasUIForm(assetName))
				{
					return true;
				}
			}
			return false;
		}

		public UIForm GetUIForm(int uiFormId)
		{
			foreach (KeyValuePair<string, UIGroup> group in _groups)
			{
				UIForm uIForm = group.Value.GetUIForm(uiFormId);
				if (uIForm != null)
				{
					return uIForm;
				}
			}
			return null;
		}

		public UIForm GetUIForm(string assetName)
		{
			if (string.IsNullOrEmpty(assetName))
			{
				Log.Warning("UIForm asset name is invalid.");
				return null;
			}
			foreach (KeyValuePair<string, UIGroup> group in _groups)
			{
				UIForm uIForm = group.Value.GetUIForm(assetName);
				if (uIForm != null)
				{
					return uIForm;
				}
			}
			return null;
		}

		public UIForm[] GetUIForms(string assetName)
		{
			if (string.IsNullOrEmpty(assetName))
			{
				Log.Warning("UIForm asset name is invalid.");
				return null;
			}
			List<UIForm> list = new List<UIForm>();
			foreach (KeyValuePair<string, UIGroup> group in _groups)
			{
				list.AddRange(group.Value.GetUIForms(assetName));
			}
			return list.ToArray();
		}

		public bool IsLoadingUIForm(int uiFormId)
		{
			foreach (LoadingFormData loadingUIForm in _loadingUIForms)
			{
				if (loadingUIForm.UIFormId == uiFormId)
				{
					return true;
				}
			}
			return false;
		}

		public bool IsLoadingUIForm(string assetName)
		{
			foreach (LoadingFormData loadingUIForm in _loadingUIForms)
			{
				if (loadingUIForm.AssertName == assetName)
				{
					return true;
				}
			}
			return false;
		}

		public bool IsValidUIForm(UIForm uiForm)
		{
			if (uiForm != null)
			{
				return HasUIForm(uiForm.Id);
			}
			return false;
		}

		public int OpenUIForm(string assetName, string groupName, object userData = null)
		{
			return OpenUIForm(assetName, groupName, false, false, userData);
		}

		public int OpenUIForm(string assetName, string groupName, bool hidden, object userData = null)
		{
			return OpenUIForm(assetName, groupName, false, hidden, userData);
		}

		public int OpenUIForm(string assetName, string groupName, bool pauseCovered, bool hidden, object userData = null)
		{
			if (string.IsNullOrEmpty(assetName))
			{
				Log.Warning("UI form asset name is invalid.");
				return -1;
			}
			if (string.IsNullOrEmpty(groupName))
			{
				Log.Warning("UI group name is invalid.");
				return -1;
			}
			UIGroup uIGroup = GetUIGroup(groupName);
			if (uIGroup == null)
			{
				Log.Warning("UIGroup '" + groupName + "' is not exist.");
				return -1;
			}
			int num = _nextUIFormId++;
			UIFormObject uIFormObject = _uiFormPool.Spawn(assetName);
			uIGroup.AddOrderOpenFormId(num);
			if (uIFormObject == null)
			{
				_loadingUIForms.Add(new LoadingFormData(num, assetName));
				Mod.Resource.LoadAsset(assetName, _loadCallbacks, new UserDataEx(num, uIGroup, pauseCovered, hidden, userData));
			}
			else
			{
				InternalOpenUIForm(num, assetName, uIGroup, (GameObject)uIFormObject.Target, pauseCovered, false, hidden, 0f, userData);
			}
			return num;
		}

		public void CloseUIForm(int uiFormId, object userData = null)
		{
			_003C_003Ec__DisplayClass40_0 _003C_003Ec__DisplayClass40_ = new _003C_003Ec__DisplayClass40_0();
			_003C_003Ec__DisplayClass40_.uiFormId = uiFormId;
			if (IsLoadingUIForm(_003C_003Ec__DisplayClass40_.uiFormId))
			{
				_unloadingUIForms.Add(_003C_003Ec__DisplayClass40_.uiFormId);
				_loadingUIForms.RemoveWhere(_003C_003Ec__DisplayClass40_._003CCloseUIForm_003Eb__0);
				return;
			}
			UIForm uIForm = GetUIForm(_003C_003Ec__DisplayClass40_.uiFormId);
			if (uIForm == null)
			{
				Log.Warning("Can not find UIForm '" + _003C_003Ec__DisplayClass40_.uiFormId + "'.");
			}
			else
			{
				CloseUIForm(uIForm, userData);
			}
		}

		public void CloseUIForm(UIForm uiForm, object userData = null)
		{
			if (uiForm == null)
			{
				Log.Warning("UIForm is invalid.");
				return;
			}
			UIGroup group = uiForm.Group;
			if (group == null)
			{
				Log.Warning("UIGroup is invalid.");
				return;
			}
			group.RemoveUIForm(uiForm);
			uiForm.OnClose(userData);
			group.Refresh();
			_recycleQueue.AddLast(uiForm);
			CloseCompleteEventArgs args = CloseCompleteEventArgs.Make(uiForm.Id, uiForm.AssetName, group, userData);
			Mod.Event.Fire(this, args);
		}

		public void CloseLoadedUIForms(object userData = null)
		{
			UIForm[] loadedUIForms = LoadedUIForms;
			for (int i = 0; i < loadedUIForms.Length; i++)
			{
				CloseUIForm(loadedUIForms[i], userData);
			}
		}

		public void CloseLoadingUIForms()
		{
			foreach (LoadingFormData loadingUIForm in _loadingUIForms)
			{
				_unloadingUIForms.Add(loadingUIForm.UIFormId);
			}
			_loadingUIForms.Clear();
		}

		public void Hidden(UIForm uiForm, bool hidden)
		{
			if (uiForm == null)
			{
				Log.Warning("UIForm is invalid.");
				return;
			}
			UIGroup group = uiForm.Group;
			if (group == null)
			{
				Log.Warning("UIGroup is invalid.");
			}
			else
			{
				group.Hidden(uiForm, hidden);
			}
		}

		public void RefocusUIForm(UIForm uiForm, object userData = null)
		{
			if (uiForm == null)
			{
				Log.Warning("UIForm is invalid.");
				return;
			}
			UIGroup group = uiForm.Group;
			if (group == null)
			{
				Log.Warning("UIGroup is invalid.");
				return;
			}
			group.RefocusUIForm(uiForm, userData);
			group.Refresh();
			uiForm.OnRefocus(userData);
		}

		public void SetUIFormLock(GameObject uiFormGo, bool locked)
		{
			if (uiFormGo == null)
			{
				Log.Warning("UIForm Instance is invalid.");
			}
			else
			{
				_uiFormPool.SetLock(uiFormGo, locked);
			}
		}

		protected override void Awake()
		{
			Mod.UI = this;
		}

		internal override void OnInit()
		{
			base.OnInit();
			_loadCallbacks = new AssetLoadCallbacks(OnLoadSuccess, OnLoadFailure, OnLoadUpdate, OnLoadDependency);
			_uiFormPool = Mod.ObjectPool.Create<UIFormObject>("UIForm Pool", false, true);
			if (_uiCamera == null)
			{
				GameObject gameObject = GameObject.FindWithTag("UICamera");
				if (gameObject == null)
				{
					gameObject = GameObject.Find("UI Camera");
				}
				_uiCamera = gameObject.GetOrAddComponent<Camera>();
			}
			for (int i = 0; i < _groupInfos.Length; i++)
			{
				UIGroupInfo uIGroupInfo = _groupInfos[i];
				if (!string.IsNullOrEmpty(uIGroupInfo.Name))
				{
					GameObject gameObject2 = new GameObject("UI Group - " + uIGroupInfo.Name);
					gameObject2.transform.parent = base.transform;
					gameObject2.hideFlags |= HideFlags.DontSave;
					gameObject2.hideFlags |= HideFlags.NotEditable;
					UIGroup group = gameObject2.AddComponent<UIGroup>();
					if (!AddUIGroup(group, uIGroupInfo.Name, uIGroupInfo.Depth))
					{
						Log.Warning("Add UIGroup '" + uIGroupInfo.Name + "' failure.");
					}
				}
			}
		}

		internal override void OnTick(float elapseSeconds, float realElapseSeconds)
		{
			Profiler.BeginSample("UIMod.OnTick");
			while (_recycleQueue.Count > 0)
			{
				UIForm value = _recycleQueue.First.Value;
				_recycleQueue.RemoveFirst();
				value.OnRecycle();
				_uiFormPool.Recycle(value.gameObject);
				_uiFormPool.Unload();
			}
			foreach (KeyValuePair<string, UIGroup> group in _groups)
			{
				group.Value.Tick(elapseSeconds, realElapseSeconds);
			}
			Profiler.EndSample();
		}

		internal override void OnExit()
		{
			CloseLoadingUIForms();
			CloseLoadedUIForms();
			_groups.Clear();
			_loadingUIForms.Clear();
			_unloadingUIForms.Clear();
			_recycleQueue.Clear();
		}

		private void InternalOpenUIForm(int uiFormId, string assetName, UIGroup uiGroup, GameObject uiFormGo, bool pauseCovered, bool isNew, bool hidden, float duration, object userData)
		{
			try
			{
				UIForm uIForm = CreateUIForm(uiFormGo, uiGroup);
				if (uIForm == null)
				{
					Log.Error("Can not create UIForm in handler.");
					return;
				}
				uIForm.OnInit(uiFormId, assetName, uiGroup, pauseCovered, isNew, hidden, userData);
				uiGroup.AddUIForm(uIForm, hidden);
				uiGroup.Refresh();
				UIFormOpenCacheData cacheFormData = new UIFormOpenCacheData(uIForm, duration, userData);
				uiGroup.AddCacheOrderOpenFormData(cacheFormData);
				if (!uiGroup.HadOrderForm(uIForm))
				{
					uiGroup.OpenCachedForm();
				}
			}
			catch (Exception ex)
			{
				OpenFailureEventArgs args = OpenFailureEventArgs.Make(uiFormId, assetName, uiGroup.Name, pauseCovered, ex.ToString(), userData);
				Mod.Event.Fire(this, args);
			}
		}

		private UIForm CreateUIForm(GameObject uiFormGo, UIGroup group)
		{
			if (uiFormGo != null)
			{
				Transform transform = uiFormGo.transform;
				transform.SetParent(group.transform);
				transform.localScale = Vector3.one;
				transform.localPosition = Vector3.zero;
				transform.localRotation = Quaternion.identity;
				return uiFormGo.GetOrAddComponent<UIForm>();
			}
			return null;
		}

		private bool AddUIGroup(UIGroup group, string groupName, int groupDepth)
		{
			if (group == null)
			{
				Log.Error("UIGroup is invalid.");
				return false;
			}
			if (string.IsNullOrEmpty(groupName))
			{
				Log.Error("UIGroup name is invalid.");
				return false;
			}
			if (HasUIGroup(groupName))
			{
				return false;
			}
			group.Init(groupName, groupDepth);
			_groups.Add(groupName, group);
			return true;
		}

		private void OnLoadSuccess(string assetName, object asset, float duration, object userData)
		{
			_003C_003Ec__DisplayClass54_0 _003C_003Ec__DisplayClass54_ = new _003C_003Ec__DisplayClass54_0();
			_003C_003Ec__DisplayClass54_.info = (UserDataEx)userData;
			_loadingUIForms.RemoveWhere(_003C_003Ec__DisplayClass54_._003COnLoadSuccess_003Eb__0);
			if (_unloadingUIForms.Remove(_003C_003Ec__DisplayClass54_.info.UIFormId))
			{
				Log.Info("Release UIForm '" + _003C_003Ec__DisplayClass54_.info.UIFormId + "' on loading success.");
				Mod.Resource.UnloadAsset((UnityEngine.Object)asset);
				return;
			}
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate((UnityEngine.Object)asset);
			gameObject.hideFlags |= HideFlags.DontSave;
			UIFormObject uIFormObject = new UIFormObject(assetName, (GameObject)asset, gameObject);
			uIFormObject.OnSpawn();
			_uiFormPool.Register(uIFormObject);
			InternalOpenUIForm(_003C_003Ec__DisplayClass54_.info.UIFormId, assetName, _003C_003Ec__DisplayClass54_.info.UIGroup, (GameObject)uIFormObject.Target, _003C_003Ec__DisplayClass54_.info.PauseCovered, true, _003C_003Ec__DisplayClass54_.info.Hidden, duration, _003C_003Ec__DisplayClass54_.info.UserData);
		}

		private void OnLoadFailure(string assetName, string message, object userData)
		{
			_003C_003Ec__DisplayClass55_0 _003C_003Ec__DisplayClass55_ = new _003C_003Ec__DisplayClass55_0();
			_003C_003Ec__DisplayClass55_.info = (UserDataEx)userData;
			_loadingUIForms.RemoveWhere(_003C_003Ec__DisplayClass55_._003COnLoadFailure_003Eb__0);
			_unloadingUIForms.Remove(_003C_003Ec__DisplayClass55_.info.UIFormId);
			OpenFailureEventArgs args = OpenFailureEventArgs.Make(_003C_003Ec__DisplayClass55_.info.UIFormId, assetName, _003C_003Ec__DisplayClass55_.info.UIGroup.Name, _003C_003Ec__DisplayClass55_.info.PauseCovered, message, _003C_003Ec__DisplayClass55_.info.UserData);
			Mod.Event.Fire(this, args);
		}

		private void OnLoadUpdate(string assetName, float progress, object userData)
		{
			UserDataEx userDataEx = (UserDataEx)userData;
			OpenUpdateEventArgs args = OpenUpdateEventArgs.Make(userDataEx.UIFormId, assetName, userDataEx.UIGroup.Name, userDataEx.PauseCovered, progress, userDataEx.UserData);
			Mod.Event.Fire(this, args);
		}

		private void OnLoadDependency(string assetName, string dependencyAssetName, int loadedCount, int totalCount, object userData)
		{
			UserDataEx userDataEx = (UserDataEx)userData;
			OpenDependencyEventArgs args = OpenDependencyEventArgs.Make(userDataEx.UIFormId, assetName, userDataEx.UIGroup.Name, userDataEx.PauseCovered, dependencyAssetName, loadedCount, totalCount, userDataEx.UserData);
			Mod.Event.Fire(this, args);
		}
	}
}
