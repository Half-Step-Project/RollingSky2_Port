using System;
using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;

[Serializable]
public class PlayerLocalGoodsAndLevelData
{
	public static bool NeedSave;

	public List<PlayerLocalGoodsData> assertDataList = new List<PlayerLocalGoodsData>();

	public List<PlayerLocalLevelData> levelDataList = new List<PlayerLocalLevelData>();

	private const int CHEAT_THRESHOLD = 1000000;

	public long serverTime;

	public int GoodsCount
	{
		get
		{
			return assertDataList.Count;
		}
	}

	public void SetGoodsNum(int goodsId, double goodsNum, bool save = true)
	{
		PlayerLocalGoodsData playerLocalGoodsData = assertDataList.Find((PlayerLocalGoodsData x) => x.GoodsId == goodsId);
		if (playerLocalGoodsData != null)
		{
			playerLocalGoodsData.Num = goodsNum;
		}
		else
		{
			playerLocalGoodsData = new PlayerLocalGoodsData();
			playerLocalGoodsData.GoodsId = goodsId;
			playerLocalGoodsData.Num = goodsNum;
			assertDataList.Add(playerLocalGoodsData);
		}
		if (save)
		{
			SaveToLocal();
		}
	}

	public bool IsHadDatas()
	{
		if (assertDataList.Count <= 0)
		{
			return levelDataList.Count > 0;
		}
		return true;
	}

