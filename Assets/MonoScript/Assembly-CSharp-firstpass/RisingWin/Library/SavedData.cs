using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using UnityEngine;

namespace RisingWin.Library
{
	public class SavedData
	{
		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass15_0
		{
			public SavedData _003C_003E4__this;

			public Action pDoneCallback;

			public Action _003C_003E9__2;

			public Action _003C_003E9__3;

			internal void _003CSave_003Eb__0()
			{
				_003C_003E4__this.BackupData(_003C_003E9__2 ?? (_003C_003E9__2 = _003CSave_003Eb__2));
			}

			internal void _003CSave_003Eb__2()
			{
				if (pDoneCallback != null)
				{
					pDoneCallback();
				}
			}

			internal void _003CSave_003Eb__1()
			{
				_003C_003E4__this.SaveData(_003C_003E9__3 ?? (_003C_003E9__3 = _003CSave_003Eb__3));
			}

			internal void _003CSave_003Eb__3()
			{
				if (pDoneCallback != null)
				{
					pDoneCallback();
				}
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass16_0
		{
			public SavedData _003C_003E4__this;

			public Action pDoneCallback;

			public Action _003C_003E9__1;

			internal void _003CBackupData_003Eb__0(string result)
			{
				_003C_003E4__this.BackupFile.Write(result, _003C_003E9__1 ?? (_003C_003E9__1 = _003CBackupData_003Eb__1));
			}

			internal void _003CBackupData_003Eb__1()
			{
				PlayerPrefsManager.SetKey(_003C_003E4__this.BackupTaskKey, true);
				if (pDoneCallback != null)
				{
					pDoneCallback();
				}
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass17_0
		{
			public SavedData _003C_003E4__this;

			public Action pDoneCallback;

			internal void _003CSaveData_003Eb__0()
			{
				PlayerPrefsManager.SetKey(_003C_003E4__this.MainTaskKey, true);
				if (pDoneCallback != null)
				{
					pDoneCallback();
				}
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass19_0
		{
			public SavedData _003C_003E4__this;

			public Action pDoneCallback;

			internal void _003CLoadFromBackupFile_003Eb__0(string result)
			{
				_003C_003E4__this.Deserialize(result, pDoneCallback);
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass20_0
		{
			public SavedData _003C_003E4__this;

			public Action pDoneCallback;

			internal void _003CLoadFromMainFile_003Eb__0(string result)
			{
				_003C_003E4__this.Deserialize(result, pDoneCallback);
			}
		}

		private IFileIO MainFile;

		private IFileIO BackupFile;

		private string MainTaskKey;

		private string BackupTaskKey;

		private Dictionary<string, string> DataContainer;

		private const bool NEED_BACKUP = true;

		public SavedData(string pPath, string pFileNameWithoutExtension)
		{
			MainFile = new AsyncFileIO(pPath, pFileNameWithoutExtension + ".json");
			BackupFile = new AsyncFileIO(pPath, pFileNameWithoutExtension + ".bak");
			MainTaskKey = string.Format("File_{0}_MainTaskDone", pFileNameWithoutExtension);
			BackupTaskKey = string.Format("File_{0}_BackupTaskDone", pFileNameWithoutExtension);
			DataContainer = new Dictionary<string, string>();
		}

		public void DeleteKeys()
		{
			PlayerPrefsManager.DeleteKey(MainTaskKey);
			PlayerPrefsManager.DeleteKey(BackupTaskKey);
		}

		public void Close()
		{
			MainFile.Close();
			BackupFile.Close();
		}

		public string ToTaskDebugMessage()
		{
			return string.Concat("Task status" + string.Format("\nIs main task done? {0}", PlayerPrefsManager.GetKey<bool>(MainTaskKey)), string.Format("\nIs backup task done? {0}", PlayerPrefsManager.GetKey<bool>(BackupTaskKey)));
		}

		public bool HasData(string pPersistentKey)
		{
			return DataContainer.ContainsKey(pPersistentKey);
		}

		public void DeleteData(string pPersistentKey)
		{
			if (DataContainer.ContainsKey(pPersistentKey))
			{
				DataContainer.Remove(pPersistentKey);
			}
		}

		public void SetData(string pPersistentKey, object pData)
		{
			if (!DataContainer.ContainsKey(pPersistentKey))
			{
				DataContainer.Add(pPersistentKey, null);
			}
			DataContainer[pPersistentKey] = JsonConvert.SerializeObject(pData);
		}

		public T GetData<T>(string pPersistentKey)
		{
			if (!DataContainer.ContainsKey(pPersistentKey))
			{
				return default(T);
			}
			return JsonConvert.DeserializeObject<T>(DataContainer[pPersistentKey]);
		}

		public void ClearData()
		{
			DataContainer.Clear();
		}

		public void Save(Action pDoneCallback)
		{
			_003C_003Ec__DisplayClass15_0 _003C_003Ec__DisplayClass15_ = new _003C_003Ec__DisplayClass15_0();
			_003C_003Ec__DisplayClass15_._003C_003E4__this = this;
			_003C_003Ec__DisplayClass15_.pDoneCallback = pDoneCallback;
			if (IsFileBroken(MainTaskKey))
			{
				Debug.LogWarning("Main file has no data or it's broken.");
				SaveData(_003C_003Ec__DisplayClass15_._003CSave_003Eb__0);
			}
			else
			{
				BackupData(_003C_003Ec__DisplayClass15_._003CSave_003Eb__1);
			}
		}

		private void BackupData(Action pDoneCallback)
		{
			_003C_003Ec__DisplayClass16_0 _003C_003Ec__DisplayClass16_ = new _003C_003Ec__DisplayClass16_0();
			_003C_003Ec__DisplayClass16_._003C_003E4__this = this;
			_003C_003Ec__DisplayClass16_.pDoneCallback = pDoneCallback;
			PlayerPrefsManager.SetKey(BackupTaskKey, false);
			MainFile.Read(_003C_003Ec__DisplayClass16_._003CBackupData_003Eb__0);
		}

		private void SaveData(Action pDoneCallback)
		{
			_003C_003Ec__DisplayClass17_0 _003C_003Ec__DisplayClass17_ = new _003C_003Ec__DisplayClass17_0();
			_003C_003Ec__DisplayClass17_._003C_003E4__this = this;
			_003C_003Ec__DisplayClass17_.pDoneCallback = pDoneCallback;
			PlayerPrefsManager.SetKey(MainTaskKey, false);
			string pContent = JsonConvert.SerializeObject(DataContainer, Formatting.Indented);
			MainFile.Write(pContent, _003C_003Ec__DisplayClass17_._003CSaveData_003Eb__0);
		}

		public void Load(Action pDoneCallback)
		{
			if (IsFileBroken(MainTaskKey))
			{
				if (IsFileBroken(BackupTaskKey))
				{
					Debug.LogWarning("Empty data or all files are broken.");
					if (pDoneCallback != null)
					{
						pDoneCallback();
					}
				}
				else
				{
					LoadFromBackupFile(pDoneCallback);
				}
				return;
			}
			try
			{
				LoadFromMainFile(pDoneCallback);
			}
			catch (Exception)
			{
				LoadFromBackupFile(pDoneCallback);
			}
		}

		private void LoadFromBackupFile(Action pDoneCallback)
		{
			_003C_003Ec__DisplayClass19_0 _003C_003Ec__DisplayClass19_ = new _003C_003Ec__DisplayClass19_0();
			_003C_003Ec__DisplayClass19_._003C_003E4__this = this;
			_003C_003Ec__DisplayClass19_.pDoneCallback = pDoneCallback;
			Debug.LogWarning("Main file is broken. Load data from backup file.");
			BackupFile.Read(_003C_003Ec__DisplayClass19_._003CLoadFromBackupFile_003Eb__0);
		}

		private void LoadFromMainFile(Action pDoneCallback)
		{
			_003C_003Ec__DisplayClass20_0 _003C_003Ec__DisplayClass20_ = new _003C_003Ec__DisplayClass20_0();
			_003C_003Ec__DisplayClass20_._003C_003E4__this = this;
			_003C_003Ec__DisplayClass20_.pDoneCallback = pDoneCallback;
			MainFile.Read(_003C_003Ec__DisplayClass20_._003CLoadFromMainFile_003Eb__0);
		}

		private void Deserialize(string pData, Action pDoneCallback)
		{
			if (string.IsNullOrEmpty(pData))
			{
				DataContainer = new Dictionary<string, string>();
			}
			else
			{
				DataContainer = JsonConvert.DeserializeObject<Dictionary<string, string>>(pData);
			}
			if (pDoneCallback != null)
			{
				pDoneCallback();
			}
		}

		private bool IsFileBroken(string pTaskKey)
		{
			if (PlayerPrefsManager.GetKey<bool>(pTaskKey))
			{
				return false;
			}
			return true;
		}
	}
}
