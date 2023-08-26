using System;
using System.IO;
using Foundation;
using UnityEngine;

public class AnimParticleEnemy : BaseEnemy
{
	public enum AnimState
	{
		None,
		Active,
		End
	}

	public enum EffState
	{
		None,
		Active,
		End
	}

	[Serializable]
	public struct EnemyData : IReadWriteBytes
	{
		public float AnimBeginDistance;

		public float AnimResetDistance;

		public float BaseBallSpeed;

		public bool AnimIfAutoPlay;

		public bool AnimIfLoop;

		public float EffBeginDistance;

		public float EffResetDistance;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			AnimBeginDistance = bytes.GetSingle(ref startIndex);
			AnimResetDistance = bytes.GetSingle(ref startIndex);
			BaseBallSpeed = bytes.GetSingle(ref startIndex);
			AnimIfAutoPlay = bytes.GetBoolean(ref startIndex);
			AnimIfLoop = bytes.GetBoolean(ref startIndex);
			EffBeginDistance = bytes.GetSingle(ref startIndex);
			EffResetDistance = bytes.GetSingle(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(AnimBeginDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(AnimResetDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(BaseBallSpeed.GetBytes(), ref offset);
				memoryStream.WriteByteArray(AnimIfAutoPlay.GetBytes(), ref offset);
				memoryStream.WriteByteArray(AnimIfLoop.GetBytes(), ref offset);
				memoryStream.WriteByteArray(EffBeginDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(EffResetDistance.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	public EnemyData data;

	private Animation anim;

	private AnimState animState;

	private EffState effState;

	private RD_AnimParticleEnemy_DATA m_rebirthData;

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
		animState = AnimState.None;
		effState = EffState.None;
		anim = GetComponentInChildren<Animation>();
		if ((bool)anim && data.BaseBallSpeed > 0f)
		{
			float speed = Railway.theRailway.SpeedForward / data.BaseBallSpeed;
			anim["anim01"].speed = speed;
			if (data.AnimIfAutoPlay)
			{
				anim.Play();
			}
		}
	}

	public override void ResetElement()
	{
		base.ResetElement();
		OnStopAnim();
		OnStopEffect();
	}

	public override void UpdateElement()
	{
		float num = 0f;
		num = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z;
		if (animState == AnimState.None)
		{
			if (num >= data.AnimBeginDistance)
			{
				OnPlayAnim();
				animState = AnimState.Active;
			}
		}
		else if (animState == AnimState.Active && num >= data.AnimResetDistance)
		{
			OnStopAnim();
			animState = AnimState.End;
		}
		if (effState == EffState.None)
		{
			if (num >= data.EffBeginDistance)
			{
				OnPlayEffect();
				effState = EffState.Active;
			}
		}
		else if (effState == EffState.Active && num >= data.EffResetDistance)
		{
			OnStopEffect();
			effState = EffState.End;
		}
	}

	private void OnPlayAnim()
	{
		if ((bool)anim)
		{
			if (data.AnimIfLoop)
			{
				anim.wrapMode = WrapMode.Loop;
			}
			else
			{
				anim.wrapMode = WrapMode.ClampForever;
			}
			anim["anim01"].normalizedTime = 0f;
			anim.Play();
		}
	}

	private void OnStopAnim()
	{
		if ((bool)anim)
		{
			anim.Play();
			anim["anim01"].normalizedTime = 0f;
			anim.Sample();
			anim.Stop();
		}
	}

	public override void SetBakeState()
	{
		anim = GetComponentInChildren<Animation>();
		if ((bool)anim && anim["anim_bake"] != null)
		{
			anim.Play("anim_bake");
			anim["anim_bake"].normalizedTime = 0.5f;
			anim.Sample();
			anim.Stop();
		}
	}

	public override void SetBaseState()
	{
		anim = GetComponentInChildren<Animation>();
		OnStopAnim();
	}

	private void OnPlayEffect()
	{
		PlayParticle();
	}

	private void OnStopEffect()
	{
		StopParticle();
	}

	private void OnTriggerEnter(Collider collider)
	{
		TriggerEnter(BaseRole.theBall);
	}

	public override void Read(string info)
	{
		data = JsonUtility.FromJson<EnemyData>(info);
	}

	public override string Write()
	{
		return JsonUtility.ToJson(data);
	}

	public override void ReadBytes(byte[] bytes)
	{
		data = StructTranslatorUtility.ToStructure<EnemyData>(bytes);
	}

	public override byte[] WriteBytes()
	{
		return StructTranslatorUtility.ToByteArray(data);
	}

	[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
	public override void RebirthReadData(object rd_data)
	{
		m_rebirthData = JsonUtility.FromJson<RD_AnimParticleEnemy_DATA>(rd_data as string);
		base.RebirthReadData((object)m_rebirthData.baseData);
		animState = m_rebirthData.animState;
		effState = m_rebirthData.effState;
		anim.SetAnimData(m_rebirthData.anim, ProcessState.Pause);
		particles.SetParticlesData(m_rebirthData.particles, ProcessState.Pause);
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override object RebirthWriteData()
	{
		return JsonUtility.ToJson(new RD_AnimParticleEnemy_DATA
		{
			baseData = (base.RebirthWriteData() as string),
			animState = animState,
			effState = effState,
			anim = anim.GetAnimData(),
			particles = particles.GetParticlesData()
		});
	}

	[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
	public override void RebirthStartGame(object rd_data)
	{
		anim.SetAnimData(m_rebirthData.anim, ProcessState.UnPause);
		particles.SetParticlesData(m_rebirthData.particles, ProcessState.UnPause);
		m_rebirthData = null;
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
		m_rebirthData = Bson.ToObject<RD_AnimParticleEnemy_DATA>(rd_data);
		base.RebirthReadData((object)m_rebirthData.baseData);
		animState = m_rebirthData.animState;
		effState = m_rebirthData.effState;
		anim.SetAnimData(m_rebirthData.anim, ProcessState.Pause);
		particles.SetParticlesData(m_rebirthData.particles, ProcessState.Pause);
	}

	public override byte[] RebirthWriteByteData()
	{
		return Bson.ToBson(new RD_AnimParticleEnemy_DATA
		{
			baseData = (base.RebirthWriteData() as string),
			animState = animState,
			effState = effState,
			anim = anim.GetAnimData(),
			particles = particles.GetParticlesData()
		});
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
		anim.SetAnimData(m_rebirthData.anim, ProcessState.UnPause);
		particles.SetParticlesData(m_rebirthData.particles, ProcessState.UnPause);
		m_rebirthData = null;
	}
}
