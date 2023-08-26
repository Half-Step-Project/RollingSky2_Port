using System;
using System.Collections;
using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;
using UnityExpansion;
using User.TileMap;

using Grid = User.TileMap.Grid;

public class GameController : MonoBehaviour
{
	public enum GAMESTATE
	{
		Initialize,
		StartAnim,
		Cutscene,
		Runing,
		Pause,
		End,
		Reset,
		RebirthReset,
		OriginRebirthReset,
		Null,
		Load,
		Exit
	}

	public static bool IfNotDeath;

	public static GameController Instance;

	private GAMESTATE m_gameState = GAMESTATE.Load;

	private GAMESTATE m_lastGameState = GAMESTATE.Load;

	private List<UIFormId> currentShowView = new List<UIFormId>();

	private BaseRole m_ball;

	private BaseCouple m_couple;

	private BaseBackgroundElement m_background;

	private Railway m_railway;

	private CameraController m_cameraController;

	private WorldConfigureController m_WorldConfigureController;

	private static MapController m_map;

	private RandomAnimController m_randAnimController;

	public PetController m_petController;

	private WorldStartInfo m_currentWorldStartInfo;

	public BaseLevel m_baseLevel;

	public PropsController m_propsController;

	public MaterialController m_materialController;

	public FpsRegister m_fpsRegister;

	public BrushController m_brushController;

	public GridController m_gridController;

	public ProgressController m_progressController;

	public FairyController m_fairyController;

	private bool m_isPlayingTryEffect;

	private int m_PlayLevelTotalTime;

	private int m_playerFreeShowRebirthCount;

	private int m_playerFreeShowRebirthIndex;

	private uint guideLineTimer;

	private uint guideLineLeftTime;

	private string m_startGrid = "0";

	private string m_startRowNumber = "0";

	private bool m_isFirstLevelStagePlugin;

	private static long m_lastPlayPluginAdTimeStemp;

	private const int EnterFormPluginAdId = 1;

	private static long m_lastRebirthPluginAdTimeStemp;

	private const int EnterRebirthFormPluginAdId = 12;

	public GAMESTATE M_gameState
	{
		get
		{
			return m_gameState;
		}
	}

