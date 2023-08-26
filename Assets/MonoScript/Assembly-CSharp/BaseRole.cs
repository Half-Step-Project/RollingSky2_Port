using System;
using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;

public abstract class BaseRole : MonoBehaviour, IOriginRebirth
{
	public enum BallState
	{
		Static,
		StartAnim,
		Cutscene,
		Move,
		JumpUp,
		JumpDown,
		Land,
		NormalDrop,
		DropDie,
		CrashDie,
		Finish,
		PathToMove,
		PathToMoveFixed,
		SwingingRope,
		Glide,
		PathToMoveLerp
	}

	public enum QTEType
	{
		Begin,
		Between,
		End
	}

	public enum JumpType
	{
		Normal,
		Super,
		QTE,
		QTEBetween,
		Ship,
		Back,
		Dance,
		NoAnim
	}

	public enum AnimType
	{
		BeforePlayState,
		SquatState,
		WaitingState,
		StandUpState,
		RunState,
		JumpState,
		SuperJumpState,
		RollState,
		QteLState,
		QteRState,
		SuccessState,
		JumpOnBoatUpState,
		JumpOnBoatDownState,
		JumpOnBoatBrretState,
		JumpOnBoatStandState,
		BoatStandRoll,
		WaitingToRunState,
		SuperJumpToRoll,
		JumpQTEBetweenState,
		DropDieState,
		SquatToStandUp,
		StandUpToRun,
		FlyState,
		FloatState,
		SkidDownState,
		DanceWithNPC,
		GrassGlide,
		Tug,
		Run_L,
		Run_R,
		Run01,
		Run02,
		Run03,
		Run04,
		Run05,
		Jump01,
		Jump02,
		Jump03,
		Jump04,
		Jump05,
		Dizziness,
		CombineDance,
		CombineDance_1,
		CombineDance_2,
		CombineDance_3,
		MirrorDance,
		MirrorDance_1,
		MirrorDance_2,
		MirrorDance_3,
		RopeDown,
		RopeDown_1,
		Flutter_1,
		Flutter_2,
		Occlusion,
		Skidding,
		Skidding_1,
		MirrorDance_4,
		MirrorDance_5,
		CombineDance_4,
		CombineDance_5,
		RollState_1,
		RollState_2,
		RollState_3,
		RideYangTuo,
		RideYangTuo_Wait,
		Length
	}

	public enum InputType
	{
		Horizen,
		FreeMove
	}

	public enum MoodFaceType
	{
		None,
		Happy_1,
		Happy_2,
		Happy_3,
		Happy_4,
		Angry_1,
		Angry_2,
		Angry_3,
		Angry_4
	}

	public class ForceHorizonData
	{
		public Vector3 BeginPos;

		public Vector3 EndPos;

		public Transform GridTrans;

		public bool GetForceHorizonPos(Vector3 currentPos, ref Vector3 targetPos)
		{
			Vector3 vector = GridTrans.InverseTransformPoint(BeginPos);
			Vector3 vector2 = GridTrans.InverseTransformPoint(EndPos);
			Vector3 vector3 = GridTrans.InverseTransformPoint(currentPos);
			if (vector3.z > vector2.z)
			{
				return false;
			}
			float num = vector2.z - vector.z;
			if (num > 0f)
			{
				float t = (vector3.z - vector.z) / num;
				targetPos = Vector3.Lerp(BeginPos, EndPos, t);
				return true;
			}
			Debug.LogError("deltaZ < 0");
			return false;
		}
	}

	public class RoleMaterialNodeData
	{
		public bool ENABLE_FRESNEL;

		public Color _FresnelColor;

		public float _FresnelPower;

		public float _FresnelScale;
	}

	public static BaseRole theBall;

	protected bool IfSpecialDie;

	protected bool IfPlayRoleEff = true;

	public RoleData CurrentRoleData;

	public BaseTile LastTile;

	public BaseTile CurrentTile;

	[HideInInspector]
	public Transform currentGrid;

	public InputType currentInputType;

	protected PetBase currentPet;

	public MoodFaceType currentMoodType;

	public RoleSkinController m_roleSkinController;

	public int PathTriggerIndex = -1;

	public int PathFixTriggerIndex = -1;

	public int PathLerpTriggerIndex = -1;

	[HideInInspector]
	public PathToMoveByRoleTrigger.PathToMoveByRoleTriggerData m_pathData;

