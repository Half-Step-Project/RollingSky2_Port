using System;
using Foundation;
using UnityEngine;

public class ContinuousCutListenForAssassinEnemy : BaseEnemy, IContinuousAttackable, IAttackable
{
	public enum State
	{
		Hide,
		Show,
		BeCut,
		Death,
		End
	}

	[Serializable]
	public struct Data
	{
		[Header("Hide animatorStateName ----------------hide")]
		[Header("Show animatorStateName ----------------show")]
		[Header("Death animatorStateName ----------------death")]
		public int groupID;

		public float showDistance;

		public float dissolveTime;

		public static Data DefaultValue
		{
			get
			{
				Data result = default(Data);
				result.groupID = 1;
				result.showDistance = -30f;
				result.dissolveTime = 1f;
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

		public RD_ElementAnimator_DATA animator;

		public RD_ElementParticle_DATA becutEffect;

		public float time;

		public float dissolveTime;
	}

	private const string HIDEANIMNAME = "hide";

	private const string SHOWANIMNAME = "show";

	private const string DEATHANIMNAME = "death";

	[Label]
	public State mState;

	public DistancSensore mDistancSensore;

	public Animator mAnimator;

	public ParticleSystem mBecutEffect;

	public Renderer[] mRenderers;

	public Data mData;

	private RebirthData mRebirthData;

	[Label]
	public float mDistance;

	[Label]
	public float mTime;

	[Label]
	public float mDissolveTime;

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
			mRenderers = transform2.GetComponentsInChildren<Renderer>();
			SetDissolveProgress(0f);
		}
		mDistancSensore = new DistancSensore(mData.showDistance);
		OnSwitchState(State.Hide);
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
			PlaySoundEffect();
			break;
		case State.Death:
			if (mAnimator != null)
			{
				mAnimator.Play("death", 0, 0f);
			}
			mDissolveTime = 0f;
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
		case State.BeCut:
			OnSwitchState(State.Death);
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
		base.ResetElement();
		if ((bool)mAnimator)
		{
			mAnimator.StopPlayback();
		}
		mDistancSensore = null;
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
		return JsonUtility.ToJson(mData);
	}

	public override byte[] WriteBytes()
	{
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

	public void OnBeCut()
	{
		OnSwitchState(State.BeCut);
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
			becutEffect = mBecutEffect.GetParticleData()
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
			becutEffect = mBecutEffect.GetParticleData()
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
