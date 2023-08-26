using System;
using System.Collections;
using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.SceneManagement;
using User.TileMap;

using Grid = User.TileMap.Grid;

public class RolePrince : BaseRole
{
	public enum TrailState
	{
		None,
		Active,
		Closing
	}

	public static readonly float DropEffectiveSpeed = 1.8f;

	public static readonly string NodeModelPoint = "modelPoint";

	public static readonly string NodeRolePart = "role";

	public static readonly string NodeBodyCenter = "Bip001";

	public static readonly string NodeBodyHead = "Bip001 Head";

	public static readonly string NodeBodyLeftFoot = "Bip001 L Foot";

	public static readonly string NodeBodyRightFoot = "Bip001 R Foot";

	public static readonly string NodeBodyLeftHand = "Bip001 L Hand";

	public static readonly string NodeBodyRightHand = "Bip001 R Hand";

	public static readonly string NodeBodyRightShoulder = "Bip001 R UpperArm";

	public static readonly string NodeBodyLeftShoulder = "Bip001 L UpperArm";

	public static readonly string NodeEffectPart = "effect";

	public static readonly string NodeEffectLocal = "localEffect";

	public static readonly string NodeEffectSmoke = "smoke";

	public static readonly string NodeEffectDeadRoot = "deadEffect";

	public static readonly string NodeEffectDeadBoom = "Tx_dead_long";

	public static readonly string NodeEffectChangeRoleRoot = "bianshenEffect";

	public static readonly string NodeEffectChangeRoleGlow = "glow_002";

	public static readonly string NodeEffectGetChest = "thiefEffect";

	public static readonly string NodeEffectRune = "trail_rune";

	public static readonly string NodeLinePart = "Line";

	public static readonly string NodeSoundPart = "sounds";

	public static readonly string NodeSoundDrop = "sound_dropDie";

	public static readonly string NodeSoundCrash = "sound_crashDie";

	public static readonly string NodeTreasure = "modelPoint/role/Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 R Clavicle/Bip001 R UpperArm/Bip001 R Forearm/Bip001 R Hand/Thief_baoxiang_a_001";

	public static readonly string NodeMoodFaces = "modelPoint/moodFaces";

	public static readonly string NodeModeHappy = "moodHappy";

	public static readonly string NodeModeAngry = "moodAngry";

	public static readonly string NodeFaceCover = "faceCover";

	public static readonly string LayerMaskAutoMesh = "AutoMesh";

	public static readonly float MaxRotateEuler = 60f;

	public static readonly float DefaultRoleMoveLimit = 10f;

	public static readonly float VehicleMoveLimit = 3.5f;

	public static readonly float PetMoveLimit = 5f;

	public static readonly float DizzinessTime = 2f;

	private RoleLineElment roleLineElement;

	private GameObject treasureElement;

	private TrailRenderer trailElement0;

	private TrailRenderer trailElement1;

	private TrailRenderer trailElement2;

	private GameObject trailRune;

	private float trailTime0;

	private float trailTime1;

	private float trailTime2;

	private Transform changeRoleObj;

	private Transform effectPart;

	private Transform faceCoverTrans;

	private Animation faceCoverAnim;

	private AudioSource soundDropDie;

	private AudioSource soundCrashDie;

	private Dictionary<int, string> animStateNameDic = new Dictionary<int, string>();

	private float dizzinessCounter;

	private bool ifDanceTogether;

	private int debugMoveIndex;

	private bool ifDropDieStatic;

	private bool ifRebirthWaiting;

	private bool ifLerpEmission;

	private float delayEmissionTime;

	private float lerpEmissionTime;

	private float beginEmissionValue;

	private float endEmissionValue;

	private float currentEmissionTime;

	private Coroutine emissionCoroutine;

	private SkinnedMeshRenderer[] emissionRender;

	private MeshRenderer[] otherRender;

	private Material[] emissionMaterial;

	private bool ifRebirthing;

	private Vector3 m_rolePostionFoPathtoMoveLerp = Vector3.zero;

	private IEnumerator SamuraiCor;

	private Vector3 m_ropeVector3_01 = Vector3.zero;

	private float m_float_01;

	private float m_float_02;

	private float m_float_03;

	private TrailState trailState;

	public float TrailFadeTime = 0.0166666675f;

	public static bool PauseDebugMove = false;

	private RD_RolePrince_DATA dataVal;

	public bool IfTrailActive
	{
		get
		{
			return trailState == TrailState.Active;
		}
	}

	public override bool IsRecordOriginRebirth
	{
		get
		{
			return true;
		}
	}

	public void Awake()
	{
		RoleConstruct();
	}

	public override void PreInitialize()
	{
		for (int i = 0; i < 65; i++)
		{
			Dictionary<int, string> dictionary = animStateNameDic;
			int key = i;
			AnimType animType = (AnimType)i;
			dictionary[key] = animType.ToString();
		}
		ballModelObj = base.transform.Find(NodeModelPoint).gameObject;
		ballRoleObj = ballModelObj.transform.Find(NodeRolePart).gameObject;
		Transform transform = base.transform.Find(NodeTreasure);
		if ((bool)transform)
		{
			treasureElement = transform.gameObject;
			treasureElement.SetActive(false);
		}
		if ((bool)ballModelObj)
		{
			emissionRender = ballModelObj.GetComponentsInChildren<SkinnedMeshRenderer>();
			otherRender = ballModelObj.GetComponentsInChildren<MeshRenderer>();
			if (ifLerpEmission && emissionRender != null && emissionRender.Length != 0)
			{
				emissionMaterial = new Material[emissionRender.Length];
				for (int j = 0; j < emissionRender.Length; j++)
				{
					emissionMaterial[j] = emissionRender[j].material;
					emissionRender[j].material = emissionMaterial[j];
				}
			}
		}
		m_runtimeAnimatorControllerIndex = 0;
		_roleAnimator = ballRoleObj.GetComponent<Animator>();
		_roleAnimator.runtimeAnimatorController = m_roleProperty.m_runtimeAnimatorControllers[m_runtimeAnimatorControllerIndex];
		_roleAnimator.speed = Railway.theRailway.SpeedForward / 6f;
		ballRoleObj.SetActive(true);
		m_roleSkinIndex = 0;
		m_roleSkinController = base.gameObject.GetComponentInChildren<RoleSkinController>();
		if (m_roleSkinController != null)
		{
			m_roleSkinController.SetSkinByIndex(m_roleSkinIndex);
		}
		effectPart = base.transform.Find(NodeEffectPart);
		Transform transform2 = effectPart.Find(NodeEffectLocal);
		Transform transform3 = base.transform.Find(NodeSoundPart);
		ballDeadObj = effectPart.Find(NodeEffectDeadRoot).gameObject;
		ballDeadObj.SetActive(false);
		deadBombEffect = ballDeadObj.transform.Find(NodeEffectDeadBoom).GetComponent<ParticleSystem>();
		deadBombEffect.Stop();
		runSmokeEffect = transform2.Find(NodeEffectSmoke).gameObject;
		runSmokeEffect.SetActive(false);
		Transform transform4 = effectPart.Find(NodeEffectGetChest);
		if ((bool)transform4)
		{
			thiefChestObj = transform4.gameObject;
			thiefChestEffect = thiefChestObj.GetComponentsInChildren<ParticleSystem>();
		}
		changeRoleObj = transform2.Find(NodeEffectChangeRoleRoot);
		if ((bool)changeRoleObj)
		{
			changeRoleObj.gameObject.SetActive(false);
		}
		if (soundDropDie == null)
		{
			soundDropDie = transform3.Find(NodeSoundDrop).GetComponent<AudioSource>();
		}
		if (soundCrashDie == null)
		{
			soundCrashDie = transform3.Find(NodeSoundCrash).GetComponent<AudioSource>();
		}
		if (DogLineEndPart == null)
		{
			DogLineEndPart = base.transform.Find("model/role/DogLineEndPoint");
		}
		InitBodyPart();
		InitTrail();
		InitMoodFaces();
	}

	public override void Initialize(Transform parent, Vector3 startLocalPos, Vector3 restartLocalPos)
	{
		base.Initialize(parent, startLocalPos, restartLocalPos);
		base.gameObject.SetActive(true);
		_roleAnimator.speed = Railway.theRailway.SpeedForward / 6f;
		base.transform.SetParent(parent);
		StartRoleLocalPos = startLocalPos;
		RestartRoleLocalPos = restartLocalPos;
		SetLocalPosition(StartRoleLocalPos);
		SpeedForward = m_roleProperty.SpeedForward;
		SlideSpeed = m_roleProperty.SlideSpeed;
		RotationSpeed = m_roleProperty.RotationSpeed;
		TrailFadeTime = m_roleProperty.TrailFadeSpeed;
		updateStateList.Clear();
		updateStateList.Add(0, UpdateStatic);
		updateStateList.Add(1, UpdateStartAnim);
		updateStateList.Add(3, UpdateMove);
		updateStateList.Add(4, UpdateJump);
		updateStateList.Add(5, UpdateJump);
		updateStateList.Add(6, UpdateLand);
		updateStateList.Add(7, UpdateNormalDrop);
		updateStateList.Add(8, UpdateDropDie);
		updateStateList.Add(9, UpdateCrash);
		updateStateList.Add(10, UpdateFinish);
		updateStateList.Add(2, UpdateCutscene);
		updateStateList.Add(11, UpdatePathToMove);
		updateStateList.Add(15, UpdatePathToMoveLerp);
		updateStateList.Add(12, UpdatePathToMoveFixed);
		updateStateList.Add(13, UpdateSwingingRope);
		InitFaceCover();
		ChangeStateTo(BallState.Static);
		PathTriggerIndex = -1;
		PathFixTriggerIndex = -1;
		PathLerpTriggerIndex = -1;
		base.IfRebirthOnVehicle = false;
		base.IfWinBeforeFinish = false;
		CurrentWorldRow = 0;
		LastRow = 0;
		base.IfDizziness = false;
		dizzinessCounter = 0f;
		ifDanceTogether = false;
		ifDropDieStatic = false;
		WaitingBall();
		m_dynamicBoneFactory = new DynamicBoneFactory(this, m_roleProperty.m_roleDynamicBoneData);
		m_dynamicBoneFactory.OnInitialize();
		base.RoleMoveLimit = DefaultRoleMoveLimit;
		StartCoroutine(WaitSwitchDynamicBoneType(DynamicBoneFactory.DynamicBoneType.Initialize));
		debugMoveIndex = 0;
		currentEmissionTime = 0f;
		GetInnerGlowData();
		ShowInnerGlowData(false);
		ifRebirthing = false;
	}

	private IEnumerator WaitSwitchDynamicBoneType(DynamicBoneFactory.DynamicBoneType dynamicBoneType)
	{
		yield return new WaitForEndOfFrame();
		m_dynamicBoneFactory.SwitchDynamicBoneType(dynamicBoneType);
	}

	public override void InitializeByRole(BaseRole oriRole)
	{
		base.InitializeByRole(oriRole);
		base.gameObject.SetActive(true);
		base.transform.SetParent(oriRole.transform.parent);
		StartRoleLocalPos = oriRole.StartRoleLocalPos;
		RestartRoleLocalPos = oriRole.RestartRoleLocalPos;
		base.transform.localPosition = oriRole.transform.localPosition;
		base.transform.localEulerAngles = oriRole.transform.localEulerAngles;
		SpeedForward = m_roleProperty.SpeedForward;
		SlideSpeed = m_roleProperty.SlideSpeed;
		RotationSpeed = m_roleProperty.RotationSpeed;
		TrailFadeTime = m_roleProperty.TrailFadeSpeed;
		updateStateList.Clear();
		updateStateList.Add(0, UpdateStatic);
		updateStateList.Add(1, UpdateStartAnim);
		updateStateList.Add(3, UpdateMove);
		updateStateList.Add(4, UpdateJump);
		updateStateList.Add(5, UpdateJump);
		updateStateList.Add(6, UpdateLand);
		updateStateList.Add(7, UpdateNormalDrop);
		updateStateList.Add(8, UpdateDropDie);
		updateStateList.Add(9, UpdateCrash);
		updateStateList.Add(10, UpdateFinish);
		updateStateList.Add(2, UpdateCutscene);
		updateStateList.Add(11, UpdatePathToMove);
		updateStateList.Add(15, UpdatePathToMoveLerp);
		updateStateList.Add(12, UpdatePathToMoveFixed);
		updateStateList.Add(13, UpdateSwingingRope);
		base.IfRebirthOnVehicle = false;
		base.IfWinBeforeFinish = false;
		CurrentWorldRow = oriRole.CurrentWorldRow;
		LastRow = oriRole.LastRow;
		CurrentTile = oriRole.CurrentTile;
		currentGrid = oriRole.currentGrid;
		if (oriRole.CurrentState == BallState.PathToMove)
		{
			PathTriggerIndex = oriRole.PathTriggerIndex;
			ChangeToPathToMove((oriRole as RolePrince).m_pathData);
			currentPathPointIndex = (oriRole as RolePrince).currentPathPointIndex;
			_roleAnimator.Play(animStateNameDic[6]);
			if ((bool)changeRoleObj)
			{
				changeRoleObj.gameObject.SetActive(true);
			}
		}
		else if (oriRole.CurrentState == BallState.PathToMoveLerp)
		{
			PathLerpTriggerIndex = oriRole.PathLerpTriggerIndex;
			ChangeToPathToMoveLerp((oriRole as RolePrince).m_pathLerpData);
			currentPathPointIndex = (oriRole as RolePrince).currentPathPointIndex;
			_roleAnimator.Play(animStateNameDic[6]);
			if ((bool)changeRoleObj)
			{
				changeRoleObj.gameObject.SetActive(true);
			}
		}
		else if (oriRole.CurrentState == BallState.PathToMoveFixed)
		{
			PathFixTriggerIndex = oriRole.PathFixTriggerIndex;
			ChangeToPathToMoveFixed((oriRole as RolePrince).m_transformPathDatas);
			currentPathPointIndex = (oriRole as RolePrince).currentPathPointIndex;
			_roleAnimator.Play(animStateNameDic[6]);
			if ((bool)changeRoleObj)
			{
				changeRoleObj.gameObject.SetActive(true);
			}
		}
		else
		{
			if ((bool)changeRoleObj)
			{
				changeRoleObj.gameObject.SetActive(true);
			}
			ChangeStateTo(oriRole.CurrentState);
		}
		base.RoleMoveLimit = oriRole.RoleMoveLimit;
		m_dynamicBoneFactory = new DynamicBoneFactory(this, m_roleProperty.m_roleDynamicBoneData);
		m_dynamicBoneFactory.OnInitialize();
		StartCoroutine(WaitSwitchDynamicBoneType(DynamicBoneFactory.DynamicBoneType.StartBall));
		jumpUtilInfo = oriRole.GetJumpUtilInfo();
		GetInnerGlowData();
		ShowInnerGlowData(false);
	}

	public override void DevInitialize(Transform parent, Vector3 startLocalPos, Vector3 restartLocalPos)
	{
		base.DevInitialize(parent, startLocalPos, restartLocalPos);
		base.transform.SetParent(parent);
		StartRoleLocalPos = startLocalPos;
		RestartRoleLocalPos = restartLocalPos;
		SetLocalPosition(StartRoleLocalPos);
	}

