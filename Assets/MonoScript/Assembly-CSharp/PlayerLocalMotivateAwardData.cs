using System;
using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;

[Serializable]
public class PlayerLocalMotivateAwardData
{
	[Serializable]
	public class SubsetCount
	{
		public int subsetId;

		public int count;

		public SubsetCount(int subsetId)
		{
			this.subsetId = subsetId;
			count = 0;
		}
	}

	public class ShowAward
	{
		public int id;

		public int minCount;

		public int maxCount;

		public ShowAward(int id, int minCount, int maxCount)
		{
			this.id = id;
			this.minCount = minCount;
			this.maxCount = maxCount;
		}
	}

	public List<SubsetCount> subsetCounts = new List<SubsetCount>();

	public List<int> certianAwardIds = new List<int>();

	private List<MotivateReward_awardSubset> selectSubsets;

	private void DebugData()
	{
		foreach (SubsetCount subsetCount in subsetCounts)
		{
			Debug.LogWarning("id " + subsetCount.subsetId + " count " + subsetCount.count);
		}
	}

	public void AddSubsetCount(int subsetId)
	{
		SubsetCount subsetCount = null;
		foreach (SubsetCount subsetCount2 in subsetCounts)
		{
			if (subsetCount2.subsetId == subsetId)
			{
				subsetCount = subsetCount2;
			}
		}
		if (subsetCount == null)
		{
			subsetCount = new SubsetCount(subsetId);
			subsetCounts.Add(subsetCount);
		}
		subsetCount.count++;
		Save();
	}

	public void ResetSubsetCount(int subsetId)
	{
		foreach (SubsetCount subsetCount in subsetCounts)
		{
			if (subsetCount.subsetId == subsetId)
			{
				subsetCounts.Remove(subsetCount);
				break;
			}
		}
		Save();
	}

	public int GetSubsetCount(int subsetId)
	{
		foreach (SubsetCount subsetCount in subsetCounts)
		{
			if (subsetCount.subsetId == subsetId)
			{
				return subsetCount.count;
			}
		}
		return 0;
	}

	public void Init()
	{
		string config = EncodeConfig.getConfig(PlayerLocalDatakey.PLAYERLOCALMOTIVATEAWARDDATA);
		if (config.Length > 0)
		{
			JsonUtility.FromJsonOverwrite(config, this);
		}
	}

	public void Save()
	{
		string values = JsonUtility.ToJson(this);
		EncodeConfig.setConfig(PlayerLocalDatakey.PLAYERLOCALMOTIVATEAWARDDATA, values);
	}

	public void AddCertainAward(int id)
	{
		certianAwardIds.Add(id);
		Save();
	}

	private MotivateReward_awardSubset GetNextSubset(int subsetId)
	{
		MotivateReward_awardSubset motivateReward_awardSubset = Mod.DataTable.Get<MotivateReward_awardSubset>()[subsetId];
		if (motivateReward_awardSubset.NextId == 0)
		{
			return motivateReward_awardSubset;
		}
		int subsetCount = GetSubsetCount(motivateReward_awardSubset.Id);
		int num = UnityEngine.Random.Range(motivateReward_awardSubset.NextMin, motivateReward_awardSubset.NextMax + 1);
		if (subsetCount >= num)
		{
			ResetSubsetCount(motivateReward_awardSubset.Id);
			return GetNextSubset(motivateReward_awardSubset.NextId);
		}
		return motivateReward_awardSubset;
	}

	private ResultGetGoods.GoodsData GetSubsetAward(MotivateReward_awardSubset subset)
	{
		MotivateReward_award[] records = Mod.DataTable.Get<MotivateReward_award>().Records;
		int num = 0;
		List<MotivateReward_award> list = new List<MotivateReward_award>();
		MotivateReward_award[] array = records;
		foreach (MotivateReward_award motivateReward_award in array)
		{
			if (motivateReward_award.SubsetId == subset.Id)
			{
				list.Add(motivateReward_award);
				num += motivateReward_award.Weight;
			}
		}
		int num2 = UnityEngine.Random.Range(1, num + 1);
		int num3 = 0;
		ResultGetGoods.GoodsData result = null;
		foreach (MotivateReward_award item in list)
		{
			num3 += item.Weight;
			if (num3 >= num2)
			{
				if (item.GoodsId != 0)
				{
					int num4 = UnityEngine.Random.Range(item.Min, item.Max + 1);
					int num5 = (int)PlayerDataModule.Instance.GetPlayGoodsNum(item.GoodsId);
					return new ResultGetGoods.GoodsData(item.GoodsId, num5, num4);
				}
				return result;
			}
		}
		return result;
	}

	public ResultGetGoods.ShowData GetAwards()
	{
		ResultGetGoods.ShowData showData = new ResultGetGoods.ShowData();
		int levelAwardCount = PlayerDataModule.Instance.GetLevelAwardCount();
		MotivateReward_awardSuperset motivateReward_awardSuperset = null;
		MotivateReward_awardSuperset[] records = Mod.DataTable.Get<MotivateReward_awardSuperset>().Records;
		foreach (MotivateReward_awardSuperset motivateReward_awardSuperset2 in records)
		{
			if (motivateReward_awardSuperset2.LevelCount == levelAwardCount)
			{
				motivateReward_awardSuperset = motivateReward_awardSuperset2;
			}
		}
		if (motivateReward_awardSuperset == null)
		{
			motivateReward_awardSuperset = Mod.DataTable.Get<MotivateReward_awardSuperset>().Max;
		}
		bool flag = PlayerDataModule.Instance.PlayerLocalLevelSeriesData.IsRare();
		selectSubsets = new List<MotivateReward_awardSubset>();
		MotivateReward_awardSubset[] records2 = Mod.DataTable.Get<MotivateReward_awardSubset>().Records;
		foreach (MotivateReward_awardSubset motivateReward_awardSubset in records2)
		{
			if ((motivateReward_awardSubset.Quality != 2 || flag) && motivateReward_awardSubset.SupersetId == motivateReward_awardSuperset.SetId)
			{
				selectSubsets.Add(GetNextSubset(motivateReward_awardSubset.Id));
			}
		}
		foreach (MotivateReward_awardSubset selectSubset in selectSubsets)
		{
			ResultGetGoods.GoodsData subsetAward = GetSubsetAward(selectSubset);
			AddSubsetCount(selectSubset.Id);
			if (subsetAward != null)
			{
				showData.AddGoodsData(subsetAward);
			}
		}
		return showData;
	}
}
