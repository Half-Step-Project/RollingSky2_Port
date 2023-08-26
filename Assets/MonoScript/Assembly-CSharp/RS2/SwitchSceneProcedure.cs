using System;
using System.Collections.Generic;
using Foundation;
using Foundation.Bugly;
using UnityEngine;
using UnityEngine.SceneManagement;
using User.TileMap;

namespace RS2
{
	internal sealed class SwitchSceneProcedure : BaseProcedure
	{
		private enum SwitchSceneStage
		{
			StartLoading,
			LoadingScene,
			LoadingLevelResources,
			LoadingUIResources,
			LoadingFinished
		}

		private int _switchToSceneId;

		private string _switchToSceneName;

		private SwitchSceneStage _stage;

		private LoadingForm _loadingForm;

		private HomeForm _homeForm;

		private readonly Dictionary<UIFormId, bool> _preOpenFormIdsFlag = new Dictionary<UIFormId, bool>();

		private float _loadProgressOffset;

		private float _loadSceneTimeSlice;

		private float _loadSceneDependencyAssetTimeSlice;

		private float _loadLevelResourcesTimeSlice;

		private int _totalLevelResourceCount;

		private int _loadedLevelResourceCount;

		private readonly Dictionary<string, bool> _loadingResourcesFlag = new Dictionary<string, bool>();

		private bool _firstLoadSceneDependencyAsset;

		private bool _firstLoadSceneUpdate;

		private static bool s_isShaderVariantWarmUpComplete;

		private static bool s_isLoadingShaderVariantCollection;

		private GameDataModule GetGameDataModule
		{
			get
			{
				return Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule);
			}
		}

