using System;
using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;

[Serializable]
public class PlayerLocalLevelData : IDataToLocal
{
	public int levelId = -1;

	[NonSerialized]
	public int levelSeriesId;

	[SerializeField]
	private int maxProgress;

	[SerializeField]
	private int maxDiamondNum;

	[SerializeField]
	private int maxCrownNum;

	public List<int> currentCanFinishTargets = new List<int>();

	public int hadFinishTargetNum;

	[SerializeField]
	private int lockState = -100;

	public long initTime;

	public int tictetsNum;

	[SerializeField]
	private int buyOutRebirth;

	[SerializeField]
	private int isPerfect;

	public int isAllStarPlay;

	public int isPerfectAllStarPlay;

	public int isClickCupTips0;

	public int isClickCupTips1;

	[NonSerialized]
	public PlayerLocalLevelSeriesData.LocalLevelSeriesData levelSeriesData;

	[NonSerialized]
	public bool isShowGiftBox;

	private List<int> lastFinishTargets = new List<int>();

	[SerializeField]
	private long m_freeEnterLevelTimeStamp;

	[SerializeField]
	private int m_FinishLevelCount;

	[SerializeField]
	private int m_PlayLevelCount;

	[SerializeField]
	private int m_totalDiamondsCount;

	[SerializeField]
	private int m_totalCrownsCount;

	[SerializeField]
	private List<int> m_crownCollect = new List<int>();

	[SerializeField]
	private List<int> m_diamondsCollect = new List<int>();

	private CryVInt32 m_CrymaxProgress = 0;

	private CryVInt32 m_CrymaxDiamondNum = 0;

	private CryVInt32 m_CrymaxCrownNum = 0;

	private CryVInt32 m_CryxlockState = 0;

	private CryVInt32 m_CrybuyOutRebirth = 0;

	public int IsPerfect
	{
		get
		{
			return isPerfect;
		}
		set
		{
			isPerfect = value;
		}
	}

	public long FreeEnterLevelTimeStamp
	{
		get
		{
			return m_freeEnterLevelTimeStamp;
		}
		set
		{
			m_freeEnterLevelTimeStamp = value;
		}
	}

	public int FinishLevelCount
	{
		get
		{
			return m_FinishLevelCount;
		}
		set
		{
			m_FinishLevelCount = value;
		}
	}

	public int PlayLevelCount
	{
		get
		{
			return m_PlayLevelCount;
		}
		set
		{
			m_PlayLevelCount = value;
		}
	}

	public int TotalDiamondsCount
	{
		get
		{
			return m_totalDiamondsCount;
		}
		set
		{
			m_totalDiamondsCount = value;
		}
	}

	public int TotalCrownsCount
	{
		get
		{
			return m_totalCrownsCount;
		}
		set
		{
			m_totalCrownsCount = value;
		}
	}

	public List<int> CrownCollect
	{
		get
		{
			return m_crownCollect;
		}
	}

	public List<int> DiamondsCollect
	{
		get
		{
			return m_diamondsCollect;
		}
	}

	public int MaxProgress
	{
		get
		{
			return m_CrymaxProgress;
		}
		set
		{
			m_CrymaxProgress = value;
		}
	}

	public int MaxDiamondNum
	{
		get
		{
			return m_CrymaxDiamondNum;
		}
		set
		{
			m_CrymaxDiamondNum = value;
		}
	}

	public int MaxCrownNum
	{
		get
		{
			return m_CrymaxCrownNum;
		}
		set
		{
			m_CrymaxCrownNum = value;
		}
	}

	public int BuyOutRebirth
	{
		get
		{
			return m_CrybuyOutRebirth;
		}
		set
		{
			m_CrybuyOutRebirth = value;
		}
	}

	public int LockState
	{
		get
		{
			return m_CryxlockState;
		}
		set
		{
			m_CryxlockState = value;
		}
	}

	public int MaxStarLevel
	{
		get
		{
			return MonoSingleton<GameTools>.Instacne.ComputerLevelStar(levelId, MaxProgress, MaxDiamondNum, MaxCrownNum);
		}
	}

	public int GetLevelSeriesId()
	{
		if (levelSeriesId == 0)
		{
			LevelMetaTableData levelMetaTableData = PlayerDataModule.Instance.GloableLevelLableData.Find((LevelMetaTableData x) => x.LevelId == levelId);
			if (levelMetaTableData == null)
			{
				Log.Info("GetLevelSeriesId levelId " + levelId);
				levelSeriesId = -1;
			}
			else
			{
				levelSeriesId = levelMetaTableData.SeriesId;
			}
		}
		return levelSeriesId;
	}

	public PlayerLocalLevelSeriesData.LocalLevelSeriesData GetLevelSeriesData()
	{
		if (levelSeriesData == null)
		{
			levelSeriesData = PlayerDataModule.Instance.PlayerLocalLevelSeriesData.GetOrAddLocalLevelSeriesData(GetLevelSeriesId());
		}
		return levelSeriesData;
	}

