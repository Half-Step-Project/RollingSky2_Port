using System;
using System.Collections.Generic;
using Foundation;
using UnityEngine;

public class ContinuousCutSendTrigger : BaseTriggerBox, IAttack
{
	public enum State
	{
		None,
		Trigger,
		Move,
		Finished
	}

	[Serializable]
	public struct Data
	{
		public int groupID;

		public float mTime;

		public EaseType easeType;

		public float mAlphaTime;

		public static Data DefaultValue
		{
			get
			{
				Data result = default(Data);
				result.groupID = 1;
				result.mTime = 0.3f;
				result.easeType = EaseType.OutBackQuartic;
				result.mAlphaTime = 1f;
				return result;
			}
		}
	}

	[Serializable]
	public class RebirthData
	{
		public RD_ElementTransform_DATA trans;
	}

	[Serializable]
	public class StopMotionsScript
	{
		public GameObject mTarget;

		public GameObject mModel;

		public TrailRenderer mTrail;

		public Renderer[] mRenderers;

		public ParticleSystem mEffect;

		[Label]
		public Vector3 mFrom;

		[Label]
		public Vector3 mTo;

		[Label]
		public float mTime;

		[Label]
		public EaseType mEaseType;

		[Label]
		public float mAlphaTime;

		public IContinuousAttackable mAttackable;

		[Label]
		public float mCurTime;

		[Label]
		public float mCurAlphaTime;

		[Label]
		public bool mIsPlaying;

		[Label]
		public bool mIsShowModel;

		[Label]
		public bool mIsPlayingAlpha;

		public StopMotionsScript(GameObject target)
		{
			mTarget = target;
		}

		public void OnInitialized()
		{
			Transform transform = mTarget.transform.Find("model");
			if (transform != null)
			{
				mModel = transform.gameObject;
				mRenderers = transform.gameObject.GetComponentsInChildren<Renderer>();
				SetModelAlpha(0f);
			}
			Transform transform2 = mTarget.transform.Find("effect");
			if (transform2 != null)
			{
				mEffect = transform2.gameObject.GetComponentInChildren<ParticleSystem>();
				if (mEffect != null)
				{
					mEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
				}
			}
			Transform transform3 = mTarget.transform.Find("trail");
			if (transform3 != null)
			{
				mTrail = transform3.gameObject.GetComponent<TrailRenderer>();
				mTrail.Clear();
			}
		}

		public void SetAttackable(IContinuousAttackable attackable)
		{
			mAttackable = attackable;
		}

		public void SetMoveData(Vector3 from, Vector3 to, float time, EaseType easeType)
		{
			mFrom = from;
			mTo = to;
			mTime = time;
			mEaseType = easeType;
		}

		public void SetAlphaData(float time)
		{
			mAlphaTime = time;
		}

		public void Play()
		{
			if (mModel != null)
			{
				mModel.transform.position = mTo;
				SetModelAlpha(0f);
			}
			if (mTrail != null)
			{
				mTrail.Clear();
				mTrail.enabled = true;
				mTrail.transform.position = mFrom;
			}
			if (mEffect != null)
			{
				mEffect.Play();
				mEffect.transform.position = mFrom;
			}
			mCurTime = 0f;
			mCurAlphaTime = 0f;
			mIsShowModel = false;
			mIsPlaying = true;
			mIsPlayingAlpha = false;
		}

		public void OnUpdate()
		{
			if (mIsPlaying)
			{
				mCurTime += Time.deltaTime;
				float num = mCurTime / mTime;
				if (mTrail.transform.position == mTo && !mIsShowModel)
				{
					SetModelAlpha(1f);
					mIsShowModel = true;
				}
				if (mTrail != null)
				{
					mTrail.transform.position = Easing.EasingVector3((num >= 1f) ? 1f : num, mFrom, mTo, 1f, mEaseType);
				}
				if (mEffect != null)
				{
					mEffect.transform.position = Easing.EasingVector3((num >= 1f) ? 1f : num, mFrom, mTo, 1f, mEaseType);
				}
				if (mCurTime >= mTime)
				{
					if (mAttackable != null)
					{
						mAttackable.OnBeCut();
					}
					mIsPlaying = false;
					mIsPlayingAlpha = true;
				}
			}
			if (!mIsPlayingAlpha)
			{
				return;
			}
			mCurAlphaTime += Time.deltaTime;
			float num2 = mCurAlphaTime / mAlphaTime;
			SetModelAlpha(Easing.EasingFloat((num2 >= 1f) ? 0f : (1f - num2), 0f, 1f, 1f));
			if (num2 >= 1f)
			{
				if (mEffect != null)
				{
					mEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
				}
				mIsPlayingAlpha = false;
			}
		}

		public void OnRefresh()
		{
			mCurTime = 0f;
			mCurAlphaTime = 0f;
			mIsPlaying = false;
			mIsShowModel = false;
			mIsPlayingAlpha = false;
		}

		private void SetModelAlpha(float a)
		{
			if (mRenderers != null)
			{
				for (int i = 0; i < mRenderers.Length; i++)
				{
					MaterialTool.SetMaterialFloat(mRenderers[i], "_Alpha", a);
				}
			}
		}
	}