	public GameDataModule GetGameDataModule
	{
		get
		{
			return Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule);
		}
	}

	public InsideGameDataModule GetInsideGameDataModule
	{
		get
		{
			return Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule);
		}
	}

	public PlayerDataModule GetPlayerDataModule
	{
		get
		{
			return Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule);
		}
	}

	public ResultPageDataModule GetResultPageDataModule
	{
		get
		{
			return Singleton<DataModuleManager>.Instance.GetDataModule<ResultPageDataModule>(DataNames.ResultPageDataModule);
		}
	}

	public WorldConfigureController GetWorldConfigureController
	{
		get
		{
			return m_WorldConfigureController;
		}
		set
		{
			m_WorldConfigureController = value;
		}
	}

	public int GetStartGrid
	{
		get
		{
			return int.Parse(m_startGrid);
		}
	}

	public int GetStartRowNumber
	{
		get
		{
			return int.Parse(m_startRowNumber);
		}
	}

	public bool IsRecordOriginRebirth
	{
		get
		{
			return true;
		}
	}

	public int GetBackgroundIndex()
	{
		int result = 0;
		if ((bool)m_background)
		{
			result = m_background.GetBackgrondIndex();
		}
		return result;
	}

	private void Awake()
	{
		Instance = this;
	}

	private void FixedUpdate()
	{
		if (m_gameState != GAMESTATE.Load && m_gameState != GAMESTATE.Exit && (bool)m_ball)
		{
			m_ball.FixedUpdateRole();
		}
	}

	private void Update()
	{
		if (m_gameState != GAMESTATE.Load && m_gameState != GAMESTATE.Exit)
		{
			OnInputEscape();
			if (m_gameState == GAMESTATE.Runing)
			{
				InputController.instance.InputUpdate();
				m_map.MapUpdate();
			}
			else if (m_gameState == GAMESTATE.Cutscene)
			{
				m_map.MapUpdate();
			}
			if (m_fairyController != null)
			{
				m_fairyController.OnUpdate();
			}
			if (m_fpsRegister != null)
			{
				m_fpsRegister.Update(Time.deltaTime, Time.unscaledDeltaTime);
			}
		}
	}

	private void LateUpdate()
	{
		if (m_gameState == GAMESTATE.Runing)
		{
			m_railway.UpdateRailway();
			m_ball.UpdateRole();
			if ((bool)m_couple)
			{
				m_couple.UpdateCouple();
			}
			m_cameraController.CameraUpdate();
			m_map.UpdateElement();
			GetInsideGameDataModule.UpdateProgress();
			m_WorldConfigureController.UpdateTheme();
		}
		else if (m_gameState == GAMESTATE.End)
		{
			if (m_ball.CurrentState == BaseRole.BallState.DropDie)
			{
				m_ball.UpdateRole();
			}
			m_cameraController.GameLoseCameraSlow();
			m_map.UpdateElement();
		}
		else if (m_gameState == GAMESTATE.Null)
		{
			if (m_ball.CurrentState == BaseRole.BallState.DropDie)
			{
				m_ball.RoleStopAnim();
			}
		}
		else if (m_gameState == GAMESTATE.Cutscene)
		{
			m_cameraController.CameraUpdate();
			m_ball.UpdateRole();
			if ((bool)m_couple)
			{
				m_couple.UpdateCouple();
			}
			m_map.UpdateElement();
		}
		else if (m_gameState == GAMESTATE.RebirthReset)
		{
			m_ball.UpdateRole();
		}
		else if (m_gameState != GAMESTATE.OriginRebirthReset && m_gameState == GAMESTATE.StartAnim)
		{
			m_cameraController.CameraUpdate();
			m_map.UpdateElement();
		}
	}

	private void OnApplicationPause(bool pausedStatus)
	{
		if (m_gameState != 0 && m_gameState != GAMESTATE.Reset && m_gameState != GAMESTATE.RebirthReset && m_gameState != GAMESTATE.OriginRebirthReset && m_gameState != GAMESTATE.Null && m_gameState != GAMESTATE.End && m_gameState != GAMESTATE.Pause && m_gameState != GAMESTATE.Cutscene && m_gameState != GAMESTATE.StartAnim && pausedStatus && !Mod.Core.IsPaused)
		{
			PauseGame();
		}
	}

	private void SwitchState(GAMESTATE _gamestate)
	{
		m_lastGameState = m_gameState;
		m_gameState = _gamestate;
	}

	public void Initialize()
	{
		Singleton<MenuMusicController>.Instance.StopMenuMusic();
		GetInsideGameDataModule.LevelEnter();
		SwitchState(GAMESTATE.Initialize);
		AddInGameEvent();
		m_randAnimController = RandomAnimController.Instance;
		m_randAnimController.OnGameEnter();
		m_WorldConfigureController = Singleton<WorldConfigureController>.Instance;
		m_WorldConfigureController.Initialize();
		m_WorldConfigureController.InitThemes(GetGameDataModule.CurLevelId);
		m_map = MapController.Instance;
		m_map.ShowRowNum = m_WorldConfigureController.m_currentLevelConfigureTable.m_worldStartInfo.m_showRow;
		m_map.CreateLevelMap();
		m_currentWorldStartInfo = m_WorldConfigureController.GetStartInfoByLevel();
		m_cameraController = CameraController.theCamera;
		m_cameraController.Initialize();
		m_railway = Railway.theRailway;
		m_railway.Initialize();
		List<int> preloadRoleIndexList = m_currentWorldStartInfo.m_preloadRoleIndexList;
		if (preloadRoleIndexList != null)
		{
			for (int i = 0; i < preloadRoleIndexList.Count; i++)
			{
				m_ball = RoleManager.CreateRole(preloadRoleIndexList[i]);
			}
		}
		m_ball = RoleManager.CreateRole(m_currentWorldStartInfo.m_roleIndex);
		m_ball.Initialize(m_railway.transform, m_railway.StartRoleLocalPos, m_railway.RestartRoleLocalPos);
		m_ball.SetEmissionInfo(m_currentWorldStartInfo.m_ifLerpEmission, m_currentWorldStartInfo.m_delayEmissionTime, m_currentWorldStartInfo.m_lerpEmissionTime, m_currentWorldStartInfo.m_beginEmissionValue, m_currentWorldStartInfo.m_endEmissionValue);
		m_ball.SetBeginEmission();
		m_couple = CoupleManager.CreateCouple(m_currentWorldStartInfo.CoupleRoleIndex);
		if ((bool)m_couple)
		{
			m_couple.Initialize(m_railway.StartCoupleWorldPos);
		}
		m_petController = PetController.Instance;
		m_petController.CreatePet(GetWorldConfigureController.m_currentLevelConfigureTable.m_worldStartInfo.m_petID);
		if (m_petController.m_petBase != null)
		{
			m_petController.m_petBase.OnGameInitialization();
			m_petController.m_petBase.SwitchPetState(PetState.Ready);
		}
		m_fairyController = FairyController.Builder();
		int playerLevelAvaterId = GetPlayerDataModule.GetPlayerLevelAvaterId();
		if (GetGameDataModule.CurLevelId != 10000)
		{
			m_fairyController.CreateFairy(playerLevelAvaterId);
		}
		m_brushController = new BrushController(m_map);
		m_gridController = new GridController(m_map);
		m_progressController = new ProgressController();
		m_propsController = PropsController.Builder();
		m_background = BackgroundManager.CreateBackground(m_currentWorldStartInfo.m_BackgroundIndex);
		if ((bool)m_background)
		{
			m_background.Initialize(m_cameraController.m_Camera);
		}
		InsideGameDataModule getInsideGameDataModule = GetInsideGameDataModule;
		getInsideGameDataModule.InitializeLevelData(GetGameDataModule.CurLevelId, m_WorldConfigureController.GetStartInfoByLevel().m_levelDeltaLength);
		getInsideGameDataModule.InitBuyOutRebirth();
		getInsideGameDataModule.ShowFreeBuffCount = 0;
		getInsideGameDataModule.DieCount = 0;
		Singleton<ProgressManager>.Instance.CurrentLevelId = GetGameDataModule.CurLevelId;
		BaseLevelFactory baseLevelFactory = new BaseLevelFactory();
		m_baseLevel = baseLevelFactory.CreateBaseLevelByCurrentLevelID();
		if (m_baseLevel != null)
		{
			m_baseLevel.OnInitialize();
		}
		m_materialController = MaterialController.m_target;
		if (m_materialController != null)
		{
			m_materialController.transform.parent = base.transform;
		}
		m_fpsRegister = new FpsRegister();
		MonoSingleton<GameTools>.Instacne.FirstTimeEnterLevelInfoC(GetGameDataModule.CurLevelId);
		ResetPluginAdTimeStamp();
	}

	public void ChangeRoleTo(BaseRole newRole)
	{
		BaseRole ball = m_ball;
		PropsName[] allNames = m_propsController.GetAllNames();
		m_propsController.RemoveAll();
		newRole.InitializeByRole(m_ball);
		m_ball = newRole;
		ball.ResetBall();
		ball.UnloadLocal();
		for (int i = 0; i < allNames.Length; i++)
		{
			m_propsController.Add(allNames[i]);
		}
	}

	public void SetActiveForRenderer(GameObject obj, bool active)
	{
		Renderer[] componentsInChildren = obj.GetComponentsInChildren<Renderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = active;
		}
	}

	private void ShowGameStartView(object sender, Foundation.EventArgs e)
	{
		ShowGameStartEventArgs showGameStartEventArgs = e as ShowGameStartEventArgs;
		if (showGameStartEventArgs == null)
		{
			return;
		}
		GameDataModule getGameDataModule = GetGameDataModule;
		if (getGameDataModule.CurLevelId == 10000)
		{
			Mod.UI.OpenUIForm(UIFormId.SceneTutorialForm);
			Mod.UI.OpenUIForm(UIFormId.RuntimeForm);
		}
		else
		{
			Mod.UI.OpenUIForm(UIFormId.TutorialGameStartForm);
			Mod.UI.OpenUIForm(UIFormId.RuntimeForm);
		}
		if ((getGameDataModule.CurLevelId != 10000 || !GetPlayerDataModule.GetCanShowTutorialTips()) && getGameDataModule.CurLevelId != 10000)
		{
			new LevelStartForm.ShowData().showRebirth = showGameStartEventArgs.ShowRebirth;
		}
		InsideGameDataModule getInsideGameDataModule = GetInsideGameDataModule;
		if (TutorialManager.Instance.GetCurrentStageId() == TutorialStageId.STAGE_FIRST_LEVEL_BUFF && getGameDataModule.CurLevelId == GameCommon.FIRST_LEVEL)
		{
			PlayerDataModule.Instance.GetFreeGoodsForTutorial();
			getInsideGameDataModule.m_theBoardRebirthCount = 0;
			GetGoodsData getGoodsData = new GetGoodsData();
			getGoodsData.GoodsTeamId = GameCommon.TUTORIAL_BUFF_GOODSTEAMID;
			getGoodsData.GoodsTeamNum = 1;
			getGoodsData.GoodsTeam = true;
			getGoodsData.CallBackFunc = delegate
			{
				Mod.Event.Fire(this, Mod.Reference.Acquire<ToClickGameStartEventArgs>().Initialize());
			};
			Mod.UI.OpenUIForm(UIFormId.GetGoodsForm, getGoodsData);
			TutorialManager.Instance.EndCurrentStage();
		}
		if (PlayerDataModule.Instance.PluginAdController.DelayRewardFunc != null)
		{
			PlayerDataModule.Instance.PluginAdController.DelayRewardFunc(PlayerDataModule.Instance.PluginAdController.DelayRewardPluginId);
			PlayerDataModule.Instance.PluginAdController.ClearDelayRewardOperate();
		}
	}

	private void OnClickGameStartButton(object sender, Foundation.EventArgs e)
	{
		if (m_baseLevel == null)
		{
			return;
		}
		Mod.Event.FireNow(this, Mod.Reference.Acquire<InstantPropsEventArgs>().Initialize());
		if (GetInsideGameDataModule.CurrentOriginRebirth != null)
		{
			Mod.UI.CloseUIForm(UIFormId.UsingAssertForm);
			if (RecordOriginRebirthManager.m_isBson)
			{
				StartRunByOriginRebirthBsonData(GetInsideGameDataModule.CurrentOriginRebirth);
			}
			else
			{
				StartRunByOriginRebirthData(GetInsideGameDataModule.CurrentOriginRebirth);
			}
			SwitchState(GAMESTATE.Runing);
			if (GetInsideGameDataModule.CurrentRebirthType == RebirthType.OriginRebirth)
			{
				if (GetInsideGameDataModule.m_isAdRebirth)
				{
					InfocUtils.Report_rollingsky2_games_Runningtime(GetGameDataModule.CurLevelId, 4, GetInsideGameDataModule.ProgressPercentage);
				}
				else
				{
					InfocUtils.Report_rollingsky2_games_Runningtime(GetGameDataModule.CurLevelId, 3, GetInsideGameDataModule.ProgressPercentage);
				}
			}
			else
			{
				InfocUtils.Report_rollingsky2_games_Runningtime(GetGameDataModule.CurLevelId, 1, GetInsideGameDataModule.ProgressPercentage);
			}
		}
		else
		{
			m_baseLevel.OnClickGameStateButton();
			InfocUtils.Report_rollingsky2_games_Runningtime(GetGameDataModule.CurLevelId, 0, 0);
		}
		Mod.UI.CloseUIForm(UIFormId.LevelTargetForm);
	}

	public void AdmissionFinished()
	{
		SetActiveForRenderer(BaseRole.theBall.gameObject, true);
		GameStartEventArgs gameStartEventArgs = Mod.Reference.Acquire<GameStartEventArgs>();
		gameStartEventArgs.Initialize(GameStartEventArgs.GameStartType.Normal);
		Mod.Event.Fire(this, gameStartEventArgs);
	}

	public void StartGame()
	{
		GetInsideGameDataModule.TheBoardStart();
		StartCoroutine(StartAnim());
		StartCoroutine(ChangeToCutscene());
		if ((bool)m_background)
		{
			m_background.StartPlayAnim();
		}
		if (m_fpsRegister != null)
		{
			m_fpsRegister.BeginSample();
		}
		m_PlayLevelTotalTime = (int)Time.realtimeSinceStartup;
		m_playerFreeShowRebirthCount = 0;
		if (PlayerDataModule.Instance.PlayerIsHadSpecialStarAbility(2))
		{
			m_playerFreeShowRebirthCount = PlayerDataModule.Instance.GetStarLevelAbilityNum(2);
		}
		if (PlayerDataModule.Instance.PlayerIsHadSpecialStarAbility(10))
		{
			m_playerFreeShowRebirthCount = PlayerDataModule.Instance.GetStarLevelAbilityNum(10);
		}
		m_playerFreeShowRebirthIndex = -1;
	}

	public void PlayBgMusic(object sender, Foundation.EventArgs e)
	{
		BgMusicPlayEventArgs bgMusicPlayEventArgs = e as BgMusicPlayEventArgs;
		PlayBgMusic(bgMusicPlayEventArgs.m_time);
	}

	public bool IsGameStateChanged()
	{
		return m_lastGameState != m_gameState;
	}

	private void PlayBgMusic(float time = 0f)
	{
		Singleton<MenuMusicController>.Instance.PlayGameMusic(GetGameDataModule.CurLevelId, time);
	}

	private IEnumerator StartAnim()
	{
		yield return new WaitForEndOfFrame();
		SwitchState(GAMESTATE.StartAnim);
		m_cameraController.PlayStartAnim();
		m_ball.ChangeToStartAnim();
		if ((bool)m_couple)
		{
			m_couple.ChangeToStartAnim();
		}
		m_map.InitElement();
		if (m_petController.m_petBase != null)
		{
			m_petController.m_petBase.SwitchPetState(PetState.Pose);
		}
	}

	private IEnumerator ChangeToCutscene()
	{
		float totalTime = 0f;
		while (totalTime <= m_currentWorldStartInfo.m_StartAnimTime)
		{
			totalTime += Time.deltaTime;
			float percent = totalTime / m_currentWorldStartInfo.m_StartAnimTime;
			Mod.Event.Fire(this, Mod.Reference.Acquire<GameCutScenePercentEventArgs>().Initialize(percent));
			yield return null;
		}
		CutScene();
		Mod.Event.Fire(this, Mod.Reference.Acquire<GameCutSceneEventArgs>());
		SwitchState(GAMESTATE.Cutscene);
	}

	private void CutScene()
	{
		m_cameraController.SwitchCameraState(CameraController.CameraState.CUTSCENE);
		m_ball.ChangeToCutscene();
		if ((bool)m_couple)
		{
			m_couple.ChangeToCutScene();
		}
		if (m_petController.m_petBase != null)
		{
			m_petController.m_petBase.SwitchPetState(PetState.Admission);
		}
		m_ball.SetPet(m_petController.m_petBase);
	}

	private void StartRunning()
	{
		InputController.instance.Reset();
		m_ball.StartBall(GetInsideGameDataModule.CurrentRebirthBoxData);
		if ((bool)m_couple)
		{
			m_couple.StartCouple();
		}
		m_railway.StartRailway();
		m_cameraController.SwitchCameraState(CameraController.CameraState.NORMAL);
		if (GetInsideGameDataModule.CurrentRebirthBoxData != null)
		{
			Singleton<MenuMusicController>.Instance.PlayGameMusic(GetGameDataModule.CurLevelId, GetInsideGameDataModule.CurrentRebirthBoxData.m_musicTime);
			if (m_petController.m_petBase != null)
			{
				m_petController.m_petBase.SwitchPetState(PetState.FastAdmission);
			}
			m_map.StartRunningForRebirthData(GetInsideGameDataModule.CurrentRebirthBoxData);
			CameraController.theCamera.StartRunningForRebirth(GetInsideGameDataModule.CurrentRebirthBoxData);
		}
	}

	private void DealWithTryLevel()
	{
		if (GetInsideGameDataModule.TryLevel)
		{
			int progressPercentage = GetInsideGameDataModule.ProgressPercentage;
			int levelTryMaxPercentById = MonoSingleton<GameTools>.Instacne.GetLevelTryMaxPercentById(GetGameDataModule.CurLevelId);
			if (levelTryMaxPercentById > 0 && progressPercentage >= levelTryMaxPercentById)
			{
				PlayTryLevelEffect();
			}
		}
	}

	private void PlayTryLevelEffect()
	{
		if (!m_isPlayingTryEffect)
		{
			m_isPlayingTryEffect = true;
			BaseRole.theBall.IsInvincible = true;
			StartCoroutine(MonoSingleton<GameTools>.Instacne.PauseGameByTime(5f, delegate
			{
				m_isPlayingTryEffect = false;
				Mod.Core.Speed = 1f;
				BaseRole.theBall.IsInvincible = false;
				GameFailEventArgs gameFailEventArgs = Mod.Reference.Acquire<GameFailEventArgs>();
				gameFailEventArgs.Initialize(GameFailEventArgs.GameFailType.TryLevel, Vector3.zero);
				Mod.Event.FireNow(this, gameFailEventArgs);
			}));
		}
	}

	private void PauseGame(bool isShowPause = true)
	{
		Debug.Log("暫停");
		currentShowView.Clear();
		List<UIFormId> allLoadUIFormIds = Mod.UI.GetAllLoadUIFormIds();
		for (int i = 0; i < allLoadUIFormIds.Count; i++)
		{
			currentShowView.Add(allLoadUIFormIds[i]);
		}
		if (currentShowView.Contains(UIFormId.ResultForm))
		{
			Mod.Event.Fire(this, Mod.Reference.Acquire<GameExitEventArgs>());
		}
		else if (!currentShowView.Contains(UIFormId.LoadingForm))
		{
			if (isShowPause)
			{
				Mod.UI.OpenUIForm(UIFormId.PauseForm);
			}
			Singleton<MenuMusicController>.Instance.PauseGameMusic();
			InputController.instance.PauseInput(true);
			Mod.Core.Speed = 0f;
			SwitchState(GAMESTATE.Pause);
			if (m_fpsRegister != null)
			{
				m_fpsRegister.Pause = true;
			}
		}
	}

	private void ResumeGame()
	{
		Mod.Core.Speed = 1f;
		SwitchState(m_lastGameState);
		Singleton<MenuMusicController>.Instance.ResumeGameMusic();
		InputController.instance.PauseInput(false);
		if (m_fpsRegister != null)
		{
			m_fpsRegister.Pause = false;
		}
	}

	private void ResetGame()
	{
		Singleton<MenuMusicController>.Instance.StopGameMusic();
		m_railway.ResetRailway();
		m_cameraController.ResetController();
		if (m_ball.CurrentRoleData.RoleId == m_currentWorldStartInfo.m_roleIndex)
		{
			m_ball.ResetBall();
			m_ball.SetBeginEmission();
		}
		else
		{
			m_ball.UnloadLocal();
			m_ball.ResetBall();
			m_ball = RoleManager.CreateRole(m_currentWorldStartInfo.m_roleIndex);
			m_ball.Initialize(m_railway.transform, m_railway.StartRoleLocalPos, m_railway.RestartRoleLocalPos);
			m_ball.ResetBall();
			m_ball.SetBeginEmission();
		}
		if ((bool)m_couple)
		{
			m_couple.ResetCouple();
		}
		if (m_propsController != null)
		{
			m_propsController.RemoveAll();
		}
		CameraLookAt.Instance.ResetLookAtTarget();
		CameraShake.ResetShakeCamra();
		if (m_petController.m_petBase != null)
		{
			m_petController.m_petBase.SwitchPetState(PetState.Ready);
		}
		if ((bool)m_background)
		{
			m_background.ResetElement();
		}
		InputController.instance.Reset();
		m_map.ResetWorld();
		m_map.CreateLevelMap();
		m_WorldConfigureController.InitThemes(GetGameDataModule.CurLevelId);
		GetInsideGameDataModule.ResetLevelData();
	}

	private void ResetBySavePointInfo(RebirthBoxData savePoint)
	{
		Singleton<MenuMusicController>.Instance.StopGameMusic();
		m_railway.ResetBySavePointInfo(savePoint);
		m_cameraController.ResetBySavePointInfo(savePoint);
		m_ball.ResetBySavePointInfo(savePoint);
		if (m_petController.m_petBase != null)
		{
			m_petController.m_petBase.SwitchPetState(PetState.RebirthReady);
		}
		if ((bool)m_couple)
		{
			m_couple.ResetBySavePointInfo(savePoint);
		}
		if ((bool)m_background)
		{
			m_background.ResetBySavePointInfo(savePoint);
		}
		InputController.instance.Reset();
		m_map.ResetWorld();
		m_map.CreateLevelMap(savePoint);
		m_map.ResetBySavePointData(savePoint);
		if (savePoint.m_rebirthVehicleData != null && savePoint.m_rebirthVehicleData.m_vehicleGridId != -1)
		{
			BaseElement baseElement = m_map.GetBaseElementForAll(savePoint.m_rebirthVehicleData.m_vehicleGridId, savePoint.m_rebirthVehicleData.m_vehiclePoint, savePoint.m_rebirthVehicleData.m_tileType, savePoint.m_rebirthVehicleData.m_vehicleID);
			if (baseElement == null)
			{
				Grid gridById = m_map.GetGridById(savePoint.m_rebirthVehicleData.m_vehicleGridId);
				GridData gridDataById = m_map.GetGridDataById(savePoint.m_rebirthVehicleData.m_vehicleGridId);
				if (gridDataById.m_gridCellDatasDictionary.ContainsKey(savePoint.m_rebirthVehicleData.m_vehiclePoint))
				{
					List<SubstanceData> substanceDatas = gridDataById.m_gridCellDatasDictionary[savePoint.m_rebirthVehicleData.m_vehiclePoint].m_substanceDatas;
					SubstanceData substanceData = null;
					for (int i = 0; i < substanceDatas.Count; i++)
					{
						if (substanceDatas[i].m_type == (int)savePoint.m_rebirthVehicleData.m_tileType && substanceDatas[i].m_id == savePoint.m_rebirthVehicleData.m_vehicleID)
						{
							substanceData = substanceDatas[i];
							break;
						}
					}
					if (substanceData != null)
					{
						baseElement = m_map.CreateSubstanceFormBuffer(gridById, savePoint.m_rebirthVehicleData.m_vehiclePoint, substanceData);
					}
				}
			}
			if (baseElement != null)
			{
				((BaseVehicle)baseElement).ResetBySavePointInfo(savePoint, m_ball);
			}
		}
		m_ball.CurrentWorldRow = savePoint.m_worldProgress;
		if (m_petController.m_petBase != null)
		{
			m_ball.SetPet(m_petController.m_petBase);
		}
		if (m_propsController != null)
		{
			m_propsController.RemoveAll();
			Mod.Event.FireNow(this, Mod.Reference.Acquire<PropsAddEventArgs>().Initialize(PropsName.REBIRTH));
		}
		m_WorldConfigureController.InitThemes(savePoint, GetGameDataModule.CurLevelId);
		GetInsideGameDataModule.ResetLevelData(savePoint);
	}

	private void GameWin()
	{
		PlayerDataModule.Instance.AddPlayerFinishLevelNum();
		StartCoroutine(SuccessAnimEnd());
	}

	private IEnumerator SuccessAnimEnd()
	{
		yield return new WaitForSeconds(m_currentWorldStartInfo.m_EndAnimTime);
		Mod.UI.CloseUIForm(UIFormId.RuntimeForm);
		Mod.UI.CloseUIForm(UIFormId.TutorialGameStartForm);
		Singleton<MenuMusicController>.Instance.StopGameMusic();
		if (!RecordOriginRebirthManager.m_isCanRecord && GetGameDataModule.CurLevelId == 10000)
		{
			Mod.Event.Fire(this, Mod.Reference.Acquire<GameExitEventArgs>());
		}
		else
		{
			Mod.UI.OpenUIForm(UIFormId.ResultForm);
		}
	}

	public void OnExitGame()
	{
		if (m_fpsRegister != null && m_fpsRegister.IsSample)
		{
			m_fpsRegister.EndSample();
		}
		if (m_randAnimController != null)
		{
			m_randAnimController.OnGameExit();
		}
		if (Singleton<MenuMusicController>.Instance != null)
		{
			Singleton<MenuMusicController>.Instance.StopGameMusic();
		}
		SwitchState(GAMESTATE.Null);
		if ((bool)m_cameraController)
		{
			m_cameraController.ResetController();
		}
		if ((bool)m_map)
		{
			m_map.ClearWorld();
		}
		if ((bool)m_ball)
		{
			m_ball.ResetBall();
		}
		if ((bool)m_couple)
		{
			m_couple.ResetCouple();
		}
		if (m_petController != null)
		{
			m_petController.DestroyLocal();
		}
		if (m_fairyController != null)
		{
			m_fairyController.DestroyLocal();
		}
		if (m_propsController != null)
		{
			m_propsController.DestroyLocal();
		}
		RoleManager.DestroyAllRoles();
		if (m_background != null)
		{
			UnityEngine.Object.Destroy(m_background.gameObject);
		}
		if (m_brushController != null)
		{
			m_brushController.Shutdown();
		}
		if (m_gridController != null)
		{
			m_gridController.Shutdown();
		}
		if (m_progressController != null)
		{
			m_progressController.Shutdown();
		}
		currentShowView.Clear();
	}

	private void ExitGame()
	{
		SwitchState(GAMESTATE.Exit);
		GameElementCollector.ResetAllElement();
		Mod.UI.CloseLoadingUIForms();
		Mod.UI.CloseLoadedUIForms();
		Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).RecordPower();
		Mod.Core.Speed = 1f;
		Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule).TryLevel = false;
		GetInsideGameDataModule.LevelLeave();
		PlayerDataModule.Instance.DeleteTempGoods();
		if (TutorialManager.Instance.GetCurrentStageId() == TutorialStageId.STAGE_FIRST_LEVEL_BUFF)
		{
			TutorialManager.Instance.EndCurrentStage();
		}
		((LevelProcedure)Mod.Procedure.Get<LevelProcedure>()).IsChangeToSwitchScene = true;
	}

	private void AddInGameEvent()
	{
		Mod.Event.Subscribe(EventArgs<GameStartEventArgs>.EventId, OnGameStart);
		Mod.Event.Subscribe(EventArgs<GamePauseEventArgs>.EventId, OnGamePause);
		Mod.Event.Subscribe(EventArgs<GameResumeEventArgs>.EventId, OnGameResume);
		Mod.Event.Subscribe(EventArgs<GameSucessEventArgs>.EventId, OnGameSuccess);
		Mod.Event.Subscribe(EventArgs<GameFailEventArgs>.EventId, OnGameFail);
		Mod.Event.Subscribe(EventArgs<GameResetEventArgs>.EventId, OnGameReset);
		Mod.Event.Subscribe(EventArgs<GameExitEventArgs>.EventId, OnGameExit);
		Mod.Event.Subscribe(EventArgs<GameRebirthResetEventArgs>.EventId, OnGameRebirthReset);
		Mod.Event.Subscribe(EventArgs<GameOriginRebirthResetEventArgs>.EventId, OnGameOriginRebirthReset);
		Mod.Event.Subscribe(EventArgs<GameOverEventArgs>.EventId, OnGameOver);
		Mod.Event.Subscribe(EventArgs<ClickGameStartButtonEventArgs>.EventId, OnClickGameStartButton);
		Mod.Event.Subscribe(EventArgs<ShowGameStartEventArgs>.EventId, ShowGameStartView);
		Mod.Event.Subscribe(EventArgs<BgMusicPlayEventArgs>.EventId, PlayBgMusic);
		GetInsideGameDataModule.SubscribeEventArgs();
	}

	private void RemoveInGameEvent()
	{
		Mod.Event.Unsubscribe(EventArgs<GameStartEventArgs>.EventId, OnGameStart);
		Mod.Event.Unsubscribe(EventArgs<GamePauseEventArgs>.EventId, OnGamePause);
		Mod.Event.Unsubscribe(EventArgs<GameResumeEventArgs>.EventId, OnGameResume);
		Mod.Event.Unsubscribe(EventArgs<GameSucessEventArgs>.EventId, OnGameSuccess);
		Mod.Event.Unsubscribe(EventArgs<GameFailEventArgs>.EventId, OnGameFail);
		Mod.Event.Unsubscribe(EventArgs<GameResetEventArgs>.EventId, OnGameReset);
		Mod.Event.Unsubscribe(EventArgs<GameExitEventArgs>.EventId, OnGameExit);
		Mod.Event.Unsubscribe(EventArgs<GameRebirthResetEventArgs>.EventId, OnGameRebirthReset);
		Mod.Event.Unsubscribe(EventArgs<GameOriginRebirthResetEventArgs>.EventId, OnGameOriginRebirthReset);
		Mod.Event.Unsubscribe(EventArgs<GameOverEventArgs>.EventId, OnGameOver);
		Mod.Event.Unsubscribe(EventArgs<ClickGameStartButtonEventArgs>.EventId, OnClickGameStartButton);
		Mod.Event.Unsubscribe(EventArgs<ShowGameStartEventArgs>.EventId, ShowGameStartView);
		Mod.Event.Unsubscribe(EventArgs<BgMusicPlayEventArgs>.EventId, PlayBgMusic);
		GetInsideGameDataModule.UnSubscribeEventArgs();
	}

	private void OnGameStart(object sender, Foundation.EventArgs e)
	{
		GameStartEventArgs gameStartEventArgs = e as GameStartEventArgs;
		if (gameStartEventArgs != null)
		{
			switch (gameStartEventArgs.StartType)
			{
			case GameStartEventArgs.GameStartType.Normal:
				StartGame();
				break;
			case GameStartEventArgs.GameStartType.ForceRun:
				Mod.UI.CloseUIForm(UIFormId.UsingAssertForm);
				Mod.UI.OpenUIForm(UIFormId.RuntimeForm);
				StartRunning();
				SwitchState(GAMESTATE.Runing);
				break;
			}
			if (TutorialManager.Instance.GetCurrentStageId() == TutorialStageId.STAGE_FIRST_LEVEL)
			{
				TutorialManager.Instance.EndCurrentStage();
			}
		}
	}

	private void OnGamePause(object sender, Foundation.EventArgs e)
	{
		GamePauseEventArgs gamePauseEventArgs = e as GamePauseEventArgs;
		if (gamePauseEventArgs != null)
		{
			PauseGame(gamePauseEventArgs.IsShowPauseForm);
			PauseGuideLineTimer();
		}
	}

	private void OnGameResume(object sender, Foundation.EventArgs e)
	{
		if (e is GameResumeEventArgs)
		{
			ResumeGame();
			ResumeGuideLineTimer();
		}
	}

	private void OnGameFail(object sender, Foundation.EventArgs e)
	{
		GameFailEventArgs gameFailEventArgs = e as GameFailEventArgs;
		if (gameFailEventArgs != null)
		{
			m_randAnimController.OnGameFinish();
			if (m_baseLevel != null)
			{
				m_baseLevel.OnGameEnd();
			}
			GameEndCloseWindows();
			InsideGameDataModule getInsideGameDataModule = GetInsideGameDataModule;
			getInsideGameDataModule.DieCount++;
			if (!Director.Ins.DidNotConsumePower && !getInsideGameDataModule.m_isOriginRebirth && !getInsideGameDataModule.TryLevel && !PlayerDataModule.Instance.PlayerRecordData.IsInNoConsumePowerTime() && TutorialManager.Instance.IsTutorialCustPower())
			{
				Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).ChangePlayerGoodsNum(1, -1.0);
				Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).RecordPlayerUsePowerNum(1);
			}
			if (m_petController.m_petBase != null)
			{
				m_petController.m_petBase.SwitchPetState(PetState.Death);
			}
			Singleton<MenuMusicController>.Instance.StopGameMusic();
			bool flag = false;
			if (!getInsideGameDataModule.TryLevel && getInsideGameDataModule.IsCanShowRebirthForm())
			{
				flag = CheckOriginRebirth(gameFailEventArgs.DiePosition);
			}
			if (flag)
			{
				if (GetGameDataModule.CurLevelId == 10000)
				{
					StartCoroutine(TutorialRebirth());
				}
				else if (m_playerFreeShowRebirthCount != 0)
				{
					if (m_playerFreeShowRebirthIndex >= 0)
					{
						if (m_playerFreeShowRebirthIndex < m_playerFreeShowRebirthCount)
						{
							StartCoroutine(StarLevelUpRebirth());
							m_playerFreeShowRebirthIndex++;
						}
						else
						{
							m_playerFreeShowRebirthIndex = 1;
							StartCoroutine(OpenOriginRebirthView());
						}
					}
					else
					{
						m_playerFreeShowRebirthIndex = 1;
						StartCoroutine(OpenOriginRebirthView());
					}
				}
				else
				{
					StartCoroutine(OpenOriginRebirthView());
				}
			}
			else
			{
				SaveProgress();
				SaveLveleTaget();
				StartCoroutine(OpenResultView(gameFailEventArgs.FailType));
			}
			PlayerDataModule.Instance.RecordPower();
			bool livebuff = GetPlayerDataModule.BufferIsEnable(GameCommon.EVERY_DAY_GIVE_POWER);
			bool shieldbuff = GetPlayerDataModule.BufferIsEnable(GameCommon.START_FREE_SHIELD);
			bool revivebuff = GetPlayerDataModule.BufferIsEnable(GameCommon.ORIGIN_REBIRTH_FREE);
			int box = (Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).GetPlayerLevelData(GetGameDataModule.CurLevelId).isShowGiftBox ? 1 : 0);
			InfocUtils.Report_rollingsky2_games_levelnum(GetGameDataModule.CurLevelId, getInsideGameDataModule.ProgressPercentage, getInsideGameDataModule.GainedDiamonds, getInsideGameDataModule.GainedCrowns, livebuff, shieldbuff, revivebuff, (int)GetResultPageDataModule.FinishedType, box);
			if (m_fpsRegister != null)
			{
				m_fpsRegister.EndSample();
			}
			Mod.Event.FireNow(this, Mod.Reference.Acquire<FollowCloseEventArgs>().Initialize());
			SwitchState(GAMESTATE.End);
		}
		StopGuideLineTimer();
	}

	private void OnGameSuccess(object sender, Foundation.EventArgs e)
	{
		if (e is GameSucessEventArgs)
		{
			m_randAnimController.OnGameFinish();
			if (m_baseLevel != null)
			{
				m_baseLevel.OnGameEnd();
			}
			GameEndCloseWindows();
			GetInsideGameDataModule.ProgressPercentage = 100;
			SaveProgress();
			SaveLveleTaget();
			m_isFirstLevelStagePlugin = false;
			GameWin();
			if (m_fpsRegister != null)
			{
				m_fpsRegister.EndSample();
			}
			SwitchState(GAMESTATE.Null);
			m_PlayLevelTotalTime = (int)Time.realtimeSinceStartup - m_PlayLevelTotalTime;
			GetPlayerDataModule.AddPlayerInLevelTotalTime(m_PlayLevelTotalTime);
			bool livebuff = GetPlayerDataModule.BufferIsEnable(GameCommon.EVERY_DAY_GIVE_POWER);
			bool shieldbuff = GetPlayerDataModule.BufferIsEnable(GameCommon.START_FREE_SHIELD);
			bool revivebuff = GetPlayerDataModule.BufferIsEnable(GameCommon.ORIGIN_REBIRTH_FREE);
			int box = (Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).GetPlayerLevelData(GetGameDataModule.CurLevelId).isShowGiftBox ? 1 : 0);
			InfocUtils.Report_rollingsky2_games_levelnum(GetGameDataModule.CurLevelId, GetInsideGameDataModule.ProgressPercentage, GetInsideGameDataModule.GainedDiamonds, GetInsideGameDataModule.GainedCrowns, livebuff, shieldbuff, revivebuff, (int)GetResultPageDataModule.FinishedType, box);
		}
	}

	private void OnGameReset(object sender, Foundation.EventArgs e)
	{
		if (e is GameResetEventArgs)
		{
			GetInsideGameDataModule.ResetBuyOutRebirth();
			GetInsideGameDataModule.DieCount = 0;
			ResetGame();
			RecordingEnterGame();
			SwitchState(GAMESTATE.Reset);
			if (m_baseLevel != null)
			{
				m_baseLevel.OnGameReset();
			}
		}
	}

	private void OnGameRebirthReset(object sender, Foundation.EventArgs e)
	{
		if (e is GameRebirthResetEventArgs)
		{
			if (GetInsideGameDataModule.CurrentRebirthBoxData != null)
			{
				ResetBySavePointInfo(GetInsideGameDataModule.CurrentRebirthBoxData);
			}
			SwitchState(GAMESTATE.RebirthReset);
			if (m_baseLevel != null)
			{
				m_baseLevel.OnGameRebirthReset();
			}
		}
	}

	private void OnGameOriginRebirthReset(object sender, Foundation.EventArgs e)
	{
		GameOriginRebirthResetEventArgs gameOriginRebirthResetEventArgs = e as GameOriginRebirthResetEventArgs;
		if (gameOriginRebirthResetEventArgs == null)
		{
			return;
		}
		if (GetInsideGameDataModule.CurrentOriginRebirth != null)
		{
			OnResetForOriginRebirth();
			if (RecordOriginRebirthManager.m_isBson)
			{
				SetOriginRebirthBsonData(GetInsideGameDataModule.CurrentOriginRebirth);
			}
			else
			{
				SetOriginRebirthData(GetInsideGameDataModule.CurrentOriginRebirth);
			}
		}
		SwitchState(GAMESTATE.OriginRebirthReset);
		if (m_baseLevel != null)
		{
			Mod.Event.FireNow(this, Mod.Reference.Acquire<ShowGameStartEventArgs>().Initialize(gameOriginRebirthResetEventArgs.CheckFreeBuff, false));
			m_baseLevel.OnGameRebirthReset();
		}
	}

	private void OnGameOver(object sender, Foundation.EventArgs e)
	{
		if (e is GameOverEventArgs)
		{
			m_PlayLevelTotalTime = (int)Time.realtimeSinceStartup - m_PlayLevelTotalTime;
			GetPlayerDataModule.AddPlayerInLevelTotalTime(m_PlayLevelTotalTime);
			SaveProgress();
			SaveLveleTaget();
			PlayerDataModule.Instance.AddPlayerFinishLevelNum();
			if (!DealEnterformPluginAd())
			{
				Mod.UI.OpenUIForm(UIFormId.ResultForm);
			}
		}
	}

	private void OnGameExit(object sender, Foundation.EventArgs e)
	{
		if (e is GameExitEventArgs)
		{
			ExitGame();
		}
	}

	private void GameEndCloseWindows()
	{
		Mod.UI.CloseUIForm(UIFormId.TutorialGameStartForm);
		Mod.UI.CloseUIForm(UIFormId.RuntimeForm);
	}

	private void SaveProgress()
	{
		InsideGameDataModule getInsideGameDataModule = GetInsideGameDataModule;
		PlayerDataModule getPlayerDataModule = GetPlayerDataModule;
		GameDataModule getGameDataModule = GetGameDataModule;
		Singleton<ProgressManager>.Instance.SetProgressFor(getGameDataModule.CurLevelId, getInsideGameDataModule.ProgressPercentage, getInsideGameDataModule.GainedDiamonds, getInsideGameDataModule.GainedCrowns);
		if (!getInsideGameDataModule.TryLevel)
		{
			PlayerLocalLevelData playerLevelData = getPlayerDataModule.GetPlayerLevelData(getGameDataModule.CurLevelId);
			int levelMaxProgress = PlayerDataModule.Instance.GetLevelMaxProgress(getGameDataModule.CurLevelId);
			Levels_levelTable levelTableById = getGameDataModule.GetLevelTableById(getGameDataModule.CurLevelId);
			int diamonds = levelTableById.Diamonds;
			int crowns = levelTableById.Crowns;
			getInsideGameDataModule.PreMaxProgress = levelMaxProgress;
			getInsideGameDataModule.PreCrowns = Mathf.Min(playerLevelData.MaxCrownNum, crowns);
			getInsideGameDataModule.PreDiamonds = Mathf.Min(playerLevelData.MaxDiamondNum, diamonds);
			getInsideGameDataModule.PreStarLevel = playerLevelData.MaxStarLevel;
			if (getInsideGameDataModule.GainedDiamonds > diamonds)
			{
				getInsideGameDataModule.GainedDiamonds = diamonds;
			}
			if (getInsideGameDataModule.GainedCrowns > crowns)
			{
				getInsideGameDataModule.GainedCrowns = crowns;
			}
			playerLevelData.PlayLevelCount++;
			if (getInsideGameDataModule.ProgressPercentage >= 100)
			{
				playerLevelData.FinishLevelCount++;
			}
			if (getInsideGameDataModule.GainedDiamonds > 0)
			{
				playerLevelData.TotalDiamondsCount += getInsideGameDataModule.GainedDiamonds;
			}
			if (getInsideGameDataModule.GainedCrowns > 0)
			{
				playerLevelData.TotalCrownsCount += getInsideGameDataModule.GainedCrowns;
			}
			Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).SetLevelMaxProgressData(getGameDataModule.CurLevelId, getInsideGameDataModule.ProgressPercentage, getInsideGameDataModule.GainedDiamonds, getInsideGameDataModule.GainedCrowns, getInsideGameDataModule.CrownCollects.ToArray(), getInsideGameDataModule.DiamondCollects.ToArray());
			if (levelMaxProgress < getInsideGameDataModule.ProgressPercentage)
			{
				Mod.Event.FireNow(this, Mod.Reference.Acquire<LevelPropertyChangeEventArgs>().Initialize(getGameDataModule.CurLevelId, LevelPropertyType.LEVEL_PROGRESS));
			}
			Mod.Event.FireNow(this, Mod.Reference.Acquire<LevelPropertyChangeEventArgs>().Initialize(getGameDataModule.CurLevelId, LevelPropertyType.PLAY_LEVEL_COUNT));
			if (getInsideGameDataModule.ProgressPercentage >= 100)
			{
				Mod.Event.FireNow(this, Mod.Reference.Acquire<LevelPropertyChangeEventArgs>().Initialize(getGameDataModule.CurLevelId, LevelPropertyType.FINISH_LEVEL_COUNT));
			}
			if (getInsideGameDataModule.GainedDiamonds > getInsideGameDataModule.PreDiamonds)
			{
				Mod.Event.FireNow(this, Mod.Reference.Acquire<LevelPropertyChangeEventArgs>().Initialize(getGameDataModule.CurLevelId, LevelPropertyType.LEVEL_DIAMOND_COUNT));
			}
			if (getInsideGameDataModule.GainedCrowns > getInsideGameDataModule.PreCrowns)
			{
				Mod.Event.FireNow(this, Mod.Reference.Acquire<LevelPropertyChangeEventArgs>().Initialize(getGameDataModule.CurLevelId, LevelPropertyType.LEVEL_CROWN_COUNT));
			}
		}
	}

	private void SaveLveleTaget()
	{
	}

	private void OnApplicationQuit()
	{
		Debug.Log("Exit!!");
	}

	private IEnumerator StartRunningIE()
	{
		yield return new WaitForSeconds(0f);
		StartRunning();
		SwitchState(GAMESTATE.Runing);
	}

	private IEnumerator OpenResultView(GameFailEventArgs.GameFailType type)
	{
		if (GameFailEventArgs.GameFailType.TryLevel == type)
		{
			Mod.Event.Fire(this, Mod.Reference.Acquire<TryLevelEndEventArgs>());
			yield return new WaitForSeconds(2.7f);
		}
		else
		{
			yield return new WaitForSeconds(1.2f);
		}
		Mod.UI.CloseUIForm(UIFormId.RuntimeForm);
		if (!DealEnterformPluginAd())
		{
			Mod.UI.OpenUIForm(UIFormId.ResultForm);
		}
		SwitchState(GAMESTATE.Null);
	}

	private IEnumerator OpenRebirthView()
	{
		yield return new WaitForSeconds(1.2f);
		Mod.UI.CloseUIForm(UIFormId.RuntimeForm);
		Mod.UI.OpenUIForm(UIFormId.RebirthForm);
		SwitchState(GAMESTATE.Null);
	}

	private IEnumerator TutorialRebirthForm()
	{
		yield return new WaitForSeconds(0.8f);
		Mod.Event.Fire(this, Mod.Reference.Acquire<TutorialResultEventArgs>());
	}

	private IEnumerator TutorialRebirth()
	{
		yield return new WaitForSeconds(0.8f);
		Mod.Event.FireNow(this, Mod.Reference.Acquire<ChangeRoleTailEffectStateArgs>().Initialize(TailEffectState.IMMEDIATELY_CLOSE));
		Mod.Event.Fire(this, Mod.Reference.Acquire<GameOriginRebirthResetEventArgs>());
		Mod.Event.Fire(this, Mod.Reference.Acquire<PropsRemoveAllEventArgs>().Initialize());
		yield return new WaitForSeconds(0.1f);
		Mod.Event.Fire(this, Mod.Reference.Acquire<ClickGameStartButtonEventArgs>().Initialize());
	}

	private IEnumerator StarLevelUpRebirth()
	{
		yield return new WaitForSeconds(0.8f);
		Mod.Event.FireNow(this, Mod.Reference.Acquire<ChangeRoleTailEffectStateArgs>().Initialize(TailEffectState.IMMEDIATELY_CLOSE));
		Mod.Event.Fire(this, Mod.Reference.Acquire<GameOriginRebirthResetEventArgs>());
	}

	private IEnumerator OpenOriginRebirthView()
	{
		yield return new WaitForSeconds(1.2f);
		Mod.UI.CloseUIForm(UIFormId.RuntimeForm);
		if (GetInsideGameDataModule.IsUnlimitedRebirthLevel)
		{
			bool flag = false;
			if (!GetInsideGameDataModule.IsHaveUnlimitedRebirth && flag)
			{
				GetGoodsData getGoodsData = new GetGoodsData();
				getGoodsData.Buy = true;
				getGoodsData.GoodsId = GameCommon.ORIGIN_REBIRTH_UNLIMITED;
				getGoodsData.GoodsNum = 1.0;
				getGoodsData.ShowExpound = true;
				getGoodsData.CallBackFunc = delegate
				{
					PlayerDataModule.Instance.ChangePlayerTempGoodsNum(GameCommon.ORIGIN_REBIRTH_UNLIMITED, 1);
					Mod.UI.OpenUIForm(UIFormId.OriginRebirthForm, new OriginRebirthForm.OriginRebirthFormData
					{
						m_isGuideUnlimitedRebirth = true
					});
				};
				Mod.UI.OpenUIForm(UIFormId.GetGoodsForm, getGoodsData);
			}
			else
			{
				Mod.Event.Fire(this, Mod.Reference.Acquire<GameOverEventArgs>());
			}
		}
		else
		{
			Mod.Event.Fire(this, Mod.Reference.Acquire<GameOverEventArgs>());
		}
		PlayerDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule);
		int curLevelId = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).CurLevelId;
		dataModule.GetPlayerLevelData(curLevelId);
		InsideGameDataModule getInsideGameDataModule = GetInsideGameDataModule;
		Mod.DataTable.Get<Levels_levelTable>().Get(curLevelId);
		SwitchState(GAMESTATE.Null);
	}

	public void DestroyLocal()
	{
		if (m_baseLevel != null)
		{
			m_baseLevel.DestroyLocal();
		}
		if (m_WorldConfigureController != null)
		{
			m_WorldConfigureController.ResetController();
		}
		if (m_materialController != null)
		{
			m_materialController.DestroyLocal();
		}
		RemoveInGameEvent();
	}

	private void OnDestroy()
	{
		if (Instance != null)
		{
			Instance = null;
		}
	}

	public void StartGuideLineTimer()
	{
		uint num = (uint)GameCommon.guideLineTime;
		if (PlayerDataModule.Instance.PlayerIsHadSpecialStarAbility(1))
		{
			num *= 2;
		}
		guideLineTimer = TimerHeap.AddTimer(num * 1000, 0u, GuideLineTimeOut);
	}

	private void ResumeGuideLineTimer()
	{
		if (guideLineLeftTime != 0)
		{
			guideLineTimer = TimerHeap.AddTimer(guideLineLeftTime, 0u, GuideLineTimeOut);
		}
	}

	private void PauseGuideLineTimer()
	{
		uint leftTimeById = TimerHeap.GetLeftTimeById(guideLineTimer);
		if (leftTimeById != 0)
		{
			StopGuideLineTimer();
			guideLineLeftTime = leftTimeById;
		}
	}

	private void StopGuideLineTimer()
	{
		if (guideLineTimer != 0)
		{
			TimerHeap.DelTimer(guideLineTimer);
		}
	}

	private void GuideLineTimeOut()
	{
		guideLineTimer = 0u;
		Mod.Event.Fire(this, Mod.Reference.Acquire<PathGuideHideEventArgs>().Initialize());
		GetInsideGameDataModule.GuideLine = false;
	}

	private void RecordingEnterGame()
	{
		if (RecordingTileMap.Instance != null && RecordingTileMap.Instance.m_isHaveRecording)
		{
			RecordingRowData recordingRowDataByGridIdAddRowNumber = RecordingTileMap.Instance.GetRecordingRowDataByGridIdAddRowNumber(RecordingTileMap.Instance.m_targetGridID, RecordingTileMap.Instance.m_targetRowNumber);
			Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule).SetCurrentRebirthBoxData(recordingRowDataByGridIdAddRowNumber.m_data);
			Mod.Event.Fire(this, Mod.Reference.Acquire<GameRebirthResetEventArgs>());
		}
	}

	private IEnumerator DelayEnterformPluginAd(float deltyTime)
	{
		yield return new WaitForSeconds(deltyTime);
		DealEnterformPluginAd();
	}

	private bool DealEnterformPluginAd()
	{
		return false;
	}

	private bool DealFirstLevelTutorialPluginAd()
	{
		int id = MonoSingleton<GameTools>.Instacne.ComputerScreenPluginAdId(1);
		ScreenPluginsAd_table screenPluginsAd_table = Mod.DataTable.Get<ScreenPluginsAd_table>()[id];
		bool flag = false;
		if (screenPluginsAd_table != null)
		{
			flag |= PlayerDataModule.Instance.PluginAdController.IsToturialScreenRead();
			if (flag)
			{
				PluginAdData pluginAdData = new PluginAdData();
				pluginAdData.PluginId = 1;
				pluginAdData.Tutorial = true;
				pluginAdData.EndHandler = delegate
				{
					m_isFirstLevelStagePlugin = false;
					ClearEnterFormPluginAdData();
					Mod.UI.OpenUIForm(UIFormId.ResultForm);
				};
				Mod.UI.OpenUIForm(UIFormId.ScreenPluginsForm, pluginAdData);
			}
		}
		return flag;
	}

	private void ClearEnterFormPluginAdData()
	{
		m_lastPlayPluginAdTimeStemp = PlayerDataModule.Instance.ServerTime;
	}

	private bool DealEnterRebirthFormPluginAd()
	{
		return false;
	}

	private void ClearEnterRebirthFormPluginAdData()
	{
		m_lastRebirthPluginAdTimeStemp = PlayerDataModule.Instance.ServerTime;
	}

	private void ResetPluginAdTimeStamp()
	{
		m_lastPlayPluginAdTimeStemp = PlayerDataModule.Instance.ServerTime;
		m_lastRebirthPluginAdTimeStemp = PlayerDataModule.Instance.ServerTime;
	}

	private void OnInputEscape()
	{
		if (InputService.keyDown_Option && (!(m_fairyController != null) || !m_fairyController.IsSkillPlaying()) && !MonoSingleton<GameTools>.Instacne.IsNewPlayerGuideLevel() && m_gameState != GAMESTATE.Cutscene && m_gameState != GAMESTATE.StartAnim)
		{
			bool flag = false;
			Debug.Log(string.Concat(m_gameState, ", ", Mod.UI.UIFormIsOpen(UIFormId.PauseForm).ToString()));
			if (m_gameState != GAMESTATE.Null && m_gameState != GAMESTATE.Reset && m_gameState != 0 && !Mod.UI.UIFormIsOpen(UIFormId.PauseForm))
			{
				flag = true;
				PauseGame();
			}
		}
	}

	private bool DealCoolPlayPackage(System.Action callback)
	{
		if (PlayerDataModule.Instance.CoolPlayPagageData.IsCanReommand())
		{
			new CoolPlayData
			{
				Type = CoolPlayData.OpenType.BUY,
				ShopId = GameCommon.COOLPLAY_PACKAGE,
				CallBack = delegate
				{
					callback();
				}
			};
			return true;
		}
		return false;
	}

	public object GetOriginRebirthData(object obj = null)
	{
		OriginRebirthForRowValue originRebirthForRowValue = new OriginRebirthForRowValue();
		SingleOriginRebirthLocationData singleOriginRebirthLocationData = (SingleOriginRebirthLocationData)obj;
		SingleOriginRebirthSceneData singleOriginRebirthSceneData = new SingleOriginRebirthSceneData
		{
			m_railwayData = (string)m_railway.GetOriginRebirthData(singleOriginRebirthLocationData),
			m_roleData = (string)m_ball.GetOriginRebirthData(singleOriginRebirthLocationData)
		};
		if ((bool)m_couple)
		{
			singleOriginRebirthSceneData.m_coupleData = m_couple.GetOriginRebirthData(singleOriginRebirthLocationData) as string;
		}
		singleOriginRebirthSceneData.m_mapData = (string)m_map.GetOriginRebirthData(singleOriginRebirthLocationData);
		singleOriginRebirthSceneData.m_worldConfigureData = (string)m_WorldConfigureController.GetOriginRebirthData(singleOriginRebirthLocationData);
		singleOriginRebirthSceneData.m_cameraData = (string)m_cameraController.GetOriginRebirthData(singleOriginRebirthLocationData);
		singleOriginRebirthSceneData.m_audioData = (string)Singleton<MenuMusicController>.Instance.GetOriginRebirthData(singleOriginRebirthLocationData);
		singleOriginRebirthSceneData.m_backgroundData = ((m_background != null) ? ((string)m_background.GetOriginRebirthData(singleOriginRebirthLocationData)) : string.Empty);
		singleOriginRebirthSceneData.m_brushData = (string)m_brushController.GetOriginRebirthData(singleOriginRebirthLocationData);
		singleOriginRebirthSceneData.m_progressData = (string)m_progressController.GetOriginRebirthData(singleOriginRebirthLocationData);
		singleOriginRebirthSceneData.m_inputData = (string)InputController.instance.GetOriginRebirthData(singleOriginRebirthLocationData);
		singleOriginRebirthSceneData.m_petData = (string)m_petController.GetOriginRebirthData(singleOriginRebirthLocationData);
		singleOriginRebirthSceneData.m_propData = (string)m_propsController.GetOriginRebirthData(singleOriginRebirthLocationData);
		singleOriginRebirthSceneData.m_gridsData = (string)m_gridController.GetOriginRebirthData(singleOriginRebirthLocationData);
		originRebirthForRowValue.m_locationData = singleOriginRebirthLocationData;
		originRebirthForRowValue.m_sceneData = singleOriginRebirthSceneData;
		return originRebirthForRowValue;
	}

	public void SetOriginRebirthData(object dataInfo)
	{
		SingleOriginRebirthSceneData sceneData = ((SingleOriginRebirthForRowData)dataInfo).m_rowValue.m_sceneData;
		Log.Info("SetOriginRebirthData");
		m_map.CreateGrids();
		m_gridController.SetOriginRebirthData(sceneData.m_gridsData);
		Singleton<MenuMusicController>.Instance.SetOriginRebirthData(sceneData.m_audioData);
		m_map.SetOriginRebirthData(sceneData.m_mapData);
		m_brushController.SetOriginRebirthData(sceneData.m_brushData);
		m_railway.SetOriginRebirthData(sceneData.m_railwayData);
		m_ball.SetOriginRebirthData(sceneData.m_roleData);
		if ((bool)m_couple)
		{
			m_couple.SetOriginRebirthData(sceneData.m_coupleData);
		}
		m_cameraController.SetOriginRebirthData(sceneData.m_cameraData);
		m_WorldConfigureController.SetOriginRebirthData(sceneData.m_worldConfigureData);
		if (m_background != null)
		{
			m_background.SetOriginRebirthData(sceneData.m_backgroundData);
		}
		m_progressController.SetOriginRebirthData(sceneData.m_progressData);
		InputController.instance.SetOriginRebirthData(sceneData.m_inputData);
		m_petController.SetOriginRebirthData(sceneData.m_petData);
		m_propsController.SetOriginRebirthData(sceneData.m_propData);
	}

	public void StartRunByOriginRebirthData(object dataInfo)
	{
		SingleOriginRebirthSceneData sceneData = ((SingleOriginRebirthForRowData)dataInfo).m_rowValue.m_sceneData;
		m_gridController.StartRunByOriginRebirthData(sceneData.m_gridsData);
		m_map.StartRunByOriginRebirthData(sceneData.m_mapData);
		m_brushController.StartRunByOriginRebirthData(sceneData.m_brushData);
		m_railway.StartRunByOriginRebirthData(sceneData.m_railwayData);
		m_ball.StartRunByOriginRebirthData(sceneData.m_roleData);
		if ((bool)m_couple)
		{
			m_couple.StartRunByOriginRebirthData(sceneData.m_coupleData);
		}
		m_WorldConfigureController.StartRunByOriginRebirthData(sceneData.m_worldConfigureData);
		m_cameraController.StartRunByOriginRebirthData(sceneData.m_cameraData);
		Singleton<MenuMusicController>.Instance.StartRunByOriginRebirthData(sceneData.m_audioData);
		if (m_background != null)
		{
			m_background.StartRunByOriginRebirthData(sceneData.m_backgroundData);
		}
		m_progressController.StartRunByOriginRebirthData(sceneData.m_progressData);
		InputController.instance.StartRunByOriginRebirthData(sceneData.m_inputData);
		m_petController.StartRunByOriginRebirthData(sceneData.m_petData);
		m_propsController.StartRunByOriginRebirthData(sceneData.m_propData);
	}

	public OriginRebirthForRowValue GetOriginRebirthBsonData(SingleOriginRebirthLocationData obj)
	{
		OriginRebirthForRowValue originRebirthForRowValue = new OriginRebirthForRowValue();
		SingleOriginRebirthSceneData singleOriginRebirthSceneData = new SingleOriginRebirthSceneData
		{
			RailwayData = m_railway.GetOriginRebirthBsonData(obj),
			RoleData = m_ball.GetOriginRebirthBsonData(obj)
		};
		if ((bool)m_couple)
		{
			singleOriginRebirthSceneData.CoupleData = m_couple.GetOriginRebirthBsonData(obj);
		}
		singleOriginRebirthSceneData.MapData = m_map.GetOriginRebirthBsonData(obj);
		singleOriginRebirthSceneData.WorldConfigureData = m_WorldConfigureController.GetOriginRebirthBsonData(obj);
		singleOriginRebirthSceneData.CameraData = m_cameraController.GetOriginRebirthBsonData(obj);
		singleOriginRebirthSceneData.AudioData = Singleton<MenuMusicController>.Instance.GetOriginRebirthBsonData(obj);
		singleOriginRebirthSceneData.BackgroundData = ((m_background != null) ? m_background.GetOriginRebirthBsonData(obj) : null);
		singleOriginRebirthSceneData.BrushData = m_brushController.GetOriginRebirthBsonData(obj);
		singleOriginRebirthSceneData.ProgressData = m_progressController.GetOriginRebirthBsonData(obj);
		singleOriginRebirthSceneData.InputData = InputController.instance.GetOriginRebirthBsonData(obj);
		singleOriginRebirthSceneData.PetData = m_petController.GetOriginRebirthBsonData(obj);
		singleOriginRebirthSceneData.PropData = m_propsController.GetOriginRebirthBsonData(obj);
		singleOriginRebirthSceneData.GridsData = m_gridController.GetOriginRebirthBsonData(obj);
		originRebirthForRowValue.m_locationData = obj;
		originRebirthForRowValue.m_sceneData = singleOriginRebirthSceneData;
		return originRebirthForRowValue;
	}

	public void SetOriginRebirthBsonData(SingleOriginRebirthForRowData dataInfo)
	{
		try
		{
			SingleOriginRebirthSceneData sceneData = dataInfo.m_rowValue.m_sceneData;
			Log.Info("SetOriginRebirthData");
			m_map.CreateGrids();
			m_gridController.SetOriginRebirthBsonData(sceneData.GridsData);
			Singleton<MenuMusicController>.Instance.SetOriginRebirthBsonData(sceneData.AudioData);
			m_map.SetOriginRebirthBsonData(sceneData.MapData);
			m_brushController.SetOriginRebirthBsonData(sceneData.BrushData);
			m_railway.SetOriginRebirthBsonData(sceneData.RailwayData);
			m_ball.SetOriginRebirthBsonData(sceneData.RoleData);
			if ((bool)m_couple)
			{
				m_couple.SetOriginRebirthBsonData(sceneData.CoupleData);
			}
			m_cameraController.SetOriginRebirthBsonData(sceneData.CameraData);
			m_WorldConfigureController.SetOriginRebirthBsonData(sceneData.WorldConfigureData);
			if (m_background != null)
			{
				m_background.SetOriginRebirthBsonData(sceneData.BackgroundData);
			}
			m_progressController.SetOriginRebirthBsonData(sceneData.ProgressData);
			InputController.instance.SetOriginRebirthBsonData(sceneData.InputData);
			m_petController.SetOriginRebirthBsonData(sceneData.PetData);
			m_propsController.SetOriginRebirthBsonData(sceneData.PropData);
		}
		catch (Exception arg)
		{
			GameDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule);
			int curLevelId = dataModule.CurLevelId;
			Log.Error(string.Format("{0} , GameController.SetOriginRebirthBsonData() is error, levelid is {1}", arg, (dataModule != null) ? curLevelId.ToString() : "null"));
		}
	}

	public void StartRunByOriginRebirthBsonData(SingleOriginRebirthForRowData dataInfo)
	{
		try
		{
			SingleOriginRebirthSceneData sceneData = dataInfo.m_rowValue.m_sceneData;
			m_gridController.StartRunByOriginRebirthBsonData(sceneData.GridsData);
			m_map.StartRunByOriginRebirthBsonData(sceneData.MapData);
			m_brushController.StartRunByOriginRebirthBsonData(sceneData.BrushData);
			m_railway.StartRunByOriginRebirthBsonData(sceneData.RailwayData);
			m_ball.StartRunByOriginRebirthBsonData(sceneData.RoleData);
			if ((bool)m_couple)
			{
				m_couple.StartRunByOriginRebirthBsonData(sceneData.CoupleData);
			}
			m_WorldConfigureController.StartRunByOriginRebirthBsonData(sceneData.WorldConfigureData);
			m_cameraController.StartRunByOriginRebirthBsonData(sceneData.CameraData);
			Singleton<MenuMusicController>.Instance.StartRunByOriginRebirthBsonData(sceneData.AudioData);
			if (m_background != null)
			{
				m_background.StartRunByOriginRebirthBsonData(sceneData.BackgroundData);
			}
			m_progressController.StartRunByOriginRebirthBsonData(sceneData.ProgressData);
			InputController.instance.StartRunByOriginRebirthBsonData(sceneData.InputData);
			m_petController.StartRunByOriginRebirthBsonData(sceneData.PetData);
			m_propsController.StartRunByOriginRebirthBsonData(sceneData.PropData);
		}
		catch (Exception arg)
		{
			GameDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule);
			int curLevelId = dataModule.CurLevelId;
			Log.Error(string.Format("{0} , GameController.StartRunByOriginRebirthBsonData() is error, levelid is {1}", arg, (dataModule != null) ? curLevelId.ToString() : "null"));
		}
	}

	public void OnResetForOriginRebirth()
	{
		Singleton<MenuMusicController>.Instance.StopGameMusic();
		InputController.instance.Reset();
		m_map.ResetWorld();
		m_railway.ResetRailway();
		m_cameraController.ResetController();
		m_ball.ResetBall();
		m_ball.SetEndEmission();
		if ((bool)m_couple)
		{
			m_couple.ResetCouple();
		}
	}

	public static SingleOriginRebirthForRowData GetOriginRebirthForRowValueByGridIdAndPoint(TileMapScripTable tileMap, OriginRebirthData originRebirth, int gridId, Point point, Vector3 position)
	{
		if (originRebirth == null)
		{
			return null;
		}
		if (tileMap == null)
		{
			return null;
		}
		SingleOriginRebirthForRowData singleOriginRebirthForRowData = null;
		List<OriginRebirthForRowData> originRebirthDatas = originRebirth.m_originRebirthDatas;
		Dictionary<OriginRebirthForRowKey, List<OriginRebirthForRowValue>> dictionary = new Dictionary<OriginRebirthForRowKey, List<OriginRebirthForRowValue>>();
		for (int i = 0; i < originRebirthDatas.Count; i++)
		{
			dictionary.Add(originRebirthDatas[i].m_rowKey, originRebirthDatas[i].m_rowValues);
		}
		OriginRebirthForRowKey originRebirthForRowKey = default(OriginRebirthForRowKey);
		List<OriginRebirthForRowValue> value = null;
		int num = gridId;
		int num2 = point.m_y;
		int num3 = 0;
		int num4 = 10;
		List<OriginRebirthForRowData> list = new List<OriginRebirthForRowData>();
		while (num2 >= 0)
		{
			originRebirthForRowKey.SetData(num, num2);
			if (dictionary.TryGetValue(originRebirthForRowKey, out value))
			{
				OriginRebirthForRowData originRebirthForRowData = new OriginRebirthForRowData();
				originRebirthForRowData.m_rowKey = originRebirthForRowKey;
				originRebirthForRowData.m_rowValues = value;
				list.Add(originRebirthForRowData);
				num3++;
				if (num3 >= num4)
				{
					break;
				}
				continue;
			}
			num2--;
			if (num2 < 0)
			{
				List<int> parentIds = tileMap.m_gridDatas.GetGridDataByID(num).m_parentIds;
				if (parentIds == null || parentIds.Count <= 0)
				{
					break;
				}
				num = parentIds[0];
				num2 = tileMap.m_gridDatas.GetGridDataByID(num).m_height - 1;
			}
		}
		float num5 = float.MaxValue;
		float num6 = num5;
		OriginRebirthForRowData originRebirthForRowData2 = null;
		OriginRebirthForRowKey rowKey = default(OriginRebirthForRowKey);
		OriginRebirthForRowValue originRebirthForRowValue = null;
		for (int j = 0; j < list.Count; j++)
		{
			originRebirthForRowData2 = list[j];
			rowKey = list[j].m_rowKey;
			for (int k = 0; k < originRebirthForRowData2.m_rowValues.Count; k++)
			{
				num6 = Vector3.Distance(originRebirthForRowData2.m_rowValues[k].m_locationData.m_position.ToVector3(), position);
				if (num6 <= num5)
				{
					originRebirthForRowValue = originRebirthForRowData2.m_rowValues[k];
					num5 = num6;
				}
			}
		}
		if (originRebirthForRowValue != null)
		{
			singleOriginRebirthForRowData = new SingleOriginRebirthForRowData();
			singleOriginRebirthForRowData.m_rowKey = rowKey;
			singleOriginRebirthForRowData.m_rowValue = originRebirthForRowValue;
		}
		return singleOriginRebirthForRowData;
	}

	public static SingleOriginRebirthForRowData GetOriginRebirthForRowValueByPosition(Vector3 position)
	{
		SingleOriginRebirthForRowData result = null;
		Grid grid = null;
		List<Grid> getGrids = m_map.GetGrids;
		for (int i = 0; i < getGrids.Count; i++)
		{
			if (getGrids[i].IsOnGrid(position))
			{
				grid = getGrids[i];
				break;
			}
		}
		if (grid == null)
		{
			for (int j = 0; j < getGrids.Count; j++)
			{
				if (getGrids[j].IsOnGridForIgnoremSamplingInterval(position))
				{
					grid = getGrids[j];
					break;
				}
			}
		}
		if (grid != null)
		{
			Point pointByPosition = grid.GetPointByPosition(position);
			result = GetOriginRebirthForRowValueByGridIdAndPoint(LevelResources.theResource.TileMap, LevelResources.theResource.OriginRebirth, grid.m_id, pointByPosition, position);
		}
		return result;
	}

	public static SingleOriginRebirthForRowData GetOriginRebirthForRowValueByPosition(Grid[] _grids, TileMapScripTable _tileMap, Vector3 position)
	{
		SingleOriginRebirthForRowData result = null;
		Grid grid = null;
		for (int i = 0; i < _grids.Length; i++)
		{
			if (_grids[i].IsOnGrid(position))
			{
				grid = _grids[i];
				break;
			}
		}
		if (grid == null)
		{
			for (int j = 0; j < _grids.Length; j++)
			{
				if (_grids[j].IsOnGridForIgnoremSamplingInterval(position))
				{
					grid = _grids[j];
					break;
				}
			}
		}
		if (grid != null)
		{
			Point pointByPosition = grid.GetPointByPosition(position);
			result = GetOriginRebirthForRowValueByGridIdAndPoint(_tileMap, LevelResources.theResource.OriginRebirth, grid.m_id, pointByPosition, position);
		}
		return result;
	}

	public static SingleOriginRebirthForRowData GetOriginRebirthForRowValueByPosition(Grid[] _grids, TileMapScripTable _tileMap, OriginRebirthData rebirthData, Vector3 position)
	{
		SingleOriginRebirthForRowData result = null;
		Grid grid = null;
		for (int i = 0; i < _grids.Length; i++)
		{
			if (_grids[i].IsOnGrid(position))
			{
				grid = _grids[i];
				break;
			}
		}
		if (grid == null)
		{
			for (int j = 0; j < _grids.Length; j++)
			{
				if (_grids[j].IsOnGridForIgnoremSamplingInterval(position))
				{
					grid = _grids[j];
					break;
				}
			}
		}
		if (grid != null)
		{
			Point pointByPosition = grid.GetPointByPosition(position);
			result = GetOriginRebirthForRowValueByGridIdAndPoint(_tileMap, rebirthData, grid.m_id, pointByPosition, position);
		}
		return result;
	}

	private bool CheckOriginRebirth(Vector3 position)
	{
		bool result = false;
		Grid grid = null;
		Point defaultValue = Point.DefaultValue;
		List<Grid> getGrids = m_map.GetGrids;
		for (int i = 0; i < getGrids.Count; i++)
		{
			if (getGrids[i].IsOnGrid(position))
			{
				grid = getGrids[i];
				break;
			}
		}
		if (grid == null)
		{
			for (int j = 0; j < getGrids.Count; j++)
			{
				if (getGrids[j].IsOnGridForIgnoremSamplingInterval(position))
				{
					grid = getGrids[j];
					break;
				}
			}
		}
		if (grid != null)
		{
			defaultValue = grid.GetPointByPosition(position);
		}
		else
		{
			grid = MapController.Instance.CurrentRoleGrid;
			defaultValue = MapController.Instance.CurrentRolePoint;
		}
		if (grid != null)
		{
			defaultValue = grid.GetPointByPosition(position);
			TileMapScripTable tileMap = LevelResources.theResource.TileMap;
			SingleOriginRebirthForRowData singleOriginRebirthForRowData = GetOriginRebirthForRowValueByGridIdAndPoint(tileMap, LevelResources.theResource.OriginRebirth, grid.m_id, defaultValue, position);
			if (GetInsideGameDataModule.CurrentOriginRebirth != null && singleOriginRebirthForRowData != null)
			{
				if (GetInsideGameDataModule.CurrentOriginRebirth.m_rowKey.m_gridID == singleOriginRebirthForRowData.m_rowKey.m_gridID)
				{
					if (GetInsideGameDataModule.CurrentOriginRebirth.m_rowValue.m_locationData.m_point.m_y >= singleOriginRebirthForRowData.m_rowValue.m_locationData.m_point.m_y)
					{
						singleOriginRebirthForRowData = GetInsideGameDataModule.CurrentOriginRebirth;
					}
				}
				else
				{
					int count = grid.m_parentGrids.Count;
					bool flag = false;
					Grid[] parentGridsByGrid = grid.GetParentGridsByGrid();
					for (int k = 0; k < parentGridsByGrid.Length; k++)
					{
						if (parentGridsByGrid[k].m_id == GetInsideGameDataModule.CurrentOriginRebirth.m_rowKey.m_gridID)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						GridData gridDataByID = tileMap.m_gridDatas.GetGridDataByID(GetInsideGameDataModule.CurrentOriginRebirth.m_rowKey.m_gridID);
						GridData gridDataByID2 = tileMap.m_gridDatas.GetGridDataByID(singleOriginRebirthForRowData.m_rowKey.m_gridID);
						int num = gridDataByID.m_beforeTheLength + GetInsideGameDataModule.CurrentOriginRebirth.m_rowKey.m_row;
						int num2 = gridDataByID2.m_beforeTheLength + singleOriginRebirthForRowData.m_rowKey.m_row;
						if (num < num2)
						{
							flag = true;
						}
					}
					if (!flag)
					{
						singleOriginRebirthForRowData = GetInsideGameDataModule.CurrentOriginRebirth;
					}
				}
			}
			GetInsideGameDataModule.SetCurrentOriginRebirth(singleOriginRebirthForRowData);
			if (GetInsideGameDataModule.CurrentOriginRebirth != null)
			{
				result = true;
			}
		}
		return result;
	}
}
