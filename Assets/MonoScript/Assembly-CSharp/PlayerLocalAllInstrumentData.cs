using System;
using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;

[Serializable]
public class PlayerLocalAllInstrumentData
{
	public List<PlayerLocalInstrumentData> m_InstrumentList = new List<PlayerLocalInstrumentData>();

	public void InitFromLocal()
	{
		string config = EncodeConfig.getConfig(PlayerLocalDatakey.PLAYERLOCALINSTRUMENTDATA);
		if (config.Length > 0)
		{
			JsonUtility.FromJsonOverwrite(config, this);
		}
		Instrument_table[] instrumentTableArr = Mod.DataTable.Get<Instrument_table>().Records;
		PlayerLocalInstrumentData playerLocalInstrumentData = null;
		bool flag = false;
		int i;
		for (i = 0; i < instrumentTableArr.Length; i++)
		{
			playerLocalInstrumentData = m_InstrumentList.Find((PlayerLocalInstrumentData x) => x.m_Id == instrumentTableArr[i].Id);
			if (playerLocalInstrumentData == null)
			{
				playerLocalInstrumentData = new PlayerLocalInstrumentData();
				playerLocalInstrumentData.InitFromMetaTableData(instrumentTableArr[i]);
				m_InstrumentList.Add(playerLocalInstrumentData);
				flag = true;
			}
		}
		for (int j = 0; j < m_InstrumentList.Count; j++)
		{
			m_InstrumentList[j].InitFromLocal();
		}
		if (flag)
		{
			SaveToLocal();
		}
	}

	private void SaveToLocal()
	{
		string values = JsonUtility.ToJson(this);
		EncodeConfig.setConfig(PlayerLocalDatakey.PLAYERLOCALINSTRUMENTDATA, values);
	}

