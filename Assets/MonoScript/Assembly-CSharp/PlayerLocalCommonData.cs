using System;
using UnityEngine;

[Serializable]
public class PlayerLocalCommonData
{
	public int freeGoodsTimes;

	public void Init()
	{
		string config = EncodeConfig.getConfig(PlayerLocalDatakey.PLAYERLOCALCOMMONDATA);
		if (config.Length > 0)
		{
			JsonUtility.FromJsonOverwrite(config, this);
		}
	}

	public void Save()
	{
		string values = JsonUtility.ToJson(this);
		EncodeConfig.setConfig(PlayerLocalDatakey.PLAYERLOCALCOMMONDATA, values);
	}

	public void ResetNextDay()
	{
		freeGoodsTimes = 0;
	}

	public bool CanGetFreeGoods()
	{
		return freeGoodsTimes < GameCommon.maxFreeGoodsTimes;
	}

	public bool CanGetFreeGoodsNoAd()
	{
		return freeGoodsTimes < GameCommon.maxFreeGoodsNoAdTimes;
	}

	public void CountFreeGoodsTimes()
	{
		freeGoodsTimes++;
		Save();
	}
}
