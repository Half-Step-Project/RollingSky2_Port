using System.Collections.Generic;
using Foundation;
using UnityEngine;

namespace RS2
{
	internal sealed class PreloadProcedure : BaseProcedure
	{
		private readonly Dictionary<string, bool> _loadedFlag = new Dictionary<string, bool>();

		private BuiltinDialogParams _buildDialogParams;

		protected override void OnEnter(IFsm<ProcedureMod> procedureOwner)
		{
			ShaderVariantsWarm();
			base.OnEnter(procedureOwner);
			_buildDialogParams.ShowType = BuiltinDialogShowType.Info;
			_buildDialogParams.InfoMessage = Mod.Localization.Get("PreloadProcedure.PreloadData");
			BuiltinDialogForm.OpenDialog(_buildDialogParams);
			Mod.Event.Subscribe(EventArgs<ConfigMod.LoadSuccessEventArgs>.EventId, OnLoadConfigSuccess);
			Mod.Event.Subscribe(EventArgs<ConfigMod.LoadFailureEventArgs>.EventId, OnLoadConfigFailure);
			Mod.Event.Subscribe(EventArgs<DataTableMod.LoadSuccessEventArgs>.EventId, OnLoadDataTableSuccess);
			Mod.Event.Subscribe(EventArgs<DataTableMod.LoadFailureEventArgs>.EventId, OnLoadDataTableFailure);
			Mod.Event.Subscribe(EventArgs<LocalizationMod.LoadSuccessEventArgs>.EventId, OnLoadDictionarySuccess);
			Mod.Event.Subscribe(EventArgs<LocalizationMod.LoadFailureEventArgs>.EventId, OnLoadDictionaryFailure);
			_loadedFlag.Clear();
			PreloadResources();
		}

		protected override void OnLeave(IFsm<ProcedureMod> procedureOwner, bool isShutdown)
		{
			Mod.Event.Unsubscribe(EventArgs<ConfigMod.LoadSuccessEventArgs>.EventId, OnLoadConfigSuccess);
			Mod.Event.Unsubscribe(EventArgs<ConfigMod.LoadFailureEventArgs>.EventId, OnLoadConfigFailure);
			Mod.Event.Unsubscribe(EventArgs<DataTableMod.LoadSuccessEventArgs>.EventId, OnLoadDataTableSuccess);
			Mod.Event.Unsubscribe(EventArgs<DataTableMod.LoadFailureEventArgs>.EventId, OnLoadDataTableFailure);
			Mod.Event.Unsubscribe(EventArgs<LocalizationMod.LoadSuccessEventArgs>.EventId, OnLoadDictionarySuccess);
			Mod.Event.Unsubscribe(EventArgs<LocalizationMod.LoadFailureEventArgs>.EventId, OnLoadDictionaryFailure);
			base.OnLeave(procedureOwner, isShutdown);
		}

		protected override void OnTick(IFsm<ProcedureMod> procedureOwner, float elapseSeconds, float realElapseSeconds)
		{
			base.OnTick(procedureOwner, elapseSeconds, realElapseSeconds);
			foreach (KeyValuePair<string, bool> item in _loadedFlag)
			{
				if (!item.Value)
				{
					return;
				}
			}
			if (!PlayerDataModule.Instance.BeginInit)
			{
				PlayerDataModule.Instance.Init();
			}
			if (PlayerDataModule.Instance.IsInited)
			{
				MonoSingleton<GameTools>.Instacne.GameAppInit();
				Singleton<UIPopupManager>.Instance.Init();
				int num = -1;
				if (TutorialManager.Instance.GetCurrentStageId() == TutorialStageId.STAGE_TUTORIAL_LEVEL)
				{
					num = 10000;
				}
				if (num > 0)
				{
					int sceneIdByLevelId = MonoSingleton<GameTools>.Instacne.GetSceneIdByLevelId(num);
					Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).CurLevelId = num;
					procedureOwner.SetData("NextSceneId", (VarInt)sceneIdByLevelId);
				}
				else
				{
					HomeForm.CurrentSeriesId = PlayerDataModule.Instance.FirstEnterSeriesId();
					procedureOwner.SetData("NextSceneId", (VarInt)1);
				}
				PlayerDataModule.Instance.SetAppLocalVersion();
				ChangeState<SwitchSceneProcedure>(procedureOwner);
			}
		}

