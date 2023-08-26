using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace Foundation
{
	[DisallowMultipleComponent]
	[AddComponentMenu("Framework/Resource")]
	public sealed class ResourceMod : ModBase
	{
		private sealed class SceneDummy
		{
		}

		private sealed class AssetLoadTask : ResourceLoadTask
		{
			private readonly List<ResourceName> _dependencyAssetNames;

			private int _loadedDependencyAssetCount;

			private readonly AssetLoadCallbacks _loadCallbacks;

			private readonly object _userData;

			private readonly ResourceLoadTask _dependTask;

			[CompilerGenerated]
			private readonly bool _003CIsMain_003Ek__BackingField;

			[CompilerGenerated]
			private readonly object[] _003CDependencyAssets_003Ek__BackingField;

			public override bool IsMain
			{
				[CompilerGenerated]
				get
				{
					return _003CIsMain_003Ek__BackingField;
				}
			}

			public override bool IsScene
			{
				get
				{
					return false;
				}
			}

			public override bool WasLoadDependencyFinish
			{
				get
				{
					return _loadedDependencyAssetCount == _dependencyAssetNames.Count;
				}
			}

			public override object[] DependencyAssets
			{
				[CompilerGenerated]
				get
				{
					return _003CDependencyAssets_003Ek__BackingField;
				}
			}

			public AssetLoadTask(ResourceName assetName, BundleInfo bundleInfo, List<ResourceName> dependencyAssetNames, AssetLoadCallbacks loadCallbacks, object userData)
				: base(assetName, bundleInfo)
			{
				_003CIsMain_003Ek__BackingField = true;
				_loadCallbacks = loadCallbacks;
				_userData = userData;
				_dependencyAssetNames = dependencyAssetNames;
				_003CDependencyAssets_003Ek__BackingField = new object[dependencyAssetNames.Count];
			}

			public AssetLoadTask(ResourceLoadTask dependTask, ResourceName assetName, BundleInfo bundleInfo, List<ResourceName> dependencyAssetNames)
				: base(assetName, bundleInfo)
			{
				_003CIsMain_003Ek__BackingField = false;
				_dependTask = dependTask;
				_dependencyAssetNames = dependencyAssetNames;
				_003CDependencyAssets_003Ek__BackingField = new object[dependencyAssetNames.Count];
			}

			public override void LoadSuccess(object asset)
			{
				ResourceLoadTask dependTask = _dependTask;
				if (dependTask != null)
				{
					dependTask.AddDependency(base.AssetName, asset);
				}
				if (_loadCallbacks.Success != null)
				{
					float duration = (float)(DateTime.Now - base.StartTime).TotalSeconds;
					AssetLoadSuccess success = _loadCallbacks.Success;
					if (success != null)
					{
						success(base.AssetName.ToString(), asset, duration, _userData);
					}
				}
			}

			public override void LoadFailure(ResourceLoadStatus loadStatus, string message)
			{
				if (_dependTask != null)
				{
					string message2 = "Can not load dependency asset " + base.AssetName.ToString() + ", status is " + loadStatus.ToString() + ", error message is " + message + ".";
					_dependTask.LoadFailure(ResourceLoadStatus.DependencyError, message2);
				}
				AssetLoadFailure failure = _loadCallbacks.Failure;
				if (failure != null)
				{
					failure(base.AssetName.ToString(), message, _userData);
				}
			}

			public override void LoadUpdate(ResourceLoadStatus loadStatus, float progress)
			{
				if (loadStatus == ResourceLoadStatus.LoadingAsset)
				{
					AssetLoadUpdate update = _loadCallbacks.Update;
					if (update != null)
					{
						update(base.AssetName.ToString(), progress, _userData);
					}
				}
			}

			public override void AddDependency(ResourceName assetName, object asset)
			{
				if (asset == null)
				{
					return;
				}
				int num = -1;
				for (int i = 0; i < _dependencyAssetNames.Count; i++)
				{
					if (_dependencyAssetNames[i] == assetName)
					{
						num = i;
					}
				}
				if (num != -1)
				{
					_loadedDependencyAssetCount++;
					DependencyAssets[num] = asset;
					AssetLoadDependency dependency = _loadCallbacks.Dependency;
					if (dependency != null)
					{
						dependency(base.AssetName.ToString(), assetName.ToString(), _loadedDependencyAssetCount, _dependencyAssetNames.Count, _userData);
					}
				}
			}
		}

		private sealed class AssetObject : SharedObject
		{
			private AssetBundle _bundle;

			private readonly object[] _dependencyAssets;

			public AssetObject(ResourceName name, object target, AssetBundle bundle, object[] dependencyAssets)
				: base(name.ToString(), target)
			{
				if (bundle == null)
				{
					Log.Error("Bundle is invalid.");
				}
				_bundle = bundle;
				_dependencyAssets = dependencyAssets;
			}

			protected internal override void OnRecycle()
			{
				base.OnRecycle();
				for (int i = 0; i < _dependencyAssets.Length; i++)
				{
					object target = _dependencyAssets[i];
					Mod.Resource.AssetPool.Recycle(target);
				}
			}

			protected internal override void OnUnload(bool force = false)
			{
				UnityEngine.Object @object;
				if (!(base.Target is SceneDummy) && (object)(@object = base.Target as UnityEngine.Object) != null && !(@object is GameObject))
				{
					Resources.UnloadAsset(@object);
				}
				base.OnUnload(force);
				Mod.Resource.BundlePool.Recycle(_bundle);
				_bundle = null;
			}
		}

		private sealed class BundleChecker
		{
			private enum CheckStatus
			{
				Unknown = 0,
				NeedUpdate = 1,
				NeedUpdateDelay = 2,
				StorageInReadOnly = 3,
				StorageInReadWrite = 4,
				Unavailable = 5,
				Disuse = 6
			}

			private struct LocalInfo
			{
				[CompilerGenerated]
				private readonly bool _003CExist_003Ek__BackingField;

				[CompilerGenerated]
				private readonly int _003CLength_003Ek__BackingField;

				[CompilerGenerated]
				private readonly uint _003CCrc_003Ek__BackingField;

				public bool Exist
				{
					[CompilerGenerated]
					get
					{
						return _003CExist_003Ek__BackingField;
					}
				}

				public int Length
				{
					[CompilerGenerated]
					get
					{
						return _003CLength_003Ek__BackingField;
					}
				}

				public uint Crc
				{
					[CompilerGenerated]
					get
					{
						return _003CCrc_003Ek__BackingField;
					}
				}

				public LocalInfo(int length, uint crc)
				{
					_003CExist_003Ek__BackingField = true;
					_003CLength_003Ek__BackingField = length;
					_003CCrc_003Ek__BackingField = crc;
				}
			}

			private struct RemoteInfo
			{
				[CompilerGenerated]
				private readonly bool _003CExist_003Ek__BackingField;

				[CompilerGenerated]
				private readonly BundleUpdateType _003CUpdateType_003Ek__BackingField;

				[CompilerGenerated]
				private readonly int[] _003CLevel_003Ek__BackingField;

				[CompilerGenerated]
				private readonly int _003CLength_003Ek__BackingField;

				[CompilerGenerated]
				private readonly uint _003CCrc_003Ek__BackingField;

				public bool Exist
				{
					[CompilerGenerated]
					get
					{
						return _003CExist_003Ek__BackingField;
					}
				}

				public BundleUpdateType UpdateType
				{
					[CompilerGenerated]
					get
					{
						return _003CUpdateType_003Ek__BackingField;
					}
				}

				public int[] Level
				{
					[CompilerGenerated]
					get
					{
						return _003CLevel_003Ek__BackingField;
					}
				}

				public int Length
				{
					[CompilerGenerated]
					get
					{
						return _003CLength_003Ek__BackingField;
					}
				}

				public uint Crc
				{
					[CompilerGenerated]
					get
					{
						return _003CCrc_003Ek__BackingField;
					}
				}

				public RemoteInfo(BundleUpdateType updateType, int[] level, int length, uint crc)
				{
					_003CExist_003Ek__BackingField = true;
					_003CUpdateType_003Ek__BackingField = updateType;
					_003CLevel_003Ek__BackingField = level;
					_003CLength_003Ek__BackingField = length;
					_003CCrc_003Ek__BackingField = crc;
				}
			}

			private sealed class CheckInfo
			{
				private RemoteInfo _remoteInfo;

				private LocalInfo _readOnlyInfo;

				private LocalInfo _readWriteInfo;

				private CheckStatus _status;

				private bool _needRemove;

				[CompilerGenerated]
				private readonly BundleName _003CBundleName_003Ek__BackingField;

				public BundleUpdateType UpdateType
				{
					get
					{
						return _remoteInfo.UpdateType;
					}
				}

				public int[] Level
				{
					get
					{
						return _remoteInfo.Level;
					}
				}

				public int Length
				{
					get
					{
						return _remoteInfo.Length;
					}
				}

				public uint Crc
				{
					get
					{
						return _remoteInfo.Crc;
					}
				}

				public BundleName BundleName
				{
					[CompilerGenerated]
					get
					{
						return _003CBundleName_003Ek__BackingField;
					}
				}

				public CheckStatus Status
				{
					get
					{
						return _status;
					}
				}

				public bool NeedRemove
				{
					get
					{
						return _needRemove;
					}
				}

				public CheckInfo(BundleName bundleName)
				{
					_003CBundleName_003Ek__BackingField = bundleName;
					_status = CheckStatus.Unknown;
					_needRemove = false;
				}

				public void SetRemoteInfo(BundleUpdateType updateType, int[] level, int length, uint crc)
				{
					if (_remoteInfo.Exist)
					{
						Log.Warning("You must set version info of '" + BundleName.ToString() + "' only once.");
					}
					else
					{
						_remoteInfo = new RemoteInfo(updateType, level, length, crc);
					}
				}

				public void SetReadOnlyInfo(int length, uint crc)
				{
					if (_readOnlyInfo.Exist)
					{
						Log.Warning("You must set readonly info of '" + BundleName.ToString() + "' only once.");
					}
					else
					{
						_readOnlyInfo = new LocalInfo(length, crc);
					}
				}

				public void SetReadWriteInfo(int length, uint crc)
				{
					if (_readWriteInfo.Exist)
					{
						Log.Warning("You must set read-write info of '" + BundleName.ToString() + "' only once.");
					}
					else
					{
						_readWriteInfo = new LocalInfo(length, crc);
					}
				}

				public void RefreshStatus(string variant)
				{
					if (!_remoteInfo.Exist)
					{
						_status = CheckStatus.Disuse;
						_needRemove = _readWriteInfo.Exist;
					}
					else if (!BundleName.IsVariant || BundleName.Variant.Equals(variant, StringComparison.OrdinalIgnoreCase))
					{
						if (_readOnlyInfo.Exist && _readOnlyInfo.Crc == _remoteInfo.Crc)
						{
							_status = CheckStatus.StorageInReadOnly;
							_needRemove = _readWriteInfo.Exist;
						}
						else if (_readWriteInfo.Exist && _readWriteInfo.Crc == _remoteInfo.Crc)
						{
							_status = CheckStatus.StorageInReadWrite;
							_needRemove = false;
						}
						else
						{
							_status = ((UpdateType != BundleUpdateType.LevelOnly) ? CheckStatus.NeedUpdate : CheckStatus.NeedUpdateDelay);
							_needRemove = _readWriteInfo.Exist;
						}
					}
					else
					{
						_status = CheckStatus.Unavailable;
						if (_readOnlyInfo.Exist && _readOnlyInfo.Crc == _remoteInfo.Crc)
						{
							_needRemove = _readWriteInfo.Exist;
						}
						else if (_readWriteInfo.Exist && _readWriteInfo.Crc == _remoteInfo.Crc)
						{
							_needRemove = false;
						}
						else
						{
							_needRemove = _readWriteInfo.Exist;
						}
					}
				}
			}

			private readonly Dictionary<BundleName, CheckInfo> _checkInfos = new Dictionary<BundleName, CheckInfo>(BundleNameComparer.Default);

			private string _roVersionListPath;

			private string _rwVersionListPath;

			private string _roResourceListPath;

			private string _rwResourceListPath;

			private string _roVersionListUri;

			private string _rwVersionListUri;

			private string _roResourceListUri;

			private string _rwResourceListUri;

			private MemoryStream _roStream;

			private BinaryReader _roReader;

			private bool _roResourceListReady;

			private bool _rwResourceListReady;

			public void Destroy()
			{
				if (_roReader != null)
				{
					_roReader.Close();
					_roReader = null;
				}
				if (_roStream != null)
				{
					_roStream.Close();
					_roStream = null;
				}
				_checkInfos.Clear();
			}

			public void DoCheck()
			{
				string readOnlyPath = Mod.Resource.ReadOnlyPath;
				string readWritePath = Mod.Resource.ReadWritePath;
				string resourceNameWithSuffix = PathUtility.GetResourceNameWithSuffix("list");
				string resourceNameWithSuffix2 = PathUtility.GetResourceNameWithSuffix("version");
				_roVersionListPath = PathUtility.GetCombinePath(readOnlyPath, resourceNameWithSuffix2);
				_rwVersionListPath = PathUtility.GetCombinePath(readWritePath, resourceNameWithSuffix2);
				_roResourceListPath = PathUtility.GetCombinePath(readOnlyPath, resourceNameWithSuffix);
				_rwResourceListPath = PathUtility.GetCombinePath(readWritePath, resourceNameWithSuffix);
				_roVersionListUri = PathUtility.GetRemotePath(_roVersionListPath);
				_rwVersionListUri = PathUtility.GetRemotePath(_rwVersionListPath);
				_roResourceListUri = PathUtility.GetRemotePath(_roResourceListPath);
				_rwResourceListUri = PathUtility.GetRemotePath(_rwResourceListPath);
				Mod.Resource.LoadBytes(_roVersionListUri, ParseReadOnlyVersionList);
			}

			private void SetRemoteInfo(BundleName bundleName, BundleUpdateType updateType, int[] level, int length, uint crc)
			{
				GetOrAddCheckInfo(bundleName).SetRemoteInfo(updateType, level, length, crc);
			}

			private void SetReadOnlyInfo(BundleName bundleName, int length, uint crc)
			{
				GetOrAddCheckInfo(bundleName).SetReadOnlyInfo(length, crc);
			}

			private void SetReadWriteInfo(BundleName bundleName, int length, uint crc)
			{
				GetOrAddCheckInfo(bundleName).SetReadWriteInfo(length, crc);
			}

			private CheckInfo GetOrAddCheckInfo(BundleName bundleName)
			{
				CheckInfo value;
				if (_checkInfos.TryGetValue(bundleName, out value) && value != null)
				{
					return value;
				}
				value = new CheckInfo(bundleName);
				_checkInfos[value.BundleName] = value;
				return value;
			}

			private void RefreshStatus()
			{
				if (!_roResourceListReady || !_rwResourceListReady)
				{
					return;
				}
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				foreach (KeyValuePair<BundleName, CheckInfo> checkInfo in _checkInfos)
				{
					CheckInfo value = checkInfo.Value;
					value.RefreshStatus(Mod.Resource.Variant);
					switch (value.Status)
					{
					case CheckStatus.StorageInReadOnly:
						ProcessBundleInfo(value.BundleName, value.Length, value.Crc, true);
						break;
					case CheckStatus.StorageInReadWrite:
						ProcessBundleInfo(value.BundleName, value.Length, value.Crc, false);
						break;
					case CheckStatus.NeedUpdate:
						num2++;
						num3 += value.Length;
						Mod.Resource.OnBundleNeedUpdate(value.BundleName, value.Length, value.Crc);
						break;
					case CheckStatus.NeedUpdateDelay:
						Mod.Resource.OnBundleNeedUpdateDelay(value.BundleName, value.Length, value.Crc, value.Level);
						break;
					default:
						Log.Error("Check bundle '" + value.BundleName.ToString() + "' error with unknown status.");
						break;
					case CheckStatus.Unavailable:
					case CheckStatus.Disuse:
						break;
					}
					if (value.NeedRemove)
					{
						num++;
						string combinePath = PathUtility.GetCombinePath(Mod.Resource.ReadWritePath, PathUtility.GetResourceNameWithSuffix(value.BundleName.ToString()));
						File.Delete(combinePath);
						Mod.Resource.ReadWriteBundleInfos.Remove(value.BundleName);
					}
				}
				Mod.Resource.OnBundleCheckComplete(num, num2, num3);
			}

			private void ParseReadOnlyVersionList(string fileUri, byte[] bytes, string message)
			{
				if (bytes == null || bytes.Length == 0)
				{
					Log.Error("ABORT: Read only version list load error, " + message);
					return;
				}
				try
				{
					_roStream = new MemoryStream(bytes);
					_roReader = new BinaryReader(_roStream);
					int internalVersion;
					if (!ParseVersionListHeader(_roReader, out internalVersion))
					{
						Log.Error("ABORT: Read only version list is invalid.");
						return;
					}
					Mod.Resource.InternalVersion = internalVersion;
					if (File.Exists(_rwVersionListPath))
					{
						Mod.Resource.LoadBytes(_rwVersionListUri, ParseReadWriteVersionList);
						return;
					}
					ParseVersionListBody(_roReader);
					if (_roReader != null)
					{
						_roReader.Close();
						_roReader = null;
					}
					if (_roStream != null)
					{
						_roStream.Close();
						_roStream = null;
					}
					if (File.Exists(_rwResourceListPath))
					{
						Mod.Resource.LoadBytes(_rwResourceListUri, ParseReadWriteResourceList);
					}
					else
					{
						_rwResourceListReady = true;
					}
					Mod.Resource.LoadBytes(_roResourceListUri, ParseReadOnlyResourceList);
				}
				catch
				{
					Log.Error("ABORT: Read only version list parse error.");
					if (_roReader != null)
					{
						_roReader.Close();
						_roReader = null;
					}
					if (_roStream != null)
					{
						_roStream.Close();
						_roStream = null;
					}
				}
			}

			private void ParseReadWriteVersionList(string fileUri, byte[] bytes, string message)
			{
				if (bytes == null || bytes.Length == 0)
				{
					Log.Warning("Read write version list load error, " + message);
					File.Delete(_rwVersionListPath);
					ParseVersionListBody(_roReader);
					if (_roReader != null)
					{
						_roReader.Close();
						_roReader = null;
					}
					if (_roStream != null)
					{
						_roStream.Close();
						_roStream = null;
					}
					if (File.Exists(_rwResourceListPath))
					{
						Mod.Resource.LoadBytes(_rwResourceListUri, ParseReadWriteResourceList);
					}
					else
					{
						_rwResourceListReady = true;
					}
					Mod.Resource.LoadBytes(_roResourceListUri, ParseReadOnlyResourceList);
					return;
				}
				using (MemoryStream input = new MemoryStream(bytes))
				{
					using (BinaryReader reader = new BinaryReader(input))
					{
						int internalVersion;
						if (!ParseVersionListHeader(reader, out internalVersion) || internalVersion < Mod.Resource.InternalVersion)
						{
							Log.Warning("Read write version list is invalid");
							File.Delete(_rwVersionListPath);
							ParseVersionListBody(_roReader);
							if (_roReader != null)
							{
								_roReader.Close();
								_roReader = null;
							}
							if (_roStream != null)
							{
								_roStream.Close();
								_roStream = null;
							}
							if (File.Exists(_rwResourceListPath))
							{
								Mod.Resource.LoadBytes(_rwResourceListUri, ParseReadWriteResourceList);
							}
							else
							{
								_rwResourceListReady = true;
							}
							Mod.Resource.LoadBytes(_roResourceListUri, ParseReadOnlyResourceList);
						}
						else
						{
							Mod.Resource.InternalVersion = internalVersion;
							ParseVersionListBody(reader);
							if (File.Exists(_rwResourceListPath))
							{
								Mod.Resource.LoadBytes(_rwResourceListUri, ParseReadWriteResourceList);
							}
							else
							{
								_rwResourceListReady = true;
							}
							Mod.Resource.LoadBytes(_roResourceListUri, ParseReadOnlyResourceList);
						}
					}
				}
			}

			private bool ParseVersionListHeader(BinaryReader reader, out int internalVersion)
			{
				internalVersion = 0;
				char[] array = reader.ReadChars(3);
				if (array[0] != VersionListHeader[0] || array[1] != VersionListHeader[1] || array[2] != VersionListHeader[2])
				{
					Log.Warning("Version list header is invalid.");
					return false;
				}
				byte b = reader.ReadByte();
				if (b != 3)
				{
					Log.Warning("Version list version is invalid.");
					return false;
				}
				string @string = Encoding.UTF8.GetString(reader.ReadBytes(reader.ReadByte()));
				if (Mod.Resource.ApplicableVersion != @string)
				{
					Log.Warning("Version list applicable version is invalid.");
					return false;
				}
				internalVersion = reader.ReadInt32();
				return true;
			}

			private void ParseVersionListBody(BinaryReader reader)
			{
				int num = reader.ReadInt32();
				BundleName[] array = new BundleName[num];
				int[] array2 = new int[num];
				Dictionary<ResourceName, ResourceName[]> dictionary = new Dictionary<ResourceName, ResourceName[]>(ResourceNameComparer.Default);
				for (int i = 0; i < num; i++)
				{
					string @string = Encoding.UTF8.GetString(reader.ReadBytes(reader.ReadByte()));
					string text = null;
					byte b = reader.ReadByte();
					if (b > 0)
					{
						text = Encoding.UTF8.GetString(reader.ReadBytes(b));
					}
					array[i] = new BundleName(@string, text);
					BundleUpdateType updateType = (BundleUpdateType)reader.ReadByte();
					int num2 = reader.ReadInt32();
					int[] array3 = new int[num2];
					for (int j = 0; j < num2; j++)
					{
						array3[j] = reader.ReadInt32();
					}
					array2[i] = reader.ReadInt32();
					uint crc = reader.ReadUInt32();
					int num3 = reader.ReadInt32();
					ResourceName[] array4 = new ResourceName[num3];
					for (int k = 0; k < num3; k++)
					{
						ResourceName resourceName = new ResourceName(Encoding.UTF8.GetString(reader.ReadBytes(reader.ReadByte())));
						array4[k] = resourceName;
						int num4 = reader.ReadInt32();
						ResourceName[] array5 = new ResourceName[num4];
						for (int l = 0; l < num4; l++)
						{
							ResourceName resourceName2 = new ResourceName(Encoding.UTF8.GetString(reader.ReadBytes(reader.ReadByte())));
							array5[l] = resourceName2;
						}
						if (array[i].Variant == null || array[i].Variant.Equals(Mod.Resource.Variant, StringComparison.OrdinalIgnoreCase))
						{
							dictionary.Add(array4[k], array5);
						}
					}
					SetRemoteInfo(array[i], updateType, array3, array2[i], crc);
					if (string.IsNullOrEmpty(text) || text.Equals(Mod.Resource.Variant, StringComparison.OrdinalIgnoreCase))
					{
						ProcessAssetInfo(array[i], array4);
					}
				}
				ProcessAssetDependencyInfo(dictionary);
			}

			private void ParseReadOnlyResourceList(string fileUri, byte[] bytes, string message)
			{
				if (_roResourceListReady)
				{
					Log.Warning("Read only resources list has been parsed.");
					return;
				}
				if (bytes == null || bytes.Length == 0)
				{
					Log.Warning("Read only resources list load error, " + message);
					_roResourceListReady = true;
					RefreshStatus();
					return;
				}
				if (!ParseResourceList(bytes, true))
				{
					Log.Error("Read only resources list is invalid.");
				}
				_roResourceListReady = true;
				RefreshStatus();
			}

			private void ParseReadWriteResourceList(string fileUri, byte[] bytes, string message)
			{
				if (_rwResourceListReady)
				{
					Log.Warning("Read write resources list has been parsed.");
					return;
				}
				if (bytes == null || bytes.Length == 0)
				{
					_rwResourceListReady = true;
					RefreshStatus();
					return;
				}
				if (!ParseResourceList(bytes, false))
				{
					Log.Warning("Read write resources list is invalid.");
					File.Delete(_rwResourceListPath);
				}
				_rwResourceListReady = true;
				RefreshStatus();
			}

			private bool ParseResourceList(byte[] bytes, bool readOnly)
			{
				try
				{
					using (MemoryStream input = new MemoryStream(bytes))
					{
						using (BinaryReader binaryReader = new BinaryReader(input))
						{
							char[] array = binaryReader.ReadChars(3);
							if (array[0] != ResourceListHeader[0] || array[1] != ResourceListHeader[1] || array[2] != ResourceListHeader[2])
							{
								Log.Warning("Resources list header is invalid.");
								return false;
							}
							byte b = binaryReader.ReadByte();
							if (b != 3)
							{
								Log.Warning("Resources list version is invalid.");
								return false;
							}
							int num = binaryReader.ReadInt32();
							for (int i = 0; i < num; i++)
							{
								string @string = Encoding.UTF8.GetString(binaryReader.ReadBytes(binaryReader.ReadByte()));
								string variant = null;
								byte b2 = binaryReader.ReadByte();
								if (b2 > 0)
								{
									variant = Encoding.UTF8.GetString(binaryReader.ReadBytes(b2));
								}
								int length = binaryReader.ReadInt32();
								uint crc = binaryReader.ReadUInt32();
								BundleName bundleName = new BundleName(@string, variant);
								if (readOnly)
								{
									SetReadOnlyInfo(bundleName, length, crc);
									continue;
								}
								SetReadWriteInfo(bundleName, length, crc);
								if (Mod.Resource.ReadWriteBundleInfos.ContainsKey(bundleName))
								{
									Log.Warning("Bundle info '" + bundleName.ToString() + "' is already exist.");
								}
								else
								{
									Mod.Resource.ReadWriteBundleInfos[bundleName] = new BundleRwInfo(length, crc);
								}
							}
						}
					}
					return true;
				}
				catch (Exception ex)
				{
					Log.Error("Parse resource list exception '" + ex.StackTrace + "'.");
					return false;
				}
			}

			private void ProcessAssetInfo(BundleName bundleName, ResourceName[] assetNames)
			{
				for (int i = 0; i < assetNames.Length; i++)
				{
					Mod.Resource.AssetInfos.Add(assetNames[i], new AssetInfo(assetNames[i], bundleName));
				}
			}

			private void ProcessAssetDependencyInfo(Dictionary<ResourceName, ResourceName[]> assetDependencyAssetNames)
			{
				foreach (KeyValuePair<ResourceName, ResourceName[]> assetDependencyAssetName in assetDependencyAssetNames)
				{
					List<ResourceName> list = new List<ResourceName>();
					List<ResourceName> list2 = new List<ResourceName>();
					for (int i = 0; i < assetDependencyAssetName.Value.Length; i++)
					{
						ResourceName resourceName = assetDependencyAssetName.Value[i];
						AssetInfo assetInfo;
						if (Mod.Resource.TryGetAssetInfo(resourceName, out assetInfo))
						{
							list.Add(resourceName);
						}
						else
						{
							list2.Add(resourceName);
						}
					}
					Mod.Resource.AssetDependencyInfos[assetDependencyAssetName.Key] = new AssetDependencyInfo(assetDependencyAssetName.Key, list, list2);
				}
			}

			private void ProcessBundleInfo(BundleName bundleName, int length, uint crc, bool storageInReadOnly)
			{
				if (Mod.Resource.BundleInfos.ContainsKey(bundleName))
				{
					Log.Warning("BundleInfo '" + bundleName.ToString() + "' is already exist.");
				}
				else
				{
					Mod.Resource.BundleInfos[bundleName] = new BundleInfo(bundleName, length, crc, storageInReadOnly);
				}
			}
		}

		private sealed class BundleObject : SharedObject
		{
			public BundleObject(BundleName name, AssetBundle target)
				: base(name.ToString(), target)
			{
			}

			protected internal override void OnRecycle()
			{
				base.OnRecycle();
			}

			protected internal override void OnUnload(bool force = false)
			{
				AssetBundle assetBundle = base.Target as AssetBundle;
				if (assetBundle != null)
				{
					assetBundle.Unload(true);
				}
				base.OnUnload(force);
			}
		}

		private struct BundleRwInfo
		{
			[CompilerGenerated]
			private readonly int _003CLength_003Ek__BackingField;

			[CompilerGenerated]
			private readonly uint _003CCrc_003Ek__BackingField;

			public int Length
			{
				[CompilerGenerated]
				get
				{
					return _003CLength_003Ek__BackingField;
				}
			}

			public uint Crc
			{
				[CompilerGenerated]
				get
				{
					return _003CCrc_003Ek__BackingField;
				}
			}

			public BundleRwInfo(int length, uint crc)
			{
				_003CLength_003Ek__BackingField = length;
				_003CCrc_003Ek__BackingField = crc;
			}
		}

		private sealed class BundleUpdater
		{
			private sealed class UpdateInfo
			{
				[CompilerGenerated]
				private readonly BundleName _003CBundleName_003Ek__BackingField;

				[CompilerGenerated]
				private readonly int _003CLength_003Ek__BackingField;

				[CompilerGenerated]
				private readonly uint _003CCrc_003Ek__BackingField;

				[CompilerGenerated]
				private readonly string _003CPath_003Ek__BackingField;

				[CompilerGenerated]
				private readonly string _003CUri_003Ek__BackingField;

				[CompilerGenerated]
				private readonly int _003CRetryCount_003Ek__BackingField;

				public BundleName BundleName
				{
					[CompilerGenerated]
					get
					{
						return _003CBundleName_003Ek__BackingField;
					}
				}

				public int Length
				{
					[CompilerGenerated]
					get
					{
						return _003CLength_003Ek__BackingField;
					}
				}

				public uint Crc
				{
					[CompilerGenerated]
					get
					{
						return _003CCrc_003Ek__BackingField;
					}
				}

				public string Path
				{
					[CompilerGenerated]
					get
					{
						return _003CPath_003Ek__BackingField;
					}
				}

				public string Uri
				{
					[CompilerGenerated]
					get
					{
						return _003CUri_003Ek__BackingField;
					}
				}

				public int RetryCount
				{
					[CompilerGenerated]
					get
					{
						return _003CRetryCount_003Ek__BackingField;
					}
				}

				public UpdateInfo(BundleName bundleName, int length, uint crc, string path, string uri, int retryCount)
				{
					_003CBundleName_003Ek__BackingField = bundleName;
					_003CLength_003Ek__BackingField = length;
					_003CCrc_003Ek__BackingField = crc;
					_003CPath_003Ek__BackingField = path;
					_003CUri_003Ek__BackingField = uri;
					_003CRetryCount_003Ek__BackingField = retryCount;
				}
			}

			private readonly Queue<UpdateInfo> _updateWaitingInfo = new Queue<UpdateInfo>();

			private readonly Dictionary<BundleName, int> _updatingTaskId = new Dictionary<BundleName, int>(BundleNameComparer.Default);

			private bool _bundleCheckComplete;

			private bool _updateAllowed;

			private bool _updateComplete;

			public int UpdatingCount { get; private set; }

			public int WaitingUpdateCount
			{
				get
				{
					return _updateWaitingInfo.Count;
				}
			}

			public BundleUpdater()
			{
				Mod.Event.Subscribe(EventArgs<DownloadMod.StartEventArgs>.EventId, OnDownloadStart);
				Mod.Event.Subscribe(EventArgs<DownloadMod.UpdateEventArgs>.EventId, OnDownloadUpdate);
				Mod.Event.Subscribe(EventArgs<DownloadMod.SuccessEventArgs>.EventId, OnDownloadSuccess);
				Mod.Event.Subscribe(EventArgs<DownloadMod.FailureEventArgs>.EventId, OnDownloadFailure);
			}

			public void DoUpdate()
			{
				if (!_bundleCheckComplete)
				{
					Log.Warning("You must check bundle complete first.");
				}
				else
				{
					_updateAllowed = true;
				}
			}

			public void Destroy()
			{
				Mod.Event.Unsubscribe(EventArgs<DownloadMod.StartEventArgs>.EventId, OnDownloadStart);
				Mod.Event.Unsubscribe(EventArgs<DownloadMod.UpdateEventArgs>.EventId, OnDownloadUpdate);
				Mod.Event.Unsubscribe(EventArgs<DownloadMod.SuccessEventArgs>.EventId, OnDownloadSuccess);
				Mod.Event.Unsubscribe(EventArgs<DownloadMod.FailureEventArgs>.EventId, OnDownloadFailure);
				_updateWaitingInfo.Clear();
				foreach (KeyValuePair<BundleName, int> item in _updatingTaskId)
				{
					Mod.Download.RemoveTask(item.Value);
				}
			}

			public void Tick(float elapseSeconds, float realElapseSeconds)
			{
				Profiler.BeginSample("ResourceMod.BundleUpdater.Tick");
				if (_updateAllowed && !_updateComplete)
				{
					if (_updateWaitingInfo.Count > 0)
					{
						if (Mod.Download.FreeAgentCount > 0)
						{
							UpdateInfo updateInfo = _updateWaitingInfo.Dequeue();
							Mod.Download.AddTask(updateInfo.Path, updateInfo.Uri, updateInfo);
							UpdatingCount++;
						}
					}
					else if (UpdatingCount <= 0)
					{
						_updateComplete = true;
						PathUtility.RemoveEmptyDirectory(Mod.Resource.ReadWritePath);
						Mod.Resource.OnBundleUpdateAllComplete();
					}
				}
				Profiler.EndSample();
			}

			public void AddBundleToUpdate(BundleName resourceName, int length, uint crc, string downloadPath, string downloadUri, int retryCount)
			{
				_updateWaitingInfo.Enqueue(new UpdateInfo(resourceName, length, crc, downloadPath, downloadUri, retryCount));
			}

			public void BundleCheckComplete(bool needGenerateRwResourceList)
			{
				_bundleCheckComplete = true;
				if (needGenerateRwResourceList)
				{
					GenerateReadWriteResourceList();
				}
			}

			private void GenerateReadWriteResourceList()
			{
				string combinePath = PathUtility.GetCombinePath(Mod.Resource.ReadWritePath, PathUtility.GetResourceNameWithSuffix("list"));
				string text = combinePath + ".bak";
				if (File.Exists(combinePath))
				{
					if (File.Exists(text))
					{
						File.Delete(text);
					}
					File.Move(combinePath, text);
				}
				using (FileStream output = new FileStream(combinePath, FileMode.CreateNew, FileAccess.Write))
				{
					try
					{
						using (BinaryWriter binaryWriter = new BinaryWriter(output))
						{
							binaryWriter.Write(ResourceListHeader);
							binaryWriter.Write((byte)3);
							SortedDictionary<BundleName, BundleRwInfo> readWriteBundleInfos = Mod.Resource.ReadWriteBundleInfos;
							binaryWriter.Write(readWriteBundleInfos.Count);
							foreach (KeyValuePair<BundleName, BundleRwInfo> item in readWriteBundleInfos)
							{
								byte[] bytes = Encoding.UTF8.GetBytes(item.Key.Name);
								binaryWriter.Write((byte)bytes.Length);
								binaryWriter.Write(bytes);
								if (!item.Key.IsVariant)
								{
									binaryWriter.Write((byte)0);
								}
								else
								{
									byte[] bytes2 = Encoding.UTF8.GetBytes(item.Key.Variant);
									binaryWriter.Write((byte)bytes2.Length);
									binaryWriter.Write(bytes2);
								}
								binaryWriter.Write(item.Value.Length);
								binaryWriter.Write(item.Value.Crc);
							}
							binaryWriter.Close();
						}
						if (File.Exists(text))
						{
							File.Delete(text);
						}
					}
					catch (Exception ex)
					{
						if (File.Exists(combinePath))
						{
							File.Delete(combinePath);
						}
						if (File.Exists(text))
						{
							File.Move(text, combinePath);
						}
						Log.Error("Pack save exception '" + ex.Message + "'.", ex);
					}
				}
			}

			private void OnDownloadStart(object sender, EventArgs args)
			{
				DownloadMod.StartEventArgs startEventArgs = (DownloadMod.StartEventArgs)args;
				UpdateInfo updateInfo;
				if ((updateInfo = startEventArgs.UserData as UpdateInfo) != null)
				{
					if (_updatingTaskId.ContainsKey(updateInfo.BundleName))
					{
						_updatingTaskId[updateInfo.BundleName] = startEventArgs.TaskId;
					}
					else
					{
						_updatingTaskId.Add(updateInfo.BundleName, startEventArgs.TaskId);
					}
					Mod.Resource.OnBundleUpdateStart(updateInfo.BundleName, startEventArgs.Path, startEventArgs.Uri, updateInfo.Length, updateInfo.RetryCount);
				}
			}

			private void OnDownloadUpdate(object sender, EventArgs args)
			{
				DownloadMod.UpdateEventArgs updateEventArgs = (DownloadMod.UpdateEventArgs)args;
				UpdateInfo updateInfo;
				if ((updateInfo = updateEventArgs.UserData as UpdateInfo) == null)
				{
					return;
				}
				if (updateEventArgs.Length > updateInfo.Length)
				{
					if (File.Exists(updateEventArgs.Path))
					{
						File.Delete(updateEventArgs.Path);
					}
					string message = "When download update, downloaded length is larger than length, need '" + updateInfo.Length + "', but current '" + updateEventArgs.Length + "'.";
					Mod.Event.Fire(this, DownloadMod.FailureEventArgs.Make(updateEventArgs.TaskId, updateEventArgs.Path, updateEventArgs.Uri, message, updateEventArgs.UserData));
				}
				else
				{
					Mod.Resource.OnBundleUpdateChanged(updateInfo.BundleName, updateEventArgs.Path, updateEventArgs.Uri, updateEventArgs.Length, updateInfo.Length);
				}
			}

			private void OnDownloadSuccess(object sender, EventArgs args)
			{
				DownloadMod.SuccessEventArgs successEventArgs = (DownloadMod.SuccessEventArgs)args;
				UpdateInfo updateInfo;
				if ((updateInfo = successEventArgs.UserData as UpdateInfo) != null)
				{
					UpdatingCount--;
					if (Mod.Resource.BundleInfos.ContainsKey(updateInfo.BundleName))
					{
						Log.Warning("Resource info '" + updateInfo.BundleName.ToString() + "' is already exist.");
					}
					Mod.Resource.BundleInfos[updateInfo.BundleName] = new BundleInfo(updateInfo.BundleName, updateInfo.Length, updateInfo.Crc, false);
					if (Mod.Resource.ReadWriteBundleInfos.ContainsKey(updateInfo.BundleName))
					{
						Log.Warning("Read-write resource info '" + updateInfo.BundleName.ToString() + "' is already exist.");
					}
					Mod.Resource.ReadWriteBundleInfos[updateInfo.BundleName] = new BundleRwInfo(updateInfo.Length, updateInfo.Crc);
					GenerateReadWriteResourceList();
					_updatingTaskId.Remove(updateInfo.BundleName);
					Mod.Resource.OnBundleUpdateSuccess(updateInfo.BundleName, successEventArgs.Path, successEventArgs.Uri, updateInfo.Length, updateInfo.Length);
				}
			}

			private void OnDownloadFailure(object sender, EventArgs args)
			{
				DownloadMod.FailureEventArgs failureEventArgs = (DownloadMod.FailureEventArgs)args;
				UpdateInfo updateInfo;
				if ((updateInfo = failureEventArgs.UserData as UpdateInfo) != null)
				{
					if (File.Exists(failureEventArgs.Path))
					{
						File.Delete(failureEventArgs.Path);
					}
					if (updateInfo.RetryCount < Mod.Resource.BundleUpdateRetryCount && _updateAllowed)
					{
						UpdatingCount--;
						UpdateInfo item = new UpdateInfo(updateInfo.BundleName, updateInfo.Length, updateInfo.Crc, updateInfo.Path, updateInfo.Uri, updateInfo.RetryCount + 1);
						_updateWaitingInfo.Enqueue(item);
					}
					else
					{
						_updatingTaskId.Remove(updateInfo.BundleName);
						Mod.Resource.OnBundleUpdateFailure(updateInfo.BundleName, failureEventArgs.Uri, updateInfo.RetryCount, failureEventArgs.Message);
					}
				}
			}
		}

		private enum BundleUpdateType
		{
			Unknown = 0,
			Global = 1,
			LevelOnly = 2
		}

		private sealed class LevelBundleChecker
		{
			private struct LevelBundleInfo
			{
				[CompilerGenerated]
				private readonly BundleName _003CBundleName_003Ek__BackingField;

				[CompilerGenerated]
				private readonly int _003CLength_003Ek__BackingField;

				[CompilerGenerated]
				private readonly uint _003CCrc_003Ek__BackingField;

				[CompilerGenerated]
				private readonly string _003CPath_003Ek__BackingField;

				[CompilerGenerated]
				private readonly string _003CUri_003Ek__BackingField;

				[CompilerGenerated]
				private readonly int _003CRetryCount_003Ek__BackingField;

				public BundleName BundleName
				{
					[CompilerGenerated]
					get
					{
						return _003CBundleName_003Ek__BackingField;
					}
				}

				public int Length
				{
					[CompilerGenerated]
					get
					{
						return _003CLength_003Ek__BackingField;
					}
				}

				public uint Crc
				{
					[CompilerGenerated]
					get
					{
						return _003CCrc_003Ek__BackingField;
					}
				}

				public string Path
				{
					[CompilerGenerated]
					get
					{
						return _003CPath_003Ek__BackingField;
					}
				}

				public string Uri
				{
					[CompilerGenerated]
					get
					{
						return _003CUri_003Ek__BackingField;
					}
				}

				public int RetryCount
				{
					[CompilerGenerated]
					get
					{
						return _003CRetryCount_003Ek__BackingField;
					}
				}

				public LevelBundleInfo(int[] level, BundleName bundleName, int length, uint crc, string path, string uri, int retryCount)
				{
					_003CBundleName_003Ek__BackingField = bundleName;
					_003CLength_003Ek__BackingField = length;
					_003CCrc_003Ek__BackingField = crc;
					_003CPath_003Ek__BackingField = path;
					_003CUri_003Ek__BackingField = uri;
					_003CRetryCount_003Ek__BackingField = retryCount;
				}
			}

			private readonly Dictionary<int, List<LevelBundleInfo>> _levelBundleInfos = new Dictionary<int, List<LevelBundleInfo>>();

			public LevelBundleChecker()
			{
				Mod.Event.Subscribe(EventArgs<BundleUpdateSuccessEventArgs>.EventId, OnBundleUpdateSuccess);
			}

			public void Destroy()
			{
				Mod.Event.Unsubscribe(EventArgs<BundleUpdateSuccessEventArgs>.EventId, OnBundleUpdateSuccess);
				_levelBundleInfos.Clear();
			}

			public void DoCheck(int level)
			{
				int num = 0;
				int num2 = 0;
				bool needUpdate = false;
				List<LevelBundleInfo> value;
				if (_levelBundleInfos.TryGetValue(level, out value) && value.Count > 0)
				{
					needUpdate = true;
					foreach (LevelBundleInfo item in value)
					{
						num++;
						num2 += item.Length;
					}
				}
				Mod.Resource.OnLevelBundleCheckComplete(level, needUpdate, num, num2);
			}

			public void ReadyToUpdate(int level)
			{
				List<LevelBundleInfo> value;
				if (!_levelBundleInfos.TryGetValue(level, out value))
				{
					return;
				}
				foreach (LevelBundleInfo item in value)
				{
					Mod.Resource.OnLevelBundleNeedUpdate(item.BundleName, item.Length, item.Crc, item.Path, item.Uri, item.RetryCount);
				}
			}

			public void AddNeedUpdateLevelBundle(int[] level, BundleName bundleName, int length, uint crc, string downloadPath, string downloadUri)
			{
				LevelBundleInfo levelBundleInfo = new LevelBundleInfo(level, bundleName, length, crc, downloadPath, downloadUri, 0);
				AddLevelBundleInfo(level, levelBundleInfo);
			}

			private void AddLevelBundleInfo(int[] level, LevelBundleInfo levelBundleInfo)
			{
				for (int i = 0; i < level.Length; i++)
				{
					if (!_levelBundleInfos.ContainsKey(level[i]))
					{
						_levelBundleInfos.Add(level[i], new List<LevelBundleInfo>());
					}
					_levelBundleInfos[level[i]].Add(levelBundleInfo);
				}
			}

			private void OnBundleUpdateSuccess(object sender, EventArgs args)
			{
				BundleUpdateSuccessEventArgs bundleUpdateSuccessEventArgs = (BundleUpdateSuccessEventArgs)args;
				foreach (KeyValuePair<int, List<LevelBundleInfo>> levelBundleInfo in _levelBundleInfos)
				{
					List<LevelBundleInfo> value = levelBundleInfo.Value;
					for (int i = 0; i < value.Count; i++)
					{
						if (value[i].BundleName.ToString().Equals(bundleUpdateSuccessEventArgs.Name))
						{
							value.RemoveAt(i);
						}
					}
				}
			}
		}

		private sealed class ResourceLoadAgent : ITaskAgent<ResourceLoadTask>
		{
			private ResourceLoadWaitingType _waitingType;

			private AssetBundleCreateRequest _loadBundleFileAsyncRequest;

			private AsyncOperation _loadAssetAsyncOperation;

			private AssetBundleRequest _loadAssetAsyncRequest;

			public ResourceLoadTask Task { get; private set; }

			void ITaskAgent<ResourceLoadTask>.Init()
			{
			}

			void ITaskAgent<ResourceLoadTask>.Boot(ResourceLoadTask task)
			{
				Task = task;
				Task.StartTime = DateTime.Now;
				_waitingType = ResourceLoadWaitingType.WaitForDependencyAsset;
				LoadDependencyAsset();
			}

			void ITaskAgent<ResourceLoadTask>.Tick(float elapseSeconds, float realElapseSeconds)
			{
				Profiler.BeginSample("ResourceMod.ResourceLoadAgent.Tick");
				if (Task.Status == TaskStatus.Doing)
				{
					if (_loadBundleFileAsyncRequest != null)
					{
						UpdateLoadFile();
					}
					if (_loadAssetAsyncRequest != null)
					{
						UpdateLoadAsset();
					}
					if (_loadAssetAsyncOperation != null)
					{
						UpdateLoadScene();
					}
					switch (_waitingType)
					{
					case ResourceLoadWaitingType.WaitForDependencyAsset:
						LoadDependencyAsset();
						break;
					case ResourceLoadWaitingType.WaitForBundle:
						LoadBundle();
						break;
					case ResourceLoadWaitingType.WaitForAsset:
						LoadAsset();
						break;
					}
					Profiler.EndSample();
				}
			}

			void ITaskAgent<ResourceLoadTask>.Recycle()
			{
				Task = null;
				_loadBundleFileAsyncRequest = null;
				_loadAssetAsyncRequest = null;
				_loadAssetAsyncOperation = null;
			}

			void ITaskAgent<ResourceLoadTask>.Exit()
			{
			}

			private void LoadDependencyAsset()
			{
				if (Task.WasLoadDependencyFinish)
				{
					_waitingType = ResourceLoadWaitingType.WaitForBundle;
					LoadBundle();
				}
			}

			private void LoadBundle()
			{
				if (!Mod.Resource.LoadingBundleNames.Contains(Task.BundleInfo.BundleName))
				{
					BundleObject bundleObject = Mod.Resource.BundlePool.Spawn(Task.BundleInfo.BundleName.ToString());
					if (bundleObject != null)
					{
						Task.Bundle = (AssetBundle)bundleObject.Target;
						_waitingType = ResourceLoadWaitingType.WaitForAsset;
						LoadAsset();
					}
					else
					{
						Mod.Resource.LoadingBundleNames.Add(Task.BundleInfo.BundleName);
						string combinePath = PathUtility.GetCombinePath(Task.BundleInfo.StorageInReadOnly ? Mod.Resource.ReadOnlyPath : Mod.Resource.ReadWritePath, PathUtility.GetResourceNameWithSuffix(Task.BundleInfo.BundleName.ToString()));
						_loadBundleFileAsyncRequest = AssetBundle.LoadFromFileAsync(combinePath, Task.BundleInfo.Crc, 16uL);
					}
				}
			}

			private void LoadAsset()
			{
				if (Mod.Resource.LoadingAssetNames.Contains(Task.AssetName))
				{
					return;
				}
				if (!Task.IsScene)
				{
					AssetObject assetObject = Mod.Resource.AssetPool.Spawn(Task.AssetName.ToString());
					if (assetObject != null)
					{
						Task.LoadSuccess(assetObject.Target);
						_waitingType = ResourceLoadWaitingType.None;
						Task.Status = TaskStatus.Done;
						return;
					}
				}
				Mod.Resource.LoadingAssetNames.Add(Task.AssetName);
				if (Task.IsScene)
				{
					string text = Task.AssetName.ToString();
					int num = text.IndexOf('.');
					string sceneName = ((num > 0) ? text.Substring(0, num) : text);
					_loadAssetAsyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
				}
				else
				{
					_loadAssetAsyncRequest = Task.Bundle.LoadAssetAsync(Task.AssetName.ToString());
				}
			}

			private void UpdateLoadFile()
			{
				if (_loadBundleFileAsyncRequest.isDone)
				{
					AssetBundle assetBundle = _loadBundleFileAsyncRequest.assetBundle;
					if (assetBundle != null)
					{
						BundleLoadCompleted(assetBundle);
					}
					else
					{
						string message = "Can not load asset bundle from file '" + Task.BundleInfo.BundleName.ToString() + "'.";
						OnLoadError(ResourceLoadStatus.BundleNotExist, message);
					}
					_loadBundleFileAsyncRequest = null;
				}
				else
				{
					OnLoadUpdate(ResourceLoadStatus.LoadingBundleFromFile, _loadBundleFileAsyncRequest.progress);
				}
			}

			private void UpdateLoadAsset()
			{
				if (_loadAssetAsyncRequest.isDone)
				{
					UnityEngine.Object asset = _loadAssetAsyncRequest.asset;
					if (asset != null)
					{
						AssetObject assetObject = new AssetObject(Task.AssetName, asset, Task.Bundle, Task.DependencyAssets);
						assetObject.OnSpawn();
						Mod.Resource.AssetPool.Register(assetObject);
						Mod.Resource.LoadingAssetNames.Remove(Task.AssetName);
						Task.LoadSuccess(assetObject.Target);
						_waitingType = ResourceLoadWaitingType.None;
						Task.Status = TaskStatus.Done;
					}
					else
					{
						string message = "Can not load asset '" + Task.AssetName.ToString() + "' from asset bundle which is not exist.";
						OnLoadError(ResourceLoadStatus.AssetError, message);
					}
					_loadAssetAsyncRequest = null;
				}
				else
				{
					OnLoadUpdate(ResourceLoadStatus.LoadingAsset, _loadAssetAsyncRequest.progress);
				}
			}

			private void UpdateLoadScene()
			{
				if (_loadAssetAsyncOperation.isDone)
				{
					if (_loadAssetAsyncOperation.allowSceneActivation)
					{
						if (Mod.Resource.LoadedSceneMap.ContainsKey(Task.AssetName.ToString()))
						{
							Log.Warning("Scene " + Task.AssetName.ToString() + " was loaded.");
						}
						SceneDummy sceneDummy = new SceneDummy();
						Mod.Resource.LoadedSceneMap[Task.AssetName.ToString()] = sceneDummy;
						AssetObject assetObject = new AssetObject(Task.AssetName, sceneDummy, Task.Bundle, Task.DependencyAssets);
						assetObject.OnSpawn();
						Mod.Resource.AssetPool.Register(assetObject);
						Mod.Resource.LoadingAssetNames.Remove(Task.AssetName);
						Task.LoadSuccess(assetObject.Target);
						_waitingType = ResourceLoadWaitingType.None;
						Task.Status = TaskStatus.Done;
					}
					else
					{
						string message = "Can not load scene asset '" + Task.AssetName.ToString() + "' from asset bundle.";
						OnLoadError(ResourceLoadStatus.SceneError, message);
					}
					_loadAssetAsyncOperation = null;
				}
				else
				{
					OnLoadUpdate(ResourceLoadStatus.LoadingScene, _loadAssetAsyncOperation.progress);
				}
			}

			private void BundleLoadCompleted(AssetBundle assetBundle)
			{
				BundleObject bundleObject = new BundleObject(Task.BundleInfo.BundleName, assetBundle);
				bundleObject.OnSpawn();
				Mod.Resource.BundlePool.Register(bundleObject);
				Mod.Resource.LoadingBundleNames.Remove(Task.BundleInfo.BundleName);
				Task.Bundle = (AssetBundle)bundleObject.Target;
				_waitingType = ResourceLoadWaitingType.WaitForAsset;
				LoadAsset();
			}

			private void OnLoadUpdate(ResourceLoadStatus resourceLoadStatus, float progress)
			{
				Task.LoadUpdate(resourceLoadStatus, progress);
			}

			private void OnLoadError(ResourceLoadStatus resourceLoadStatus, string message)
			{
				if (_waitingType == ResourceLoadWaitingType.WaitForBundle)
				{
					Mod.Resource.LoadingBundleNames.Remove(Task.BundleInfo.BundleName);
				}
				Mod.Resource.LoadingAssetNames.Remove(Task.AssetName);
				Task.Status = TaskStatus.Error;
				Task.LoadFailure(resourceLoadStatus, message);
			}
		}

		private enum ResourceLoadStatus
		{
			Ok = 0,
			NotInBundle = 1,
			BundleNotExist = 2,
			BundleTypeError = 3,
			AssetError = 4,
			SceneError = 5,
			DependencyError = 6,
			LoadingBytes = 7,
			LoadingBundleFromFile = 8,
			LoadingBundleFromMemory = 9,
			LoadingAsset = 10,
			LoadingScene = 11,
			LoadBytesComplete = 12,
			LoadBundleFromFileComplete = 13,
			LoadBundleFromMemoryComplete = 14,
			LoadAssetComplete = 15,
			LoadSceneComplete = 16
		}

		private abstract class ResourceLoadTask : Task
		{
			[CompilerGenerated]
			private readonly ResourceName _003CAssetName_003Ek__BackingField;

			[CompilerGenerated]
			private readonly BundleInfo _003CBundleInfo_003Ek__BackingField;

			public ResourceName AssetName
			{
				[CompilerGenerated]
				get
				{
					return _003CAssetName_003Ek__BackingField;
				}
			}

			public BundleInfo BundleInfo
			{
				[CompilerGenerated]
				get
				{
					return _003CBundleInfo_003Ek__BackingField;
				}
			}

			public DateTime StartTime { get; set; }

			public AssetBundle Bundle { get; set; }

			public bool IsSpawned { get; set; }

			public abstract bool IsMain { get; }

			public abstract bool IsScene { get; }

			public abstract bool WasLoadDependencyFinish { get; }

			public abstract object[] DependencyAssets { get; }

			protected ResourceLoadTask(ResourceName assetName, BundleInfo bundleInfo)
			{
				_003CAssetName_003Ek__BackingField = assetName;
				_003CBundleInfo_003Ek__BackingField = bundleInfo;
			}

			public abstract void LoadSuccess(object asset);

			public abstract void LoadFailure(ResourceLoadStatus loadStatus, string message);

			public abstract void LoadUpdate(ResourceLoadStatus loadStatus, float progress);

			public abstract void AddDependency(ResourceName assetName, object asset);
		}

		private sealed class ResourceLoadTaskComparer : IEqualityComparer<ResourceLoadTask>
		{
			[CompilerGenerated]
			private static readonly IEqualityComparer<ResourceLoadTask> _003CDefault_003Ek__BackingField = new ResourceLoadTaskComparer();

			public static IEqualityComparer<ResourceLoadTask> Default
			{
				[CompilerGenerated]
				get
				{
					return _003CDefault_003Ek__BackingField;
				}
			}

			public bool Equals(ResourceLoadTask x, ResourceLoadTask y)
			{
				if (x == null && y == null)
				{
					return true;
				}
				if (x == null || y == null)
				{
					return false;
				}
				return x.AssetName.Equals(y.AssetName);
			}

			public int GetHashCode(ResourceLoadTask obj)
			{
				return obj.AssetName.GetHashCode();
			}
		}

		private enum ResourceLoadWaitingType
		{
			None = 0,
			WaitForDependencyAsset = 1,
			WaitForBundle = 2,
			WaitForAsset = 3
		}

		private sealed class SceneLoadTask : ResourceLoadTask
		{
			private readonly List<ResourceName> _dependencyAssetNames;

			private int _loadedDependencyAssetCount;

			private readonly SceneLoadCallbacks _loadCallbacks;

			private readonly object _userData;

			[CompilerGenerated]
			private readonly object[] _003CDependencyAssets_003Ek__BackingField;

			public override bool IsMain
			{
				get
				{
					return true;
				}
			}

			public override bool IsScene
			{
				get
				{
					return true;
				}
			}

			public override bool WasLoadDependencyFinish
			{
				get
				{
					return _loadedDependencyAssetCount == _dependencyAssetNames.Count;
				}
			}

			public override object[] DependencyAssets
			{
				[CompilerGenerated]
				get
				{
					return _003CDependencyAssets_003Ek__BackingField;
				}
			}

			public SceneLoadTask(ResourceName assetName, BundleInfo bundleInfo, List<ResourceName> dependencyAssetNames, SceneLoadCallbacks loadCallbacks, object userData)
				: base(assetName, bundleInfo)
			{
				_loadCallbacks = loadCallbacks;
				_userData = userData;
				_dependencyAssetNames = dependencyAssetNames;
				_003CDependencyAssets_003Ek__BackingField = new object[dependencyAssetNames.Count];
			}

			public override void LoadSuccess(object asset)
			{
				if (_loadCallbacks.Success != null)
				{
					float duration = (float)(DateTime.Now - base.StartTime).TotalSeconds;
					SceneLoadSuccess success = _loadCallbacks.Success;
					if (success != null)
					{
						success(base.AssetName.ToString(), duration, _userData);
					}
				}
			}

			public override void LoadFailure(ResourceLoadStatus loadStatus, string message)
			{
				SceneLoadFailure failure = _loadCallbacks.Failure;
				if (failure != null)
				{
					failure(base.AssetName.ToString(), message, _userData);
				}
			}

			public override void LoadUpdate(ResourceLoadStatus loadStatus, float progress)
			{
				if (loadStatus == ResourceLoadStatus.LoadingScene)
				{
					SceneLoadUpdate update = _loadCallbacks.Update;
					if (update != null)
					{
						update(base.AssetName.ToString(), progress, _userData);
					}
				}
			}

			public override void AddDependency(ResourceName assetName, object asset)
			{
				if (asset == null)
				{
					return;
				}
				int num = -1;
				for (int i = 0; i < _dependencyAssetNames.Count; i++)
				{
					if (_dependencyAssetNames[i] == assetName)
					{
						num = i;
					}
				}
				if (num != -1)
				{
					_loadedDependencyAssetCount++;
					DependencyAssets[num] = asset;
					SceneLoadDependencyAsset dependency = _loadCallbacks.Dependency;
					if (dependency != null)
					{
						dependency(base.AssetName.ToString(), assetName.ToString(), _loadedDependencyAssetCount, _dependencyAssetNames.Count, _userData);
					}
				}
			}
		}

		private sealed class VersionListChecker
		{
			private uint _crc;

			public VersionListChecker()
			{
				Mod.Event.Subscribe(EventArgs<DownloadMod.SuccessEventArgs>.EventId, OnDownloadSuccess);
				Mod.Event.Subscribe(EventArgs<DownloadMod.FailureEventArgs>.EventId, OnDownloadFailure);
			}

			public void Destroy()
			{
				Mod.Event.Unsubscribe(EventArgs<DownloadMod.SuccessEventArgs>.EventId, OnDownloadSuccess);
				Mod.Event.Unsubscribe(EventArgs<DownloadMod.FailureEventArgs>.EventId, OnDownloadFailure);
			}

			public bool DoCheck(int latestVersion, uint latestVersionListCrc)
			{
				string combinePath = PathUtility.GetCombinePath(Mod.Resource.ReadWritePath, PathUtility.GetResourceNameWithSuffix("version"));
				Mod.Resource.InternalVersion = latestVersion;
				if (!File.Exists(combinePath))
				{
					Log.Info("Latest internal version is '" + latestVersion + "', local version is not exist.");
					return true;
				}
				byte[] array = File.ReadAllBytes(combinePath);
				uint num = BitConverter.ToUInt32(Verifier.GetCrc32(array), 0);
				if (num != latestVersionListCrc)
				{
					Log.Info("Latest internal '" + latestVersion + "' CRC of version.dat is different from local version.dat");
					File.Delete(combinePath);
					return true;
				}
				MemoryStream memoryStream = null;
				string @string;
				int num2;
				try
				{
					memoryStream = new MemoryStream(array);
					using (BinaryReader binaryReader = new BinaryReader(memoryStream))
					{
						memoryStream = null;
						char[] array2 = binaryReader.ReadChars(3);
						if (array2[0] != VersionListHeader[0] || array2[1] != VersionListHeader[1] || array2[2] != VersionListHeader[2])
						{
							Log.Info("Latest internal version is '" + latestVersion + "', local version is invalid.");
							File.Delete(combinePath);
							return true;
						}
						byte b = binaryReader.ReadByte();
						if (b != 3)
						{
							Log.Info("local version list is invalid.");
							File.Delete(combinePath);
							return true;
						}
						@string = Encoding.UTF8.GetString(binaryReader.ReadBytes(binaryReader.ReadByte()));
						num2 = binaryReader.ReadInt32();
					}
				}
				catch
				{
					Log.Info("Latest internal version is '" + latestVersion + "', local version is invalid.");
					File.Delete(combinePath);
					return true;
				}
				finally
				{
					if (memoryStream != null)
					{
						memoryStream.Dispose();
					}
				}
				if (@string != Mod.Resource.ApplicableVersion || num2 != latestVersion)
				{
					Log.Info("Applicable version is '" + Mod.Resource.ApplicableVersion + "', latest internal version is '" + latestVersion + "', local version is '" + num2 + "'.");
					File.Delete(combinePath);
					return true;
				}
				Log.Info("Applicable version is '" + @string + "', latest internal version is '" + latestVersion + "', local version is up-to-date.");
				return false;
			}

			public void DoUpdate(uint crc)
			{
				_crc = crc;
				string resourceNameWithSuffix = PathUtility.GetResourceNameWithSuffix("version");
				string resourceNameWithCrcAndSuffix = PathUtility.GetResourceNameWithCrcAndSuffix("version", _crc);
				string combinePath = PathUtility.GetCombinePath(Mod.Resource.ReadWritePath, resourceNameWithSuffix);
				string remoteResourcePath = PathUtility.GetRemoteResourcePath(Mod.Resource.UriPrefix, resourceNameWithCrcAndSuffix);
				Mod.Download.AddTask(combinePath, remoteResourcePath, this);
			}

			private void OnDownloadSuccess(object sender, EventArgs args)
			{
				DownloadMod.SuccessEventArgs successEventArgs = (DownloadMod.SuccessEventArgs)args;
				if (successEventArgs.UserData == this)
				{
					byte[] bytes = File.ReadAllBytes(successEventArgs.Path);
					uint num = BitConverter.ToUInt32(Verifier.GetCrc32(bytes), 0);
					if (_crc != num)
					{
						File.Delete(successEventArgs.Path);
						string message = string.Format("Latest version list crc error, need '{0:X8}', file hash is '{1:X8}'.", _crc, num);
						Mod.Event.Fire(this, DownloadMod.FailureEventArgs.Make(successEventArgs.TaskId, successEventArgs.Path, successEventArgs.Uri, message, successEventArgs.UserData));
					}
					else
					{
						Mod.Resource.OnVersionListUpdateSuccess(successEventArgs.Path, successEventArgs.Uri);
					}
				}
			}

			private void OnDownloadFailure(object sender, EventArgs args)
			{
				DownloadMod.FailureEventArgs failureEventArgs = (DownloadMod.FailureEventArgs)args;
				if (failureEventArgs.UserData == this)
				{
					File.Delete(failureEventArgs.Path);
					Mod.Resource.OnVersionListUpdateFailure(failureEventArgs.Uri, failureEventArgs.Message);
				}
			}
		}

		[SerializeField]
		private string _remoteHostRoot = "http://49.51.38.144:80/";

		[SerializeField]
		[Range(1f, 3f)]
		private int _bundleUpdateRetryCount = 3;

		[SerializeField]
		[Range(1f, 64f)]
		private int _loadAgentCount = 32;

		[SerializeField]
		private EditorLoaderBase _editorLoader;

		private const string InvalidId = "Unknown";

		[CompilerGenerated]
		private readonly Dictionary<BundleName, BundleInfo> _003CBundleInfos_003Ek__BackingField = new Dictionary<BundleName, BundleInfo>(BundleNameComparer.Default);

		[CompilerGenerated]
		private readonly Dictionary<ResourceName, AssetInfo> _003CAssetInfos_003Ek__BackingField = new Dictionary<ResourceName, AssetInfo>(ResourceNameComparer.Default);

		[CompilerGenerated]
		private readonly Dictionary<ResourceName, AssetDependencyInfo> _003CAssetDependencyInfos_003Ek__BackingField = new Dictionary<ResourceName, AssetDependencyInfo>(ResourceNameComparer.Default);

		[CompilerGenerated]
		private readonly SortedDictionary<BundleName, BundleRwInfo> _003CReadWriteBundleInfos_003Ek__BackingField = new SortedDictionary<BundleName, BundleRwInfo>(BundleNameComparer.Default);

		private int _internalVersion;

		private bool _unloadUnusedAssets;

		private bool _performGc;

		private AsyncOperation _asyncUnloadUnusedAssets;

		public const string VersionListFilename = "version";

		public const string ResourceListFilename = "list";

		public const byte VersionListVersion = 3;

		public const byte ResourceListVersion = 3;

		public static readonly char[] VersionListHeader = new char[3] { 'R', 'S', 'V' };

		public static readonly char[] ResourceListHeader = new char[3] { 'R', 'S', 'R' };

		public const int BundleOffset = 16;

		private VersionListChecker _versionListChecker;

		private BundleChecker _bundleChecker;

		private BundleUpdater _bundleUpdater;

		private LevelBundleChecker _levelBundleChecker;

		private readonly TaskPool<ResourceLoadTask> _loadPool = new TaskPool<ResourceLoadTask>();

		[CompilerGenerated]
		private readonly Dictionary<string, SceneDummy> _003CLoadedSceneMap_003Ek__BackingField = new Dictionary<string, SceneDummy>(StringComparer.OrdinalIgnoreCase);

		[CompilerGenerated]
		private readonly HashSet<ResourceName> _003CLoadingAssetNames_003Ek__BackingField = new HashSet<ResourceName>(ResourceNameComparer.Default);

		[CompilerGenerated]
		private readonly HashSet<BundleName> _003CLoadingBundleNames_003Ek__BackingField = new HashSet<BundleName>(BundleNameComparer.Default);

		private Dictionary<BundleName, BundleInfo> BundleInfos
		{
			[CompilerGenerated]
			get
			{
				return _003CBundleInfos_003Ek__BackingField;
			}
		}

		private Dictionary<ResourceName, AssetInfo> AssetInfos
		{
			[CompilerGenerated]
			get
			{
				return _003CAssetInfos_003Ek__BackingField;
			}
		}

		private Dictionary<ResourceName, AssetDependencyInfo> AssetDependencyInfos
		{
			[CompilerGenerated]
			get
			{
				return _003CAssetDependencyInfos_003Ek__BackingField;
			}
		}

		private SortedDictionary<BundleName, BundleRwInfo> ReadWriteBundleInfos
		{
			[CompilerGenerated]
			get
			{
				return _003CReadWriteBundleInfos_003Ek__BackingField;
			}
		}

		private ObjectPool<AssetObject> AssetPool { get; set; }

		private ObjectPool<BundleObject> BundlePool { get; set; }

		public string ReadOnlyPath { get; private set; }

		public string ReadWritePath { get; private set; }

		public string UriPrefix { get; private set; }

		public string BuildInfoUri { get; private set; }

		public string RemoteResourceFolder { get; private set; }

		public string ApplicableVersion { get; private set; }

		public int InternalVersion
		{
			get
			{
				return _internalVersion;
			}
			private set
			{
				if (value != _internalVersion)
				{
					_internalVersion = value;
					SetUriPrefix(_internalVersion);
				}
			}
		}

		public string Variant { get; set; }

		public int AssetCount
		{
			get
			{
				return AssetInfos.Count;
			}
		}

		public int BundleCount
		{
			get
			{
				return BundleInfos.Count;
			}
		}

		public int BundleUpdateRetryCount
		{
			get
			{
				return _bundleUpdateRetryCount;
			}
		}

		public int BundleUpdateWaitingCount
		{
			get
			{
				BundleUpdater bundleUpdater = _bundleUpdater;
				if (bundleUpdater == null)
				{
					return 0;
				}
				return bundleUpdater.WaitingUpdateCount;
			}
		}

		public int BundleUpdatingCount
		{
			get
			{
				BundleUpdater bundleUpdater = _bundleUpdater;
				if (bundleUpdater == null)
				{
					return 0;
				}
				return bundleUpdater.UpdatingCount;
			}
		}

		public int TotalBundleLoadAgentCount
		{
			get
			{
				return _loadPool.TotalAgentCount;
			}
		}

		public int FreeBundleLoadAgentCount
		{
			get
			{
				return _loadPool.FreeAgentCount;
			}
		}

		public int WorkingBundleLoadAgentCount
		{
			get
			{
				return _loadPool.WorkingAgentCount;
			}
		}

		public int WaitingBundleLoadTaskCount
		{
			get
			{
				return _loadPool.WaitingTaskCount;
			}
		}

		private Dictionary<string, SceneDummy> LoadedSceneMap
		{
			[CompilerGenerated]
			get
			{
				return _003CLoadedSceneMap_003Ek__BackingField;
			}
		}

		private HashSet<ResourceName> LoadingAssetNames
		{
			[CompilerGenerated]
			get
			{
				return _003CLoadingAssetNames_003Ek__BackingField;
			}
		}

		private HashSet<BundleName> LoadingBundleNames
		{
			[CompilerGenerated]
			get
			{
				return _003CLoadingBundleNames_003Ek__BackingField;
			}
		}

		public bool VersionListCheck(int latestVersion, uint versionListCrc)
		{
			if (Mod.Core.EditorMode)
			{
				return false;
			}
			return _versionListChecker.DoCheck(latestVersion, versionListCrc);
		}

		public void VersionListUpdate(uint crc)
		{
			if (!Mod.Core.EditorMode)
			{
				_versionListChecker.DoUpdate(crc);
			}
		}

		public void BundleCheck()
		{
			if (!Mod.Core.EditorMode)
			{
				_bundleChecker.DoCheck();
			}
		}

		public void UpdateBundles()
		{
			if (!Mod.Core.EditorMode)
			{
				_bundleUpdater.DoUpdate();
			}
		}

		public void LevelBundleCheck(int level)
		{
			if (!Mod.Core.EditorMode)
			{
				_levelBundleChecker.DoCheck(level);
			}
		}

		public void StartUpdateLevelBundle(int level)
		{
			if (!Mod.Core.EditorMode)
			{
				BundleUpdater bundleUpdater = _bundleUpdater;
				if (bundleUpdater != null)
				{
					bundleUpdater.Destroy();
				}
				_bundleUpdater = new BundleUpdater();
				_levelBundleChecker.ReadyToUpdate(level);
				_bundleUpdater.BundleCheckComplete(false);
				_bundleUpdater.DoUpdate();
			}
		}

		public void StopUpdateLevelBundle()
		{
			if (!Mod.Core.EditorMode && _bundleUpdater != null)
			{
				_bundleUpdater.Destroy();
				_bundleUpdater = null;
			}
		}

		public void LoadAsset(string assetName, AssetLoadCallbacks loadCallbacks, object userData = null)
		{
			if (string.IsNullOrEmpty(assetName))
			{
				Log.Error("Asset name is invalid.");
			}
			else if (Mod.Core.EditorMode)
			{
				_editorLoader.LoadAsset(assetName, loadCallbacks, userData);
			}
			else
			{
				LoadMainAsset(new ResourceName(assetName), loadCallbacks, userData);
			}
		}

		public void UnloadAsset(object asset)
		{
			Log.Assert(asset is UnityEngine.Object, "Param type is not UnityEngine.Object.");
			if (asset == null)
			{
				Log.Warning("Asset is invalid.");
			}
			else if (!Mod.Core.EditorMode)
			{
				AssetPool.Recycle(asset);
				AssetPool.Unload();
			}
		}

		public void UnloadUnusedAssets(bool performGc = false)
		{
			if (!Mod.Core.EditorMode && _asyncUnloadUnusedAssets == null)
			{
				_performGc = performGc;
				_unloadUnusedAssets = true;
			}
		}

		internal void LoadScene(string assetName, SceneLoadCallbacks loadCallbacks, object userData = null)
		{
			if (string.IsNullOrEmpty(assetName))
			{
				Log.Error("Scene asset name is invalid.");
			}
			else if (Mod.Core.EditorMode)
			{
				_editorLoader.LoadScene(assetName, loadCallbacks, userData);
			}
			else
			{
				LoadLevelAsset(new ResourceName(assetName), loadCallbacks, userData);
			}
		}

		internal void UnloadScene(string assetName, SceneUnloadCallbacks unloadCallbacks, object userData = null)
		{
			if (string.IsNullOrEmpty(assetName))
			{
				Log.Error("Scene asset name is invalid.");
				return;
			}
			if (Mod.Core.EditorMode)
			{
				_editorLoader.UnloadScene(assetName, unloadCallbacks, userData);
				return;
			}
			SceneDummy value;
			if (LoadedSceneMap.TryGetValue(assetName, out value))
			{
				AssetPool.Recycle(value);
				LoadedSceneMap.Remove(assetName);
			}
			StartCoroutine(UnloadSceneCo(assetName, unloadCallbacks, userData));
		}

		private void LoadBytes(string fileUri, LoadBytesCallback loadCallback)
		{
			StartCoroutine(LoadBytesCo(fileUri, loadCallback));
		}

		private void SetUriPrefix(int internalVersion)
		{
			switch (Application.platform)
			{
			case RuntimePlatform.Android:
				UriPrefix = _remoteHostRoot + "downloadFile/" + RemoteResourceFolder + "/" + ApplicableVersion + "/" + internalVersion + "/android/";
				break;
			case RuntimePlatform.IPhonePlayer:
				UriPrefix = _remoteHostRoot + "downloadFile/" + RemoteResourceFolder + "/" + ApplicableVersion + "/" + internalVersion + "/ios/";
				break;
			case RuntimePlatform.WindowsEditor:
				UriPrefix = _remoteHostRoot + "downloadFile/" + RemoteResourceFolder + "/" + ApplicableVersion + "/" + internalVersion + "/windows/";
				break;
			default:
				UriPrefix = "Unknown";
				break;
			}
		}

		public void SetRemoteResourceFolder(string folder)
		{
			RemoteResourceFolder = folder;
			BuildInfoUri = _remoteHostRoot + "downloadFile/" + RemoteResourceFolder + "/" + ApplicableVersion + "/buildinfo.json";
		}

		protected override void Awake()
		{
			Mod.Resource = this;
		}

		internal override void OnInit()
		{
			base.OnInit();
			ReadOnlyPath = Application.streamingAssetsPath;
			ReadWritePath = Application.dataPath;
			ApplicableVersion = Application.version;
			_versionListChecker = new VersionListChecker();
			_bundleChecker = new BundleChecker();
			_bundleUpdater = new BundleUpdater();
			_levelBundleChecker = new LevelBundleChecker();
			AssetPool = Mod.ObjectPool.Create<AssetObject>("Assets Pool", true, true);
			BundlePool = Mod.ObjectPool.Create<BundleObject>("Bundles Pool", true, true);
			for (int i = 0; i < _loadAgentCount; i++)
			{
				_loadPool.AddAgent(new ResourceLoadAgent());
			}
		}

		internal override void OnTick(float elapseSeconds, float realElapseSeconds)
		{
			Profiler.BeginSample("ResourceMod.OnTick");
			if (Mod.Core.EditorMode && _editorLoader)
			{
				_editorLoader.Tick(elapseSeconds, realElapseSeconds);
				return;
			}
			BundleUpdater bundleUpdater = _bundleUpdater;
			if (bundleUpdater != null)
			{
				bundleUpdater.Tick(elapseSeconds, realElapseSeconds);
			}
			TaskPool<ResourceLoadTask> loadPool = _loadPool;
			if (loadPool != null)
			{
				loadPool.Tick(elapseSeconds, realElapseSeconds);
			}
			if (_asyncUnloadUnusedAssets == null && _unloadUnusedAssets)
			{
				_asyncUnloadUnusedAssets = Resources.UnloadUnusedAssets();
				_unloadUnusedAssets = false;
			}
			AsyncOperation asyncUnloadUnusedAssets = _asyncUnloadUnusedAssets;
			if (asyncUnloadUnusedAssets != null && asyncUnloadUnusedAssets.isDone)
			{
				if (_performGc)
				{
					Profiler.BeginSample("ResourceMod.OnTick.GC.Collect");
					_performGc = false;
					GC.Collect();
					Profiler.EndSample();
				}
				_asyncUnloadUnusedAssets = null;
			}
			Profiler.EndSample();
		}

		internal override void OnExit()
		{
			if (_versionListChecker != null)
			{
				_versionListChecker.Destroy();
				_versionListChecker = null;
			}
			if (_bundleChecker != null)
			{
				_bundleChecker.Destroy();
				_bundleChecker = null;
			}
			if (_bundleUpdater != null)
			{
				_bundleUpdater.Destroy();
				_bundleUpdater = null;
			}
			if (_levelBundleChecker != null)
			{
				_levelBundleChecker.Destroy();
				_levelBundleChecker = null;
			}
			LoadingAssetNames.Clear();
			LoadingBundleNames.Clear();
			BundleInfos.Clear();
			AssetInfos.Clear();
			AssetDependencyInfos.Clear();
			ReadWriteBundleInfos.Clear();
			_loadPool.Destroy();
		}

		private bool TryGetBundleInfo(BundleName bundleName, out BundleInfo bundleInfo)
		{
			return BundleInfos.TryGetValue(bundleName, out bundleInfo);
		}

		private bool TryGetAssetInfo(ResourceName assetName, out AssetInfo assetInfo)
		{
			if (string.IsNullOrEmpty(assetName.ToString()))
			{
				Log.Warning("Asset name is invalid.");
				assetInfo = default(AssetInfo);
				return false;
			}
			return AssetInfos.TryGetValue(assetName, out assetInfo);
		}

		private bool TryGetAssetDependencyInfo(ResourceName assetName, out AssetDependencyInfo assetDependencyInfo)
		{
			if (string.IsNullOrEmpty(assetName.ToString()))
			{
				Log.Warning("Asset name is invalid.");
				assetDependencyInfo = default(AssetDependencyInfo);
				return false;
			}
			return AssetDependencyInfos.TryGetValue(assetName, out assetDependencyInfo);
		}

		private bool CheckAsset(ResourceName assetName, out BundleInfo bundleInfo, out List<ResourceName> dependencyAssetNames, out List<ResourceName> scatteredDependencyAssetNames)
		{
			bundleInfo = default(BundleInfo);
			dependencyAssetNames = null;
			scatteredDependencyAssetNames = null;
			if (string.IsNullOrEmpty(assetName.ToString()))
			{
				return false;
			}
			AssetInfo assetInfo;
			if (!TryGetAssetInfo(assetName, out assetInfo))
			{
				return false;
			}
			if (!TryGetBundleInfo(assetInfo.BundleName, out bundleInfo))
			{
				return false;
			}
			AssetDependencyInfo assetDependencyInfo;
			if (!TryGetAssetDependencyInfo(assetName, out assetDependencyInfo))
			{
				return true;
			}
			dependencyAssetNames = assetDependencyInfo.DependencyAssetNames;
			scatteredDependencyAssetNames = assetDependencyInfo.ScatteredDependencyAssetNames;
			return true;
		}

		private void OnVersionListUpdateSuccess(string path, string uri)
		{
			VersionListUpdateSuccessEventArgs args = VersionListUpdateSuccessEventArgs.Make(path, uri);
			Mod.Event.Fire(this, args);
		}

		private void OnVersionListUpdateFailure(string uri, string message)
		{
			VersionListUpdateFailureEventArgs args = VersionListUpdateFailureEventArgs.Make(uri, message);
			Mod.Event.Fire(this, args);
		}

		private void OnBundleNeedUpdate(BundleName bundleName, int length, uint crc)
		{
			_bundleUpdater.AddBundleToUpdate(bundleName, length, crc, PathUtility.GetCombinePath(ReadWritePath, PathUtility.GetResourceNameWithSuffix(bundleName.ToString())), PathUtility.GetRemoteResourcePath(UriPrefix, PathUtility.GetResourceNameWithCrcAndSuffix(bundleName.ToString(), crc)), 0);
		}

		private void OnBundleNeedUpdateDelay(BundleName bundleName, int length, uint crc, int[] level)
		{
			_levelBundleChecker.AddNeedUpdateLevelBundle(level, bundleName, length, crc, PathUtility.GetCombinePath(ReadWritePath, PathUtility.GetResourceNameWithSuffix(bundleName.ToString())), PathUtility.GetRemoteResourcePath(UriPrefix, PathUtility.GetResourceNameWithCrcAndSuffix(bundleName.ToString(), crc)));
		}

		private void OnBundleCheckComplete(int removedCount, int updateCount, int updateTotalLength)
		{
			if (_versionListChecker != null)
			{
				_versionListChecker.Destroy();
				_versionListChecker = null;
			}
			if (_bundleChecker != null)
			{
				_bundleChecker.Destroy();
				_bundleChecker = null;
			}
			_bundleUpdater.BundleCheckComplete(removedCount > 0);
			if (updateCount <= 0)
			{
				_bundleUpdater.Destroy();
				_bundleUpdater = null;
			}
			BundleCheckCompleteEventArgs args = BundleCheckCompleteEventArgs.Make(removedCount, updateCount, updateTotalLength);
			Mod.Event.Fire(this, args);
		}

		private void OnLevelBundleCheckComplete(int level, bool needUpdate, int updateCount, int updateTotalLength)
		{
			LevelBundleCheckCompleteEventArgs args = LevelBundleCheckCompleteEventArgs.Make(level, needUpdate, updateCount, updateTotalLength);
			Mod.Event.Fire(this, args);
		}

		private void OnLevelBundleNeedUpdate(BundleName bundleName, int length, uint crc, string downloadPath, string downloadUri, int retryCount)
		{
			_bundleUpdater.AddBundleToUpdate(bundleName, length, crc, downloadPath, downloadUri, retryCount);
		}

		private void OnBundleUpdateStart(BundleName bundleName, string path, string uri, int length, int retryCount)
		{
			BundleUpdateStartEventArgs args = BundleUpdateStartEventArgs.Make(bundleName.ToString(), path, uri, length, retryCount);
			Mod.Event.Fire(this, args);
		}

		private void OnBundleUpdateChanged(BundleName bundleName, string path, string uri, int length, int zipLength)
		{
			BundleUpdateChangedEventArgs args = BundleUpdateChangedEventArgs.Make(bundleName.ToString(), path, uri, length, zipLength);
			Mod.Event.Fire(this, args);
		}

		private void OnBundleUpdateSuccess(BundleName bundleName, string path, string uri, int length, int zipLength)
		{
			BundleUpdateSuccessEventArgs args = BundleUpdateSuccessEventArgs.Make(bundleName.ToString(), path, uri, length, zipLength);
			Mod.Event.Fire(this, args);
		}

		private void OnBundleUpdateFailure(BundleName bundleName, string uri, int retryCount, string message)
		{
			BundleUpdateFailureEventArgs args = BundleUpdateFailureEventArgs.Make(bundleName.ToString(), uri, retryCount, BundleUpdateRetryCount, message);
			Mod.Event.Fire(this, args);
		}

		private void OnBundleUpdateAllComplete()
		{
			if (_bundleUpdater != null)
			{
				_bundleUpdater.Destroy();
				_bundleUpdater = null;
			}
			BunldeUpdateAllCompleteEventArgs args = BunldeUpdateAllCompleteEventArgs.Make();
			Mod.Event.Fire(this, args);
		}

		private void LoadMainAsset(ResourceName assetName, AssetLoadCallbacks loadCallbacks, object userData)
		{
			BundleInfo bundleInfo;
			List<ResourceName> dependencyAssetNames;
			List<ResourceName> scatteredDependencyAssetNames;
			if (!CheckAsset(assetName, out bundleInfo, out dependencyAssetNames, out scatteredDependencyAssetNames))
			{
				string message = "'" + assetName.ToString() + "' can not find at AssetBundle.";
				AssetLoadFailure failure = loadCallbacks.Failure;
				if (failure != null)
				{
					failure(assetName.ToString(), message, userData);
				}
				return;
			}
			AssetLoadTask assetLoadTask = new AssetLoadTask(assetName, bundleInfo, dependencyAssetNames, loadCallbacks, userData);
			for (int i = 0; i < dependencyAssetNames.Count; i++)
			{
				ResourceName assetName2 = dependencyAssetNames[i];
				if (!LoadDependencyAsset(assetName2, assetLoadTask))
				{
					string message2 = "Can not add dependency asset task '" + assetName2.ToString() + "' when add main asset task '" + assetName.ToString() + "'.";
					AssetLoadFailure failure2 = loadCallbacks.Failure;
					if (failure2 != null)
					{
						failure2(assetName.ToString(), message2, userData);
					}
					return;
				}
			}
			_loadPool.AddTask(assetLoadTask);
		}

		private void LoadLevelAsset(ResourceName assetName, SceneLoadCallbacks loadCallbacks, object userData)
		{
			BundleInfo bundleInfo;
			List<ResourceName> dependencyAssetNames;
			List<ResourceName> scatteredDependencyAssetNames;
			if (!CheckAsset(assetName, out bundleInfo, out dependencyAssetNames, out scatteredDependencyAssetNames))
			{
				string message = "'" + assetName.ToString() + "' can not find at AssetBundle.";
				SceneLoadFailure failure = loadCallbacks.Failure;
				if (failure != null)
				{
					failure(assetName.ToString(), message, userData);
				}
				return;
			}
			SceneLoadTask sceneLoadTask = new SceneLoadTask(assetName, bundleInfo, dependencyAssetNames, loadCallbacks, userData);
			for (int i = 0; i < dependencyAssetNames.Count; i++)
			{
				ResourceName assetName2 = dependencyAssetNames[i];
				if (!LoadDependencyAsset(assetName2, sceneLoadTask))
				{
					string message2 = "Can not load dependency asset '" + assetName2.ToString() + "' when load scene '" + assetName.ToString() + "'.";
					SceneLoadFailure failure2 = loadCallbacks.Failure;
					if (failure2 != null)
					{
						failure2(assetName.ToString(), message2, userData);
					}
					return;
				}
			}
			_loadPool.AddTask(sceneLoadTask);
		}

		private bool LoadDependencyAsset(ResourceName assetName, ResourceLoadTask dependTask)
		{
			BundleInfo bundleInfo;
			List<ResourceName> dependencyAssetNames;
			List<ResourceName> scatteredDependencyAssetNames;
			if (!CheckAsset(assetName, out bundleInfo, out dependencyAssetNames, out scatteredDependencyAssetNames))
			{
				Log.Warning("'" + assetName.ToString() + "' can not find at AssetBundle.");
				return false;
			}
			AssetLoadTask assetLoadTask = new AssetLoadTask(dependTask, assetName, bundleInfo, dependencyAssetNames);
			for (int i = 0; i < dependencyAssetNames.Count; i++)
			{
				ResourceName assetName2 = dependencyAssetNames[i];
				if (!LoadDependencyAsset(assetName2, assetLoadTask))
				{
					Log.Warning("Can not add dependency asset load task '" + assetName2.ToString() + "' when add dependency asset load task '" + assetName.ToString() + "'.");
					return false;
				}
			}
			_loadPool.AddTask(assetLoadTask);
			return true;
		}

		private IEnumerator LoadBytesCo(string fileUri, LoadBytesCallback loadCallback)
		{
			using (UnityWebRequest www = UnityWebRequest.Get(fileUri))
			{
				yield return www.SendWebRequest();
				string message = string.Empty;
				if (www.isNetworkError)
				{
					message = www.error;
				}
				else if (www.isHttpError)
				{
					message = string.Format("http error: {0}", www.responseCode);
				}
				byte[] data = www.downloadHandler.data;
				if (loadCallback != null)
				{
					loadCallback(fileUri, data, message);
				}
			}
		}

		private IEnumerator UnloadSceneCo(string assetName, SceneUnloadCallbacks unloadCallbacks, object userData)
		{
			string sceneName = Mod.Scene.GetName(assetName);
			AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(sceneName);
			if (asyncOperation == null)
			{
				yield break;
			}
			yield return asyncOperation;
			if (asyncOperation.allowSceneActivation)
			{
				SceneUnloadSuccess success = unloadCallbacks.Success;
				if (success != null)
				{
					success(assetName, userData);
				}
			}
			else
			{
				SceneUnloadFailure failure = unloadCallbacks.Failure;
				if (failure != null)
				{
					failure(assetName, "Unload Failure", userData);
				}
			}
		}
	}
}
