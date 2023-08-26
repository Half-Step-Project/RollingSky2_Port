using System;
using Foundation;
using UnityEngine;

public class NormalPathVehicle : BaseVehicle
{
	public enum State
	{
		WaitPath,
		Path,
		Wait,
		Enter,
		Stay,
		Exit
	}

	[Serializable]
	public struct Data
	{
		[Header("waitPath -> OnWaitPath")]
		[Header("Path     -> OnPath")]
		[Header("Wait     -> OnWait")]
		[Header("Enter    -> OnEnter")]
		[Header("Stay     -> OnStay")]
		[Header("Exit     -> OnExit")]
		[Header("")]
		[Header("路径数据")]
		public PathToMoveData m_pathData;

		public float m_waitDistance;

		public bool m_isLookAt;

		public float m_speed;

		[Header("基础数据")]
		public float CameraTargetScaler;

		public float BallSlideSpeed;

		public float InputNormalizeSpeed;

		public float InputSensitivity;

		public bool m_isListenStay;

		public static Data DefaultValue
		{
			get
			{
				Data result = default(Data);
				result.m_pathData = new PathToMoveData
				{
					m_positions = new Vector3[4]
					{
						new Vector3(0f, 0f, -1f),
						new Vector3(0f, 1f, -0.7f),
						new Vector3(0f, 1f, -0.3f),
						new Vector3(0f, 0f, 0f)
					},
					m_smooth = 20
				};
				result.m_waitDistance = -8f;
				result.m_isLookAt = true;
				result.m_speed = 2.5f;
				result.CameraTargetScaler = 0.25f;
				result.BallSlideSpeed = 2f;
				result.InputNormalizeSpeed = 2f;
				result.InputSensitivity = 1.2f;
				result.m_isListenStay = false;
				return result;
			}
		}
	}

	public class RebirthData
	{
		public RD_ElementTransform_DATA tran;

		public RD_ElementAnimator_DATA anim;

		public RD_ElementTransform_DATA moveTran;

		public RD_ElementParticle_DATA[] particles;

		public State state;

		public bool ifIgnoreRoleCollide;

		public RD_BezierMove_Data bezier;
	}

	public Animator m_animator;

	public GameObject m_moveObj;

	public State m_state;

	public Data m_data = Data.DefaultValue;

	private BezierMover m_bezierMover;

	[SerializeField]
	private float m_currentDistance;

	private float m_deltaLocZ;

	private Vector3 m_beginPos = Vector3.zero;

	private Vector3 m_targetPos = Vector3.zero;

	private Vector3 m_moveLocDir = Vector3.forward;

	private bool ifIgnoreRoleCollide;

	private float deltaX;

	private bool m_isParticlePlaying;

	private RebirthData m_rebirthData;

	public bool IsParticlePlaying
	{
		get
		{
			return m_isParticlePlaying;
		}
	}

	public override bool IfRebirthRecord
	{
		get
		{
			return true;
		}
	}

	public override void Initialize()
	{
		base.Initialize();
		m_state = State.WaitPath;
		m_animator = base.gameObject.GetComponentInChildren<Animator>();
		m_moveObj = base.gameObject.transform.Find("model").gameObject;
		m_data.m_pathData.RefreshBezierPositions(base.gameObject);
		m_bezierMover = new BezierMover(m_data.m_pathData.m_bezierPositions);
		if (m_moveObj != null)
		{
			m_moveObj.transform.position = m_data.m_pathData.m_bezierPositions[0];
		}
		PlayAnimation(State.WaitPath);
		ifIgnoreRoleCollide = false;
	}

	public override void UpdateElement()
	{
		base.UpdateElement();
		switch (m_state)
		{
		case State.WaitPath:
			OnWaitPathUpdate();
			break;
		case State.Path:
			OnPathUpdate();
			break;
		case State.Stay:
			OnStayUpdate();
			break;
		case State.Wait:
		case State.Enter:
		case State.Exit:
			break;
		}
	}

	public override void ResetElement()
	{
		base.ResetElement();
		if (m_moveObj != null)
		{
			m_moveObj.transform.position = m_data.m_pathData.m_bezierPositions[0];
		}
		if (m_animator != null)
		{
			m_animator.StopPlayback();
		}
		ifIgnoreRoleCollide = false;
	}

	public override void DepartFromBall(BaseRole ball, bool ifDestroy)
	{
		if (m_state == State.Stay)
		{
			base.DepartFromBall(ball, ifDestroy);
			OnExit();
		}
	}

	public override void GiveRebirthTo(BaseRole ball)
	{
		ifIgnoreRoleCollide = true;
		base.TriggerEnter(ball);
		ResetInputParam();
		AddToBall(ball);
		m_state = State.Stay;
	}

