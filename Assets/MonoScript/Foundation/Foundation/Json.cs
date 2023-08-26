using System;
using System.Text;

namespace Foundation
{
	public static class Json
	{
		public static IJsonHandler Handler { get; set; }

		public static string ToJson(object obj)
		{
			if (Handler == null)
			{
				Log.Warning("JSON handler is null.");
				return null;
			}
			return Handler.ToJson(obj);
		}

		public static byte[] ToJsonData(object obj)
		{
			return Encoding.UTF8.GetBytes(ToJson(obj));
		}

		public static T ToObject<T>(string json)
		{
			if (Handler == null)
			{
				Log.Warning("JSON handler is null.");
				return default(T);
			}
			return Handler.ToObject<T>(json);
		}

		public static object ToObject(Type type, string json)
		{
			if (Handler == null)
			{
				Log.Warning("JSON handler is null.");
				return null;
			}
			if ((object)type == null)
			{
				Log.Warning("Object type is null.");
				return null;
			}
			return Handler.ToObject(type, json);
		}

		public static T ToObject<T>(byte[] jsonData)
		{
			return ToObject<T>(Encoding.UTF8.GetString(jsonData));
		}

		public static object ToObject(Type type, byte[] jsonData)
		{
			return ToObject(type, Encoding.UTF8.GetString(jsonData));
		}
	}
}
