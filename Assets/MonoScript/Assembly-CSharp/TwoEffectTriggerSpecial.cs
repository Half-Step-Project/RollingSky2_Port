using System;
using System.IO;
using Foundation;
using UnityEngine;

public class TwoEffectTriggerSpecial : BaseEnemy, IBrushTrigger
{
	public enum TwoEffState
	{
		Null,
		Wait,
		Active,
		ActiveTwice,
		End
	}

	[Serializable]
	public struct EffectData : IReadWriteBytes
	{
		public float ResetDistance;

		public Vector3 BeginTriggerPos;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			ResetDistance = bytes.GetSingle(ref startIndex);
			BeginTriggerPos = bytes.GetVector3(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(ResetDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(BeginTriggerPos.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	public static readonly string NodeBeginTrigger = "beginTrigger";

	public static readonly string NodeActiveTrigger = "activeTrigger";

	private Transform model;

	private Transform distanceEff;

	private Transform triggerEff;

	private ParticleSystem[] distanceParticles;

	private ParticleSystem[] triggerParticles;

	private TwoEffState currentState;

	private Animation anim;

	public EffectData data;

	private Transform beginTrigger;

	private Transform activeTrigger;

	private RD_TwoEffectTriggerSpecial_DATA m_rebirthData;

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
		model = base.transform.Find("model");
		distanceEff = base.transform.Find("distanceEffect");
		triggerEff = base.transform.Find("triggerEffect");
		if ((bool)distanceEff)
		{
			distanceParticles = distanceEff.GetComponentsInChildren<ParticleSystem>();
		}
		if ((bool)triggerEff)
		{
			triggerParticles = triggerEff.GetComponentsInChildren<ParticleSystem>();
		}
		model.gameObject.SetActive(true);
		anim = model.GetComponentInChildren<Animation>();
		PlayAnim(anim, true);
		StopDistanceEff();
		StopTriggerEff();
		currentState = TwoEffState.Null;
		beginTrigger = base.transform.Find(NodeBeginTrigger);
		activeTrigger = base.transform.Find(NodeActiveTrigger);
	}

	public override void ResetElement()
	{
		base.ResetElement();
		model.gameObject.SetActive(true);
		StopDistanceEff();
		StopTriggerEff();
		PlayAnim(anim, false);
		currentState = TwoEffState.Null;
	}

	public override void UpdateElement()
	{
		float num = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z;
		if (currentState == TwoEffState.Active && num >= data.ResetDistance)
		{
			StopTriggerEff();
			currentState = TwoEffState.End;
		}
	}

	public void TriggerEnter(BaseRole ball, Collider collider)
	{
		if (currentState == TwoEffState.Null)
		{
			if (collider.transform == beginTrigger)
			{
				model.gameObject.SetActive(true);
				PlayDistanceEff();
				currentState = TwoEffState.Wait;
			}
		}
		else if (currentState == TwoEffState.Wait && collider.transform == activeTrigger)
		{
			model.gameObject.SetActive(false);
			StopDistanceEff();
			PlayTriggerEff();
			currentState = TwoEffState.Active;
		}
	}

	public override void Read(string info)
	{
		data = JsonUtility.FromJson<EffectData>(info);
		base.transform.Find(NodeBeginTrigger).position = data.BeginTriggerPos;
	}

	public override string Write()
	{
		Transform transform = base.transform.Find(NodeBeginTrigger);
		data.BeginTriggerPos = transform.position;
		return JsonUtility.ToJson(data);
	}

	public override void ReadBytes(byte[] bytes)
	{
		data = StructTranslatorUtility.ToStructure<EffectData>(bytes);
		base.transform.Find(NodeBeginTrigger).position = data.BeginTriggerPos;
	}

	public override byte[] WriteBytes()
	{
		Transform transform = base.transform.Find(NodeBeginTrigger);
		data.BeginTriggerPos = transform.position;
		return StructTranslatorUtility.ToByteArray(data);
	}

	private void PlayDistanceEff()
	{
		model.gameObject.SetActive(true);
		distanceEff.gameObject.SetActive(true);
		distanceParticles.PlayParticle();
	}

	private void StopDistanceEff()
	{
		distanceParticles.StopParticle();
		distanceEff.gameObject.SetActive(false);
		model.gameObject.SetActive(false);
	}

	private void PlayTriggerEff()
	{
		triggerParticles.PlayParticle();
	}

	private void StopTriggerEff()
	{
		triggerParticles.StopParticle();
	}

	[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
	public override void RebirthReadData(object rd_data)
	{
		m_rebirthData = JsonUtility.FromJson<RD_TwoEffectTriggerSpecial_DATA>(rd_data as string);
		model.SetTransData(m_rebirthData.model);
		distanceEff.SetTransData(m_rebirthData.distanceEff);
		triggerEff.SetTransData(m_rebirthData.triggerEff);
		distanceParticles.SetParticlesData(m_rebirthData.distanceParticles, ProcessState.Pause);
		triggerParticles.SetParticlesData(m_rebirthData.triggerParticles, ProcessState.Pause);
		currentState = m_rebirthData.currentState;
		anim.SetAnimData(m_rebirthData.anim, ProcessState.Pause);
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override object RebirthWriteData()
	{
		return JsonUtility.ToJson(new RD_TwoEffectTriggerSpecial_DATA
		{
			model = model.GetTransData(),
			distanceEff = distanceEff.GetTransData(),
			triggerEff = triggerEff.GetTransData(),
			distanceParticles = distanceParticles.GetParticlesData(),
			triggerParticles = triggerParticles.GetParticlesData(),
			currentState = currentState,
			anim = anim.GetAnimData()
		});
	}

	[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
	public override void RebirthStartGame(object rd_data)
	{
		anim.SetAnimData(m_rebirthData.anim, ProcessState.UnPause);
		distanceParticles.SetParticlesData(m_rebirthData.distanceParticles, ProcessState.UnPause);
		triggerParticles.SetParticlesData(m_rebirthData.triggerParticles, ProcessState.UnPause);
		m_rebirthData = null;
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
		m_rebirthData = Bson.ToObject<RD_TwoEffectTriggerSpecial_DATA>(rd_data);
		model.SetTransData(m_rebirthData.model);
		distanceEff.SetTransData(m_rebirthData.distanceEff);
		triggerEff.SetTransData(m_rebirthData.triggerEff);
		distanceParticles.SetParticlesData(m_rebirthData.distanceParticles, ProcessState.Pause);
		triggerParticles.SetParticlesData(m_rebirthData.triggerParticles, ProcessState.Pause);
		currentState = m_rebirthData.currentState;
		anim.SetAnimData(m_rebirthData.anim, ProcessState.Pause);
	}

	public override byte[] RebirthWriteByteData()
	{
		return Bson.ToBson(new RD_TwoEffectTriggerSpecial_DATA
		{
			model = model.GetTransData(),
			distanceEff = distanceEff.GetTransData(),
			triggerEff = triggerEff.GetTransData(),
			distanceParticles = distanceParticles.GetParticlesData(),
			triggerParticles = triggerParticles.GetParticlesData(),
			currentState = currentState,
			anim = anim.GetAnimData()
		});
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
		anim.SetAnimData(m_rebirthData.anim, ProcessState.UnPause);
		distanceParticles.SetParticlesData(m_rebirthData.distanceParticles, ProcessState.UnPause);
		triggerParticles.SetParticlesData(m_rebirthData.triggerParticles, ProcessState.UnPause);
		m_rebirthData = null;
	}
}