	public override void ForceSetTrans()
	{
		base.transform.eulerAngles = Vector3.zero;
		Transform parent = base.transform.parent;
		base.transform.parent = BaseRole.theBall.transform;
		base.transform.localPosition = Vector3.zero;
		base.transform.parent = parent;
	}

	public override void OnRaycastHit(bool isRaycast, RaycastHit hit, BaseRole ball)
	{
		base.OnRaycastHit(isRaycast, hit, ball);
		if (!isRaycast)
		{
			if (IsParticlePlaying)
			{
				StopParticle();
			}
		}
		else if (!IsParticlePlaying)
		{
			PlayParticle();
		}
	}

	public override void TriggerEnter(BaseRole ball)
	{
		if (!ifIgnoreRoleCollide && m_state == State.Wait)
		{
			OnEnter(ball);
		}
	}

	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		if (m_data.m_pathData.m_positions != null && m_data.m_pathData.m_positions.Length >= 4 && (m_data.m_pathData.m_positions.Length - 1) % 3 == 0)
		{
			ThreeBezier.DrawGizmos(base.gameObject, m_data.m_pathData.m_positions, m_data.m_pathData.m_smooth);
		}
	}

	private void OnWaitPathUpdate()
	{
		if (Application.isPlaying)
		{
			m_currentDistance = base.transform.parent.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z;
		}
		if (m_currentDistance >= m_data.m_waitDistance)
		{
			m_state = State.Path;
			PlayAnimation(State.Path);
		}
	}

	private void OnPathUpdate()
	{
		if (m_bezierMover != null)
		{
			m_deltaLocZ = Railway.theRailway.SpeedForward * Time.deltaTime * m_data.m_speed;
			m_beginPos = m_moveObj.transform.position;
			bool num = m_bezierMover.MoveForwardByDis(m_deltaLocZ, m_beginPos, ref m_targetPos, ref m_moveLocDir);
			m_moveObj.transform.position = m_targetPos;
			if (m_data.m_isLookAt)
			{
				m_moveObj.transform.forward = m_moveLocDir;
			}
			if (num)
			{
				OnWait();
			}
		}
	}

	private void OnWait()
	{
		m_state = State.Wait;
		if (m_moveObj != null)
		{
			m_moveObj.transform.position = m_data.m_pathData.m_bezierPositions[m_data.m_pathData.m_bezierPositions.Length - 1];
		}
		PlayAnimation(State.Wait);
	}

	private void OnEnter(BaseRole ball)
	{
		m_state = State.Enter;
		if (m_moveObj != null)
		{
			m_moveObj.transform.position = base.transform.position;
		}
		ResetInputParam();
		AddToBall(ball);
		BaseRole.theBall.BeginCloseTrail();
		ball.TriggerRolePlayAnim(BaseRole.AnimType.JumpOnBoatStandState);
		PlayParticle();
		if (m_data.m_isListenStay)
		{
			PlayAnimation(State.Stay);
		}
		else
		{
			PlayAnimation(State.Enter);
		}
		m_state = State.Stay;
	}

	private void OnStayUpdate()
	{
		BaseRole theBall = BaseRole.theBall;
		float inputPercent = InputController.InputPercent;
		float num = theBall.BallMoveOffset();
		if (!theBall.IfPlayBoatStandRoll())
		{
			if (Input.GetMouseButton(0))
			{
				float num2 = Mathf.Abs(inputPercent - num);
				if (inputPercent > num)
				{
					deltaX = num2 * theBall.SlideSpeed;
				}
				else if (inputPercent < num)
				{
					deltaX = num2 * (0f - theBall.SlideSpeed);
				}
				else
				{
					deltaX = 0f;
				}
			}
			else
			{
				deltaX /= 1.2f;
			}
		}
		else
		{
			deltaX = 0f;
		}
		Vector3 localEulerAngles = base.transform.localEulerAngles;
		localEulerAngles.z = Mathf.Clamp(deltaX * -4f, -30f, 30f);
		base.transform.localEulerAngles = localEulerAngles;
	}

	private void OnExit()
	{
		m_state = State.Exit;
		PlayAnimation(State.Exit);
		StopParticle();
	}

	private void ResetInputParam()
	{
		BaseRole.theBall.ResetInputParam(m_data.BallSlideSpeed);
		CameraController.theCamera.ResetInputParam(m_data.CameraTargetScaler);
		InputController.instance.ResetInputParam(m_data.InputNormalizeSpeed, m_data.InputSensitivity);
	}

	protected override void PlayParticle()
	{
		base.PlayParticle();
		m_isParticlePlaying = true;
	}

	protected override void StopParticle()
	{
		base.StopParticle();
		m_isParticlePlaying = false;
	}

	private void PlayAnimation(State state)
	{
		string text = string.Empty;
		switch (state)
		{
		case State.WaitPath:
			text = "OnWaitPath";
			break;
		case State.Path:
			text = "OnPath";
			break;
		case State.Wait:
			text = "OnWait";
			break;
		case State.Enter:
			text = "OnEnter";
			break;
		case State.Stay:
			text = "OnStay";
			break;
		case State.Exit:
			text = "OnExit";
			break;
		}
		if (!string.IsNullOrEmpty(text) && m_animator != null)
		{
			m_animator.Play(text, 0, 0f);
		}
	}

	public override void Read(string info)
	{
		m_data = JsonUtility.FromJson<Data>(info);
	}

	public override string Write()
	{
		return JsonUtility.ToJson(m_data);
	}

	public override void ReadBytes(byte[] bytes)
	{
		m_data = Bson.ToObject<Data>(bytes);
	}

	public override byte[] WriteBytes()
	{
		return Bson.ToBson(m_data);
	}

	public override void SetDefaultValue(object[] objs)
	{
		if (objs != null && objs.Length != 0)
		{
			m_data = (Data)objs[0];
		}
	}

	[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
	public override void RebirthReadData(object rd_data)
	{
		RebirthData rebirthData = JsonUtility.FromJson<RebirthData>(rd_data as string);
		m_state = rebirthData.state;
		base.gameObject.transform.SetTransData(rebirthData.tran);
		if ((bool)m_moveObj)
		{
			m_moveObj.transform.SetTransData(rebirthData.moveTran);
		}
		m_animator.SetAnimData(rebirthData.anim, ProcessState.Pause);
		particles.SetParticlesData(rebirthData.particles, ProcessState.Pause);
		m_bezierMover.SetBezierData(rebirthData.bezier);
		m_rebirthData = rebirthData;
		ifIgnoreRoleCollide = false;
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override object RebirthWriteData()
	{
		RebirthData rebirthData = new RebirthData();
		rebirthData.tran = base.gameObject.transform.GetTransData();
		if ((bool)m_moveObj)
		{
			rebirthData.moveTran = m_moveObj.transform.GetTransData();
		}
		rebirthData.anim = m_animator.GetAnimData();
		rebirthData.particles = particles.GetParticlesData();
		rebirthData.state = m_state;
		rebirthData.bezier = m_bezierMover.GetBezierData();
		return JsonUtility.ToJson(rebirthData);
	}

	[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
	public override void RebirthStartGame(object rd_data)
	{
		if (m_rebirthData != null)
		{
			m_animator.SetAnimData(m_rebirthData.anim, ProcessState.Pause);
			particles.SetParticlesData(m_rebirthData.particles, ProcessState.Pause);
			m_rebirthData = null;
		}
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
		RebirthData rebirthData = Bson.ToObject<RebirthData>(rd_data);
		m_state = rebirthData.state;
		base.gameObject.transform.SetTransData(rebirthData.tran);
		if ((bool)m_moveObj)
		{
			m_moveObj.transform.SetTransData(rebirthData.moveTran);
		}
		m_animator.SetAnimData(rebirthData.anim, ProcessState.Pause);
		particles.SetParticlesData(rebirthData.particles, ProcessState.Pause);
		m_bezierMover.SetBezierData(rebirthData.bezier);
		m_rebirthData = rebirthData;
		ifIgnoreRoleCollide = false;
	}

	public override byte[] RebirthWriteByteData()
	{
		RebirthData rebirthData = new RebirthData();
		rebirthData.tran = base.gameObject.transform.GetTransData();
		if ((bool)m_moveObj)
		{
			rebirthData.moveTran = m_moveObj.transform.GetTransData();
		}
		rebirthData.anim = m_animator.GetAnimData();
		rebirthData.particles = particles.GetParticlesData();
		rebirthData.state = m_state;
		rebirthData.bezier = m_bezierMover.GetBezierData();
		return Bson.ToBson(rebirthData);
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
		if (m_rebirthData != null)
		{
			m_animator.SetAnimData(m_rebirthData.anim, ProcessState.Pause);
			particles.SetParticlesData(m_rebirthData.particles, ProcessState.Pause);
			m_rebirthData = null;
		}
	}

	public override void RebirthResetByRole(BaseRole role)
	{
		ifIgnoreRoleCollide = true;
		if (m_moveObj != null)
		{
			m_moveObj.transform.position = base.transform.position;
		}
		base.TriggerEnter(role);
		ResetInputParam();
		AddToBall(role);
		m_state = State.Stay;
	}
}
