using System;
using Foundation;
using RS2;
using UnityEngine;

public class ExchangeStoreDataModule : IDataModule
{
	public enum BuyType
	{
		Normal,
		AD
	}

	[Serializable]
	public struct ProductData
	{
		public int mID;

		[NonSerialized]
		public ExchangeStore_table mTable;

		public int mBuyCount;

		public int mAdCount;

		public long mTime;
	}

	[Serializable]
	public struct SaveData
	{
		public ProductData[] mProductDatas;
	}

	public SaveData mSaveData;

	public DataNames GetName()
	{
		return DataNames.ExchangeStoreDataModule;
	}

	public ExchangeStoreDataModule()
	{
		Read();
	}

	public void BuySuccess(int index, BuyType buyType)
	{
		if (buyType == BuyType.AD)
		{
			mSaveData.mProductDatas[index].mAdCount++;
			mSaveData.mProductDatas[index].mTime = PlayerDataModule.Instance.ServerTime;
		}
		mSaveData.mProductDatas[index].mBuyCount++;
		Write();
	}

	public bool IsOnCDTime(int index)
	{
		if (mSaveData.mProductDatas[index].mTime == 0L)
		{
			return false;
		}
		long num = PlayerDataModule.Instance.ServerTime - mSaveData.mProductDatas[index].mTime;
		long num2 = mSaveData.mProductDatas[index].mTable.Ad_cd * 1000;
		if (num < num2)
		{
			return num > 0;
		}
		return false;
	}

	public long GetCdRemainingTime(int index)
	{
		if (mSaveData.mProductDatas[index].mTime == 0L)
		{
			return 0L;
		}
		long num = mSaveData.mProductDatas[index].mTable.Ad_cd * 1000;
		return mSaveData.mProductDatas[index].mTime + num - PlayerDataModule.Instance.ServerTime;
	}

	public bool IsAd(int index)
	{
		return mSaveData.mProductDatas[index].mTable.Ad_get == 1;
	}

	public bool IsNeedRedPoint()
	{
		bool result = false;
		for (int i = 0; i < mSaveData.mProductDatas.Length; i++)
		{
			if (IsAd(i) && !IsOnCDTime(i) && MonoSingleton<GameTools>.Instacne.CanShowAd(ADScene.NONE))
			{
				result = true;
				break;
			}
		}
		return result;
	}

	private void Read()
	{
		ExchangeStore_table[] records = Mod.DataTable.Get<ExchangeStore_table>().Records;
		string jsonData = GetJsonData();
		if (string.IsNullOrEmpty(jsonData))
		{
			mSaveData = default(SaveData);
			mSaveData.mProductDatas = new ProductData[records.Length];
			for (int i = 0; i < records.Length; i++)
			{
				ProductData productData = default(ProductData);
				productData.mID = records[i].Id;
				productData.mTable = records[i];
				productData.mTime = 0L;
				mSaveData.mProductDatas[i] = productData;
			}
			return;
		}
		mSaveData = JsonUtility.FromJson<SaveData>(jsonData);
		if (mSaveData.mProductDatas.Length != records.Length)
		{
			mSaveData = default(SaveData);
			mSaveData.mProductDatas = new ProductData[records.Length];
			for (int j = 0; j < records.Length; j++)
			{
				ProductData productData2 = default(ProductData);
				productData2.mID = records[j].Id;
				productData2.mTable = records[j];
				productData2.mTime = 0L;
				mSaveData.mProductDatas[j] = productData2;
			}
		}
		else
		{
			for (int k = 0; k < mSaveData.mProductDatas.Length; k++)
			{
				mSaveData.mProductDatas[k].mTable = records[k];
			}
		}
	}

	private void Write()
	{
		string eXCHANGESTOREDATA = PlayerLocalDatakey.EXCHANGESTOREDATA;
		string values = JsonUtility.ToJson(mSaveData);
		EncodeConfig.setConfig(eXCHANGESTOREDATA, values);
	}

	private string GetJsonData()
	{
		return EncodeConfig.getConfig(PlayerLocalDatakey.EXCHANGESTOREDATA, string.Empty);
	}
}
