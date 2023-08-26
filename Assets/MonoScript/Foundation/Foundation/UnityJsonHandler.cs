using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Foundation
{
	public sealed class UnityJsonHandler : IJsonHandler
	{
		[CompilerGenerated]
		private static readonly UnityJsonHandler _003CDefault_003Ek__BackingField = new UnityJsonHandler();

		public static UnityJsonHandler Default
		{
			[CompilerGenerated]
			get
			{
				return _003CDefault_003Ek__BackingField;
			}
		}

		public string ToJson(object obj)
		{
			return JsonUtility.ToJson(obj);
		}

		public T ToObject<T>(string json)
		{
			return JsonUtility.FromJson<T>(json);
		}

		public object ToObject(Type type, string json)
		{
			return JsonUtility.FromJson(json, type);
		}
	}
}
