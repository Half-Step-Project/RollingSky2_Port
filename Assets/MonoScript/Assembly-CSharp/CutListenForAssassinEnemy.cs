using System;
using Foundation;
using RS2;
using UnityEngine;

public class CutListenForAssassinEnemy : BaseEnemy, IAttackable
{
	public enum State
	{
		Hide,
		Show,
		Delay,
		BeCut,
		Knock,
		Death,
		End
	}

	[Serializable]
	public struct Data
	{
		[Header("Hide animatorStateName ----------------hide")]
		[Header("Show animatorStateName ----------------show")]
		[Header("Death animatorStateName ----------------death")]
		[Header("Knock animatorStateName ----------------knock")]
		public int groupID;

		public float showDistance;

		public float delay;

		public float dissolveTime;

		public PathToMoveData pathData;

		public float speed;

		public static Data DefaultValue
		{
			get
			{
				Data result = default(Data);
				result.groupID = 1;
				result.showDistance = -30f;
				result.delay = 0.5f;
				result.dissolveTime = 1f;
				result.pathData = PathToMoveData.DefaultValue;
				result.speed = 1f;
				return result;
			}
		}
	}

	[Serializable]
	public class RebirthData
	{
		public State state;

		public bool isSensoreStay;

		public RD_ElementTransform_DATA trans;

		public RD_ElementTransform_DATA moveObj;

		public RD_ElementAnimator_DATA animator;

		public RD_ElementParticle_DATA becutEffect;

		public float time;

		public float dissolveTime;

		public RD_BezierMove_Data bezier;

		public bool isFinished;
	}

	private const string HIDEANIMNAME = "hide";

	private const string SHOWANIMNAME = "show";

	private const string DEATHANIMNAME = "death";

	private const string KNOCKANIMNAME = "knock";

	[Label]
	public State mState;

	public DistancSensore mDistancSensore;

	private BezierMover m_bezierMover;

	public Animator mAnimator;

	public GameObject mMoveObj;

	public ParticleSystem mBecutEffect;

	public Renderer[] mRenderers;

	public Data mData = Data.DefaultValue;

	private RebirthData mRebirthData;

	[Label]
	public float mDistance;

	[Label]
	public float mTime;

	[Label]
	public float mDissolveTime;

	[Label]
	public bool mIsFinished;

	private float mDeltaLocZ;

	private Vector3 mBeginPos = Vector3.zero;

	private Vector3 mTargetPos = Vector3.zero;

	private Vector3 mMoveLocDir = Vector3.forward;

	public override void Initialize()
	{
		base.Initialize();
		mDistance = 0f;
		mTime = 0f;
		mAnimator = base.gameObject.GetComponentInChildren<Animator>();
		Transform transform = base.gameObject.transform.Find("effect");
		if (transform != null)
		{
			mBecutEffect = transform.gameObject.GetComponent<ParticleSystem>();
			mBecutEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
		}
		Transform transform2 = base.gameObject.transform.Find("model");
		if (transform2 != null)
		{
			mMoveObj = transform2.gameObject;
			mRenderers = transform2.GetComponentsInChildren<Renderer>();
			SetDissolveProgress(0f);
		}
		mDistancSensore = new DistancSensore(mData.showDistance);
		m_bezierMover = new BezierMover(mData.pathData.m_bezierPositions);
		if (mMoveObj != null)
		{
			mMoveObj.gameObject.SetActive(true);
			mMoveObj.transform.position = mData.pathData.m_bezierPositions[0];
		}
		mIsFinished = false;
		OnSwitchState(State.Hide);
		Mod.Event.Subscribe(EventArgs<CutEventArg>.EventId, OnListen);
		CutManager.SubAttackables(mData.groupID, this);
	}

