using System;
using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDataModule : IDataModule
{
	private static PlayerDataModule m_instance;

	private bool isLocalSaved = true;

	private bool isNetworkSaved = true;

	private uint powerTimerId;

	private bool m_isBeginInit;

	private bool isLoginInDifferentDay;

	private bool m_isServerNewUser;

	public static bool ApplicationQuit;

	private long m_adKeyTimeStamp = -1L;

	private long m_adPowerTimeStamp = -1L;

	private Dictionary<int, CryVDouble> playerAssert = new Dictionary<int, CryVDouble>();

	private Dictionary<int, IActiveTimeController> m_playerActiveTimeDic = new Dictionary<int, IActiveTimeController>();

	private PlayerLocalGoodsAndLevelData playerAllLocalData = new PlayerLocalGoodsAndLevelData();

	private SynChangeDataBuffer changeDataBuffer = new SynChangeDataBuffer();

	private SynChangeLevelDataBuffer changeLevelDataBuffer = new SynChangeLevelDataBuffer();

	private long m_serverTime;

	private PlayerLocalSequenceLoginData m_SequenceLoginData = new PlayerLocalSequenceLoginData();

	private PlayerLocalRecordData playerRecordData = new PlayerLocalRecordData();

	private PlayerLocalTargetsData playerLevelTargetData = new PlayerLocalTargetsData();

	private PlayerLocalGiftpackageData playerGiftPackageData = new PlayerLocalGiftpackageData();

	private PlayerLocalMotivateAwardData playerMotivateAwardData = new PlayerLocalMotivateAwardData();

	private PlayerLocalCommonData playerCommonData = new PlayerLocalCommonData();

	private PlayerLocalLevelSeriesData playerLevelSeriesData = new PlayerLocalLevelSeriesData();

	private PlayerLocalVideoAwardData playerLocalVideoAwardData = new PlayerLocalVideoAwardData();

	private List<PlayerBufferData> m_bufferList = new List<PlayerBufferData>();

	private ScreenPluginAdController m_PluginAdController = new ScreenPluginAdController();

	private PlayerLocalCoolPlayPackageData m_CoolPlayPagageData = new PlayerLocalCoolPlayPackageData();

	private PlayerLuckTurnTableLocalData m_LuckTurnLocalData = new PlayerLuckTurnTableLocalData();

	private const int START_FREE_SHIELD_TUTORIAL = 27;

	private const int ORIGIN_REBIRTH_FREE_TUTORIAL = 28;

	private const int GUIDELINE_TUTORIAL = 29;

	private PlayerLocalAllInstrumentData m_playerInstrumentLocalData = new PlayerLocalAllInstrumentData();

	private SynChangePlayerInstrumentDataBuffer m_SynInstrumentBuffer = new SynChangePlayerInstrumentDataBuffer();

	private uint m_ProductTimerId;

	private int m_InstrumentProductCount;

	private double m_offLineProductReputationNum;

	private double m_offLineProductGoldNum;

	private int m_offLineTime;

	private long m_speedUpEndTimeStamp;

	private double m_lastProductSpeed;

	private Dictionary<int, PlayerLocalLevelData> playerLevelDic = new Dictionary<int, PlayerLocalLevelData>();

	private List<LevelMetaTableData> m_golableLevelData = new List<LevelMetaTableData>();

	private LevelMetaTableData m_lastEndterLevelData = new LevelMetaTableData();

	private PlayerLocalAllMessionData m_MessionData = new PlayerLocalAllMessionData();

	private SynChangePlayerMessionBuffer m_MessionBuffer = new SynChangePlayerMessionBuffer();

	private CryVInt32 m_Level = 1;

	private CryVInt32 m_StarLevel = 0;

	private CryVInt32 m_HideLevel = 1;

	private PlayerLocalPropertyData m_PropertLocalData = new PlayerLocalPropertyData();

	private SynChangePlayerPropertyBuffer m_PropertySynBuffer;

	private uint levelTargetTimerId;

	public bool IsServerNewUser
	{
		get
		{
			return m_isServerNewUser;
		}
	}

	public bool IsLoginInDifferentDay
	{
		get
		{
			return isLoginInDifferentDay;
		}
	}

	public bool BeginInit
	{
		get
		{
			return m_isBeginInit;
		}
	}

	public bool IsInited { get; set; }

	public static PlayerDataModule Instance
	{
		get
		{
			if (m_instance == null)
			{
				m_instance = new PlayerDataModule();
			}
			return m_instance;
		}
	}

	public Dictionary<int, IActiveTimeController> PlayerActiveTimeDic
	{
		get
		{
			return m_playerActiveTimeDic;
		}
	}

	public PlayerLocalGoodsAndLevelData PlayerAllLocalData
	{
		get
		{
			return playerAllLocalData;
		}
	}

	public long ServerTime
	{
		get
		{
			if (m_serverTime <= 0)
			{
				m_serverTime = (long)TimeTools.ChangeUnixTime(DateTime.Now);
			}
			return m_serverTime + (int)(Time.realtimeSinceStartup * 1000f);
		}
		set
		{
			m_serverTime = value;
		}
	}

	public PlayerLocalSequenceLoginData SequenceLoginData
	{
		get
		{
			return m_SequenceLoginData;
		}
	}

	public PlayerLocalRecordData PlayerRecordData
	{
		get
		{
			return playerRecordData;
		}
	}

	public PlayerLocalTargetsData PlayerLevelTargetData
	{
		get
		{
			return playerLevelTargetData;
		}
	}

	public PlayerLocalGiftpackageData PlayerGiftPackageData
	{
		get
		{
			return playerGiftPackageData;
		}
	}

	public PlayerLocalMotivateAwardData PlayerMotivateAwardData
	{
		get
		{
			return playerMotivateAwardData;
		}
	}

	public PlayerLocalCommonData PlayerCommonData
	{
		get
		{
			return playerCommonData;
		}
	}

	public PlayerLocalLevelSeriesData PlayerLocalLevelSeriesData
	{
		get
		{
			return playerLevelSeriesData;
		}
	}

	public PlayerLocalVideoAwardData PlayerLocalVideoAwardData
	{
		get
		{
			return playerLocalVideoAwardData;
		}
	}

	public ScreenPluginAdController PluginAdController
	{
		get
		{
			return m_PluginAdController;
		}
	}

	public PlayerLocalCoolPlayPackageData CoolPlayPagageData
	{
		get
		{
			return m_CoolPlayPagageData;
		}
	}

	public PlayerLuckTurnTableLocalData LuckTurnLocalData
	{
		get
		{
			return m_LuckTurnLocalData;
		}
	}

	public double OffLineProductReputationNum
	{
		get
		{
			return m_offLineProductReputationNum;
		}
	}

	public double OffLineProductGoldNum
	{
		get
		{
			return m_offLineProductGoldNum;
		}
	}

	public int OffLineTime
	{
		get
		{
			return m_offLineTime;
		}
		set
		{
			m_offLineTime = value;
		}
	}

	public PlayerLocalAllInstrumentData PlayerInstrumentLocalData
	{
		get
		{
			return m_playerInstrumentLocalData;
		}
	}

	public LevelEnterType CurrentEnterLevelType { get; set; }

	public List<LevelMetaTableData> GloableLevelLableData
	{
		get
		{
			return m_golableLevelData;
		}
	}

	public LevelMetaTableData LastEndterLevelData
	{
		get
		{
			return m_lastEndterLevelData;
		}
	}

	public PlayerLocalAllMessionData MessionData
	{
		get
		{
			return m_MessionData;
		}
	}

	public PlayerLocalPropertyData PropertyLocalData
	{
		get
		{
			return m_PropertLocalData;
		}
	}

	private PlayerDataModule()
	{
	}

	public void Init()
	{
		m_isBeginInit = true;
		InitDataFromLocal();
		MonoSingleton<GameTools>.Instacne.RequestCloudData();
		MonoSingleton<NetWorkManager>.Instacne.IsSynNetwork = Director.Ins.ConnectedGameServer;
		if (MonoSingleton<NetWorkManager>.Instacne.IsSynNetwork)
		{
			BuiltinDialogForm.Current.ShowProgressContent(Mod.Localization.GetInfoById(295));
			GetPlayerAllDataFromServer(GetPlayerDataSuccessHanlder, GetPlayerDataFailedHandler);
		}
		else
		{
			InitData();
		}
		ApplicationQuit = false;
	}

	private void GetPlayerDataFailedHandler()
	{
		int cnt = 5;
		MonoTimer timer = new MonoTimer(1f, true);
		timer.Elapsed += delegate
		{
			string content = string.Format(Mod.Localization.GetInfoById(296), cnt);
			BuiltinDialogForm.Current.ShowProgressContent(content);
			cnt--;
			if (cnt < 1)
			{
				BuiltinDialogForm.Current.ShowProgressContent(Mod.Localization.GetInfoById(295));
				GetPlayerAllDataFromServer(GetPlayerDataSuccessHanlder, GetPlayerDataFailedHandler);
				timer.Stop();
			}
		};
		timer.FireElapsedOnStop = false;
		timer.Start();
	}

	private void GetPlayerDataSuccessHanlder()
	{
		BuiltinDialogForm.Current.ShowProgressContent(Mod.Localization.GetInfoById(297));
	}

	private void LaunchSetting()
	{
		RecordLaunchCount();
		if (IsAppFirstLaunch())
		{
			SetAppLaunchTime(DateTime.Now);
			SetMusicInstrumentAdTime(DateTime.Now);
			SetFirstAppLaunchTime(DateTime.Now);
			SetDefaultAssertNum();
			InitLevelResetTime(DateTime.Now);
			playerRecordData.WatchPowerAdNum = GameCommon.getPowerByAdMaxNumEveryDay;
			string info = string.Format("type1=1&open_interval=0");
			InfocUtils.ReportInfoc(InfocTable.rollingsky2_games_openshow, info);
			InfocUtils.Report_rollingsky2_games_gamefiller(1);
		}
		else
		{
			DateTime dateTime = TimeTools.StringToDatetime(GetAppLaunchTime());
			DateTime now = DateTime.Now;
			int num = (int)(now - dateTime).TotalSeconds;
			string info2 = string.Format("type1=1&open_interval={0}", num);
			InfocUtils.ReportInfoc(InfocTable.rollingsky2_games_openshow, info2);
			if (!TimeTools.IsSameDay(dateTime, now))
			{
				isLoginInDifferentDay = true;
				playerGiftPackageData.TodayHadRecommand = false;
				InfoScore();
				playerRecordData.WatchPowerAdNum = GameCommon.getPowerByAdMaxNumEveryDay;
				playerLevelSeriesData.ResetMotivateAward();
				PlayerCommonData.ResetNextDay();
			}
			else
			{
				isLoginInDifferentDay = false;
			}
			DealLevelTargets(now);
			double powerNum = GetPlayGoodsNum(1);
			if (powerNum < (double)GameCommon.startPowerRecoverThreshold)
			{
				if (!IsHadPowerCDStartTimestamp())
				{
					RecordPower();
				}
				else
				{
					float num2 = LocalPowerCdGoing();
					int powerCDByAd = MonoSingleton<GameTools>.Instacne.GetPowerCDByAd();
					int b = (int)((num2 + 1f) / (float)powerCDByAd);
					b = Mathf.Max(0, b);
					if (powerNum + (double)b >= (double)GameCommon.startPowerRecoverThreshold)
					{
						SetPlayGoodsNum(1, GameCommon.startPowerRecoverThreshold);
						ClearPowerCDStartTimestamp();
					}
					else
					{
						SetPlayGoodsNum(1, powerNum + (double)b);
						float num3 = num2 - (float)(powerCDByAd * b);
						RecordPowerCDStartTimestamp((int)(0f - num3));
						float num4 = (float)powerCDByAd - num3;
						if (num4 <= 0f)
						{
							RecordPowerCDStartTimestamp();
							uint num5 = (uint)((float)MonoSingleton<GameTools>.Instacne.GetPowerCDByAd() * 1000f);
							powerTimerId = TimerHeap.AddTimer(num5, num5, PowerTimerCdEndHandler);
						}
						else
						{
							powerTimerId = TimerHeap.AddTimer((uint)(num4 * 1000f), 0u, delegate
							{
								TimerHeap.DelTimer(powerTimerId);
								ChangePlayerGoodsNum(1, GameCommon.powerRecoverNum, AssertChangeType.TIME);
								powerNum = GetPlayGoodsNum(1);
								if (powerNum >= (double)GameCommon.startPowerRecoverThreshold)
								{
									ClearPowerCDStartTimestamp();
								}
								else
								{
									RecordPowerCDStartTimestamp();
									uint num6 = (uint)((float)MonoSingleton<GameTools>.Instacne.GetPowerCDByAd() * 1000f);
									powerTimerId = TimerHeap.AddTimer(num6, num6, PowerTimerCdEndHandler);
								}
							});
						}
					}
				}
			}
			else
			{
				ClearPowerCDStartTimestamp();
			}
			SetAppLaunchTime(now);
			SetMusicInstrumentAdTime(DateTime.Now);
		}
		if (GetCanShowTutorialTips() && IsHadUnLockLevelExceptLevelOne())
		{
			ForbidShowTutorialTips();
		}
		BuildinTutorialForm.Init();
	}

	private void OnReportRisk()
	{
		playerAllLocalData.ClearData();
		changeDataBuffer.Clear();
		changeLevelDataBuffer.Clear();
		PostPlayerAllDataToServer();
		CommonAlertData commonAlertData = new CommonAlertData();
		commonAlertData.showType = CommonAlertData.AlertShopType.COMMON;
		commonAlertData.alertContent = Mod.Localization.GetInfoById(206);
		commonAlertData.callBackFunc = delegate
		{
			Application.Quit();
		};
		commonAlertData.cancelCallBackFunc = delegate
		{
			Application.Quit();
		};
		commonAlertData.closeCallBackFunc = delegate
		{
			Application.Quit();
		};
		Mod.UI.OpenUIForm(UIFormId.CommonAlertForm, commonAlertData);
	}

	public void RecordPowerCDStartTimestamp(int offsetSeconds = 0)
	{
		string values = TimeTools.DateTimeToString(DateTime.Now.AddSeconds(offsetSeconds));
		EncodeConfig.setConfig(PlayerLocalDatakey.POWERCDSTART, values);
	}

	private int LocalPowerCdGoing()
	{
		DateTime dateTime = TimeTools.StringToDatetime(EncodeConfig.getConfig(PlayerLocalDatakey.POWERCDSTART));
		int num = (int)(DateTime.Now - dateTime).TotalSeconds;
		if (num <= 0)
		{
			return 0;
		}
		return num;
	}

	public bool IsHadPowerCDStartTimestamp()
	{
		if (EncodeConfig.getConfig(PlayerLocalDatakey.POWERCDSTART).Length <= 0)
		{
			return false;
		}
		return true;
	}

	public void ClearPowerCDStartTimestamp()
	{
		EncodeConfig.clearConfig(PlayerLocalDatakey.POWERCDSTART);
		TimerHeap.DelTimer(powerTimerId);
	}

	public void RecordPower()
	{
		if (GetPlayGoodsNum(1) <= (double)GameCommon.startPowerRecoverThreshold && !IsHadPowerCDStartTimestamp())
		{
			RecordPowerCDStartTimestamp();
			uint num = (uint)((float)MonoSingleton<GameTools>.Instacne.GetPowerCDByAd() * 1000f);
			powerTimerId = TimerHeap.AddTimer(num, num, PowerTimerCdEndHandler);
		}
	}

	private void PowerTimerCdEndHandler()
	{
		if (!(GetPlayGoodsNum(1) < (double)GameCommon.startPowerRecoverThreshold))
		{
			return;
		}
		bool flag = Mod.UI.UIFormIsOpen(UIFormId.HomeForm);
		ChangePlayerGoodsNum(1, GameCommon.powerRecoverNum, AssertChangeType.TIME, NetWorkSynType.NORMAL, true, flag);
		if (!flag)
		{
			PlayerLocalGoodsAndLevelData.NeedSave = true;
		}
		if (GetPlayGoodsNum(1) >= (double)GameCommon.startPowerRecoverThreshold)
		{
			ClearPowerCDStartTimestamp();
		}
		else
		{
			RecordPowerCDStartTimestamp();
		}
		if (Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).PlayerRecordData.firstWaitTime_GetPower == 0)
		{
			Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).PlayerRecordData.firstWaitTime_GetPower = 1;
			Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).PlayerRecordData.SaveToLocal();
			if (MonoSingleton<GameTools>.Instacne.IsFirstLanuchGameIn24Hour())
			{
				InfocUtils.Report_rollingsky2_games_gamefiller(6);
			}
		}
	}

	public uint PowerCdGoing()
	{
		return TimerHeap.GetLeftTimeById(powerTimerId);
	}

	public AbsTimerData GetPowerTimer()
	{
		return TimerHeap.GetTimer(powerTimerId);
	}

	private void RecordLaunchCount()
	{
		int @int = EncodeConfig.getInt(PlayerLocalDatakey.APPLAUNCHCOUNT);
		@int++;
		EncodeConfig.setInt(PlayerLocalDatakey.APPLAUNCHCOUNT, @int);
	}

	public int GetLaunchCount()
	{
		return EncodeConfig.getInt(PlayerLocalDatakey.APPLAUNCHCOUNT);
	}

	public bool IsAppFirstLaunch()
	{
		return EncodeConfig.getInt(PlayerLocalDatakey.APPLAUNCHCOUNT) == 1;
	}

	public string GetAppLaunchTime()
	{
		return EncodeConfig.getConfig(PlayerLocalDatakey.APPLAUNCHTIME);
	}

	public string GetFirstAppLaunchTime()
	{
		return EncodeConfig.getConfig(PlayerLocalDatakey.APPFIRSTLAUNCHTIME);
	}

	public void SetAppLaunchTime(DateTime time)
	{
		string values = TimeTools.DateTimeToString(time);
		EncodeConfig.setConfig(PlayerLocalDatakey.APPLAUNCHTIME, values);
	}

	public void SetMusicInstrumentAdTime(DateTime time)
	{
		string values = TimeTools.DateTimeToString(time);
		EncodeConfig.setConfig(PlayerLocalDatakey.MUSICINSTRUMENTADTIME, values);
	}

	public DateTime GetMusicInstrumentAdTime()
	{
		return TimeTools.StringToDatetime(EncodeConfig.getConfig(PlayerLocalDatakey.MUSICINSTRUMENTADTIME));
	}

	public void SetFirstAppLaunchTime(DateTime time)
	{
		string values = TimeTools.DateTimeToString(time);
		EncodeConfig.setConfig(PlayerLocalDatakey.APPFIRSTLAUNCHTIME, values);
	}

	public bool GetCanShowTutorial()
	{
		return EncodeConfig.getInt(PlayerLocalDatakey.SHOWTUTORIAL) == 0;
	}

	public void ForbidShowTutorial()
	{
		EncodeConfig.setInt(PlayerLocalDatakey.SHOWTUTORIAL, 1);
	}

	public bool GetCanShowTutorialTips()
	{
		return EncodeConfig.getInt(PlayerLocalDatakey.SHOWTUTORIALTIPS) == 0;
	}

	public void ForbidShowTutorialTips()
	{
		EncodeConfig.setInt(PlayerLocalDatakey.SHOWTUTORIALTIPS, 1);
	}

	public void RecordPlayerUsePowerNum(int num)
	{
		int @int = EncodeConfig.getInt(PlayerLocalDatakey.PLAYERUSEPOWERNUM);
		@int += num;
		EncodeConfig.setInt(PlayerLocalDatakey.PLAYERUSEPOWERNUM, @int);
	}

	public int GetPlayerUsePowerNum()
	{
		return EncodeConfig.getInt(PlayerLocalDatakey.PLAYERUSEPOWERNUM);
	}

	public void AddPlayerEnterLevelTotalNum()
	{
		int @int = EncodeConfig.getInt(PlayerLocalDatakey.PLAYERENTERLEVELTOTALNUM);
		@int++;
		EncodeConfig.setInt(PlayerLocalDatakey.PLAYERENTERLEVELTOTALNUM, @int);
	}

	public int GetPlayerTotalEnterLevelNum()
	{
		return EncodeConfig.getInt(PlayerLocalDatakey.PLAYERENTERLEVELTOTALNUM);
	}

	public int GetPlayerOpenGetGoodsFormNum()
	{
		return EncodeConfig.getInt(PlayerLocalDatakey.PLAYEROPENGETGOODSFORMNUM);
	}

	public void SetPlayerOpenGetGoodsFormNum(int num)
	{
		EncodeConfig.setInt(PlayerLocalDatakey.PLAYEROPENGETGOODSFORMNUM, num);
	}

	public static void SetGameClouldDataToLocal(string configData)
	{
		EncodeConfig.setConfig(PlayerLocalDatakey.GAMECLOULDCONFIG, configData);
	}

	public static string GetGameCloudDataFromLocal()
	{
		return EncodeConfig.getConfig(PlayerLocalDatakey.GAMECLOULDCONFIG);
	}

	public void AddPlayerFinishLevelNum(int num = 1)
	{
		int @int = EncodeConfig.getInt(PlayerLocalDatakey.PLAYERFINISHLEVELNUM);
		@int += num;
		EncodeConfig.setInt(PlayerLocalDatakey.PLAYERFINISHLEVELNUM, @int);
	}

	public int GetPlayerFinishLevelNum()
	{
		return EncodeConfig.getInt(PlayerLocalDatakey.PLAYERFINISHLEVELNUM);
	}

	public void AddPlayerInLevelTotalTime(int lastTime)
	{
		if (lastTime > 0)
		{
			int @int = EncodeConfig.getInt(PlayerLocalDatakey.PLAYERPLAYINLEVLE_TOTALTIME);
			@int += lastTime;
			EncodeConfig.setInt(PlayerLocalDatakey.PLAYERPLAYINLEVLE_TOTALTIME, @int);
		}
	}

	public int GetPlayerInLevelTotalTime()
	{
		return EncodeConfig.getInt(PlayerLocalDatakey.PLAYERPLAYINLEVLE_TOTALTIME);
	}

	public string GetAppLocalVersion()
	{
		return EncodeConfig.getConfig(PlayerLocalDatakey.APPLASTVERSION);
	}

	public void SetAppLocalVersion()
	{
		EncodeConfig.setConfig(PlayerLocalDatakey.APPLASTVERSION, Application.version);
	}

	public long GetAdKeyTimeStamp()
	{
		if (m_adKeyTimeStamp < 0)
		{
			string config = EncodeConfig.getConfig(PlayerLocalDatakey.ADKEYCDSTAMP);
			if (string.IsNullOrEmpty(config))
			{
				m_adKeyTimeStamp = 0L;
			}
			else
			{
				long result = 0L;
				if (long.TryParse(config, out result))
				{
					m_adKeyTimeStamp = result;
				}
				else
				{
					m_adKeyTimeStamp = ServerTime;
				}
			}
		}
		return m_adKeyTimeStamp;
	}

	public void SaveADKeyTimeStamp(long timpStamp)
	{
		m_adKeyTimeStamp = timpStamp;
		EncodeConfig.setConfig(PlayerLocalDatakey.ADKEYCDSTAMP, timpStamp.ToString());
	}

	public long GetAdPowerTimeStamp()
	{
		if (m_adPowerTimeStamp < 0)
		{
			string config = EncodeConfig.getConfig(PlayerLocalDatakey.ADPOWERCDSTAMP);
			if (string.IsNullOrEmpty(config))
			{
				m_adPowerTimeStamp = 0L;
			}
			else
			{
				long result = 0L;
				if (long.TryParse(config, out result))
				{
					m_adPowerTimeStamp = result;
				}
				else
				{
					m_adPowerTimeStamp = ServerTime;
				}
			}
		}
		return m_adPowerTimeStamp;
	}

	public void SaveADPowerTimeStamp(long timpStamp)
	{
		m_adPowerTimeStamp = timpStamp;
		EncodeConfig.setConfig(PlayerLocalDatakey.ADPOWERCDSTAMP, timpStamp.ToString());
	}

	public bool IsSoundPlayOn()
	{
		if (Mod.Setting.GetInt("Setting.SoundMuted") != 1)
		{
			return false;
		}
		return true;
	}

	public bool IsMusicPlayOn()
	{
		if (Mod.Setting.GetInt("Setting.MusicMuted") != 1)
		{
			return false;
		}
		return true;
	}

	public DataNames GetName()
	{
		return DataNames.PlayerDataModule;
	}

	private void InfoScore()
	{
		Dictionary<int, PlayerLocalLevelData>.Enumerator enumerator = playerLevelDic.GetEnumerator();
		int num = 0;
		int playerTotalDiamondsNum = GetPlayerTotalDiamondsNum();
		int playerTotalCrownsNum = GetPlayerTotalCrownsNum();
		double playGoodsNum = GetPlayGoodsNum(6);
		int num2 = 0;
		double playGoodsNum2 = GetPlayGoodsNum(1);
		double playGoodsNum3 = GetPlayGoodsNum(3);
		double playGoodsNum4 = GetPlayGoodsNum(11);
		while (enumerator.MoveNext())
		{
			if (enumerator.Current.Value.MaxProgress >= 100)
			{
				num++;
			}
		}
		string info = string.Format("allscore={0}&alldiamond={1}&allcrown={2}&allkey={3}&allshield={4}&allheart={5}&allmagic={6}&allstar={7}", num, playerTotalDiamondsNum, playerTotalCrownsNum, playGoodsNum, num2, playGoodsNum2, playGoodsNum4, playGoodsNum3);
		InfocUtils.ReportInfoc(InfocTable.rollingsky2_games_dayscore, info);
	}

	private void InitData()
	{
		List<int> list = new List<int>
		{
			131100, 121100, 141100, 122100, 112100, 141300, 111201, 131301, 151102, 161102,
			151301
		};
		playerAllLocalData.InitLevelDefaultData();
		if (playerAllLocalData != null)
		{
			for (int i = 0; i < playerAllLocalData.GoodsCount; i++)
			{
				playerAssert.Add(playerAllLocalData.assertDataList[i].GoodsId, playerAllLocalData.assertDataList[i].Num);
			}
			for (int j = 0; j < playerAllLocalData.levelDataList.Count; j++)
			{
				if (!playerLevelDic.ContainsKey(playerAllLocalData.levelDataList[j].levelId))
				{
					if (list.Contains(playerAllLocalData.levelDataList[j].levelId) && playerAllLocalData.levelDataList[j].LockState > 0)
					{
						playerAllLocalData.levelDataList[j].SetLockState(-1);
					}
					playerLevelDic.Add(playerAllLocalData.levelDataList[j].levelId, playerAllLocalData.levelDataList[j]);
				}
			}
		}
		playerAllLocalData.SaveToLocal();
		playerLevelTargetData.Init(IsCanOpenLevelTarget());
		LaunchSetting();
		InitPlayerTimer();
		TutorialManager.Instance.Init();
		InitPlayerBufferData();
		InitMessionForOldVersion();
		if (TutorialManager.Instance.IsTutorialStageFinish(TutorialStageId.STAGE_INSTRUMENT_CALL))
		{
			StartProductGoods();
		}
		IsInited = true;
	}

	private void InitDataFromLocal()
	{
		InitLevelMetaTableData();
		playerAllLocalData.InitFromLocal();
		PlayerMotivateAwardData.Init();
		PlayerCommonData.Init();
		PlayerLocalLevelSeriesData.Init();
		PlayerLocalVideoAwardData.Load();
		CoolPlayPagageData.Init();
		m_LuckTurnLocalData.Init();
		MonoSingleton<GameTools>.Instacne.InitDataFromTable();
		if (MonoSingleton<NetWorkManager>.Instacne.IsSynNetwork)
		{
			changeDataBuffer.InitFromLocal();
			changeLevelDataBuffer.InitFromLocal();
		}
		InitLastEnterLevelData();
		SequenceLoginData.Init();
		InitPropertyDataFromLocal();
		InitInstrucmentFromLocal();
		InitMessionDataFromLocal();
	}

	private void InitPlayerTimer()
	{
		if (GetPlayGoodsNum(GameCommon.EVERY_DAY_GIVE_POWER) > 0.0)
		{
			IActiveTimeController activeTimeController = new ActiveGivePowerController();
			activeTimeController.Init(DateTime.Now);
			PlayerActiveTimeDic.Add(GameCommon.EVERY_DAY_GIVE_POWER, activeTimeController);
		}
	}

	private void InitPlayerBufferData()
	{
		PlayerBufferData item = new PlayerBufferData(GameCommon.START_FREE_SHIELD);
		m_bufferList.Add(item);
		item = new PlayerBufferData(GameCommon.EVERY_DAY_GIVE_POWER);
		m_bufferList.Add(item);
		item = new PlayerBufferData(GameCommon.ORIGIN_REBIRTH_FREE);
		m_bufferList.Add(item);
		item = new PlayerBufferData(GameCommon.GuideLine);
		m_bufferList.Add(item);
		item = new PlayerBufferData(GameCommon.REMOVE_AD_TIME);
		m_bufferList.Add(item);
	}

	public bool BufferIsEnable(int id)
	{
		PlayerBufferData playerBufferDataByBufferID = GetPlayerBufferDataByBufferID(id);
		return BufferIsEnable(playerBufferDataByBufferID);
	}

	public bool BufferIsEnable(PlayerBufferData data)
	{
		if (data != null)
		{
			return data.IsEnable();
		}
		return false;
	}

	public bool BufferIsEnableByTime(int id)
	{
		PlayerBufferData playerBufferDataByBufferID = GetPlayerBufferDataByBufferID(id);
		return BufferIsEnableByTime(playerBufferDataByBufferID);
	}

	public bool BufferIsEnableByTime(PlayerBufferData data)
	{
		if (data != null)
		{
			return data.IsTimeEnable();
		}
		return false;
	}

	public bool BufferIsEnableForever(int id)
	{
		PlayerBufferData playerBufferDataByBufferID = GetPlayerBufferDataByBufferID(id);
		return BufferIsEnableForever(playerBufferDataByBufferID);
	}

	public bool BufferIsEnableForever(PlayerBufferData data)
	{
		if (data != null)
		{
			return data.IsEnableForever();
		}
		return false;
	}

	public PlayerBufferData GetPlayerBufferDataByBufferID(int bufferID)
	{
		if (m_bufferList == null)
		{
			return null;
		}
		PlayerBufferData result = null;
		for (int i = 0; i < m_bufferList.Count; i++)
		{
			if (m_bufferList[i].BufferId == bufferID)
			{
				result = m_bufferList[i];
				break;
			}
		}
		return result;
	}

	public void DeleteTempGoods()
	{
		InsideGameDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule);
		if (dataModule.needRemoveTempGoods)
		{
			dataModule.needRemoveTempGoods = false;
			bool num = BufferIsEnable(GameCommon.START_FREE_SHIELD);
			bool flag = BufferIsEnable(GameCommon.ORIGIN_REBIRTH_FREE);
			bool flag2 = BufferIsEnable(GameCommon.GuideLine);
			if (!num)
			{
				ChangePlayerTempGoodsNum(GameCommon.SHIELD_ITEM, -1);
			}
			if (!flag)
			{
				ChangePlayerTempGoodsNum(GameCommon.REBIRTH_FREE_ITEM, -1);
			}
			if (!flag2)
			{
				ChangePlayerTempGoodsNum(GameCommon.GuideLine, -1);
			}
		}
		Mod.Event.Fire(this, ChangeTempGoodsEvent.Make(false));
	}

	public void GetTempGoods(bool directActive = false)
	{
		bool num = BufferIsEnable(GameCommon.START_FREE_SHIELD);
		bool flag = BufferIsEnable(GameCommon.ORIGIN_REBIRTH_FREE);
		bool flag2 = BufferIsEnable(GameCommon.GuideLine);
		InsideGameDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule);
		if (!num)
		{
			if (directActive)
			{
				Mod.Event.FireNow(this, Mod.Reference.Acquire<PropsAddEventArgs>().Initialize(PropsName.SHIELD));
			}
			else
			{
				ChangePlayerTempGoodsNum(GameCommon.SHIELD_ITEM, 1);
			}
		}
		if (!flag)
		{
			if (directActive)
			{
				dataModule.HasUseRebirthItem = true;
				Mod.Event.FireNow(this, Mod.Reference.Acquire<PropsAddEventArgs>().Initialize(PropsName.REBIRTH));
			}
			else
			{
				ChangePlayerTempGoodsNum(GameCommon.REBIRTH_FREE_ITEM, 1);
			}
		}
		if (!flag2)
		{
			if (directActive)
			{
				dataModule.GuideLine = true;
				Mod.Event.FireNow(this, Mod.Reference.Acquire<PropsAddEventArgs>().Initialize(PropsName.PATHGUIDE));
			}
			else
			{
				ChangePlayerTempGoodsNum(GameCommon.GuideLine, 1);
			}
		}
		if (!directActive)
		{
			dataModule.needRemoveTempGoods = true;
			Mod.Event.Fire(this, ChangeTempGoodsEvent.Make(true));
		}
	}

	public void GetFreeGoodsForTutorial()
	{
		ChangePlayerGoodsNum(27, 1.0);
		ChangePlayerGoodsNum(28, 1.0);
		ChangePlayerGoodsNum(29, 1.0);
	}

	private void SetDefaultAssertNum()
	{
		if (GetPlayGoodsNum(1) <= 0.0)
		{
			SetPlayGoodsNum(1, GameCommon.firstPowerNum);
		}
		if (GetPlayGoodsNum(GameCommon.GuideLine) <= 0.0)
		{
			SetPlayGoodsNum(GameCommon.GuideLine, GameCommon.firstGuideNum);
		}
	}

	public double GetPlayGoodsNum(int goodsId)
	{
		CryVDouble value;
		if (playerAssert.TryGetValue(goodsId, out value))
		{
			return value;
		}
		return 0.0;
	}

	private void AddPlayGoodsNum(int goodsId, int num)
	{
		if (num > 0)
		{
			if (playerAssert.ContainsKey(goodsId))
			{
				Dictionary<int, CryVDouble> dictionary = playerAssert;
				dictionary[goodsId] = (double)dictionary[goodsId] + (double)num;
				playerAllLocalData.SetGoodsNum(goodsId, playerAssert[goodsId]);
			}
			else
			{
				playerAssert.Add(goodsId, num);
				playerAllLocalData.SetGoodsNum(goodsId, num);
			}
		}
	}

	private void SetPlayGoodsNum(int goodsId, double num)
	{
		if (num <= 0.0)
		{
			return;
		}
		if (playerAssert.ContainsKey(goodsId))
		{
			if ((double)playerAssert[goodsId] != num)
			{
				playerAssert[goodsId] = num;
				playerAllLocalData.SetGoodsNum(goodsId, num);
			}
		}
		else
		{
			playerAssert.Add(goodsId, num);
			playerAllLocalData.SetGoodsNum(goodsId, num);
		}
	}

	public void ChangePlayerGoodsNumPieces(int goodsId, double changeNum, AssertChangeType changeType = AssertChangeType.LEVEL_CONSUME, NetWorkSynType synType = NetWorkSynType.NORMAL, bool needFireEvent = true, bool save = true)
	{
		Goods_goodsTable goods_goodsTable = Mod.DataTable.Get<Goods_goodsTable>().Get(goodsId);
		if (goods_goodsTable.FullGoodsId == -1)
		{
			ChangePlayerGoodsNum(goodsId, changeNum, changeType, synType, needFireEvent, save);
			return;
		}
		double playGoodsNum = GetPlayGoodsNum(goodsId);
		double num = playGoodsNum + changeNum;
		Goods_goodsTable goods_goodsTable2 = Mod.DataTable.Get<Goods_goodsTable>().Get(goods_goodsTable.FullGoodsId);
		int num2 = (int)(num / (double)goods_goodsTable2.PartsNum);
		if (num2 != 0)
		{
			ChangePlayerGoodsNum(goods_goodsTable.FullGoodsId, num2, changeType, synType, needFireEvent, save);
		}
		double num3 = num - (double)(num2 * goods_goodsTable2.PartsNum) - playGoodsNum;
		if (num3 != 0.0)
		{
			ChangePlayerGoodsNum(goodsId, num3, changeType, synType, needFireEvent, save);
		}
	}

	public void ChangePlayerGoodsNum(int goodsId, double changeNum, AssertChangeType changeType = AssertChangeType.LEVEL_CONSUME, NetWorkSynType synType = NetWorkSynType.IMEDIATELY, bool needFireEvent = true, bool save = true)
	{
		if (ApplicationQuit || changeNum == 0.0)
		{
			return;
		}
		bool flag = false;
		Goods_goodsTable goods_goodsTable = Mod.DataTable.Get<Goods_goodsTable>()[goodsId];
		int num = 0;
		if (goods_goodsTable != null && goods_goodsTable.FunctionType == 1)
		{
			num = (int)((double)(goods_goodsTable.FunctionNum * 60 * 1000) * changeNum);
			PlayerRecordData.AddNoConsumePowerTime(num);
			Mod.Event.FireNow(this, Mod.Reference.Acquire<NoConsumePowerTimeChanged>().Initialize(num));
			return;
		}
		if (goods_goodsTable != null && goods_goodsTable.FunctionType == 2)
		{
			num = (int)((double)(goods_goodsTable.FunctionNum * 60 * 1000) * changeNum);
			m_bufferList.Find((PlayerBufferData x) => x.BufferId == GameCommon.START_FREE_SHIELD).AddBufferTime(num);
			Mod.Event.FireNow(this, Mod.Reference.Acquire<BufferTimeChanged>().Initialize(num));
			return;
		}
		if (goods_goodsTable != null && goods_goodsTable.FunctionType == 3)
		{
			num = (int)((double)(goods_goodsTable.FunctionNum * 60 * 1000) * changeNum);
			m_bufferList.Find((PlayerBufferData x) => x.BufferId == GameCommon.ORIGIN_REBIRTH_FREE).AddBufferTime(num);
			Mod.Event.FireNow(this, Mod.Reference.Acquire<BufferTimeChanged>().Initialize(num));
			return;
		}
		if (goods_goodsTable != null && goods_goodsTable.FunctionType == 4)
		{
			num = (int)((double)(goods_goodsTable.FunctionNum * 60 * 1000) * changeNum);
			m_bufferList.Find((PlayerBufferData x) => x.BufferId == GameCommon.GuideLine).AddBufferTime(num);
			Mod.Event.FireNow(this, Mod.Reference.Acquire<BufferTimeChanged>().Initialize(num));
			return;
		}
		if (goods_goodsTable != null && goods_goodsTable.FunctionType == 5)
		{
			num = (int)((double)(goods_goodsTable.FunctionNum * 60 * 1000) * changeNum);
			m_bufferList.Find((PlayerBufferData x) => x.BufferId == GameCommon.REMOVE_AD_TIME).AddBufferTime(num);
			Mod.Event.FireNow(this, Mod.Reference.Acquire<BufferTimeChanged>().Initialize(num));
			return;
		}
		if (playerAssert.ContainsKey(goodsId))
		{
			Dictionary<int, CryVDouble> dictionary = playerAssert;
			dictionary[goodsId] = (double)dictionary[goodsId] + changeNum;
			if ((double)playerAssert[goodsId] < 0.0)
			{
				playerAssert[goodsId] = 0.0;
			}
		}
		else
		{
			if (changeNum < 0.0)
			{
				changeNum = 0.0;
			}
			playerAssert.Add(goodsId, changeNum);
			flag = true;
		}
		playerAllLocalData.SetGoodsNum(goodsId, playerAssert[goodsId], save);
		if (needFireEvent)
		{
			Mod.Event.FireNow(this, Mod.Reference.Acquire<GameGoodsNumChangeEventArgs>().Initialize(goodsId, changeNum, changeType));
		}
		if (MonoSingleton<NetWorkManager>.Instacne.IsSynNetwork)
		{
			switch (synType)
			{
			case NetWorkSynType.NORMAL:
				changeDataBuffer.ChangeGoods(goodsId, playerAssert[goodsId]);
				changeDataBuffer.SynChangeData();
				break;
			case NetWorkSynType.IMEDIATELY:
				changeDataBuffer.ChangeGoods(goodsId, playerAssert[goodsId]);
				changeDataBuffer.SynChangeData(true);
				break;
			}
		}
		if (flag && goodsId == GameCommon.EVERY_DAY_GIVE_POWER)
		{
			ActiveGivePowerController activeGivePowerController = new ActiveGivePowerController();
			((IActiveTimeController)activeGivePowerController).Init(DateTime.Now);
			((IActiveTimeController)activeGivePowerController).Reward();
			((IActiveTimeController)activeGivePowerController).ResetTime();
		}
	}

	public void ChangePlayerTempGoodsNum(int goodsId, int changeNum)
	{
		if (changeNum == 0)
		{
			return;
		}
		if (playerAssert.ContainsKey(goodsId))
		{
			Dictionary<int, CryVDouble> dictionary = playerAssert;
			dictionary[goodsId] = (double)dictionary[goodsId] + (double)changeNum;
			if ((double)playerAssert[goodsId] < 0.0)
			{
				playerAssert[goodsId] = 0.0;
			}
		}
		else
		{
			if (changeNum < 0)
			{
				changeNum = 0;
			}
			playerAssert.Add(goodsId, changeNum);
		}
	}

	public int GetPlayerTotalDiamondsNum()
	{
		Dictionary<int, PlayerLocalLevelData>.Enumerator enumerator = playerLevelDic.GetEnumerator();
		int num = 0;
		while (enumerator.MoveNext())
		{
			num += enumerator.Current.Value.MaxDiamondNum;
		}
		return num;
	}

	public int GetPlayerTotalCrownsNum()
	{
		Dictionary<int, PlayerLocalLevelData>.Enumerator enumerator = playerLevelDic.GetEnumerator();
		int num = 0;
		while (enumerator.MoveNext())
		{
			num += enumerator.Current.Value.MaxCrownNum;
		}
		return num;
	}

	public int GetLevelMaxDiamond(int levelId)
	{
		int result = 0;
		PlayerLocalLevelData playerLevelData = GetPlayerLevelData(levelId);
		if (playerLevelData != null)
		{
			result = playerLevelData.MaxDiamondNum;
		}
		return result;
	}

	public bool IsInNewPlayerProtectedStage()
	{
		return !TutorialManager.Instance.IsForceTutorialEnd();
	}

	public void DealBuyResult(bool isSuccess, int shopId)
	{
		if (isSuccess && shopId == GameCommon.COOLPLAY_PACKAGE)
		{
			CoolPlayPagageData.AddPacakge();
		}
	}

	private void InitInstrucmentFromLocal()
	{
		m_playerInstrumentLocalData.InitFromLocal();
	}

	private void SetLastProductTimeStamp()
	{
		EncodeConfig.setConfig(values: ServerTime.ToString(), key: PlayerLocalDatakey.PLAYERLOCALINSTRUMENTPRODUCTSTAMPDATA);
	}

	private long GetLastProductTimeStamp()
	{
		long result = 0L;
		string config = EncodeConfig.getConfig(PlayerLocalDatakey.PLAYERLOCALINSTRUMENTPRODUCTSTAMPDATA);
		if (string.IsNullOrEmpty(config))
		{
			result = ServerTime;
		}
		else if (!long.TryParse(config, out result))
		{
			result = ServerTime;
		}
		return result;
	}

	private void OnlineProduct(bool isNormal)
	{
		if (ApplicationQuit)
		{
			return;
		}
		float num = 0f;
		num = ((!isNormal) ? GameCommon.instrumentProductSpeeedUpFrequency : GameCommon.instrumentProductFrequency);
		double currentProductReputaionSpeed = GetCurrentProductReputaionSpeed();
		if (currentProductReputaionSpeed != m_lastProductSpeed)
		{
			m_lastProductSpeed = currentProductReputaionSpeed;
			Mod.Event.FireNow(this, Mod.Reference.Acquire<InstrumentPropertyChangeEventArgs>().Initialize(new List<int>(), InstrumentPropertyType.PRODUCT_SPEED));
		}
		double changeNum = (double)num * currentProductReputaionSpeed;
		ChangePlayerGoodsNum(GameCommon.REPUTATION_ID, changeNum, AssertChangeType.GOODS_PRODUCT, NetWorkSynType.NORMAL);
		m_InstrumentProductCount++;
		if (m_InstrumentProductCount % 10 == 0)
		{
			SetLastProductTimeStamp();
		}
		if (isNormal)
		{
			if (IsProductSpeedUpGoing())
			{
				TimerHeap.DelTimer(m_ProductTimerId);
				m_ProductTimerId = TimerHeap.AddTimer((uint)(GameCommon.instrumentProductSpeeedUpFrequency * 1000f), (uint)(GameCommon.instrumentProductSpeeedUpFrequency * 1000f), OnlineProduct, false);
			}
		}
		else if (!IsProductSpeedUpGoing())
		{
			TimerHeap.DelTimer(m_ProductTimerId);
			m_ProductTimerId = TimerHeap.AddTimer((uint)(GameCommon.instrumentProductFrequency * 1000f), (uint)(GameCommon.instrumentProductFrequency * 1000f), OnlineProduct, true);
		}
	}

	public double OffLineProductReputation()
	{
		long lastProductTimeStamp = GetLastProductTimeStamp();
		long num = ServerTime - lastProductTimeStamp;
		double num2 = 0.0;
		int num3 = GameCommon.offlineProductMinTime * 1000;
		if (num > num3)
		{
			int num4 = (int)((float)num * 0.001f);
			if (num4 > GameCommon.offlineProductReputationMaxTime)
			{
				num4 = GameCommon.offlineProductReputationMaxTime;
			}
			num2 = ComputerProductReputationByTime(num4);
		}
		return num2 + OffLineSpeedUpProductReputaion() * (double)(GameCommon.instrumentAdProductFactor - 1);
	}

	private double OffLineSpeedUpProductReputaion(int factor = 1)
	{
		long lastProductTimeStamp = GetLastProductTimeStamp();
		long num = GetProductSpeedUpEndTimeStamp() - lastProductTimeStamp;
		double num2 = 0.0;
		int num3 = GameCommon.offlineProductMinTime * 1000;
		if (num > num3)
		{
			int num4 = (int)((float)num * 0.001f);
			if (num4 > GameCommon.offlineProductReputationMaxTime)
			{
				num4 = GameCommon.offlineProductReputationMaxTime;
			}
			num2 = ComputerProductReputationByTime(num4);
		}
		return num2 * (double)factor;
	}

	public double OffLineProductionGold()
	{
		long lastProductTimeStamp = GetLastProductTimeStamp();
		long num = ServerTime - lastProductTimeStamp;
		int num2 = GameCommon.offlineProductMinTime * 1000;
		if (num > num2)
		{
			int num3 = GameCommon.offlineProductGoldMaxTime * 1000;
			if (num > num3)
			{
				num = num3;
			}
			double num4 = GetOffLineProductionGoldByTime(num);
			if (PlayerIsHadSpecialStarAbility(9))
			{
				num4 *= (double)GetStarLevelAbilityNum(9);
			}
			return num4;
		}
		return 0.0;
	}

	public double GetOffLineProductionGoldByTime(long time)
	{
		if (time <= 0)
		{
			return 0.0;
		}
		PlayerLocalInstrumentData baton = GetBaton();
		double num = (double)baton.GetBatonOffLineProductBase(GetPlayerStarLevel(), baton.Level) * (double)time * 0.0010000000474974513;
		float num2 = (float)GetBatonProductAdd() * 0.01f + 1f;
		return num * (double)num2;
	}

	public int GetOffLineTime()
	{
		long lastProductTimeStamp = GetLastProductTimeStamp();
		return (int)((float)(ServerTime - lastProductTimeStamp) * 0.001f);
	}

	private long GetProductSpeedUpEndTimeStamp()
	{
		long result = -1L;
		string config = EncodeConfig.getConfig(PlayerLocalDatakey.INSTRUMENTPRODUCTSPEEDUPTIMESTAMP);
		if (!string.IsNullOrEmpty(config) && !long.TryParse(config, out result))
		{
			result = ServerTime;
		}
		return result;
	}

	public void AddProductSpeedUpTime(int speedUpTime)
	{
		long num = GetProductSpeedUpEndTimeStamp();
		if (num < 0 || num < ServerTime)
		{
			num = ServerTime;
		}
		num = (m_speedUpEndTimeStamp = num + speedUpTime * 1000);
		EncodeConfig.setConfig(PlayerLocalDatakey.INSTRUMENTPRODUCTSPEEDUPTIMESTAMP, num.ToString());
	}

	public int ProductSpeedUpLeftTime()
	{
		int num = (int)((float)(GetProductSpeedUpEndTimeStamp() - ServerTime) * 0.001f);
		if (num < 0)
		{
			num = 0;
		}
		return num;
	}

	public bool IsProductSpeedUpGoing()
	{
		return m_speedUpEndTimeStamp > ServerTime;
	}

	public float ProductSpeedUpFactor()
	{
		return 2f;
	}

	public void StartProductGoods()
	{
		m_offLineTime = GetOffLineTime();
		m_offLineProductReputationNum = OffLineProductReputation();
		m_offLineProductGoldNum = OffLineProductionGold();
		SetLastProductTimeStamp();
		m_speedUpEndTimeStamp = GetProductSpeedUpEndTimeStamp();
		m_lastProductSpeed = GetCurrentProductReputaionSpeed();
		if (IsProductSpeedUpGoing())
		{
			m_ProductTimerId = TimerHeap.AddTimer((uint)(GameCommon.instrumentProductSpeeedUpFrequency * 1000f), (uint)(GameCommon.instrumentProductSpeeedUpFrequency * 1000f), OnlineProduct, false);
		}
		else
		{
			m_ProductTimerId = TimerHeap.AddTimer((uint)(GameCommon.instrumentProductFrequency * 1000f), (uint)(GameCommon.instrumentProductFrequency * 1000f), OnlineProduct, true);
		}
	}

	public double ComputerProductReputationByTime(int second)
	{
		double num = 0.0;
		PlayerLocalInstrumentData playerLocalInstrumentData = null;
		List<PlayerLocalInstrumentData> allUnLockInstuments = GetAllUnLockInstuments();
		for (int i = 0; i < allUnLockInstuments.Count; i++)
		{
			playerLocalInstrumentData = m_playerInstrumentLocalData.m_InstrumentList[i];
			if ((int)playerLocalInstrumentData.LockState == -1 && !playerLocalInstrumentData.IsBaton())
			{
				num += m_playerInstrumentLocalData.m_InstrumentList[i].ComputerProductNumByTime(second, m_StarLevel, playerLocalInstrumentData.Level);
			}
		}
		return num;
	}

	public double GetCurrentProductReputaionSpeed()
	{
		return ComputerProductReputationByTime(1);
	}

	public float GetAllInstrumentTotalLevelPercentAdd()
	{
		return (float)GetAllInstrumentTotalLevel() * 0.001f;
	}

	private int GetAllInstrumentTotalLevel()
	{
		int num = 0;
		List<PlayerLocalInstrumentData> allUnLockInstuments = GetAllUnLockInstuments();
		for (int i = 0; i < allUnLockInstuments.Count; i++)
		{
			if (!allUnLockInstuments[i].IsBaton())
			{
				num += (int)allUnLockInstuments[i].Level;
			}
		}
		return num;
	}

	public float GetAllInstrumentStarLevelAbilityReputationPercentAdd()
	{
		float result = 0f;
		if (PlayerIsHadSpecialStarAbility(3))
		{
			result = (float)GetStarLevelAbilityNum(3) * 0.01f;
		}
		return result;
	}

	public PlayerLocalInstrumentData GetInstrumentDataById(int id)
	{
		return m_playerInstrumentLocalData.GetInstrumentDataById(id);
	}

	public UpInstrumentResultState UpInstrumentLevel(int id, int levelCount = 1)
	{
		UpInstrumentResultState num = m_playerInstrumentLocalData.UpLevel(id, levelCount);
		if (num == UpInstrumentResultState.SUCCESS)
		{
			PlayerLocalInstrumentData instrumentDataById = GetInstrumentDataById(id);
			SynInstrumentPropertyToServer(new List<PlayerLocalInstrumentData> { instrumentDataById });
			Mod.Event.FireNow(this, Mod.Reference.Acquire<InstrumentPropertyChangeEventArgs>().Initialize(new List<int> { id }, InstrumentPropertyType.LEVEL));
		}
		return num;
	}

	public UpInstrumentResultState ForceUpInstrumentLevel(int id, int levelCount = 1)
	{
		UpInstrumentResultState num = m_playerInstrumentLocalData.ForceUpLevel(id, levelCount);
		if (num == UpInstrumentResultState.SUCCESS)
		{
			PlayerLocalInstrumentData instrumentDataById = GetInstrumentDataById(id);
			SynInstrumentPropertyToServer(new List<PlayerLocalInstrumentData> { instrumentDataById });
			Mod.Event.FireNow(this, Mod.Reference.Acquire<InstrumentPropertyChangeEventArgs>().Initialize(new List<int> { id }, InstrumentPropertyType.LEVEL));
		}
		return num;
	}

	public PlayInstrumentResultState PlayInstrument(int instrumentId, out double productCount)
	{
		PlayInstrumentResultState num = PlayerInstrumentLocalData.Play(instrumentId, out productCount);
		if (num == PlayInstrumentResultState.SUCCESS)
		{
			Mod.Event.FireNow(this, Mod.Reference.Acquire<InstrumentPropertyChangeEventArgs>().Initialize(new List<int> { instrumentId }, InstrumentPropertyType.PLAY_COUNT));
		}
		return num;
	}

	public int GetTotalPlayCount()
	{
		List<PlayerLocalInstrumentData> allUnLockInstuments = GetAllUnLockInstuments();
		int num = 0;
		for (int i = 0; i < allUnLockInstuments.Count; i++)
		{
			num += allUnLockInstuments[i].m_PlayCount;
		}
		return num;
	}

	public PlayerLocalInstrumentData GetBaton()
	{
		return GetInstrumentDataById(1);
	}

	public bool IsBaton(int id)
	{
		return id == 1;
	}

	public int GetBatonProductAdd()
	{
		PlayerLocalInstrumentData baton = GetBaton();
		if (baton != null)
		{
			return baton.GetProductGoldPercentAdd(m_StarLevel, baton.Level) + GetBatonStarLevelAbilityGoldPercentAdd();
		}
		return 0;
	}

	private int GetBatonStarLevelAbilityGoldPercentAdd()
	{
		int result = 0;
		if (PlayerIsHadSpecialStarAbility(5))
		{
			result = GetStarLevelAbilityNum(5);
		}
		return result;
	}

	public void PutOnInstrument(int id, int slotIndex)
	{
		if (m_playerInstrumentLocalData.PutOnInstrument(id, slotIndex))
		{
			PlayerLocalInstrumentData instrumentDataById = GetInstrumentDataById(id);
			SynInstrumentPropertyToServer(new List<PlayerLocalInstrumentData> { instrumentDataById });
			Mod.Event.FireNow(this, Mod.Reference.Acquire<InstrumentPropertyChangeEventArgs>().Initialize(new List<int> { id }, InstrumentPropertyType.PUTON_STATE));
		}
	}

	public void PutOffInstrument(int id)
	{
		if (m_playerInstrumentLocalData.PutOffInstrument(id))
		{
			PlayerLocalInstrumentData instrumentDataById = GetInstrumentDataById(id);
			SynInstrumentPropertyToServer(new List<PlayerLocalInstrumentData> { instrumentDataById });
			Mod.Event.FireNow(this, Mod.Reference.Acquire<InstrumentPropertyChangeEventArgs>().Initialize(new List<int> { id }, InstrumentPropertyType.PUTON_STATE));
		}
	}

	public void UnlockInstrumentByStarLevel(int starLevel, int level)
	{
		List<PlayerLocalInstrumentData> allInstrumentDataList = GetAllInstrumentDataList();
		List<int> list = new List<int>();
		List<PlayerLocalInstrumentData> list2 = new List<PlayerLocalInstrumentData>();
		int num = -1;
		int num2 = -1;
		for (int i = 0; i < allInstrumentDataList.Count; i++)
		{
			if (allInstrumentDataList[i].IsBaton())
			{
				continue;
			}
			num = allInstrumentDataList[i].GetUnLockNeedStarLevel();
			num2 = allInstrumentDataList[i].GetUnLockNeedLevel();
			if ((int)allInstrumentDataList[i].LockState > 0)
			{
				if (starLevel >= num && level >= num2)
				{
					m_playerInstrumentLocalData.UnlockInstrument(allInstrumentDataList[i].m_Id);
					list2.Add(allInstrumentDataList[i]);
					list.Add(allInstrumentDataList[i].m_Id);
				}
			}
			else if (starLevel < num || level < num2)
			{
				m_playerInstrumentLocalData.LockInstrument(allInstrumentDataList[i].m_Id);
				list2.Add(allInstrumentDataList[i]);
				list.Add(allInstrumentDataList[i].m_Id);
			}
		}
		if (list.Count > 0)
		{
			Mod.Event.FireNow(this, Mod.Reference.Acquire<InstrumentPropertyChangeEventArgs>().Initialize(list, InstrumentPropertyType.LOCK_STATE));
			SynInstrumentPropertyToServer(list2);
		}
	}

	public List<PlayerLocalInstrumentData> GetAllUnLockInstuments()
	{
		return m_playerInstrumentLocalData.GetAllUnLockInstuments();
	}

	public List<PlayerLocalInstrumentData> GetAllPlayingInstruments()
	{
		new List<PlayerLocalInstrumentData>();
		return GetAllUnLockInstuments().FindAll((PlayerLocalInstrumentData x) => (int)x.PlayState > 0);
	}

	public void ResetAllUnLockInstrumentLevel(int level = 1)
	{
		m_playerInstrumentLocalData.ResetAllUnlockInstrumentLevel();
		List<PlayerLocalInstrumentData> allUnLockInstuments = GetAllUnLockInstuments();
		SynInstrumentPropertyToServer(allUnLockInstuments);
	}

	public int GetInstrumentIconId(int instrumentId)
	{
		return GetInstrumentDataById(instrumentId).IconId;
	}

	public int InstrumentCanUpMaxLevel()
	{
		return GetPlayerLevel() + GameCommon.instrumentLimitLevel;
	}

	public List<PlayerLocalInstrumentData> GetAllInstrumentDataList()
	{
		return m_playerInstrumentLocalData.m_InstrumentList;
	}

	public int AllUnLockInstrumentCurrentMaxLevel()
	{
		int num = 1;
		List<PlayerLocalInstrumentData> allUnLockInstuments = GetAllUnLockInstuments();
		for (int i = 0; i < allUnLockInstuments.Count; i++)
		{
			if ((int)allUnLockInstuments[i].Level > num)
			{
				num = allUnLockInstuments[i].Level;
			}
		}
		return num;
	}

	public float InstrumentProductAdditonalPercent()
	{
		return Instance.GetAllInstrumentTotalLevelPercentAdd() + Instance.GetAllInstrumentStarLevelAbilityReputationPercentAdd();
	}

	public int GetUnLockInstrmentCount(bool containBaton = false)
	{
		int num = GetAllUnLockInstuments().Count;
		if (!containBaton)
		{
			num--;
		}
		if (num <= 0)
		{
			return 0;
		}
		return num;
	}

	private void SynInstrumentPropertyToServer(List<PlayerLocalInstrumentData> instrumentDataList, NetWorkSynType type = NetWorkSynType.NORMAL)
	{
		if (instrumentDataList != null && instrumentDataList.Count > 0 && MonoSingleton<NetWorkManager>.Instacne.IsSynNetwork)
		{
			switch (type)
			{
			case NetWorkSynType.NORMAL:
				m_SynInstrumentBuffer.ChangeInstrumentData(instrumentDataList);
				m_SynInstrumentBuffer.SynInstrumentData(false);
				break;
			case NetWorkSynType.IMEDIATELY:
				m_SynInstrumentBuffer.ChangeInstrumentData(instrumentDataList);
				m_SynInstrumentBuffer.SynInstrumentData(true);
				break;
			case NetWorkSynType.NONE:
				break;
			}
		}
	}

	private void InitLevelMetaTableData()
	{
		LevelSeries_table[] records = Mod.DataTable.Get<LevelSeries_table>().Records;
		int num = -1;
		List<int> list = new List<int>();
		List<int> list2 = new List<int>();
		Levels_levelTable levels_levelTable = null;
		for (int i = 0; i < records.Length; i++)
		{
			num = records[i].Id;
			LevelSeries_table levelSeries_table = Mod.DataTable.Get<LevelSeries_table>()[num];
			if (levelSeries_table == null)
			{
				continue;
			}
			string[] array = levelSeries_table.LevelChapters.Split('|');
			list.Clear();
			int result = -1;
			for (int j = 0; j < array.Length; j++)
			{
				if (int.TryParse(array[j], out result))
				{
					list.Add(result);
				}
			}
			int result2 = -1;
			for (int k = 0; k < list.Count; k++)
			{
				result = list[k];
				list2.Clear();
				LevelChapters_table levelChapters_table = Mod.DataTable.Get<LevelChapters_table>()[result];
				if (levelChapters_table != null)
				{
					string[] array2 = levelChapters_table.Sections.Split('|');
					for (int l = 0; l < array2.Length; l++)
					{
						if (int.TryParse(array2[l], out result2))
						{
							list2.Add(result2);
						}
					}
				}
				for (int m = 0; m < list2.Count; m++)
				{
					result2 = list2[m];
					LevelSections_table levelSections_table = Mod.DataTable.Get<LevelSections_table>()[result2];
					if (levelSections_table == null)
					{
						continue;
					}
					string[] array3 = levelSections_table.Levels.Split('|');
					int result3 = -1;
					for (int n = 0; n < array3.Length; n++)
					{
						if (int.TryParse(array3[n], out result3))
						{
							levels_levelTable = Mod.DataTable.Get<Levels_levelTable>()[result3];
							if (levels_levelTable != null && levels_levelTable.IsShow == 1)
							{
								LevelMetaTableData item = new LevelMetaTableData(num, result, result2, result3, n, levels_levelTable.IsShow > 0);
								m_golableLevelData.Add(item);
							}
						}
					}
				}
			}
		}
	}

	private void InitLastEnterLevelData()
	{
		string config = EncodeConfig.getConfig(PlayerLocalDatakey.PLAYERLASTENTELEVELDATA);
		if (config.Length > 0)
		{
			JsonUtility.FromJsonOverwrite(config, LastEndterLevelData);
		}
	}

	public void SaveLastEnterLevelData()
	{
		string values = JsonUtility.ToJson(LastEndterLevelData);
		EncodeConfig.setConfig(PlayerLocalDatakey.PLAYERLASTENTELEVELDATA, values);
	}

	public int GetLevelAwardCount()
	{
		int num = 0;
		if (!TutorialManager.Instance.IsTutorialStageFinish(TutorialStageId.STAGE_FIRST_LEVEL))
		{
			return 0;
		}
		num++;
		foreach (PlayerLocalLevelData value in playerLevelDic.Values)
		{
			if (value.IsPerfect == 1)
			{
				num += 2;
			}
			else if (value.MaxProgress >= 100)
			{
				num++;
			}
		}
		return num;
	}

	public PlayerLocalLevelData GetPlayerLevelData(int levelId)
	{
		PlayerLocalLevelData value = null;
		if (playerLevelDic.TryGetValue(levelId, out value))
		{
			if (value == null)
			{
				value = new PlayerLocalLevelData(levelId);
			}
		}
		else
		{
			value = new PlayerLocalLevelData(levelId);
			playerLevelDic.Add(levelId, value);
		}
		return value;
	}

	public void SetLevelMaxProgressData(int levelId, int percentage, int maxDiamondsCollected, int maxCrownsCollected, int[] crownCollect, int[] diamondsCollect)
	{
		PlayerLocalLevelData value = null;
		bool flag = false;
		if (playerLevelDic.TryGetValue(levelId, out value))
		{
			if (value != null)
			{
				if (value.MaxProgress < 100 && percentage >= 100 && levelId != 10000)
				{
					ChangePlayerGoodsNum(6, GameCommon.firstFinishLevelGetKeysMum);
					flag = true;
				}
				value.SetLevelMaxProgressData(percentage, maxDiamondsCollected, maxCrownsCollected, crownCollect, diamondsCollect);
				changeLevelDataBuffer.ChangeLevelData(levelId, value);
				changeLevelDataBuffer.SynLevelData(false);
			}
			else
			{
				value = new PlayerLocalLevelData(levelId);
				if (value.MaxProgress < 100 && percentage >= 100)
				{
					ChangePlayerGoodsNum(6, GameCommon.firstFinishLevelGetKeysMum);
					flag = true;
				}
				value.SetLevelMaxProgressData(percentage, maxDiamondsCollected, maxCrownsCollected, crownCollect, diamondsCollect);
				changeLevelDataBuffer.ChangeLevelData(levelId, value);
				changeLevelDataBuffer.SynLevelData(false);
			}
		}
		else
		{
			value = new PlayerLocalLevelData(levelId);
			if (value.MaxProgress < 100 && percentage >= 100)
			{
				ChangePlayerGoodsNum(6, GameCommon.firstFinishLevelGetKeysMum);
				flag = true;
			}
			value.SetLevelMaxProgressData(percentage, maxDiamondsCollected, maxCrownsCollected, crownCollect, diamondsCollect);
			playerLevelDic.Add(levelId, value);
			changeLevelDataBuffer.ChangeLevelData(levelId, value);
			changeLevelDataBuffer.SynLevelData(false);
		}
		playerAllLocalData.SetLevelMaxProgressData(levelId, percentage, maxDiamondsCollected, maxCrownsCollected, crownCollect, diamondsCollect);
		if (levelId == 1 && percentage >= GameCommon.levelTargetOpenFirstLevelPercent && !PlayerLevelTargetData.IsOpened())
		{
			PlayerLevelTargetData.OpenLevelTarget();
		}
		if (MonoSingleton<GameTools>.Instacne.IsFirstLanuchGameIn24Hour() && flag)
		{
			InfocUtils.Report_rollingsky2_games_gamefiller(3, levelId);
		}
	}

	public int GetLevelMaxProgress(int levelId)
	{
		int result = 0;
		PlayerLocalLevelData playerLevelData = GetPlayerLevelData(levelId);
		if (playerLevelData != null)
		{
			return playerLevelData.MaxProgress;
		}
		return result;
	}

	public int GetLevelMaxCrown(int levelId)
	{
		int result = 0;
		PlayerLocalLevelData playerLevelData = GetPlayerLevelData(levelId);
		if (playerLevelData != null)
		{
			return playerLevelData.MaxCrownNum;
		}
		return result;
	}

	public void SetLevelLockState(int levelId, int state)
	{
		PlayerLocalLevelData value = null;
		if (playerLevelDic.TryGetValue(levelId, out value))
		{
			if (value != null)
			{
				bool flag = false;
				if (value.LockState != state)
				{
					flag = true;
				}
				value.LockState = state;
				if (flag)
				{
					changeLevelDataBuffer.ChangeLevelData(levelId, value);
					changeLevelDataBuffer.SynLevelData(false);
				}
			}
			else
			{
				value = new PlayerLocalLevelData(levelId);
				value.LockState = state;
				changeLevelDataBuffer.ChangeLevelData(levelId, value);
				changeLevelDataBuffer.SynLevelData(false);
			}
		}
		else
		{
			value = new PlayerLocalLevelData(levelId);
			value.LockState = state;
			playerLevelDic.Add(levelId, value);
			changeLevelDataBuffer.ChangeLevelData(levelId, value);
			changeLevelDataBuffer.SynLevelData(false);
		}
		playerAllLocalData.SetLevelLockState(levelId, state);
		if (levelId > 1 && state < 0)
		{
			if (!PlayerLevelTargetData.IsOpened())
			{
				PlayerLevelTargetData.OpenLevelTarget();
			}
			if (GetCanShowTutorialTips())
			{
				ForbidShowTutorialTips();
			}
		}
		if (state < 0)
		{
			Mod.Event.FireNow(this, Mod.Reference.Acquire<LevelUnLockEventArgs>().Initialize(levelId));
		}
		if (MonoSingleton<GameTools>.Instacne.IsFirstLanuchGameIn24Hour() && state < 0 && Mathf.Abs(state) < 20000)
		{
			switch (levelId)
			{
			case 2:
				InfocUtils.Report_rollingsky2_games_gamefiller(15);
				break;
			case 3:
				InfocUtils.Report_rollingsky2_games_gamefiller(16);
				break;
			case 4:
				InfocUtils.Report_rollingsky2_games_gamefiller(21);
				break;
			case 5:
				InfocUtils.Report_rollingsky2_games_gamefiller(22);
				break;
			case 6:
				InfocUtils.Report_rollingsky2_games_gamefiller(23);
				break;
			}
		}
	}

	public void SetLevelTicketNum(int levelId, int num)
	{
		PlayerLocalLevelData value = null;
		if (playerLevelDic.TryGetValue(levelId, out value))
		{
			if (value != null)
			{
				value.SetTicketNum(num);
			}
		}
		else
		{
			value = new PlayerLocalLevelData(levelId);
			value.SetTicketNum(num);
			playerLevelDic.Add(levelId, value);
			changeLevelDataBuffer.ChangeLevelData(levelId, value);
			changeLevelDataBuffer.SynLevelData(false);
		}
		if (playerAllLocalData != null)
		{
			playerAllLocalData.SetLevelTicketNum(levelId, num);
		}
	}

	public void SetLevelAdEnterTime(int levelId, int lastTime)
	{
		PlayerLocalLevelData value = null;
		if (playerLevelDic.TryGetValue(levelId, out value))
		{
			if (value != null)
			{
				value.FreeEnterLevelTimeStamp = ServerTime + lastTime * 1000;
			}
		}
		else
		{
			value = new PlayerLocalLevelData(levelId);
			value.FreeEnterLevelTimeStamp = ServerTime + lastTime * 1000;
			playerLevelDic.Add(levelId, value);
		}
		if (playerAllLocalData != null)
		{
			playerAllLocalData.SetLevelAdEnterTime(levelId, lastTime);
		}
	}

	public int FirstEnterSeriesId()
	{
		int num = -1;
		int num2 = 2;
		num = ((TutorialManager.Instance.GetCurrentStageId() == TutorialStageId.STAGE_HOME_MENU) ? GameCommon.FIRST_SERIES : (GetAppLocalVersion().Equals(Application.version) ? Instance.LastEndterLevelData.SeriesId : GameCommon.newVersionShowSeriesId));
		if (num <= 0)
		{
			num = num2;
		}
		return num;
	}

	public int GetPlayerFinishLevelCount(bool exceptTutorial = true)
	{
		Dictionary<int, PlayerLocalLevelData>.Enumerator enumerator = playerLevelDic.GetEnumerator();
		int num = 0;
		while (enumerator.MoveNext())
		{
			if (enumerator.Current.Value.MaxProgress >= 100 && (!exceptTutorial || enumerator.Current.Value.levelId != 10000))
			{
				num++;
			}
		}
		return num;
	}

	public int GetPlayerFinishLevelTotalCount(bool exceptTutorial = true)
	{
		Dictionary<int, PlayerLocalLevelData>.Enumerator enumerator = playerLevelDic.GetEnumerator();
		int num = 0;
		while (enumerator.MoveNext())
		{
			if (!exceptTutorial || enumerator.Current.Value.levelId != 10000)
			{
				num += enumerator.Current.Value.FinishLevelCount;
			}
		}
		return num;
	}

	public int GetPlayerFinishLevelTotalCountByLevelDifficulty(LevelDifficulty difficulty, bool exceptTutorial = true)
	{
		Dictionary<int, PlayerLocalLevelData>.Enumerator enumerator = playerLevelDic.GetEnumerator();
		int num = 0;
		int num2 = 0;
		while (enumerator.MoveNext())
		{
			num2 = enumerator.Current.Value.levelId;
			if ((!exceptTutorial || enumerator.Current.Value.levelId != 10000) && difficulty == GetLevelDifficulty(num2))
			{
				num += enumerator.Current.Value.FinishLevelCount;
			}
		}
		return num;
	}

	public int GetPlayerPlayLevelTotalCount(bool exceptTutorial = true)
	{
		Dictionary<int, PlayerLocalLevelData>.Enumerator enumerator = playerLevelDic.GetEnumerator();
		int num = 0;
		while (enumerator.MoveNext())
		{
			if (!exceptTutorial || enumerator.Current.Value.levelId != 10000)
			{
				num += enumerator.Current.Value.PlayLevelCount;
			}
		}
		return num;
	}

	public int GetPlayerAllLevelTotalDiamondsCount(bool exceptTutorial = true)
	{
		Dictionary<int, PlayerLocalLevelData>.Enumerator enumerator = playerLevelDic.GetEnumerator();
		int num = 0;
		while (enumerator.MoveNext())
		{
			if (!exceptTutorial || enumerator.Current.Value.levelId != 10000)
			{
				num += enumerator.Current.Value.TotalDiamondsCount;
			}
		}
		return num;
	}

	public int GetPlayerAllLevelTotalCrownsCount(bool exceptTutorial = true)
	{
		Dictionary<int, PlayerLocalLevelData>.Enumerator enumerator = playerLevelDic.GetEnumerator();
		int num = 0;
		while (enumerator.MoveNext())
		{
			if (!exceptTutorial || enumerator.Current.Value.levelId != 10000)
			{
				num += enumerator.Current.Value.TotalCrownsCount;
			}
		}
		return num;
	}

	public int GetPlayerAllLevelTotalDiamondsMaxCount(bool exceptTutorial = true)
	{
		Dictionary<int, PlayerLocalLevelData>.Enumerator enumerator = playerLevelDic.GetEnumerator();
		int num = 0;
		while (enumerator.MoveNext())
		{
			if (!exceptTutorial || enumerator.Current.Value.levelId != 10000)
			{
				num += enumerator.Current.Value.GetCollectedDiamoundCount();
			}
		}
		return num;
	}

	public int GetPlayerAllLevelTotalCrownsMaxCount(bool exceptTutorial = true)
	{
		Dictionary<int, PlayerLocalLevelData>.Enumerator enumerator = playerLevelDic.GetEnumerator();
		int num = 0;
		while (enumerator.MoveNext())
		{
			if (!exceptTutorial || enumerator.Current.Value.levelId != 10000)
			{
				num += enumerator.Current.Value.GetCollectedCrownCount();
			}
		}
		return num;
	}

	public LevelDifficulty GetLevelDifficulty(int levelId)
	{
		List<LevelMetaTableData>.Enumerator enumerator = GloableLevelLableData.GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (enumerator.Current.LevelId == levelId)
			{
				return (LevelDifficulty)enumerator.Current.DifficultDrgee;
			}
		}
		return LevelDifficulty.NONE;
	}

	public bool IsCanOpenLevelTarget()
	{
		bool result = false;
		Dictionary<int, PlayerLocalLevelData>.Enumerator enumerator = playerLevelDic.GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (enumerator.Current.Value.levelId == 1 && enumerator.Current.Value.MaxProgress >= GameCommon.levelTargetOpenFirstLevelPercent)
			{
				result = true;
				break;
			}
			if (enumerator.Current.Value.levelId > 1 && enumerator.Current.Value.LockState < 0)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	public bool IsHadUnLockLevelExceptLevelOne()
	{
		bool result = false;
		Dictionary<int, PlayerLocalLevelData>.Enumerator enumerator = playerLevelDic.GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (enumerator.Current.Value.levelId > 1 && enumerator.Current.Value.LockState < 0)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	public void SetLevelBuyOutRebirth(int levelId, int isBuy)
	{
		PlayerLocalLevelData value = null;
		if (playerLevelDic.TryGetValue(levelId, out value))
		{
			if (value != null)
			{
				bool flag = false;
				if (value.GetBuyOutRebirth() != isBuy)
				{
					flag = true;
				}
				value.SetBuyOutRebirth(isBuy);
				if (flag)
				{
					changeLevelDataBuffer.ChangeLevelData(levelId, value);
					changeLevelDataBuffer.SynLevelData(false);
				}
			}
			else
			{
				value = new PlayerLocalLevelData(levelId);
				value.SetBuyOutRebirth(isBuy);
				changeLevelDataBuffer.ChangeLevelData(levelId, value);
				changeLevelDataBuffer.SynLevelData(false);
			}
		}
		else
		{
			value = new PlayerLocalLevelData(levelId);
			value.SetBuyOutRebirth(isBuy);
			playerLevelDic.Add(levelId, value);
			changeLevelDataBuffer.ChangeLevelData(levelId, value);
			changeLevelDataBuffer.SynLevelData(false);
		}
		playerAllLocalData.SetLevelBuyOutRebirth(levelId, isBuy);
	}

	public void SetLevelIsPerfect(int levelId, int isPerfect)
	{
		PlayerLocalLevelData value = null;
		if (playerLevelDic.TryGetValue(levelId, out value))
		{
			if (value != null)
			{
				bool flag = false;
				if (value.IsPerfect != isPerfect)
				{
					flag = true;
				}
				value.IsPerfect = isPerfect;
				if (flag)
				{
					changeLevelDataBuffer.ChangeLevelData(levelId, value);
					changeLevelDataBuffer.SynLevelData(false);
				}
			}
			else
			{
				value = new PlayerLocalLevelData(levelId);
				value.IsPerfect = isPerfect;
				changeLevelDataBuffer.ChangeLevelData(levelId, value);
				changeLevelDataBuffer.SynLevelData(false);
			}
		}
		else
		{
			value = new PlayerLocalLevelData(levelId);
			value.IsPerfect = isPerfect;
			playerLevelDic.Add(levelId, value);
			changeLevelDataBuffer.ChangeLevelData(levelId, value);
			changeLevelDataBuffer.SynLevelData(false);
		}
		playerAllLocalData.SetLevelIsPerfect(levelId, isPerfect);
	}

	private void InitMessionDataFromLocal()
	{
		m_MessionData.InitFromLocal();
		AddEventHandler();
	}

	private void InitMessionForOldVersion()
	{
		PlayerMission_table[] records = Mod.DataTable.Get<PlayerMission_table>().Records;
		List<int> list = new List<int>();
		PlayerLocalLevelData playerLocalLevelData = null;
		for (int i = 0; i < records.Length; i++)
		{
			if (records[i].Type == 6)
			{
				playerLocalLevelData = GetPlayerLevelData(records[i].TypeId);
				if (((playerLocalLevelData != null) ? playerLocalLevelData.MaxProgress : 0) >= records[i].TargetNum)
				{
					list.Add(records[i].Id);
				}
			}
			if (list.Count > 0)
			{
				FinishMession(list);
			}
		}
	}

	public void FinishMession(List<int> idList)
	{
		List<PlayerLocalMessionData> list = new List<PlayerLocalMessionData>();
		PlayerLocalMessionData playerLocalMessionData = null;
		for (int i = 0; i < idList.Count; i++)
		{
			if (m_MessionData.FinishMession(idList[i]))
			{
				playerLocalMessionData = MessionData.GetMessionDataById(idList[i]);
				list.Add(playerLocalMessionData);
			}
		}
		if (list.Count > 0)
		{
			Mod.Event.FireNow(this, Mod.Reference.Acquire<MessionFinishedEventArgs>().Initialize(idList));
			SynChangMessionToServer(list);
		}
	}

	public bool MessionIsFinished(int messionId)
	{
		return m_MessionData.IsFinishMession(messionId);
	}

	public bool IsLevelMession(int messionId)
	{
		PlayerMission_table playerMission_table = Mod.DataTable.Get<PlayerMission_table>()[messionId];
		if (playerMission_table != null)
		{
			switch (playerMission_table.Type)
			{
			case 1:
			case 2:
			case 3:
			case 4:
			case 6:
			case 10:
			case 11:
			case 12:
			case 13:
			case 14:
				return true;
			default:
				return false;
			}
		}
		return false;
	}

	public MessionType GetMessionTypeById(int messionId)
	{
		PlayerMission_table playerMission_table = Mod.DataTable.Get<PlayerMission_table>()[messionId];
		if (playerMission_table != null)
		{
			return (MessionType)playerMission_table.Type;
		}
		return MessionType.NONE;
	}

	public int GetMessionTypeId(int messionId)
	{
		PlayerMission_table playerMission_table = Mod.DataTable.Get<PlayerMission_table>()[messionId];
		if (playerMission_table != null)
		{
			return playerMission_table.TypeId;
		}
		return -1;
	}

	public string GetMessionDesc(int messionId)
	{
		PlayerMission_table playerMission_table = Mod.DataTable.Get<PlayerMission_table>()[messionId];
		if (playerMission_table != null)
		{
			return Mod.Localization.GetInfoById(playerMission_table.Desc);
		}
		return "";
	}

	public void AddEventHandler()
	{
		Mod.Event.Subscribe(EventArgs<GameGoodsNumChangeEventArgs>.EventId, OnPlayerAssetChange);
		Mod.Event.Subscribe(EventArgs<InstrumentPropertyChangeEventArgs>.EventId, InstrumentPropertyEventHandler);
		Mod.Event.Subscribe(EventArgs<LevelPropertyChangeEventArgs>.EventId, LevelPropertyEventHandler);
	}

	private void LevelPropertyEventHandler(object sender, Foundation.EventArgs args)
	{
		LevelPropertyChangeEventArgs levelPropertyChangeEventArgs = (LevelPropertyChangeEventArgs)args;
		if (levelPropertyChangeEventArgs == null)
		{
			return;
		}
		PlayerMission_table[] records = Mod.DataTable.Get<PlayerMission_table>().Records;
		List<int> list = new List<int>();
		List<int> list2 = new List<int>();
		int levelId = levelPropertyChangeEventArgs.LevelId;
		PlayerLocalLevelData playerLocalLevelData = null;
		if (levelPropertyChangeEventArgs.ChangeType == LevelPropertyType.FINISH_LEVEL_COUNT)
		{
			int playerFinishLevelTotalCount = GetPlayerFinishLevelTotalCount();
			for (int i = 0; i < records.Length; i++)
			{
				if (records[i].Type == 1)
				{
					if (playerFinishLevelTotalCount >= records[i].TargetNum)
					{
						list.Add(records[i].Id);
					}
					list2.Add(records[i].Id);
				}
				else if (records[i].Type == 12)
				{
					if (GetPlayerFinishLevelTotalCountByLevelDifficulty(LevelDifficulty.NORMAL) >= records[i].TargetNum)
					{
						list.Add(records[i].Id);
					}
					list2.Add(records[i].Id);
				}
				else if (records[i].Type == 13)
				{
					if (GetPlayerFinishLevelTotalCountByLevelDifficulty(LevelDifficulty.Difficulty) >= records[i].TargetNum)
					{
						list.Add(records[i].Id);
					}
					list2.Add(records[i].Id);
				}
				else if (records[i].Type == 14)
				{
					if (GetPlayerFinishLevelTotalCountByLevelDifficulty(LevelDifficulty.Crazy) >= records[i].TargetNum)
					{
						list.Add(records[i].Id);
					}
					list2.Add(records[i].Id);
				}
			}
		}
		else if (levelPropertyChangeEventArgs.ChangeType == LevelPropertyType.PLAY_LEVEL_COUNT)
		{
			int playerPlayLevelTotalCount = GetPlayerPlayLevelTotalCount();
			for (int j = 0; j < records.Length; j++)
			{
				if (records[j].Type == 2)
				{
					if (playerPlayLevelTotalCount >= records[j].TargetNum)
					{
						list.Add(records[j].Id);
					}
					list2.Add(records[j].Id);
				}
			}
		}
		else if (levelPropertyChangeEventArgs.ChangeType == LevelPropertyType.LEVEL_DIAMOND_COUNT)
		{
			int playerAllLevelTotalDiamondsMaxCount = GetPlayerAllLevelTotalDiamondsMaxCount();
			playerLocalLevelData = GetPlayerLevelData(levelId);
			int num = ((playerLocalLevelData != null) ? playerLocalLevelData.GetCollectedDiamoundCount() : 0);
			for (int k = 0; k < records.Length; k++)
			{
				if (records[k].Type == 3)
				{
					if (playerAllLevelTotalDiamondsMaxCount >= records[k].TargetNum)
					{
						list.Add(records[k].Id);
					}
					list2.Add(records[k].Id);
				}
				else if (records[k].Type == 4 && levelId == records[k].TypeId)
				{
					if (num >= records[k].TargetNum)
					{
						list.Add(records[k].Id);
					}
					list2.Add(records[k].Id);
				}
			}
		}
		else if (levelPropertyChangeEventArgs.ChangeType == LevelPropertyType.LEVEL_CROWN_COUNT)
		{
			int playerAllLevelTotalCrownsMaxCount = GetPlayerAllLevelTotalCrownsMaxCount();
			playerLocalLevelData = GetPlayerLevelData(levelId);
			int num2 = ((playerLocalLevelData != null) ? playerLocalLevelData.GetCollectedCrownCount() : 0);
			for (int l = 0; l < records.Length; l++)
			{
				if (records[l].Type == 10)
				{
					if (playerAllLevelTotalCrownsMaxCount >= records[l].TargetNum)
					{
						list.Add(records[l].Id);
					}
					list2.Add(records[l].Id);
				}
				else if (records[l].Type == 11 && levelId == records[l].TypeId)
				{
					if (num2 >= records[l].TargetNum)
					{
						list.Add(records[l].Id);
					}
					list2.Add(records[l].Id);
				}
			}
		}
		else if (levelPropertyChangeEventArgs.ChangeType == LevelPropertyType.LEVEL_PROGRESS)
		{
			playerLocalLevelData = GetPlayerLevelData(levelId);
			int num3 = ((playerLocalLevelData != null) ? playerLocalLevelData.MaxProgress : 0);
			for (int m = 0; m < records.Length; m++)
			{
				if (records[m].Type == 6 && levelId == records[m].TypeId)
				{
					if (num3 >= records[m].TargetNum)
					{
						list.Add(records[m].Id);
					}
					list2.Add(records[m].Id);
				}
			}
		}
		if (list.Count > 0)
		{
			FinishMession(list);
		}
		if (list2.Count > 0)
		{
			Mod.Event.Fire(this, Mod.Reference.Acquire<MessionChangeEventArgs>().Initialize(list2));
		}
	}

	private void InstrumentPropertyEventHandler(object sender, Foundation.EventArgs args)
	{
		InstrumentPropertyChangeEventArgs instrumentPropertyChangeEventArgs = (InstrumentPropertyChangeEventArgs)args;
		if (instrumentPropertyChangeEventArgs == null)
		{
			return;
		}
		PlayerMission_table[] records = Mod.DataTable.Get<PlayerMission_table>().Records;
		List<int> list = new List<int>();
		List<int> list2 = new List<int>();
		List<int> instrumentIds = instrumentPropertyChangeEventArgs.InstrumentIds;
		for (int i = 0; i < instrumentIds.Count; i++)
		{
			int num = instrumentIds[i];
			if (instrumentPropertyChangeEventArgs.ChangeType == InstrumentPropertyType.PLAY_COUNT)
			{
				int totalPlayCount = GetTotalPlayCount();
				for (int j = 0; j < records.Length; j++)
				{
					if (records[j].Type == 9)
					{
						if (totalPlayCount >= records[j].TargetNum)
						{
							list.Add(records[j].Id);
						}
						list2.Add(records[j].Id);
					}
				}
			}
			else
			{
				if (instrumentPropertyChangeEventArgs.ChangeType != 0)
				{
					continue;
				}
				PlayerLocalInstrumentData instrumentDataById = GetInstrumentDataById(num);
				if (instrumentDataById == null)
				{
					continue;
				}
				for (int k = 0; k < records.Length; k++)
				{
					if (records[k].Type == 5)
					{
						if (num == records[k].TypeId && (int)instrumentDataById.Level >= records[k].TargetNum)
						{
							list.Add(records[k].Id);
						}
						if (records[k].TypeId == -1 && (int)instrumentDataById.Level >= records[k].TargetNum)
						{
							list.Add(records[k].Id);
						}
						list2.Add(records[k].Id);
					}
				}
			}
		}
		if (list.Count > 0)
		{
			FinishMession(list);
		}
		if (list2.Count > 0)
		{
			Mod.Event.Fire(this, Mod.Reference.Acquire<MessionChangeEventArgs>().Initialize(list2));
		}
	}

	private void OnPlayerAssetChange(object sender, Foundation.EventArgs args)
	{
		GameGoodsNumChangeEventArgs gameGoodsNumChangeEventArgs = (GameGoodsNumChangeEventArgs)args;
		if (gameGoodsNumChangeEventArgs == null)
		{
			return;
		}
		int goodsId = gameGoodsNumChangeEventArgs.GoodsId;
		double changeNum = gameGoodsNumChangeEventArgs.ChangeNum;
		if (!new List<int>
		{
			4,
			11,
			2,
			GameCommon.GuideLine
		}.Contains(goodsId))
		{
			return;
		}
		if (changeNum > 0.0)
		{
			PropertyLocalData.AddGoodsTotalGetNum(goodsId, changeNum);
		}
		else if (changeNum < 0.0)
		{
			PropertyLocalData.AddGoodsTotalUsedNum(goodsId, changeNum * -1.0);
		}
		double goodsTotalUsedNum = PropertyLocalData.GetGoodsTotalUsedNum(goodsId);
		double goodsTotalGetNum = PropertyLocalData.GetGoodsTotalGetNum(goodsId);
		PlayerMission_table[] records = Mod.DataTable.Get<PlayerMission_table>().Records;
		List<int> list = new List<int>();
		List<int> list2 = new List<int>();
		for (int i = 0; i < records.Length; i++)
		{
			if (records[i].Type == 7 && goodsId == records[i].TypeId)
			{
				if (goodsTotalUsedNum >= (double)records[i].TargetNum)
				{
					list.Add(records[i].Id);
				}
				list2.Add(records[i].Id);
			}
			else if (records[i].Type == 8 && goodsId == records[i].TypeId)
			{
				if (goodsTotalGetNum >= (double)records[i].TargetNum)
				{
					list.Add(records[i].Id);
				}
				list2.Add(records[i].Id);
			}
		}
		if (list.Count > 0)
		{
			FinishMession(list);
		}
		if (list2.Count > 0)
		{
			Mod.Event.Fire(this, Mod.Reference.Acquire<MessionChangeEventArgs>().Initialize(list2));
		}
	}

	private int GetMessionFinishCount(int messionId)
	{
		PlayerMission_table playerMission_table = Mod.DataTable.Get<PlayerMission_table>()[messionId];
		PlayerLocalLevelData playerLocalLevelData = null;
		int result = 0;
		if (playerMission_table != null)
		{
			switch (playerMission_table.Type)
			{
			case 1:
				result = GetPlayerFinishLevelTotalCount();
				break;
			case 2:
				result = GetPlayerPlayLevelTotalCount();
				break;
			case 3:
				result = GetPlayerAllLevelTotalDiamondsMaxCount();
				break;
			case 4:
				playerLocalLevelData = GetPlayerLevelData(playerMission_table.TypeId);
				result = ((playerLocalLevelData != null) ? playerLocalLevelData.TotalDiamondsCount : 0);
				break;
			case 5:
				if (playerMission_table.TypeId > 0)
				{
					result = GetInstrumentDataById(playerMission_table.TypeId).Level;
				}
				else if (playerMission_table.TypeId == -1)
				{
					result = AllUnLockInstrumentCurrentMaxLevel();
				}
				break;
			case 6:
				playerLocalLevelData = GetPlayerLevelData(playerMission_table.TypeId);
				result = ((playerLocalLevelData != null) ? playerLocalLevelData.MaxProgress : 0);
				break;
			case 7:
				result = (int)PropertyLocalData.GetGoodsTotalUsedNum(playerMission_table.TypeId);
				break;
			case 8:
				result = (int)PropertyLocalData.GetGoodsTotalGetNum(playerMission_table.TypeId);
				break;
			case 9:
				result = GetTotalPlayCount();
				break;
			case 10:
				result = GetPlayerAllLevelTotalCrownsMaxCount();
				break;
			case 11:
				playerLocalLevelData = GetPlayerLevelData(playerMission_table.TypeId);
				result = ((playerLocalLevelData != null) ? playerLocalLevelData.TotalCrownsCount : 0);
				break;
			case 12:
				result = GetPlayerFinishLevelTotalCountByLevelDifficulty(LevelDifficulty.NORMAL);
				break;
			case 13:
				result = GetPlayerFinishLevelTotalCountByLevelDifficulty(LevelDifficulty.Difficulty);
				break;
			case 14:
				result = GetPlayerFinishLevelTotalCountByLevelDifficulty(LevelDifficulty.Crazy);
				break;
			}
		}
		return result;
	}

	private int GetMessionTargetNum(int messionId)
	{
		PlayerMission_table playerMission_table = Mod.DataTable.Get<PlayerMission_table>()[messionId];
		int result = 0;
		if (playerMission_table != null)
		{
			result = playerMission_table.TargetNum;
		}
		return result;
	}

	public MessionProgressData GetMessionPrgressPartList(int messionId)
	{
		int messionFinishCount = GetMessionFinishCount(messionId);
		int messionTargetNum = GetMessionTargetNum(messionId);
		MessionProgressData result = new MessionProgressData(messionFinishCount, messionTargetNum);
		if (messionFinishCount >= messionTargetNum && !MessionIsFinished(messionId))
		{
			FinishMession(new List<int> { messionId });
		}
		return result;
	}

	private void SynChangMessionToServer(List<PlayerLocalMessionData> messionList, NetWorkSynType type = NetWorkSynType.IMEDIATELY)
	{
		if (MonoSingleton<NetWorkManager>.Instacne.IsSynNetwork)
		{
			switch (type)
			{
			case NetWorkSynType.NORMAL:
				m_MessionBuffer.ChangeMessionData(messionList);
				m_MessionBuffer.SynMessionlData(false);
				break;
			case NetWorkSynType.IMEDIATELY:
				m_MessionBuffer.ChangeMessionData(messionList);
				m_MessionBuffer.SynMessionlData(true);
				break;
			case NetWorkSynType.NONE:
				break;
			}
		}
	}

	public void GetPlayerAllDataFromServer(UnityAction successHandle = null, UnityAction failedHanlder = null)
	{
		NetMessageData netMessageData = ObjectPool<NetMessageData>.Instance.Get();
		netMessageData.messageId = NetMessageId.GET_ALLPLAYERDATA;
		netMessageData.content = "";
		netMessageData.succesHandler = delegate(NetMessageResultData content)
		{
			if (content.code == 0)
			{
				m_isServerNewUser = false;
				PlayerAllServerData playerAllServerData = JsonUtility.FromJson<PlayerAllServerData>(content.data);
				m_serverTime = playerAllServerData.serverTime;
				double num = playerAllLocalData.CompareWithServerData(playerAllServerData);
				int num2 = SequenceLoginData.CompareWithServerData(playerAllServerData);
				int num3 = PlayerLocalLevelSeriesData.CompareWithServerData(playerAllServerData);
				int num4 = PlayerGiftPackageData.CompareWithServerData(playerAllServerData);
				int num5 = MessionData.CompareWithServerData(playerAllServerData);
				int num6 = PropertyLocalData.CompareWithServerData(playerAllServerData);
				int num7 = PlayerInstrumentLocalData.CompareWithServerData(playerAllServerData);
				if (true && num > 0.0 && num2 > 0 && num3 > 0 && num4 > 0 && num5 > 0 && num6 > 0 && num7 > 0)
				{
					changeDataBuffer.Clear();
					changeLevelDataBuffer.Clear();
					PostPlayerAllDataToServer();
				}
				if (num < 0.0)
				{
					playerAllLocalData.ResetFromServerData(playerAllServerData);
				}
				if (num2 < 0)
				{
					SequenceLoginData.ResetFromServerData(playerAllServerData);
				}
				if (num3 < 0)
				{
					PlayerLocalLevelSeriesData.ResetFromServerData(playerAllServerData);
				}
				if (num4 < 0)
				{
					PlayerGiftPackageData.ResetFromServerData(playerAllServerData);
				}
				if (num5 < 0)
				{
					MessionData.ResetFromServerData(playerAllServerData);
				}
				if (num6 < 0)
				{
					ResetFromServerData(playerAllServerData);
				}
				if (num7 < 0)
				{
					PlayerInstrumentLocalData.ResetFromServerData(playerAllServerData);
				}
			}
			else if (content.code == -1)
			{
				m_isServerNewUser = true;
				changeDataBuffer.Clear();
				PostPlayerAllDataToServer();
			}
			else
			{
				MonoSingleton<NetWorkManager>.Instacne.IsSynNetwork = false;
			}
			if (successHandle != null)
			{
				successHandle();
			}
			InitData();
		};
		netMessageData.errorHandler = delegate
		{
			if (failedHanlder != null)
			{
				failedHanlder();
			}
		};
		MonoSingleton<NetWorkManager>.Instacne.Send(netMessageData);
	}

	public void PostPlayerAllDataToServer()
	{
		NetMessageData netMessageData = ObjectPool<NetMessageData>.Instance.Get();
		netMessageData.messageId = NetMessageId.POST_ALLPLAYERDATA;
		PlayerAllServerData playerAllServerData = new PlayerAllServerData();
		playerAllServerData.InitFromLocalData();
		string text = (netMessageData.content = JsonUtility.ToJson(playerAllServerData));
		netMessageData.succesHandler = delegate
		{
		};
		netMessageData.errorHandler = delegate(string content)
		{
			Debug.LogWarning("PostPlayerAllDataToServerError:" + content);
		};
		MonoSingleton<NetWorkManager>.Instacne.Send(netMessageData);
	}

	private void InitPropertyDataFromLocal()
	{
		m_PropertLocalData.InitFromLocal();
		m_Level = m_PropertLocalData.Level;
		m_StarLevel = m_PropertLocalData.StarLevel;
		m_HideLevel = m_PropertLocalData.HideLevel;
		if (m_PropertySynBuffer == null)
		{
			m_PropertySynBuffer = new SynChangePlayerPropertyBuffer();
		}
		if (MonoSingleton<NetWorkManager>.Instacne.IsSynNetwork)
		{
			m_PropertySynBuffer.InitFromLocal();
		}
	}

	public int GetPlayerLevel()
	{
		return m_Level;
	}

	public bool IsPlayerUpLevelNeedGoodsEnough()
	{
		int playerUpLevelNeedGoodsId = GetPlayerUpLevelNeedGoodsId(m_Level);
		double playGoodsNum = GetPlayGoodsNum(playerUpLevelNeedGoodsId);
		double playerUpLevelNeedGoodsNum = GetPlayerUpLevelNeedGoodsNum(m_Level);
		return playGoodsNum >= playerUpLevelNeedGoodsNum;
	}

	public ResultState UpPlayerLevel(out int rewardId)
	{
		rewardId = -1;
		int playerUpLevelNeedGoodsId = GetPlayerUpLevelNeedGoodsId(m_Level);
		double playGoodsNum = GetPlayGoodsNum(playerUpLevelNeedGoodsId);
		double playerUpLevelNeedGoodsNum = GetPlayerUpLevelNeedGoodsNum(m_Level);
		if (playGoodsNum < playerUpLevelNeedGoodsNum)
		{
			return ResultState.GOOD_NOT_ENOUGH;
		}
		int playerUpLevelNeedFinishiMessionId = GetPlayerUpLevelNeedFinishiMessionId(m_Level);
		if (playerUpLevelNeedFinishiMessionId > 0 && !MessionIsFinished(playerUpLevelNeedFinishiMessionId))
		{
			return ResultState.MESSION_NOT_FINISH;
		}
		int num = m_Level;
		m_Level = (int)m_Level + 1;
		m_HideLevel = (int)m_HideLevel + 1;
		m_PropertLocalData.Level = m_Level;
		m_PropertLocalData.HideLevel = m_HideLevel;
		m_PropertLocalData.LastUpLevelTimeStamp = ServerTime;
		m_PropertLocalData.SaveToLocal();
		ChangePlayerGoodsNum(playerUpLevelNeedGoodsId, playerUpLevelNeedGoodsNum * -1.0);
		UnlockInstrumentByStarLevel(m_StarLevel, m_Level);
		SynChangPropertyToServer();
		int num2 = (rewardId = GetRewardGoodsTeamId(num));
		Mod.Event.FireNow(this, Mod.Reference.Acquire<PlayerPropertyChangeEventArgs>().Initialize(PlayerPropertyType.LEVEL));
		if (playerUpLevelNeedFinishiMessionId > 0)
		{
			InfocUtils.Report_rollingsky2_games_Mission(num, m_StarLevel);
		}
		return ResultState.SUCCESS;
	}

	public int GetPlayerUpLevelNeedGoodsId(int level = 1)
	{
		PlayerLevel_table playerLevel_table = Mod.DataTable.Get<PlayerLevel_table>()[level];
		if (playerLevel_table != null)
		{
			return playerLevel_table.LevelUpConsumeGoodsID;
		}
		return GameCommon.REPUTATION_ID;
	}

	public int GetPlayerUpLevelNeedFinishiMessionId(int level)
	{
		PlayerLevel_table playerLevel_table = Mod.DataTable.Get<PlayerLevel_table>()[level];
		int result = -1;
		if (playerLevel_table != null)
		{
			List<int> list = MonoSingleton<GameTools>.Instacne.StringToIntList(playerLevel_table.LevelUpMission);
			if (list.Count > (int)m_StarLevel)
			{
				result = list[m_StarLevel];
			}
		}
		return result;
	}

	public int GetNextEffectMessionId(int fromLevel, ref int targetLevel)
	{
		int num = -1;
		for (int i = fromLevel + 1; i <= GameCommon.PLAYER_MAX_LEVEL; i++)
		{
			num = GetPlayerUpLevelNeedFinishiMessionId(i);
			if (num > 0)
			{
				targetLevel = i;
				break;
			}
		}
		return num;
	}

	public double GetPlayerUpLevelNeedGoodsNum(int level)
	{
		double result = 0.0;
		PlayerLevel_table playerLevel_table = Mod.DataTable.Get<PlayerLevel_table>()[level];
		if (playerLevel_table != null)
		{
			int playerStarLevel = GetPlayerStarLevel();
			List<double> list = MonoSingleton<GameTools>.Instacne.StringToDoubleList(playerLevel_table.LevelUpConsumeGoodsNum);
			if (playerStarLevel < list.Count)
			{
				result = list[playerStarLevel];
			}
		}
		return result;
	}

	public int GetRewardGoodsTeamId(int level)
	{
		int result = -1;
		PlayerLevel_table playerLevel_table = Mod.DataTable.Get<PlayerLevel_table>()[level];
		if (playerLevel_table != null)
		{
			int playerStarLevel = GetPlayerStarLevel();
			result = MonoSingleton<GameTools>.Instacne.StringToIntList(playerLevel_table.RewardId)[playerStarLevel];
		}
		return result;
	}

	public int GetPlayerStarLevel()
	{
		return m_StarLevel;
	}

	public UpStarLevelResultState UpPlayerStarLevel()
	{
		int num = m_StarLevel;
		int upStarLevelGoodsId = GetUpStarLevelGoodsId(num);
		double playGoodsNum = GetPlayGoodsNum(upStarLevelGoodsId);
		long playerUpStarNeedGoodsNum = GetPlayerUpStarNeedGoodsNum(num);
		if (playGoodsNum < (double)playerUpStarNeedGoodsNum)
		{
			return UpStarLevelResultState.GOOD_NOT_ENOUGH;
		}
		PlayerStarLevel_table playerStarLevel_table = Mod.DataTable.Get<PlayerStarLevel_table>()[m_StarLevel];
		if (playerStarLevel_table != null && GetPlayerLevel() < playerStarLevel_table.StarUpNeedLevel)
		{
			return UpStarLevelResultState.LEVEL_NOT_ENOUGH;
		}
		m_StarLevel = (int)m_StarLevel + 1;
		m_PropertLocalData.StarLevel = m_StarLevel;
		m_Level = 1;
		m_PropertLocalData.Level = m_Level;
		m_PropertLocalData.SaveToLocal();
		ResetAllUnLockInstrumentLevel();
		UnlockInstrumentByStarLevel(m_StarLevel, m_Level);
		double playGoodsNum2 = GetPlayGoodsNum(GameCommon.REPUTATION_ID);
		ChangePlayerGoodsNum(GameCommon.REPUTATION_ID, playGoodsNum2 * -1.0);
		ChangePlayerGoodsNum(upStarLevelGoodsId, playerUpStarNeedGoodsNum * -1);
		SynChangPropertyToServer();
		Mod.Event.FireNow(this, Mod.Reference.Acquire<PlayerPropertyChangeEventArgs>().Initialize(PlayerPropertyType.STARLEVEL));
		return UpStarLevelResultState.SUCCESS;
	}

	public bool IsUpLevelShowAd()
	{
		return ServerTime - m_PropertLocalData.LastUpLevelTimeStamp >= GameCommon.upPlayerLevelByAdMaxTime * 3600 * 1000;
	}

	public void ResetUpLevelTime()
	{
		m_PropertLocalData.LastUpLevelTimeStamp = ServerTime;
		m_PropertLocalData.SaveToLocal();
	}

	public long GetPlayerUpStarNeedGoodsNum(int willtoStarLevel)
	{
		PlayerStarLevel_table playerStarLevel_table = Mod.DataTable.Get<PlayerStarLevel_table>()[willtoStarLevel];
		long result = 0L;
		if (playerStarLevel_table != null && !long.TryParse(playerStarLevel_table.StarUpNeedGoodsCount, out result))
		{
			result = 0L;
		}
		return result;
	}

	private int GetUpStarLevelGoodsId(int starLevel)
	{
		PlayerStarLevel_table playerStarLevel_table = Mod.DataTable.Get<PlayerStarLevel_table>()[starLevel];
		if (playerStarLevel_table != null)
		{
			return playerStarLevel_table.StarUpNeedGoodsId;
		}
		return 3;
	}

	public void ResetFromServerData(PlayerAllServerData serverData)
	{
		if (serverData != null && serverData.propertyData != null)
		{
			m_PropertLocalData.InitFromServerData(serverData.propertyData);
			m_Level = serverData.propertyData.level;
			m_StarLevel = serverData.propertyData.starLevel;
			m_HideLevel = serverData.propertyData.hideLevel;
		}
	}

	public int GetStarLevelAbilityNum(int abilityLevel)
	{
		if (abilityLevel > 10)
		{
			return 0;
		}
		PlayerStarAbility_table playerStarAbility_table = Mod.DataTable.Get<PlayerStarAbility_table>()[abilityLevel];
		if (playerStarAbility_table != null)
		{
			return playerStarAbility_table.AblilityNum;
		}
		return 0;
	}

	public bool PlayerIsHadSpecialStarAbility(int abilityLevel)
	{
		if (Mod.DataTable == null || Mod.DataTable.Get<PlayerStarAbility_table>() == null)
		{
			return false;
		}
		PlayerStarAbility_table playerStarAbility_table = Mod.DataTable.Get<PlayerStarAbility_table>()[abilityLevel];
		if (playerStarAbility_table == null)
		{
			return false;
		}
		return (int)m_StarLevel >= playerStarAbility_table.MinStarLevel;
	}

	public string GetSpecialStarAbilityDesc(int abilityLevel)
	{
		if (abilityLevel > 10)
		{
			return "";
		}
		PlayerStarAbility_table playerStarAbility_table = Mod.DataTable.Get<PlayerStarAbility_table>()[abilityLevel];
		if (playerStarAbility_table != null)
		{
			return Mod.Localization.GetInfoById(playerStarAbility_table.Desc);
		}
		return "";
	}

	public bool IfCanPlayerParticularStarUpgradeByAbilityLevel(int abilityLevel, ref bool ifHaveEnoughLevel, ref bool ifHaveEnoughCoin)
	{
		ifHaveEnoughLevel = (ifHaveEnoughCoin = false);
		if (abilityLevel > 10)
		{
			return false;
		}
		PlayerStarLevel_table playerStarLevel_table = Mod.DataTable.Get<PlayerStarLevel_table>()[abilityLevel];
		if (playerStarLevel_table != null)
		{
			int starUpNeedLevel = playerStarLevel_table.StarUpNeedLevel;
			int playerLevel = GetPlayerLevel();
			ifHaveEnoughLevel = playerLevel >= starUpNeedLevel;
			int starUpNeedGoodsId = playerStarLevel_table.StarUpNeedGoodsId;
			long result = 0L;
			if (!long.TryParse(playerStarLevel_table.StarUpNeedGoodsCount, out result))
			{
				result = 0L;
			}
			double playGoodsNum = Instance.GetPlayGoodsNum(starUpNeedGoodsId);
			ifHaveEnoughCoin = playGoodsNum >= (double)result;
		}
		return ifHaveEnoughLevel & ifHaveEnoughCoin;
	}

	public void GetPlayerParticulerStarUpgradeDataByAbilityLevel(int abilityLevel, ref int level, ref int goodId, ref long goodNum)
	{
		level = 0;
		goodId = 3;
		goodNum = 0L;
		if (abilityLevel > 10)
		{
			return;
		}
		PlayerStarLevel_table playerStarLevel_table = Mod.DataTable.Get<PlayerStarLevel_table>()[abilityLevel];
		if (playerStarLevel_table != null)
		{
			level = playerStarLevel_table.StarUpNeedLevel;
			goodId = playerStarLevel_table.StarUpNeedGoodsId;
			if (!long.TryParse(playerStarLevel_table.StarUpNeedGoodsCount, out goodNum))
			{
				goodNum = 0L;
			}
		}
	}

	public void DealAbilityStarLevel_7()
	{
		int num = 0;
		if (Instance.PlayerIsHadSpecialStarAbility(7))
		{
			num = Instance.GetStarLevelAbilityNum(7);
		}
		if (num > 0)
		{
			ChangePlayerGoodsNum(6, num, AssertChangeType.AD);
			BroadCastData broadCastData = new BroadCastData();
			broadCastData.GoodId = 6;
			broadCastData.Type = BroadCastType.GOODS;
			broadCastData.Info = string.Format("X{0}", num);
			MonoSingleton<BroadCastManager>.Instacne.BroadCast(broadCastData);
		}
	}

	public string GetPlayerTitleByStarLevel(int starLevel)
	{
		string result = "";
		PlayerStarLevel_table playerStarLevel_table = Mod.DataTable.Get<PlayerStarLevel_table>()[starLevel];
		if (playerStarLevel_table != null)
		{
			result = Mod.Localization.GetInfoById(playerStarLevel_table.PlayerTitleName);
		}
		return result;
	}

	public int GetPlayerHeadIcon()
	{
		int result = -1;
		PlayerStarLevel_table playerStarLevel_table = Mod.DataTable.Get<PlayerStarLevel_table>()[m_StarLevel];
		if (playerStarLevel_table != null)
		{
			result = playerStarLevel_table.PlayerIcon;
		}
		return result;
	}

	public string GetPlayerStageAvaterName()
	{
		string result = "";
		PlayerStarLevel_table playerStarLevel_table = Mod.DataTable.Get<PlayerStarLevel_table>()[m_StarLevel];
		if (playerStarLevel_table != null)
		{
			result = playerStarLevel_table.PlayerStageAvatar;
		}
		return result;
	}

	public int GetPlayerLevelAvaterId()
	{
		int result = 1;
		PlayerStarLevel_table playerStarLevel_table = Mod.DataTable.Get<PlayerStarLevel_table>()[m_StarLevel];
		if (playerStarLevel_table != null)
		{
			result = playerStarLevel_table.PlayerLevelAvatar;
		}
		return result;
	}

	public int GetPlayerStarLevelFormIcon()
	{
		int result = -1;
		PlayerStarLevel_table playerStarLevel_table = Mod.DataTable.Get<PlayerStarLevel_table>()[m_StarLevel];
		if (playerStarLevel_table != null)
		{
			result = playerStarLevel_table.PlayerStarLevelIcon;
		}
		return result;
	}

	private void SynChangPropertyToServer(NetWorkSynType type = NetWorkSynType.NORMAL)
	{
		if (MonoSingleton<NetWorkManager>.Instacne.IsSynNetwork)
		{
			switch (type)
			{
			case NetWorkSynType.NORMAL:
				m_PropertySynBuffer.ChangePropertyData(m_PropertLocalData);
				m_PropertySynBuffer.SynPropertylData(false);
				break;
			case NetWorkSynType.IMEDIATELY:
				m_PropertySynBuffer.ChangePropertyData(m_PropertLocalData);
				m_PropertySynBuffer.SynPropertylData(true);
				break;
			case NetWorkSynType.NONE:
				break;
			}
		}
	}

	private void InitLevelResetTime(DateTime launchTime)
	{
	}

	private void DealLevelTargets(DateTime nowTime)
	{
	}

	private void ResetLevelTargets()
	{
		playerLevelTargetData.ResetTarget();
		DateTime levelResetTime = GetLevelResetTime().AddDays(1.0);
		SetLevelResetTime(levelResetTime);
	}

	private void ResetLevelTargetTimeHandler()
	{
		ResetLevelTargets();
		TimerHeap.DelTimer(levelTargetTimerId);
		DealLevelTargets(DateTime.Now);
	}

	private DateTime GetLevelResetTime()
	{
		if (string.IsNullOrEmpty(EncodeConfig.getConfig(PlayerLocalDatakey.PLAYERLEVELRESETTIME)))
		{
			InitLevelResetTime(DateTime.Now);
		}
		return TimeTools.StringToDatetime(EncodeConfig.getConfig(PlayerLocalDatakey.PLAYERLEVELRESETTIME));
	}

	private void SetLevelResetTime(DateTime time)
	{
		string values = TimeTools.DateTimeToString(time);
		EncodeConfig.setConfig(PlayerLocalDatakey.PLAYERLEVELRESETTIME, values);
	}
}
