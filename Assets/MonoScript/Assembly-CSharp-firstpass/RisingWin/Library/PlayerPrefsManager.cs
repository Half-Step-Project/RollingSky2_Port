using System;
using System.IO;
using UnityEngine;

namespace RisingWin.Library
{
	public class PlayerPrefsManager
	{
		private const string FOLDER = "SavedData";

		private const string SLOT_NAME = "Auto-saved";

		public static void DoInitializationIfNeeded()
		{
		}

		public static TType GetOrCreateKey<TType>(string key, object value)
		{
			if (!HasKey(key))
			{
				SetKey(key, value);
				return (TType)value;
			}
			return GetKey<TType>(key);
		}

		public static bool HasKey(string key)
		{
			return PlayerPrefs.HasKey(key);
		}

		public static void SetKey(string key, object value)
		{
			if (value is string)
			{
				PlayerPrefs.SetString(key, value.ToString());
			}
			else if (value is int)
			{
				PlayerPrefs.SetInt(key, Convert.ToInt32(value));
			}
			else if (value is float)
			{
				PlayerPrefs.SetFloat(key, Convert.ToSingle(value));
			}
			else if (value is long)
			{
				PlayerPrefs.SetString(key, value.ToString());
			}
			else if (value is ulong)
			{
				PlayerPrefs.SetString(key, value.ToString());
			}
			else if (value is bool)
			{
				PlayerPrefs.SetString(key, value.ToString());
			}
		}

		public static TType GetKey<TType>(string key)
		{
			Type typeFromHandle = typeof(TType);
			object obj = new object();
			if (typeFromHandle == typeof(string))
			{
				obj = PlayerPrefs.GetString(key);
			}
			else if (typeFromHandle == typeof(int))
			{
				obj = PlayerPrefs.GetInt(key);
			}
			else if (typeFromHandle == typeof(float))
			{
				obj = PlayerPrefs.GetFloat(key);
			}
			else if (typeFromHandle == typeof(long))
			{
				obj = long.Parse(PlayerPrefs.GetString(key));
			}
			else if (typeFromHandle == typeof(ulong))
			{
				obj = ulong.Parse(PlayerPrefs.GetString(key));
			}
			else if (typeFromHandle == typeof(bool))
			{
				string @string = PlayerPrefs.GetString(key);
				obj = ((!string.IsNullOrEmpty(@string)) ? ((object)bool.Parse(@string)) : ((object)false));
			}
			return (TType)obj;
		}

		public static void DeleteKey(string key)
		{
			PlayerPrefs.DeleteKey(key);
		}

		public static void DeleteAll()
		{
			PlayerPrefs.DeleteAll();
			Singleton<SavedDataManager>.Instance.Reset();
			string path = Path.Combine(Application.persistentDataPath, "SavedData");
			if (Directory.Exists(path))
			{
				Directory.Delete(path, true);
			}
		}
	}
}
