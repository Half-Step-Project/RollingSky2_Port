using System;
using Foundation;
using UnityEngine;

public class AnimEffectTrigger : BaseTriggerBox
{
	private Animation anim;

	private Transform model;

	private Transform effect;

	private Transform starTrans;

	private MeshRenderer starMeshRender;

	protected ParticleSystem[] particles;

	private RD_AnimEffectTrigger_DATA m_rebirthData;

	public override bool IfRebirthRecord
	{
		get
		{
			return true;
		}
	}

	public override void TriggerEnter(BaseRole ball)
	{
		if ((bool)anim)
		{
			anim.gameObject.SetActive(false);
		}
		if ((bool)model)
		{
			model.gameObject.SetActive(false);
		}
		if ((bool)starMeshRender)
		{
			starMeshRender.enabled = false;
		}
		PlayParticle();
		PlaySoundEffect();
	}

	public override void Initialize()
	{
		base.Initialize();
		anim = GetComponentInChildren<Animation>();
		if ((bool)anim)
		{
			anim.gameObject.SetActive(true);
			anim.wrapMode = WrapMode.Loop;
			anim["anim01"].normalizedTime = 0f;
			anim.Play();
		}
		model = base.transform.Find("model");
		if ((bool)model)
		{
			model.gameObject.SetActive(true);
			starTrans = model.Find("starEffect");
			if ((bool)starTrans)
			{
				starMeshRender = starTrans.gameObject.GetComponent<MeshRenderer>();
				if ((bool)starMeshRender)
				{
					starMeshRender.enabled = true;
				}
			}
		}
		effect = base.transform.Find("effect");
		if ((bool)effect)
		{
			particles = effect.GetComponentsInChildren<ParticleSystem>();
			StopParticle();
		}
	}

	public override void ResetElement()
	{
		if ((bool)anim)
		{
			anim.gameObject.SetActive(true);
			anim.Play();
			anim["anim01"].normalizedTime = 0f;
			anim.Sample();
			anim.Stop();
		}
		if ((bool)model)
		{
			model.gameObject.SetActive(true);
		}
		if ((bool)starMeshRender)
		{
			starMeshRender.enabled = true;
		}
		StopParticle();
	}

	protected void PlayParticle()
	{
		if (particles != null)
		{
			for (int i = 0; i < particles.Length; i++)
			{
				particles[i].Play();
			}
		}
	}

	protected void StopParticle()
	{
		if (particles != null)
		{
			for (int i = 0; i < particles.Length; i++)
			{
				particles[i].Stop();
			}
		}
	}

	[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
	public override void RebirthReadData(object rd_data)
	{
		m_rebirthData = JsonUtility.FromJson<RD_AnimEffectTrigger_DATA>(rd_data as string);
		model.SetTransData(m_rebirthData.model);
		effect.SetTransData(m_rebirthData.effect);
		anim.SetAnimData(m_rebirthData.anim, ProcessState.Pause);
		particles.SetParticlesData(m_rebirthData.particles, ProcessState.Pause);
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override object RebirthWriteData()
	{
		return JsonUtility.ToJson(new RD_AnimEffectTrigger_DATA
		{
			anim = anim.GetAnimData(),
			model = model.GetTransData(),
			effect = effect.GetTransData(),
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
		m_rebirthData = Bson.ToObject<RD_AnimEffectTrigger_DATA>(rd_data);
		model.SetTransData(m_rebirthData.model);
		effect.SetTransData(m_rebirthData.effect);
		anim.SetAnimData(m_rebirthData.anim, ProcessState.Pause);
		particles.SetParticlesData(m_rebirthData.particles, ProcessState.Pause);
	}

	public override byte[] RebirthWriteByteData()
	{
		return Bson.ToBson(new RD_AnimEffectTrigger_DATA
		{
			anim = anim.GetAnimData(),
			model = model.GetTransData(),
			effect = effect.GetTransData(),
			particles = particles.GetParticlesData()
		});
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
		anim.SetAnimData(m_rebirthData.anim, ProcessState.UnPause);
		particles.SetParticlesData(m_rebirthData.particles, ProcessState.UnPause);
		m_rebirthData = null;
	}

	private void OnTriggerEnter(Collider other)
	{
		if ((bool)other.gameObject.GetGameComponent<AutoMoveJumpTrigger>() && (bool)starMeshRender)
		{
			starMeshRender.enabled = false;
		}
	}
}
