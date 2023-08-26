using System;
using UnityEngine;

namespace Foundation
{
	[DisallowMultipleComponent]
	[AddComponentMenu("Framework/Setting")]
	public sealed class SettingMod : ModBase
	{
		public void Save()
		{
			PlayerPrefs.Save();
		}

		public bool Contains(string key)
		{
			return PlayerPrefs.HasKey(key);
		}

		public void Remove(string key)
		{
			PlayerPrefs.DeleteKey(key);
		}

		public void RemoveAll()
		{
			PlayerPrefs.DeleteAll();
		}

		public bool GetBool(string key, bool defaultValue = false)
		{
			return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) != 0;
		}

		public void SetBool(string key, bool value = false)
		{
			PlayerPrefs.SetInt(key, value ? 1 : 0);
		}

		public int GetInt(string key, int defaultValue = 0)
		{
			return PlayerPrefs.GetInt(key, defaultValue);
		}

		public void SetInt(string key, int value)
		{
			PlayerPrefs.SetInt(key, value);
		}

		public float GetFloat(string key, float defaultValue = 0f)
		{
			return PlayerPrefs.GetFloat(key, defaultValue);
		}

		public void SetFloat(string key, float value)
		{
			PlayerPrefs.SetFloat(key, value);
		}

		public string GetString(string key, string defaultValue = null)
		{
			return PlayerPrefs.GetString(key, defaultValue);
		}

		public void SetString(string key, string value)
		{
			PlayerPrefs.SetString(key, value);
		}

		public T GetObject<T>(string key, T defaultObj = default(T))
		{
			string @string = PlayerPrefs.GetString(key, null);
			if (@string != null)
			{
				return Json.ToObject<T>(@string);
			}
			return defaultObj;
		}

		public object GetObject(Type type, string key, object defaultObj = null)
		{
			string @string = PlayerPrefs.GetString(key, null);
			if (@string != null)
			{
				return Json.ToObject(type, @string);
			}
			return defaultObj;
		}

		public void SetObject<T>(string key, T obj)
		{
			PlayerPrefs.SetString(key, Json.ToJson(obj));
		}

		public void SetObject(string key, object obj)
		{
			PlayerPrefs.SetString(key, Json.ToJson(obj));
		}

		protected override void Awake()
		{
			Mod.Setting = this;
		}

		internal override void OnTick(float elapseSeconds, float realElapseSeconds)
		{
		}
	}
}
