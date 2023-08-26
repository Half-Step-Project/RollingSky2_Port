using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.Rendering;
using VacuumShaders.CurvedWorld;

public class CameraController : MonoBehaviour, IOriginRebirth
{
	public enum CameraState
	{
		NORMAL,
		WAITFORQTE,
		QTE,
		QTEEND,
		WINSTOP,
		NULL,
		CUTSCENE,
		STARTANIM,
		FOLLOW,
		PATHTOMOVE,
		WINSTATIC
	}

	public static readonly string CameraEffectNode = "CameraEffect";

	public static CameraController theCamera;

	public CameraMover m_cameraMover;

	public Transform m_Camera;

	public readonly float m_centerX = 2.5f;

	public OpaqueSortMode MainCameraOpaqueSortMode;

	public float m_TileFarClip = -1f;

	public float m_DefaultFarClipPlane;

	private float gameLoseSlowTimer;

	private float gameLoseSlowTime = 2f;

	private float gameWinSlowTimer;

	private float gameWinSlowTime = 2f;

	private Animator cameraAnimator;

	public Animation m_cameraAnimation;

	public CurvedWorld_EagleEye m_curvedWorldEagleEye;

	public CurvedWorld_Controller m_curvedWorldController;

	public bool EnableCurvedWorld;

	public CameraState m_cameraState;

	private ParticleSystem[][] cameraParticles;

	private int currentParticleIndex = -1;

	private float[] layerDistances = new float[32];

	private bool ifRebirthing;

	private Transform effectRoot;

	public bool IfDieShakeCamera = true;

	private float m_PerformanceScaledTileFarClip = -1f;

	private const int LAYERINDEX_GROUND = 9;

	private const int LAYERINDEX_ENEMY = 10;

	private const int LAYERINDEX_BACK = 24;

	private const float DEFAULT_BACK_DISTANCE = 0f;

	private float startfarClip = 100f;

	public Vector3 CamStartLocalPos;

	public float CamStartFov;

	public Vector3 CamStartTargetPos;

	public Vector3 StartLocalPos;

	public Vector3 StartLocalEuler;

	public float CurveXAxisBendSize;

	public float CurveYAxisBendSize;

	public float CurveXBias;

	public float CurveYBias;

	private PathToMoveByCameraTrigger.ElementData m_pathToMoveByCameraData;

	private BezierMover m_bezierMover;

	private float m_deltaLocZ;

	private Vector3 m_beginPos = Vector3.zero;

	private Vector3 m_targetPos = Vector3.zero;

	private Vector3 m_moveLocDir = Vector3.forward;

	private bool m_isFinish;

	private FollowData m_followData;

	public bool m_isPlayingCurvedBend;

	public CurvedBendData m_curvedBendFrom;

	public CurvedBendData m_curvedBendTo;

	public float m_curvedBendDuration;

	public float m_currentCurvedBendTime;

	private float m_curvedBendProgress;

	private RD_CameraController_DATA m_rebirthData;

	public FollowData GetFollowData
	{
		get
		{
			return m_followData;
		}
	}

	public CurvedBendData GetCurvedBendCurrent
	{
		get
		{
			CurvedBendData result = default(CurvedBendData);
			if (m_curvedWorldController != null)
			{
				result.m_xAxisBendSize = m_curvedWorldController._V_CW_Bend_X;
				result.m_yAxisBendSize = m_curvedWorldController._V_CW_Bend_Y;
				result.m_xBias = m_curvedWorldController._V_CW_Bias_X;
				result.m_yBias = m_curvedWorldController._V_CW_Bias_Y;
			}
			return result;
		}
	}

	public bool IsRecordOriginRebirth
	{
		get
		{
			return true;
		}
	}

	private void Awake()
	{
		theCamera = this;
	}

	private void OnDestroy()
	{
		if (theCamera != null)
		{
			theCamera = null;
		}
	}

	private void OnEnable()
	{
		Mod.Event.Subscribe(EventArgs<FollowOpenEventArgs>.EventId, OnFollowOpenHandler);
		Mod.Event.Subscribe(EventArgs<FollowCloseEventArgs>.EventId, OnFollowCloseHandler);
		Mod.Event.Subscribe(EventArgs<CameraParticlePlayEventArgs>.EventId, OnChangeCameraParticle);
		Mod.Event.Subscribe(EventArgs<CameraParticlePlayByNameEventArgs>.EventId, OnChangeCameraParticleByName);
		Mod.Event.Subscribe(EventArgs<PathToMoveByCameraEventArgs>.EventId, OnPathToMoveByCameraHandler);
	}

	private void OnDisable()
	{
		if (Mod.Event != null)
		{
			Mod.Event.Unsubscribe(EventArgs<FollowOpenEventArgs>.EventId, OnFollowOpenHandler);
			Mod.Event.Unsubscribe(EventArgs<FollowCloseEventArgs>.EventId, OnFollowCloseHandler);
			Mod.Event.Unsubscribe(EventArgs<CameraParticlePlayEventArgs>.EventId, OnChangeCameraParticle);
			Mod.Event.Unsubscribe(EventArgs<CameraParticlePlayByNameEventArgs>.EventId, OnChangeCameraParticleByName);
			Mod.Event.Unsubscribe(EventArgs<PathToMoveByCameraEventArgs>.EventId, OnPathToMoveByCameraHandler);
		}
	}

	protected void Update()
	{
		Profiler.BeginSample("CameraController_Update");
		if (m_TileFarClip > 0f)
		{
			Camera main = Camera.main;
			float num = Mathf.Min(m_PerformanceScaledTileFarClip, main.farClipPlane);
			layerDistances[9] = num;
			layerDistances[10] = num;
			layerDistances[24] = 0f;
			main.layerCullDistances = layerDistances;
		}
		Profiler.EndSample();
	}