	public override void SetEmissionInfo(bool ifEmission, float delayTime, float emissionTime, float beginEmission, float endEmission)
	{
		ifLerpEmission = ifEmission;
		delayEmissionTime = delayTime;
		lerpEmissionTime = emissionTime;
		beginEmissionValue = beginEmission;
		endEmissionValue = endEmission;
	}

	public override void SetBeginEmission()
	{
		if (ifLerpEmission && emissionRender != null && emissionRender.Length != 0)
		{
			emissionMaterial = new Material[emissionRender.Length];
			for (int i = 0; i < emissionRender.Length; i++)
			{
				emissionMaterial[i] = emissionRender[i].material;
				emissionMaterial[i].SetFloat("_Emmission", beginEmissionValue);
				emissionRender[i].material = emissionMaterial[i];
			}
		}
	}

	public override void SetEndEmission()
	{
		if (ifLerpEmission && emissionRender != null && emissionRender.Length != 0)
		{
			emissionMaterial = new Material[emissionRender.Length];
			for (int i = 0; i < emissionRender.Length; i++)
			{
				emissionMaterial[i] = emissionRender[i].material;
				emissionMaterial[i].SetFloat("_Emmission", endEmissionValue);
				emissionRender[i].material = emissionMaterial[i];
			}
		}
	}

	public override void ResetBall()
	{
		base.IsInvincible = false;
		base.transform.localPosition = StartRoleLocalPos;
		ballRoleObj.SetActive(true);
		ChangeStateTo(BallState.Static);
		if (base.gameObject.activeSelf)
		{
			StartCoroutine(WaitSwitchDynamicBoneType(DynamicBoneFactory.DynamicBoneType.ResetBall));
		}
		SetLocalPosition(StartRoleLocalPos);
		m_isHaveWind = false;
		m_runtimeAnimatorControllerIndex = 0;
		if (_roleAnimator != null)
		{
			_roleAnimator.runtimeAnimatorController = m_roleProperty.m_runtimeAnimatorControllers[m_runtimeAnimatorControllerIndex];
			_roleAnimator.speed = Railway.theRailway.SpeedForward / 6f;
		}
		m_roleSkinIndex = 0;
		if (m_roleSkinController != null)
		{
			m_roleSkinController.SetSkinByIndex(m_roleSkinIndex);
		}
		base.transform.eulerAngles = Vector3.zero;
		ballModelObj.transform.localPosition = Vector3.zero;
		ballModelObj.transform.eulerAngles = Vector3.zero;
		CurrentTile = null;
		LastTile = null;
		currentGrid = null;
		if (CurrentVehicle != null)
		{
			CurrentVehicle.DepartFromBall(this, false);
			CurrentVehicle = null;
		}
		currentPet = null;
		collideTileList.Clear();
		deadBombEffect.Stop();
		PathTriggerIndex = -1;
		PathFixTriggerIndex = -1;
		PathLerpTriggerIndex = -1;
		ballDeadObj.SetActive(false);
		CloseTrail();
		runSmokeEffect.SetActive(false);
		jumpType = JumpType.Normal;
		ResetInputParam();
		ClearForceHorizonData();
		base.IfRebirthOnVehicle = false;
		base.IfWinBeforeFinish = false;
		IfSpecialDie = false;
		IfPlayRoleEff = true;
		CurrentWorldRow = 0;
		LastRow = 0;
		WaitingBall();
		currentInputType = InputType.Horizen;
		jumpUtilInfo = null;
		base.RoleMoveLimit = DefaultRoleMoveLimit;
		ChangeRuntimeAnimatorControllerByIndex(0, false);
		base.IfHaveTreasureChest = false;
		base.IfDizziness = false;
		dizzinessCounter = 0f;
		ifDanceTogether = false;
		ifDropDieStatic = false;
		if ((bool)treasureElement)
		{
			treasureElement.SetActive(false);
		}
		ResetMoodFaces();
		ForceShowCoverFace(false);
		debugMoveIndex = 0;
		ResetAnimFlag();
		ShowInnerGlowData(false);
		currentEmissionTime = 0f;
		ifRebirthing = false;
		if (emissionCoroutine != null)
		{
			StopCoroutine(emissionCoroutine);
		}
	}

	public override void ResetBySavePointInfo(RebirthBoxData savePoint)
	{
		ResetBall();
		m_runtimeAnimatorControllerIndex = savePoint.m_roleData.m_runtimeAnimatorControllerIndex;
		_roleAnimator.runtimeAnimatorController = m_roleProperty.m_runtimeAnimatorControllers[m_runtimeAnimatorControllerIndex];
		if (m_roleSkinController != null)
		{
			m_roleSkinIndex = savePoint.m_roleData.m_roleSkinIndex;
			m_roleSkinController.SetSkinByIndex(m_roleSkinIndex);
		}
		RolePlayAnim(AnimType.WaitingState);
		IfSpecialDie = false;
		IfPlayRoleEff = true;
		SetLocalPosition(savePoint.m_roleData.RoleLocalPos);
		base.IfRebirthOnVehicle = savePoint.m_rebirthVehicleData != null && savePoint.m_rebirthVehicleData.m_vehicleGridId != -1;
		bool ifRebirthOnVehicle = base.IfRebirthOnVehicle;
		if (savePoint.m_rebirthWindData.m_isHaveWind)
		{
			Mod.Event.FireNow(this, Mod.Reference.Acquire<WindOpenEventArgs>().Initialize(savePoint.m_rebirthWindData.m_windData));
		}
		m_pathData = savePoint.m_roleData.m_pathToMovebyRoleData;
		currentPathPointIndex = savePoint.m_roleData.m_currentPathToMoveIndex;
		m_transformPathDatas = savePoint.m_roleData.m_pathToMoveFixedByRoleTriggerData;
		base.RoleMoveLimit = savePoint.m_roleData.m_roleMoveLimit;
		StartCoroutine(WaitSwitchDynamicBoneType(DynamicBoneFactory.DynamicBoneType.RebirthResetBall));
	}

	public override RebirthRoleData GetRebirthData(Transform boxTriggerTransform)
	{
		RebirthRoleData rebirthRoleData = new RebirthRoleData();
		Vector3 localPosition = base.transform.localPosition;
		Vector3 vector = base.transform.InverseTransformPoint(boxTriggerTransform.position);
		rebirthRoleData.RoleLocalPos = new Vector3(localPosition.x + vector.x, localPosition.y + vector.y, localPosition.z);
		if (CurrentState == BallState.PathToMove || CurrentState == BallState.PathToMoveFixed)
		{
			Vector3 origin = new Vector3(boxTriggerTransform.position.x, base.transform.position.y, base.transform.position.z) + Vector3.up * 3f;
			RaycastHit hitInfo;
			if (base.IfOnVehicle && Physics.Raycast(origin, Vector3.down, out hitInfo, 10f, LayerMask.GetMask(LayerMaskAutoMesh)))
			{
				Vector3 vector2 = base.transform.InverseTransformPoint(hitInfo.point);
				rebirthRoleData.RoleLocalPos = vector2 + localPosition;
			}
		}
		rebirthRoleData.m_roleState = CurrentState;
		rebirthRoleData.m_currentPathToMoveIndex = currentPathPointIndex;
		rebirthRoleData.m_pathToMovebyRoleData = m_pathData;
		rebirthRoleData.m_pathToMoveFixedByRoleTriggerData = m_transformPathDatas;
		rebirthRoleData.m_roleAnimType = m_currentAnimType;
		rebirthRoleData.m_trailActive = IfTrailActive;
		rebirthRoleData.m_runtimeAnimatorControllerIndex = m_runtimeAnimatorControllerIndex;
		rebirthRoleData.m_roleSkinIndex = m_roleSkinIndex;
		rebirthRoleData.m_roleMoveLimit = base.RoleMoveLimit;
		return rebirthRoleData;
	}

	public override void StartBall(RebirthBoxData savePoint = null)
	{
		InputController.instance.EnableInput(true);
		ballRoleObj.SetActive(true);
		if (base.IfRebirthOnVehicle && savePoint != null)
		{
			RolePlayAnim(savePoint.m_roleData.m_roleAnimType);
		}
		if (savePoint != null)
		{
			currentGrid = MapController.Instance.GetGridById(savePoint.m_gridId).transform;
			if (savePoint.m_roleData.m_roleState == BallState.PathToMove)
			{
				m_pathData.m_ifHaveEffect = savePoint.m_roleData.m_trailActive;
				ChangeToPathToMove(m_pathData, savePoint.m_roleData.m_currentPathToMoveIndex);
			}
			else if (savePoint.m_roleData.m_roleState == BallState.PathToMoveFixed)
			{
				m_pathData.m_ifHaveEffect = savePoint.m_roleData.m_trailActive;
				ChangeToPathToMoveFixed(m_transformPathDatas, savePoint.m_roleData.m_currentPathToMoveIndex);
			}
			else
			{
				ChangeStateTo(BallState.Move);
			}
		}
		else
		{
			currentGrid = MapController.Instance.GetGridByFirst().transform;
			ChangeStateTo(BallState.Move);
			if (CurrentVehicle != null)
			{
				CurrentVehicle.OnGameStart();
			}
		}
		if (m_isHaveWind)
		{
			m_dynamicBoneFactory.SwitchDynamicBoneType(m_windData.m_isRightWind ? DynamicBoneFactory.DynamicBoneType.OpenRightWind : DynamicBoneFactory.DynamicBoneType.OpenLeftWind);
		}
		else
		{
			m_dynamicBoneFactory.SwitchDynamicBoneType(DynamicBoneFactory.DynamicBoneType.StartBall);
		}
	}

	public override void CrashBall()
	{
		if (base.IfDie || CurrentState == BallState.Cutscene || base.IsInvincible)
		{
			return;
		}
		if (!IfSpecialDie && base.GetGameController.m_propsController != null && base.GetGameController.m_propsController.IsContains(PropsName.SHIELD))
		{
			Mod.Event.FireNow(this, Mod.Reference.Acquire<PropsTriggerEventArgs>().Initialize(PropsName.SHIELD));
			return;
		}
		if (CameraController.theCamera.IfDieShakeCamera)
		{
			CameraShake.ShakeCamera(0.5f, true);
		}
		if (base.gameObject.name == "RoleSamurai(Clone)")
		{
			_roleAnimator.StopPlayback();
			SwitchSamuraiGobj(true);
		}
		ballRoleObj.SetActive(false);
		if (IfPlayRoleEff)
		{
			deadBombEffect.Play();
			ballDeadObj.SetActive(true);
		}
		if (CurrentVehicle != null)
		{
			CurrentVehicle.DepartFromBall(this, false);
			CurrentVehicle.DieTogether(this);
			CurrentVehicle = null;
		}
		GameFailEventArgs gameFailEventArgs = Mod.Reference.Acquire<GameFailEventArgs>();
		if (IfSpecialDie)
		{
			gameFailEventArgs.Initialize(GameFailEventArgs.GameFailType.Special, base.transform.position);
		}
		else
		{
			gameFailEventArgs.Initialize(GameFailEventArgs.GameFailType.Crash, base.transform.position);
		}
		Mod.Event.Fire(this, gameFailEventArgs);
		Singleton<DataModuleManager>.Instance.GetDataModule<ResultPageDataModule>(DataNames.ResultPageDataModule).FinishedType = LevelFinishedType.Killed;
		if (runSmokeEffect.activeSelf)
		{
			runSmokeEffect.SetActive(false);
		}
		ChangeStateTo(BallState.CrashDie);
		if (GameController.Instance.GetPlayerDataModule.IsSoundPlayOn())
		{
			soundCrashDie.Play();
		}
	}

	public override void SpecialCrashBall(Vector3 diePos, bool ifMoveCamera = true)
	{
		CrashBall();
	}

	public override void BeginDropDie()
	{
		if (GameController.IfNotDeath || base.IfDie)
		{
			return;
		}
		Mod.Event.FireNow(this, Mod.Reference.Acquire<DropDieEventArgs>().Initialize(base.transform.position));
		if (IfSpecialDie)
		{
			SpecialCrashBall(base.transform.position);
			return;
		}
		Vector3 diePos = base.transform.position;
		if ((bool)CurrentVehicle)
		{
			CurrentVehicle.DepartFromBall(this, false);
			CurrentVehicle.DieTogether(this);
			CurrentVehicle = null;
		}
		if (base.IfJumpingDown && jumpUtilInfo != null)
		{
			diePos = jumpUtilInfo.BeginPos;
		}
		if (base.IfDropping)
		{
			diePos = dropBeginPos;
		}
		ChangeStateTo(BallState.DropDie);
		if (runSmokeEffect.activeSelf)
		{
			runSmokeEffect.SetActive(false);
		}
		RolePlayAnim(AnimType.DropDieState);
		UpdateDropDie();
		GameFailEventArgs gameFailEventArgs = Mod.Reference.Acquire<GameFailEventArgs>();
		gameFailEventArgs.Initialize(GameFailEventArgs.GameFailType.Drop, diePos);
		Mod.Event.Fire(this, gameFailEventArgs);
		Singleton<DataModuleManager>.Instance.GetDataModule<ResultPageDataModule>(DataNames.ResultPageDataModule).FinishedType = LevelFinishedType.Fall;
		if (GameController.Instance.GetPlayerDataModule.IsSoundPlayOn())
		{
			soundDropDie.Play();
		}
	}

	public override void CallBeginJump(Vector3 beginPos, Vector3 endPos, Vector3 normalDir, float heightY, JumpType jType, bool isShowTrail = true)
	{
		if (!base.IfJumping)
		{
			if (base.IfOnVehicle && !CurrentVehicle.CanJump)
			{
				DepartVehicle();
			}
			if (runSmokeEffect.activeSelf)
			{
				runSmokeEffect.SetActive(false);
			}
			BeginJump(beginPos, endPos, normalDir, heightY, jType, isShowTrail);
			ChangeStateTo(BallState.JumpUp);
		}
	}

	public override void CallBeginDrop(Vector3 beginPos, Vector3 endPos, Vector3 normalDir, float heightY)
	{
		BeginDrop(beginPos, endPos, normalDir, heightY);
		ChangeStateTo(BallState.NormalDrop);
	}

	public override void CallEndJump(float tileY, bool ifContinue = false)
	{
		if (!base.IfJumpingDown)
		{
			return;
		}
		ResetPosY(tileY);
		BeginCloseTrail();
		if (CurrentState == BallState.JumpDown)
		{
			ChangeStateTo(BallState.Land);
			if (!ifContinue)
			{
				if (jumpType == JumpType.Ship)
				{
					RolePlayAnim(AnimType.JumpOnBoatStandState);
				}
				else if (jumpType == JumpType.Super)
				{
					RolePlayAnim(AnimType.SuperJumpToRoll);
				}
				else if (jumpType == JumpType.QTE || jumpType == JumpType.QTEBetween)
				{
					RolePlayAnim(AnimType.SuperJumpToRoll);
				}
			}
			else if (jumpType != JumpType.QTE)
			{
				JumpType jumpType2 = jumpType;
				int num = 3;
			}
		}
		jumpUtilInfo = null;
	}

