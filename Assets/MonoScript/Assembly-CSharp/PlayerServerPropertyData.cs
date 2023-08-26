using System;
using System.Collections.Generic;

[Serializable]
public class PlayerServerPropertyData
{
	public int level;

	public int starLevel;

	public int hideLevel;

	public List<PlayerServerGoodsStatisticalData> playerGoodsRecordData = new List<PlayerServerGoodsStatisticalData>();

	public void InitFromLocalData(PlayerLocalPropertyData localData)
	{
		level = localData.Level;
		starLevel = localData.StarLevel;
		hideLevel = localData.HideLevel;
		for (int i = 0; i < localData.PlayerGoodsRecordData.Count; i++)
		{
			new PlayerServerGoodsStatisticalData().InitFromLocal(localData.PlayerGoodsRecordData[i]);
		}
	}
}
