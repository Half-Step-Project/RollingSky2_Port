using System;
using System.Collections.Generic;
using UnityEngine;

namespace Umeng
{
	public class Analytics
	{
		private static string _AppKey;

		private static string _ChannelId;

		public static string AppKey
		{
			get
			{
				return _AppKey;
			}
		}

		public static string ChannelId
		{
			get
			{
				return _ChannelId;
			}
		}

		public static void Start()
		{
		}

		public static void SetLogEnabled(bool value)
		{
		}

		public static void Event(string eventId)
		{
		}

		public static void Event(string eventId, string label)
		{
		}

		public static void Event(string eventId, Dictionary<string, string> attributes)
		{
		}

		public static void PageBegin(string pageName)
		{
		}

		public static void PageEnd(string pageName)
		{
		}

		public static void Event(string eventId, Dictionary<string, string> attributes, int value)
		{
			try
			{
				if (attributes == null)
				{
					attributes = new Dictionary<string, string>();
				}
				if (attributes.ContainsKey("__ct__"))
				{
					attributes["__ct__"] = value.ToString();
					Event(eventId, attributes);
				}
				else
				{
					attributes.Add("__ct__", value.ToString());
					Event(eventId, attributes);
					attributes.Remove("__ct__");
				}
			}
			catch (Exception)
			{
			}
		}

		public static string GetDeviceInfo()
		{
			return null;
		}

		public static void SetLogEncryptEnabled(bool value)
		{
		}

		public static void SetLatency(int value)
		{
		}

		private static void CreateUmengManger()
		{
			GameObject gameObject = new GameObject();
			gameObject.AddComponent<UmengManager>();
			gameObject.name = "UmengManager";
		}
	}
}