	private void OnSwitchState(State state)
	{
		mState = state;
		switch (mState)
		{
		case State.Hide:
			if ((bool)mAnimator)
			{
				mAnimator.Play("hide", 0, 0f);
			}
			break;
		case State.Show:
			if (mAnimator != null)
			{
				mAnimator.Play("show", 0, 0f);
			}
			break;
		case State.BeCut:
			if (mBecutEffect != null)
			{
				mBecutEffect.Play();
			}
			break;
		case State.Knock:
			if (mAnimator != null)
			{
				mAnimator.Play("knock", 0, 0f);
			}
			break;
		case State.Death:
			if (mAnimator != null)
			{
				mAnimator.Play("death", 0, 0f);
			}
			mDissolveTime = 0f;
			break;
		case State.Delay:
			break;
		}
	}

	public override void UpdateElement()
	{
		base.UpdateElement();
		switch (mState)
		{
		case State.Hide:
			if (Application.isPlaying)
			{
				mDistance = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z;
				if (mDistancSensore.Run(mDistance))
				{
					OnSwitchState(State.Show);
				}
			}
			break;
		case State.Delay:
			mTime += Time.deltaTime;
			if (mTime >= mData.delay)
			{
				OnSwitchState(State.BeCut);
			}
			break;
		case State.BeCut:
			OnSwitchState(State.Knock);
			break;
		case State.Knock:
			if (m_bezierMover != null && !mIsFinished)
			{
				mDeltaLocZ = Railway.theRailway.SpeedForward * Time.deltaTime * mData.speed;
				mBeginPos = mMoveObj.transform.position;
				mIsFinished = m_bezierMover.MoveForwardByDis(mDeltaLocZ, mBeginPos, ref mTargetPos, ref mMoveLocDir);
				mMoveObj.transform.position = mTargetPos;
				if (mIsFinished)
				{
					OnSwitchState(State.Death);
				}
			}
			break;
		case State.Death:
			mDissolveTime += Time.deltaTime;
			if (mDissolveTime >= mData.dissolveTime)
			{
				SetDissolveProgress(1f);
				OnSwitchState(State.End);
			}
			else
			{
				SetDissolveProgress(mDissolveTime / mData.dissolveTime);
			}
			break;
		case State.Show:
			break;
		}
	}