	public override void CallEndDrop(float tileY)
	{
		if (CurrentState == BallState.NormalDrop && Mathf.Abs(dropDeltaY) > DropEffectiveSpeed)
		{
			dropDeltaY = 0f;
			ResetPosY(tileY);
			if (ifRebirthing)
			{
				RolePlayAnim(AnimType.WaitingState, 0, 0f, true);
			}
			else
			{
				RolePlayAnim(AnimType.SuperJumpToRoll);
			}
			ChangeStateTo(BallState.Land);
		}
	}

	public override void ChangeToStartAnim()
	{
		ChangeStateTo(BallState.StartAnim);
		RolePlayAnim(AnimType.SquatToStandUp);
	}

	public override void ChangeToCutscene()
	{
		StartCoroutine(WaitSwitchDynamicBoneType(DynamicBoneFactory.DynamicBoneType.Cutscene));
		ChangeStateTo(BallState.Cutscene);
		if (!runSmokeEffect.activeSelf)
		{
			runSmokeEffect.SetActive(true);
		}
		RolePlayAnim(AnimType.StandUpToRun);
	}

	public override void ChangeToSwingingRope(SwingingRipeData swingingRipeData)
	{
		base.ChangeToSwingingRope(swingingRipeData);
		m_swingingRipeData = swingingRipeData;
		ChangeStateTo(BallState.SwingingRope);
	}

	public override void ChangeToMove()
	{
		TriggerRolePlayAnim(AnimType.RunState);
		ChangeStateTo(BallState.Move);
	}

	public override void ChangeToWin(Vector3 pos)
	{
		if (SceneManager.GetActiveScene().name == "Home_Rainbow_E")
		{
			ballRoleObj.SetActive(false);
		}
		SetPosition(pos);
		Mod.Event.Fire(this, Mod.Reference.Acquire<GameSucessEventArgs>());
		Singleton<DataModuleManager>.Instance.GetDataModule<ResultPageDataModule>(DataNames.ResultPageDataModule).FinishedType = LevelFinishedType.Win;
		RolePlayAnim(AnimType.SuccessState);
		m_dynamicBoneFactory.SwitchDynamicBoneType(DynamicBoneFactory.DynamicBoneType.ChangeToWin);
		ChangeStateTo(BallState.Finish);
	}

	public override void ChangeToFly()
	{
		if (base.IfJumping)
		{
			RolePlayAnim(AnimType.FlyState);
		}
	}

	public override void ChangeToFloat()
	{
		if (base.IfJumping)
		{
			RolePlayAnim(AnimType.FloatState);
		}
	}

	public override void ChangeToGlide()
	{
		if (base.IfJumping)
		{
			ChangeStateTo(BallState.Glide);
		}
	}

	public override void CallChangeToPathToMove(PathToMoveByRoleTrigger.PathToMoveByRoleTriggerData pathData, int triggerIndex, int pathPointIndex = 0)
	{
		PathTriggerIndex = triggerIndex;
		ChangeToPathToMove(pathData, pathPointIndex);
	}

	public override void CallChangeToPathToMoveLerp(PathToMoveByRoleForLerpTrigger.PathToMoveByRoleForLerpTriggerData pathData, int triggerIndex, int pathPointIndex = 0)
	{
		PathLerpTriggerIndex = triggerIndex;
		ChangeToPathToMoveLerp(pathData, pathPointIndex);
		m_rolePostionFoPathtoMoveLerp = currentGrid.transform.InverseTransformPoint(base.transform.position);
	}

	public override void CallChangeToPathToMoveFixed(PathToMoveFixedByRoleTrigger.PathToMoveFixedByRoleTriggerData pathData, int triggerIndex, int pathPointIndex = 0)
	{
		PathFixTriggerIndex = triggerIndex;
		ChangeToPathToMoveFixed(pathData, pathPointIndex);
	}

	protected override void ChangeToPathToMove(PathToMoveByRoleTrigger.PathToMoveByRoleTriggerData pathData, int pathPointIndex = 0)
	{
		m_pathData = pathData;
		if (pathData.m_bezierPositions == null || pathData.m_bezierPositions.Length == 0)
		{
			Vector3[] array = new Vector3[m_pathData.m_positions.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = currentGrid.InverseTransformPoint(m_pathData.m_positions[i]);
			}
			m_path = ThreeBezier.GetPathByPositions(array, m_pathData.m_smooth);
		}
		else
		{
			m_path = pathData.m_bezierPositions;
		}
		currentPathPointIndex = pathPointIndex;
		if (m_pathData.m_ifHaveEffect && !ifRebirthing)
		{
			OpenTrail();
		}
		ChangeStateTo(BallState.PathToMove);
	}

	protected override void ChangeToPathToMoveLerp(PathToMoveByRoleForLerpTrigger.PathToMoveByRoleForLerpTriggerData pathData, int pathPointIndex = 0)
	{
		m_pathLerpData = pathData;
		if (pathData.m_pathData.m_bezierPositions == null || pathData.m_pathData.m_bezierPositions.Length == 0)
		{
			Vector3[] array = new Vector3[pathData.m_pathData.m_positions.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = currentGrid.InverseTransformPoint(pathData.m_pathData.m_positions[i]);
			}
			m_path = ThreeBezier.GetPathByPositions(array, pathData.m_pathData.m_smooth);
		}
		else
		{
			m_path = pathData.m_pathData.m_bezierPositions;
		}
		currentPathPointIndex = pathPointIndex;
		if (pathData.m_pathData.m_ifHaveEffect && !ifRebirthing)
		{
			OpenTrail();
		}
		ChangeStateTo(BallState.PathToMoveLerp);
	}

	protected override void ChangeToPathToMoveFixed(PathToMoveFixedByRoleTrigger.PathToMoveFixedByRoleTriggerData pathData, int pathPointIndex = 0)
	{
		m_transformPathDatas = pathData;
		if (pathData.m_bezierTransfroms == null || pathData.m_bezierTransfroms.Length == 0)
		{
			PathToMoveTransform[] array = new PathToMoveTransform[m_transformPathDatas.m_transfroms.Length];
			m_transformPathDatas.m_transfroms.CopyTo(array, 0);
			for (int i = 0; i < array.Length; i++)
			{
				array[i].m_position = currentGrid.InverseTransformPoint(array[i].m_position);
			}
			m_transformsPath = ThreeBezier.GetPathByPathToMoveTransform(array, m_transformPathDatas.m_smooth);
		}
		else
		{
			m_transformsPath = pathData.m_bezierTransfroms;
		}
		currentPathPointIndex = pathPointIndex;
		ChangeStateTo(BallState.PathToMoveFixed);
	}

	private void SwitchSamuraiGobj(bool open)
	{
		ballRoleObj.transform.Find("Bip001/Bip001 Prop1/dao1").gameObject.SetActive(open);
		Transform transform = ballRoleObj.transform.Find("Bip001/Bip001 Prop2");
		for (int i = 0; i < 3; i++)
		{
			transform.GetChild(i).gameObject.SetActive(open);
		}
		ballRoleObj.transform.Find("CenterFoot").gameObject.SetActive(open);
		ballRoleObj.transform.Find("jianke").gameObject.SetActive(open);
	}

	private IEnumerator DelayOpenSamuraiGobj()
	{
		yield return new WaitForSeconds(1f);
		SwitchSamuraiGobj(true);
	}

	protected override void RolePlayAnim(AnimType animType, int layerID = 0, float normalizedTime = 0f, bool ifForce = false)
	{
		_roleAnimator.SetBool("BoatStandRoll", false);
		_roleAnimator.SetBool("SuperJumpToRoll", false);
		if (ifForce)
		{
			_roleAnimator.Play(animStateNameDic[(int)animType], 0, 0f);
		}
		else
		{
			switch (animType)
			{
			case AnimType.JumpState:
				if (_roleAnimator.GetCurrentAnimatorStateInfo(layerID).IsName(animStateNameDic[4]))
				{
					_roleAnimator.SetBool("RunToJump", true);
				}
				else
				{
					_roleAnimator.Play(animStateNameDic[(int)animType], layerID, normalizedTime);
				}
				break;
			case AnimType.SuperJumpState:
				if (_roleAnimator.GetCurrentAnimatorStateInfo(layerID).IsName(animStateNameDic[4]))
				{
					_roleAnimator.SetBool("RunToSuperJump", true);
				}
				else
				{
					_roleAnimator.Play(animStateNameDic[(int)animType], layerID, normalizedTime);
				}
				break;
			case AnimType.SuperJumpToRoll:
				if (_roleAnimator.GetCurrentAnimatorStateInfo(layerID).IsName(animStateNameDic[6]))
				{
					_roleAnimator.SetBool("SuperJumpToRoll", true);
				}
				else
				{
					_roleAnimator.Play(animStateNameDic[7], layerID, normalizedTime);
				}
				break;
			case AnimType.SquatToStandUp:
				if (_roleAnimator.GetCurrentAnimatorStateInfo(layerID).IsName(animStateNameDic[1]))
				{
					_roleAnimator.SetBool("SquatToStandUp", true);
				}
				else
				{
					_roleAnimator.Play(animStateNameDic[3], layerID, normalizedTime);
				}
				break;
			case AnimType.StandUpToRun:
				if (_roleAnimator.GetCurrentAnimatorStateInfo(layerID).IsName(animStateNameDic[21]))
				{
					_roleAnimator.SetBool("StandUpToRun", true);
				}
				else
				{
					_roleAnimator.Play(animStateNameDic[4], layerID, normalizedTime);
				}
				break;
			case AnimType.FlyState:
				if (_roleAnimator.GetCurrentAnimatorStateInfo(layerID).IsName(animStateNameDic[6]))
				{
					_roleAnimator.SetBool("SuperJumpToFly", true);
				}
				else
				{
					_roleAnimator.Play(animStateNameDic[(int)animType], layerID, normalizedTime);
				}
				break;
			case AnimType.FloatState:
				if (_roleAnimator.GetCurrentAnimatorStateInfo(layerID).IsName(animStateNameDic[22]))
				{
					_roleAnimator.SetBool("FlyToFloat", true);
				}
				else
				{
					_roleAnimator.Play(animStateNameDic[(int)animType], layerID, normalizedTime);
				}
				break;
			case AnimType.BoatStandRoll:
				if (_roleAnimator.GetCurrentAnimatorStateInfo(layerID).IsName(animStateNameDic[14]))
				{
					_roleAnimator.SetBool("BoatStandRoll", true);
				}
				else
				{
					_roleAnimator.Play(animStateNameDic[(int)animType], layerID, normalizedTime);
				}
				break;
			case AnimType.SuccessState:
				if (!_roleAnimator.GetCurrentAnimatorStateInfo(layerID).IsName(animStateNameDic[10]))
				{
					_roleAnimator.Play(animStateNameDic[(int)animType], layerID, normalizedTime);
				}
				break;
			case AnimType.SquatState:
				_roleAnimator.Play(animStateNameDic[(int)animType], layerID, normalizedTime);
				break;
			default:
				_roleAnimator.Play(animStateNameDic[(int)animType], layerID, normalizedTime);
				break;
			}
		}
		m_currentAnimType = animType;
	}

	public override void RoleStopAnim()
	{
		_roleAnimator.speed = 0f;
	}

	public override void TriggerRolePlayAnim(AnimType animType, bool ifForce = true)
	{
		if (!ifRebirthing && !base.IfDie)
		{
			RolePlayAnim(animType, 0, 0f, true);
		}
	}

	public override void ChangeRuntimeAnimatorControllerByIndex(int index, bool ifPlay = true)
	{
		if (!(_roleAnimator != null) || !(m_roleProperty != null) || m_roleProperty.m_runtimeAnimatorControllers == null || m_roleProperty.m_runtimeAnimatorControllers.Length <= index)
		{
			return;
		}
		RuntimeAnimatorController runtimeAnimatorController = m_roleProperty.m_runtimeAnimatorControllers[index];
		if (runtimeAnimatorController != null)
		{
			m_runtimeAnimatorControllerIndex = index;
			AnimatorStateInfo currentAnimatorStateInfo = _roleAnimator.GetCurrentAnimatorStateInfo(0);
			int shortNameHash = currentAnimatorStateInfo.shortNameHash;
			float normalizedTime = currentAnimatorStateInfo.normalizedTime;
			_roleAnimator.runtimeAnimatorController = runtimeAnimatorController;
			if (ifPlay)
			{
				_roleAnimator.Play(shortNameHash, 0, normalizedTime);
			}
		}
	}

	public override void ChangeRoleSkinByIndex(int index)
	{
		if (m_roleSkinController != null && m_roleSkinController.SetSkinByIndex(index))
		{
			m_roleSkinIndex = index;
		}
	}

	public override void ResetAnimFlag()
	{
		_roleAnimator.SetBool("RunToJump", false);
		_roleAnimator.SetBool("RunToSuperJump", false);
		_roleAnimator.SetBool("SuperJumpToRoll", false);
		_roleAnimator.SetBool("SquatToStandUp", false);
		_roleAnimator.SetBool("StandUpToRun", false);
		_roleAnimator.SetBool("SuperJumpToFly", false);
		_roleAnimator.SetBool("FlyToFloat", false);
		_roleAnimator.SetBool("BoatStandRoll", false);
	}

	public override bool IfPlayBoatStandRoll()
	{
		return _roleAnimator.GetCurrentAnimatorStateInfo(0).IsName(animStateNameDic[15]);
	}

	protected virtual void UpdateCutscene()
	{
		float speedForward = Railway.theRailway.SpeedForward;
		float num = base.transform.localPosition.z + speedForward * Time.deltaTime - StartRoleLocalPos.z;
		float num2 = RestartRoleLocalPos.z - StartRoleLocalPos.z;
		float num3 = num / num2;
		if (num3 > 1f)
		{
			num3 = 1f;
		}
		base.transform.localPosition = StartRoleLocalPos + (RestartRoleLocalPos - StartRoleLocalPos) * num3;
		if (num3 == 1f)
		{
			GameStartEventArgs gameStartEventArgs = Mod.Reference.Acquire<GameStartEventArgs>();
			gameStartEventArgs.Initialize(GameStartEventArgs.GameStartType.ForceRun);
			Mod.Event.Fire(this, gameStartEventArgs);
			ChangeStateTo(BallState.Static);
		}
		roleLineElement.UpdateLine();
	}

	protected virtual void UpdateStatic()
	{
		if ((bool)CurrentVehicle)
		{
			CurrentVehicle.ForceSetTrans();
		}
	}

	protected virtual void UpdateStartAnim()
	{
		roleLineElement.UpdateLine();
	}

	protected virtual void UpdateMove()
	{
		if (!runSmokeEffect.activeSelf)
		{
			runSmokeEffect.SetActive(true);
		}
		if (currentInputType == InputType.Horizen)
		{
			if (!base.IfWinBeforeFinish)
			{
				MoveX(InputController.InputPercent);
			}
			else
			{
				ResetTurning();
				ReleaseHandByWind();
			}
			UpdateForceMove();
			if (CurrentTile == null)
			{
				if (!base.IfOnVehicle && !base.IsInvincible)
				{
					BeginDropDie();
				}
			}
			else
			{
				ResetPosY(CurrentTile.RealPosY + CurrentTile.TileMagin);
			}
			if (CurrentDancer != null && CurrentDancer.GetDancingState() == 5)
			{
				ballModelObj.transform.localRotation = Quaternion.identity;
			}
		}
		else if (currentInputType == InputType.FreeMove && !base.IfWinBeforeFinish)
		{
			FlyMove(InputController.FreeOffset);
		}
		roleLineElement.UpdateLine();
	}