	public void Initialize()
	{
		SwitchCameraState(CameraState.NORMAL);
		Camera main = Camera.main;
		StartLocalPos = base.transform.localPosition;
		StartLocalEuler = base.transform.localEulerAngles;
		CamStartLocalPos = main.transform.localPosition;
		CamStartFov = main.fieldOfView;
		CamStartTargetPos = CameraLookAt.Instance.target.localPosition;
		startfarClip = main.farClipPlane;
		gameLoseSlowTimer = gameLoseSlowTime;
		gameWinSlowTimer = gameWinSlowTime;
		m_cameraAnimation = GetComponent<Animation>();
		m_Camera = main.transform;
		m_Camera.transform.SetParent(base.transform);
		m_cameraMover = new CameraMover(base.gameObject.transform, m_centerX);
		cameraAnimator = main.GetComponent<Animator>();
		if (m_curvedWorldController == null)
		{
			m_curvedWorldController = base.gameObject.GetComponentInChildren<CurvedWorld_Controller>();
		}
		m_curvedWorldEagleEye.enabled = EnableCurvedWorld;
		m_curvedWorldEagleEye.fieldOfView = main.fieldOfView;
		m_curvedWorldController.enabled = EnableCurvedWorld;
		effectRoot = m_Camera.Find("effectRoot");
		if (effectRoot == null)
		{
			effectRoot = m_Camera.parent.Find("effectRoot");
		}
		Transform transform = main.transform.Find(CameraEffectNode);
		if (transform == null)
		{
			transform = main.transform.parent.Find(CameraEffectNode);
		}
		if ((bool)transform)
		{
			int childCount = transform.childCount;
			cameraParticles = new ParticleSystem[childCount][];
			for (int i = 0; i < childCount; i++)
			{
				Transform transform2 = transform.Find("effect" + (i + 1));
				if ((bool)transform2)
				{
					cameraParticles[i] = transform2.GetComponentsInChildren<ParticleSystem>();
				}
			}
			StopAllParticles();
		}
		for (int j = 0; j < layerDistances.Length; j++)
		{
			layerDistances[j] = m_DefaultFarClipPlane;
		}
		if (m_TileFarClip > 0f)
		{
			float num = m_TileFarClip * (float)(Screen.height * 9) / (float)(Screen.width * 16);
			num = (m_PerformanceScaledTileFarClip = ((num > m_TileFarClip) ? num : m_TileFarClip));
			num = ((num > main.farClipPlane) ? main.farClipPlane : num);
			int @int = Mod.Setting.GetInt("Setting.QualityLevel");
			if (DeviceManager.Instance.IsLowEndQualityLevel(@int))
			{
				num *= 0.75f;
			}
			layerDistances[9] = num;
			layerDistances[10] = num;
		}
		layerDistances[24] = 0f;
		main.layerCullDistances = layerDistances;
		main.opaqueSortMode = MainCameraOpaqueSortMode;
		RecordCurvedWorld();
		ifRebirthing = false;
	}

	public void DevInitialize()
	{
		m_Camera = Camera.main.transform;
	}

	public int RegistAnimationClip(AnimationClip animClip, string animName)
	{
		return animClip.GetInstanceID();
	}

	public void TriggerPlayAnimClip(AnimationClip animClip, string animName)
	{
		if (m_cameraAnimation != null)
		{
			m_cameraAnimation.AddClip(animClip, animName);
			if (!ifRebirthing)
			{
				m_cameraAnimation.Play(animName);
			}
		}
	}

	public void TriggerRemoveAnimClip(AnimationClip animClip, string animName)
	{
		if (m_cameraAnimation != null && m_cameraAnimation.GetClip(animName) != null)
		{
			m_cameraAnimation.RemoveClip(animClip);
		}
	}

	public void ResetController()
	{
		ResetEffectRoot();
		base.transform.parent = Railway.theRailway.transform;
		base.transform.localPosition = StartLocalPos;
		base.transform.localEulerAngles = StartLocalEuler;
		m_cameraMover.Reset();
		gameLoseSlowTimer = gameLoseSlowTime;
		gameWinSlowTimer = gameWinSlowTime;
		if ((bool)m_cameraAnimation && m_cameraAnimation.GetClipCount() > 0)
		{
			m_cameraAnimation.Play();
			m_cameraAnimation[m_cameraAnimation.clip.name].normalizedTime = 0f;
			m_cameraAnimation.Sample();
			m_cameraAnimation.Stop();
		}
		Camera main = Camera.main;
		main.transform.parent = base.transform;
		main.farClipPlane = startfarClip;
		main.transform.localPosition = CamStartLocalPos;
		main.fieldOfView = CamStartFov;
		CameraLookAt.Instance.target.localPosition = CamStartTargetPos;
		CloseFollow();
		StopAllParticles();
		ResetCurvedWorld();
		SwitchCameraState(CameraState.NORMAL);
		ifRebirthing = false;
	}

