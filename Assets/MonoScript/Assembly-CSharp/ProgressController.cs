using Foundation;
using UnityEngine;

public class ProgressController : IOriginRebirth
{
	public bool IsRecordOriginRebirth
	{
		get
		{
			return true;
		}
	}

	public void Shutdown()
	{
	}

	public object GetOriginRebirthData(object obj = null)
	{
		string empty = string.Empty;
		OriginRebirthProgressData originRebirthProgressData = default(OriginRebirthProgressData);
		InsideGameDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule);
		originRebirthProgressData.m_levelProgress = dataModule.ProgressPercentage;
		return JsonUtility.ToJson(originRebirthProgressData);
	}

	public void SetOriginRebirthData(object dataInfo)
	{
		string text = (string)dataInfo;
		if (!string.IsNullOrEmpty(text))
		{
			OriginRebirthProgressData originRebirthProgressData = JsonUtility.FromJson<OriginRebirthProgressData>(text);
			Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule).ProgressPercentage = originRebirthProgressData.m_levelProgress;
		}
	}

	public void StartRunByOriginRebirthData(object dataInfo)
	{
	}

	public byte[] GetOriginRebirthBsonData(object obj = null)
	{
		byte[] array = new byte[0];
		OriginRebirthProgressData originRebirthProgressData = default(OriginRebirthProgressData);
		InsideGameDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule);
		originRebirthProgressData.m_levelProgress = dataModule.ProgressPercentage;
		return Bson.ToBson(originRebirthProgressData);
	}

	public void SetOriginRebirthBsonData(byte[] dataInfo)
	{
		if (dataInfo != null)
		{
			OriginRebirthProgressData originRebirthProgressData = Bson.ToObject<OriginRebirthProgressData>(dataInfo);
			Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule).ProgressPercentage = originRebirthProgressData.m_levelProgress;
		}
	}

	public void StartRunByOriginRebirthBsonData(byte[] dataInfo)
	{
	}

	public static OriginRebirthProgressData LoadData(string json)
	{
		return JsonUtility.FromJson<OriginRebirthProgressData>(json);
	}

	public static OriginRebirthProgressData LoadData(byte[] bson)
	{
		return Bson.ToObject<OriginRebirthProgressData>(bson);
	}
}