	protected virtual void UpdatePathToMoveFixed()
	{
		Railway theRailway = Railway.theRailway;
		float z = currentGrid.transform.InverseTransformPoint(theRailway.transform.position).z;
		float num = z + theRailway.SpeedForward * Time.deltaTime;
		Vector3 nextP = Vector3.zero;
		Quaternion nextR = Quaternion.identity;
		Vector3 nextS = Vector3.one;
		if (GetPathNextPathToMoveTransform(num, m_transformsPath, ref nextP, ref nextR, ref nextS))
		{
			Vector3 vector = currentGrid.transform.InverseTransformPoint(base.transform.position);
			Vector3 position = vector;
			float num2 = nextP.z - z;
			float num3 = 1f;
			if (num2 > 0f)
			{
				num3 = (num - z) / num2;
				position.y = Mathf.Lerp(vector.y, nextP.y, num3);
				position.x = Mathf.Lerp(vector.x, nextP.x, num3);
			}
			base.transform.position = currentGrid.transform.TransformPoint(position);
			ballModelObj.transform.rotation = Quaternion.Lerp(ballModelObj.transform.rotation, nextR, m_transformPathDatas.m_rotationSpeed * Time.deltaTime);
		}
		else if (m_transformPathDatas.m_isFinishedStop)
		{
			ballModelObj.transform.rotation = Quaternion.identity;
		}
		else
		{
			if (!base.IfWinBeforeFinish)
			{
				ChangeToMove();
			}
			ballModelObj.transform.rotation = Quaternion.identity;
		}
		roleLineElement.UpdateLine();
	}

	protected virtual void UpdatePathToMoveLerp()
	{
		float num = 0f;
		int nextIndex = 0;
		Railway theRailway = Railway.theRailway;
		float z = currentGrid.InverseTransformPoint(base.transform.position).z;
		float num2 = z + theRailway.SpeedForward * Time.deltaTime;
		Vector3 nextP = Vector3.zero;
		if (GetPathNextPoint(num2, m_path, ref nextP, ref nextIndex))
		{
			Vector3 vector = currentGrid.transform.InverseTransformPoint(base.transform.position);
			Vector3 position = vector;
			float num3 = nextP.z - z;
			float num4 = 1f;
			if (num3 > 0f)
			{
				num4 = (num2 - z) / num3;
				if (nextIndex >= m_pathLerpData.m_lerpIndex)
				{
					position.y = Mathf.Lerp(vector.y, nextP.y, num4);
					position.x = Mathf.Lerp(vector.x, nextP.x, num4);
				}
				else
				{
					float t = (vector.z - m_rolePostionFoPathtoMoveLerp.z) / (m_path[m_pathLerpData.m_lerpIndex].z - m_rolePostionFoPathtoMoveLerp.z);
					position.y = Mathf.Lerp(m_rolePostionFoPathtoMoveLerp.y, m_path[m_pathLerpData.m_lerpIndex].y, t);
					position.x = Mathf.Lerp(m_rolePostionFoPathtoMoveLerp.x, m_path[m_pathLerpData.m_lerpIndex].x, t);
				}
			}
			base.transform.position = currentGrid.transform.TransformPoint(position);
			ballModelObj.transform.rotation = Quaternion.Lerp(ballModelObj.transform.rotation, Quaternion.identity, Time.deltaTime);
			if (base.IfOnVehicle)
			{
				CurrentVehicle.SetVehicleRotation(ballModelObj.transform, new Vector3(0f, position.y - vector.y, num2 - z), num * base.RealRotateSpeed, MaxRotateEuler);
			}
		}
		else
		{
			if (m_pathLerpData.m_pathData.m_ifHaveEffect)
			{
				CloseTrail();
			}
			if (!m_pathLerpData.m_pathData.m_isFinishedStop)
			{
				ChangeToMove();
			}
		}
		roleLineElement.UpdateLine();
	}

	protected virtual void UpdatePathToMove()
	{
		float num = 0f;
		Railway theRailway = Railway.theRailway;
		float z = currentGrid.InverseTransformPoint(base.transform.position).z;
		float num2 = z + theRailway.SpeedForward * Time.deltaTime;
		Vector3 nextP = Vector3.zero;
		if (currentInputType == InputType.Horizen)
		{
			num = MoveX(InputController.InputPercent);
		}
		else if (currentInputType == InputType.FreeMove && !base.IfWinBeforeFinish)
		{
			FlyMove(InputController.FreeOffset);
		}
		if (m_pathData.m_ifHaveEffect)
		{
			RotateTrail();
		}
		Vector3 origin = base.transform.position + base.transform.up * 3f;
		RaycastHit hitInfo;
		if (base.IfOnVehicle && Physics.Raycast(origin, Vector3.down, out hitInfo, 10f, LayerMask.GetMask(LayerMaskAutoMesh)))
		{
			Vector3 point = hitInfo.point;
			base.transform.position = point;
			ballModelObj.transform.up = Vector3.Lerp(ballModelObj.transform.up, hitInfo.normal, Time.deltaTime * 10f);
			CurrentVehicle.OnRaycastHit(true, hitInfo, this);
		}
		else
		{
			if (GetPathNextPoint(num2, m_path, ref nextP))
			{
				Vector3 vector = currentGrid.transform.InverseTransformPoint(base.transform.position);
				Vector3 position = vector;
				float num3 = nextP.z - z;
				float num4 = 1f;
				if (num3 > 0f)
				{
					num4 = (num2 - z) / num3;
					position.y = Mathf.Lerp(vector.y, nextP.y, num4);
				}
				base.transform.position = currentGrid.transform.TransformPoint(position);
				if (base.IfOnVehicle)
				{
					CurrentVehicle.SetVehicleRotation(ballModelObj.transform, new Vector3(0f, position.y - vector.y, num2 - z), num * base.RealRotateSpeed, MaxRotateEuler);
				}
			}
			else
			{
				if (m_pathData.m_ifHaveEffect)
				{
					CloseTrail();
				}
				if (!m_pathData.m_isFinishedStop)
				{
					Vector3 vector2 = currentGrid.transform.InverseTransformPoint(base.transform.position);
					Vector3 position2 = vector2;
					position2.y = Mathf.Lerp(vector2.y, nextP.y, 1f);
					base.transform.position = currentGrid.transform.TransformPoint(position2);
					if (base.IfOnVehicle)
					{
						CurrentVehicle.SetVehicleRotation(ballModelObj.transform, new Vector3(0f, position2.y - vector2.y, num2 - z), num * base.RealRotateSpeed, MaxRotateEuler);
					}
					ChangeToMove();
				}
			}
			if (base.IfOnVehicle)
			{
				CurrentVehicle.OnRaycastHit(false, default(RaycastHit), this);
			}
		}
		roleLineElement.UpdateLine();
	}

	protected virtual void UpdateSwingingRope()
	{
		if (currentInputType == InputType.Horizen)
		{
			MoveX(InputController.InputPercent);
		}
		else if (currentInputType == InputType.FreeMove && !base.IfWinBeforeFinish)
		{
			FlyMove(InputController.FreeOffset);
		}
		if (m_swingingRipeData == null || !(BaseRole.theBall != null) || !(ballModelObj != null))
		{
			return;
		}
		m_ropeVector3_01 = BaseRole.theBall.transform.position;
		m_float_01 = Vector3.Distance(m_swingingRipeData.m_axisPosition, new Vector3(m_ropeVector3_01.x, m_swingingRipeData.m_axisPosition.y, m_ropeVector3_01.z));
		if (m_float_01 <= m_swingingRipeData.m_radius)
		{
			m_float_02 = Mathf.Pow(m_swingingRipeData.m_radius, 2f) - Mathf.Pow(m_ropeVector3_01.x - m_swingingRipeData.m_axisPosition.x, 2f) - Mathf.Pow(m_ropeVector3_01.z - m_swingingRipeData.m_axisPosition.z, 2f);
			m_float_03 = 0f;
			if (m_ropeVector3_01.y > m_swingingRipeData.m_axisPosition.y)
			{
				m_float_03 = m_swingingRipeData.m_axisPosition.y + Mathf.Sqrt(m_float_02);
			}
			else
			{
				m_float_03 = m_swingingRipeData.m_axisPosition.y - Mathf.Sqrt(m_float_02);
			}
			m_ropeVector3_01 = new Vector3(m_ropeVector3_01.x, m_float_03, m_ropeVector3_01.z);
			BaseRole.theBall.transform.position = m_ropeVector3_01;
			Quaternion rotation = Quaternion.FromToRotation(Vector3.up, (m_swingingRipeData.m_axisPosition - m_ropeVector3_01).normalized);
			ballModelObj.transform.rotation = rotation;
		}
		else
		{
			Log.Info("on finished");
		}
	}

	protected virtual void UpdateJump()
	{
		if (!base.IfWinBeforeFinish)
		{
			MoveX(InputController.InputPercent);
		}
		else
		{
			ResetTurning();
			ReleaseHandByWind();
		}
		UpdateForceMove();
		bool flag = Jumping() <= 0.5f;
		if (CurrentState == BallState.JumpUp && !flag)
		{
			ChangeStateTo(BallState.JumpDown);
			if (jumpType == JumpType.Ship)
			{
				RolePlayAnim(AnimType.JumpOnBoatDownState);
			}
		}
		roleLineElement.UpdateLine();
	}

	protected virtual void UpdateLand()
	{
		ChangeStateTo(BallState.Move);
		UpdateMove();
	}

	protected virtual void UpdateNormalDrop()
	{
		if (!base.IfWinBeforeFinish)
		{
			MoveX(InputController.InputPercent);
		}
		Dropping();
		roleLineElement.UpdateLine();
	}

	protected virtual void UpdateDropDie()
	{
		Vector3 position = base.transform.position;
		if (!ifDropDieStatic)
		{
			position += 6f * Time.deltaTime * base.transform.forward;
		}
		position -= 6f * Time.deltaTime * base.transform.up;
		base.transform.position = position;
		roleLineElement.UpdateLine();
	}

	protected virtual void UpdateCrash()
	{
	}

	protected virtual void UpdateFinish()
	{
		roleLineElement.UpdateLine();
	}

	private bool GetPathNextPoint(float targetZ, Vector3[] bezierGridPoints, ref Vector3 nextP)
	{
		for (int i = currentPathPointIndex; i < bezierGridPoints.Length; i++)
		{
			if (targetZ <= bezierGridPoints[i].z)
			{
				currentPathPointIndex = i;
				nextP = bezierGridPoints[currentPathPointIndex];
				return true;
			}
		}
		currentPathPointIndex = bezierGridPoints.Length - 1;
		nextP = bezierGridPoints[currentPathPointIndex];
		return false;
	}

	private bool GetPathNextPoint(float targetZ, Vector3[] bezierGridPoints, ref Vector3 nextP, ref int nextIndex)
	{
		bool pathNextPoint = GetPathNextPoint(targetZ, bezierGridPoints, ref nextP);
		nextIndex = currentPathPointIndex;
		return pathNextPoint;
	}

	private bool GetPathNextPathToMoveTransform(float targetZ, PathToMoveTransform[] bezierGridPoints, ref Vector3 nextP, ref Quaternion nextR, ref Vector3 nextS)
	{
		for (int i = currentPathPointIndex; i < bezierGridPoints.Length; i++)
		{
			if (targetZ <= bezierGridPoints[i].m_position.z)
			{
				currentPathPointIndex = i;
				nextP = bezierGridPoints[currentPathPointIndex].m_position;
				nextR = bezierGridPoints[currentPathPointIndex].m_rotation;
				nextS = bezierGridPoints[currentPathPointIndex].m_size;
				return true;
			}
		}
		currentPathPointIndex = bezierGridPoints.Length - 1;
		nextP = bezierGridPoints[currentPathPointIndex].m_position;
		nextR = bezierGridPoints[currentPathPointIndex].m_rotation;
		nextS = bezierGridPoints[currentPathPointIndex].m_size;
		return false;
	}

	protected override void OnTriggerEnter(Collider collider)
	{
		if (!base.IfDie)
		{
			BaseElement gameComponent = collider.gameObject.GetGameComponent<BaseElement>();
			if (gameComponent is IBrushTrigger)
			{
				((IBrushTrigger)gameComponent).TriggerEnter(this, collider);
			}
			else if ((bool)gameComponent)
			{
				gameComponent.TriggerEnter(this);
			}
		}
	}

	protected override void OnTriggerExit(Collider collider)
	{
		if (!base.IfDie)
		{
			BaseTile gameComponent = collider.gameObject.GetGameComponent<BaseTile>();
			if ((bool)gameComponent)
			{
				gameComponent.TriggerExit(this);
			}
		}
	}

	public override void OnTileEnter(BaseTile tile)
	{
		if (!base.IfDie)
		{
			collideTileList.Insert(0, tile);
			LastTile = CurrentTile;
			CurrentTile = tile;
			currentGrid = CurrentTile.groupTransform;
		}
	}

	public override void OnTileExit(BaseTile tile)
	{
		bool ifDie = base.IfDie;
	}

	protected override void UpdateResetTileList()
	{
		for (int i = 0; i < collideTileList.Count; i++)
		{
			BaseTile baseTile = collideTileList[i];
			if ((bool)baseTile)
			{
				Vector3 tileCenter = baseTile.TileCenter;
				if (Mathf.Abs(tileCenter.x - base.transform.position.x) > baseTile.TileWidth || Mathf.Abs(tileCenter.z - base.transform.position.z) > baseTile.TileHeight)
				{
					collideTileList.RemoveAt(i);
					i--;
				}
			}
			else
			{
				collideTileList.RemoveAt(i);
				i--;
			}
		}
		if (collideTileList.Count > 0)
		{
			CurrentTile = collideTileList[0];
		}
		else
		{
			CurrentTile = null;
		}
	}

	public override void AddVehicle(BaseVehicle vehicle)
	{
		if (CurrentVehicle == null)
		{
			CurrentVehicle = vehicle;
		}
		else
		{
			Log.Error("Already have a vehicle:" + CurrentVehicle.name);
		}
	}

	public override void DepartVehicle(bool ifDestroy = false)
	{
		if ((bool)CurrentVehicle)
		{
			CurrentVehicle.DepartFromBall(this, ifDestroy);
			CurrentVehicle = null;
			RolePlayAnim(AnimType.RunState);
		}
	}

	public override void DepartFromPet()
	{
		Log.Error("Depart From Pet");
	}

	public override void GainTreasureChest(bool ifGain)
	{
		base.IfHaveTreasureChest = ifGain;
		if (ifGain)
		{
			ChangeRuntimeAnimatorControllerByIndex(1);
			if ((bool)treasureElement)
			{
				treasureElement.SetActive(true);
			}
		}
		else
		{
			ChangeRuntimeAnimatorControllerByIndex(0);
			RolePlayAnim(AnimType.Dizziness);
			base.IfDizziness = true;
			dizzinessCounter = DizzinessTime;
			if ((bool)treasureElement)
			{
				treasureElement.SetActive(false);
			}
			ResetMoodFaces(MoodFaceType.Angry_1);
		}
		if (thiefChestEffect.Length != 0)
		{
			thiefChestEffect[0].Play();
		}
	}

