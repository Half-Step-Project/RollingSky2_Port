using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Foundation
{
	[DisallowMultipleComponent]
	[AddComponentMenu("Framework/Data Table")]
	public sealed class DataTableMod : ModBase
	{
		private sealed class LoadData
		{
			[CompilerGenerated]
			private readonly Type _003CType_003Ek__BackingField;

			[CompilerGenerated]
			private readonly object _003CUserData_003Ek__BackingField;

			public Type Type
			{
				[CompilerGenerated]
				get
				{
					return _003CType_003Ek__BackingField;
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

			public LoadData(Type type, object userData)
			{
				_003CType_003Ek__BackingField = type;
				_003CUserData_003Ek__BackingField = userData;
			}
		}

		private sealed class Table<T> : IDataTable<T>, IDataTable where T : class, IRecord, new()
		{
			private readonly Dictionary<int, T> _records = new Dictionary<int, T>();

			[CompilerGenerated]
			private readonly Type _003CRecordType_003Ek__BackingField = typeof(T);

			public Type RecordType
			{
				[CompilerGenerated]
				get
				{
					return _003CRecordType_003Ek__BackingField;
				}
			}

			public int Count
			{
				get
				{
					return _records.Count;
				}
			}

			public T Min { get; private set; }

			public T Max { get; private set; }

			IRecord[] IDataTable.Records
			{
				get
				{
					int num = 0;
					IRecord[] array = new IRecord[_records.Count];
					foreach (KeyValuePair<int, T> record in _records)
					{
						array[num++] = record.Value;
					}
					return array;
				}
			}

			public T[] Records
			{
				get
				{
					int num = 0;
					T[] array = new T[_records.Count];
					foreach (KeyValuePair<int, T> record in _records)
					{
						array[num++] = record.Value;
					}
					return array;
				}
			}

			public T this[int id]
			{
				get
				{
					T value;
					if (!_records.TryGetValue(id, out value))
					{
						return null;
					}
					return value;
				}
			}

			public bool Contains(int id)
			{
				return _records.ContainsKey(id);
			}

			public bool Contains(Predicate<T> condition)
			{
				if (condition == null)
				{
					return false;
				}
				foreach (KeyValuePair<int, T> record in _records)
				{
					if (condition(record.Value))
					{
						return true;
					}
				}
				return false;
			}

			public T Get(int id)
			{
				return this[id];
			}

			public T Get(Predicate<T> condition)
			{
				if (condition == null)
				{
					return null;
				}
				foreach (KeyValuePair<int, T> record in _records)
				{
					if (condition(record.Value))
					{
						return record.Value;
					}
				}
				return null;
			}

			public T[] Filter(Predicate<T> condition)
			{
				if (condition == null)
				{
					return null;
				}
				List<T> list = new List<T>();
				foreach (KeyValuePair<int, T> record in _records)
				{
					if (condition(record.Value))
					{
						list.Add(record.Value);
					}
				}
				return list.ToArray();
			}

			public T[] Sort(Comparison<T> comparison)
			{
				if (comparison == null)
				{
					return null;
				}
				int num = 0;
				T[] array = new T[_records.Count];
				foreach (KeyValuePair<int, T> record in _records)
				{
					array[num++] = record.Value;
				}
				Array.Sort(array, comparison);
				return array;
			}

			public T[] Filter(Predicate<T> condition, Comparison<T> comparison)
			{
				if (condition == null || comparison == null)
				{
					return null;
				}
				List<T> list = new List<T>();
				foreach (KeyValuePair<int, T> record in _records)
				{
					if (condition(record.Value))
					{
						list.Add(record.Value);
					}
				}
				list.Sort(comparison);
				return list.ToArray();
			}

			public void RemoveAll()
			{
				_records.Clear();
			}

			public int Add(byte[] bytes, int position)
			{
				T val = new T();
				try
				{
					position = val.Parse(bytes, position);
				}
				catch (Exception ex)
				{
					Log.Error(typeof(T).Name + ": " + ex.Message + "\n" + ex.StackTrace);
					return position;
				}
				if (Contains(val.TheId))
				{
					Log.Warning("Already exist '" + val.TheId + "' in " + GetType().FullName + ".");
				}
				_records.Add(val.TheId, val);
				if (Min == null || Min.TheId > val.TheId)
				{
					Min = val;
				}
				if (Max == null || Max.TheId < val.TheId)
				{
					Max = val;
				}
				return position;
			}
		}

		public sealed class LoadDependencyEventArgs : EventArgs<LoadDependencyEventArgs>
		{
			public Type RecordType { get; private set; }

			public string AssetName { get; private set; }

			public string DependencyAssetName { get; private set; }

			public int LoadedCount { get; private set; }

			public int TotalCount { get; private set; }

			public object UserData { get; private set; }

			public static LoadDependencyEventArgs Make(Type recordType, string assetName, string dependencyAssetName, int loadedCount, int totalCount, object userData)
			{
				LoadDependencyEventArgs loadDependencyEventArgs = Mod.Reference.Acquire<LoadDependencyEventArgs>();
				loadDependencyEventArgs.RecordType = recordType;
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
			public Type RecordType { get; private set; }

			public string AssetName { get; private set; }

			public string Message { get; private set; }

			public object UserData { get; private set; }

			public static LoadFailureEventArgs Make(Type recordType, string assetName, string message, object userData)
			{
				LoadFailureEventArgs loadFailureEventArgs = Mod.Reference.Acquire<LoadFailureEventArgs>();
				loadFailureEventArgs.RecordType = recordType;
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
			public Type RecordType { get; private set; }

			public string AssetName { get; private set; }

			public float Duration { get; private set; }

			public object UserData { get; private set; }

			public static LoadSuccessEventArgs Make(Type recordType, string assetName, float duration, object userData)
			{
				LoadSuccessEventArgs loadSuccessEventArgs = Mod.Reference.Acquire<LoadSuccessEventArgs>();
				loadSuccessEventArgs.RecordType = recordType;
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
			public Type RecordType { get; private set; }

			public string AssetName { get; private set; }

			public float Progress { get; private set; }

			public object UserData { get; private set; }

			public static LoadUpdateEventArgs Make(Type rowType, string assetName, float progress, object userData)
			{
				LoadUpdateEventArgs loadUpdateEventArgs = Mod.Reference.Acquire<LoadUpdateEventArgs>();
				loadUpdateEventArgs.RecordType = rowType;
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

		private readonly Dictionary<Type, IDataTable> _tables = new Dictionary<Type, IDataTable>();

		private AssetLoadCallbacks _loadCallbacks;

		public int Count
		{
			get
			{
				return _tables.Count;
			}
		}

		public IDataTable[] DataTables
		{
			get
			{
				int num = 0;
				IDataTable[] array = new IDataTable[_tables.Count];
				foreach (KeyValuePair<Type, IDataTable> table in _tables)
				{
					array[num++] = table.Value;
				}
				return array;
			}
		}

		public void Load<T>(string assetName, object userData = null)
		{
			if (Contains<T>())
			{
				LoadSuccessEventArgs args = LoadSuccessEventArgs.Make(typeof(T), assetName, 0f, userData);
				Mod.Event.Fire(this, args);
			}
			else
			{
				Mod.Resource.LoadAsset(assetName, _loadCallbacks, new LoadData(typeof(T), userData));
			}
		}

		public bool Contains<T>()
		{
			return _tables.ContainsKey(typeof(T));
		}

		public IDataTable<T> Get<T>() where T : IRecord
		{
			IDataTable value;
			if (!_tables.TryGetValue(typeof(T), out value))
			{
				return null;
			}
			return (IDataTable<T>)value;
		}

		public IDataTable<T> Create<T>(byte[] bytes) where T : class, IRecord, new()
		{
			if (_tables.ContainsKey(typeof(T)))
			{
				Log.Warning("Already exist data table '" + typeof(T).FullName + "'.");
				return null;
			}
			Table<T> table = new Table<T>();
			int position;
			int recordCount = GetRecordCount(bytes, out position);
			for (int i = 0; i < recordCount; i++)
			{
				position = table.Add(bytes, position);
			}
			_tables.Add(typeof(T), table);
			return table;
		}

		public bool Remove<T>() where T : IRecord, new()
		{
			IDataTable value;
			if (_tables.TryGetValue(typeof(T), out value))
			{
				value.RemoveAll();
				return _tables.Remove(typeof(T));
			}
			return false;
		}

		protected override void Awake()
		{
			Mod.DataTable = this;
		}

		internal override void OnInit()
		{
			base.OnInit();
			_loadCallbacks = new AssetLoadCallbacks(OnLoadSuccess, OnLoadFailure, OnLoadUpdate, OnLoadDependencyAsset);
		}

		internal override void OnTick(float elapseSeconds, float realElapseSeconds)
		{
		}

		internal override void OnExit()
		{
			base.OnExit();
			_tables.Clear();
		}

		private IDataTable Create(Type type, byte[] bytes)
		{
			if ((object)type == null)
			{
				Log.Warning("Data record type is invalid.");
				return null;
			}
			if (!typeof(IRecord).IsAssignableFrom(type))
			{
				Log.Warning("Data row type '" + type.FullName + "' is invalid.");
				return null;
			}
			if (_tables.ContainsKey(type))
			{
				Log.Warning("Already exist data table '" + type.FullName + "'.");
				return null;
			}
			Type type2 = typeof(Table<>).MakeGenericType(type);
			IDataTable dataTable = Activator.CreateInstance(type2) as IDataTable;
			if (dataTable == null)
			{
				return null;
			}
			int position;
			int recordCount = GetRecordCount(bytes, out position);
			for (int i = 0; i < recordCount; i++)
			{
				position = dataTable.Add(bytes, position);
			}
			_tables.Add(type, dataTable);
			return dataTable;
		}

		private int GetRecordCount(byte[] bytes, out int position)
		{
			position = 0;
			if (bytes == null)
			{
				Log.Error("Bytes is invalid");
				return 0;
			}
			if (bytes.Length < 4)
			{
				Log.Error("Bytes is too small");
				return 0;
			}
			int network = BitConverter.ToInt32(bytes, 0);
			network = IPAddress.NetworkToHostOrder(network);
			position = 4;
			return network;
		}

		private void OnLoadSuccess(string assetName, object asset, float duration, object userData)
		{
			LoadData loadData = (LoadData)userData;
			byte[] bytes = ((TextAsset)asset).bytes;
			Mod.Resource.UnloadAsset((UnityEngine.Object)asset);
			if ((object)loadData.Type == null)
			{
				string message = assetName + " data record type is invalid.";
				OnLoadFailure(assetName, message, userData);
			}
			else if (Create(loadData.Type, bytes) == null)
			{
				string message2 = loadData.Type.FullName + " create failure.";
				OnLoadFailure(assetName, message2, userData);
			}
			else
			{
				LoadSuccessEventArgs args = LoadSuccessEventArgs.Make(loadData.Type, assetName, duration, loadData.UserData);
				Mod.Event.Fire(this, args);
			}
		}

		private void OnLoadFailure(string assetName, string message, object userData)
		{
			LoadData loadData = (LoadData)userData;
			LoadFailureEventArgs args = LoadFailureEventArgs.Make(loadData.Type, assetName, message, loadData.UserData);
			Mod.Event.Fire(this, args);
		}

		private void OnLoadUpdate(string assetName, float progress, object userData)
		{
			LoadData loadData = (LoadData)userData;
			LoadUpdateEventArgs args = LoadUpdateEventArgs.Make(loadData.Type, assetName, progress, loadData.UserData);
			Mod.Event.Fire(this, args);
		}

		private void OnLoadDependencyAsset(string assetName, string dependencyAssetName, int loadedCount, int totalCount, object userData)
		{
			LoadData loadData = (LoadData)userData;
			LoadDependencyEventArgs args = LoadDependencyEventArgs.Make(loadData.Type, assetName, dependencyAssetName, loadedCount, totalCount, loadData.UserData);
			Mod.Event.Fire(this, args);
		}
	}
}
