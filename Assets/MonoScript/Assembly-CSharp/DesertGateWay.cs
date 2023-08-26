using System;
using Foundation;
using UnityEngine;

public class DesertGateWay : BaseEnemy
{
	private Animation waitingAnim;

	private Animation openModelAnim;

	private Animation openEffectAnim;

	private ParticleSystem[] waitingEffect;

	private ParticleSystem[] openEffect;

	private RD_DesertGateWay_DATA m_rebirthData;

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
		commonState = CommonState.None;
		Transform transform = base.transform.Find("WaitingEffect");
		Transform transform2 = base.transform.Find("OpenModel");
		Transform transform3 = base.transform.Find("OpenEffect");
		waitingAnim = transform.GetComponentInChildren<Animation>();
		openModelAnim = transform2.GetComponentInChildren<Animation>();
		openEffectAnim = transform3.GetComponentInChildren<Animation>();
		waitingEffect = transform.GetComponentsInChildren<ParticleSystem>();
		openEffect = transform3.GetComponentsInChildren<ParticleSystem>();
		PlayParticle(waitingEffect, true);
		PlayParticle(openEffect, false);
		if ((bool)waitingAnim)
		{
			waitingAnim.wrapMode = WrapMode.Loop;
			waitingAnim.Play();
		}
	}

	public override void InitElement()
	{
		if (commonState == CommonState.None)
		{
			PlayParticle(openEffect, true);
			PlayParticle(waitingEffect, false);
			openModelAnim.Play("anim02");
			PlayAnim(openEffectAnim, true);
			commonState = CommonState.Active;
		}
	}

	public override void ResetElement()
	{
		base.ResetElement();
		PlayParticle(waitingEffect, true);
		PlayParticle(openEffect, false);
		if ((bool)waitingAnim)
		{
			waitingAnim.wrapMode = WrapMode.Loop;
			waitingAnim.Play();
		}
		PlayAnim(openModelAnim, false);
		PlayAnim(openEffectAnim, false);
	}

	[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
	public override void RebirthReadData(object rd_data)
	{
		m_rebirthData = JsonUtility.FromJson<RD_DesertGateWay_DATA>(rd_data as string);
		waitingAnim.SetAnimData(m_rebirthData.waitingAnim, ProcessState.Pause);
		openModelAnim.SetAnimData(m_rebirthData.openModelAnim, ProcessState.Pause);
		openEffectAnim.SetAnimData(m_rebirthData.openEffectAnim, ProcessState.Pause);
		waitingEffect.SetParticlesData(m_rebirthData.waitingEffect, ProcessState.Pause);
		openEffect.SetParticlesData(m_rebirthData.openEffect, ProcessState.Pause);
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override object RebirthWriteData()
	{
		return JsonUtility.ToJson(new RD_DesertGateWay_DATA
		{
			waitingAnim = waitingAnim.GetAnimData(),
			openModelAnim = openModelAnim.GetAnimData(),
			openEffectAnim = openEffectAnim.GetAnimData(),
			waitingEffect = waitingEffect.GetParticlesData(),
			openEffect = openEffect.GetParticlesData()
		});
	}

	[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
	public override void RebirthStartGame(object rd_data)
	{
		waitingAnim.SetAnimData(m_rebirthData.waitingAnim, ProcessState.UnPause);
		openModelAnim.SetAnimData(m_rebirthData.openModelAnim, ProcessState.UnPause);
		openEffectAnim.SetAnimData(m_rebirthData.openEffectAnim, ProcessState.UnPause);
		waitingEffect.SetParticlesData(m_rebirthData.waitingEffect, ProcessState.UnPause);
		openEffect.SetParticlesData(m_rebirthData.openEffect, ProcessState.UnPause);
		m_rebirthData = null;
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
		m_rebirthData = Bson.ToObject<RD_DesertGateWay_DATA>(rd_data);
		waitingAnim.SetAnimData(m_rebirthData.waitingAnim, ProcessState.Pause);
		openModelAnim.SetAnimData(m_rebirthData.openModelAnim, ProcessState.Pause);
		openEffectAnim.SetAnimData(m_rebirthData.openEffectAnim, ProcessState.Pause);
		waitingEffect.SetParticlesData(m_rebirthData.waitingEffect, ProcessState.Pause);
		openEffect.SetParticlesData(m_rebirthData.openEffect, ProcessState.Pause);
	}

	public override byte[] RebirthWriteByteData()
	{
		return Bson.ToBson(new RD_DesertGateWay_DATA
		{
			waitingAnim = waitingAnim.GetAnimData(),
			openModelAnim = openModelAnim.GetAnimData(),
			openEffectAnim = openEffectAnim.GetAnimData(),
			waitingEffect = waitingEffect.GetParticlesData(),
			openEffect = openEffect.GetParticlesData()
		});
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
		waitingAnim.SetAnimData(m_rebirthData.waitingAnim, ProcessState.UnPause);
		openModelAnim.SetAnimData(m_rebirthData.openModelAnim, ProcessState.UnPause);
		openEffectAnim.SetAnimData(m_rebirthData.openEffectAnim, ProcessState.UnPause);
		waitingEffect.SetParticlesData(m_rebirthData.waitingEffect, ProcessState.UnPause);
		openEffect.SetParticlesData(m_rebirthData.openEffect, ProcessState.UnPause);
		m_rebirthData = null;
	}
}