		private void PreloadResources()
		{
			LoadDataTable<Scenes_sceneTable>("Scenes_sceneTable");
			LoadDataTable<UIForms_uiformTable>("UIForms_uiformTable");
			LoadDataTable<Award_awardTable>("Award_awardTable");
			LoadDataTable<CommonData_commonDataTable>("CommonData_commonDataTable");
			LoadDataTable<Goods_goodsTable>("Goods_goodsTable");
			LoadDataTable<GoodsTeam_goodsTeamTable>("GoodsTeam_goodsTeamTable");
			LoadDataTable<Language_languagetable>("Language_languagetable");
			LoadDataTable<Levels_levelTable>("Levels_levelTable");
			LoadDataTable<LevelTarget_levelTargetTable>("LevelTarget_levelTargetTable");
			LoadDataTable<Shops_shopTable>("Shops_shopTable");
			LoadDataTable<UISound_uiSoundTable>("UISound_uiSoundTable");
			LoadDataTable<LevelOrder_levelOrderTable>("LevelOrder_levelOrderTable");
			LoadDataTable<Music_levelMusicTable>("Music_levelMusicTable");
			LoadDataTable<LevelUnLock_table>("LevelUnLock_table");
			LoadDataTable<LevelProgressAward_table>("LevelProgressAward_table");
			LoadDataTable<SequenceLoginAward_table>("SequenceLoginAward_table");
			LoadDataTable<OriginRebirth_Discount>("OriginRebirth_Discount");
			LoadDataTable<LevelsResolution_table>("LevelsResolution_table");
			LoadDataTable<MotivateReward_needEnergy>("MotivateReward_needEnergy");
			LoadDataTable<MotivateReward_awardSuperset>("MotivateReward_awardSuperset");
			LoadDataTable<MotivateReward_awardSubset>("MotivateReward_awardSubset");
			LoadDataTable<MotivateReward_award>("MotivateReward_award");
			LoadDataTable<LevelSeries_table>("LevelSeries_table");
			LoadDataTable<LevelChapters_table>("LevelChapters_table");
			LoadDataTable<LevelSections_table>("LevelSections_table");
			LoadDataTable<VideoAward_videoAwardTable>("VideoAward_videoAwardTable");
			LoadDataTable<ScreenPluginsAd_table>("ScreenPluginsAd_table");
			LoadDataTable<Instrument_table>("Instrument_table");
			LoadDataTable<InstrumentGrow_1>("InstrumentGrow_1");
			LoadDataTable<InstrumentGrow_2>("InstrumentGrow_2");
			LoadDataTable<InstrumentGrow_3>("InstrumentGrow_3");
			LoadDataTable<InstrumentGrow_4>("InstrumentGrow_4");
			LoadDataTable<InstrumentGrow_5>("InstrumentGrow_5");
			LoadDataTable<InstrumentGrow_6>("InstrumentGrow_6");
			LoadDataTable<InstrumentGrow_7>("InstrumentGrow_7");
			LoadDataTable<InstrumentGrow_8>("InstrumentGrow_8");
			LoadDataTable<InstrumentGrow_9>("InstrumentGrow_9");
			LoadDataTable<InstrumentGrow_10>("InstrumentGrow_10");
			LoadDataTable<InstrumentGrow_11>("InstrumentGrow_11");
			LoadDataTable<InstrumentGrow_12>("InstrumentGrow_12");
			LoadDataTable<InstrumentGrow_13>("InstrumentGrow_13");
			LoadDataTable<InstrumentGrow_14>("InstrumentGrow_14");
			LoadDataTable<InstrumentGrow_15>("InstrumentGrow_15");
			LoadDataTable<InstrumentGrow_16>("InstrumentGrow_16");
			LoadDataTable<PlayerLevel_table>("PlayerLevel_table");
			LoadDataTable<PlayerMission_table>("PlayerMission_table");
			LoadDataTable<PlayerStarLevel_table>("PlayerStarLevel_table");
			LoadDataTable<PlayerStarAbility_table>("PlayerStarAbility_table");
			LoadDataTable<InstrumentBatonOffLineBaseProduct_table>("InstrumentBatonOffLineBaseProduct_table");
			LoadDataTable<ExchangeStore_table>("ExchangeStore_table");
			LoadDataTable<LuckyTurntable_table>("LuckyTurntable_table");
		}

