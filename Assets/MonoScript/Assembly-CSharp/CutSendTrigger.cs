using System;
using Foundation;
using RS2;
using UnityEngine;

public class CutSendTrigger : BaseTriggerBox, IAttack
{
	[Serializable]
	public struct Data
	{
		public int groupID;

		public bool lookFirst;

		public static Data DefaultValue
		{
			get
			{
				Data result = default(Data);
				result.groupID = 1;
				result.lookFirst = false;
				return result;
			}
		}
	}

	[Serializable]
	public class RebirthData
	{
		public RD_ElementTransform_DATA trans;

		public RD_ElementAnim_DATA anim;

		public RD_ElementTransform_DATA animTrans;

		public RD_ElementParticle_DATA gasathEffect;

		public RD_ElementTransform_DATA gasathTrans;

		public float curTime;

		public bool IsPlayGasath;
	}

	[SerializeField]
	private Animation mSwordAnim;

	[SerializeField]
	private ParticleSystem mGasatheringEffect;

	public Data mData;

	private RebirthData mRebirthData;

	private bool mIsPlayGasatheringEffect;

	private float mAnimTime = 3f;

	private float mCurrentTime;

	public override void Initialize()
	{
		base.Initialize();
		mSwordAnim = base.gameObject.GetComponentInChildren<Animation>();
		if ((bool)mSwordAnim)
		{
			mSwordAnim.Stop();
			if (mSwordAnim.clip != null)
			{
				mAnimTime = mSwordAnim.clip.length;
			}
		}
		Transform transform = base.gameObject.transform.Find("Gasathering");
		if (transform != null)
		{
			mGasatheringEffect = transform.gameObject.GetComponent<ParticleSystem>();
			if (mGasatheringEffect != null)
			{
				mGasatheringEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
			}
		}
		mIsPlayGasatheringEffect = false;
		mCurrentTime = 0f;
		CutManager.SubAttacks(mData.groupID, this);
	}

	public override void UpdateElement()
	{
		if (mIsPlayGasatheringEffect)
		{
			mCurrentTime += Time.deltaTime;
			if (mCurrentTime >= mAnimTime)
			{
				mIsPlayGasatheringEffect = false;
			}
			BaseRole theBall = BaseRole.theBall;
			if ((bool)theBall.BodyPart.RoleCenter && mGasatheringEffect != null)
			{
				mGasatheringEffect.transform.position = theBall.BodyPart.RoleCenter.transform.position;
			}
		}
	}

	public override void ResetElement()
	{
		CutManager.UnSubAttacks(mData.groupID);
		base.ResetElement();
		mIsPlayGasatheringEffect = false;
		mCurrentTime = 0f;
		if ((bool)mSwordAnim)
		{
			mSwordAnim.Stop();
		}
		if (mGasatheringEffect != null)
		{
			mGasatheringEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
		}
	}

	public override void TriggerEnter(BaseRole ball)
	{
		base.TriggerEnter(ball);
		BaseRole theBall = BaseRole.theBall;
		Quaternion localRotation = Quaternion.identity;
		if (mData.lookFirst)
		{
			IAttackable attackableByDistanceZ = CutManager.GetAttackableByDistanceZ(mData.groupID, base.gameObject.transform.position);
			if (attackableByDistanceZ != null)
			{
				localRotation = Quaternion.LookRotation(attackableByDistanceZ.GetPosition() - theBall.ballModelObj.transform.position);
			}
		}
		theBall.ballModelObj.transform.localRotation = localRotation;
		if (theBall.BodyPart.RoleRightHand != null && mSwordAnim != null)
		{
			mSwordAnim.transform.position = theBall.BodyPart.RoleRightHand.transform.position;
			mSwordAnim.transform.rotation = theBall.ballModelObj.transform.rotation;
			mSwordAnim.Play();
		}
		if ((bool)theBall.BodyPart.RoleCenter && mGasatheringEffect != null)
		{
			mGasatheringEffect.transform.position = theBall.BodyPart.RoleCenter.transform.position;
			mGasatheringEffect.Play(true);
			mIsPlayGasatheringEffect = true;
		}
		CutEventArg args = Mod.Reference.Acquire<CutEventArg>().Initialize(new GroupSendData
		{
			groupID = mData.groupID
		});
		Mod.Event.FireNow(EventArgs<CutEventArg>.EventId, args);
	}