	public override void ForceShowTreasureChest(bool ifShow)
	{
		if ((bool)treasureElement)
		{
			treasureElement.SetActive(ifShow);
		}
	}

	public override void ForceShowCoverFace(bool ifShow)
	{
		if (!faceCoverTrans)
		{
			return;
		}
		faceCoverTrans.gameObject.SetActive(ifShow);
		if (!faceCoverAnim)
		{
			return;
		}
		faceCoverAnim.wrapMode = WrapMode.Loop;
		faceCoverAnim.Play(PlayMode.StopAll);
		if (ifShow)
		{
			return;
		}
		foreach (AnimationState item in faceCoverAnim)
		{
			item.normalizedTime = 0f;
		}
		faceCoverAnim.Sample();
		faceCoverAnim.Stop();
	}

	public override void CombineDancer(IDanceCombine dancer, bool ifTogether = false)
	{
		CurrentDancer = dancer;
		ifDanceTogether = ifTogether;
		if (ifTogether)
		{
			ballModelObj.transform.localRotation = Quaternion.identity;
			dancer.DancerTrans.parent = ballRoleObj.transform;
			dancer.OnCombineDancer(ballRoleObj.transform);
		}
	}

	public override void SeparateDancer(IDanceCombine dancer, bool ifClearRef = false)
	{
		if (CurrentDancer != null)
		{
			dancer.DancerTrans.parent = null;
			dancer.OnSeperateDancer(ballRoleObj.transform);
			if (ifClearRef)
			{
				CurrentDancer = null;
			}
		}
	}

	public override void DanceTogether(int danceType)
	{
		if (CurrentDancer != null)
		{
			CurrentDancer.OnDanceTogether(danceType);
			DanceByType(danceType);
		}
	}

	private void DanceByType(int danceType)
	{
		int num = 0;
		switch (danceType)
		{
		case 1:
		case 2:
		case 3:
		case 4:
			num = danceType - 1;
			RolePlayAnim((AnimType)(45 + num));
			break;
		case 5:
		case 6:
		case 7:
		case 8:
			num = danceType - 5;
			RolePlayAnim((AnimType)(41 + num));
			break;
		case 9:
			RolePlayAnim(AnimType.RunState);
			break;
		case 10:
			RolePlayAnim(AnimType.RunState);
			break;
		case 11:
			RolePlayAnim(AnimType.MirrorDance_4);
			break;
		case 12:
			RolePlayAnim(AnimType.MirrorDance_5);
			break;
		case 13:
			RolePlayAnim(AnimType.CombineDance_4);
			break;
		case 14:
			RolePlayAnim(AnimType.CombineDance_5);
			break;
		case 0:
			break;
		}
	}

	protected override float MoveX(float targetX)
	{
		float num = 0f;
		InputController instance = InputController.instance;
		if (!instance.IfEnableInput() || instance.IfPauseInput())
		{
			ResetTurning();
		}
		else
		{
			num = GetMoveXParam(targetX);
			SetTurningX(num * base.RealRotateSpeed, MaxRotateEuler);
			float deltaX = num;
			float num2 = 10.5f;
			bool flag = false;
			if (currentGrid != null && !flag)
			{
				Grid component = currentGrid.gameObject.GetComponent<Grid>();
				if (component != null)
				{
					num2 = (float)component.m_y * 0.5f;
					flag = true;
				}
			}
			if (CurrentTile != null && CurrentTile.CurrentGrid != null && !flag)
			{
				num2 = (float)CurrentTile.CurrentGrid.m_y * 0.5f;
				flag = true;
			}
			if (LastTile != null && LastTile.CurrentGrid != null && !flag)
			{
				num2 = (float)LastTile.CurrentGrid.m_y * 0.5f;
				flag = true;
			}
			float posDeltaX = currentGrid.InverseTransformPoint(base.transform.position).x - num2;
			InfluencedByRole(ref deltaX, posDeltaX);
			InfluencedByVehicle(ref deltaX, posDeltaX);
			InfluencedByPet(ref deltaX, posDeltaX);
			InfluencedByWind(true, ref deltaX);
			deltaX *= Time.smoothDeltaTime;
			SetPosition(base.transform.position + base.transform.right * deltaX);
		}
		return num;
	}

	protected override void FlyMove(Vector2 deltaPos)
	{
		Vector3 position = base.transform.position + base.transform.up * deltaPos.y + base.transform.right * deltaPos.x;
		base.transform.position = position;
	}

	protected override float GetMoveXParam(float targetX)
	{
		float num = 0f;
		float num2 = BallMoveOffset();
		float num3 = Mathf.Abs(targetX - num2);
		if (targetX > num2)
		{
			return num3 * SlideSpeed;
		}
		if (targetX < num2)
		{
			return num3 * (0f - SlideSpeed);
		}
		return 0f;
	}

	private void SetTurningX(float degree, float limitX)
	{
		degree = Mathf.Max(0f - MaxRotateEuler, Mathf.Min(MaxRotateEuler, degree));
		ResetTurning(degree);
	}

	protected override void ResetTurning(float horizonDegree = 0f)
	{
		Quaternion b = Quaternion.LookRotation(Quaternion.Euler(new Vector3(0f, horizonDegree, 0f)) * new Vector3(0f, 0f, 1f) + base.transform.position - base.transform.position);
		ballModelObj.transform.rotation = Quaternion.Slerp(ballModelObj.transform.rotation, b, Time.deltaTime * 10f);
	}

	protected override void ReleaseHandByWind()
	{
		float deltaX = 0f;
		InfluencedByWind(false, ref deltaX);
		deltaX *= Time.smoothDeltaTime;
		SetPosition(base.transform.position + base.transform.right * deltaX);
	}

	private void InfluencedByRole(ref float deltaX, float posDeltaX)
	{
		if ((deltaX > 0f && posDeltaX > base.RoleMoveLimit) || (deltaX < 0f && posDeltaX < 0f - base.RoleMoveLimit))
		{
			deltaX = 0f;
		}
	}

	private void InfluencedByVehicle(ref float deltaX, float posDeltaX)
	{
		if ((bool)currentGrid && base.IfOnVehicle && ((deltaX > 0f && posDeltaX > VehicleMoveLimit) || (deltaX < 0f && posDeltaX < 0f - VehicleMoveLimit)))
		{
			deltaX = 0f;
		}
	}

	private void InfluencedByPet(ref float deltaX, float posDeltaX)
	{
		if ((bool)currentGrid && currentPet != null && ((deltaX > 0f && posDeltaX > PetMoveLimit) || (deltaX < 0f && posDeltaX < 0f - PetMoveLimit)))
		{
			deltaX = 0f;
		}
	}

	private float InfluencedByWind(bool ifHaveInput, ref float deltaX)
	{
		if (ifHaveInput)
		{
			if (m_isHaveWind)
			{
				if (m_windData.m_isRightWind)
				{
					if (deltaX > 0f)
					{
						deltaX *= m_windData.m_windIntensity;
					}
					else if (deltaX < 0f)
					{
						deltaX /= m_windData.m_windResistance;
					}
				}
				else if (deltaX > 0f)
				{
					deltaX /= m_windData.m_windResistance;
				}
				else if (deltaX < 0f)
				{
					deltaX *= m_windData.m_windIntensity;
				}
			}
		}
		else if (m_isHaveWind)
		{
			deltaX = 1f;
			if (m_windData.m_isRightWind)
			{
				deltaX *= m_windData.m_stillWindIntensity;
			}
			else
			{
				deltaX *= m_windData.m_stillWindIntensity;
			}
		}
		return deltaX;
	}

	protected override void MoveForward()
	{
		SetPosition(base.transform.position + base.transform.forward * SpeedForward * Time.deltaTime);
	}

	protected void LerpToRailPath(float percent, float offset)
	{
		Transform transform = Railway.theRailway.transform;
		Vector3 position = base.transform.position;
		Vector3 b = transform.position + offset * transform.right;
		b.y = position.y;
		SetPosition(Vector3.Lerp(position, b, percent));
	}

	protected override void UpdateForceMove()
	{
		if (!InputController.instance.IfEnableInput() && forceHorizonData != null)
		{
			Vector3 targetPos = Vector3.zero;
			Vector3 position = base.transform.position;
			if (forceHorizonData.GetForceHorizonPos(position, ref targetPos))
			{
				position.x = targetPos.x;
				base.transform.position = position;
			}
			else
			{
				ClearForceHorizonData();
			}
		}
	}

	protected override void UpdateDizziness()
	{
		if (base.IfDizziness)
		{
			dizzinessCounter -= Time.deltaTime;
			if (dizzinessCounter <= 0f)
			{
				dizzinessCounter = 0f;
				base.IfDizziness = false;
			}
		}
	}

	protected override void BeginJump(Vector3 beginPos, Vector3 endPos, Vector3 normalDir, float heightY, JumpType jType, bool isShowTrail = true)
	{
		if (jumpUtilInfo == null)
		{
			jumpUtilInfo = new JumpUtil(beginPos, endPos, normalDir, heightY, base.transform.position.y);
		}
		else
		{
			jumpUtilInfo.SetJumpInfo(beginPos, endPos, normalDir, heightY, base.transform.position.y);
		}
		jumpType = jType;
		switch (jumpType)
		{
		case JumpType.Normal:
			isShowTrail = false;
			if (ifRebirthing)
			{
				RolePlayAnim(AnimType.SuperJumpState);
			}
			else
			{
				RolePlayAnim(AnimType.JumpState);
			}
			break;
		case JumpType.Super:
			RolePlayAnim(AnimType.SuperJumpState);
			break;
		case JumpType.QTE:
			RolePlayAnim(AnimType.SuperJumpState);
			break;
		case JumpType.QTEBetween:
			RolePlayAnim(AnimType.JumpQTEBetweenState);
			break;
		case JumpType.Ship:
			RolePlayAnim(AnimType.JumpOnBoatUpState);
			break;
		}
		if (isShowTrail)
		{
			OpenTrail();
		}
		if (!isShowTrail)
		{
			CloseTrail();
		}
	}

	protected override float Jumping()
	{
		float percent = 0f;
		float heightByPosZ = jumpUtilInfo.GetHeightByPosZ(base.transform.position, ref percent);
		SetPositionY(heightByPosZ);
		if (jumpType == JumpType.Super || jumpType == JumpType.Ship || jumpType == JumpType.QTE || jumpType == JumpType.QTEBetween)
		{
			RotateTrail();
		}
		return percent;
	}

	public override float GetJumpPercent()
	{
		if (jumpUtilInfo != null)
		{
			return jumpUtilInfo.GetPercentByPosZ(base.transform.position);
		}
		return 0f;
	}

	protected override void ResetPosY(float tileY)
	{
		SetPositionY(tileY + 0.05f);
	}

	protected override void BeginDrop(Vector3 beginPos, Vector3 endPos, Vector3 normalDir, float heightY)
	{
		dropBeginY = base.transform.position.y;
		dropBeginPos = beginPos;
		dropEndPos = endPos;
		dropDir = normalDir;
		dropHeightY = heightY;
		dropDistance = (beginPos - endPos).magnitude;
		dropDeltaY = 0f;
		RolePlayAnim(AnimType.JumpState);
	}

	protected override void Dropping()
	{
		float num = 0f - Vector3.Dot(dropBeginPos - BaseRole.BallPosition, dropDir);
		dropDeltaY = -1f * (num * num) * dropHeightY / (dropDistance * dropDistance);
		SetPositionY(dropBeginY + dropDeltaY);
	}

	private void InitTrail()
	{
		ballJumpTailEffect = base.transform.Find("effect/superJumpEffect").gameObject;
		ballJumpTailEffect.SetActive(false);
		ballJumpWingsEffect = ballJumpTailEffect.transform.Find("Wings").gameObject;
		ballJumpWingsEffect.transform.rotation = default(Quaternion);
		if (trailRune == null)
		{
			Transform transform = base.transform.Find("effect/superJumpEffect/" + NodeEffectRune);
			if ((bool)transform)
			{
				trailRune = transform.gameObject;
				trailRune.SetActive(false);
			}
		}
		if (trailElement0 == null)
		{
			trailElement0 = base.transform.Find("effect/superJumpEffect/arcaneFlash").GetComponent<TrailRenderer>();
			trailTime0 = trailElement0.time;
		}
		if (trailElement1 == null)
		{
			trailElement1 = base.transform.Find("effect/superJumpEffect/Wings/TRAIL1").GetComponent<TrailRenderer>();
			trailTime1 = trailElement1.time;
		}
		if (trailElement2 == null)
		{
			trailElement2 = base.transform.Find("effect/superJumpEffect/Wings/TRAIL2").GetComponent<TrailRenderer>();
			trailTime2 = trailElement2.time;
		}
		trailState = TrailState.None;
		currentInputType = InputType.Horizen;
	}

	private void ResetTrailParam()
	{
		if (trailElement0 != null)
		{
			trailElement0.time = trailTime0;
		}
		if (trailElement1 != null)
		{
			trailElement1.time = trailTime1;
		}
		if (trailElement2 != null)
		{
			trailElement2.time = trailTime2;
		}
	}

	private void OpenTrail()
	{
		if (trailState == TrailState.Closing)
		{
			ResetTrailParam();
		}
		ballJumpTailEffect.SetActive(true);
		ballJumpWingsEffect.transform.rotation = default(Quaternion);
		trailState = TrailState.Active;
	}

	public override void BeginCloseTrail()
	{
		trailState = TrailState.Closing;
	}

	private void CloseTrail()
	{
		if (trailState != 0)
		{
			ResetTrailParam();
			if (ballJumpTailEffect.activeSelf)
			{
				ballJumpTailEffect.SetActive(false);
				ballJumpWingsEffect.transform.rotation = default(Quaternion);
			}
			if ((bool)ballJumpWingsEffect)
			{
				ballJumpWingsEffect.transform.rotation = default(Quaternion);
			}
			trailState = TrailState.None;
		}
	}

	private void CloseTrailImmediately()
	{
		if (trailElement0 != null)
		{
			trailElement0.Clear();
		}
		if (trailElement1 != null)
		{
			trailElement1.Clear();
		}
		if (trailElement2 != null)
		{
			trailElement2.Clear();
		}
	}

	private void RotateTrail()
	{
		ballJumpWingsEffect.transform.Rotate(-Vector3.forward * 7f);
	}

	protected override void UpdateTrailEffect()
	{
		if (trailState != TrailState.Closing)
		{
			return;
		}
		if (trailElement0 != null)
		{
			float num = trailElement0.time - TrailFadeTime;
			trailElement0.time = num;
			if (num <= 0f)
			{
				CloseTrail();
			}
		}
		if (trailElement1 != null)
		{
			trailElement1.time -= TrailFadeTime;
		}
		if (trailElement2 != null)
		{
			trailElement2.time -= TrailFadeTime;
		}
	}

	private void WaitingBall()
	{
		_roleAnimator.SetBool("SquatToStandUp", false);
		RolePlayAnim(AnimType.SquatState);
	}