	[HideInInspector]
	public PathToMoveByRoleForLerpTrigger.PathToMoveByRoleForLerpTriggerData m_pathLerpData;

	[HideInInspector]
	public PathToMoveFixedByRoleTrigger.PathToMoveFixedByRoleTriggerData m_transformPathDatas;

	protected Vector3[] m_path;

	protected PathToMoveTransform[] m_transformsPath;

	protected const float m_roleRotationSpeed = 10f;

	[HideInInspector]
	public int currentPathPointIndex;

	protected int m_runtimeAnimatorControllerIndex;

	protected int m_roleSkinIndex;

	public SwingingRipeData m_swingingRipeData;

	private GameController m_gameController;

	public BaseVehicle CurrentVehicle;

	public BallState CurrentState;

	public IDanceCombine CurrentDancer;

	private System.Action currentUpdate;

	protected Dictionary<int, System.Action> updateStateList = new Dictionary<int, System.Action>();

	public List<BaseTile> collideTileList = new List<BaseTile>();

	public const float DEFAULT_SLIDESPEED = 4f;

	public Vector3 StartRoleLocalPos;

	public Vector3 RestartRoleLocalPos;

	[HideInInspector]
	public float SlideSpeed = 4f;

	[HideInInspector]
	public float SpeedForward = 5f;

	public DynamicBoneFactory m_dynamicBoneFactory;

	public RoleProperty m_roleProperty;

	public bool m_isHaveWind;

	public WindOpenTrigger.WindData m_windData;

	public AnimType m_currentAnimType;

	protected JumpUtil jumpUtilInfo;

	protected JumpType jumpType;

	protected float dropBeginY;

	protected Vector3 dropBeginPos;

	protected Vector3 dropEndPos;

	protected Vector3 dropDir;

	protected float dropDistance;

	protected float dropHeightY;

	protected float dropDeltaY;

	protected ForceHorizonData forceHorizonData;

	public RoleBodyPart BodyPart;

	public GameObject ballModelObj;

	public GameObject ballRoleObj;

	protected GameObject ballJumpTailEffect;

	protected GameObject ballJumpWingsEffect;

	protected GameObject runSmokeEffect;

	protected GameObject thiefChestObj;

	protected ParticleSystem[] thiefChestEffect = new ParticleSystem[0];

	protected GameObject moodFacePart;

	protected GameObject moodHappyNode;

	protected GameObject[] moodHappyNodeList = new GameObject[0];

	protected GameObject moodAngryNode;

	public Transform DogLineEndPart;

	protected GameObject ballDeadObj;

	protected ParticleSystem deadBombEffect;

	protected float RotationSpeed = 8f;

	protected Animator _roleAnimator;

	[HideInInspector]
	public int CurrentWorldRow;

	[HideInInspector]
	public int LastRow;

	private Renderer[] m_bodyRenderers;

	private Dictionary<int, RoleMaterialNodeData> m_materialDatas = new Dictionary<int, RoleMaterialNodeData>();

	public static Vector3 BallPosition
	{
		get
		{
			if ((bool)theBall)
			{
				return theBall.transform.position;
			}
			return Vector3.zero;
		}
	}

	public bool IfJumping
	{
		get
		{
			if (CurrentState != BallState.JumpUp)
			{
				return CurrentState == BallState.JumpDown;
			}
			return true;
		}
	}

	public bool IfJumpingUp
	{
		get
		{
			return CurrentState == BallState.JumpUp;
		}
	}

	public bool IfJumpingDown
	{
		get
		{
			return CurrentState == BallState.JumpDown;
		}
	}

	public bool IfDropping
	{
		get
		{
			return CurrentState == BallState.NormalDrop;
		}
	}

	public bool IfDie
	{
		get
		{
			if (CurrentState == BallState.DropDie || CurrentState == BallState.CrashDie)
			{
				return true;
			}
			return false;
		}
	}

	public bool IsInvincible { get; set; }

	public bool IfOnVehicle
	{
		get
		{
			return CurrentVehicle != null;
		}
	}

	public bool IfWinBeforeFinish { get; set; }

	public bool IfHaveTreasureChest { get; set; }

	public bool IfDizziness { get; set; }

	public GameController GetGameController
	{
		get
		{
			if (m_gameController == null)
			{
				m_gameController = GameController.Instance;
			}
			return m_gameController;
		}
	}

	public bool IfRebirthOnVehicle { get; protected set; }