	public int CompareWithServerData(PlayerAllServerData server)
	{
		if (server == null || server.instrumentDataList == null)
		{
			return 1;
		}
		List<PlayerLocalInstrumentData> allUnLockInstuments = GetAllUnLockInstuments();
		if (allUnLockInstuments.Count > server.instrumentDataList.Count)
		{
			return 1;
		}
		if (allUnLockInstuments.Count == server.instrumentDataList.Count)
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < allUnLockInstuments.Count; i++)
			{
				num += (int)allUnLockInstuments[i].Level;
				num2 += server.instrumentDataList[i].level;
			}
			return num - num2;
		}
		return -1;
	}

	public void ResetFromServerData(PlayerAllServerData serverData)
	{
		if (serverData == null || serverData.propertyData == null)
		{
			return;
		}
		PlayerLocalInstrumentData playerLocalInstrumentData = null;
		int i;
		for (i = 0; i < serverData.instrumentDataList.Count; i++)
		{
			playerLocalInstrumentData = m_InstrumentList.Find((PlayerLocalInstrumentData x) => x.m_Id == serverData.instrumentDataList[i].id);
			if (playerLocalInstrumentData != null)
			{
				playerLocalInstrumentData.InitFromServerData(serverData.instrumentDataList[i]);
			}
		}
	}

	public PlayerLocalInstrumentData GetInstrumentDataById(int id)
	{
		return m_InstrumentList.Find((PlayerLocalInstrumentData x) => x.m_Id == id);
	}

	public bool PutOnInstrument(int id, int slotIndex)
	{
		PlayerLocalInstrumentData instrumentDataById = GetInstrumentDataById(id);
		if (instrumentDataById.IsBaton())
		{
			return false;
		}
		if (instrumentDataById != null)
		{
			instrumentDataById.SlotIndex = slotIndex;
			instrumentDataById.PlayState = 1;
			instrumentDataById.SaveToLocal();
			SaveToLocal();
			return true;
		}
		return false;
	}

	public bool PutOffInstrument(int id)
	{
		PlayerLocalInstrumentData instrumentDataById = GetInstrumentDataById(id);
		if (instrumentDataById.IsBaton())
		{
			return false;
		}
		if (instrumentDataById != null)
		{
			instrumentDataById.SlotIndex = -1;
			instrumentDataById.PlayState = -1;
			instrumentDataById.SaveToLocal();
			SaveToLocal();
			return true;
		}
		return false;
	}

	public List<PlayerLocalInstrumentData> GetAllUnLockInstuments()
	{
		new List<PlayerLocalInstrumentData>();
		return m_InstrumentList.FindAll((PlayerLocalInstrumentData x) => (int)x.LockState < 0);
	}

	public void ResetAllUnlockInstrumentLevel(int level = 1)
	{
		List<PlayerLocalInstrumentData> allUnLockInstuments = GetAllUnLockInstuments();
		for (int i = 0; i < allUnLockInstuments.Count; i++)
		{
			allUnLockInstuments[i].Level = level;
			allUnLockInstuments[i].SaveToLocal();
		}
		SaveToLocal();
	}

	public UpInstrumentResultState UpLevel(int id, int levelCount = 1)
	{
		PlayerLocalInstrumentData instrumentDataById = GetInstrumentDataById(id);
		int num = (int)instrumentDataById.Level + levelCount;
		if (num > PlayerDataModule.Instance.InstrumentCanUpMaxLevel())
		{
			return UpInstrumentResultState.LEVEL_REACH_MAX;
		}
		double num2 = 0.0;
		for (int i = instrumentDataById.Level; i < num; i++)
		{
			num2 += instrumentDataById.GetUpLevelConsumeCount(PlayerDataModule.Instance.GetPlayerStarLevel(), i);
		}
		if (PlayerDataModule.Instance.GetPlayGoodsNum(3) >= num2)
		{
			instrumentDataById.Level = (int)instrumentDataById.Level + levelCount;
			instrumentDataById.m_HideLevel += levelCount;
			instrumentDataById.SaveToLocal();
			SaveToLocal();
			PlayerDataModule.Instance.ChangePlayerGoodsNum(3, num2 * -1.0);
			return UpInstrumentResultState.SUCCESS;
		}
		return UpInstrumentResultState.GOOD_NOT_ENOUGH;
	}

	public UpInstrumentResultState ForceUpLevel(int id, int levelCount = 1)
	{
		PlayerLocalInstrumentData instrumentDataById = GetInstrumentDataById(id);
		if ((int)instrumentDataById.Level + levelCount > PlayerDataModule.Instance.InstrumentCanUpMaxLevel())
		{
			return UpInstrumentResultState.LEVEL_REACH_MAX;
		}
		instrumentDataById.Level = (int)instrumentDataById.Level + levelCount;
		instrumentDataById.m_HideLevel += levelCount;
		instrumentDataById.SaveToLocal();
		SaveToLocal();
		return UpInstrumentResultState.SUCCESS;
	}

	public PlayInstrumentResultState Play(int id, out double productCount)
	{
		productCount = 0.0;
		PlayerLocalInstrumentData instrumentDataById = GetInstrumentDataById(id);
		int num = ((!instrumentDataById.Play()) ? 1 : 0);
		if (num == 0)
		{
			instrumentDataById.m_PlayCount++;
			Instrument_table instrument_table = Mod.DataTable.Get<Instrument_table>()[instrumentDataById.m_Id];
			int unLockInstrmentCount = PlayerDataModule.Instance.GetUnLockInstrmentCount();
			double num2 = 0.0;
			if (unLockInstrmentCount > 0)
			{
				num2 = PlayerDataModule.Instance.GetCurrentProductReputaionSpeed() / (double)unLockInstrmentCount;
			}
			productCount = num2;
			PlayerDataModule.Instance.ChangePlayerGoodsNum(instrument_table.ProductGoods, num2, AssertChangeType.INSTRUMENT_PLAY, NetWorkSynType.NORMAL);
			instrumentDataById.SaveToLocal();
			SaveToLocal();
		}
		return (PlayInstrumentResultState)num;
	}

	public void UnlockInstrument(int id)
	{
		PlayerLocalInstrumentData instrumentDataById = GetInstrumentDataById(id);
		if (instrumentDataById != null)
		{
			instrumentDataById.LockState = -1;
			if ((int)instrumentDataById.SlotIndex < GameCommon.MAX_SLOT_NUM)
			{
				instrumentDataById.PlayState = -1;
			}
			instrumentDataById.SaveToLocal();
		}
		SaveToLocal();
	}

	public void LockInstrument(int id)
	{
		PlayerLocalInstrumentData instrumentDataById = GetInstrumentDataById(id);
		if (instrumentDataById != null)
		{
			instrumentDataById.LockState = 1;
			instrumentDataById.PlayState = -1;
			instrumentDataById.SaveToLocal();
		}
		SaveToLocal();
	}
}