	protected virtual void InitBodyPart()
	{
		Dictionary<string, GameObject> dictionary = ViewTools.CollectAllGameObjects(base.gameObject);
		Transform root = dictionary[NodeRolePart].transform;
		Transform transform = dictionary[NodeBodyCenter].transform;
		Transform head = dictionary[NodeBodyHead].transform;
		Transform leftFoot = dictionary[NodeBodyLeftFoot].transform;
		Transform rightFoot = dictionary[NodeBodyRightFoot].transform;
		Transform leftShoulder = dictionary[NodeBodyLeftShoulder].transform;
		Transform rightShoulder = dictionary[NodeBodyRightShoulder].transform;
		Transform leftHand = dictionary[NodeBodyLeftHand].transform;
		Transform rightHand = dictionary[NodeBodyRightHand].transform;
		Transform waist = transform;
		BodyPart = new RoleBodyPart(root, transform, head, leftFoot, rightFoot, leftShoulder, rightShoulder, leftHand, rightHand, waist);
		roleLineElement = new RoleLineElment();
		GameObject value = null;
		if (dictionary.TryGetValue(NodeLinePart, out value))
		{
			roleLineElement.InitLine(value.transform);
		}
		else
		{
			roleLineElement.InitLine(null);
		}
	}

	private void InitMoodFaces()
	{
		Transform transform = base.transform.Find(NodeMoodFaces);
		if (transform != null)
		{
			moodFacePart = transform.gameObject;
			moodFacePart.SetActive(true);
			Transform transform2 = transform.Find(NodeModeHappy);
			if ((bool)transform2)
			{
				moodHappyNode = transform2.gameObject;
				moodHappyNode.SetActive(true);
				int childCount = transform2.childCount;
				moodHappyNodeList = new GameObject[childCount];
				for (int i = 0; i < childCount; i++)
				{
					string n = string.Format("{0}_{1}", NodeModeHappy, i + 1);
					moodHappyNodeList[i] = transform2.Find(n).gameObject;
					moodHappyNodeList[i].SetActive(false);
				}
			}
			Transform transform3 = transform.Find(NodeModeAngry);
			if ((bool)transform3)
			{
				moodAngryNode = transform3.gameObject;
				moodAngryNode.SetActive(false);
			}
		}
		currentMoodType = MoodFaceType.None;
	}

	private void InitFaceCover()
	{
		if ((bool)BodyPart.RoleHead)
		{
			faceCoverTrans = BodyPart.RoleHead.Find(NodeFaceCover);
			if ((bool)faceCoverTrans)
			{
				faceCoverAnim = faceCoverTrans.GetComponentInChildren<Animation>();
				ForceShowCoverFace(false);
			}
		}
	}

	private void ResetMoodFaces(MoodFaceType moodFaceType = MoodFaceType.None)
	{
		if (moodFaceType == MoodFaceType.Angry_1)
		{
			return;
		}
		currentMoodType = moodFaceType;
		if ((bool)moodHappyNode)
		{
			int num = -1;
			num = (int)(((uint)(moodFaceType - 1) > 3u) ? ((MoodFaceType)(-1)) : (moodFaceType - 1));
			for (int i = 0; i < moodHappyNodeList.Length; i++)
			{
				if (i == num)
				{
					moodHappyNodeList[i].SetActive(true);
				}
				else if (moodHappyNodeList[i].activeSelf)
				{
					moodHappyNodeList[i].SetActive(false);
				}
			}
		}
		if ((bool)moodAngryNode)
		{
			moodAngryNode.SetActive(moodFaceType == MoodFaceType.Angry_1);
		}
	}

	public override void MountEffect(Transform effect)
	{
		effect.parent = effectPart;
		effect.localPosition = Vector3.zero;
	}

	protected override void RoleRegisterListener()
	{
		Mod.Event.Subscribe(EventArgs<WindOpenEventArgs>.EventId, OnWindOpenEventHandler);
		Mod.Event.Subscribe(EventArgs<WindCloseEventArgs>.EventId, OnWindCloseEventHandler);
		Mod.Event.Subscribe(EventArgs<RoleChangeMoodEventArgs>.EventId, OnChangeMoodFace);
		Mod.Event.Subscribe(EventArgs<RoleDropDieMoveEventArgs>.EventId, OnRoleDropDieMove);
		Mod.Event.Subscribe(EventArgs<ChangeRoleTailEffectStateArgs>.EventId, OnChangeRoleTailEffect);
		Mod.Event.Subscribe(EventArgs<GameStartEventArgs>.EventId, OnGameStart);
		Mod.Event.Subscribe(EventArgs<SelectSkillFireEventArgs>.EventId, OnSelectSkillFinished);
	}

	protected override void RoleUnregisterListener()
	{
		Mod.Event.Unsubscribe(EventArgs<WindOpenEventArgs>.EventId, OnWindOpenEventHandler);
		Mod.Event.Unsubscribe(EventArgs<WindCloseEventArgs>.EventId, OnWindCloseEventHandler);
		Mod.Event.Unsubscribe(EventArgs<RoleChangeMoodEventArgs>.EventId, OnChangeMoodFace);
		Mod.Event.Unsubscribe(EventArgs<RoleDropDieMoveEventArgs>.EventId, OnRoleDropDieMove);
		Mod.Event.Unsubscribe(EventArgs<ChangeRoleTailEffectStateArgs>.EventId, OnChangeRoleTailEffect);
		Mod.Event.Unsubscribe(EventArgs<GameStartEventArgs>.EventId, OnGameStart);
		Mod.Event.Unsubscribe(EventArgs<SelectSkillFireEventArgs>.EventId, OnSelectSkillFinished);
	}

	private void OnGameStart(object sender, Foundation.EventArgs e)
	{
		GameStartEventArgs gameStartEventArgs = e as GameStartEventArgs;
		if (gameStartEventArgs != null && gameStartEventArgs.StartType == GameStartEventArgs.GameStartType.Normal && ifLerpEmission)
		{
			emissionCoroutine = StartCoroutine(StartChangeEmission());
		}
	}

	private IEnumerator StartChangeEmission()
	{
		yield return new WaitForSeconds(delayEmissionTime);
		do
		{
			currentEmissionTime += Time.deltaTime;
			if (emissionRender != null && emissionRender.Length != 0)
			{
				emissionMaterial = new Material[emissionRender.Length];
				float t = currentEmissionTime / lerpEmissionTime;
				float value = Mathf.Lerp(beginEmissionValue, endEmissionValue, t);
				for (int i = 0; i < emissionRender.Length; i++)
				{
					emissionMaterial[i] = emissionRender[i].material;
					emissionMaterial[i].SetFloat("_Emmission", value);
					emissionRender[i].material = emissionMaterial[i];
				}
			}
			yield return null;
		}
		while (currentEmissionTime <= lerpEmissionTime);
	}

	private void OnSelectSkillFinished(object sender, Foundation.EventArgs e)
	{
		ShowRoleRender(true);
	}

	private void OnWindOpenEventHandler(object sender, Foundation.EventArgs e)
	{
		WindOpenEventArgs windOpenEventArgs = e as WindOpenEventArgs;
		m_isHaveWind = true;
		m_windData = windOpenEventArgs.m_windData;
		m_dynamicBoneFactory.SwitchDynamicBoneType(m_windData.m_isRightWind ? DynamicBoneFactory.DynamicBoneType.OpenRightWind : DynamicBoneFactory.DynamicBoneType.OpenLeftWind);
		Log.Info("Maggic OnWindOpenEventHandler---------------------------------->");
	}

	private void OnWindCloseEventHandler(object sender, Foundation.EventArgs e)
	{
		m_isHaveWind = false;
		m_dynamicBoneFactory.SwitchDynamicBoneType(DynamicBoneFactory.DynamicBoneType.CloseWind);
		Log.Info("Maggic OnWindCloseEventHandler---------------------------------->");
	}

	private void OnChangeMoodFace(object sender, Foundation.EventArgs e)
	{
		RoleChangeMoodEventArgs roleChangeMoodEventArgs = e as RoleChangeMoodEventArgs;
		if (roleChangeMoodEventArgs != null)
		{
			ResetMoodFaces(roleChangeMoodEventArgs.MoodFaceType);
		}
	}

	private void OnRoleDropDieMove(object sender, Foundation.EventArgs e)
	{
		RoleDropDieMoveEventArgs roleDropDieMoveEventArgs = e as RoleDropDieMoveEventArgs;
		if (roleDropDieMoveEventArgs != null)
		{
			ifDropDieStatic = roleDropDieMoveEventArgs.IfDropDieStatic;
		}
	}

	private void OnChangeRoleTailEffect(object sender, Foundation.EventArgs e)
	{
		ChangeRoleTailEffectStateArgs changeRoleTailEffectStateArgs = e as ChangeRoleTailEffectStateArgs;
		if (changeRoleTailEffectStateArgs != null)
		{
			if (changeRoleTailEffectStateArgs.TailEffectState == TailEffectState.IMMEDIATELY_CLOSE)
			{
				CloseTrailImmediately();
			}
			else if (changeRoleTailEffectStateArgs.TailEffectState == TailEffectState.RESTART)
			{
				ResetTrailParam();
			}
		}
	}

	protected override void OnDestroyLocal()
	{
		roleLineElement.DestroyLine();
	}

	protected override void UpdateDebugMove()
	{
	}

	[Obsolete("this is Obsolete,please  please use GetOriginRebirthBsonData !")]
	public override object GetOriginRebirthData(object obj = null)
	{
		RD_RolePrince_DATA rD_RolePrince_DATA = new RD_RolePrince_DATA();
		rD_RolePrince_DATA.IfWinBeforeFinish = base.IfWinBeforeFinish;
		rD_RolePrince_DATA.IfRebirthOnVehicle = base.IfRebirthOnVehicle;
		rD_RolePrince_DATA.RoleMoveLimit = base.RoleMoveLimit;
		rD_RolePrince_DATA.ifDropDieStatic = ifDropDieStatic;
		rD_RolePrince_DATA.CurrentState = CurrentState;
		rD_RolePrince_DATA.roleRootTransData = base.transform.GetTransData();
		rD_RolePrince_DATA.roleModelTransData = ballModelObj.transform.GetTransData();
		rD_RolePrince_DATA.roleRoleTransData = ballRoleObj.transform.GetTransData();
		rD_RolePrince_DATA.m_runtimeAnimatorControllerIndex = m_runtimeAnimatorControllerIndex;
		rD_RolePrince_DATA.m_roleSkinIndex = m_roleSkinIndex;
		rD_RolePrince_DATA.m_currentAnimType = m_currentAnimType;
		string animName = string.Empty;
		float animTime = 0f;
		if (TryGetCurrentAnimInfo(_roleAnimator, ref animName, ref animTime))
		{
			RD_ElementAnim_DATA rD_ElementAnim_DATA = new RD_ElementAnim_DATA();
			rD_ElementAnim_DATA.animaName = animName;
			rD_ElementAnim_DATA.animTime = animTime;
			rD_ElementAnim_DATA.ifPlaying = true;
			rD_RolePrince_DATA.roleAnimData = rD_ElementAnim_DATA;
		}
		rD_RolePrince_DATA.jumpInfo = jumpUtilInfo.GetJumpInfo();
		rD_RolePrince_DATA.jumpType = jumpType;
		rD_RolePrince_DATA.ifDroping = CurrentState == BallState.NormalDrop;
		if (rD_RolePrince_DATA.ifDroping)
		{
			rD_RolePrince_DATA.dropBeginPos = dropBeginPos;
			rD_RolePrince_DATA.dropEndPos = dropEndPos;
			rD_RolePrince_DATA.dropDir = dropDir;
			rD_RolePrince_DATA.dropDistance = dropDistance;
			rD_RolePrince_DATA.dropBeginY = dropBeginY;
			rD_RolePrince_DATA.dropHeightY = dropHeightY;
			rD_RolePrince_DATA.dropDeltaY = dropDeltaY;
		}
		if (CurrentState == BallState.PathToMove)
		{
			rD_RolePrince_DATA.ifHavePathTrigger = PathTriggerIndex > 0;
			rD_RolePrince_DATA.pathTriggerIndex = PathTriggerIndex;
			rD_RolePrince_DATA.pathPointIndex = currentPathPointIndex;
		}
		else
		{
			rD_RolePrince_DATA.ifHavePathTrigger = false;
			rD_RolePrince_DATA.pathTriggerIndex = -1;
			rD_RolePrince_DATA.pathPointIndex = 0;
		}
		if (CurrentState == BallState.PathToMoveFixed)
		{
			rD_RolePrince_DATA.ifHavePathFixedTrigger = PathFixTriggerIndex > 0;
			rD_RolePrince_DATA.pathFixedTriggerIndex = PathFixTriggerIndex;
			rD_RolePrince_DATA.pathFixedPointIndex = currentPathPointIndex;
		}
		else
		{
			rD_RolePrince_DATA.ifHavePathFixedTrigger = false;
			rD_RolePrince_DATA.pathFixedTriggerIndex = -1;
			rD_RolePrince_DATA.pathFixedPointIndex = 0;
		}
		if (CurrentState == BallState.PathToMoveLerp)
		{
			rD_RolePrince_DATA.ifHavePathLerpTrigger = PathLerpTriggerIndex > 0;
			rD_RolePrince_DATA.pathLerpTriggerIndex = PathLerpTriggerIndex;
			rD_RolePrince_DATA.pathLerpPointIndex = currentPathPointIndex;
			rD_RolePrince_DATA.rolePosForLerp = m_rolePostionFoPathtoMoveLerp;
		}
		else
		{
			rD_RolePrince_DATA.ifHavePathLerpTrigger = false;
			rD_RolePrince_DATA.pathLerpTriggerIndex = -1;
			rD_RolePrince_DATA.pathLerpPointIndex = 0;
			rD_RolePrince_DATA.rolePosForLerp = Vector3.zero;
		}
		rD_RolePrince_DATA.LastTileId = -1;
		rD_RolePrince_DATA.CurrentTileId = -1;
		rD_RolePrince_DATA.currentGrid = -1;
		if (LastTile != null)
		{
			rD_RolePrince_DATA.LastTileId = LastTile.m_uuId;
		}
		if (CurrentTile != null)
		{
			rD_RolePrince_DATA.CurrentTileId = CurrentTile.m_uuId;
		}
		if (currentGrid != null)
		{
			rD_RolePrince_DATA.currentGrid = currentGrid.GetComponent<Grid>().m_id;
		}
		rD_RolePrince_DATA.IfHaveTreasureChest = base.IfHaveTreasureChest;
		rD_RolePrince_DATA.treasureElementShow = treasureElement != null && treasureElement.activeSelf;
		rD_RolePrince_DATA.IfShowfaceCover = faceCoverTrans != null && faceCoverTrans.gameObject.activeSelf;
		rD_RolePrince_DATA.faceCoverAnimData = faceCoverAnim.GetAnimData();
		rD_RolePrince_DATA.currentMoodType = (int)currentMoodType;
		BaseElement baseElement = CurrentDancer as BaseElement;
		if (baseElement == null)
		{
			rD_RolePrince_DATA.dancerUuid = -1;
		}
		else
		{
			rD_RolePrince_DATA.dancerUuid = baseElement.m_uuId;
		}
		if (m_swingingRipeData != null)
		{
			rD_RolePrince_DATA.m_swingingRipeData = m_swingingRipeData;
		}
		rD_RolePrince_DATA.m_isHaveWind = m_isHaveWind;
		if (m_isHaveWind)
		{
			rD_RolePrince_DATA.m_windData = m_windData;
		}
		rD_RolePrince_DATA.CurrentVehicle = new RD_Vehicle_DATA();
		rD_RolePrince_DATA.CurrentVehicle.ifOnVehicle = base.IfOnVehicle;
		rD_RolePrince_DATA.CurrentVehicle.vehicleId = (base.IfOnVehicle ? CurrentVehicle.m_uuId : (-1));
		rD_RolePrince_DATA.SlideSpeed = SlideSpeed;
		rD_RolePrince_DATA.CurrentWorldRow = CurrentWorldRow;
		rD_RolePrince_DATA.LastRow = LastRow;
		return JsonUtility.ToJson(rD_RolePrince_DATA);
	}

