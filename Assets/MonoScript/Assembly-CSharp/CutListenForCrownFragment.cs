using System;
using Foundation;
using RS2;
using UnityEngine;

public class CutListenForCrownFragment : BaseTriggerBox, IAwardFragement, IAward
{
	public enum State
	{
		Hide,
		Delay,
		Show,
		Trigger,
		End
	}

	[Serializable]
	public struct Data
	{
		public int groupID;

		public float resetDistance;

		public float delay;

		[Label]
		public int sortID;

		[Label]
		public int needCount;

		[Label]
		public int completeSortID;

		public static Data DefaultValue
		{
			get
			{
				Data result = default(Data);
				result.groupID = 1;
				result.resetDistance = 20f;
				result.delay = 0.5f;
				result.sortID = 0;
				result.needCount = 0;
				result.completeSortID = 0;
				return result;
			}
		}
	}

	public struct RebirthData
	{
		public RD_ElementTransform_DATA trans;

		public RD_ElementTransform_DATA model;

		public State state;

		public float Time;
	}

	private const string ANIMNAME = "anim01";

	protected Animation mAnim;

	protected Animation mShowAnim;

	protected Animation mHideAnim;

	protected ParticleSystem mEffect;

	protected Transform mModel;

	public Data mData;

	[Label]
	public State mState;

	[Label]
	public float mTime;

	[Label]
	public float mDistanse;

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
		mAnim = GetComponentInChildren<Animation>();
		mEffect = GetComponentInChildren<ParticleSystem>();
		mModel = base.transform.Find("model");
		OnSwitchState(State.Hide);
		if (mShowAnim == null)
		{
			Transform transform = base.transform.Find("model/showNode");
			if (transform != null)
			{
				mShowAnim = transform.gameObject.GetComponent<Animation>();
			}
		}
		if (mHideAnim == null)
		{
			Transform transform2 = base.transform.Find("model/hideNode");
			if (transform2 != null)
			{
				mHideAnim = transform2.gameObject.GetComponent<Animation>();
			}
		}
		if (mShowAnim != null)
		{
			mShowAnim.gameObject.SetActive(false);
		}
		if (mHideAnim != null)
		{
			mHideAnim.gameObject.SetActive(false);
		}
		InsideGameDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule);
		DropType completeDropType = GetCompleteDropType();
		int completeSortID = GetCompleteSortID();
		if (dataModule.IsShowForAwardByDropType(completeDropType, completeSortID))
		{
			mAnim = mShowAnim;
		}
		else
		{
			mAnim = mHideAnim;
		}
		if (mAnim != null)
		{
			mAnim.gameObject.SetActive(true);
		}
		mTime = 0f;
		Mod.Event.Subscribe(EventArgs<CutEventArg>.EventId, OnListen);
	}

	protected void OnSwitchState(State state)
	{
		switch (state)
		{
		case State.Hide:
			if ((bool)mModel)
			{
				mModel.gameObject.SetActive(false);
			}
			if ((bool)mEffect)
			{
				mEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
			}
			break;
		case State.Delay:
			if ((bool)mModel)
			{
				mModel.gameObject.SetActive(false);
			}
			break;
		case State.Show:
			if ((bool)mModel)
			{
				mModel.gameObject.SetActive(true);
			}
			if ((bool)mAnim)
			{
				mAnim.Play("anim01");
			}
			break;
		case State.Trigger:
			if ((bool)mModel)
			{
				mModel.gameObject.SetActive(false);
			}
			if ((bool)mEffect)
			{
				mEffect.Play();
			}
			PlaySoundEffect();
			break;
		case State.End:
			ResetElement();
			break;
		}
		mState = State.Hide;
	}

	public override void UpdateElement()
	{
		base.UpdateElement();
		if (mState == State.End)
		{
			return;
		}
		State state = mState;
		if (state == State.Delay)
		{
			mTime += Time.deltaTime;
			if (mTime >= mData.delay)
			{
				OnSwitchState(State.Show);
			}
		}
		mDistanse = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z;
		if (mDistanse >= mData.resetDistance)
		{
			OnSwitchState(State.End);
		}
	}

	public override void ResetElement()
	{
		Mod.Event.Unsubscribe(EventArgs<CutEventArg>.EventId, OnListen);
		base.ResetElement();
		if ((bool)mModel)
		{
			mModel.gameObject.SetActive(true);
		}
		if (mAnim != null)
		{
			PlayAnim(mAnim, false);
		}
		if ((bool)mEffect)
		{
			mEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
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
		if (mModel.gameObject.activeSelf)
		{
			ball.GainCrownFragment(m_uuId);
			Mod.Event.FireNow(this, ChangeRoleIntroductionIdentifierEventArgs.Make(true, 16, mData.needCount));
			OnSwitchState(State.Trigger);
		}
	}

	private void OnListen(object sender, Foundation.EventArgs e)
	{
		CutEventArg cutEventArg = e as CutEventArg;
		if (cutEventArg != null && cutEventArg.mData.groupID == mData.groupID)
		{
			mState = State.Delay;
		}
	}

	public override void RebirthReadData(object rd_data)
	{
		RebirthData rebirthData = JsonUtility.FromJson<RebirthData>((string)rd_data);
		base.transform.SetTransData(rebirthData.trans);
		if (mModel != null)
		{
			mModel.SetTransData(rebirthData.model);
		}
		mTime = rebirthData.Time;
		mState = rebirthData.state;
	}

	public override object RebirthWriteData()
	{
		RebirthData rebirthData = default(RebirthData);
		rebirthData.trans = base.transform.GetTransData();
		if (mModel != null)
		{
			rebirthData.model = mModel.GetTransData();
		}
		rebirthData.Time = mTime;
		rebirthData.state = mState;
		return JsonUtility.ToJson(rebirthData);
	}

	public override void RebirthReadDataForDrop(object rd_data)
	{
		OnSwitchState(State.Hide);
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
		RebirthData rebirthData = Bson.ToObject<RebirthData>(rd_data);
		base.transform.SetTransData(rebirthData.trans);
		if (mModel != null)
		{
			mModel.SetTransData(rebirthData.model);
		}
		mTime = rebirthData.Time;
		mState = rebirthData.state;
	}

	public override byte[] RebirthWriteByteData()
	{
		RebirthData rebirthData = default(RebirthData);
		rebirthData.trans = base.transform.GetTransData();
		if (mModel != null)
		{
			rebirthData.model = mModel.GetTransData();
		}
		rebirthData.Time = mTime;
		rebirthData.state = mState;
		return Bson.ToBson(rebirthData);
	}

	public override void RebirthReadByteDataForDrop(byte[] rd_data)
	{
		OnSwitchState(State.Hide);
	}

	public int GetNeedFragementCount()
	{
		return mData.needCount;
	}

	public void SetNeedFragementCount(int count)
	{
		mData.needCount = count;
	}

	public void SetCompleteSortID(int sortID)
	{
		mData.completeSortID = sortID;
	}

	public int GetCompleteSortID()
	{
		return mData.completeSortID;
	}

	public virtual DropType GetCompleteDropType()
	{
		return DropType.CROWN;
	}

	public int GetAwardSortID()
	{
		return mData.sortID;
	}

	public void SetAwardSortID(int id)
	{
		mData.sortID = id;
	}

	public virtual DropType GetDropType()
	{
		return DropType.CROWNFRAGMENT;
	}
}