	public StopMotionsScript[] mStopMotions = new StopMotionsScript[9];

	[Label]
	public State mState;

	public Data mData;

	private RebirthData mRebirthData;

	[Label]
	public int mIndex = -1;

	private List<IAttackable> mAttackables;

	[Label]
	public bool mIsPlaying;

	[Label]
	public bool mIsToNext;

	public override void Initialize()
	{
		base.Initialize();
		OnSwitchState(State.None);
		CutManager.SubAttacks(mData.groupID, this);
	}

	public override void UpdateElement()
	{
		base.UpdateElement();
		switch (mState)
		{
		case State.Move:
			OnMoveUpdate();
			break;
		case State.None:
		case State.Trigger:
		case State.Finished:
			break;
		}
	}

	public override void ResetElement()
	{
		CutManager.UnSubAttacks(mData.groupID);
		for (int i = 0; i < mStopMotions.Length; i++)
		{
			mStopMotions[i].OnRefresh();
		}
		base.ResetElement();
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

	private void OnSwitchState(State state)
	{
		mState = state;
		switch (state)
		{
		case State.None:
			OnNone();
			break;
		case State.Trigger:
			OnTrigger();
			break;
		case State.Move:
		case State.Finished:
			break;
		}
	}

	private void FindTileChindren()
	{
		for (int i = 0; i < mStopMotions.Length; i++)
		{
			StopMotionsScript stopMotionsScript = new StopMotionsScript(base.gameObject.transform.Find("Array/StopMotions" + i).gameObject);
			stopMotionsScript.OnInitialized();
			mStopMotions[i] = stopMotionsScript;
		}
	}

	private void OnNone()
	{
		FindTileChindren();
		mIsPlaying = false;
		mIsToNext = false;
	}

	private void OnTrigger()
	{
		mAttackables = CutManager.GetAttackables(mData.groupID);
		if (mStopMotions.Length >= mAttackables.Count && mAttackables.Count >= 1)
		{
			Vector3 vector = BaseRole.theBall.ballModelObj.transform.position;
			for (int i = 0; i < mAttackables.Count; i++)
			{
				if (mAttackables[i] is IContinuousAttackable)
				{
					IContinuousAttackable continuousAttackable = (IContinuousAttackable)mAttackables[i];
					Vector3 position = continuousAttackable.GetPosition();
					Vector3 from = vector;
					mStopMotions[i].SetMoveData(from, position, mData.mTime, mData.easeType);
					mStopMotions[i].SetAlphaData(mData.mAlphaTime);
					mStopMotions[i].SetAttackable(continuousAttackable);
					vector = position;
				}
			}
			mIsPlaying = true;
			mIsToNext = true;
			mIndex = -1;
		}
		else
		{
			Log.Error("ContinuousCutSendTrigger mStopMotions.length have errer");
		}
		OnSwitchState(State.Move);
	}

	private void OnMoveUpdate()
	{
		if (mAttackables == null || mAttackables.Count == 0 || mStopMotions.Length < mAttackables.Count)
		{
			OnSwitchState(State.Finished);
		}
		else
		{
			if (!mIsPlaying)
			{
				return;
			}
			if (mIsToNext)
			{
				mIndex++;
				if (mIndex > mAttackables.Count)
				{
					mIsToNext = false;
					mIsPlaying = false;
					OnSwitchState(State.Finished);
				}
				else
				{
					mStopMotions[mIndex].Play();
				}
				mIsToNext = false;
			}
			if (mIndex == mAttackables.Count - 1)
			{
				if (!mStopMotions[mIndex].mIsPlayingAlpha)
				{
					mIsToNext = true;
				}
			}
			else if (mIndex < mAttackables.Count - 1 && !mStopMotions[mIndex].mIsPlaying)
			{
				mIsToNext = true;
			}
			for (int i = 0; i < mStopMotions.Length; i++)
			{
				mStopMotions[i].OnUpdate();
			}
		}
	}

	private void OnFinished()
	{
	}

	public override void TriggerEnter(BaseRole ball)
	{
		OnSwitchState(State.Trigger);
	}

	public Vector3 GetPosition()
	{
		return base.gameObject.transform.position;
	}

	public override object RebirthWriteData()
	{
		return JsonUtility.ToJson(new RebirthData
		{
			trans = base.gameObject.transform.GetTransData()
		});
	}

	public override byte[] RebirthWriteByteData()
	{
		return Bson.ToBson(new RebirthData
		{
			trans = base.gameObject.transform.GetTransData()
		});
	}

	public override void RebirthReadData(object rd_data)
	{
		if (rd_data != null)
		{
			string text = (string)rd_data;
			if (!string.IsNullOrEmpty(text))
			{
				mRebirthData = JsonUtility.FromJson<RebirthData>(text);
				base.gameObject.transform.SetTransData(mRebirthData.trans);
			}
		}
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
		if (rd_data != null)
		{
			mRebirthData = Bson.ToObject<RebirthData>(rd_data);
			base.gameObject.transform.SetTransData(mRebirthData.trans);
		}
	}
}