	[Obsolete("this is Obsolete,please  please use SetOriginRebirthBsonData !")]
	public override void SetOriginRebirthData(object dataInfo)
	{
		ifRebirthing = true;
		debugMoveIndex = 0;
		dataVal = JsonUtility.FromJson<RD_RolePrince_DATA>(dataInfo as string);
		base.IfWinBeforeFinish = dataVal.IfWinBeforeFinish;
		base.IfRebirthOnVehicle = dataVal.IfRebirthOnVehicle;
		base.RoleMoveLimit = dataVal.RoleMoveLimit;
		ifDropDieStatic = dataVal.ifDropDieStatic;
		CurrentState = dataVal.CurrentState;
		ChangeStateTo(CurrentState);
		base.transform.SetTransData(dataVal.roleRootTransData);
		ballModelObj.transform.SetTransData(dataVal.roleModelTransData);
		ballRoleObj.transform.SetTransData(dataVal.roleRoleTransData);
		m_runtimeAnimatorControllerIndex = dataVal.m_runtimeAnimatorControllerIndex;
		_roleAnimator.runtimeAnimatorController = m_roleProperty.m_runtimeAnimatorControllers[m_runtimeAnimatorControllerIndex];
		m_roleSkinIndex = dataVal.m_roleSkinIndex;
		if (m_roleSkinController != null)
		{
			m_roleSkinController.SetSkinByIndex(m_roleSkinIndex);
		}
		ElementRebirthUtil.SetJumpInfo(ref jumpUtilInfo, dataVal.jumpInfo);
		jumpType = dataVal.jumpType;
		if (dataVal.ifDroping)
		{
			dropBeginPos = dataVal.dropBeginPos;
			dropEndPos = dataVal.dropEndPos;
			dropDir = dataVal.dropDir;
			dropDistance = dataVal.dropDistance;
			dropBeginY = dataVal.dropBeginY;
			dropHeightY = dataVal.dropHeightY;
			dropDeltaY = dataVal.dropDeltaY;
		}
		if (dataVal.ifHavePathTrigger)
		{
			int pathTriggerIndex = dataVal.pathTriggerIndex;
			if (pathTriggerIndex > 0)
			{
				BaseElement element;
				MapController.Instance.TryGetElementByUUID<BaseElement>(pathTriggerIndex, out element);
				if (element != null)
				{
					currentGrid = element.groupTransform;
				}
				IPathRebirth pathRebirth = element as IPathRebirth;
				if (pathRebirth != null)
				{
					ChangeToPathToMove(pathRebirth.GetPathData(), dataVal.pathPointIndex);
				}
			}
		}
		if (dataVal.ifHavePathLerpTrigger)
		{
			int pathLerpTriggerIndex = dataVal.pathLerpTriggerIndex;
			if (pathLerpTriggerIndex > 0)
			{
				BaseElement element2;
				MapController.Instance.TryGetElementByUUID<BaseElement>(pathLerpTriggerIndex, out element2);
				if (element2 != null)
				{
					currentGrid = element2.groupTransform;
				}
				IPathLerpRebirth pathLerpRebirth = element2 as IPathLerpRebirth;
				if (pathLerpRebirth != null)
				{
					ChangeToPathToMoveLerp(pathLerpRebirth.GetPathLerpData(), dataVal.pathLerpPointIndex);
					m_rolePostionFoPathtoMoveLerp = dataVal.rolePosForLerp;
				}
			}
		}
		if (dataVal.ifHavePathFixedTrigger)
		{
			int pathFixedTriggerIndex = dataVal.pathFixedTriggerIndex;
			if (pathFixedTriggerIndex > 0)
			{
				BaseElement element3;
				MapController.Instance.TryGetElementByUUID<BaseElement>(pathFixedTriggerIndex, out element3);
				if (element3 != null)
				{
					currentGrid = element3.groupTransform;
				}
				IPathFixRebirth pathFixRebirth = element3 as IPathFixRebirth;
				if (pathFixRebirth != null)
				{
					ChangeToPathToMoveFixed(pathFixRebirth.GetPathData(), dataVal.pathFixedPointIndex);
				}
			}
		}
		if (dataVal.LastTileId > 0)
		{
			BaseElement element4;
			MapController.Instance.TryGetElementByUUID<BaseElement>(dataVal.LastTileId, out element4);
			if ((bool)element4)
			{
				LastTile = element4 as BaseTile;
			}
		}
		if (dataVal.CurrentTileId > 0)
		{
			BaseElement element5;
			MapController.Instance.TryGetElementByUUID<BaseElement>(dataVal.CurrentTileId, out element5);
			if ((bool)element5)
			{
				CurrentTile = element5 as BaseTile;
				currentGrid = CurrentTile.groupTransform;
				collideTileList.Insert(0, CurrentTile);
			}
		}
		if (currentGrid == null && dataVal.currentGrid >= 0)
		{
			currentGrid = MapController.Instance.GetGridById(dataVal.currentGrid).transform;
		}
		if (currentGrid == null && dataVal.currentGrid >= 0)
		{
			Grid currentRoleGrid = MapController.Instance.CurrentRoleGrid;
			if (currentRoleGrid != null)
			{
				currentGrid = currentRoleGrid.transform;
			}
		}
		base.IfHaveTreasureChest = dataVal.IfHaveTreasureChest;
		if ((bool)treasureElement)
		{
			treasureElement.SetActive(dataVal.treasureElementShow);
			treasureElement.SetActive(false);
		}
		if (faceCoverTrans != null)
		{
			faceCoverTrans.gameObject.SetActive(dataVal.IfShowfaceCover);
			faceCoverAnim.SetAnimData(dataVal.faceCoverAnimData, ProcessState.Pause);
			faceCoverTrans.gameObject.SetActive(false);
		}
		currentMoodType = (MoodFaceType)dataVal.currentMoodType;
		ResetMoodFaces(currentMoodType);
		int dancerUuid = dataVal.dancerUuid;
		BaseElement element6;
		if (dancerUuid > 0 && MapController.Instance.TryGetElementByUUID<BaseElement>(dancerUuid, out element6))
		{
			CurrentDancer = element6 as IDanceCombine;
			ifDanceTogether = dataVal.ifDanceTogether;
			CombineDancer(CurrentDancer, ifDanceTogether);
		}
		m_swingingRipeData = dataVal.m_swingingRipeData;
		m_isHaveWind = dataVal.m_isHaveWind;
		if (m_isHaveWind)
		{
			m_windData = dataVal.m_windData;
			Mod.Event.FireNow(this, Mod.Reference.Acquire<WindOpenEventArgs>().Initialize(m_windData));
		}
		if (dataVal.CurrentVehicle.ifOnVehicle)
		{
			BaseVehicle element7;
			MapController.Instance.TryGetElementByUUID<BaseVehicle>(dataVal.CurrentVehicle.vehicleId, out element7);
			if ((bool)element7)
			{
				Vector3 localScale = ballRoleObj.transform.localScale;
				element7.RebirthResetByRole(this);
				ballRoleObj.transform.localScale = localScale;
			}
			else
			{
				Log.Error("Cannot find the vehicle " + dataVal.CurrentVehicle.vehicleId);
			}
		}
		SlideSpeed = dataVal.SlideSpeed;
		CurrentWorldRow = dataVal.CurrentWorldRow;
		LastRow = dataVal.LastRow;
		m_currentAnimType = dataVal.m_currentAnimType;
		ifRebirthWaiting = false;
		if (base.IfOnVehicle)
		{
			RD_ElementAnim_DATA roleAnimData = dataVal.roleAnimData;
			if (roleAnimData.animaName == "RideYangTuo")
			{
				_roleAnimator.Play(AnimType.RideYangTuo_Wait.ToString(), 0, 0f);
				_roleAnimator.enabled = false;
			}
			else
			{
				_roleAnimator.Play(roleAnimData.animaName, 0, roleAnimData.animTime);
			}
		}
		else if (base.IfJumping)
		{
			RolePlayAnim(AnimType.SuperJumpState, 0, 0f, true);
		}
		else if (base.IfDropping)
		{
			RolePlayAnim(AnimType.SuperJumpState, 0, 0f, true);
		}
		else if (CurrentState == BallState.SwingingRope)
		{
			RD_ElementAnim_DATA roleAnimData2 = dataVal.roleAnimData;
			_roleAnimator.Play(roleAnimData2.animaName, 0, roleAnimData2.animTime);
		}
		else if (CurrentState == BallState.PathToMove)
		{
			m_currentAnimType = dataVal.m_currentAnimType;
			RD_ElementAnim_DATA roleAnimData3 = dataVal.roleAnimData;
			if (m_currentAnimType == AnimType.WaitingState || m_currentAnimType == AnimType.RunState || m_currentAnimType == AnimType.StandUpToRun || m_currentAnimType == AnimType.SuperJumpToRoll)
			{
				RolePlayAnim(AnimType.WaitingState, 0, 0f, true);
			}
			else
			{
				_roleAnimator.Play(roleAnimData3.animaName, 0, roleAnimData3.animTime);
			}
		}
		else
		{
			ifRebirthWaiting = dataVal.m_currentAnimType == AnimType.WaitingState;
			RolePlayAnim(AnimType.WaitingState, 0, 0f, true);
		}
		StartCoroutine(WaitSwitchDynamicBoneType(DynamicBoneFactory.DynamicBoneType.RebirthResetBall));
	}

	[Obsolete("this is Obsolete,please  please use StartRunByOriginRebirthBsonData !")]
	public override void StartRunByOriginRebirthData(object dataInfo)
	{
		ifRebirthing = false;
		if (!base.IfOnVehicle && !base.IfJumping && !base.IfDropping && CurrentState != BallState.SwingingRope)
		{
			if (CurrentState == BallState.PathToMove)
			{
				if (m_currentAnimType == AnimType.WaitingState)
				{
					RolePlayAnim(AnimType.RunState, 0, 0f, true);
				}
			}
			else if (ifRebirthWaiting)
			{
				RolePlayAnim(AnimType.WaitingState, 0, 0f, true);
			}
			else
			{
				RolePlayAnim(AnimType.RunState, 0, 0f, true);
			}
		}
		bool flag = (bool)faceCoverAnim;
		base.IfHaveTreasureChest = dataVal.IfHaveTreasureChest;
		if ((bool)treasureElement)
		{
			treasureElement.SetActive(dataVal.treasureElementShow);
		}
		if (faceCoverTrans != null)
		{
			faceCoverTrans.gameObject.SetActive(dataVal.IfShowfaceCover);
		}
		if (m_isHaveWind)
		{
			m_dynamicBoneFactory.SwitchDynamicBoneType(m_windData.m_isRightWind ? DynamicBoneFactory.DynamicBoneType.OpenRightWind : DynamicBoneFactory.DynamicBoneType.OpenLeftWind);
		}
		else
		{
			m_dynamicBoneFactory.SwitchDynamicBoneType(DynamicBoneFactory.DynamicBoneType.StartBall);
		}
	}

	public override byte[] GetOriginRebirthBsonData(object obj = null)
	{
		RD_RolePrince_DATA rD_RolePrince_DATA = new RD_RolePrince_DATA();
		rD_RolePrince_DATA.IfWinBeforeFinish = base.IfWinBeforeFinish;
		rD_RolePrince_DATA.IfRebirthOnVehicle = base.IfRebirthOnVehicle;
		rD_RolePrince_DATA.RoleMoveLimit = base.RoleMoveLimit;
		rD_RolePrince_DATA.ifDropDieStatic = ifDropDieStatic;
		rD_RolePrince_DATA.CurrentState = CurrentState;
		rD_RolePrince_DATA.roleRootTransData = base.transform.GetTransData();
		rD_RolePrince_DATA.roleModelTransData = ballModelObj.transform.GetTransData();
		rD_RolePrince_DATA.roleRoleTransData = ballRoleObj.transform.GetTransData();
		rD_RolePrince_DATA.m_runtimeAnimatorControllerIndex = m_runtimeAnimatorControllerIndex;
		rD_RolePrince_DATA.m_roleSkinIndex = m_roleSkinIndex;
		rD_RolePrince_DATA.m_currentAnimType = m_currentAnimType;
		string animName = string.Empty;
		float animTime = 0f;
		if (TryGetCurrentAnimInfo(_roleAnimator, ref animName, ref animTime))
		{
			RD_ElementAnim_DATA rD_ElementAnim_DATA = new RD_ElementAnim_DATA();
			rD_ElementAnim_DATA.animaName = animName;
			rD_ElementAnim_DATA.animTime = animTime;
			rD_ElementAnim_DATA.ifPlaying = true;
			rD_RolePrince_DATA.roleAnimData = rD_ElementAnim_DATA;
		}
		rD_RolePrince_DATA.jumpInfo = jumpUtilInfo.GetJumpInfo();
		rD_RolePrince_DATA.jumpType = jumpType;
		rD_RolePrince_DATA.ifDroping = CurrentState == BallState.NormalDrop;
		if (rD_RolePrince_DATA.ifDroping)
		{
			rD_RolePrince_DATA.dropBeginPos = dropBeginPos;
			rD_RolePrince_DATA.dropEndPos = dropEndPos;
			rD_RolePrince_DATA.dropDir = dropDir;
			rD_RolePrince_DATA.dropDistance = dropDistance;
			rD_RolePrince_DATA.dropBeginY = dropBeginY;
			rD_RolePrince_DATA.dropHeightY = dropHeightY;
			rD_RolePrince_DATA.dropDeltaY = dropDeltaY;
		}
		if (CurrentState == BallState.PathToMove)
		{
			rD_RolePrince_DATA.ifHavePathTrigger = PathTriggerIndex > 0;
			rD_RolePrince_DATA.pathTriggerIndex = PathTriggerIndex;
			rD_RolePrince_DATA.pathPointIndex = currentPathPointIndex;
		}
		else
		{
			rD_RolePrince_DATA.ifHavePathTrigger = false;
			rD_RolePrince_DATA.pathTriggerIndex = -1;
			rD_RolePrince_DATA.pathPointIndex = 0;
		}
		if (CurrentState == BallState.PathToMoveFixed)
		{
			rD_RolePrince_DATA.ifHavePathFixedTrigger = PathFixTriggerIndex > 0;
			rD_RolePrince_DATA.pathFixedTriggerIndex = PathFixTriggerIndex;
			rD_RolePrince_DATA.pathFixedPointIndex = currentPathPointIndex;
		}
		else
		{
			rD_RolePrince_DATA.ifHavePathFixedTrigger = false;
			rD_RolePrince_DATA.pathFixedTriggerIndex = -1;
			rD_RolePrince_DATA.pathFixedPointIndex = 0;
		}
		if (CurrentState == BallState.PathToMoveLerp)
		{
			rD_RolePrince_DATA.ifHavePathLerpTrigger = PathLerpTriggerIndex > 0;
			rD_RolePrince_DATA.pathLerpTriggerIndex = PathLerpTriggerIndex;
			rD_RolePrince_DATA.pathLerpPointIndex = currentPathPointIndex;
			rD_RolePrince_DATA.rolePosForLerp = m_rolePostionFoPathtoMoveLerp;
		}
		else
		{
			rD_RolePrince_DATA.ifHavePathLerpTrigger = false;
			rD_RolePrince_DATA.pathLerpTriggerIndex = -1;
			rD_RolePrince_DATA.pathLerpPointIndex = 0;
			rD_RolePrince_DATA.rolePosForLerp = Vector3.zero;
		}
		rD_RolePrince_DATA.LastTileId = -1;
		rD_RolePrince_DATA.CurrentTileId = -1;
		rD_RolePrince_DATA.currentGrid = -1;
		if (LastTile != null)
		{
			rD_RolePrince_DATA.LastTileId = LastTile.m_uuId;
		}
		if (CurrentTile != null)
		{
			rD_RolePrince_DATA.CurrentTileId = CurrentTile.m_uuId;
		}
		if (currentGrid != null)
		{
			rD_RolePrince_DATA.currentGrid = currentGrid.GetComponent<Grid>().m_id;
		}
		rD_RolePrince_DATA.IfHaveTreasureChest = base.IfHaveTreasureChest;
		rD_RolePrince_DATA.treasureElementShow = treasureElement != null && treasureElement.activeSelf;
		rD_RolePrince_DATA.IfShowfaceCover = faceCoverTrans != null && faceCoverTrans.gameObject.activeSelf;
		rD_RolePrince_DATA.faceCoverAnimData = faceCoverAnim.GetAnimData();
		rD_RolePrince_DATA.currentMoodType = (int)currentMoodType;
		BaseElement baseElement = CurrentDancer as BaseElement;
		if (baseElement == null)
		{
			rD_RolePrince_DATA.dancerUuid = -1;
		}
		else
		{
			rD_RolePrince_DATA.dancerUuid = baseElement.m_uuId;
		}
		if (m_swingingRipeData != null)
		{
			rD_RolePrince_DATA.m_swingingRipeData = m_swingingRipeData;
		}
		rD_RolePrince_DATA.m_isHaveWind = m_isHaveWind;
		if (m_isHaveWind)
		{
			rD_RolePrince_DATA.m_windData = m_windData;
		}
		rD_RolePrince_DATA.CurrentVehicle = new RD_Vehicle_DATA();
		rD_RolePrince_DATA.CurrentVehicle.ifOnVehicle = base.IfOnVehicle;
		rD_RolePrince_DATA.CurrentVehicle.vehicleId = (base.IfOnVehicle ? CurrentVehicle.m_uuId : (-1));
		rD_RolePrince_DATA.SlideSpeed = SlideSpeed;
		rD_RolePrince_DATA.CurrentWorldRow = CurrentWorldRow;
		rD_RolePrince_DATA.LastRow = LastRow;
		return Bson.ToBson(rD_RolePrince_DATA);
	}