	public float RoleMoveLimit { get; set; }

	protected float RealRotateSpeed
	{
		get
		{
			if (IfOnVehicle && CurrentVehicle.VehicleRotateSpeed >= 0f)
			{
				return CurrentVehicle.VehicleRotateSpeed;
			}
			return RotationSpeed;
		}
	}

	public int GainedCrownFragment
	{
		get
		{
			return GameController.Instance.GetInsideGameDataModule.GainedCrownFragments;
		}
	}

	public int GainedDiamondFragment
	{
		get
		{
			return GameController.Instance.GetInsideGameDataModule.GainedDiamondFragments;
		}
	}

	public virtual bool IsRecordOriginRebirth
	{
		get
		{
			return true;
		}
	}

	public JumpUtil GetJumpUtilInfo()
	{
		return jumpUtilInfo;
	}

	protected virtual void RoleConstruct()
	{
	}

	protected abstract void RoleRegisterListener();

	protected abstract void RoleUnregisterListener();

	protected virtual void OnTriggerEnter(Collider collider)
	{
	}

	protected virtual void OnTriggerExit(Collider collider)
	{
	}

	public virtual void PreInitialize()
	{
	}

	public virtual void Initialize(Transform parent, Vector3 startLocalPos, Vector3 restartLocalPos)
	{
		theBall = this;
		RoleRegisterListener();
	}

	public virtual void SetEmissionInfo(bool ifEmission, float delayTime, float emissionTime, float beginEmission, float endEmission)
	{
	}

	public virtual void SetBeginEmission()
	{
	}

	public virtual void SetEndEmission()
	{
	}

	public void UnloadLocal(bool ifHide = true)
	{
		RoleUnregisterListener();
		base.gameObject.SetActive(!ifHide);
	}

	public void DestroyLocal()
	{
		OnDestroyLocal();
		UnityEngine.Object.Destroy(base.gameObject);
		if (theBall != null)
		{
			theBall = null;
		}
	}

	protected virtual void OnUnloadLocal()
	{
	}

	protected abstract void OnDestroyLocal();

	public virtual void DevInitialize(Transform parent, Vector3 startLocalPos, Vector3 restartLocalPos)
	{
		RoleRegisterListener();
	}

	public virtual void InitializeByRole(BaseRole oriRole)
	{
		theBall = this;
		RoleRegisterListener();
	}

	public virtual void StartBall(RebirthBoxData savePoint = null)
	{
	}

	public virtual void ResetBall()
	{
	}

	public virtual void ResetBySavePointInfo(RebirthBoxData savePoint)
	{
	}

	public abstract RebirthRoleData GetRebirthData(Transform boxTriggerTransform);

	public virtual void ResetInputParam(float speed = 4f)
	{
		SlideSpeed = speed;
	}

	public virtual void ChangeRuntimeAnimatorControllerByIndex(int index, bool ifPlay = true)
	{
	}

	public virtual void ChangeRoleSkinByIndex(int index)
	{
	}

	public virtual void CrashBall()
	{
	}

	public virtual void BeginDropDie()
	{
	}

	public virtual void SpecialCrashBall(Vector3 diePos, bool ifMoveCamera = true)
	{
	}

	public virtual void CallBeginJump(Vector3 beginPos, Vector3 endPos, Vector3 normalDir, float heightY, JumpType jType, bool isShowTrail = true)
	{
	}

	public virtual void CallEndJump(float tileY, bool ifContinue = false)
	{
	}

	public virtual void CallBeginDrop(Vector3 beginPos, Vector3 endPos, Vector3 normalDir, float heightY)
	{
	}

	public virtual void CallEndDrop(float tileY)
	{
	}

	public virtual void ChangeToCutscene()
	{
	}

	public virtual void ChangeToStartAnim()
	{
	}

	public virtual void CallChangeToPathToMove(PathToMoveByRoleTrigger.PathToMoveByRoleTriggerData pathData, int triggerIndex, int pathPointIndex = 0)
	{
	}

	public virtual void CallChangeToPathToMoveLerp(PathToMoveByRoleForLerpTrigger.PathToMoveByRoleForLerpTriggerData pathData, int triggerIndex, int pathPointIndex = 0)
	{
	}

	public virtual void CallChangeToPathToMoveFixed(PathToMoveFixedByRoleTrigger.PathToMoveFixedByRoleTriggerData pathData, int triggerIndex, int pathPointIndex = 0)
	{
	}

