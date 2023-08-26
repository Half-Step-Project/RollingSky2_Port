using System;
using System.Collections.Generic;
using System.Reflection;
using Foundation;
using RS2;
using UnityEngine;

[Serializable]
public class PlayerLocalInstrumentData
{
	public int m_Id;

	public int m_Level;

	public int m_HideLevel;

	public int m_SlotIndex;

	public int m_PlayState = -1;

	public int m_lockState = -1;

	public int m_PlayCount;

	private float m_LastPlayTime = -1f;

	private int m_IconId = -1;

	private Assembly m_currentAssembly;

	private string m_typeName = "";

	private Type m_growTableType;

	private Type m_dataTableModType;

	private MethodInfo m_dataTableGetMethod;

	private object m_growTableRecod;

	private MethodInfo m_dataRecordGetMethod;

	private object m_growTableObj;

	private string m_productionMethodName = "get_StarLevel_production";

	private string m_consumeMethodName = "get_StarLevel_consume";

	private CryVInt32 m_EncrylockState = -1;

	private CryVInt32 m_EncryLevel;

	private CryVInt32 m_EncrySlotIndex;

	private CryVInt32 m_EncryPlayState;

	public int IconId
	{
		get
		{
			if (m_IconId < 0)
			{
				Instrument_table instrument_table = Mod.DataTable.Get<Instrument_table>()[m_Id];
				if (instrument_table != null)
				{
					m_IconId = instrument_table.IconId;
				}
			}
			return m_IconId;
		}
	}

	public CryVInt32 LockState
	{
		get
		{
			return m_EncrylockState;
		}
		set
		{
			m_EncrylockState = value;
		}
	}

	public CryVInt32 Level
	{
		get
		{
			return m_EncryLevel;
		}
		set
		{
			m_EncryLevel = value;
		}
	}

	public CryVInt32 SlotIndex
	{
		get
		{
			return m_EncrySlotIndex;
		}
		set
		{
			m_EncrySlotIndex = value;
		}
	}

	public CryVInt32 PlayState
	{
		get
		{
			return m_EncryPlayState;
		}
		set
		{
			m_EncryPlayState = value;
		}
	}

	public void InitFromLocal()
	{
		m_EncryLevel = m_Level;
		m_EncrySlotIndex = m_SlotIndex;
		m_EncryPlayState = m_PlayState;
		m_EncrylockState = m_lockState;
		InitGrowTableReflection(m_Id);
	}

	public int CompareWithServer(PlayerServerInstrumentData serverData)
	{
		if (m_lockState == -1 && serverData.lockState != -1)
		{
			return 1;
		}
		if (m_Level > serverData.level)
		{
			return 1;
		}
		return 0;
	}

	public void SaveToLocal()
	{
		m_Level = m_EncryLevel;
		m_SlotIndex = m_EncrySlotIndex;
		m_PlayState = m_EncryPlayState;
		m_lockState = m_EncrylockState;
	}

	private void InitGrowTableReflection(int id)
	{
		m_currentAssembly = Assembly.GetExecutingAssembly();
		m_typeName = string.Format("RS2.InstrumentGrow_{0}", id);
		m_growTableType = m_currentAssembly.GetType(m_typeName);
		m_dataTableModType = typeof(DataTableMod);
		m_dataTableGetMethod = m_dataTableModType.GetMethod("Get");
		m_dataTableGetMethod = m_dataTableGetMethod.MakeGenericMethod(m_growTableType);
		m_growTableRecod = m_dataTableGetMethod.Invoke(Mod.DataTable, null);
		m_dataRecordGetMethod = m_growTableRecod.GetType().GetMethod("Get", new Type[1] { typeof(int) });
	}

	public void InitFromServerData(PlayerServerInstrumentData serverData)
	{
		m_Id = serverData.id;
		m_EncryLevel = serverData.level;
		m_EncrySlotIndex = serverData.slotIndex;
		m_EncryPlayState = serverData.playState;
		m_HideLevel = serverData.hideLevel;
		m_EncrylockState = serverData.lockState;
		m_PlayCount = serverData.playCount;
		SaveToLocal();
	}

	public void InitFromMetaTableData(Instrument_table table)
	{
		m_Id = table.Id;
		m_Level = 1;
		m_HideLevel = 1;
		int unLockPlayerLevel = table.UnLockPlayerLevel;
		int unLockPlayerStarLevel = table.UnLockPlayerStarLevel;
		if (PlayerDataModule.Instance.GetPlayerStarLevel() >= unLockPlayerStarLevel && m_Level >= unLockPlayerLevel)
		{
			m_lockState = -1;
		}
		else
		{
			m_lockState = 1;
		}
		m_SlotIndex = table.DefaultSlot;
		if (m_lockState == -1)
		{
			m_PlayState = 1;
		}
		else
		{
			m_PlayState = -1;
		}
		m_IconId = table.IconId;
	}

	public int GetProductGoodsId()
	{
		Instrument_table instrument_table = Mod.DataTable.Get<Instrument_table>()[m_Id];
		if (instrument_table != null)
		{
			return instrument_table.ProductGoods;
		}
		if (m_Id == 1)
		{
			return 3;
		}
		return 32;
	}

	public int UnlockStar()
	{
		Instrument_table instrument_table = Mod.DataTable.Get<Instrument_table>()[m_Id];
		if (instrument_table != null)
		{
			return instrument_table.UnLockPlayerStarLevel;
		}
		return -1;
	}

	public int UnlockLevel()
	{
		Instrument_table instrument_table = Mod.DataTable.Get<Instrument_table>()[m_Id];
		if (instrument_table != null)
		{
			return instrument_table.UnLockPlayerLevel;
		}
		return -1;
	}

