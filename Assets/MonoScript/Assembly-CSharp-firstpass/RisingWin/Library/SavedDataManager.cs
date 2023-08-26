using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RisingWin.Library
{
	public class SavedDataManager : Singleton<SavedDataManager>
	{
		public delegate object CreateDelegate();

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass9_0<T>
		{
			public SavedDataManager _003C_003E4__this;

			public SavedData savedData;

			public string pPersistentKey;

			public Action<T> pDoneCallback;

			internal void _003CGetData_003Eb__0()
			{
				_003C_003E4__this.GetData(savedData, pPersistentKey, pDoneCallback);
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass11_0<T>
		{
			public CreateDelegate pCreateHandler;

			public SavedDataManager _003C_003E4__this;

			public string pSlotName;

			public string pPersistentKey;

			public Action<T> pDoneCallback;

			internal void _003CGetOrCreateData_003Eb__0(T data)
			{
				if (object.Equals(data, default(T)))
				{
					try
					{
						_003C_003Ec__DisplayClass11_1<T> _003C_003Ec__DisplayClass11_ = new _003C_003Ec__DisplayClass11_1<T>
						{
							CS_0024_003C_003E8__locals1 = this,
							defaultData = (T)pCreateHandler()
						};
						_003C_003E4__this.SetData(pSlotName, pPersistentKey, _003C_003Ec__DisplayClass11_.defaultData, _003C_003Ec__DisplayClass11_._003CGetOrCreateData_003Eb__1);
						return;
					}
					catch (Exception ex)
					{
						throw ex;
					}
				}
				if (pDoneCallback != null)
				{
					pDoneCallback(data);
				}
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass11_1<T>
		{
			public T defaultData;

			public _003C_003Ec__DisplayClass11_0<T> CS_0024_003C_003E8__locals1;

			internal void _003CGetOrCreateData_003Eb__1()
			{
				if (CS_0024_003C_003E8__locals1.pDoneCallback != null)
				{
					CS_0024_003C_003E8__locals1.pDoneCallback(defaultData);
				}
			}
		}

		private Dictionary<string, SavedData> savedDataContainer = new Dictionary<string, SavedData>();

		public void CreateSavedData(string pFolderName, string pSlotName)
		{
			SavedData value = new SavedData(pFolderName, pSlotName);
			savedDataContainer.Add(pSlotName, value);
		}

		public void Reset()
		{
			foreach (SavedData value in savedDataContainer.Values)
			{
				value.Close();
				value.DeleteKeys();
			}
		}

		public bool HasData(string pSlotName, string pPersistentKey)
		{
			return GetSavedData(pSlotName).HasData(pPersistentKey);
		}

		public void DeleteData(string pSlotName, string pPersistentKey, Action pDoneCallback)
		{
			GetSavedData(pSlotName).DeleteData(pPersistentKey);
			SaveDataIfNeeded(pSlotName, pDoneCallback);
		}

		public void SetData(string pSlotName, string pPersistentKey, object pData, Action pDoneCallback)
		{
			GetSavedData(pSlotName).SetData(pPersistentKey, pData);
			SaveDataIfNeeded(pSlotName, pDoneCallback);
		}

		private void SaveDataIfNeeded(string pSlotName, Action pDoneCallback)
		{
			SaveData(pSlotName, pDoneCallback);
		}

		public void SaveData(string pSlotName, Action pDoneCallback)
		{
			GetSavedData(pSlotName).Save(pDoneCallback);
		}

		public void GetData<T>(string pSlotName, string pPersistentKey, Action<T> pDoneCallback)
		{
			_003C_003Ec__DisplayClass9_0<T> _003C_003Ec__DisplayClass9_ = new _003C_003Ec__DisplayClass9_0<T>();
			_003C_003Ec__DisplayClass9_._003C_003E4__this = this;
			_003C_003Ec__DisplayClass9_.pPersistentKey = pPersistentKey;
			_003C_003Ec__DisplayClass9_.pDoneCallback = pDoneCallback;
			_003C_003Ec__DisplayClass9_.savedData = GetSavedData(pSlotName);
			_003C_003Ec__DisplayClass9_.savedData.Load(_003C_003Ec__DisplayClass9_._003CGetData_003Eb__0);
		}

		private void GetData<T>(SavedData pData, string pPersistentKey, Action<T> pDoneCallback)
		{
			T data = pData.GetData<T>(pPersistentKey);
			if (pDoneCallback != null)
			{
				pDoneCallback(data);
			}
		}

		public void GetOrCreateData<T>(string pSlotName, string pPersistentKey, CreateDelegate pCreateHandler, Action<T> pDoneCallback)
		{
			_003C_003Ec__DisplayClass11_0<T> _003C_003Ec__DisplayClass11_ = new _003C_003Ec__DisplayClass11_0<T>();
			_003C_003Ec__DisplayClass11_.pCreateHandler = pCreateHandler;
			_003C_003Ec__DisplayClass11_._003C_003E4__this = this;
			_003C_003Ec__DisplayClass11_.pSlotName = pSlotName;
			_003C_003Ec__DisplayClass11_.pPersistentKey = pPersistentKey;
			_003C_003Ec__DisplayClass11_.pDoneCallback = pDoneCallback;
			GetData<T>(_003C_003Ec__DisplayClass11_.pSlotName, _003C_003Ec__DisplayClass11_.pPersistentKey, _003C_003Ec__DisplayClass11_._003CGetOrCreateData_003Eb__0);
		}

		private SavedData GetSavedData(string pSlotName)
		{
			if (!savedDataContainer.ContainsKey(pSlotName))
			{
				return null;
			}
			return savedDataContainer[pSlotName];
		}
	}
}