	protected virtual void ChangeToPathToMove(PathToMoveByRoleTrigger.PathToMoveByRoleTriggerData pathData, int pathPointIndex = 0)
	{
	}

	protected virtual void ChangeToPathToMoveLerp(PathToMoveByRoleForLerpTrigger.PathToMoveByRoleForLerpTriggerData pathData, int pathPointIndex = 0)
	{
	}

	protected virtual void ChangeToPathToMoveFixed(PathToMoveFixedByRoleTrigger.PathToMoveFixedByRoleTriggerData pathData, int pathPointIndex = 0)
	{
	}

	public virtual void ChangeToMove()
	{
	}

	public virtual void ChangeToSwingingRope(SwingingRipeData data)
	{
	}

	public virtual void ChangeToFly()
	{
	}

	public virtual void ChangeToGlide()
	{
	}

	public virtual void ChangeToFloat()
	{
	}

	public virtual void ChangeToWin(Vector3 pos)
	{
	}

	public virtual void UpdateRole()
	{
		currentUpdate();
		UpdateDebugMove();
		UpdateTrailEffect();
		UpdateDizziness();
		if (CurrentState != BallState.Cutscene && CurrentState != BallState.DropDie)
		{
			UpdateProgress();
		}
	}

	public virtual void FixedUpdateRole()
	{
		UpdateResetTileList();
	}

	protected virtual void UpdateDizziness()
	{
	}

	protected virtual void UpdateDebugMove()
	{
	}

	protected virtual void ChangeStateTo(BallState state)
	{
		CurrentState = state;
		currentUpdate = updateStateList[(int)CurrentState];
	}

	public virtual void OnTileEnter(BaseTile tile)
	{
	}

	public virtual void OnTileExit(BaseTile tile)
	{
	}

	protected virtual void UpdateResetTileList()
	{
	}

	public virtual void AddVehicle(BaseVehicle vehicle)
	{
	}

	public virtual void DepartVehicle(bool ifDestroy = false)
	{
	}

	public virtual void DepartFromPet()
	{
	}

	public void SetPet(PetBase pet)
	{
		currentPet = pet;
	}

	public virtual void SetSpecialDieFlag(bool ifTreeKill, bool ifPlayRoleEff)
	{
		IfSpecialDie = ifTreeKill;
		IfPlayRoleEff = ifPlayRoleEff;
	}

	public virtual void GainDiamond(int uuid, int sortID, int number = 1)
	{
		Mod.Event.FireNow(this, Mod.Reference.Acquire<GainedDropEventArgs>().Initialize(new DropData(uuid, DropType.DIAMOND, sortID, number)));
	}

	public virtual void GainCrown(int uuid, int sortID, int number = 1)
	{
		Mod.Event.FireNow(this, Mod.Reference.Acquire<GainedDropEventArgs>().Initialize(new DropData(uuid, DropType.CROWN, sortID, number)));
	}

	public virtual void GainTreasureChest(bool ifGain)
	{
	}

	public virtual void ForceShowTreasureChest(bool ifShow)
	{
	}

	public virtual void ForceShowCoverFace(bool ifShow)
	{
	}

	public virtual void CombineDancer(IDanceCombine dancer, bool ifTogether = false)
	{
	}

	public virtual void SeparateDancer(IDanceCombine dancer, bool ifClearRef = false)
	{
	}

	public virtual void DanceTogether(int danceType)
	{
	}

	protected virtual float MoveX(float targetX)
	{
		return targetX;
	}

	protected virtual void FlyMove(Vector2 deltaPos)
	{
	}

	protected virtual float GetMoveXParam(float targetX)
	{
		return targetX;
	}

	protected virtual void ResetTurning(float horizonDegree = 0f)
	{
	}

	protected virtual void ReleaseHandByWind()
	{
	}

	protected virtual void MoveForward()
	{
	}

	protected virtual void UpdateForceMove()
	{
	}

	protected virtual void BeginJump(Vector3 beginPos, Vector3 endPos, Vector3 normalDir, float heightY, JumpType jType, bool isShowTrail = true)
	{
	}

	protected virtual float Jumping()
	{
		return 0f;
	}

	protected virtual void ResetPosY(float tileY)
	{
	}

	public virtual void BeginCloseTrail()
	{
	}

	public virtual float GetJumpPercent()
	{
		return 0f;
	}