	public bool IsBaton()
	{
		return m_Id == 1;
	}

	public List<List<int>> SoundId()
	{
		Instrument_table instrument_table = Mod.DataTable.Get<Instrument_table>()[m_Id];
		List<List<int>> list = new List<List<int>>();
		if (instrument_table != null)
		{
			string[] array = instrument_table.SoundID.Split('&');
			int result = -1;
			for (int i = 0; i < array.Length; i++)
			{
				if (string.IsNullOrEmpty(array[i]))
				{
					continue;
				}
				List<int> list2 = new List<int>();
				string[] array2 = array[i].Split('|');
				for (int j = 0; j < array2.Length; j++)
				{
					if (!string.IsNullOrEmpty(array2[j]) && int.TryParse(array2[j], out result))
					{
						list2.Add(result);
					}
				}
				list.Add(list2);
			}
		}
		return list;
	}

	public string Name()
	{
		Instrument_table instrument_table = Mod.DataTable.Get<Instrument_table>()[m_Id];
		if (instrument_table != null)
		{
			return Mod.Localization.GetInfoById(instrument_table.Name);
		}
		return string.Empty;
	}

	public string AssetName()
	{
		Instrument_table instrument_table = Mod.DataTable.Get<Instrument_table>()[m_Id];
		if (instrument_table != null)
		{
			return instrument_table.AssetName;
		}
		return string.Empty;
	}

	public double ComputerProductNumByTime(int second, int starLevel, int level, float factor = 1f)
	{
		double productReputationBaseNum = GetProductReputationBaseNum(starLevel, level);
		float num = PlayerDataModule.Instance.InstrumentProductAdditonalPercent();
		productReputationBaseNum *= 1.0 + (double)num;
		productReputationBaseNum = (double)second * productReputationBaseNum * (double)factor;
		if (PlayerDataModule.Instance.IsProductSpeedUpGoing())
		{
			productReputationBaseNum *= (double)GameCommon.instrumentAdProductFactor;
		}
		return productReputationBaseNum;
	}

	public int GetProductGoldPercentAdd(int starLevel, int level)
	{
		int result = 0;
		if (m_dataRecordGetMethod != null)
		{
			m_growTableObj = m_dataRecordGetMethod.Invoke(m_growTableRecod, new object[1] { level });
			string text = m_growTableObj.GetType().GetMethod(m_productionMethodName).Invoke(m_growTableObj, null) as string;
			if (!string.IsNullOrEmpty(text))
			{
				List<int> list = MonoSingleton<GameTools>.Instacne.StringToIntList(text);
				if (list.Count > starLevel)
				{
					result = list[starLevel];
				}
			}
		}
		return result;
	}

	public double GetProductReputationBaseNum(int starLevel, int level)
	{
		double result = 0.0;
		if (m_dataRecordGetMethod != null)
		{
			m_growTableObj = m_dataRecordGetMethod.Invoke(m_growTableRecod, new object[1] { level });
			string text = m_growTableObj.GetType().GetMethod(m_productionMethodName).Invoke(m_growTableObj, null) as string;
			if (!string.IsNullOrEmpty(text))
			{
				List<double> list = MonoSingleton<GameTools>.Instacne.StringToDoubleList(text);
				if (list.Count > starLevel)
				{
					result = list[starLevel];
				}
			}
		}
		return result;
	}

	public double GetUpLevelConsumeCount(int starLevel, int level)
	{
		double result = 0.0;
		if (m_dataRecordGetMethod != null)
		{
			m_growTableObj = m_dataRecordGetMethod.Invoke(m_growTableRecod, new object[1] { level });
			string text = m_growTableObj.GetType().GetMethod(m_consumeMethodName).Invoke(m_growTableObj, null) as string;
			if (!string.IsNullOrEmpty(text))
			{
				List<long> list = MonoSingleton<GameTools>.Instacne.StringToLongList(text);
				if (list.Count > starLevel)
				{
					result = list[starLevel];
				}
			}
		}
		return result;
	}

	public bool Play()
	{
		if (Time.realtimeSinceStartup - m_LastPlayTime >= 0.1f)
		{
			m_LastPlayTime = Time.realtimeSinceStartup;
			return true;
		}
		return false;
	}

	public int GetBatonOffLineProductBase(int starLevel, int level)
	{
		int result = 0;
		if (IsBaton())
		{
			InstrumentBatonOffLineBaseProduct_table instrumentBatonOffLineBaseProduct_table = Mod.DataTable.Get<InstrumentBatonOffLineBaseProduct_table>()[m_Level];
			if (instrumentBatonOffLineBaseProduct_table != null)
			{
				List<int> list = MonoSingleton<GameTools>.Instacne.StringToIntList(instrumentBatonOffLineBaseProduct_table.OfflineBaseNum);
				if (list.Count > starLevel)
				{
					result = list[starLevel];
				}
			}
		}
		return result;
	}

	public int GetUnLockNeedStarLevel()
	{
		Instrument_table instrument_table = Mod.DataTable.Get<Instrument_table>()[m_Id];
		if (instrument_table != null)
		{
			return instrument_table.UnLockPlayerStarLevel;
		}
		return -1;
	}

	public int GetUnLockNeedLevel()
	{
		Instrument_table instrument_table = Mod.DataTable.Get<Instrument_table>()[m_Id];
		if (instrument_table != null)
		{
			return instrument_table.UnLockPlayerLevel;
		}
		return -1;
	}

	public int GetAdDirectUpLevel()
	{
		Instrument_table instrument_table = Mod.DataTable.Get<Instrument_table>()[m_Id];
		if (instrument_table != null)
		{
			return instrument_table.AdDirectUpLevel;
		}
		return 0;
	}
}