	private void ResetEffectRoot()
	{
		if (!effectRoot)
		{
			return;
		}
		int childCount = effectRoot.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = effectRoot.GetChild(i);
			Animation componentInChildren = child.GetComponentInChildren<Animation>();
			if ((bool)componentInChildren)
			{
				componentInChildren.Rewind();
				componentInChildren.Sample();
				componentInChildren.Stop();
			}
			ParticleSystem component = child.GetComponent<ParticleSystem>();
			if ((bool)component)
			{
				component.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
			}
		}
	}

	private void ResetCurvedWorld()
	{
		if (m_curvedWorldController != null && EnableCurvedWorld)
		{
			m_curvedWorldController._V_CW_Bend_X = CurveXAxisBendSize;
			m_curvedWorldController._V_CW_Bend_Y = CurveYAxisBendSize;
			m_curvedWorldController._V_CW_Bias_X = CurveXBias;
			m_curvedWorldController._V_CW_Bias_Y = CurveYBias;
			m_curvedBendFrom = default(CurvedBendData);
			m_curvedBendFrom.m_xAxisBendSize = CurveXAxisBendSize;
			m_curvedBendFrom.m_yAxisBendSize = CurveYAxisBendSize;
			m_curvedBendFrom.m_xBias = CurveXBias;
			m_curvedBendFrom.m_yBias = CurveYBias;
			m_curvedBendTo = m_curvedBendFrom;
			PlayCurvedBend(m_curvedBendFrom, m_curvedBendTo, 1f, 1f);
		}
	}

	private void RecordCurvedWorld()
	{
		if (m_curvedWorldController != null && EnableCurvedWorld)
		{
			CurveXAxisBendSize = m_curvedWorldController._V_CW_Bend_X;
			CurveYAxisBendSize = m_curvedWorldController._V_CW_Bend_Y;
			CurveXBias = m_curvedWorldController._V_CW_Bias_X;
			CurveYBias = m_curvedWorldController._V_CW_Bias_Y;
			m_curvedBendFrom = default(CurvedBendData);
			m_curvedBendFrom.m_xAxisBendSize = CurveXAxisBendSize;
			m_curvedBendFrom.m_yAxisBendSize = CurveYAxisBendSize;
			m_curvedBendFrom.m_xBias = CurveXBias;
			m_curvedBendFrom.m_yBias = CurveYBias;
			m_curvedBendTo = m_curvedBendFrom;
		}
	}

	public void ResetBySavePointInfo(RebirthBoxData savePoint)
	{
		base.transform.parent = Railway.theRailway.gameObject.transform;
		base.transform.localPosition = savePoint.m_cameraData.CameraLocalPos;
		base.transform.localEulerAngles = savePoint.m_cameraData.CameraLocalAngle;
		CameraLookAt.Instance.target.localPosition = savePoint.m_cameraData.CameraTargetLocalPos;
		CameraLookAt.Instance.m_lookAtSlerpSpeed = savePoint.m_cameraData.m_lookAtSlerpSpeed;
		m_cameraMover.Reset();
		gameLoseSlowTimer = gameLoseSlowTime;
		gameWinSlowTimer = gameWinSlowTime;
		Camera.main.transform.parent = base.transform;
		Camera.main.transform.localPosition = savePoint.m_cameraData.RealCameraLocalPos;
		Camera.main.transform.localEulerAngles = Vector3.zero;
		string animationName = savePoint.m_cameraData.m_animationName;
		if (!string.IsNullOrEmpty(animationName))
		{
			AnimationState animationState = m_cameraAnimation[animationName];
			if (animationState == null || animationState.clip == null)
			{
				animationState = LoadAnimationState(animationName);
			}
			animationState.time = savePoint.m_cameraData.m_animationTime;
			m_cameraAnimation.Play(animationState.name);
			m_cameraAnimation.Sample();
			m_cameraAnimation.Stop();
		}
		if (EnableCurvedWorld)
		{
			m_curvedBendFrom = savePoint.m_cameraData.m_curvedBendData.m_from;
			m_curvedBendTo = savePoint.m_cameraData.m_curvedBendData.m_to;
			m_curvedBendDuration = savePoint.m_cameraData.m_curvedBendData.m_duration;
			m_currentCurvedBendTime = savePoint.m_cameraData.m_curvedBendData.m_currentTime;
			SetCurvedBend(savePoint.m_cameraData.m_curvedBendData.m_current);
		}
		Camera.main.farClipPlane = savePoint.m_cameraData.m_farClipPlane;
		Camera.main.fieldOfView = savePoint.m_cameraData.m_fiedOfView;
		if (savePoint.m_cameraData.m_isPlayParticle)
		{
			int playingParticleIndex = savePoint.m_cameraData.m_playingParticleIndex;
			PlayParticleByIndex(playingParticleIndex);
		}
		else
		{
			StopAllParticles();
		}
		SwitchCameraState(CameraState.NORMAL);
	}

	public RebirthCameraData GetRebirthData(Transform boxTriggerTransform)
	{
		RebirthCameraData rebirthCameraData = new RebirthCameraData();
		Vector3 localPosition = base.transform.localPosition;
		Vector3 localEulerAngles = base.transform.localEulerAngles;
		rebirthCameraData.CameraLocalPos = new Vector3(0f, localPosition.y, localPosition.z);
		rebirthCameraData.CameraLocalAngle = localEulerAngles;
		rebirthCameraData.RealCameraLocalPos = Camera.main.transform.localPosition;
		rebirthCameraData.CameraTargetLocalPos = CameraLookAt.Instance.target.transform.localPosition;
		rebirthCameraData.m_lookAtSlerpSpeed = CameraLookAt.Instance.m_lookAtSlerpSpeed;
		FollowData getFollowData = theCamera.GetFollowData;
		if (getFollowData != null)
		{
			rebirthCameraData.m_followData = getFollowData.Clone();
		}
		if (m_cameraAnimation.isPlaying)
		{
			string empty = string.Empty;
			foreach (AnimationState item in m_cameraAnimation)
			{
				if (m_cameraAnimation.IsPlaying(item.name))
				{
					empty = item.name;
					break;
				}
			}
			if (!string.IsNullOrEmpty(empty))
			{
				rebirthCameraData.m_animationName = empty;
				rebirthCameraData.m_animationTime = m_cameraAnimation[rebirthCameraData.m_animationName].time;
			}
		}
		else
		{
			Vector3 vector = base.transform.InverseTransformPoint(boxTriggerTransform.position);
			rebirthCameraData.CameraLocalPos = new Vector3(localPosition.x + vector.x, localPosition.y, localPosition.z);
		}
		rebirthCameraData.m_cameraState = m_cameraState;
		if (EnableCurvedWorld)
		{
			RebirthCurvedBendData rebirthCurvedBendData = new RebirthCurvedBendData();
			rebirthCurvedBendData.m_from = m_curvedBendFrom;
			rebirthCurvedBendData.m_to = m_curvedBendTo;
			rebirthCurvedBendData.m_current = GetCurvedBendCurrent;
			rebirthCurvedBendData.m_duration = m_curvedBendDuration;
			rebirthCurvedBendData.m_currentTime = m_currentCurvedBendTime;
			rebirthCurvedBendData.m_isPlaying = m_isPlayingCurvedBend;
			rebirthCameraData.m_curvedBendData = rebirthCurvedBendData;
		}
		rebirthCameraData.m_farClipPlane = Camera.main.farClipPlane;
		rebirthCameraData.m_fiedOfView = Camera.main.fieldOfView;
		rebirthCameraData.m_isPlayParticle = IsCameraParticlePlaying();
		rebirthCameraData.m_playingParticleIndex = currentParticleIndex;
		return rebirthCameraData;
	}

	private bool IsCameraParticlePlaying()
	{
		if (cameraParticles != null && cameraParticles.Length != 0)
		{
			return currentParticleIndex >= 0;
		}
		return false;
	}

	public void StartRunningForRebirth(RebirthBoxData savePoint)
	{
		if (savePoint == null || savePoint.m_cameraData == null)
		{
			return;
		}
		string animationName = savePoint.m_cameraData.m_animationName;
		if (!string.IsNullOrEmpty(animationName))
		{
			AnimationState animationState = m_cameraAnimation[animationName];
			if (animationState == null || animationState.clip == null)
			{
				animationState = LoadAnimationState(animationName);
			}
			animationState.time = savePoint.m_cameraData.m_animationTime;
			m_cameraAnimation.Play(animationState.name);
		}
		if (savePoint.m_cameraData.m_followData != null)
		{
			Mod.Event.FireNow(this, Mod.Reference.Acquire<FollowOpenEventArgs>().Initialize(savePoint.m_cameraData.m_followData));
		}
		if (EnableCurvedWorld)
		{
			RebirthCurvedBendData curvedBendData = savePoint.m_cameraData.m_curvedBendData;
			if (curvedBendData.m_isPlaying)
			{
				PlayCurvedBend(curvedBendData.m_from, curvedBendData.m_to, curvedBendData.m_duration, curvedBendData.m_currentTime);
			}
		}
	}

	private AnimationState LoadAnimationState(string animationClipName)
	{
		string path = string.Format("{0}{1}.anim", "Assets/_RS2Art/Res/Brush/Related/Animations/", animationClipName);
		AnimationClip relatedAnimationClipByPath = MapController.Instance.GetRelatedAnimationClipByPath(path);
		m_cameraAnimation.AddClip(relatedAnimationClipByPath, animationClipName);
		return m_cameraAnimation[animationClipName];
	}

	public void PlayStartAnim()
	{
		if (m_cameraAnimation.GetClipCount() > 0)
		{
			m_cameraAnimation.Play();
		}
		SwitchCameraState(CameraState.STARTANIM);
	}

	public void OnEnterLevel()
	{
	}

	public void SwitchCameraState(CameraState _state)
	{
		if (_state != CameraState.FOLLOW)
		{
			CloseFollow();
		}
		switch (_state)
		{
		}
		m_cameraState = _state;
	}

	private void OnPathToMoveByCameraHandler(object sender, EventArgs e)
	{
		PathToMoveByCameraTrigger.ElementData elementData = (m_pathToMoveByCameraData = ((PathToMoveByCameraEventArgs)e).m_elementData);
		m_bezierMover = new BezierMover();
		m_bezierMover.ResetData(elementData.m_pathData.m_bezierPositions);
		SwitchCameraState(CameraState.PATHTOMOVE);
	}

	private void OnPathToMoveByCameraUpData()
	{
		if (m_bezierMover != null)
		{
			m_deltaLocZ = Railway.theRailway.SpeedForward * Time.deltaTime * m_pathToMoveByCameraData.m_speed;
			if (!m_pathToMoveByCameraData.m_isLocalSpace)
			{
				m_beginPos = m_Camera.transform.position;
				m_isFinish = m_bezierMover.MoveForwardByZ(m_deltaLocZ, null, m_beginPos, ref m_targetPos, ref m_moveLocDir);
				Vector3 targetPos = m_targetPos;
				targetPos.z = m_Camera.transform.position.z;
				m_Camera.transform.position = targetPos;
			}
			else
			{
				m_beginPos = m_Camera.transform.localPosition;
				m_isFinish = m_bezierMover.MoveForwardByDis(m_deltaLocZ, m_beginPos, ref m_targetPos, ref m_moveLocDir);
				m_Camera.transform.localPosition = m_targetPos;
			}
			if (m_isFinish)
			{
				SwitchCameraState(CameraState.NORMAL);
			}
		}
	}

	public void ChangeStateToWinStop()
	{
		base.transform.parent = null;
		SwitchCameraState(CameraState.WINSTOP);
	}

	public void ChangeStateToWinStatic()
	{
		base.transform.parent = null;
		SwitchCameraState(CameraState.WINSTATIC);
	}

	public void CameraUpdate()
	{
		switch (m_cameraState)
		{
		case CameraState.NORMAL:
			m_cameraMover.UpdateCameraHorizontal();
			break;
		case CameraState.WINSTOP:
			GameWinCameraSlow();
			break;
		case CameraState.FOLLOW:
			m_cameraMover.UpdateCameraHorizontalAddFollow();
			break;
		case CameraState.PATHTOMOVE:
			OnPathToMoveByCameraUpData();
			break;
		default:
			m_cameraMover.UpdateCameraHorizontal();
			break;
		case CameraState.WAITFORQTE:
		case CameraState.QTE:
		case CameraState.NULL:
		case CameraState.CUTSCENE:
		case CameraState.STARTANIM:
		case CameraState.WINSTATIC:
			break;
		}
		OnCurvedBendDataUpdata();
	}

	public void OpenFollow(FollowData followData)
	{
		if (followData != null)
		{
			m_followData = followData;
			SwitchCameraState(CameraState.FOLLOW);
		}
	}

	public void CloseFollow()
	{
		m_followData = null;
	}

	public void ResetInputParam(float target = 0.25f)
	{
		m_cameraMover.ResetInputParam(target);
	}

	public void RecordingReset(Vector3 localPosition)
	{
		base.transform.localPosition = localPosition;
		m_cameraMover.Reset();
		gameLoseSlowTimer = gameLoseSlowTime;
		gameWinSlowTimer = gameWinSlowTime;
		Camera.main.transform.parent = base.transform;
		Camera.main.transform.localPosition = Vector3.zero;
		Camera.main.transform.localEulerAngles = Vector3.zero;
		SwitchCameraState(CameraState.NORMAL);
	}

	public void PlayAnim(string animName)
	{
		SwitchCameraState(CameraState.CUTSCENE);
		cameraAnimator.Play(animName);
	}

	public void GameLoseCameraSlow()
	{
		if ((bool)m_cameraAnimation && m_cameraAnimation.isPlaying)
		{
			m_cameraAnimation.Stop();
		}
		float animPercent;
		if (!FloatAnim.AnimElapsed(ref gameLoseSlowTimer, gameLoseSlowTime, out animPercent, true) && IfDieShakeCamera)
		{
			float num = 0.2f * animPercent;
			Vector3 vector = base.transform.forward * num;
			vector.y = 0f;
			base.transform.position += vector;
		}
	}

	public void GameWinCameraSlow()
	{
		float animPercent;
		if (!FloatAnim.AnimElapsed(ref gameWinSlowTimer, gameWinSlowTime, out animPercent, true))
		{
			float num = 0.13f * animPercent;
			Vector3 vector = base.transform.forward * num;
			vector.y = 0f;
			base.transform.position += vector;
		}
	}

	private void OnFollowOpenHandler(object sender, EventArgs e)
	{
		FollowOpenEventArgs followOpenEventArgs = e as FollowOpenEventArgs;
		OpenFollow(followOpenEventArgs.m_followData);
	}

	private void OnFollowCloseHandler(object sender, EventArgs e)
	{
		CloseFollow();
	}

	private void OnChangeCameraParticle(object sender, EventArgs e)
	{
		int @int = Mod.Setting.GetInt("Setting.QualityLevel");
		if (DeviceManager.Instance.IsLowEndQualityLevel(@int))
		{
			return;
		}
		CameraParticlePlayEventArgs cameraParticlePlayEventArgs = e as CameraParticlePlayEventArgs;
		if (cameraParticlePlayEventArgs != null)
		{
			if (cameraParticlePlayEventArgs.IfPlay)
			{
				PlayParticleByIndex(cameraParticlePlayEventArgs.ParticleIndex);
			}
			else
			{
				StopAllParticles();
			}
		}
	}

	private void OnChangeCameraParticleByName(object sender, EventArgs e)
	{
		if (!(sender as BaseElement == null))
		{
			CameraParticlePlayByNameEventArgs cameraParticlePlayByNameEventArgs = e as CameraParticlePlayByNameEventArgs;
			PlayEffectByData(cameraParticlePlayByNameEventArgs.data);
		}
	}

	private void PlayEffectByData(ChangeCameraEffectByNameTrigger.Data data)
	{
		if (effectRoot == null)
		{
			return;
		}
		ChangeCameraEffectByNameTrigger.ShowData[] datas = data.datas;
		for (int i = 0; i < datas.Length; i++)
		{
			ChangeCameraEffectByNameTrigger.ShowData showData = datas[i];
			string n = showData.name;
			bool isShow = showData.isShow;
			Transform transform = effectRoot.Find(n);
			ParticleSystem component = transform.GetComponent<ParticleSystem>();
			if (component == null)
			{
				continue;
			}
			if (isShow)
			{
				component.Play();
				Animation componentInChildren = transform.GetComponentInChildren<Animation>();
				if ((bool)componentInChildren)
				{
					componentInChildren.Play();
				}
			}
			else
			{
				component.Stop();
			}
		}
	}

	private void PlayParticleByIndex(int index)
	{
		if (cameraParticles == null)
		{
			return;
		}
		for (int i = 0; i < cameraParticles.Length; i++)
		{
			if (index == i)
			{
				PlayParticles(cameraParticles[i]);
				currentParticleIndex = index;
			}
			else
			{
				PlayParticles(cameraParticles[i], false);
			}
		}
	}

	private void StopParticleByIndex(int index, bool ifAll = false)
	{
		if (ifAll)
		{
			PlayParticles(cameraParticles[index], false);
		}
		else
		{
			StopAllParticles();
		}
		currentParticleIndex = -1;
	}

	private void PlayParticles(ParticleSystem[] particles, bool ifPlay = true)
	{
		if (particles == null || particles.Length == 0)
		{
			return;
		}
		for (int i = 0; i < particles.Length; i++)
		{
			if (ifPlay)
			{
				particles[i].Play();
			}
			else
			{
				particles[i].Stop();
			}
		}
	}

	private void StopAllParticles()
	{
		if (cameraParticles != null)
		{
			for (int i = 0; i < cameraParticles.Length; i++)
			{
				PlayParticles(cameraParticles[i], false);
			}
		}
		currentParticleIndex = -1;
		StopEffectRoot();
	}

	private void StopEffectRoot()
	{
		if (effectRoot == null)
		{
			return;
		}
		int childCount = effectRoot.childCount;
		for (int i = 0; i < childCount; i++)
		{
			ParticleSystem component = effectRoot.GetChild(i).GetComponent<ParticleSystem>();
			if (component != null)
			{
				component.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
			}
		}
	}

	public void PlayCurvedBend(CurvedBendData from, CurvedBendData to, float duration, float time)
	{
		m_curvedBendFrom = from;
		m_curvedBendTo = to;
		m_curvedBendDuration = duration;
		m_currentCurvedBendTime = time;
		m_isPlayingCurvedBend = true;
	}

	public void PlayCurvedBend(CurvedBendData to, float duration, float time)
	{
		m_curvedBendFrom = GetCurvedBendCurrent;
		m_curvedBendTo = to;
		m_curvedBendDuration = duration;
		m_currentCurvedBendTime = time;
		m_isPlayingCurvedBend = true;
	}

	public void StopCurvedBend()
	{
		m_isPlayingCurvedBend = false;
	}

	private void OnCurvedBendDataUpdata()
	{
		if (EnableCurvedWorld && m_isPlayingCurvedBend)
		{
			m_currentCurvedBendTime += Time.deltaTime;
			m_curvedBendProgress = ((m_curvedBendDuration != 0f) ? (m_currentCurvedBendTime / m_curvedBendDuration) : 1f);
			m_curvedWorldController._V_CW_Bend_X = Mathf.Lerp(m_curvedBendFrom.m_xAxisBendSize, m_curvedBendTo.m_xAxisBendSize, m_curvedBendProgress);
			m_curvedWorldController._V_CW_Bias_X = Mathf.Lerp(m_curvedBendFrom.m_xBias, m_curvedBendTo.m_xBias, m_curvedBendProgress);
			m_curvedWorldController._V_CW_Bend_Y = Mathf.Lerp(m_curvedBendFrom.m_yAxisBendSize, m_curvedBendTo.m_yAxisBendSize, m_curvedBendProgress);
			m_curvedWorldController._V_CW_Bias_Y = Mathf.Lerp(m_curvedBendFrom.m_yBias, m_curvedBendTo.m_yBias, m_curvedBendProgress);
			if (Mathf.Abs(m_curvedWorldController._V_CW_Bend_X - m_curvedBendTo.m_xAxisBendSize) <= 0.04f && Mathf.Abs(m_curvedWorldController._V_CW_Bias_X - m_curvedBendTo.m_xBias) <= 0.04f && Mathf.Abs(m_curvedWorldController._V_CW_Bend_Y - m_curvedBendTo.m_yAxisBendSize) <= 0.04f && Mathf.Abs(m_curvedWorldController._V_CW_Bias_Y - m_curvedBendTo.m_yBias) <= 0.04f)
			{
				m_curvedWorldController._V_CW_Bend_X = m_curvedBendTo.m_xAxisBendSize;
				m_curvedWorldController._V_CW_Bias_X = m_curvedBendTo.m_xBias;
				m_curvedWorldController._V_CW_Bend_Y = m_curvedBendTo.m_yAxisBendSize;
				m_curvedWorldController._V_CW_Bias_Y = m_curvedBendTo.m_yBias;
				m_isPlayingCurvedBend = false;
			}
		}
	}

	private void SetCurvedBend(CurvedBendData data)
	{
		if (EnableCurvedWorld)
		{
			m_curvedWorldController._V_CW_Bend_X = data.m_xAxisBendSize;
			m_curvedWorldController._V_CW_Bias_X = data.m_xBias;
			m_curvedWorldController._V_CW_Bend_Y = data.m_yAxisBendSize;
			m_curvedWorldController._V_CW_Bias_Y = data.m_yBias;
		}
	}

	public object GetOriginRebirthData(object obj = null)
	{
		RD_CameraController_DATA rD_CameraController_DATA = new RD_CameraController_DATA();
		rD_CameraController_DATA.m_cameraState = m_cameraState;
		rD_CameraController_DATA.ifPlayParticles = IsCameraParticlePlaying();
		rD_CameraController_DATA.currentParticleIndex = currentParticleIndex;
		rD_CameraController_DATA.ifFollow = m_followData != null;
		rD_CameraController_DATA.m_followData = m_followData;
		rD_CameraController_DATA.m_isPlayingCurvedBend = m_isPlayingCurvedBend;
		rD_CameraController_DATA.m_curvedBendFrom = m_curvedBendFrom;
		rD_CameraController_DATA.m_curvedBendTo = m_curvedBendTo;
		rD_CameraController_DATA.m_curvedBendDuration = m_curvedBendDuration;
		rD_CameraController_DATA.m_currentCurvedBendTime = m_currentCurvedBendTime;
		rD_CameraController_DATA.m_curvedBendProgress = m_curvedBendProgress;
		rD_CameraController_DATA.mainCamTransData = Camera.main.transform.GetTransData();
		rD_CameraController_DATA.farClipPlane = Camera.main.farClipPlane;
		rD_CameraController_DATA.fieldOfView = Camera.main.fieldOfView;
		rD_CameraController_DATA.cameraMover = m_cameraMover.GetCameraMoverData();
		string animName = string.Empty;
		float animTime = 0f;
		if (TryGetCurrentAnimInfo(m_cameraAnimation, ref animName, ref animTime))
		{
			RD_ElementAnim_DATA rD_ElementAnim_DATA = (rD_CameraController_DATA.cameraAnimation = m_cameraAnimation.GetAnimData(animName));
		}
		if (rD_CameraController_DATA.ifPlayParticles)
		{
			ParticleSystem[] particles = cameraParticles[rD_CameraController_DATA.currentParticleIndex];
			rD_CameraController_DATA.cameraParticlesData = particles.GetParticlesData();
		}
		rD_CameraController_DATA.effectRootData = effectRoot.GetParticlesData();
		rD_CameraController_DATA.effectAnimations = effectRoot.GetAnimationData();
		rD_CameraController_DATA.targetTransData = CameraLookAt.Instance.target.transform.GetTransData();
		rD_CameraController_DATA.m_lookAtSlerpSpeed = CameraLookAt.Instance.m_lookAtSlerpSpeed;
		if (EnableCurvedWorld)
		{
			rD_CameraController_DATA.currentCurveBendData = GetCurvedBendCurrent;
		}
		return JsonUtility.ToJson(rD_CameraController_DATA);
	}

	public void SetOriginRebirthData(object dataInfo)
	{
		ifRebirthing = true;
		CloseFollow();
		RD_CameraController_DATA rD_CameraController_DATA = (m_rebirthData = JsonUtility.FromJson<RD_CameraController_DATA>(dataInfo as string));
		m_cameraState = m_rebirthData.m_cameraState;
		currentParticleIndex = m_rebirthData.currentParticleIndex;
		if (m_rebirthData.ifFollow)
		{
			m_followData = m_rebirthData.m_followData;
		}
		m_isPlayingCurvedBend = m_rebirthData.m_isPlayingCurvedBend;
		m_curvedBendFrom = m_rebirthData.m_curvedBendFrom;
		m_curvedBendTo = m_rebirthData.m_curvedBendTo;
		m_curvedBendDuration = m_rebirthData.m_curvedBendDuration;
		m_currentCurvedBendTime = m_rebirthData.m_currentCurvedBendTime;
		m_curvedBendProgress = m_rebirthData.m_curvedBendProgress;
		m_cameraMover.SetCameraMoverData(m_rebirthData.cameraMover);
		Camera main = Camera.main;
		main.farClipPlane = m_rebirthData.farClipPlane;
		main.fieldOfView = m_rebirthData.fieldOfView;
		main.transform.SetTransData(m_rebirthData.mainCamTransData);
		RD_ElementAnim_DATA cameraAnimation = m_rebirthData.cameraAnimation;
		if (cameraAnimation != null && cameraAnimation.ifPlaying)
		{
			string animaName = cameraAnimation.animaName;
			AnimationClip clip = m_cameraAnimation.clip;
			if (clip != null)
			{
				if (clip.name != animaName)
				{
					LoadAnimationState(animaName);
				}
			}
			else
			{
				LoadAnimationState(animaName);
			}
			m_cameraAnimation.SetAnimData(cameraAnimation, ProcessState.Pause);
		}
		if (m_rebirthData.ifPlayParticles)
		{
			PlayParticleByIndex(m_rebirthData.currentParticleIndex);
		}
		else
		{
			StopAllParticles();
		}
		if ((bool)effectRoot)
		{
			int childCount = effectRoot.childCount;
			for (int i = 0; i < childCount; i++)
			{
				Transform child = effectRoot.GetChild(i);
				Animation componentInChildren = child.GetComponentInChildren<Animation>();
				if ((bool)componentInChildren)
				{
					componentInChildren.SetAnimData(m_rebirthData.effectAnimations[i], ProcessState.UnPause);
				}
				ParticleSystem component = child.GetComponent<ParticleSystem>();
				if ((bool)component)
				{
					component.SetParticleData(m_rebirthData.effectRootData[i], ProcessState.UnPause);
				}
			}
		}
		if (EnableCurvedWorld)
		{
			SetCurvedBend(m_rebirthData.currentCurveBendData);
		}
		m_cameraMover.UpdateCameraHorizontal();
		CameraLookAt.Instance.target.transform.SetTransData(m_rebirthData.targetTransData);
		CameraLookAt.Instance.m_lookAtSlerpSpeed = m_rebirthData.m_lookAtSlerpSpeed;
		OpenFollow(m_followData);
		if (EnableCurvedWorld)
		{
			SetCurvedBend(m_rebirthData.currentCurveBendData);
			if (m_isPlayingCurvedBend)
			{
				PlayCurvedBend(m_curvedBendFrom, m_curvedBendTo, m_curvedBendDuration, m_currentCurvedBendTime);
			}
		}
	}

	public void StartRunByOriginRebirthData(object dataInfo)
	{
		ifRebirthing = false;
		if (m_rebirthData != null)
		{
			m_cameraAnimation.SetAnimData(m_rebirthData.cameraAnimation, ProcessState.UnPause);
		}
		m_rebirthData = null;
	}

	public byte[] GetOriginRebirthBsonData(object obj = null)
	{
		byte[] array = new byte[0];
		RD_CameraController_DATA rD_CameraController_DATA = new RD_CameraController_DATA();
		rD_CameraController_DATA.m_cameraState = m_cameraState;
		rD_CameraController_DATA.ifPlayParticles = IsCameraParticlePlaying();
		rD_CameraController_DATA.currentParticleIndex = currentParticleIndex;
		rD_CameraController_DATA.ifFollow = m_followData != null;
		rD_CameraController_DATA.m_followData = m_followData;
		rD_CameraController_DATA.m_isPlayingCurvedBend = m_isPlayingCurvedBend;
		rD_CameraController_DATA.m_curvedBendFrom = m_curvedBendFrom;
		rD_CameraController_DATA.m_curvedBendTo = m_curvedBendTo;
		rD_CameraController_DATA.m_curvedBendDuration = m_curvedBendDuration;
		rD_CameraController_DATA.m_currentCurvedBendTime = m_currentCurvedBendTime;
		rD_CameraController_DATA.m_curvedBendProgress = m_curvedBendProgress;
		rD_CameraController_DATA.mainCamTransData = Camera.main.transform.GetTransData();
		rD_CameraController_DATA.farClipPlane = Camera.main.farClipPlane;
		rD_CameraController_DATA.fieldOfView = Camera.main.fieldOfView;
		rD_CameraController_DATA.cameraMover = m_cameraMover.GetCameraMoverData();
		string animName = string.Empty;
		float animTime = 0f;
		if (TryGetCurrentAnimInfo(m_cameraAnimation, ref animName, ref animTime))
		{
			RD_ElementAnim_DATA rD_ElementAnim_DATA = (rD_CameraController_DATA.cameraAnimation = m_cameraAnimation.GetAnimData(animName));
		}
		if (rD_CameraController_DATA.ifPlayParticles)
		{
			ParticleSystem[] particles = cameraParticles[rD_CameraController_DATA.currentParticleIndex];
			rD_CameraController_DATA.cameraParticlesData = particles.GetParticlesData();
		}
		rD_CameraController_DATA.effectRootData = effectRoot.GetParticlesData();
		rD_CameraController_DATA.effectAnimations = effectRoot.GetAnimationData();
		rD_CameraController_DATA.targetTransData = CameraLookAt.Instance.target.transform.GetTransData();
		rD_CameraController_DATA.m_lookAtSlerpSpeed = CameraLookAt.Instance.m_lookAtSlerpSpeed;
		if (EnableCurvedWorld)
		{
			rD_CameraController_DATA.currentCurveBendData = GetCurvedBendCurrent;
		}
		return Bson.ToBson(rD_CameraController_DATA);
	}

	public void SetOriginRebirthBsonData(byte[] dataInfo)
	{
		ifRebirthing = true;
		CloseFollow();
		RD_CameraController_DATA rD_CameraController_DATA = (m_rebirthData = Bson.ToObject<RD_CameraController_DATA>(dataInfo));
		m_cameraState = m_rebirthData.m_cameraState;
		currentParticleIndex = m_rebirthData.currentParticleIndex;
		if (m_rebirthData.ifFollow)
		{
			m_followData = m_rebirthData.m_followData;
		}
		m_isPlayingCurvedBend = m_rebirthData.m_isPlayingCurvedBend;
		m_curvedBendFrom = m_rebirthData.m_curvedBendFrom;
		m_curvedBendTo = m_rebirthData.m_curvedBendTo;
		m_curvedBendDuration = m_rebirthData.m_curvedBendDuration;
		m_currentCurvedBendTime = m_rebirthData.m_currentCurvedBendTime;
		m_curvedBendProgress = m_rebirthData.m_curvedBendProgress;
		m_cameraMover.SetCameraMoverData(m_rebirthData.cameraMover);
		Camera main = Camera.main;
		main.farClipPlane = m_rebirthData.farClipPlane;
		main.fieldOfView = m_rebirthData.fieldOfView;
		main.transform.SetTransData(m_rebirthData.mainCamTransData);
		RD_ElementAnim_DATA cameraAnimation = m_rebirthData.cameraAnimation;
		if (cameraAnimation != null && cameraAnimation.ifPlaying)
		{
			string animaName = cameraAnimation.animaName;
			AnimationClip clip = m_cameraAnimation.clip;
			if (clip != null)
			{
				if (clip.name != animaName)
				{
					LoadAnimationState(animaName);
				}
			}
			else
			{
				LoadAnimationState(animaName);
			}
			m_cameraAnimation.SetAnimData(cameraAnimation, ProcessState.Pause);
		}
		if (m_rebirthData.ifPlayParticles)
		{
			PlayParticleByIndex(m_rebirthData.currentParticleIndex);
		}
		else
		{
			StopAllParticles();
		}
		if ((bool)effectRoot)
		{
			int childCount = effectRoot.childCount;
			for (int i = 0; i < childCount; i++)
			{
				Transform child = effectRoot.GetChild(i);
				Animation componentInChildren = child.GetComponentInChildren<Animation>();
				if ((bool)componentInChildren)
				{
					componentInChildren.SetAnimData(m_rebirthData.effectAnimations[i], ProcessState.UnPause);
				}
				ParticleSystem component = child.GetComponent<ParticleSystem>();
				if ((bool)component)
				{
					component.SetParticleData(m_rebirthData.effectRootData[i], ProcessState.UnPause);
				}
			}
		}
		if (EnableCurvedWorld)
		{
			SetCurvedBend(m_rebirthData.currentCurveBendData);
		}
		m_cameraMover.UpdateCameraHorizontal();
		CameraLookAt.Instance.target.transform.SetTransData(m_rebirthData.targetTransData);
		CameraLookAt.Instance.m_lookAtSlerpSpeed = m_rebirthData.m_lookAtSlerpSpeed;
		OpenFollow(m_followData);
		if (EnableCurvedWorld)
		{
			SetCurvedBend(m_rebirthData.currentCurveBendData);
			if (m_isPlayingCurvedBend)
			{
				PlayCurvedBend(m_curvedBendFrom, m_curvedBendTo, m_curvedBendDuration, m_currentCurvedBendTime);
			}
		}
	}

	public void StartRunByOriginRebirthBsonData(byte[] dataInfo)
	{
		ifRebirthing = false;
		if (m_rebirthData != null)
		{
			m_cameraAnimation.SetAnimData(m_rebirthData.cameraAnimation, ProcessState.UnPause);
		}
		m_rebirthData = null;
	}

	private bool TryGetCurrentAnimInfo(Animation anim, ref string animName, ref float animTime)
	{
		if (anim.isPlaying)
		{
			foreach (AnimationState item in anim)
			{
				if (anim.IsPlaying(item.name))
				{
					animName = item.name;
					break;
				}
			}
			animTime = anim[animName].time;
		}
		return anim.isPlaying;
	}

	private void ResetCameraX(params Transform[] trans)
	{
		for (int i = 0; i < trans.Length; i++)
		{
			Vector3 localPosition = trans[i].localPosition;
			localPosition.x = 0f;
			trans[i].localPosition = localPosition;
		}
	}
}