		private PlayerDataModule GetPlayerDataModule
		{
			get
			{
				return Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule);
			}
		}

		protected override void OnEnter(IFsm<ProcedureMod> procedureOwner)
		{
			base.OnEnter(procedureOwner);
			Mod.Event.Subscribe(EventArgs<SceneMod.LoadSuccessEventArgs>.EventId, OnLoadSceneSuccess);
			Mod.Event.Subscribe(EventArgs<SceneMod.LoadFailureEventArgs>.EventId, OnLoadSceneFailure);
			Mod.Event.Subscribe(EventArgs<SceneMod.LoadUpdateEventArgs>.EventId, OnLoadSceneUpdate);
			Mod.Event.Subscribe(EventArgs<SceneMod.LoadDependencyEventArgs>.EventId, OnLoadSceneDependencyAsset);
			Mod.Event.Subscribe(EventArgs<UIMod.OpenSuccessEventArgs>.EventId, OnOpenUIFormSuccess);
			SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(0));
			_stage = SwitchSceneStage.StartLoading;
			Mod.UI.OpenUIForm(UIFormId.LoadingForm, this);
			_loadingForm = null;
			_preOpenFormIdsFlag.Clear();
			_loadingResourcesFlag.Clear();
			_loadProgressOffset = 0f;
			_firstLoadSceneDependencyAsset = true;
			_firstLoadSceneUpdate = true;
			GameElementCollector.ResetAllElement();
			Mod.Core.ResetToNormalSpeed();
			_switchToSceneId = procedureOwner.GetData<VarInt>("NextSceneId").Value;
			Scenes_sceneTable scenes_sceneTable = Mod.DataTable.Get<Scenes_sceneTable>().Get(_switchToSceneId);
			if (scenes_sceneTable == null)
			{
				Log.Error(string.Format("Can not load scene '{0}' from data table.", _switchToSceneId.ToString()));
				return;
			}
			BuglyAgent.SetScene(_switchToSceneId);
			_switchToSceneName = scenes_sceneTable.AssetName;
			if (_switchToSceneId == 1)
			{
				_loadSceneDependencyAssetTimeSlice = 0.8f;
				_loadSceneTimeSlice = 0.1f;
				_loadLevelResourcesTimeSlice = 0.1f;
			}
			else
			{
				_loadSceneDependencyAssetTimeSlice = 0.5f;
				_loadSceneTimeSlice = 0.2f;
				_loadLevelResourcesTimeSlice = 0.3f;
			}
		}

		protected override void OnLeave(IFsm<ProcedureMod> procedureOwner, bool isShutdown)
		{
			Mod.Event.Unsubscribe(EventArgs<SceneMod.LoadSuccessEventArgs>.EventId, OnLoadSceneSuccess);
			Mod.Event.Unsubscribe(EventArgs<SceneMod.LoadFailureEventArgs>.EventId, OnLoadSceneFailure);
			Mod.Event.Unsubscribe(EventArgs<SceneMod.LoadUpdateEventArgs>.EventId, OnLoadSceneUpdate);
			Mod.Event.Unsubscribe(EventArgs<SceneMod.LoadDependencyEventArgs>.EventId, OnLoadSceneDependencyAsset);
			Mod.Event.Unsubscribe(EventArgs<UIMod.OpenSuccessEventArgs>.EventId, OnOpenUIFormSuccess);
			if (_loadingForm != null)
			{
				_loadingForm.Close();
			}
			_loadingForm = null;
			_preOpenFormIdsFlag.Clear();
			_loadingResourcesFlag.Clear();
			base.OnLeave(procedureOwner, isShutdown);
		}

		protected override void OnTick(IFsm<ProcedureMod> procedureOwner, float elapseSeconds, float realElapseSeconds)
		{
			base.OnTick(procedureOwner, elapseSeconds, realElapseSeconds);
			switch (_stage)
			{
			case SwitchSceneStage.LoadingLevelResources:
				foreach (KeyValuePair<string, bool> item in _loadingResourcesFlag)
				{
					if (!item.Value)
					{
						return;
					}
				}
				OnLoadLevelResourcesFinished();
				break;
			case SwitchSceneStage.LoadingUIResources:
				foreach (KeyValuePair<UIFormId, bool> item2 in _preOpenFormIdsFlag)
				{
					if (!item2.Value)
					{
						return;
					}
				}
				if (!(_homeForm == null) && _homeForm.isLoadComplete())
				{
					OnPreOpenFormsFinished();
				}
				break;
			case SwitchSceneStage.LoadingFinished:
				if (_switchToSceneId == 1)
				{
					ChangeState<MenuProcedure>(procedureOwner);
				}
				else
				{
					ChangeState<LevelProcedure>(procedureOwner);
				}
				break;
			case SwitchSceneStage.StartLoading:
			case SwitchSceneStage.LoadingScene:
				break;
			}
		}

		private void LoadScene()
		{
			_stage = SwitchSceneStage.LoadingScene;
			string[] loadedAssetNames = Mod.Scene.LoadedAssetNames;
			for (int i = 0; i < loadedAssetNames.Length; i++)
			{
				Mod.Scene.UnloadScene(loadedAssetNames[i]);
			}
			string sceneAsset = AssetUtility.GetSceneAsset(_switchToSceneName);
			Mod.Scene.LoadScene(sceneAsset, this);
		}

		private void OnLoadSceneSuccess(object sender, Foundation.EventArgs e)
		{
			if (((SceneMod.LoadSuccessEventArgs)e).UserData == this)
			{
				if (_switchToSceneId == 1)
				{
					_loadingForm.SetProgress(_loadSceneTimeSlice + _loadSceneDependencyAssetTimeSlice);
					PreLoadUIResources();
				}
				else
				{
					LoadLevelResources();
				}
			}
		}

		private void OnLoadSceneFailure(object sender, Foundation.EventArgs e)
		{
			SceneMod.LoadFailureEventArgs loadFailureEventArgs = (SceneMod.LoadFailureEventArgs)e;
			if (loadFailureEventArgs.UserData == this)
			{
				Log.Error(string.Format("Load scene '{0}' failure, error message '{1}'.", loadFailureEventArgs.AssetName, loadFailureEventArgs.Message));
			}
		}

		private void OnLoadSceneUpdate(object sender, Foundation.EventArgs e)
		{
			SceneMod.LoadUpdateEventArgs loadUpdateEventArgs = (SceneMod.LoadUpdateEventArgs)e;
			if (loadUpdateEventArgs.UserData == this)
			{
				if (_firstLoadSceneUpdate)
				{
					_loadProgressOffset = _loadingForm.Progress;
					_firstLoadSceneUpdate = false;
				}
				_loadingForm.SetProgress(_loadProgressOffset + loadUpdateEventArgs.Progress * _loadSceneTimeSlice);
			}
		}

		private void OnLoadSceneDependencyAsset(object sender, Foundation.EventArgs e)
		{
			if (_firstLoadSceneDependencyAsset)
			{
				_loadProgressOffset = _loadingForm.Progress;
				_firstLoadSceneDependencyAsset = false;
			}
			SceneMod.LoadDependencyEventArgs loadDependencyEventArgs = (SceneMod.LoadDependencyEventArgs)e;
			if (loadDependencyEventArgs.UserData == this)
			{
				_loadingForm.SetProgress(_loadProgressOffset + (float)loadDependencyEventArgs.LoadedCount / (float)loadDependencyEventArgs.TotalCount * _loadSceneDependencyAssetTimeSlice);
			}
		}

		private void LoadLevelResources()
		{
			_loadProgressOffset = _loadingForm.Progress;
			_stage = SwitchSceneStage.LoadingLevelResources;
			if (Director.Ins.DevelopMode)
			{
				if (DevelopmentTool.Instance != null && GetGameDataModule.CurLevelId == DevelopmentTool.Instance.DeveLevel)
				{
					MapController.Instance.LoadDevMapTable(DevelopmentTool.Instance.DevGridGroup);
				}
				else
				{
					MapController.Instance.LoadMapTable();
				}
			}
			else
			{
				MapController.Instance.LoadMapTable();
			}
			List<string> levelBrushPathList = MapController.Instance.GetLevelBrushPathList();
			List<string> levelResourcesPath = LevelResources.theResource.GetLevelResourcesPath();
			List<RelatedAssetData> relatedAssetData = MapController.Instance.GetRelatedAssetData();
			int count = relatedAssetData.Count;
			string text = FairysManager.LoadFairyPath(GetPlayerDataModule.GetPlayerLevelAvaterId());
			if (!string.IsNullOrEmpty(text) && !levelBrushPathList.Contains(text))
			{
				levelBrushPathList.Add(text);
			}
			for (int i = 0; i < levelResourcesPath.Count; i++)
			{
				if (!string.IsNullOrEmpty(levelResourcesPath[i]) && !levelBrushPathList.Contains(levelResourcesPath[i]))
				{
					levelBrushPathList.Add(levelResourcesPath[i]);
				}
			}
			for (int j = 0; j < count; j++)
			{
				RelatedAssetData relatedAssetData2 = relatedAssetData[j];
				if (relatedAssetData2 != null && relatedAssetData2.AssetType == 1 && !levelBrushPathList.Contains(relatedAssetData2.Path))
				{
					levelBrushPathList.Add(relatedAssetData2.Path);
				}
			}
			LoadResources(levelBrushPathList);
		}

		private void LoadResources(List<string> list)
		{
			_totalLevelResourceCount = list.Count;
			_loadedLevelResourceCount = 0;
			for (int i = 0; i < list.Count; i++)
			{
				string text = list[i];
				_loadingResourcesFlag[text] = false;
				Mod.Resource.LoadAsset(text, new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object userData)
				{
					if (!LevelResources.theResource.IsResourcLoaded(assetName))
					{
						LevelResources.theResource.AddResource(assetName, (UnityEngine.Object)asset);
					}
					_loadingResourcesFlag[assetName] = true;
					_loadedLevelResourceCount++;
					_loadingForm.SetProgress(_loadProgressOffset + (float)_loadedLevelResourceCount / (float)_totalLevelResourceCount * _loadLevelResourcesTimeSlice);
				}, delegate(string assetName, string errorMessage, object userData)
				{
					Log.Error(string.Format("Can not load resource from '{0}' with errormessage {1}", assetName, errorMessage));
				}));
			}
		}

		private void OnLoadLevelResourcesFinished()
		{
			MapController.Instance.InstantiationBrush();
			GameElementCollector.RegisterAllElement();
			_loadingForm.SetProgress(1f);
			PreLoadUIResources();
		}

		private void PreLoadUIResources()
		{
			_stage = SwitchSceneStage.LoadingUIResources;
			if (_switchToSceneId == 1)
			{
				_preOpenFormIdsFlag.Add(UIFormId.HomeForm, false);
				_preOpenFormIdsFlag.Add(UIFormId.MenuViewForm, false);
				Mod.UI.OpenUIForm(UIFormId.MenuViewForm, this);
			}
			else
			{
				OnPreOpenFormsFinished();
			}
		}

		private void OnOpenUIFormSuccess(object sender, Foundation.EventArgs e)
		{
			UIMod.OpenSuccessEventArgs openSuccessEventArgs = (UIMod.OpenSuccessEventArgs)e;
			UIFormId uIFormId = Mod.UI.GetUIFormId(openSuccessEventArgs.UIForm);
			switch (uIFormId)
			{
			case UIFormId.LoadingForm:
				if (BuiltinDialogForm.Current != null)
				{
					BuiltinDialogForm.Destory();
				}
				_loadingForm = (LoadingForm)openSuccessEventArgs.UIForm.Logic;
				_loadingForm.SetProgress(0f);
				if (_switchToSceneId == 1 && GameController.Instance != null)
				{
					if (GameController.Instance != null)
					{
						GameController.Instance.OnExitGame();
						GameController.Instance.DestroyLocal();
					}
					if (LevelResources.theResource != null)
					{
						LevelResources.theResource.OnLevelExit();
					}
				}
				LoadScene();
				return;
			case UIFormId.HomeForm:
				_homeForm = (HomeForm)openSuccessEventArgs.UIForm.Logic;
				break;
			}
			if (_preOpenFormIdsFlag.ContainsKey(uIFormId))
			{
				_preOpenFormIdsFlag[uIFormId] = true;
				_loadProgressOffset = _loadingForm.Progress;
				_loadingForm.SetProgress(_loadProgressOffset + _loadLevelResourcesTimeSlice / (float)_preOpenFormIdsFlag.Count);
			}
		}

		private void OnPreOpenFormsFinished()
		{
			if (s_isShaderVariantWarmUpComplete)
			{
				_stage = SwitchSceneStage.LoadingFinished;
				return;
			}
			string text = "ShaderVariants1";
			if (s_isLoadingShaderVariantCollection)
			{
				return;
			}
			s_isLoadingShaderVariantCollection = true;
			try
			{
				Mod.Resource.LoadAsset(AssetUtility.GetShaderVariantsAsset(text), new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object userData)
				{
					ShaderVariantCollection shaderVariantCollection = asset as ShaderVariantCollection;
					if (shaderVariantCollection != null)
					{
						shaderVariantCollection.WarmUp();
						s_isShaderVariantWarmUpComplete = true;
						s_isLoadingShaderVariantCollection = false;
						_stage = SwitchSceneStage.LoadingFinished;
					}
					Mod.Resource.UnloadAsset(asset);
				}, delegate(string assetName, string errorMessage, object userData)
				{
					Log.Error(string.Format("Can not load Load ShaderVariants from '{0}' with error message '{1}", assetName, errorMessage));
					s_isShaderVariantWarmUpComplete = false;
					s_isLoadingShaderVariantCollection = false;
					_stage = SwitchSceneStage.LoadingFinished;
				}));
			}
			catch (Exception arg)
			{
				Log.Error(string.Format("Warmup shaderVariantsAsset({0}) failed.({1})", text, arg));
				s_isShaderVariantWarmUpComplete = false;
				s_isLoadingShaderVariantCollection = false;
				_stage = SwitchSceneStage.LoadingFinished;
			}
		}
	}
}