	public PlayerLocalLevelData(int levelId)
	{
		this.levelId = levelId;
		Levels_levelTable levels_levelTable = Mod.DataTable.Get<Levels_levelTable>()[levelId];
		if (levels_levelTable == null)
		{
			return;
		}
		lockState = levels_levelTable.LockState;
		m_CryxlockState = lockState;
		initTime = PlayerDataModule.Instance.ServerTime;
		string[] array = levels_levelTable.TargetIds.Split('|');
		int levelTargetSelectedNum = GameCommon.levelTargetSelectedNum;
		new List<int>();
		if (levelTargetSelectedNum < array.Length)
		{
			int[] random = MonoSingleton<GameTools>.Instacne.GetRandom(array.Length, levelTargetSelectedNum);
			for (int i = 0; i < random.Length; i++)
			{
				currentCanFinishTargets.Add(int.Parse(array[random[i]]));
			}
		}
		else
		{
			for (int j = 0; j < array.Length; j++)
			{
				currentCanFinishTargets.Add(int.Parse(array[j]));
			}
		}
		int diamonds = levels_levelTable.Diamonds;
		int crowns = levels_levelTable.Crowns;
		m_crownCollect.Clear();
		m_diamondsCollect.Clear();
		for (int k = 0; k < crowns; k++)
		{
			m_crownCollect.Add(0);
		}
		for (int l = 0; l < diamonds; l++)
		{
			m_diamondsCollect.Add(0);
		}
	}

	public bool SetLevelMaxProgressData(int percentage, int maxDiamondsCollected, int maxCrownsCollected, int[] crownCollect, int[] diamondsCollect)
	{
		bool result = true;
		if (MaxProgress < percentage)
		{
			MaxProgress = percentage;
		}
		if (MaxCrownNum < maxCrownsCollected)
		{
			MaxCrownNum = maxCrownsCollected;
		}
		if (MaxDiamondNum < maxDiamondsCollected)
		{
			MaxDiamondNum = maxDiamondsCollected;
		}
		int num = ((crownCollect != null) ? crownCollect.Length : 0);
		for (int i = 0; i < num; i++)
		{
			if (m_crownCollect[i] != crownCollect[i])
			{
				SaveCrownCollect(crownCollect);
			}
		}
		num = ((diamondsCollect != null) ? diamondsCollect.Length : 0);
		for (int j = 0; j < num; j++)
		{
			if (m_diamondsCollect[j] != diamondsCollect[j])
			{
				SaveDiamondsCollect(diamondsCollect);
			}
		}
		return result;
	}

	public void SetLastFinishTargets(List<int> goalList)
	{
		if (lastFinishTargets == null)
		{
			lastFinishTargets = new List<int>();
		}
		lastFinishTargets.Clear();
		int i = 0;
		for (int count = goalList.Count; i < count; i++)
		{
			lastFinishTargets.Add(goalList[i]);
		}
	}

	public List<int> GetLastFinishTargets()
	{
		if (lastFinishTargets == null)
		{
			lastFinishTargets = new List<int>();
		}
		return lastFinishTargets;
	}

	public List<int> GetCurrentCanFinishTargets()
	{
		return currentCanFinishTargets;
	}

	public void SetCurrentCanFinishTargets(List<int> goalList)
	{
		if (currentCanFinishTargets == null)
		{
			currentCanFinishTargets = new List<int>();
		}
		if (goalList != currentCanFinishTargets)
		{
			currentCanFinishTargets.Clear();
			int i = 0;
			for (int count = goalList.Count; i < count; i++)
			{
				currentCanFinishTargets.Add(goalList[i]);
			}
		}
	}

	public int GetLockState()
	{
		return m_CryxlockState;
	}

	public void SetLockState(int state)
	{
		m_CryxlockState = state;
	}

	public int GetBuyOutRebirth()
	{
		return BuyOutRebirth;
	}

	public void SetBuyOutRebirth(int isBuy)
	{
		BuyOutRebirth = isBuy;
	}

	public void Clear()
	{
		levelId = -1;
		MaxProgress = 0;
		MaxDiamondNum = 0;
		MaxCrownNum = 0;
		hadFinishTargetNum = 0;
		LockState = -100;
	}

	public void InitFormServerData(PlayerServerLevelData serverData)
	{
		if (serverData != null)
		{
			levelId = serverData.levelId;
			MaxProgress = serverData.maxProgress;
			MaxDiamondNum = serverData.maxDiamondNum;
			MaxCrownNum = serverData.maxCrownNum;
			LockState = serverData.lockState;
			BuyOutRebirth = serverData.buyOutRebirth;
			IsPerfect = serverData.isPerfect;
			FinishLevelCount = serverData.finishLevelCount;
			PlayLevelCount = serverData.playLevelCount;
			int num = Mathf.Min(serverData.crownCollect.Count, m_crownCollect.Count);
			for (int i = 0; i < num; i++)
			{
				m_crownCollect[i] = serverData.crownCollect[i];
			}
			num = Mathf.Min(serverData.diamondsCollect.Count, m_diamondsCollect.Count);
			for (int j = 0; j < num; j++)
			{
				m_diamondsCollect[j] = serverData.diamondsCollect[j];
			}
		}
	}