	public Vector3 GetPosition()
	{
		return base.gameObject.transform.position;
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

	public override object RebirthWriteData()
	{
		RebirthData rebirthData = new RebirthData();
		rebirthData.trans = base.gameObject.transform.GetTransData();
		if (mSwordAnim != null)
		{
			rebirthData.animTrans = mSwordAnim.transform.GetTransData();
		}
		rebirthData.anim = mSwordAnim.GetAnimData();
		rebirthData.gasathEffect = mGasatheringEffect.GetParticleData();
		if (mGasatheringEffect != null)
		{
			rebirthData.gasathTrans = mGasatheringEffect.transform.GetTransData();
		}
		rebirthData.IsPlayGasath = mIsPlayGasatheringEffect;
		rebirthData.curTime = mCurrentTime;
		return JsonUtility.ToJson(rebirthData);
	}

	public override byte[] RebirthWriteByteData()
	{
		RebirthData rebirthData = new RebirthData();
		rebirthData.trans = base.gameObject.transform.GetTransData();
		if (mSwordAnim != null)
		{
			rebirthData.animTrans = mSwordAnim.transform.GetTransData();
		}
		rebirthData.anim = mSwordAnim.GetAnimData();
		rebirthData.gasathEffect = mGasatheringEffect.GetParticleData();
		if (mGasatheringEffect != null)
		{
			rebirthData.gasathTrans = mGasatheringEffect.transform.GetTransData();
		}
		rebirthData.IsPlayGasath = mIsPlayGasatheringEffect;
		rebirthData.curTime = mCurrentTime;
		return Bson.ToBson(rebirthData);
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
			if (mSwordAnim != null)
			{
				mSwordAnim.transform.SetTransData(mRebirthData.animTrans);
			}
			mSwordAnim.SetAnimData(mRebirthData.anim, ProcessState.Pause);
			if (mGasatheringEffect != null)
			{
				mGasatheringEffect.transform.SetTransData(mRebirthData.gasathTrans);
				mGasatheringEffect.SetParticleData(mRebirthData.gasathEffect, ProcessState.Pause);
			}
			mIsPlayGasatheringEffect = mRebirthData.IsPlayGasath;
			mCurrentTime = mRebirthData.curTime;
		}
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
		if (rd_data != null)
		{
			mRebirthData = Bson.ToObject<RebirthData>(rd_data);
			base.gameObject.transform.SetTransData(mRebirthData.trans);
			if (mSwordAnim != null)
			{
				mSwordAnim.transform.SetTransData(mRebirthData.animTrans);
			}
			mSwordAnim.SetAnimData(mRebirthData.anim, ProcessState.Pause);
			if (mGasatheringEffect != null)
			{
				mGasatheringEffect.transform.SetTransData(mRebirthData.gasathTrans);
				mGasatheringEffect.SetParticleData(mRebirthData.gasathEffect, ProcessState.Pause);
			}
			mIsPlayGasatheringEffect = mRebirthData.IsPlayGasath;
			mCurrentTime = mRebirthData.curTime;
		}
	}

	public override void RebirthStartGame(object rd_data)
	{
		if (mRebirthData != null)
		{
			mSwordAnim.SetAnimData(mRebirthData.anim, ProcessState.UnPause);
			mGasatheringEffect.SetParticleData(mRebirthData.gasathEffect, ProcessState.UnPause);
			mRebirthData = null;
		}
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
		if (mRebirthData != null)
		{
			mSwordAnim.SetAnimData(mRebirthData.anim, ProcessState.UnPause);
			mGasatheringEffect.SetParticleData(mRebirthData.gasathEffect, ProcessState.UnPause);
			mRebirthData = null;
		}
	}
}