	public override void SetOriginRebirthBsonData(byte[] dataInfo)
	{
		ifRebirthing = true;
		debugMoveIndex = 0;
		dataVal = Bson.ToObject<RD_RolePrince_DATA>(dataInfo);
		base.IfWinBeforeFinish = dataVal.IfWinBeforeFinish;
		base.IfRebirthOnVehicle = dataVal.IfRebirthOnVehicle;
		base.RoleMoveLimit = dataVal.RoleMoveLimit;
		ifDropDieStatic = dataVal.ifDropDieStatic;
		CurrentState = dataVal.CurrentState;
		ChangeStateTo(CurrentState);
		base.transform.SetTransData(dataVal.roleRootTransData);
		ballModelObj.transform.SetTransData(dataVal.roleModelTransData);
		ballRoleObj.transform.SetTransData(dataVal.roleRoleTransData);
		m_runtimeAnimatorControllerIndex = dataVal.m_runtimeAnimatorControllerIndex;
		_roleAnimator.runtimeAnimatorController = m_roleProperty.m_runtimeAnimatorControllers[m_runtimeAnimatorControllerIndex];
		m_roleSkinIndex = dataVal.m_roleSkinIndex;
		if (m_roleSkinController != null)
		{
			m_roleSkinController.SetSkinByIndex(m_roleSkinIndex);
		}
		ElementRebirthUtil.SetJumpInfo(ref jumpUtilInfo, dataVal.jumpInfo);
		jumpType = dataVal.jumpType;
		if (dataVal.ifDroping)
		{
			dropBeginPos = dataVal.dropBeginPos;
			dropEndPos = dataVal.dropEndPos;
			dropDir = dataVal.dropDir;
			dropDistance = dataVal.dropDistance;
			dropBeginY = dataVal.dropBeginY;
			dropHeightY = dataVal.dropHeightY;
			dropDeltaY = dataVal.dropDeltaY;
		}
		if (dataVal.ifHavePathTrigger)
		{
			int pathTriggerIndex = dataVal.pathTriggerIndex;
			if (pathTriggerIndex > 0)
			{
				BaseElement element;
				MapController.Instance.TryGetElementByUUID<BaseElement>(pathTriggerIndex, out element);
				if (element != null)
				{
					currentGrid = element.groupTransform;
				}
				IPathRebirth pathRebirth = element as IPathRebirth;
				if (pathRebirth != null)
				{
					ChangeToPathToMove(pathRebirth.GetPathData(), dataVal.pathPointIndex);
				}
			}
		}
		if (dataVal.ifHavePathFixedTrigger)
		{
			int pathFixedTriggerIndex = dataVal.pathFixedTriggerIndex;
			if (pathFixedTriggerIndex > 0)
			{
				BaseElement element2;
				MapController.Instance.TryGetElementByUUID<BaseElement>(pathFixedTriggerIndex, out element2);
				if (element2 != null)
				{
					currentGrid = element2.groupTransform;
				}
				IPathFixRebirth pathFixRebirth = element2 as IPathFixRebirth;
				if (pathFixRebirth != null)
				{
					ChangeToPathToMoveFixed(pathFixRebirth.GetPathData(), dataVal.pathFixedPointIndex);
				}
			}
		}
		if (dataVal.ifHavePathLerpTrigger)
		{
			int pathLerpTriggerIndex = dataVal.pathLerpTriggerIndex;
			if (pathLerpTriggerIndex > 0)
			{
				BaseElement element3;
				MapController.Instance.TryGetElementByUUID<BaseElement>(pathLerpTriggerIndex, out element3);
				if (element3 != null)
				{
					currentGrid = element3.groupTransform;
				}
				IPathLerpRebirth pathLerpRebirth = element3 as IPathLerpRebirth;
				if (pathLerpRebirth != null)
				{
					ChangeToPathToMoveLerp(pathLerpRebirth.GetPathLerpData(), dataVal.pathLerpPointIndex);
					m_rolePostionFoPathtoMoveLerp = dataVal.rolePosForLerp;
				}
			}
		}
		if (dataVal.LastTileId > 0)
		{
			BaseElement element4;
			MapController.Instance.TryGetElementByUUID<BaseElement>(dataVal.LastTileId, out element4);
			if ((bool)element4)
			{
				LastTile = element4 as BaseTile;
			}
		}
		if (dataVal.CurrentTileId > 0)
		{
			BaseElement element5;
			MapController.Instance.TryGetElementByUUID<BaseElement>(dataVal.CurrentTileId, out element5);
			if ((bool)element5)
			{
				CurrentTile = element5 as BaseTile;
				currentGrid = CurrentTile.groupTransform;
				collideTileList.Insert(0, CurrentTile);
			}
		}
		if (currentGrid == null && dataVal.currentGrid >= 0)
		{
			currentGrid = MapController.Instance.GetGridById(dataVal.currentGrid).transform;
		}
		if (currentGrid == null && dataVal.currentGrid >= 0)
		{
			Grid currentRoleGrid = MapController.Instance.CurrentRoleGrid;
			if (currentRoleGrid != null)
			{
				currentGrid = currentRoleGrid.transform;
			}
		}
		base.IfHaveTreasureChest = dataVal.IfHaveTreasureChest;
		if ((bool)treasureElement)
		{
			treasureElement.SetActive(dataVal.treasureElementShow);
			treasureElement.SetActive(false);
		}
		if (faceCoverTrans != null)
		{
			faceCoverTrans.gameObject.SetActive(dataVal.IfShowfaceCover);
			faceCoverAnim.SetAnimData(dataVal.faceCoverAnimData, ProcessState.Pause);
			faceCoverTrans.gameObject.SetActive(false);
		}
		currentMoodType = (MoodFaceType)dataVal.currentMoodType;
		ResetMoodFaces(currentMoodType);
		int dancerUuid = dataVal.dancerUuid;
		BaseElement element6;
		if (dancerUuid > 0 && MapController.Instance.TryGetElementByUUID<BaseElement>(dancerUuid, out element6))
		{
			CurrentDancer = element6 as IDanceCombine;
			ifDanceTogether = dataVal.ifDanceTogether;
			CombineDancer(CurrentDancer, ifDanceTogether);
		}
		m_swingingRipeData = dataVal.m_swingingRipeData;
		m_isHaveWind = dataVal.m_isHaveWind;
		if (m_isHaveWind)
		{
			m_windData = dataVal.m_windData;
			Mod.Event.FireNow(this, Mod.Reference.Acquire<WindOpenEventArgs>().Initialize(m_windData));
		}
		if (dataVal.CurrentVehicle.ifOnVehicle)
		{
			BaseVehicle element7;
			MapController.Instance.TryGetElementByUUID<BaseVehicle>(dataVal.CurrentVehicle.vehicleId, out element7);
			if ((bool)element7)
			{
				Vector3 localScale = ballRoleObj.transform.localScale;
				element7.RebirthResetByRole(this);
				ballRoleObj.transform.localScale = localScale;
			}
			else
			{
				Log.Error("Cannot find the vehicle " + dataVal.CurrentVehicle.vehicleId);
			}
		}
		SlideSpeed = dataVal.SlideSpeed;
		CurrentWorldRow = dataVal.CurrentWorldRow;
		LastRow = dataVal.LastRow;
		m_currentAnimType = dataVal.m_currentAnimType;
		ifRebirthWaiting = false;
		if (base.IfOnVehicle)
		{
			RD_ElementAnim_DATA roleAnimData = dataVal.roleAnimData;
			if (roleAnimData.animaName == "RideYangTuo")
			{
				_roleAnimator.Play(AnimType.RideYangTuo_Wait.ToString(), 0, 0f);
			}
			else
			{
				_roleAnimator.Play(roleAnimData.animaName, 0, roleAnimData.animTime);
			}
		}
		else if (base.IfJumping)
		{
			RolePlayAnim(AnimType.SuperJumpState, 0, 0f, true);
		}
		else if (base.IfDropping)
		{
			RolePlayAnim(AnimType.SuperJumpState, 0, 0f, true);
		}
		else if (CurrentState == BallState.SwingingRope)
		{
			RD_ElementAnim_DATA roleAnimData2 = dataVal.roleAnimData;
			_roleAnimator.Play(roleAnimData2.animaName, 0, roleAnimData2.animTime);
		}
		else if (CurrentState == BallState.PathToMove)
		{
			m_currentAnimType = dataVal.m_currentAnimType;
			RD_ElementAnim_DATA roleAnimData3 = dataVal.roleAnimData;
			if (m_currentAnimType == AnimType.WaitingState || m_currentAnimType == AnimType.RunState || m_currentAnimType == AnimType.StandUpToRun || m_currentAnimType == AnimType.SuperJumpToRoll)
			{
				RolePlayAnim(AnimType.WaitingState, 0, 0f, true);
			}
			else
			{
				_roleAnimator.Play(roleAnimData3.animaName, 0, roleAnimData3.animTime);
			}
		}
		else
		{
			ifRebirthWaiting = dataVal.m_currentAnimType == AnimType.WaitingState;
			RolePlayAnim(AnimType.WaitingState, 0, 0f, true);
		}
		StartCoroutine(WaitSwitchDynamicBoneType(DynamicBoneFactory.DynamicBoneType.RebirthResetBall));
		ShowRoleRender(Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).CurLevelId == 10000);
	}

	public override void StartRunByOriginRebirthBsonData(byte[] dataInfo)
	{
		ifRebirthing = false;
		if (base.IfOnVehicle)
		{
			RD_ElementAnim_DATA roleAnimData = Bson.ToObject<RD_RolePrince_DATA>(dataInfo).roleAnimData;
			if (roleAnimData.animaName == "RideYangTuo")
			{
				_roleAnimator.Play(roleAnimData.animaName, 0, roleAnimData.animTime);
			}
		}
		else if (!base.IfJumping && !base.IfDropping && CurrentState != BallState.SwingingRope)
		{
			if (CurrentState == BallState.PathToMove)
			{
				if (m_currentAnimType == AnimType.WaitingState)
				{
					RolePlayAnim(AnimType.RunState, 0, 0f, true);
				}
			}
			else if (ifRebirthWaiting)
			{
				RolePlayAnim(AnimType.WaitingState, 0, 0f, true);
			}
			else
			{
				RolePlayAnim(AnimType.RunState, 0, 0f, true);
			}
		}
		bool flag = (bool)faceCoverAnim;
		base.IfHaveTreasureChest = dataVal.IfHaveTreasureChest;
		if ((bool)treasureElement)
		{
			treasureElement.SetActive(dataVal.treasureElementShow);
		}
		if (faceCoverTrans != null)
		{
			faceCoverTrans.gameObject.SetActive(dataVal.IfShowfaceCover);
		}
		if (m_isHaveWind)
		{
			m_dynamicBoneFactory.SwitchDynamicBoneType(m_windData.m_isRightWind ? DynamicBoneFactory.DynamicBoneType.OpenRightWind : DynamicBoneFactory.DynamicBoneType.OpenLeftWind);
		}
		else
		{
			m_dynamicBoneFactory.SwitchDynamicBoneType(DynamicBoneFactory.DynamicBoneType.StartBall);
		}
		if (!_roleAnimator.enabled)
		{
			_roleAnimator.enabled = true;
		}
	}

	private bool TryGetCurrentAnimInfo(Animator animator, ref string animName, ref float animTime)
	{
		AnimatorStateInfo currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
		for (int i = 0; i < 65; i++)
		{
			string text = animStateNameDic[i];
			if (currentAnimatorStateInfo.IsName(text))
			{
				animName = text;
				animTime = currentAnimatorStateInfo.normalizedTime;
				break;
			}
		}
		return true;
	}

	private void ShowRoleRender(bool ifShow)
	{
		if (emissionRender == null)
		{
			return;
		}
		for (int i = 0; i < emissionRender.Length; i++)
		{
			emissionRender[i].enabled = ifShow;
		}
		for (int j = 0; j < otherRender.Length; j++)
		{
			otherRender[j].enabled = ifShow;
		}
		if ((bool)CurrentVehicle)
		{
			MeshRenderer[] componentsInChildren = GetComponentsInChildren<MeshRenderer>();
			for (int k = 0; k < componentsInChildren.Length; k++)
			{
				componentsInChildren[k].enabled = ifShow;
			}
		}
	}
}