	public void CheckUnLockByTime()
	{
		Levels_levelTable levels_levelTable = Mod.DataTable.Get<Levels_levelTable>()[levelId];
		if (levels_levelTable == null)
		{
			return;
		}
		string[] array = levels_levelTable.UnLockIds.Split('|');
		LevelUnLock_table levelUnLock_table = null;
		for (int i = 0; i < array.Length; i++)
		{
			levelUnLock_table = Mod.DataTable.Get<LevelUnLock_table>()[int.Parse(array[i])];
			if (levelUnLock_table != null && levelUnLock_table.LockType == 3)
			{
				int num = levelUnLock_table.UnLockNum * 60 * 1000;
				if (PlayerDataModule.Instance.ServerTime > initTime + num)
				{
					LockState = MonoSingleton<GameTools>.Instacne.GenerateUnLockState(LevelLockType.TIME);
				}
			}
		}
	}

	public long LeftUnlockTime()
	{
		string[] array = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).GetLevelTableById(levelId).UnLockIds.Split('|');
		LevelUnLock_table levelUnLock_table = null;
		for (int i = 0; i < array.Length; i++)
		{
			levelUnLock_table = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).GetLevelUnLockById(int.Parse(array[i]));
			if (levelUnLock_table != null && levelUnLock_table.LockType == 3)
			{
				int num = levelUnLock_table.UnLockNum * 60 * 1000;
				return initTime + num - PlayerDataModule.Instance.ServerTime;
			}
		}
		return 0L;
	}

	public void SetTicketNum(int num)
	{
		if (num < 0)
		{
			num = 0;
		}
		tictetsNum = num;
	}

	public int GetTicketNum()
	{
		return tictetsNum;
	}

	public bool IsNewUnLock()
	{
		if (LockState < 0 && MaxProgress <= 0)
		{
			return true;
		}
		return false;
	}

	public bool IsCanFreeEnterLevel()
	{
		return FreeEnterLevelTimeStamp > PlayerDataModule.Instance.ServerTime;
	}

	public bool IsCrownCollectFinished()
	{
		int num = 1;
		for (int i = 0; i < m_crownCollect.Count; i++)
		{
			num &= m_crownCollect[i];
		}
		return num == 1;
	}

	public int GetCollectedCrownCount()
	{
		int num = 0;
		foreach (int item in m_crownCollect)
		{
			if (item == 1)
			{
				num++;
			}
		}
		return num;
	}

	public int GetCollectedDiamoundCount()
	{
		int num = 0;
		foreach (int item in m_diamondsCollect)
		{
			if (item == 1)
			{
				num++;
			}
		}
		return num;
	}

	public bool IsDiamondsCollectFinished()
	{
		int num = 1;
		for (int i = 0; i < m_diamondsCollect.Count; i++)
		{
			num &= m_diamondsCollect[i];
		}
		return num == 1;
	}

	public void ResetCrownCollected()
	{
		for (int i = 0; i < m_crownCollect.Count; i++)
		{
			m_crownCollect[i] = 0;
		}
	}

	public void ResetDiamondsCollected()
	{
		for (int i = 0; i < m_diamondsCollect.Count; i++)
		{
			m_diamondsCollect[i] = 0;
		}
	}

	public void SaveCrownCollect(int[] crownCollect)
	{
		if (crownCollect.Length == m_crownCollect.Count)
		{
			for (int i = 0; i < crownCollect.Length; i++)
			{
				m_crownCollect[i] = crownCollect[i];
			}
		}
	}

	public void SaveDiamondsCollect(int[] diamonsCollect)
	{
		if (diamonsCollect.Length == m_diamondsCollect.Count)
		{
			for (int i = 0; i < diamonsCollect.Length; i++)
			{
				m_diamondsCollect[i] = diamonsCollect[i];
			}
		}
	}

	public void InitFromLocal()
	{
		m_CrymaxProgress = maxProgress;
		m_CrymaxCrownNum = maxCrownNum;
		m_CrymaxDiamondNum = maxDiamondNum;
		m_CryxlockState = lockState;
		m_CrybuyOutRebirth = buyOutRebirth;
		Levels_levelTable levels_levelTable = Mod.DataTable.Get<Levels_levelTable>()[levelId];
		if (levels_levelTable != null)
		{
			for (int i = Mathf.Min(levels_levelTable.Diamonds, m_diamondsCollect.Count); i < levels_levelTable.Diamonds; i++)
			{
				m_diamondsCollect.Add(0);
			}
			for (int j = Mathf.Min(levels_levelTable.Crowns, m_crownCollect.Count); j < levels_levelTable.Crowns; j++)
			{
				m_crownCollect.Add(0);
			}
		}
	}

	public void SaveToLocal()
	{
		maxProgress = m_CrymaxProgress;
		maxCrownNum = m_CrymaxCrownNum;
		maxDiamondNum = m_CrymaxDiamondNum;
		lockState = m_CryxlockState;
		buyOutRebirth = m_CrybuyOutRebirth;
	}
}
