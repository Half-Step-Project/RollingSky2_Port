using System;
using System.Runtime.CompilerServices;
using LitJson;

namespace Foundation
{
	public sealed class LitJsonHandler : IJsonHandler
	{
		[CompilerGenerated]
		private static readonly LitJsonHandler _003CDefault_003Ek__BackingField = new LitJsonHandler();

		public static LitJsonHandler Default
		{
			[CompilerGenerated]
			get
			{
				return _003CDefault_003Ek__BackingField;
			}
		}

		public string ToJson(object obj)
		{
			return JsonMapper.ToJson(obj);
		}

		public T ToObject<T>(string json)
		{
			return JsonMapper.ToObject<T>(json);
		}

		public object ToObject(Type type, string json)
		{
			Log.Error("NotImplementedException");
			return null;
		}
	}
}
