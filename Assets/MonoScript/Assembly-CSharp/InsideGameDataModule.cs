using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;

public class InsideGameDataModule : IDataModule
{
	private RebirthBoxData m_currentRebirthBoxData;

	private SingleOriginRebirthForRowData m_currentOriginRebirth;

	public bool m_isOriginRebirth;

	public float m_rebirthProgress;

	private bool m_isTryLevel;

	private bool m_isNewRecord;

	private int m_preMaxProgress;

	private int m_preDiamonds;

	private int m_preCrowns;

	private int m_preStarLevel;

	private bool guideLine;

	public Dictionary<int, int> triggeredBuyOutRebirth = new Dictionary<int, int>();

	private RebirthType m_rebirthType;

	public int m_theBoardShowRebirthAdCount;

	public bool m_isAdRebirth;

	public int m_theBoardRebirthCount;

	public int m_theLevelPlayCount;

	public bool needRemoveTempGoods;

	public int m_theLevelShowOriginRebirthDiscountCount;

	private int m_LevelActualLength;

	private int m_ProgressPercentage;

	private int m_GainedDiamonds;

	private int m_GainedCrowns;

	private int m_GainedDiamondFragments;

	private int m_GainedCrownFragments;

	private int m_useRoleId = -1;

	public int m_usePetId = -1;

	public int m_useClothId = -1;

	private List<int> m_crownCollects = new List<int>();

	private List<int> m_diamondCollects = new List<int>();

	private bool m_IsCrownCollectsFinished;

	private bool m_IsDiamondCollectsFinished;

	private int m_currentCrownCollectCount;

	private bool m_isCollectsFinishedForStart;

	private int m_currentDiamondCollectCount;

	private Dictionary<int, DropData> m_dropDatas = new Dictionary<int, DropData>();

	public RebirthBoxData CurrentRebirthBoxData
	{
		get
		{
			return m_currentRebirthBoxData;
		}
	}

	public SingleOriginRebirthForRowData CurrentOriginRebirth
	{
		get
		{
			return m_currentOriginRebirth;
		}
	}

	public int DieCount { get; set; }

	public int PreStarLevel
	{
		get
		{
			return m_preStarLevel;
		}
		set
		{
			m_preStarLevel = value;
		}
	}

	public int PreCrowns
	{
		get
		{
			return m_preCrowns;
		}
		set
		{
			m_preCrowns = value;
		}
	}

	public int PreDiamonds
	{
		get
		{
			return m_preDiamonds;
		}
		set
		{
			m_preDiamonds = value;
		}
	}

	public int PreMaxProgress
	{
		get
		{
			return m_preMaxProgress;
		}
		set
		{
			m_preMaxProgress = value;
		}
	}

	public bool TryLevel
	{
		get
		{
			return m_isTryLevel;
		}
		set
		{
			m_isTryLevel = value;
		}
	}

	public bool GuideLine
	{
		get
		{
			return guideLine;
		}
		set
		{
			guideLine = value;
		}
	}

	public Vector3 BuyOutRebirthPoint { get; set; }

	public Vector3 FirstBuyOutRebirthPoint { get; set; }

	public int BuyOutRebirthIndex { get; set; }

	public int BuyOutRebirthProgress { get; set; }

	public int FirstBuyOutRebirthProgress { get; set; }

	public int FirstBuyOutRebirthUUID { get; set; }

	public int DieCountForShowPromote { get; set; }

	public int FurthestBuyOutRebirthProgress { get; set; }

	public Dictionary<int, int> TriggeredBuyOutRebirth
	{
		get
		{
			return triggeredBuyOutRebirth;
		}
	}

	public int BuyOutRebirthGainDiamounds { get; set; }

	public int BuyOutRebirthGainCrowns { get; set; }

	public int BuyOutRebirthGainDiamoundFragments { get; set; }

	public int BuyOutRebirthGainCrownFragments { get; set; }

	public int BuyOutRebirthPreDiamounds { get; set; }

	public int BuyOutRebirthPreCrowns { get; set; }

	public int ShowFreeBuffCount { get; set; }

	public int RestartCount { get; set; }

	public RebirthType CurrentRebirthType
	{
		get
		{
			return m_rebirthType;
		}
	}

	public int m_maxShowRebirthAdByTheBoardCount
	{
		get
		{
			return GameCommon.rebirthMaxAdNum;
		}
	}

	private int m_maxTheBoardRebirthCount
	{
		get
		{
			return GameCommon.rebirthMaxNum;
		}
	}