	public override void ResetElement()
	{
		CutManager.UnSubAttackables(mData.groupID, this);
		Mod.Event.Unsubscribe(EventArgs<CutEventArg>.EventId, OnListen);
		mIsFinished = false;
		base.ResetElement();
		if ((bool)mAnimator)
		{
			mAnimator.StopPlayback();
		}
		mDistancSensore = null;
	}

	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		if (mData.pathData.m_positions != null && mData.pathData.m_positions.Length >= 4 && (mData.pathData.m_positions.Length - 1) % 3 == 0)
		{
			ThreeBezier.DrawGizmos(base.gameObject, mData.pathData.m_positions, mData.pathData.m_smooth);
		}
	}

	private void SetDissolveProgress(float progress)
	{
		if (mRenderers == null)
		{
			return;
		}
		for (int i = 0; i < mRenderers.Length; i++)
		{
			if (!(mRenderers[i] == null))
			{
				MaterialTool.SetMaterialFloat(mRenderers[i], "_Progress", progress);
			}
		}
	}

	public override string Write()
	{
		mData.pathData.RefreshBezierPositions(base.gameObject, true);
		return JsonUtility.ToJson(mData);
	}

	public override byte[] WriteBytes()
	{
		mData.pathData.RefreshBezierPositions(base.gameObject, true);
		return Bson.ToBson(mData);
	}

	public override void Read(string info)
	{
		mData = JsonUtility.FromJson<Data>(info);
	}

	public override void ReadBytes(byte[] bytes)
	{
		mData = Bson.ToObject<Data>(bytes);
	}

	public override void SetDefaultValue(object[] objs)
	{
		mData = (Data)objs[0];
	}

	private void OnListen(object sender, Foundation.EventArgs e)
	{
		CutEventArg cutEventArg = e as CutEventArg;
		if (cutEventArg != null && cutEventArg.mData.groupID == mData.groupID)
		{
			OnSwitchState(State.Delay);
		}
	}

	public override void TriggerEnter(BaseRole ball)
	{
		if (mState == State.Show)
		{
			base.TriggerEnter(ball);
		}
	}

	public Vector3 GetPosition()
	{
		return base.gameObject.transform.position;
	}

	public override object RebirthWriteData()
	{
		return JsonUtility.ToJson(new RebirthData
		{
			trans = base.gameObject.transform.GetTransData(),
			time = mTime,
			dissolveTime = mDissolveTime,
			state = mState,
			isSensoreStay = (mDistancSensore != null && mDistancSensore.m_isTriggerStay),
			animator = mAnimator.GetAnimData(),
			becutEffect = mBecutEffect.GetParticleData(),
			moveObj = mMoveObj.transform.GetTransData(),
			bezier = m_bezierMover.GetBezierData(),
			isFinished = mIsFinished
		});
	}

	public override byte[] RebirthWriteByteData()
	{
		return Bson.ToBson(new RebirthData
		{
			trans = base.gameObject.transform.GetTransData(),
			time = mTime,
			dissolveTime = mDissolveTime,
			state = mState,
			isSensoreStay = (mDistancSensore != null && mDistancSensore.m_isTriggerStay),
			animator = mAnimator.GetAnimData(),
			becutEffect = mBecutEffect.GetParticleData(),
			moveObj = mMoveObj.transform.GetTransData(),
			bezier = m_bezierMover.GetBezierData(),
			isFinished = mIsFinished
		});
	}

	public override void RebirthReadData(object rd_data)
	{
		if (rd_data == null)
		{
			return;
		}
		string text = (string)rd_data;
		if (!string.IsNullOrEmpty(text))
		{
			mRebirthData = JsonUtility.FromJson<RebirthData>(text);
			base.gameObject.transform.SetTransData(mRebirthData.trans);
			mTime = mRebirthData.time;
			mDissolveTime = mRebirthData.dissolveTime;
			mState = mRebirthData.state;
			if ((bool)mMoveObj)
			{
				mMoveObj.transform.SetTransData(mRebirthData.moveObj);
			}
			m_bezierMover.SetBezierData(mRebirthData.bezier);
			mIsFinished = mRebirthData.isFinished;
			OnSwitchState(mState);
			if (mDistancSensore != null)
			{
				mDistancSensore.m_isTriggerStay = mRebirthData.isSensoreStay;
			}
			mAnimator.SetAnimData(mRebirthData.animator, ProcessState.Pause);
			mBecutEffect.SetParticleData(mRebirthData.becutEffect, ProcessState.Pause);
		}
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
		if (rd_data != null)
		{
			mRebirthData = Bson.ToObject<RebirthData>(rd_data);
			base.gameObject.transform.SetTransData(mRebirthData.trans);
			mTime = mRebirthData.time;
			mDissolveTime = mRebirthData.dissolveTime;
			mState = mRebirthData.state;
			if ((bool)mMoveObj)
			{
				mMoveObj.transform.SetTransData(mRebirthData.moveObj);
			}
			m_bezierMover.SetBezierData(mRebirthData.bezier);
			mIsFinished = mRebirthData.isFinished;
			OnSwitchState(mState);
			if (mDistancSensore != null)
			{
				mDistancSensore.m_isTriggerStay = mRebirthData.isSensoreStay;
			}
			mAnimator.SetAnimData(mRebirthData.animator, ProcessState.Pause);
			mBecutEffect.SetParticleData(mRebirthData.becutEffect, ProcessState.Pause);
		}
	}

	public override void RebirthStartGame(object rd_data)
	{
		if (mRebirthData != null)
		{
			mAnimator.SetAnimData(mRebirthData.animator, ProcessState.UnPause);
			mBecutEffect.SetParticleData(mRebirthData.becutEffect, ProcessState.UnPause);
			mRebirthData = null;
		}
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
		if (mRebirthData != null)
		{
			mAnimator.SetAnimData(mRebirthData.animator, ProcessState.UnPause);
			mBecutEffect.SetParticleData(mRebirthData.becutEffect, ProcessState.UnPause);
			mRebirthData = null;
		}
	}
}
