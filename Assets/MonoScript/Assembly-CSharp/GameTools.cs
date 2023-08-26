using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Foundation;
using My.Core;
using RS2;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameTools : MonoSingleton<GameTools>
{
	private uint lastEnableInputTimerId;

	public int CallIsCanShowAdFailedCount { get; set; }

	public void RequestCloudData()
	{
		string text = My.Core.Singleton<CloudConfigHelper>.Instance.TextConfig1;
		Log.Info("configStr:" + text);
		bool flag = true;
		if (string.IsNullOrEmpty(text))
		{
			text = PlayerDataModule.GetGameCloudDataFromLocal();
			flag = false;
		}
		if (!string.IsNullOrEmpty(text))
		{
			CloudConfigData cloudConfigData = JsonUtility.FromJson<CloudConfigData>(text);
			if (cloudConfigData != null)
			{
				UpdateDataFromServer(cloudConfigData);
			}
			if (flag)
			{
				PlayerDataModule.SetGameClouldDataToLocal(text);
			}
		}
	}

	public void InitDataFromTable()
	{
		IDataTable<CommonData_commonDataTable> dataTable = Mod.DataTable.Get<CommonData_commonDataTable>();
		CultureInfo cultureInfo = (CultureInfo)CultureInfo.CurrentCulture.Clone();
		cultureInfo.NumberFormat.CurrencyDecimalSeparator = ".";
		if (dataTable != null)
		{
			CommonData_commonDataTable commonData_commonDataTable = dataTable[1];
			if (commonData_commonDataTable != null)
			{
				GameCommon.firstPowerNum = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[2];
			if (commonData_commonDataTable != null)
			{
				GameCommon.firstRebirthNum = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[3];
			if (commonData_commonDataTable != null)
			{
				GameCommon.firstShieldNum = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[4];
			if (commonData_commonDataTable != null)
			{
				GameCommon.powerCdTime = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[5];
			if (commonData_commonDataTable != null)
			{
				GameCommon.screenPlguinStartId = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[6];
			if (commonData_commonDataTable != null)
			{
				GameCommon.levelTargetMaxCanFinNum = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[7];
			if (commonData_commonDataTable != null)
			{
				GameCommon.levelTargetSelectedNum = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[8];
			if (commonData_commonDataTable != null)
			{
				GameCommon.rebirthMaxNum = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[9];
			if (commonData_commonDataTable != null)
			{
				GameCommon.rebirthMaxAdNum = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[10];
			if (commonData_commonDataTable != null)
			{
				GameCommon.levelTargetResetTime = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[11];
			if (commonData_commonDataTable != null)
			{
				GameCommon.firstFinishLevelGetKeysMum = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[12];
			if (commonData_commonDataTable != null)
			{
				GameCommon.shopFreeAdResetTime = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[13];
			if (commonData_commonDataTable != null)
			{
				GameCommon.shopFreeAdTodayShowMaxCount = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[14];
			if (commonData_commonDataTable != null)
			{
				GameCommon.powerRecoverNum = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[15];
			if (commonData_commonDataTable != null)
			{
				GameCommon.powerRecoverNum_byAd = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[16];
			if (commonData_commonDataTable != null)
			{
				GameCommon.levelTargetOpenFirstLevelPercent = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[17];
			if (commonData_commonDataTable != null)
			{
				GameCommon.useAssetOpenTotalFinishLevelNum = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[18];
			if (commonData_commonDataTable != null)
			{
				GameCommon.shopResetTime = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[19];
			if (commonData_commonDataTable != null)
			{
				GameCommon.getPowerByAdMaxNumEveryDay = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[20];
			if (commonData_commonDataTable != null)
			{
				GameCommon.timeOutFormGiveGoodsId = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[21];
			if (commonData_commonDataTable != null)
			{
				GameCommon.openBuffFunctionLevelTime = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[22];
			if (commonData_commonDataTable != null)
			{
				GameCommon.needHeartRebirthMaxNumber = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[23];
			if (commonData_commonDataTable != null)
			{
				GameCommon.tyrLevelNumPerAd = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[24];
			if (commonData_commonDataTable != null)
			{
				GameCommon.startPowerRecoverThreshold = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[25];
			if (commonData_commonDataTable != null)
			{
				GameCommon.motivateRewardCD = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[26];
			if (commonData_commonDataTable != null)
			{
				GameCommon.motivateRewardNeedEnergy = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[27];
			if (commonData_commonDataTable != null)
			{
				GameCommon.progressMotivateEnergy = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[28];
			if (commonData_commonDataTable != null)
			{
				GameCommon.diamoundMotivateEnergy = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[29];
			if (commonData_commonDataTable != null)
			{
				GameCommon.crownMotivateEnergy = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[30];
			if (commonData_commonDataTable != null)
			{
				GameCommon.payUserThreshold = float.Parse(commonData_commonDataTable.Num, NumberStyles.Any, cultureInfo);
			}
			commonData_commonDataTable = dataTable[31];
			if (commonData_commonDataTable != null)
			{
				GameCommon.screenPluginAdProtectedTime = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[32];
			if (commonData_commonDataTable != null)
			{
				GameCommon.screenPluginAdCDTime = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[33];
			if (commonData_commonDataTable != null)
			{
				GameCommon.moneyGiftPackageShopID = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[34];
			if (commonData_commonDataTable != null)
			{
				GameCommon.powerGiftPackageShopID = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[35];
			if (commonData_commonDataTable != null)
			{
				GameCommon.powerRecoveryGiftPackageShopID = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[36];
			if (commonData_commonDataTable != null)
			{
				GameCommon.removeAdGiftPackageShopID = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[37];
			if (commonData_commonDataTable != null)
			{
				string[] array = commonData_commonDataTable.Num.Split('|');
				GameCommon.removeAdMoneyShopIDs = new int[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					GameCommon.removeAdMoneyShopIDs[i] = int.Parse(array[i]);
				}
			}
			commonData_commonDataTable = dataTable[38];
			if (commonData_commonDataTable != null)
			{
				GameCommon.showFiveStarThreshold = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[39];
			if (commonData_commonDataTable != null)
			{
				GameCommon.showRemoveAdForScreenCount = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[40];
			if (commonData_commonDataTable != null)
			{
				string[] array2 = commonData_commonDataTable.Num.Split('|');
				GameCommon.ignoreFiveStarLevelIds = new int[array2.Length];
				for (int j = 0; j < array2.Length; j++)
				{
					GameCommon.ignoreFiveStarLevelIds[j] = int.Parse(array2[j]);
				}
			}
			commonData_commonDataTable = dataTable[41];
			if (commonData_commonDataTable != null)
			{
				GameCommon.newVersionShowSeriesId = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[42];
			if (commonData_commonDataTable != null)
			{
				GameCommon.isOpenScreenPlugin = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[43];
			if (commonData_commonDataTable != null)
			{
				GameCommon.showFreeBuffThreshold = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[44];
			if (commonData_commonDataTable != null)
			{
				GameCommon.adFreeLevelEnterTime = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[45];
			if (commonData_commonDataTable != null)
			{
				GameCommon.AdAwardItemCount = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[46];
			if (commonData_commonDataTable != null)
			{
				GameCommon.AdAwardRestartTimes = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[47];
			if (commonData_commonDataTable != null)
			{
				GameCommon.firstGuideNum = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[48];
			if (commonData_commonDataTable != null)
			{
				GameCommon.maxMotivateAwardTimes = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[49];
			if (commonData_commonDataTable != null)
			{
				GameCommon.maxFreeGoodsTimes = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[50];
			if (commonData_commonDataTable != null)
			{
				GameCommon.maxFreeGoodsNoAdTimes = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[51];
			if (commonData_commonDataTable != null)
			{
				GameCommon.originRebirthContinueTime = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[52];
			if (commonData_commonDataTable != null)
			{
				GameCommon.originRebirthContinueShowTime = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[54];
			if (commonData_commonDataTable != null)
			{
				GameCommon.offlineProductReputationMaxTime = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[55];
			if (commonData_commonDataTable != null)
			{
				GameCommon.offlineProductGoldMaxTime = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[56];
			if (commonData_commonDataTable != null)
			{
				GameCommon.offlineProductMinTime = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[57];
			if (commonData_commonDataTable != null)
			{
				GameCommon.instrumentSpeedUpTime = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[58];
			if (commonData_commonDataTable != null)
			{
				GameCommon.instrumentSpeedUpMaxTime = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[59];
			if (commonData_commonDataTable != null)
			{
				GameCommon.offlineProductCostDiamound = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[60];
			if (commonData_commonDataTable != null)
			{
				GameCommon.instrumentSpeedUpCostDiamound = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[61];
			if (commonData_commonDataTable != null)
			{
				GameCommon.upPlayerLevelByAdMaxTime = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[62];
			if (commonData_commonDataTable != null)
			{
				GameCommon.keyGiftPackageShopID = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[63];
			if (commonData_commonDataTable != null)
			{
				GameCommon.instrumentLimitLevel = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[64];
			if (commonData_commonDataTable != null)
			{
				GameCommon.instrumentAdProductFactor = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[65];
			if (commonData_commonDataTable != null)
			{
				GameCommon.instrumentProductFrequency = float.Parse(commonData_commonDataTable.Num, NumberStyles.Any, cultureInfo);
			}
			commonData_commonDataTable = dataTable[66];
			if (commonData_commonDataTable != null)
			{
				GameCommon.instrumentProductSpeeedUpFrequency = float.Parse(commonData_commonDataTable.Num, NumberStyles.Any, cultureInfo);
			}
			commonData_commonDataTable = dataTable[67];
			if (commonData_commonDataTable != null)
			{
				GameCommon.instrumenCommonBroadFrequency = float.Parse(commonData_commonDataTable.Num, NumberStyles.Any, cultureInfo);
			}
			commonData_commonDataTable = dataTable[68];
			if (commonData_commonDataTable != null)
			{
				GameCommon.instrumenSpeedUpBroadFrequency = float.Parse(commonData_commonDataTable.Num, NumberStyles.Any, cultureInfo);
			}
			commonData_commonDataTable = dataTable[69];
			if (commonData_commonDataTable != null)
			{
				GameCommon.networkVerifyLastTime = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[70];
			if (commonData_commonDataTable != null)
			{
				GameCommon.guideLineTime = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[71];
			if (commonData_commonDataTable != null)
			{
				GameCommon.instrumentLevelShowTime = float.Parse(commonData_commonDataTable.Num, NumberStyles.Any, cultureInfo);
			}
			commonData_commonDataTable = dataTable[72];
			if (commonData_commonDataTable != null)
			{
				GameCommon.instrumentLevelClickShowTime = float.Parse(commonData_commonDataTable.Num, NumberStyles.Any, cultureInfo);
			}
			commonData_commonDataTable = dataTable[73];
			if (commonData_commonDataTable != null)
			{
				GameCommon.coolPlayPackageContidtion_X = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[74];
			if (commonData_commonDataTable != null)
			{
				GameCommon.coolPlayPackageContidtion_Y = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[75];
			if (commonData_commonDataTable != null)
			{
				GameCommon.coolPlayPackageContidtion_Z = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[76];
			if (commonData_commonDataTable != null)
			{
				GameCommon.luckyTurnTableSpentGemsNum = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[77];
			if (commonData_commonDataTable != null)
			{
				GameCommon.showAdInInstrument = int.Parse(commonData_commonDataTable.Num);
			}
			commonData_commonDataTable = dataTable[78];
			if (commonData_commonDataTable != null)
			{
				GameCommon.InstrumentUnlockLV = int.Parse(commonData_commonDataTable.Num);
			}
		}
		LevelOrder_levelOrderTable[] records = Mod.DataTable.Get<LevelOrder_levelOrderTable>().Records;
		if (records.Length > 2)
		{
			GameCommon.FIRST_SERIES = records[0].LevelSeriesId;
			GameCommon.SECOND_SERIES = records[1].LevelSeriesId;
			GameCommon.THIRD_SERIES = records[2].LevelSeriesId;
		}
	}

	private void UpdateDataFromServer(CloudConfigData commonData)
	{
		GameCommon.screenPluginAdProtectedTime = commonData.m_PluginAdProtectedTime;
		GameCommon.screenPluginAdCDTime = commonData.m_pluginAdCDTime;
		GameCommon.screenPlguinStartId = commonData.m_PluginAdId;
		GameCommon.isOpenScreenPlugin = commonData.m_isOpenScreenPlugin;
	}

	public int[] GetRandom(int total, int outNum)
	{
		int[] array = new int[total];
		int[] array2 = new int[outNum];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = i;
		}
		int num = array.Length - 1;
		int num2 = -1;
		for (int j = 0; j < array.Length; j++)
		{
			num2 = UnityEngine.Random.Range(0, num);
			if (j >= array2.Length)
			{
				break;
			}
			array2[j] = array[num2];
			array[num2] = array[num];
			num--;
		}
		return array2;
	}

	public string GetCloudLanguage(SystemLanguage type)
	{
		string result = "en";
		switch (type)
		{
		case SystemLanguage.English:
			result = "en";
			break;
		case SystemLanguage.Spanish:
			result = "es";
			break;
		case SystemLanguage.ChineseSimplified:
			result = "zh_cn";
			break;
		case SystemLanguage.ChineseTraditional:
			result = "zh_tw";
			break;
		case SystemLanguage.Japanese:
			result = "ja";
			break;
		case SystemLanguage.French:
			result = "fr";
			break;
		case SystemLanguage.German:
			result = "de";
			break;
		case SystemLanguage.Italian:
			result = "it";
			break;
		case SystemLanguage.Dutch:
			result = "nl";
			break;
		case SystemLanguage.Russian:
			result = "ru";
			break;
		case SystemLanguage.Arabic:
			result = "ar";
			break;
		case SystemLanguage.Korean:
			result = "ko";
			break;
		}
		return result;
	}

	public UnityAction<ADScene> GetDefaultFailCallback()
	{
		return delegate
		{
			BroadCastData data = new BroadCastData
			{
				Type = BroadCastType.INFO,
				Info = "Play failed,try again later!"
			};
			MonoSingleton<BroadCastManager>.Instacne.BroadCast(data);
			EnableInput();
		};
	}

	public UnityAction<ADScene> GetDefaultNoAdCallback()
	{
		return delegate
		{
			BroadCastData data = new BroadCastData
			{
				Type = BroadCastType.INFO,
				Info = "Preparing,try again later!"
			};
			MonoSingleton<BroadCastManager>.Instacne.BroadCast(data);
			EnableInput();
		};
	}

	public void EnableInput()
	{
		UIInputModule.UIInput.EnableInput = true;
	}

	public void DisableInputForAWhile(uint time = 10000u, UnityAction enableCallback = null)
	{
		UIInputModule.UIInput.EnableInput = false;
		TimerToEnableInput(time, enableCallback);
	}

	public void TimerToEnableInput(uint time, UnityAction enableCallback = null)
	{
		uint enableInputTimerId = 0u;
		enableInputTimerId = TimerHeap.AddTimer(time, 0u, delegate
		{
			TimerHeap.DelTimer(enableInputTimerId);
			if (enableInputTimerId == lastEnableInputTimerId)
			{
				EnableInput();
				if (enableCallback != null)
				{
					enableCallback();
				}
			}
		});
		lastEnableInputTimerId = enableInputTimerId;
	}

	public void PlayVideoAdAndDisableInput(ADScene scene, UnityAction<ADScene> successCallBack)
	{
		UIInputModule.UIInput.EnableInput = false;
		PlayerVideoAd(scene, successCallBack, GetDefaultFailCallback(), GetDefaultNoAdCallback(), delegate
		{
			EnableInput();
		}, delegate
		{
			EnableInput();
		});
		TimerToEnableInput(5000u);
	}

	public void PlayerVideoAd(ADScene scene, UnityAction<ADScene> successCallBack, UnityAction<ADScene> failCallBack = null, UnityAction<ADScene> noAdCallBack = null, UnityAction<ADScene> startAdCallBack = null, UnityAction<ADScene> closeAdCallBack = null)
	{
		InfocUtils.ReportRewardAdShowChance((int)scene);
		bool successFlag = false;
		bool closeFlag = false;
		if (failCallBack == null)
		{
			failCallBack = GetDefaultFailCallback();
		}
		if (noAdCallBack == null)
		{
			noAdCallBack = GetDefaultNoAdCallback();
		}
		if (My.Core.Singleton<ADHelper>.Instance.RewardeVideoCanShow(scene))
		{
			Log.Info("ADHelper.Instance.RewardeVideoCanShow scene " + scene);
			My.Core.Singleton<ADHelper>.Instance.ShowRewardedVideo(scene, delegate(ADCallbackEventArgs x)
			{
				Log.Info(string.Concat("ShowRewardedVideo x.Scene ", x.Scene, " x.Status ", x.Status));
				if (x.Scene == scene && x.Status == ADStatus.RewardeVideoSuccess)
				{
					successFlag = true;
					if (closeFlag && successFlag)
					{
						Mod.Event.FireNow(this, Mod.Reference.Acquire<AdPlayEventArgs>().Initialize(1, (int)scene));
						successCallBack(scene);
						PlayerDataModule.Instance.DealAbilityStarLevel_7();
					}
				}
				else if (x.Scene == scene && x.Status == ADStatus.RewardeVideoStarted)
				{
					if (Mod.Procedure.Current is MenuProcedure)
					{
						AdPauseMusic();
					}
					Mod.Event.FireNow(this, Mod.Reference.Acquire<AdPlayEventArgs>().Initialize(0, (int)scene));
					if (startAdCallBack != null)
					{
						startAdCallBack(scene);
					}
				}
				else if (x.Scene == scene && x.Status == ADStatus.RewardeVideoClosed)
				{
					if (Mod.Procedure.Current is MenuProcedure)
					{
						AdResumeMusic();
					}
					closeFlag = true;
					if (closeFlag && successFlag)
					{
						Mod.Event.FireNow(this, Mod.Reference.Acquire<AdPlayEventArgs>().Initialize(1, (int)scene));
						successCallBack(scene);
						PlayerDataModule.Instance.DealAbilityStarLevel_7();
					}
					if (closeAdCallBack != null)
					{
						closeAdCallBack(scene);
					}
				}
				else
				{
					Mod.Event.FireNow(this, Mod.Reference.Acquire<AdPlayEventArgs>().Initialize(2, (int)scene));
					if (failCallBack != null)
					{
						failCallBack(scene);
					}
				}
			});
		}
		else if (My.Core.Singleton<ADHelper>.Instance.RecommandVideoCanShow(scene, true))
		{
			Log.Info("ADHelper.Instance.RecommandVideoCanShow scene " + scene);
			Mod.Event.FireNow(this, Mod.Reference.Acquire<AdPlayEventArgs>().Initialize(0, (int)scene));
			My.Core.Singleton<ADHelper>.Instance.ShowRecommandVideo(scene, delegate(ADCallbackEventArgs x)
			{
				Log.Info(string.Concat("ShowRecommandVideo x.Scene ", x.Scene, " x.Status ", x.Status));
				if (x.Scene == scene && x.Status == ADStatus.RecommandVideoSuccess)
				{
					Mod.Event.FireNow(this, Mod.Reference.Acquire<AdPlayEventArgs>().Initialize(1, (int)scene));
					successCallBack(scene);
					PlayerDataModule.Instance.DealAbilityStarLevel_7();
				}
				else if (x.Scene == scene && x.Status == ADStatus.RewardeVideoStarted)
				{
					Mod.Event.FireNow(this, Mod.Reference.Acquire<AdPlayEventArgs>().Initialize(0, (int)scene));
					if (startAdCallBack != null)
					{
						startAdCallBack(scene);
					}
				}
				else
				{
					Mod.Event.FireNow(this, Mod.Reference.Acquire<AdPlayEventArgs>().Initialize(2, (int)scene));
					if (failCallBack != null)
					{
						failCallBack(scene);
					}
				}
				if (Mod.Procedure.Current is MenuProcedure)
				{
					AdResumeMusic();
				}
			});
			if (Mod.Procedure.Current is MenuProcedure)
			{
				AdPauseMusic();
			}
			My.Core.Singleton<ADHelper>.Instance.RequestRewardeVideo();
		}
		else
		{
			Log.Info("ADHelper PlayerVideoAd noAdCallBack " + scene);
			My.Core.Singleton<ADHelper>.Instance.RequestRewardeVideo();
			Mod.Event.FireNow(this, Mod.Reference.Acquire<AdPlayEventArgs>().Initialize(3, (int)scene));
			if (noAdCallBack != null)
			{
				noAdCallBack(scene);
			}
		}
	}

	public void GameAppInit()
	{
		RegisterAllDataModules();
		RegisterGloableEvent();
		Singleton<MenuMusicController>.Instance.Init();
		InvokeRepeating("Tick", 0f, 0.02f);
		MonoSingleton<PluginManager>.Instacne.InitThird();
	}

	private void Tick()
	{
		if (!PlayerDataModule.ApplicationQuit)
		{
			TimerHeap.Tick();
		}
	}

	private void RegisterAllDataModules()
	{
		DataModuleManager dataModuleManager = Singleton<DataModuleManager>.Instance;
		dataModuleManager.RegisterDataModule(PlayerDataModule.Instance);
		dataModuleManager.RegisterDataModule(new GameDataModule());
		dataModuleManager.RegisterDataModule(new InsideGameDataModule());
		dataModuleManager.RegisterDataModule(new ResultPageDataModule());
		dataModuleManager.RegisterDataModule(new TutorialDataModule());
		dataModuleManager.RegisterDataModule(new ShopDataModule());
		dataModuleManager.RegisterDataModule(new ExchangeStoreDataModule());
	}

	private void RegisterGloableEvent()
	{
		Mod.Event.Subscribe(EventArgs<UIMod.OpenFailureEventArgs>.EventId, OnOpenUIFormFailed);
	}

	private void OnOpenUIFormFailed(object sender, Foundation.EventArgs e)
	{
		UIMod.OpenFailureEventArgs openFailureEventArgs = (UIMod.OpenFailureEventArgs)e;
		Log.Error(string.Format("Open UIForm failure, asset name '{0}', UIGroup name '{1}', pause covered UI form '{2}', error message '{3}'.", openFailureEventArgs.AssetName, openFailureEventArgs.GroupName, openFailureEventArgs.PauseCoveredUIForm.ToString(), openFailureEventArgs.Message));
	}

	public Sprite CreateSpriteByTexture2D(Texture2D tex)
	{
		return Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100f);
	}

	public void AddGoodsByProductId(string productId)
	{
		Shops_shopTable shops_shopTable = null;
		Shops_shopTable[] record = Mod.DataTable.Get<Shops_shopTable>().Records;
		if (shops_shopTable != null)
		{
			int goodsTeamid = shops_shopTable.GoodsTeamid;
			int count = shops_shopTable.Count;
			Dictionary<int, int>.Enumerator enumerator = DealGoodsTeamById(goodsTeamid, count).GetEnumerator();
			while (enumerator.MoveNext())
			{
				Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).ChangePlayerGoodsNum(enumerator.Current.Key, enumerator.Current.Value);
			}
		}
	}

	public Dictionary<int, int> DealGoodsTeamById(int goodsTeamId, int count = 1)
	{
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		GoodsTeam_goodsTeamTable goodsTeam_goodsTeamTable = Mod.DataTable.Get<GoodsTeam_goodsTeamTable>()[goodsTeamId];
		if (goodsTeam_goodsTeamTable != null)
		{
			string[] array = goodsTeam_goodsTeamTable.GoodsIds.Split('|');
			string[] array2 = goodsTeam_goodsTeamTable.GoodsCount.Split('|');
			for (int i = 0; i < array.Length; i++)
			{
				dictionary.Add(int.Parse(array[i]), int.Parse(array2[i]) * count);
			}
		}
		return dictionary;
	}

	public int GoodsTeamIconId(int goodsTeamId)
	{
		Dictionary<int, int> dictionary = DealGoodsTeamById(goodsTeamId);
		if (dictionary.Count == 1)
		{
			Dictionary<int, int>.Enumerator enumerator = dictionary.GetEnumerator();
			enumerator.MoveNext();
			return GetGoodsIconIdByGoodsId(enumerator.Current.Key);
		}
		GoodsTeam_goodsTeamTable goodsTeam_goodsTeamTable = Mod.DataTable.Get<GoodsTeam_goodsTeamTable>()[goodsTeamId];
		if (goodsTeam_goodsTeamTable != null)
		{
			return goodsTeam_goodsTeamTable.IconId;
		}
		return 0;
	}

	public string GetGoodsTeamDesc(int goodsTeamId)
	{
		GoodsTeam_goodsTeamTable goodsTeam_goodsTeamTable = Mod.DataTable.Get<GoodsTeam_goodsTeamTable>()[goodsTeamId];
		if (goodsTeam_goodsTeamTable != null)
		{
			return Mod.Localization.GetInfoById(goodsTeam_goodsTeamTable.Desc);
		}
		return "";
	}

	public string GetGoodsTeamName(int goodsTeamId)
	{
		GoodsTeam_goodsTeamTable goodsTeam_goodsTeamTable = Mod.DataTable.Get<GoodsTeam_goodsTeamTable>()[goodsTeamId];
		if (goodsTeam_goodsTeamTable != null)
		{
			return Mod.Localization.GetInfoById(goodsTeam_goodsTeamTable.Name);
		}
		return "";
	}

	public string GetGoodsName(int goodsId)
	{
		Goods_goodsTable goods_goodsTable = Mod.DataTable.Get<Goods_goodsTable>()[goodsId];
		return Mod.Localization.GetInfoById(goods_goodsTable.Name);
	}

	public string GetGoodsDesc(int goodsId)
	{
		Goods_goodsTable goods_goodsTable = Mod.DataTable.Get<Goods_goodsTable>()[goodsId];
		return Mod.Localization.GetInfoById(goods_goodsTable.Desc);
	}

	public int GetSceneIdByLevelId(int levelId)
	{
		int result = 0;
		Levels_levelTable levels_levelTable = Mod.DataTable.Get<Levels_levelTable>()[levelId];
		if (levels_levelTable != null)
		{
			Scenes_sceneTable[] records = Mod.DataTable.Get<Scenes_sceneTable>().Records;
			for (int i = 0; i < records.Length; i++)
			{
				if (levels_levelTable.LevelName.Equals(records[i].AssetName))
				{
					result = records[i].Id;
					break;
				}
			}
		}
		return result;
	}

	public void SetTextSpecialFont(string fontName, params Text[] param)
	{
		Mod.Resource.LoadAsset(AssetUtility.GetFontAsset(fontName), new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object userData)
		{
			for (int i = 0; i < param.Length; i++)
			{
				param[i].font = asset as Font;
			}
			Mod.Resource.UnloadAsset(asset);
		}, delegate(string assetName, string status, object userData)
		{
			Log.Error(string.Format("Can not load item '{0}' failed.", assetName));
		}));
	}

	public int GetGoodsIconIdByGoodsId(int goodsId)
	{
		if (goodsId < 0)
		{
			return -1;
		}
		return Mod.DataTable.Get<Goods_goodsTable>()[goodsId].IconId;
	}

	private void OnApplicationPause(bool isPause)
	{
		My.Core.Singleton<ADHelper>.Instance.ApplicationPause(isPause);
	}

	private void OnApplicationQuit()
	{
		PlayerDataModule.ApplicationQuit = true;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		PlayerDataModule.ApplicationQuit = true;
	}

	private bool TempShieldedUmeng()
	{
		return true;
	}

	public bool IsFirstLanuchGameIn24Hour()
	{
		string firstAppLaunchTime = Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).GetFirstAppLaunchTime();
		if (firstAppLaunchTime.Equals(""))
		{
			return false;
		}
		DateTime dateTime = TimeTools.StringToDatetime(firstAppLaunchTime);
		if (!((DateTime.Now - dateTime).TotalHours >= 24.0))
		{
			return true;
		}
		return false;
	}

	public void AdPauseMusic()
	{
	}

	public void AdResumeMusic()
	{
	}

	public bool CanShowAd(ADScene adScene)
	{
		if (My.Core.Singleton<ADHelper>.Instance.RewardeVideoCanShow(adScene) || My.Core.Singleton<ADHelper>.Instance.RecommandVideoCanShow(adScene, true))
		{
			CallIsCanShowAdFailedCount = 0;
			return true;
		}
		CallIsCanShowAdFailedCount++;
		if (CallIsCanShowAdFailedCount % 10 == 0)
		{
			RequreNativeAd();
		}
		return false;
	}

	public void FirstTimeEnterLevelInfoC(int levelId)
	{
		if (MonoSingleton<GameTools>.Instacne.IsFirstLanuchGameIn24Hour() && PlayerDataModule.Instance.PlayerRecordData.GetInfoCGameFillerState(levelId) <= 0)
		{
			InfocUtils.Report_rollingsky2_games_gamefiller(2, levelId);
			PlayerDataModule.Instance.PlayerRecordData.SetInfoCGameFillerState(levelId, 1);
		}
	}

	public int GetPowerCDByAd()
	{
		return GameCommon.powerCdTime;
	}

	public bool CanShowLevleForm()
	{
		return false;
	}

	public void AdapteSpecialScreen(RectTransform target, int adapterY = 50)
	{
		if (target != null)
		{
			target.anchoredPosition = new Vector2(target.anchoredPosition.x, target.anchoredPosition.y - (float)adapterY);
		}
	}

	public int GenerateUnLockState(LevelLockType lockType, int goodsId = 0, ChengJiuType chengjinType = ChengJiuType.NONE)
	{
		return ((int)lockType * 10000 + (int)chengjinType * 10) * -1;
	}

	public int GetLevelTryMaxPercentById(int levelId)
	{
		int result = 0;
		Levels_levelTable levels_levelTable = Mod.DataTable.Get<Levels_levelTable>()[levelId];
		if (levels_levelTable != null)
		{
			result = levels_levelTable.TryPercent;
		}
		return result;
	}

	public void ShowBuyResult(bool isSuccess, int goodsTeamId = 0, int goodsTeamNum = 0)
	{
		if (isSuccess)
		{
			GetGoodsData getGoodsData = new GetGoodsData();
			getGoodsData.GoodsTeamNum = goodsTeamNum;
			getGoodsData.Buy = true;
			getGoodsData.GoodsTeamId = goodsTeamId;
			getGoodsData.GoodsTeam = true;
			getGoodsData.closeCallback = delegate(GetGoodsForm form)
			{
				GetAwardForm form2 = GetAwardForm.Form;
				if ((bool)form2)
				{
					GameObject[] array = GameObject.FindGameObjectsWithTag("ItemMoveTarget");
					UIMoveTarget uIMoveTarget = null;
					GameObject[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						UIMoveTarget component = array2[i].GetComponent<UIMoveTarget>();
						if (!(component == null) && component.id == 203)
						{
							uIMoveTarget = component;
						}
					}
					foreach (KeyValuePair<int, int> item in DealGoodsTeamById(goodsTeamId, goodsTeamNum))
					{
						if (item.Key == 6 && uIMoveTarget != null)
						{
							double playGoodsNum = PlayerDataModule.Instance.GetPlayGoodsNum(6);
							uIMoveTarget.SetData(playGoodsNum - (double)item.Value, playGoodsNum, -1);
							RectTransform component2 = form.m_awardIcon.GetComponent<RectTransform>();
							DisableInputForAWhile(5000u);
							form2.StartMove(component2.position, component2.sizeDelta * 0.65f, "diamound", item.Key, uIMoveTarget, item.Value, delegate
							{
								EnableInput();
							});
						}
					}
				}
				Mod.Event.FireNow(this, Mod.Reference.Acquire<PauseResponseGoodsNumChangeEventArgs>().Initialize(false));
			};
			Singleton<UIPopupManager>.Instance.PopupUI(UIFormId.GetGoodsForm, getGoodsData, UIPopupManager.PriorityType.Priority_50);
		}
		else
		{
			Mod.UI.OpenUIForm(UIFormId.MoneyBuyFailForm, MoneyBuyFailForm.FailType.Fail);
		}
	}

	public bool IsNetWorkReachable()
	{
		return Application.internetReachability == NetworkReachability.NotReachable;
	}

	public int ComputerLevelStar(int levelId, int percent, int diamonds, int crowns)
	{
		int num = 0;
		if (percent >= 100)
		{
			num++;
		}
		Levels_levelTable levels_levelTable = Mod.DataTable.Get<Levels_levelTable>()[levelId];
		if (diamonds >= levels_levelTable.Diamonds)
		{
			num++;
		}
		if (crowns >= levels_levelTable.Crowns)
		{
			num++;
		}
		return num;
	}

	public IEnumerator PauseGameByTime(float lastTime, UnityAction action)
	{
		float tempTime = 0f;
		while (tempTime <= lastTime)
		{
			Mod.Core.Speed = 1f - tempTime / lastTime;
			tempTime += Time.unscaledDeltaTime;
			yield return null;
		}
		if (action != null)
		{
			action();
		}
		yield return null;
	}

	public string TimeFormat_HH_MM_SS(long second, string format = "hh:mm:ss")
	{
		long num = second / 86400;
		long num2 = second % 86400 / 3600;
		long num3 = second % 3600 / 60;
		long num4 = second % 60;
		if (num > 0)
		{
			return string.Format("{0} Days", num);
		}
		format = ((num2 > 9) ? format.Replace("hh", num2.ToString()) : ((num2 <= 0) ? format.Replace("hh", "00") : format.Replace("hh", "0" + num2)));
		format = ((num3 > 9) ? format.Replace("mm", num3.ToString()) : ((num3 <= 0) ? format.Replace("mm", "00") : format.Replace("mm", "0" + num3)));
		format = ((num4 > 9) ? format.Replace("ss", num4.ToString()) : ((num4 <= 0) ? format.Replace("ss", "00") : format.Replace("ss", "0" + num4)));
		return format;
	}

	public string TimeFormat_120Minute(long second)
	{
		long num = second / 3600;
		long num2 = second / 60;
		if (num > 2)
		{
			return num + "hr";
		}
		if (num2 > 0)
		{
			return num2 + "min";
		}
		if (second > 0)
		{
			return num2 + "s";
		}
		return "";
	}

	public string CommonTimeFormat(long totalSeconds, bool colon = false)
	{
		string result = "";
		long ticks = totalSeconds * 10000000;
		TimeSpan timeSpan = new TimeSpan(ticks);
		int days = timeSpan.Days;
		int hours = timeSpan.Hours;
		int minutes = timeSpan.Minutes;
		int seconds = timeSpan.Seconds;
		int num = days / 24;
		if (days > 0)
		{
			result = ((!colon) ? string.Format("{0}d{1}h", days, hours) : string.Format("{0}:{1}", days, hours));
		}
		else if (hours > 0)
		{
			result = ((!colon) ? string.Format("{0}h{1}m", hours, minutes) : string.Format("{0}:{1}", hours, minutes));
		}
		else if (minutes > 0)
		{
			result = ((!colon) ? string.Format("{0}m{1}s", minutes, seconds) : string.Format("{0:D2}:{1:D2}", minutes, seconds));
		}
		else if (seconds >= 0)
		{
			result = ((!colon) ? string.Format("{0}s", seconds) : string.Format("{0:D2}:{1:D2}", minutes, seconds));
		}
		return result;
	}

	public string CommonTimeFormatForOne(long totalSeconds, int minHours = 0)
	{
		string result = "";
		long ticks = totalSeconds * 10000000;
		TimeSpan timeSpan = new TimeSpan(ticks);
		int days = timeSpan.Days;
		int hours = timeSpan.Hours;
		int minutes = timeSpan.Minutes;
		int seconds = timeSpan.Seconds;
		int num = days / 24;
		if (days > 0)
		{
			result = string.Format("{0}d", days);
		}
		else if (hours > minHours)
		{
			result = string.Format("{0}h", hours);
		}
		else if (minutes > 0)
		{
			result = string.Format("{0}m", minutes);
		}
		else if (seconds >= 0)
		{
			result = string.Format("{0}s", seconds);
		}
		return result;
	}

	public string CommonTimeFormatMinutes(long totalSeconds)
	{
		long ticks = totalSeconds * 10000000;
		return string.Format("{0}m", new TimeSpan(ticks).Minutes);
	}

	public string GetLeftTimeToday()
	{
		return TimeFormat_HH_MM_SS((long)GetLeftTimeTodayTotalSeconds());
	}

	public double GetLeftTimeTodayTotalSeconds()
	{
		int year = DateTime.Now.Year;
		int month = DateTime.Now.Month;
		int day = DateTime.Now.Day;
		return (new DateTime(year, month, day, 23, 59, 59) - DateTime.Now).TotalSeconds;
	}

	public int GetLevelMuiscPercent(int levelId)
	{
		int num = Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).GetPlayerLevelData(levelId).MaxProgress;
		Levels_levelTable levelTableById = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).GetLevelTableById(levelId);
		if (num < levelTableById.DefaultMusicPercent)
		{
			num = levelTableById.DefaultMusicPercent;
		}
		return num;
	}

	public float UIScale()
	{
		float num = 1280f;
		float num2 = 720f / num;
		float num3 = (float)Screen.width * 1f;
		float num4 = (float)Screen.height * 1f;
		float num5 = num3 / num4;
		if (num5 > num2)
		{
			return 1f;
		}
		return num5 / num2;
	}

	public void CommonBuyOperate(int shopId, UIFormId id = UIFormId.Undefined)
	{
		if (shopId < 1)
		{
			return;
		}
		if (id != 0)
		{
			PlayerDataModule.Instance.PluginAdController.CommonBuyRecord.Add(id);
		}
		if (Mod.DataTable.Get<Shops_shopTable>()[shopId] != null)
		{
			if (!HasInternet())
			{
				Mod.UI.OpenUIForm(UIFormId.MoneyBuyFailForm, MoneyBuyFailForm.FailType.UnNet);
			}
			InfocUtils.Report_rollingsky2_games_neigou(shopId, 2);
		}
	}

	public bool HasInternet()
	{
		return Application.internetReachability != NetworkReachability.NotReachable;
	}

	public bool IsOpenBuffFunction()
	{
		return PlayerDataModule.Instance.GetPlayerInLevelTotalTime() >= GameCommon.openBuffFunctionLevelTime;
	}

	public float GetGlobalCanvasWith()
	{
		RectTransform rectTransform = Mod.UI.GetUIGroup("First").transform as RectTransform;
		if (rectTransform != null)
		{
			return rectTransform.sizeDelta.x;
		}
		return 720f;
	}

	public Color32 GetSeriesBgColor(int seriesID)
	{
		LevelSeries_table levelSeries_table = Mod.DataTable.Get<LevelSeries_table>()[seriesID];
		Color32 result = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
		if (levelSeries_table != null)
		{
			string[] array = levelSeries_table.LevelBgColor.Split('|');
			for (int i = 0; i < array.Length; i++)
			{
				int.Parse(array[i]);
				switch (i)
				{
				case 0:
					result.r = (byte)int.Parse(array[i]);
					break;
				case 1:
					result.g = (byte)int.Parse(array[i]);
					break;
				case 2:
					result.b = (byte)int.Parse(array[i]);
					break;
				case 3:
					result.a = (byte)int.Parse(array[i]);
					break;
				}
			}
		}
		return result;
	}

	public bool IsCanOperateBackToMenu()
	{
		bool result = true;
		int currentStageId = TutorialManager.Instance.GetCurrentStageId();
		if (currentStageId == TutorialStageId.STAGE_TUTORIAL_LEVEL || currentStageId == TutorialStageId.STAGE_HOME_MENU)
		{
			result = false;
		}
		return result;
	}

	public bool IsNewPlayerGuideLevel()
	{
		return TutorialManager.Instance.GetCurrentStageId() == TutorialStageId.STAGE_TUTORIAL_LEVEL;
	}

	public string CountSize(int size)
	{
		string result = "";
		float num = 0f;
		num = size;
		if (num < 1024f)
		{
			result = num.ToString("F2") + "Byte";
		}
		else if (num >= 1024f && num < 1048576f)
		{
			result = (num / 1024f).ToString("F2") + "K";
		}
		else if (num >= 1048576f && num < 1.07374182E+09f)
		{
			result = (num / 1024f / 1024f).ToString("F2") + "M";
		}
		else if (num >= 1.07374182E+09f)
		{
			result = (num / 1024f / 1024f / 1024f).ToString("F2") + "G";
		}
		return result;
	}

	public Transform RecursiveFindChild(Transform parent, string childName)
	{
		Transform transform = null;
		Transform transform2 = null;
		for (int i = 0; i < parent.childCount; i++)
		{
			transform2 = parent.GetChild(i);
			if (transform2.name.Equals(childName))
			{
				return transform2;
			}
			transform = RecursiveFindChild(transform2, childName);
			if (transform != null && transform.name.Equals(childName))
			{
				return transform;
			}
		}
		return transform;
	}

	public List<int> GetAllLevelsBySeriesId(int seriesId)
	{
		List<int> list = new List<int>();
		LevelSeries_table levelSeries_table = Mod.DataTable.Get<LevelSeries_table>()[seriesId];
		if (levelSeries_table != null)
		{
			string[] array = levelSeries_table.LevelChapters.Split('|');
			int result = -1;
			for (int i = 0; i < array.Length; i++)
			{
				if (int.TryParse(array[i], out result))
				{
					List<int> allLevelsByChapterId = GetAllLevelsByChapterId(result);
					list.AddRange(allLevelsByChapterId);
				}
			}
		}
		return list;
	}

	public List<int> GetAllLevelsByChapterId(int chapterId)
	{
		List<int> list = new List<int>();
		LevelChapters_table levelChapters_table = Mod.DataTable.Get<LevelChapters_table>()[chapterId];
		if (levelChapters_table != null)
		{
			string[] array = levelChapters_table.Sections.Split('|');
			int result = -1;
			for (int i = 0; i < array.Length; i++)
			{
				if (int.TryParse(array[i], out result))
				{
					List<int> allLevelsBySectionId = GetAllLevelsBySectionId(result);
					list.AddRange(allLevelsBySectionId);
				}
			}
		}
		return list;
	}

	public List<int> GetAllLevelsBySectionId(int sectionsId)
	{
		List<int> list = new List<int>();
		LevelSections_table levelSections_table = Mod.DataTable.Get<LevelSections_table>()[sectionsId];
		if (levelSections_table != null)
		{
			string[] array = levelSections_table.Levels.Split('|');
			int result = -1;
			for (int i = 0; i < array.Length; i++)
			{
				if (int.TryParse(array[i], out result))
				{
					list.Add(result);
				}
			}
		}
		return list;
	}

	public string GetLevelNameById(int levelId)
	{
		Levels_levelTable levels_levelTable = Mod.DataTable.Get<Levels_levelTable>()[levelId];
		if (levels_levelTable != null)
		{
			return levels_levelTable.LevelName;
		}
		return "";
	}

	public List<int> StringToIntList(string source, char charactor = '|')
	{
		List<int> list = new List<int>();
		string[] array = source.Split(charactor);
		int result = -1;
		for (int i = 0; i < array.Length; i++)
		{
			if (int.TryParse(array[i], out result))
			{
				list.Add(result);
			}
		}
		return list;
	}

	public List<double> StringToDoubleList(string source, char charactor = '|')
	{
		List<double> list = new List<double>();
		string[] array = source.Split(charactor);
		double result = -1.0;
		for (int i = 0; i < array.Length; i++)
		{
			if (double.TryParse(array[i], out result))
			{
				list.Add(result);
			}
		}
		return list;
	}

	public List<long> StringToLongList(string source, char charactor = '|')
	{
		List<long> list = new List<long>();
		string[] array = source.Split(charactor);
		long result = -1L;
		for (int i = 0; i < array.Length; i++)
		{
			if (long.TryParse(array[i], out result))
			{
				list.Add(result);
			}
		}
		return list;
	}

	public bool IsClickInItemZone(RectTransform sourceObject)
	{
		Vector3 validScreenPos = GetValidScreenPos(Input.mousePosition);
		Vector3 position = Mod.UI.UICamera.ScreenToWorldPoint(validScreenPos);
		Vector3 point = sourceObject.InverseTransformPoint(position);
		return sourceObject.rect.Contains(point);
	}

	public bool CanShowGiftIcon()
	{
		bool flag = PlayerDataModule.Instance.IsAppFirstLaunch();
		bool num = PlayerDataModule.Instance.GetPlayerLevelData(GameCommon.THIRD_LEVEL).MaxProgress >= 1;
		bool flag2 = PlayerDataModule.Instance.GetPlayerLevelData(GameCommon.FIRST_LEVEL).MaxProgress >= 1;
		int nextGiftId = PlayerDataModule.Instance.PlayerGiftPackageData.GetNextGiftId();
		if ((num || (flag2 && !flag)) && nextGiftId != -1)
		{
			return true;
		}
		return false;
	}

	public bool CanAutoShowGiftForm()
	{
		PlayerLocalGiftpackageData playerGiftPackageData = PlayerDataModule.Instance.PlayerGiftPackageData;
		bool isHadRecommand = playerGiftPackageData.IsHadRecommand;
		if (CanShowGiftIcon() && (!isHadRecommand || playerGiftPackageData.CanGetFree()) && TutorialManager.Instance.IsTutorialStageFinish(TutorialStageId.STAGE_TAPTAP))
		{
			return true;
		}
		return false;
	}

	public void RequreNativeAd()
	{
		if (!My.Core.Singleton<ADHelper>.Instance.RewardeVideoCanShow(ADScene.NONE))
		{
			My.Core.Singleton<ADHelper>.Instance.RequestRewardeVideo();
		}
		if (!My.Core.Singleton<ADHelper>.Instance.InterstitialCanShow(ADScene.NONE))
		{
			My.Core.Singleton<ADHelper>.Instance.RequestInterstitial();
		}
	}

	public bool IsNullAndLogError(UnityEngine.Object obj, string errorString)
	{
		if (obj == null)
		{
			Log.Error(errorString);
			return true;
		}
		return false;
	}

	public bool IsNullAndLogWarning(UnityEngine.Object obj, string errorString)
	{
		if (obj == null)
		{
			Log.Warning(errorString);
			return true;
		}
		return false;
	}

	public bool IsNullAndNoLog(UnityEngine.Object obj)
	{
		return obj == null;
	}

	public bool IsNullAndLogInfo(UnityEngine.Object obj, string errorString)
	{
		if (obj == null)
		{
			Log.Info(errorString);
			return true;
		}
		return false;
	}

	public void AFReportRetention()
	{
		if (EncodeConfig.getConfig(PlayerLocalDatakey.AFREPORTRETENTION, "0").Equals("0"))
		{
			int day = TimeTools.StringToDatetime(Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).GetFirstAppLaunchTime()).Day;
			int day2 = DateTime.Now.Day;
			if (day != day2)
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("loginDays", "1");
				AppsFlyer.trackRichEvent("retention-day-1", dictionary);
				EncodeConfig.setConfig(PlayerLocalDatakey.AFREPORTRETENTION, "1");
			}
		}
	}

	public string GetProductRealPrice(int productId)
	{
		string result = "";
		Shops_shopTable shops_shopTable = Mod.DataTable.Get<Shops_shopTable>()[productId];
		if (shops_shopTable == null)
		{
			return result;
		}
		result = shops_shopTable.Price;
		ProductInfo productInfo = MonoSingleton<PluginManager>.Instacne.GetProductInfo();
		string product_id_ios = shops_shopTable.Product_id_ios;
		if (productInfo != null)
		{
			for (int i = 0; i < productInfo.productinInfoList.Count; i++)
			{
				if (productInfo.productinInfoList[i].productId == product_id_ios)
				{
					result = productInfo.productinInfoList[i].price;
					break;
				}
			}
		}
		return result;
	}

	public void CommonRestoreBuy()
	{
		Shops_shopTable[] records = Mod.DataTable.Get<Shops_shopTable>().Records;
		List<string> list = new List<string>();
		for (int i = 0; i < records.Length; i++)
		{
			int canRecover = records[i].CanRecover;
			int num = 1;
		}
		if (list.Count > 0)
		{
			Mod.UI.OpenUIForm(UIFormId.ShopLoadingForm);
			My.Core.Singleton<IAPHelper>.Instance.RestoreProduct(list.ToArray());
		}
	}

	public Vector3 GetValidScreenPos(Vector3 inputPos)
	{
		float num = inputPos.x;
		float num2 = inputPos.y;
		if (num <= 0f)
		{
			num = 0.001f;
		}
		if (num >= (float)Screen.width)
		{
			num = Screen.width - 1;
		}
		if (num2 <= 0f)
		{
			num2 = 0.001f;
		}
		if (num2 >= (float)Screen.height)
		{
			num2 = Screen.height - 1;
		}
		return new Vector3(num, num2, inputPos.z);
	}

	public int ComputerScreenPluginAdId(int souceId)
	{
		return GameCommon.screenPlguinStartId + souceId;
	}

	public bool NeedAutoShowLoginAward()
	{
		bool flag = PlayerDataModule.Instance.IsAppFirstLaunch();
		bool isLoginInDifferentDay = PlayerDataModule.Instance.IsLoginInDifferentDay;
		if (TutorialManager.Instance.GetCurrentStageId() != TutorialStageId.STAGE_FIRST_LEVEL && PlayerDataModule.Instance.SequenceLoginData.IsCanShowGetForm() && Time.realtimeSinceStartup > 30f && TutorialManager.Instance.IsTutorialStageFinish(TutorialStageId.STAGE_TAPTAP) && (flag || isLoginInDifferentDay))
		{
			return true;
		}
		return false;
	}

	public bool NeedAutoShowOfflineReward()
	{
		return TutorialManager.Instance.IsTutorialStageFinish(TutorialStageId.STAGE_TAPTAP) & (PlayerDataModule.Instance.OffLineTime > GameCommon.offlineProductMinTime);
	}

	public string ProductsCountToString(double number)
	{
		SystemLanguage language = Mod.Localization.Language;
		string empty = string.Empty;
		if ((uint)(language - 40) <= 1u)
		{
			if (number >= 10000.0)
			{
				if (number % 10000.0 == 0.0)
				{
					return number / 10000.0 + Mod.Localization.GetInfoById(196);
				}
				return (number * 1.0 / 10000.0).ToString(".0") + Mod.Localization.GetInfoById(196);
			}
			return number.ToString();
		}
		if (number >= 1000.0)
		{
			if (number % 1000.0 == 0.0)
			{
				return number / 1000.0 + "k";
			}
			return (number * 1.0 / 1000.0).ToString(".0") + "k";
		}
		return number.ToString();
	}

	public bool IsPreparing(int levelId)
	{
		bool result = false;
		Levels_levelTable levels_levelTable = Mod.DataTable.Get<Levels_levelTable>()[levelId];
		if (levels_levelTable != null)
		{
			Scenes_sceneTable[] records = Mod.DataTable.Get<Scenes_sceneTable>().Records;
			for (int i = 0; i < records.Length; i++)
			{
				if (records[i].AssetName.Equals(levels_levelTable.LevelName))
				{
					result = records[i].IsInVersion == 0;
					break;
				}
			}
		}
		return result;
	}

	public bool NeedShowRemoveAd()
	{
		bool flag = PlayerDataModule.Instance.IsInNewPlayerProtectedStage();
		bool flag2 = PlayerDataModule.Instance.GetPlayGoodsNum(GameCommon.REMOVE_AD) > 0.0;
		bool flag3 = PlayerDataModule.Instance.PlayerRecordData.m_shopRemoveAdFormOpenCount == 0;
		bool flag4 = PlayerDataModule.Instance.PlayerRecordData.m_ScreenPluginsFormOpenTime == 0;
		bool flag5 = PlayerDataModule.Instance.PlayerRecordData.m_currentShowRemoveAdForScreenCount >= GameCommon.showRemoveAdForScreenCount;
		if (!IsHomeForm())
		{
			return false;
		}
		if (flag || flag4 || flag2)
		{
			return false;
		}
		if (Mod.UI.UIFormIsOpen(UIFormId.LevelEnterForm))
		{
			return false;
		}
		if (flag3)
		{
			return true;
		}
		if (flag5 && !flag3)
		{
			return true;
		}
		return false;
	}

	private bool IsHomeForm()
	{
		if (Mod.UI.UIFormIsOpen(UIFormId.MenuViewForm))
		{
			MenuForm menuForm = Mod.UI.GetUIForm(UIFormId.MenuViewForm) as MenuForm;
			if (menuForm != null && menuForm.CurrentOpenFormId == UIFormId.HomeForm)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsLevelInVersion(int levelId)
	{
		bool result = false;
		Levels_levelTable levels_levelTable = Mod.DataTable.Get<Levels_levelTable>()[levelId];
		if (levels_levelTable != null)
		{
			Scenes_sceneTable[] records = Mod.DataTable.LoadOrGet<Scenes_sceneTable>().Records;
			foreach (Scenes_sceneTable scenes_sceneTable in records)
			{
				if (scenes_sceneTable.IsInVersion == 1 && levels_levelTable.LevelName.Equals(scenes_sceneTable.AssetName))
				{
					return true;
				}
			}
		}
		return result;
	}

	public void AddGoodsByShopTable(int shopTableId)
	{
		AddGoodsByShopTable(Mod.DataTable.Get<Shops_shopTable>().Get(shopTableId));
	}

	public void AddGoodsByShopTable(Shops_shopTable shopTable)
	{
		if (shopTable != null)
		{
			int goodsTeamid = shopTable.GoodsTeamid;
			int count = shopTable.Count;
			Dictionary<int, int>.Enumerator enumerator = MonoSingleton<GameTools>.Instacne.DealGoodsTeamById(goodsTeamid, count).GetEnumerator();
			PlayerDataModule playerDataModule = PlayerDataModule.Instance;
			while (enumerator.MoveNext())
			{
				playerDataModule.ChangePlayerGoodsNum(enumerator.Current.Key, enumerator.Current.Value, AssertChangeType.SHOP_REALMONEY);
			}
			Mod.Event.Fire(this, Mod.Reference.Acquire<BuySuccessEventArgs>().Initialize(shopTable.Id));
		}
	}

	public void DelayForTest(float delayTime, UnityAction delayCallback)
	{
		StartCoroutine(DoDelay(delayTime, delayCallback));
	}

	private IEnumerator DoDelay(float delayTime, UnityAction delayCallback)
	{
		yield return new WaitForSeconds(delayTime);
		if (delayCallback != null)
		{
			delayCallback();
		}
	}

	public string DoubleToFormatString(double num, string format = "0.0")
	{
		string text = "";
		double num2 = 1000.0;
		double num3 = 1000.0 * num2;
		double num4 = 1000.0 * num3;
		double num5 = 1000.0 * num4;
		double num6 = 1000.0 * num5;
		double num7 = 1000.0 * num6;
		double num8 = 1000.0 * num7;
		double num9 = 1000.0 * num8;
		double num10 = 1000.0 * num9;
		double num11 = 1000.0 * num10;
		double num12 = 1000.0 * num11;
		double num13 = 1000.0 * num12;
		double num14 = 1000.0 * num13;
		double num15 = 1000.0 * num14;
		double num16 = 1000.0 * num15;
		double num17 = 1000.0 * num16;
		double num18 = 1000.0 * num17;
		double num19 = 1000.0 * num18;
		double num20 = 1000.0 * num19;
		double num21 = 1000.0 * num20;
		double num22 = 1000.0 * num21;
		if (num > num22)
		{
			return string.Format("{0}V", (num / num22).ToString(format));
		}
		if (num > num21)
		{
			return string.Format("{0}No", (num / num21).ToString(format));
		}
		if (num > num20)
		{
			return string.Format("{0}Oc", (num / num20).ToString(format));
		}
		if (num > num19)
		{
			return string.Format("{0}Se", (num / num19).ToString(format));
		}
		if (num > num18)
		{
			return string.Format("{0}se", (num / num18).ToString(format));
		}
		if (num > num17)
		{
			return string.Format("{0}Qu", (num / num17).ToString(format));
		}
		if (num > num16)
		{
			return string.Format("{0}qu", (num / num16).ToString(format));
		}
		if (num > num15)
		{
			return string.Format("{0}Tr", (num / num15).ToString(format));
		}
		if (num > num14)
		{
			return string.Format("{0}D", (num / num14).ToString(format));
		}
		if (num > num13)
		{
			return string.Format("{0}U", (num / num13).ToString(format));
		}
		if (num > num12)
		{
			return string.Format("{0}d", (num / num12).ToString(format));
		}
		if (num > num11)
		{
			return string.Format("{0}N", (num / num11).ToString(format));
		}
		if (num > num10)
		{
			return string.Format("{0}O", (num / num10).ToString(format));
		}
		if (num > num9)
		{
			return string.Format("{0}S", (num / num9).ToString(format));
		}
		if (num > num8)
		{
			return string.Format("{0}s", (num / num8).ToString(format));
		}
		if (num > num7)
		{
			return string.Format("{0}Q", (num / num7).ToString(format));
		}
		if (num > num6)
		{
			return string.Format("{0}q", (num / num6).ToString(format));
		}
		if (num > num5)
		{
			return string.Format("{0}T", (num / num5).ToString(format));
		}
		if (num > num4)
		{
			return string.Format("{0}B", (num / num4).ToString(format));
		}
		if (num > num3)
		{
			return string.Format("{0}M", (num / num3).ToString(format));
		}
		if (num > num2 * 10.0)
		{
			return string.Format("{0}K", (num / num2).ToString(format));
		}
		return num.ToString("0");
	}

	public bool CheckSeriesLock(int seriesId)
	{
		bool result = false;
		LevelSeries_table levelSeries_table = Mod.DataTable.Get<LevelSeries_table>()[seriesId];
		if (levelSeries_table == null)
		{
			return true;
		}
		int unlockStar = levelSeries_table.UnlockStar;
		int unlockLevel = levelSeries_table.UnlockLevel;
		int playerStarLevel = PlayerDataModule.Instance.GetPlayerStarLevel();
		int playerLevel = PlayerDataModule.Instance.GetPlayerLevel();
		if (unlockStar > playerStarLevel)
		{
			result = true;
		}
		else if (unlockStar == playerStarLevel && unlockLevel > playerLevel)
		{
			result = true;
		}
		foreach (LevelMetaTableData item in PlayerDataModule.Instance.GloableLevelLableData.FindAll((LevelMetaTableData x) => x.SeriesId == seriesId))
		{
			PlayerLocalLevelData playerLevelData = PlayerDataModule.Instance.GetPlayerLevelData(item.LevelId);
			if (playerLevelData != null && playerLevelData.MaxProgress > 0)
			{
				return false;
			}
		}
		return result;
	}
}
