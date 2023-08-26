using System;
using Foundation;
using RS2;
using UnityEngine;

public class RoleOnFireTrigger : BaseTriggerBox
{
	public enum State
	{
		None,
		TriggerOnFire,
		OnFire,
		TriggerOutFire,
		OutFire,
		Death
	}

	[Serializable]
	public struct Data
	{
		public int groupID;

		public float deathDistancs;

		public float resetDistance;

		public static Data DefaultValue
		{
			get
			{
				Data result = default(Data);
				result.groupID = 1;
				result.deathDistancs = 20f;
				result.resetDistance = 30f;
				return result;
			}
		}
	}

	[Serializable]
	public class RebirthData
	{
		public RD_ElementTransform_DATA trans;

		public RD_ElementTransform_DATA onFireTrans;

		public RD_ElementTransform_DATA outFireTrans;

		public RD_ElementParticle_DATA onFireEffect;

		public RD_ElementParticle_DATA outFireEffect;
	}

	public Transform mOnFireTran;

	public Transform mOutFireTran;

	public ParticleSystem mOnFireEffect;

	public ParticleSystem mOutFireEffect;

	[Label]
	public State mState;

	public Data mData;

	private RebirthData mRebirthData;

	[Label]
	public float mDistance;

	public override void Initialize()
	{
		mOnFireTran = base.gameObject.transform.Find("onFire");
		if (mOnFireTran != null)
		{
			mOnFireEffect = mOnFireTran.gameObject.GetComponent<ParticleSystem>();
			mOnFireEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
		}
		mOutFireTran = base.gameObject.transform.Find("outFire");
		if (mOutFireTran != null)
		{
			mOutFireEffect = mOutFireTran.gameObject.GetComponent<ParticleSystem>();
			mOutFireEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
		}
		Mod.Event.Subscribe(EventArgs<OutFireEventArgs>.EventId, OnListenEvent);
	}

	public override void UpdateElement()
	{
		if (mState == State.None)
		{
			return;
		}
		mDistance = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z;
		if (mDistance >= mData.resetDistance)
		{
			SwitchState(State.None);
			ResetElement();
			return;
		}
		switch (mState)
		{
		case State.TriggerOnFire:
			SwitchState(State.OnFire);
			break;
		case State.OnFire:
			OnOnFireUpdate();
			break;
		case State.TriggerOutFire:
			SwitchState(State.OutFire);
			break;
		case State.OutFire:
			OnOutFireUpdate();
			break;
		case State.None:
		case State.Death:
			break;
		}
	}

	public override void ResetElement()
	{
		Mod.Event.Unsubscribe(EventArgs<OutFireEventArgs>.EventId, OnListenEvent);
	}

	private void SwitchState(State state)
	{
		mState = state;
		switch (state)
		{
		case State.TriggerOnFire:
			if (mOnFireEffect != null)
			{
				mOnFireEffect.Play();
			}
			if (mOutFireEffect != null)
			{
				mOutFireEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
			}
			break;
		case State.TriggerOutFire:
			if (mOutFireEffect != null)
			{
				mOutFireEffect.Play();
			}
			if (mOnFireEffect != null)
			{
				mOnFireEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
			}
			break;
		case State.Death:
			if (!GameController.IfNotDeath)
			{
				BaseRole.theBall.CrashBall();
			}
			break;
		case State.None:
		case State.OnFire:
		case State.OutFire:
			break;
		}
	}

	private void OnOnFireUpdate()
	{
		if (mDistance >= mData.deathDistancs)
		{
			SwitchState(State.Death);
		}
		else if (mOnFireEffect != null)
		{
			Transform roleCenter = BaseRole.theBall.BodyPart.RoleCenter;
			mOnFireEffect.transform.position = roleCenter.transform.position;
		}
	}

	private void OnOutFireUpdate()
	{
		if (mOutFireEffect != null)
		{
			Transform roleCenter = BaseRole.theBall.BodyPart.RoleCenter;
			mOutFireEffect.transform.position = roleCenter.transform.position;
		}
	}

	private void OnListenEvent(object sender, Foundation.EventArgs e)
	{
		OutFireEventArgs outFireEventArgs = e as OutFireEventArgs;
		if (outFireEventArgs != null && outFireEventArgs.mData.groupID == mData.groupID)
		{
			SwitchState(State.TriggerOutFire);
		}
	}

	public override void TriggerEnter(BaseRole ball)
	{
		SwitchState(State.TriggerOnFire);
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
		return JsonUtility.ToJson(new RebirthData
		{
			trans = base.gameObject.transform.GetTransData(),
			onFireTrans = mOnFireTran.GetTransData(),
			outFireTrans = mOutFireTran.GetTransData(),
			onFireEffect = mOnFireEffect.GetParticleData(),
			outFireEffect = mOutFireEffect.GetParticleData()
		});
	}

	public override byte[] RebirthWriteByteData()
	{
		return Bson.ToBson(new RebirthData
		{
			trans = base.gameObject.transform.GetTransData(),
			onFireTrans = mOnFireTran.GetTransData(),
			outFireTrans = mOutFireTran.GetTransData(),
			onFireEffect = mOnFireEffect.GetParticleData(),
			outFireEffect = mOutFireEffect.GetParticleData()
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
				mOnFireTran.SetTransData(mRebirthData.onFireTrans);
				mOutFireTran.SetTransData(mRebirthData.outFireTrans);
				mOnFireEffect.SetParticleData(mRebirthData.onFireEffect, ProcessState.Pause);
				mOutFireEffect.SetParticleData(mRebirthData.outFireEffect, ProcessState.Pause);
			}
		}
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
		if (rd_data != null)
		{
			mRebirthData = Bson.ToObject<RebirthData>(rd_data);
			base.gameObject.transform.SetTransData(mRebirthData.trans);
			mOnFireTran.SetTransData(mRebirthData.onFireTrans);
			mOutFireTran.SetTransData(mRebirthData.outFireTrans);
			mOnFireEffect.SetParticleData(mRebirthData.onFireEffect, ProcessState.Pause);
			mOutFireEffect.SetParticleData(mRebirthData.outFireEffect, ProcessState.Pause);
		}
	}

	public override void RebirthStartGame(object rd_data)
	{
		if (mRebirthData != null)
		{
			mOnFireEffect.SetParticleData(mRebirthData.onFireEffect, ProcessState.UnPause);
			mOutFireEffect.SetParticleData(mRebirthData.outFireEffect, ProcessState.UnPause);
			mRebirthData = null;
		}
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
		if (mRebirthData != null)
		{
			mOnFireEffect.SetParticleData(mRebirthData.onFireEffect, ProcessState.UnPause);
			mOutFireEffect.SetParticleData(mRebirthData.outFireEffect, ProcessState.UnPause);
			mRebirthData = null;
		}
	}
}