		private void LoadConfig(string configName)
		{
			string configAsset = AssetUtility.GetConfigAsset(configName);
			_loadedFlag.Add(configName, false);
			Mod.Config.Load(configAsset, this);
		}

		private void LoadDataTable<T>(string name) where T : IRecord
		{
			string dataTableAsset = AssetUtility.GetDataTableAsset(name);
			_loadedFlag.Add(dataTableAsset, false);
			Mod.DataTable.Load<T>(dataTableAsset, this);
		}

		private void LoadDictionary(string dictionaryName)
		{
			string dictionaryAsset = AssetUtility.GetDictionaryAsset(dictionaryName);
			_loadedFlag.Add(dictionaryAsset, false);
			Mod.Localization.Load(dictionaryAsset, this);
		}

		private void ShaderVariantsWarm()
		{
			Mod.Resource.LoadAsset(AssetUtility.GetShaderVariantsAsset("ShaderVariants"), new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object userData)
			{
				ShaderVariantCollection shaderVariantCollection = asset as ShaderVariantCollection;
				if (shaderVariantCollection != null)
				{
					shaderVariantCollection.WarmUp();
				}
				Mod.Resource.UnloadAsset(asset);
			}, delegate(string assetName, string errorMessage, object userData)
			{
				Log.Error(string.Format("Can not load Load ShaderVariants from '{0}' with error message '{1}", assetName, errorMessage));
			}));
		}

		private void OnLoadConfigSuccess(object sender, EventArgs e)
		{
			ConfigMod.LoadSuccessEventArgs loadSuccessEventArgs = (ConfigMod.LoadSuccessEventArgs)e;
			if (loadSuccessEventArgs.UserData == this)
			{
				_loadedFlag[loadSuccessEventArgs.AssetName] = true;
			}
		}

		private void OnLoadConfigFailure(object sender, EventArgs e)
		{
			ConfigMod.LoadFailureEventArgs loadFailureEventArgs = (ConfigMod.LoadFailureEventArgs)e;
			if (loadFailureEventArgs.UserData == this)
			{
				Log.Error(string.Format("Can not load config '{0}' with error message '{1}'.", loadFailureEventArgs.AssetName, loadFailureEventArgs.Message));
			}
		}

		private void OnLoadDataTableSuccess(object sender, EventArgs e)
		{
			DataTableMod.LoadSuccessEventArgs loadSuccessEventArgs = (DataTableMod.LoadSuccessEventArgs)e;
			if (loadSuccessEventArgs.UserData == this)
			{
				_loadedFlag[loadSuccessEventArgs.AssetName] = true;
			}
		}

		private void OnLoadDataTableFailure(object sender, EventArgs e)
		{
			DataTableMod.LoadFailureEventArgs loadFailureEventArgs = (DataTableMod.LoadFailureEventArgs)e;
			if (loadFailureEventArgs.UserData == this)
			{
				Log.Error(string.Format("Can not load data table '{0}' with error message '{1}'.", loadFailureEventArgs.AssetName, loadFailureEventArgs.Message));
			}
		}

		private void OnLoadDictionarySuccess(object sender, EventArgs e)
		{
			LocalizationMod.LoadSuccessEventArgs loadSuccessEventArgs = (LocalizationMod.LoadSuccessEventArgs)e;
			if (loadSuccessEventArgs.UserData == this)
			{
				_loadedFlag[loadSuccessEventArgs.AssetName] = true;
			}
		}

		private void OnLoadDictionaryFailure(object sender, EventArgs e)
		{
			LocalizationMod.LoadFailureEventArgs loadFailureEventArgs = (LocalizationMod.LoadFailureEventArgs)e;
			if (loadFailureEventArgs.UserData == this)
			{
				Log.Error(string.Format("Can not load dictionary '{0}' with error message '{1}'.", loadFailureEventArgs.AssetName, loadFailureEventArgs.Message));
			}
		}
	}
}
