using System;

namespace RisingWin.Library
{
	public class PlayerPrefsAdapter
	{
		public static void DeleteAll()
		{
			PlayerPrefsManager.DeleteAll();
		}

		public static void DeleteKey(string key)
		{
			PlayerPrefsManager.DeleteKey(key);
		}

		public static float GetFloat(string key)
		{
			return GetFloat(key, 0f);
		}

		public static float GetFloat(string key, float defaultValue)
		{
			float num = PlayerPrefsManager.GetKey<float>(key);
			if (num == 0f)
			{
				num = defaultValue;
			}
			return num;
		}

		public static int GetInt(string key)
		{
			return GetInt(key, 0);
		}

		public static int GetInt(string key, int defaultValue)
		{
			int num = PlayerPrefsManager.GetKey<int>(key);
			if (num == 0)
			{
				num = defaultValue;
			}
			return num;
		}

		public static string GetString(string key)
		{
			return GetString(key, string.Empty);
		}

		public static string GetString(string key, string defaultValue)
		{
			string text = PlayerPrefsManager.GetKey<string>(key);
			if (string.IsNullOrEmpty(text))
			{
				text = defaultValue;
			}
			return text;
		}

		public static long GetLong(string key, long defaultValue)
		{
			string key2 = PlayerPrefsManager.GetKey<string>(key);
			if (string.IsNullOrEmpty(key2))
			{
				return defaultValue;
			}
			return ToLong(key2);
		}

		public static ulong GetUlong(string key, ulong defaultValue)
		{
			string key2 = PlayerPrefsManager.GetKey<string>(key);
			if (string.IsNullOrEmpty(key2))
			{
				return defaultValue;
			}
			return ToULong(key2);
		}

		public static bool HasKey(string key)
		{
			return PlayerPrefsManager.HasKey(key);
		}

		public static void Save()
		{
			throw new NotImplementedException("Now is Automatic save mode.");
		}

		public static void SetFloat(string key, float value)
		{
			PlayerPrefsManager.SetKey(key, value);
		}

		public static void SetInt(string key, int value)
		{
			PlayerPrefsManager.SetKey(key, value);
		}

		public static void SetString(string key, string value)
		{
			PlayerPrefsManager.SetKey(key, value);
		}

		public static void SetLong(string key, long value)
		{
			PlayerPrefsManager.SetKey(key, value);
		}

		public static void SetUlong(string key, ulong value)
		{
			PlayerPrefsManager.SetKey(key, value);
		}

		public static long ToLong(string value)
		{
			long result;
			long.TryParse(value, out result);
			return result;
		}

		public static ulong ToULong(string value)
		{
			ulong result;
			ulong.TryParse(value, out result);
			return result;
		}
	}
}
