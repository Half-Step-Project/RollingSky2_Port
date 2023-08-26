using System;

namespace Foundation
{
	public interface IJsonHandler
	{
		string ToJson(object obj);

		T ToObject<T>(string json);

		object ToObject(Type type, string json);
	}
}