	protected virtual void BeginDrop(Vector3 beginPos, Vector3 endPos, Vector3 normalDir, float heightY)
	{
	}

	protected virtual void Dropping()
	{
	}

	protected virtual void RolePlayAnim(AnimType animType, int layerID = 0, float normalizedTime = 0f, bool ifForce = false)
	{
	}

	public virtual void RoleStopAnim()
	{
	}

	public virtual void TriggerRolePlayAnim(AnimType animType, bool ifForce = true)
	{
	}

	public virtual void ResetAnimFlag()
	{
	}

	public virtual void ChangeInputType(InputType inputType)
	{
		currentInputType = inputType;
	}

	public virtual bool IfPlayBoatStandRoll()
	{
		return false;
	}

	protected void SetPosition(Vector3 pos)
	{
		base.transform.position = pos;
	}

	protected void SetPositionX(float posX)
	{
		Vector3 position = base.transform.position;
		position.x = posX;
		SetPosition(position);
	}

	protected void SetPositionY(float posY)
	{
		Vector3 position = base.transform.position;
		position.y = posY;
		SetPosition(position);
	}

	protected void SetPositionZ(float posZ)
	{
		Vector3 position = base.transform.position;
		position.z = posZ;
		SetPosition(position);
	}

	protected void SetLocalPosition(Vector3 pos)
	{
		base.transform.localPosition = pos;
	}

	protected void SetLocalPositionX(float posX)
	{
		Vector3 localPosition = base.transform.localPosition;
		localPosition.x = posX;
		SetLocalPosition(localPosition);
	}

	protected void SetLocalPositionY(float posY)
	{
		Vector3 localPosition = base.transform.localPosition;
		localPosition.y = posY;
		SetLocalPosition(localPosition);
	}

	protected void SetLocalPositionZ(float posZ)
	{
		Vector3 localPosition = base.transform.localPosition;
		localPosition.z = posZ;
		SetLocalPosition(localPosition);
	}

	public float BallMoveOffset()
	{
		return Railway.theRailway.GetRightOffset(base.transform.position);
	}

	public void ForceMoveTo(Vector3 pos)
	{
		base.transform.position = pos;
	}

	protected virtual void InitProgress()
	{
	}

	protected virtual void UpdateProgress()
	{
		int currentGridRow = MapController.Instance.GetCurrentGridRow(base.transform.position);
		if (LastRow == 0)
		{
			LastRow = currentGridRow;
		}
		int num = currentGridRow - LastRow;
		if (num < -1)
		{
			num = 1;
		}
		else if (num > 1)
		{
			num = -1;
		}
		CurrentWorldRow += num;
		LastRow = currentGridRow;
	}

	public virtual void MountEffect(Transform effect)
	{
	}

	public void SetForceHorizonData(ForceHorizonData data)
	{
		forceHorizonData = data;
	}

	public void ClearForceHorizonData()
	{
		forceHorizonData = null;
	}

	protected virtual void UpdateTrailEffect()
	{
	}

	public virtual void GainCrownFragment(int uuid, int number = 1)
	{
		Mod.Event.FireNow(this, Mod.Reference.Acquire<GainedDropEventArgs>().Initialize(new DropData(uuid, DropType.CROWNFRAGMENT, 0, number)));
	}

	public virtual void GainDiamondFragment(int uuid, int number = 1)
	{
		Mod.Event.FireNow(this, Mod.Reference.Acquire<GainedDropEventArgs>().Initialize(new DropData(uuid, DropType.DIAMONDFRAGMENT, 0, number)));
	}

	public virtual void ResetCrownFragmentCount()
	{
		GameController.Instance.GetInsideGameDataModule.GainedCrownFragments = 0;
	}

	public virtual void ResetDiamondFragmentCount()
	{
		GameController.Instance.GetInsideGameDataModule.GainedDiamondFragments = 0;
	}

