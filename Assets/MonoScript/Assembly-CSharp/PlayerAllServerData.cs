using System;
using System.Collections.Generic;

[Serializable]
public class PlayerAllServerData
{
	public List<PlayerServerGoodsData> assertDataList = new List<PlayerServerGoodsData>();

	public List<PlayerServerLevelData> levelDataList = new List<PlayerServerLevelData>();

	public PlayerLocalSequenceLoginData sequenceLoginData = new PlayerLocalSequenceLoginData();

	public PlayerLocalLevelSeriesData levelSeriesData = new PlayerLocalLevelSeriesData();

	public PlayerLocalGiftpackageData giftData = new PlayerLocalGiftpackageData();

	public long serverTime;

	public PlayerServerPropertyData propertyData = new PlayerServerPropertyData();

	public List<PlayerServerInstrumentData> instrumentDataList = new List<PlayerServerInstrumentData>();

	public List<PlayerServerMessionData> messionDataList = new List<PlayerServerMessionData>();

	public void InitFromLocalData()
	{
		PlayerLocalGoodsAndLevelData playerAllLocalData = PlayerDataModule.Instance.PlayerAllLocalData;
		PlayerServerGoodsData playerServerGoodsData = null;
		for (int i = 0; i < playerAllLocalData.assertDataList.Count; i++)
		{
			playerServerGoodsData = new PlayerServerGoodsData();
			playerServerGoodsData.InitFromLocalData(playerAllLocalData.assertDataList[i]);
			assertDataList.Add(playerServerGoodsData);
		}
		PlayerServerLevelData playerServerLevelData = null;
		for (int j = 0; j < playerAllLocalData.levelDataList.Count; j++)
		{
			if (MonoSingleton<GameTools>.Instacne.IsLevelInVersion(playerAllLocalData.levelDataList[j].levelId))
			{
				playerServerLevelData = new PlayerServerLevelData();
				playerServerLevelData.InitFromLocalData(playerAllLocalData.levelDataList[j]);
				levelDataList.Add(playerServerLevelData);
			}
		}
		sequenceLoginData = PlayerDataModule.Instance.SequenceLoginData;
		levelSeriesData = PlayerDataModule.Instance.PlayerLocalLevelSeriesData;
		giftData = PlayerDataModule.Instance.PlayerGiftPackageData;
		PlayerLocalPropertyData propertyLocalData = PlayerDataModule.Instance.PropertyLocalData;
		propertyData.InitFromLocalData(propertyLocalData);
		PlayerLocalAllInstrumentData playerInstrumentLocalData = PlayerDataModule.Instance.PlayerInstrumentLocalData;
		PlayerServerInstrumentData playerServerInstrumentData = null;
		for (int k = 0; k < playerInstrumentLocalData.m_InstrumentList.Count; k++)
		{
			if ((int)playerInstrumentLocalData.m_InstrumentList[k].LockState < 0)
			{
				playerServerInstrumentData = new PlayerServerInstrumentData();
				playerServerInstrumentData.InitFromLocalData(playerInstrumentLocalData.m_InstrumentList[k]);
				instrumentDataList.Add(playerServerInstrumentData);
			}
		}
		PlayerLocalAllMessionData messionData = PlayerDataModule.Instance.MessionData;
		messionDataList.Clear();
		PlayerServerMessionData playerServerMessionData = null;
		for (int l = 0; l < messionData.missionLocalData.Count; l++)
		{
			playerServerMessionData = new PlayerServerMessionData();
			playerServerMessionData.InitFromLocalData(messionData.missionLocalData[l]);
			messionDataList.Add(playerServerMessionData);
		}
	}
}
