using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace JsonDotNet.Extras.CustomConverters
{
	public class Vector2Converter : JsonConverter
	{
		[Serializable]
		[CompilerGenerated]
		private sealed class _003C_003Ec
		{
			public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

			public static Func<JProperty, bool> _003C_003E9__0_0;

			public static Func<JProperty, string> _003C_003E9__0_1;

			internal bool _003CWriteJson_003Eb__0_0(JProperty p)
			{
				if (!(p.Name == "x"))
				{
					return p.Name == "y";
				}
				return true;
			}

			internal string _003CWriteJson_003Eb__0_1(JProperty p)
			{
				return p.Name;
			}
		}

		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			JToken jToken = JToken.FromObject(value);
			if (jToken.Type != JTokenType.Object)
			{
				jToken.WriteTo(writer);
				return;
			}
			JObject obj = (JObject)jToken;
			IList<string> content = obj.Properties().Where(_003C_003Ec._003C_003E9__0_0 ?? (_003C_003Ec._003C_003E9__0_0 = _003C_003Ec._003C_003E9._003CWriteJson_003Eb__0_0)).Select(_003C_003Ec._003C_003E9__0_1 ?? (_003C_003Ec._003C_003E9__0_1 = _003C_003Ec._003C_003E9._003CWriteJson_003Eb__0_1))
				.ToList();
			obj.AddFirst(new JProperty("Keys", new JArray(content)));
			obj.WriteTo(writer);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(Vector2);
		}
	}
}