	public void ShowInnerGlowData(bool enable, InnerGlowType type = InnerGlowType.Invincible)
	{
		if (!(theBall != null) || !(theBall.ballModelObj != null))
		{
			return;
		}
		Color color = new Color32(0, 213, byte.MaxValue, byte.MaxValue);
		float value = 2f;
		float value2 = 0.01f;
		switch (type)
		{
		case InnerGlowType.Shield:
			color = new Color32(0, 213, byte.MaxValue, byte.MaxValue);
			value = 2f;
			value2 = 0.01f;
			break;
		case InnerGlowType.Invincible:
			color = new Color32(byte.MaxValue, 216, 0, byte.MaxValue);
			value = 2f;
			value2 = 0.01f;
			break;
		}
		if (m_bodyRenderers == null)
		{
			m_bodyRenderers = theBall.ballModelObj.GetComponentsInChildren<Renderer>();
		}
		if (!enable && m_materialDatas.Count > 0)
		{
			for (int i = 0; i < m_bodyRenderers.Length; i++)
			{
				if (m_bodyRenderers[i] != null && (bool)m_bodyRenderers[i].sharedMaterial && !MaterialTool.IsTransparentMaterial(m_bodyRenderers[i]))
				{
					RoleMaterialNodeData value3 = null;
					if (m_materialDatas.TryGetValue(m_bodyRenderers[i].GetInstanceID(), out value3) && value3 != null)
					{
						MaterialTool.SetMaterialKeyWord(m_bodyRenderers[i], "ENABLE_FRESNEL", value3.ENABLE_FRESNEL);
						MaterialTool.SetMaterialColor(m_bodyRenderers[i], "_FresnelColor", value3._FresnelColor);
						MaterialTool.SetMaterialFloat(m_bodyRenderers[i], "_FresnelPower", value3._FresnelPower);
						MaterialTool.SetMaterialFloat(m_bodyRenderers[i], "_FresnelScale", value3._FresnelScale);
					}
				}
			}
			return;
		}
		for (int j = 0; j < m_bodyRenderers.Length; j++)
		{
			if (m_bodyRenderers[j] != null && (bool)m_bodyRenderers[j].sharedMaterial && !MaterialTool.IsTransparentMaterial(m_bodyRenderers[j]))
			{
				MaterialTool.SetMaterialKeyWord(m_bodyRenderers[j], "ENABLE_FRESNEL", enable);
				MaterialTool.SetMaterialColor(m_bodyRenderers[j], "_FresnelColor", color);
				MaterialTool.SetMaterialFloat(m_bodyRenderers[j], "_FresnelPower", value);
				MaterialTool.SetMaterialFloat(m_bodyRenderers[j], "_FresnelScale", value2);
			}
		}
	}

	public void GetInnerGlowData()
	{
		m_materialDatas.Clear();
		if (!(theBall != null) || !(theBall.ballModelObj != null))
		{
			return;
		}
		if (m_bodyRenderers == null)
		{
			m_bodyRenderers = theBall.ballModelObj.GetComponentsInChildren<Renderer>();
		}
		for (int i = 0; i < m_bodyRenderers.Length; i++)
		{
			if (m_bodyRenderers[i] != null && (bool)m_bodyRenderers[i].sharedMaterial && !MaterialTool.IsTransparentMaterial(m_bodyRenderers[i]))
			{
				RoleMaterialNodeData roleMaterialNodeData = new RoleMaterialNodeData();
				roleMaterialNodeData.ENABLE_FRESNEL = MaterialTool.GetMaterialKeyWord(m_bodyRenderers[i], "ENABLE_FRESNEL");
				roleMaterialNodeData._FresnelColor = MaterialTool.GetMaterialColor(m_bodyRenderers[i], "_FresnelColor");
				roleMaterialNodeData._FresnelPower = MaterialTool.GetMaterialFloat(m_bodyRenderers[i], "_FresnelPower");
				roleMaterialNodeData._FresnelScale = MaterialTool.GetMaterialFloat(m_bodyRenderers[i], "_FresnelScale");
				m_materialDatas[m_bodyRenderers[i].GetInstanceID()] = roleMaterialNodeData;
			}
		}
	}

	[Obsolete("this is Obsolete,please  please use GetOriginRebirthBsonData !")]
	public virtual object GetOriginRebirthData(object obj = null)
	{
		return string.Empty;
	}

	[Obsolete("this is Obsolete,please  please use SetOriginRebirthBsonData !")]
	public virtual void SetOriginRebirthData(object dataInfo)
	{
	}

	[Obsolete("this is Obsolete,please  please use StartRunByOriginRebirthBsonData !")]
	public virtual void StartRunByOriginRebirthData(object dataInfo)
	{
	}

	public virtual byte[] GetOriginRebirthBsonData(object obj = null)
	{
		return null;
	}

	public virtual void SetOriginRebirthBsonData(byte[] dataInfo)
	{
	}

	public virtual void StartRunByOriginRebirthBsonData(byte[] dataInfo)
	{
	}
}