	public void SetLevelMaxProgressData(int levelId, int percentage, int maxDiamondsCollected, int maxCrownsCollected, int[] crownCollect, int[] diamondsCollect)
	{
		PlayerLocalLevelData playerLocalLevelData = levelDataList.Find((PlayerLocalLevelData x) => x.levelId == levelId);
		if (playerLocalLevelData != null)
		{
			playerLocalLevelData.SetLevelMaxProgressData(percentage, maxDiamondsCollected, maxCrownsCollected, crownCollect, diamondsCollect);
		}
		else
		{
			playerLocalLevelData = Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).GetPlayerLevelData(levelId);
			playerLocalLevelData.SetLevelMaxProgressData(percentage, maxDiamondsCollected, maxCrownsCollected, crownCollect, diamondsCollect);
			levelDataList.Add(playerLocalLevelData);
		}
		SaveToLocal();
	}

	public void SetLevelBuyOutRebirth(int levelId, int isBuy)
	{
		PlayerLocalLevelData playerLocalLevelData = levelDataList.Find((PlayerLocalLevelData x) => x.levelId == levelId);
		if (playerLocalLevelData != null)
		{
			playerLocalLevelData.SetBuyOutRebirth(isBuy);
		}
		else
		{
			playerLocalLevelData = new PlayerLocalLevelData(levelId);
			playerLocalLevelData.SetBuyOutRebirth(isBuy);
			levelDataList.Add(playerLocalLevelData);
		}
		SaveToLocal();
	}

	public void SetLevelIsPerfect(int levelId, int isPerfect)
	{
		PlayerLocalLevelData playerLocalLevelData = levelDataList.Find((PlayerLocalLevelData x) => x.levelId == levelId);
		if (playerLocalLevelData == null)
		{
			playerLocalLevelData = new PlayerLocalLevelData(levelId);
			levelDataList.Add(playerLocalLevelData);
		}
		playerLocalLevelData.IsPerfect = isPerfect;
		SaveToLocal();
	}

	public void SetLocalLevelIsAllStarPlay(int levelId, int isPlay)
	{
		PlayerLocalLevelData playerLocalLevelData = levelDataList.Find((PlayerLocalLevelData x) => x.levelId == levelId);
		if (playerLocalLevelData == null)
		{
			playerLocalLevelData = new PlayerLocalLevelData(levelId);
			levelDataList.Add(playerLocalLevelData);
		}
		playerLocalLevelData.isAllStarPlay = isPlay;
		SaveToLocal();
	}

	public void SetLocalLevelIsPerfectAllStarPlay(int levelId, int isPlay)
	{
		PlayerLocalLevelData playerLocalLevelData = levelDataList.Find((PlayerLocalLevelData x) => x.levelId == levelId);
		if (playerLocalLevelData == null)
		{
			playerLocalLevelData = new PlayerLocalLevelData(levelId);
			levelDataList.Add(playerLocalLevelData);
		}
		playerLocalLevelData.isPerfectAllStarPlay = isPlay;
		SaveToLocal();
	}

	public void SetLocalLevelIsClickCupTips0(int levelId, int isClick)
	{
		PlayerLocalLevelData playerLocalLevelData = levelDataList.Find((PlayerLocalLevelData x) => x.levelId == levelId);
		if (playerLocalLevelData == null)
		{
			playerLocalLevelData = new PlayerLocalLevelData(levelId);
			levelDataList.Add(playerLocalLevelData);
		}
		playerLocalLevelData.isClickCupTips0 = isClick;
		SaveToLocal();
	}

	public void SetLocalLevelIsClickCupTips1(int levelId, int isClick)
	{
		PlayerLocalLevelData playerLocalLevelData = levelDataList.Find((PlayerLocalLevelData x) => x.levelId == levelId);
		if (playerLocalLevelData == null)
		{
			playerLocalLevelData = new PlayerLocalLevelData(levelId);
			levelDataList.Add(playerLocalLevelData);
		}
		playerLocalLevelData.isClickCupTips1 = isClick;
		SaveToLocal();
	}

	public void SetLevelLockState(int levelId, int state)
	{
		PlayerLocalLevelData playerLocalLevelData = levelDataList.Find((PlayerLocalLevelData x) => x.levelId == levelId);
		if (playerLocalLevelData != null)
		{
			playerLocalLevelData.LockState = state;
		}
		else
		{
			playerLocalLevelData = new PlayerLocalLevelData(levelId);
			playerLocalLevelData.LockState = state;
			levelDataList.Add(playerLocalLevelData);
		}
		SaveToLocal();
	}

	public void SetLevelTicketNum(int levelId, int num)
	{
		PlayerLocalLevelData playerLocalLevelData = levelDataList.Find((PlayerLocalLevelData x) => x.levelId == levelId);
		if (playerLocalLevelData != null)
		{
			playerLocalLevelData.SetTicketNum(num);
		}
		else
		{
			playerLocalLevelData = new PlayerLocalLevelData(levelId);
			playerLocalLevelData.SetTicketNum(num);
			levelDataList.Add(playerLocalLevelData);
		}
		SaveToLocal();
	}

	public void SetLevelAdEnterTime(int levelId, int lastTime)
	{
		PlayerLocalLevelData playerLocalLevelData = levelDataList.Find((PlayerLocalLevelData x) => x.levelId == levelId);
		if (playerLocalLevelData != null)
		{
			playerLocalLevelData.FreeEnterLevelTimeStamp = PlayerDataModule.Instance.ServerTime + lastTime * 1000;
		}
		else
		{
			playerLocalLevelData = new PlayerLocalLevelData(levelId);
			playerLocalLevelData.FreeEnterLevelTimeStamp = PlayerDataModule.Instance.ServerTime + lastTime * 1000;
			levelDataList.Add(playerLocalLevelData);
		}
		SaveToLocal();
	}

	public void InitFromLocal()
	{
		string config = EncodeConfig.getConfig(PlayerLocalDatakey.PLAYEGOODSDATA);
		if (config.Length > 0)
		{
			JsonUtility.FromJsonOverwrite(config, this);
		}
		for (int j = 0; j < assertDataList.Count; j++)
		{
			assertDataList[j].InitFromLocal();
		}
		levelDataList.RemoveAll((PlayerLocalLevelData x) => !MonoSingleton<GameTools>.Instacne.IsLevelInVersion(x.levelId));
		Levels_levelTable[] levelTableArr = Mod.DataTable.Get<Levels_levelTable>().Records;
		PlayerLocalLevelData playerLocalLevelData = null;
		bool flag = false;
		int i;
		for (i = 0; i < levelTableArr.Length; i++)
		{
			if (MonoSingleton<GameTools>.Instacne.IsLevelInVersion(levelTableArr[i].Id))
			{
				playerLocalLevelData = levelDataList.Find((PlayerLocalLevelData x) => x.levelId == levelTableArr[i].Id);
				if (playerLocalLevelData == null)
				{
					playerLocalLevelData = new PlayerLocalLevelData(levelTableArr[i].Id);
					levelDataList.Add(playerLocalLevelData);
					flag = true;
				}
			}
		}
		for (int k = 0; k < levelDataList.Count; k++)
		{
			levelDataList[k].InitFromLocal();
		}
		if (flag)
		{
			SaveToLocal();
		}
	}

	public void SaveToLocal()
	{
		for (int i = 0; i < assertDataList.Count; i++)
		{
			assertDataList[i].SaveToLocal();
		}
		for (int j = 0; j < levelDataList.Count; j++)
		{
			levelDataList[j].SaveToLocal();
		}
		string values = JsonUtility.ToJson(this);
		Debug.LogWarning("SaveToLocal");
		EncodeConfig.setConfig(PlayerLocalDatakey.PLAYEGOODSDATA, values);
		NeedSave = false;
	}

	public double CompareWithServerData(PlayerAllServerData server)
	{
		if (server == null)
		{
			return 1.0;
		}
		double num = 0.0;
		PlayerLocalGoodsData playerLocalGoodsData = assertDataList.Find((PlayerLocalGoodsData x) => x.GoodsId == 6);
		PlayerServerGoodsData playerServerGoodsData = server.assertDataList.Find((PlayerServerGoodsData x) => x.goodsId == 6);
		double num2 = ((playerLocalGoodsData != null) ? playerLocalGoodsData.Num : 0.0);
		double num3 = ((playerServerGoodsData != null) ? playerServerGoodsData.num : 0.0);
		if (num3 >= 1000000.0)
		{
			ClearData();
			return 1.0;
		}
		num = num2 - num3;
		if (num != 0.0)
		{
			return num;
		}
		playerLocalGoodsData = assertDataList.Find((PlayerLocalGoodsData x) => x.GoodsId == 3);
		playerServerGoodsData = server.assertDataList.Find((PlayerServerGoodsData x) => x.goodsId == 3);
		double num4 = ((playerLocalGoodsData != null) ? playerLocalGoodsData.Num : 0.0);
		double num5 = ((playerServerGoodsData != null) ? playerServerGoodsData.num : 0.0);
		num = num4 - num5;
		if (num != 0.0)
		{
			return num;
		}
		playerLocalGoodsData = assertDataList.Find((PlayerLocalGoodsData x) => x.GoodsId == GameCommon.REPUTATION_ID);
		playerServerGoodsData = server.assertDataList.Find((PlayerServerGoodsData x) => x.goodsId == GameCommon.REPUTATION_ID);
		double num6 = ((playerLocalGoodsData != null) ? playerLocalGoodsData.Num : 0.0);
		double num7 = ((playerServerGoodsData != null) ? playerServerGoodsData.num : 0.0);
		num = num6 - num7;
		if (num != 0.0)
		{
			return num;
		}
		PlayerLocalLevelData playerLocalLevelData = null;
		PlayerServerLevelData playerServerLevelData = null;
		int num8 = 0;
		int num9 = 0;
		int i = 0;
		for (int count = levelDataList.Count; i < count; i++)
		{
			playerLocalLevelData = levelDataList[i];
			if (playerLocalLevelData != null && playerLocalLevelData.LockState <= 0 && MonoSingleton<GameTools>.Instacne.IsLevelInVersion(playerLocalLevelData.levelId))
			{
				num8++;
			}
		}
		int j = 0;
		for (int count2 = server.levelDataList.Count; j < count2; j++)
		{
			playerServerLevelData = server.levelDataList[j];
			if (playerServerLevelData != null && playerServerLevelData.lockState <= 0 && playerServerLevelData.levelId >= 10000)
			{
				num9++;
			}
		}
		num = num8 - num9;
		double num10 = 0.0;
		return num;
	}

	public void ResetFromServerData(PlayerAllServerData serverData)
	{
		assertDataList.Clear();
		PlayerLocalGoodsData playerLocalGoodsData = null;
		int i = 0;
		for (int count = serverData.assertDataList.Count; i < count; i++)
		{
			playerLocalGoodsData = new PlayerLocalGoodsData();
			playerLocalGoodsData.InitFormServerData(serverData.assertDataList[i]);
			assertDataList.Add(playerLocalGoodsData);
		}
		levelDataList.Clear();
		PlayerLocalLevelData playerLocalLevelData = null;
		int j = 0;
		for (int count2 = serverData.levelDataList.Count; j < count2; j++)
		{
			if (MonoSingleton<GameTools>.Instacne.IsLevelInVersion(serverData.levelDataList[j].levelId))
			{
				playerLocalLevelData = new PlayerLocalLevelData(serverData.levelDataList[j].levelId);
				playerLocalLevelData.InitFormServerData(serverData.levelDataList[j]);
				levelDataList.Add(playerLocalLevelData);
			}
		}
		serverTime = serverData.serverTime;
		SaveToLocal();
	}

	public void ClearData()
	{
		assertDataList.Clear();
		levelDataList.Clear();
		SaveToLocal();
	}

	public void InitLevelDefaultData()
	{
		List<int> list = new List<int>();
		List<LevelMetaTableData>.Enumerator enumator = PlayerDataModule.Instance.GloableLevelLableData.GetEnumerator();
		while (enumator.MoveNext())
		{
			if (!levelDataList.Exists((PlayerLocalLevelData x) => x.levelId == enumator.Current.LevelId))
			{
				list.Add(enumator.Current.LevelId);
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			levelDataList.Add(new PlayerLocalLevelData(list[i]));
		}
		SaveToLocal();
	}

	public void CheckNeedSave()
	{
		if (NeedSave)
		{
			SaveToLocal();
		}
	}
}