	public int FreeRebirthCount
	{
		get
		{
			Goods_goodsTable goods_goodsTable = Mod.DataTable.Get<Goods_goodsTable>()[GameCommon.ORIGIN_REBIRTH_FREE];
			if (goods_goodsTable != null)
			{
				return goods_goodsTable.FunctionNum;
			}
			return 0;
		}
	}

	public bool HasUseRebirthItem { get; set; }

	public bool IsHaveUnlimitedRebirth
	{
		get
		{
			return PlayerDataModule.Instance.GetPlayGoodsNum(GameCommon.ORIGIN_REBIRTH_UNLIMITED) > 0.0;
		}
	}

	public bool IsUnlimitedRebirthLevel
	{
		get
		{
			GameDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule);
			bool num = dataModule.CurLevelId == GameCommon.FIRST_LEVEL;
			bool flag = PlayerDataModule.Instance.GetLevelMaxProgress(dataModule.CurLevelId) >= 100;
			if (num)
			{
				return !flag;
			}
			return false;
		}
	}

	public int LevelActualLength
	{
		get
		{
			return m_LevelActualLength;
		}
	}

	public int ProgressPercentage
	{
		get
		{
			return m_ProgressPercentage;
		}
		set
		{
			if (m_ProgressPercentage != value)
			{
				m_ProgressPercentage = value;
				if (m_ProgressPercentage <= 0)
				{
					m_ProgressPercentage = 0;
				}
				if (m_ProgressPercentage >= 100)
				{
					m_ProgressPercentage = 100;
				}
				OnProgress();
			}
		}
	}

	public int GainedDiamonds
	{
		get
		{
			return m_GainedDiamonds;
		}
		set
		{
			m_GainedDiamonds = value;
		}
	}

	public int GainedCrowns
	{
		get
		{
			return m_GainedCrowns;
		}
		set
		{
			m_GainedCrowns = value;
		}
	}

	public int GainedDiamondFragments
	{
		get
		{
			return m_GainedDiamondFragments;
		}
		set
		{
			m_GainedDiamondFragments = value;
		}
	}

	public int GainedCrownFragments
	{
		get
		{
			return m_GainedCrownFragments;
		}
		set
		{
			m_GainedCrownFragments = value;
		}
	}

	public int UseRoleId
	{
		get
		{
			return m_useRoleId;
		}
		set
		{
			m_useRoleId = value;
		}
	}

	public int UsePetId
	{
		get
		{
			return m_usePetId;
		}
		set
		{
			m_usePetId = value;
		}
	}

	public int UseClothId
	{
		get
		{
			return m_useClothId;
		}
		set
		{
			m_useClothId = value;
		}
	}

	public int CurrentCrownCollectCount
	{
		get
		{
			return m_currentCrownCollectCount;
		}
	}

	public int CurrentDiamondCollectCount
	{
		get
		{
			return m_currentDiamondCollectCount;
		}
	}

	public List<int> CrownCollects
	{
		get
		{
			return m_crownCollects;
		}
	}

	public List<int> DiamondCollects
	{
		get
		{
			return m_diamondCollects;
		}
	}

	private Dictionary<int, DropData> DropDatas
	{
		get
		{
			return m_dropDatas;
		}
	}

	public void SetCurrentRebirthBoxData(RebirthBoxData rebirthBoxData)
	{
		m_currentRebirthBoxData = rebirthBoxData;
		if (rebirthBoxData == null)
		{
			m_isOriginRebirth = false;
			m_rebirthProgress = 0f;
			m_theBoardRebirthCount = 0;
			m_theBoardShowRebirthAdCount = 0;
		}
	}

	public void SetCurrentOriginRebirth(SingleOriginRebirthForRowData data, RebirthType rebirthType = RebirthType.OriginRebirth)
	{
		m_currentOriginRebirth = data;
		m_isAdRebirth = false;
		if (data != null)
		{
			m_rebirthType = rebirthType;
		}
	}

	public bool IsCanShowOriginRebirthDiscount(out int? shopID)
	{
		bool result = false;
		int curLevelId = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).CurLevelId;
		string[] array = Mod.DataTable.Get<Levels_levelTable>().Get(curLevelId).OriginRebirthDiscountid.Split('|');
		shopID = null;
		int[] array2 = new int[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			int result2 = 0;
			if (!int.TryParse(array[i], out result2))
			{
				Log.Error("IsCanShowOriginRebirthDiscount(),idsarray[{0}] is not to int", i);
			}
			else
			{
				OriginRebirth_Discount originRebirth_Discount = Mod.DataTable.Get<OriginRebirth_Discount>().Get(result2);
				if (originRebirth_Discount.Min <= ProgressPercentage && ProgressPercentage <= originRebirth_Discount.Max && (float)m_theLevelPlayCount >= originRebirth_Discount.Count && m_theLevelShowOriginRebirthDiscountCount == 0 && m_theBoardRebirthCount == 0)
				{
					shopID = originRebirth_Discount.ShopID;
					result = true;
					break;
				}
			}
			array2[i] = result2;
		}
		return result;
	}

	public bool IsCanShowRebirthForm()
	{
		bool result = false;
		if (m_theBoardRebirthCount < m_maxTheBoardRebirthCount)
		{
			if ((int)Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).GetPlayGoodsNum(2) <= 0)
			{
				if (m_theBoardRebirthCount < m_maxTheBoardRebirthCount)
				{
					result = true;
				}
			}
			else
			{
				result = true;
			}
		}
		return result;
	}

	public int GetAwardNumByType(DropType dropType)
	{
		int result = 0;
		switch (dropType)
		{
		case DropType.CROWNFRAGMENT:
			result = GainedCrownFragments;
			break;
		case DropType.DIAMONDFRAGMENT:
			result = GainedDiamondFragments;
			break;
		default:
			Debug.LogError("Donot have the element");
			break;
		}
		return result;
	}

	private void RefreshDiamondCollectCount()
	{
		m_currentDiamondCollectCount = 0;
		for (int i = 0; i < m_diamondCollects.Count; i++)
		{
			if (m_diamondCollects[i] == 1)
			{
				m_currentDiamondCollectCount++;
			}
		}
		m_IsDiamondCollectsFinished = m_currentDiamondCollectCount >= 10;
	}

	private void RefreshCrownCollectCount()
	{
		m_currentCrownCollectCount = 0;
		for (int i = 0; i < m_crownCollects.Count; i++)
		{
			if (m_crownCollects[i] == 1)
			{
				m_currentCrownCollectCount++;
			}
		}
		m_IsCrownCollectsFinished = m_currentCrownCollectCount >= 3;
	}

	public DataNames GetName()
	{
		return DataNames.InsideGameDataModule;
	}

	private void Refresh()
	{
		SetCurrentRebirthBoxData(null);
		SetCurrentOriginRebirth(null);
		m_dropDatas.Clear();
		m_isOriginRebirth = false;
		m_theBoardRebirthCount = 0;
		m_theBoardShowRebirthAdCount = 0;
	}

	public void ClearDropDatas()
	{
		m_dropDatas.Clear();
	}

	public void ClearRebirthCount()
	{
		m_theBoardRebirthCount = 0;
		m_theBoardShowRebirthAdCount = 0;
	}

	public void InitializeLevelData(int level, int deltaLevelLength = 0)
	{
		m_ProgressPercentage = 0;
		m_GainedDiamonds = 0;
		m_GainedCrowns = 0;
		m_GainedCrownFragments = 0;
		m_GainedDiamondFragments = 0;
		Vector3 position = Railway.theRailway.transform.TransformPoint(BaseRole.BallPosition);
		m_LevelActualLength = MapController.Instance.GetLevelLength() - MapController.Instance.GetCurrentGridRow(position);
		m_LevelActualLength += deltaLevelLength;
	}

	public void InitBuyOutRebirth()
	{
		ResetBuyOutRebirth();
		DieCountForShowPromote = 0;
		FurthestBuyOutRebirthProgress = -1;
	}

	public void ResetBuyOutRebirth()
	{
		BuyOutRebirthPoint = Vector3.zero;
		FirstBuyOutRebirthPoint = Vector3.zero;
		TriggeredBuyOutRebirth.Clear();
		BuyOutRebirthGainDiamounds = 0;
		BuyOutRebirthGainCrowns = 0;
		BuyOutRebirthGainDiamoundFragments = 0;
		BuyOutRebirthGainCrownFragments = 0;
		BuyOutRebirthPreDiamounds = 0;
		BuyOutRebirthPreCrowns = 0;
		BuyOutRebirthIndex = 0;
	}

	public void ResetFirstBuyOutRebirth()
	{
		TriggeredBuyOutRebirth.Clear();
		TriggeredBuyOutRebirth.Add(FirstBuyOutRebirthUUID, FirstBuyOutRebirthUUID);
		BuyOutRebirthIndex = 1;
	}

	public void TheBoardStart()
	{
		Refresh();
		m_theLevelPlayCount++;
	}

	public void TheBoardFinished()
	{
		Refresh();
	}

	public void LevelEnter()
	{
		Refresh();
		m_theLevelShowOriginRebirthDiscountCount = 0;
		m_theLevelPlayCount = 0;
		GameDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule);
		PlayerLocalLevelData playerLevelData = PlayerDataModule.Instance.GetPlayerLevelData(dataModule.CurLevelId);
		m_crownCollects = playerLevelData.CrownCollect;
		m_diamondCollects = playerLevelData.DiamondsCollect;
		m_IsCrownCollectsFinished = playerLevelData.IsCrownCollectFinished();
		m_IsDiamondCollectsFinished = playerLevelData.IsDiamondsCollectFinished();
		RefreshDiamondCollectCount();
		RefreshCrownCollectCount();
		m_isCollectsFinishedForStart = IsCollectFinished();
	}

	public void LevelLeave()
	{
		Refresh();
		m_theLevelShowOriginRebirthDiscountCount = 0;
		m_theLevelPlayCount = 0;
	}

	public void ResetLevelData(RebirthBoxData data = null)
	{
		if (data != null && data.m_rebirthProgressData != null)
		{
			m_ProgressPercentage = data.m_rebirthProgressData.m_levelProgress;
			m_GainedDiamonds = data.m_rebirthProgressData.m_gainedDiamonds;
			m_GainedCrowns = data.m_rebirthProgressData.m_gainedCrowns;
			m_GainedDiamondFragments = data.m_rebirthProgressData.m_gainedDiamondFragments;
			m_GainedCrownFragments = data.m_rebirthProgressData.m_gainedCrownFragments;
		}
		else
		{
			m_ProgressPercentage = 0;
			m_GainedDiamonds = 0;
			m_GainedCrowns = 0;
			m_GainedCrownFragments = 0;
			m_GainedDiamondFragments = 0;
		}
	}

	public void UpdateProgress()
	{
		float num = (float)BaseRole.theBall.CurrentWorldRow / (float)m_LevelActualLength;
		if (num > 1f)
		{
			num = 1f;
		}
		ProgressPercentage = MathUtils.Floored(num * 100f);
	}

	public void OnProgress()
	{
		Mod.Event.FireNow(this, Mod.Reference.Acquire<ProgressEventArgs>().Initialize(ProgressPercentage));
	}

	public void OnDiamonds()
	{
		Mod.Event.FireNow(this, Mod.Reference.Acquire<DiamondsChangeEventArgs>().Initialize(m_GainedDiamonds));
	}

	public void OnCrowns()
	{
		Mod.Event.FireNow(this, Mod.Reference.Acquire<CrownsChangeEventArgs>().Initialize(m_GainedCrowns));
	}

	public DropData? FindDropDataByUUID(int uuid)
	{
		DropData value;
		if (m_dropDatas.TryGetValue(uuid, out value))
		{
			return value;
		}
		return null;
	}

	public bool IsCollectBySortID(DropType type, int sortID)
	{
		switch (type)
		{
		case DropType.DIAMOND:
			if (m_diamondCollects == null || m_diamondCollects.Count <= sortID)
			{
				return false;
			}
			return m_diamondCollects[sortID] == 1;
		case DropType.CROWN:
			if (m_crownCollects == null || m_crownCollects.Count <= sortID)
			{
				return false;
			}
			return m_crownCollects[sortID] == 1;
		default:
			return false;
		}
	}

	public bool IsCollectFinishedByDropType(DropType type)
	{
		switch (type)
		{
		case DropType.DIAMOND:
			return m_IsDiamondCollectsFinished;
		case DropType.CROWN:
			return m_IsCrownCollectsFinished;
		default:
			return false;
		}
	}

	public bool IsCollectFinished()
	{
		if (m_IsDiamondCollectsFinished)
		{
			return m_IsCrownCollectsFinished;
		}
		return false;
	}

	public bool IsShowForAwardByDropType(DropType type, int sortID)
	{
		bool flag = IsCollectFinished();
		if (flag && m_isCollectsFinishedForStart)
		{
			return true;
		}
		if (flag && !m_isCollectsFinishedForStart)
		{
			return false;
		}
		if (!flag)
		{
			return !IsCollectBySortID(type, sortID);
		}
		return false;
	}

	private void GainedDrop(DropData dropData)
	{
		switch (dropData.m_type)
		{
		case DropType.DIAMOND:
			if (!m_dropDatas.ContainsKey(dropData.m_uuid))
			{
				GameController.Instance.GetInsideGameDataModule.GainedDiamonds += dropData.m_number;
				m_dropDatas[dropData.m_uuid] = dropData;
				m_diamondCollects[dropData.m_sortID] = 1;
				RefreshDiamondCollectCount();
			}
			break;
		case DropType.CROWN:
			if (!m_dropDatas.ContainsKey(dropData.m_uuid))
			{
				GameController.Instance.GetInsideGameDataModule.GainedCrowns += dropData.m_number;
				m_dropDatas[dropData.m_uuid] = dropData;
				m_crownCollects[dropData.m_sortID] = 1;
				RefreshCrownCollectCount();
			}
			break;
		case DropType.DIAMONDFRAGMENT:
			if (!m_dropDatas.ContainsKey(dropData.m_uuid))
			{
				GameController.Instance.GetInsideGameDataModule.GainedDiamondFragments += dropData.m_number;
				m_dropDatas[dropData.m_uuid] = dropData;
			}
			break;
		case DropType.CROWNFRAGMENT:
			if (!m_dropDatas.ContainsKey(dropData.m_uuid))
			{
				GameController.Instance.GetInsideGameDataModule.GainedCrownFragments += dropData.m_number;
				m_dropDatas[dropData.m_uuid] = dropData;
			}
			break;
		case DropType.TRIGGERFRAGMENT:
			if (!m_dropDatas.ContainsKey(dropData.m_uuid))
			{
				m_dropDatas[dropData.m_uuid] = dropData;
			}
			break;
		}
	}

	public void SubscribeEventArgs()
	{
		Mod.Event.Subscribe(EventArgs<GainedDropEventArgs>.EventId, HandlerGainedDrop);
		Mod.Event.Subscribe(EventArgs<GameFailEventArgs>.EventId, OnGameFailed);
		Mod.Event.Subscribe(EventArgs<GameResetEventArgs>.EventId, OnGameReset);
		Mod.Event.Subscribe(EventArgs<GameSucessEventArgs>.EventId, OnGameSucess);
	}

	public void UnSubscribeEventArgs()
	{
		Mod.Event.Unsubscribe(EventArgs<GainedDropEventArgs>.EventId, HandlerGainedDrop);
		Mod.Event.Unsubscribe(EventArgs<GameFailEventArgs>.EventId, OnGameFailed);
		Mod.Event.Unsubscribe(EventArgs<GameResetEventArgs>.EventId, OnGameReset);
		Mod.Event.Unsubscribe(EventArgs<GameSucessEventArgs>.EventId, OnGameSucess);
	}

	private void HandlerGainedDrop(object sender, EventArgs e)
	{
		GainedDropEventArgs gainedDropEventArgs = e as GainedDropEventArgs;
		if (gainedDropEventArgs != null)
		{
			GainedDrop(gainedDropEventArgs.m_dropData);
		}
	}

	private void OnGameFailed(object sender, EventArgs e)
	{
		if (ProgressPercentage >= FurthestBuyOutRebirthProgress && FurthestBuyOutRebirthProgress >= 0)
		{
			DieCountForShowPromote++;
		}
	}

	private void OnGameReset(object sender, EventArgs e)
	{
	}

	private void OnGameSucess(object sender, EventArgs e)
	{
		InitBuyOutRebirth();
	}

	public void SetBuyOutRebirthData(bool isFirst = false)
	{
		m_isOriginRebirth = false;
		ClearDropDatas();
		ClearRebirthCount();
		GainedCrowns = BuyOutRebirthGainCrowns;
		GainedDiamonds = BuyOutRebirthGainDiamounds;
		GainedDiamondFragments = BuyOutRebirthGainDiamoundFragments;
		GainedCrownFragments = BuyOutRebirthGainCrownFragments;
		PreDiamonds = BuyOutRebirthPreDiamounds;
		PreCrowns = BuyOutRebirthPreCrowns;
		SingleOriginRebirthForRowData singleOriginRebirthForRowData = null;
		singleOriginRebirthForRowData = ((!isFirst) ? GameController.GetOriginRebirthForRowValueByPosition(BuyOutRebirthPoint) : GameController.GetOriginRebirthForRowValueByPosition(FirstBuyOutRebirthPoint));
		SetCurrentOriginRebirth(singleOriginRebirthForRowData, RebirthType.BuyOutRebirth);
	}
}
