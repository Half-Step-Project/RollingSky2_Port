using System;
using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;

[Serializable]
public class PlayerLocalTargetsData
{
	public List<int> currentCanFinishTargets = new List<int>();

	public int hadFinishTargetNum;

	public bool isOpened;

	public bool isFinishedTutorial;

	[NonSerialized]
	private List<int> lastFinishTargets = new List<int>();

	[NonSerialized]
	private Dictionary<int, int> lastFinishTargetsAward = new Dictionary<int, int>();

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

	private void SetCurrentCanFinishTargets(List<int> goalList)
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
			SaveToLocal();
		}
	}

	public void SetHadFinishTargetNum(int num)
	{
		if (hadFinishTargetNum != num)
		{
			hadFinishTargetNum = num;
			SaveToLocal();
		}
	}

	public void ResetTarget()
	{
		currentCanFinishTargets.Clear();
		InitDataInFirst();
	}

	public void SetLastFinishTargetAward(Dictionary<int, int> award)
	{
		lastFinishTargetsAward = new Dictionary<int, int>(award);
	}

	public Dictionary<int, int> GetLastFinishTargetAward()
	{
		if (lastFinishTargetsAward == null)
		{
			lastFinishTargetsAward = new Dictionary<int, int>();
		}
		return lastFinishTargetsAward;
	}

	private void InitFromLocal()
	{
		string config = EncodeConfig.getConfig(PlayerLocalDatakey.PLAYERLEVELTARGETSDATA);
		if (config.Length > 0)
		{
			JsonUtility.FromJsonOverwrite(config, this);
			return;
		}
		InitDataInFirst();
		SaveToLocal();
	}

	public void Init(bool isOpen)
	{
		InitFromLocal();
		if (!isOpened)
		{
			isOpened = isOpen;
			if (isOpened)
			{
				SaveToLocal();
			}
		}
	}

	private void InitDataInFirst()
	{
		LevelTarget_levelTargetTable[] records = Mod.DataTable.Get<LevelTarget_levelTargetTable>().Records;
		int levelTargetSelectedNum = GameCommon.levelTargetSelectedNum;
		new List<int>();
		if (levelTargetSelectedNum < records.Length)
		{
			int[] random = MonoSingleton<GameTools>.Instacne.GetRandom(records.Length, levelTargetSelectedNum);
			for (int i = 0; i < random.Length; i++)
			{
				currentCanFinishTargets.Add(records[random[i]].Id);
			}
		}
		else
		{
			for (int j = 0; j < records.Length; j++)
			{
				currentCanFinishTargets.Add(records[j].Id);
			}
		}
		hadFinishTargetNum = 0;
	}

	public void OpenLevelTarget()
	{
		isOpened = true;
		SaveToLocal();
	}

	public bool IsOpened()
	{
		return isOpened;
	}

	public bool IsFinishTutorial()
	{
		return isFinishedTutorial;
	}

	public void SetFinishTutorial(bool isFinish)
	{
		if (isFinishedTutorial != isFinish)
		{
			isFinishedTutorial = isFinish;
			SaveToLocal();
		}
	}

	public void SaveToLocal()
	{
		string values = JsonUtility.ToJson(this);
		EncodeConfig.setConfig(PlayerLocalDatakey.PLAYERLEVELTARGETSDATA, values);
	}

	public void RefreshTargets(bool isAll = true, int specialId = -1)
	{
		int count = currentCanFinishTargets.Count;
		List<int> list = new List<int>();
		LevelTarget_levelTargetTable[] records = Mod.DataTable.Get<LevelTarget_levelTargetTable>().Records;
		int num = -1;
		if (count >= records.Length)
		{
			return;
		}
		for (int i = 0; i < records.Length; i++)
		{
			num = records[i].Id;
			if (!currentCanFinishTargets.Contains(num))
			{
				list.Add(num);
			}
		}
		if (isAll)
		{
			int[] random = MonoSingleton<GameTools>.Instacne.GetRandom(list.Count, GameCommon.levelTargetSelectedNum);
			List<int> list2 = new List<int>();
			for (int j = 0; j < random.Length; j++)
			{
				list2.Add(list[random[j]]);
			}
			SetCurrentCanFinishTargets(list2);
			return;
		}
		int[] random2 = MonoSingleton<GameTools>.Instacne.GetRandom(list.Count, 1);
		int value = list[random2[0]];
		int index = currentCanFinishTargets.FindIndex((int x) => x == specialId);
		if (GameCommon.levelTargetMaxCanFinNum - hadFinishTargetNum < GameCommon.levelTargetSelectedNum)
		{
			currentCanFinishTargets[index] = currentCanFinishTargets[currentCanFinishTargets.Count - 1];
			currentCanFinishTargets[currentCanFinishTargets.Count - 1] = value;
		}
		else
		{
			currentCanFinishTargets[index] = value;
		}
	}
}
