using System;
using System.Text;
using Newtonsoft.Json;

public class ExampleSaveData
{
	public string data;

	public long saveTimes;

	public ExampleSaveData()
	{
	}

	public ExampleSaveData(ExampleSaveData cloneData)
	{
		CloneFrom(cloneData);
	}

	public void SetDefault()
	{
		data = "{}";
		saveTimes = DateTime.Now.Ticks;
	}

	public void CloneFrom(ExampleSaveData cloneData)
	{
		data = cloneData.data;
		saveTimes = cloneData.saveTimes;
	}

	public static string ToJson(ExampleSaveData ExampleSaveData)
	{
		return JsonConvert.SerializeObject(ExampleSaveData);
	}

	public static ExampleSaveData FromJson(byte[] data)
	{
		return JsonConvert.DeserializeObject<ExampleSaveData>(Encoding.UTF8.GetString(data));
	}

	public static ExampleSaveData FromJson(string data)
	{
		return JsonConvert.DeserializeObject<ExampleSaveData>(data);
	}
}
