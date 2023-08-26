using System;
using System.Collections.Generic;
using System.IO;
using Foundation;
using UnityEngine;

public class ParticleSystemAlphaEnemy : BaseEnemy
{
	[Serializable]
	public struct ElementData : IReadWriteBytes
	{
		[Header("shader 中Color的变量名称")]
		public string m_propertyNameForColor;

		public float m_begin;

		public float m_end;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			m_propertyNameForColor = bytes.GetStringWithSize(ref startIndex);
			m_begin = bytes.GetSingle(ref startIndex);
			m_end = bytes.GetSingle(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(m_propertyNameForColor.GetBytesWithSize(), ref offset);
				memoryStream.WriteByteArray(m_begin.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_end.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	public ElementData m_data;

	private float m_distance;

	private List<Renderer> m_renderers = new List<Renderer>();

	private List<Color> m_alphas = new List<Color>();

	private List<ParticleSystem> m_trailsParticleSystems = new List<ParticleSystem>();

	private List<Color> m_trailsAlphas = new List<Color>();

	private Color m_color = Color.white;

	private ParticleSystem.TrailModule m_trailModule0;

	private Animation m_animation;

	private RD_ParticleSystemAlphaEnemy_DATA m_rebirthData;

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
		m_renderers.Clear();
		m_alphas.Clear();
		m_trailsParticleSystems.Clear();
		ParticleSystem[] componentsInChildren = base.gameObject.GetComponentsInChildren<ParticleSystem>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].trails.enabled)
			{
				m_trailsParticleSystems.Add(componentsInChildren[i]);
				m_trailsAlphas.Add(componentsInChildren[i].trails.colorOverTrail.color);
			}
		}
		Renderer[] componentsInChildren2 = base.gameObject.GetComponentsInChildren<Renderer>(true);
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			m_renderers.Add(componentsInChildren2[j]);
			m_alphas.Add(MaterialTool.GetMaterialColor(componentsInChildren2[j], m_data.m_propertyNameForColor));
		}
		PlayParticle();
		if (m_animation == null)
		{
			m_animation = base.gameObject.GetComponentInChildren<Animation>();
		}
		if (m_animation != null)
		{
			m_animation.Play();
		}
	}

	public override void UpdateElement()
	{
		base.UpdateElement();
		m_distance = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z;
		if (m_distance >= m_data.m_begin && m_distance < m_data.m_end)
		{
			float t = 1f - (m_distance - m_data.m_begin) / (m_data.m_end - m_data.m_begin);
			for (int i = 0; i < m_renderers.Count; i++)
			{
				m_color.r = m_alphas[i].r;
				m_color.g = m_alphas[i].g;
				m_color.b = m_alphas[i].b;
				m_color.a = Mathf.Lerp(0f, m_alphas[i].a, t);
				MaterialTool.SetMaterialColor(m_renderers[i], m_data.m_propertyNameForColor, m_color);
			}
			for (int j = 0; j < m_trailsParticleSystems.Count; j++)
			{
				m_color.r = m_trailsAlphas[j].r;
				m_color.g = m_trailsAlphas[j].g;
				m_color.b = m_trailsAlphas[j].b;
				m_color.a = Mathf.Lerp(0f, m_trailsAlphas[j].a, t);
				m_trailModule0 = m_trailsParticleSystems[j].trails;
				m_trailModule0.colorOverTrail = m_color;
			}
		}
	}

	public override string Write()
	{
		return JsonUtility.ToJson(m_data);
	}

	public override void Read(string info)
	{
		m_data = JsonUtility.FromJson<ElementData>(info);
	}

	public override void ReadBytes(byte[] bytes)
	{
		m_data = StructTranslatorUtility.ToStructure<ElementData>(bytes);
	}

	public override byte[] WriteBytes()
	{
		return StructTranslatorUtility.ToByteArray(m_data);
	}

	public override void SetDefaultValue(object[] objs)
	{
		m_data = (ElementData)objs[0];
	}

	public override void ResetElement()
	{
		base.ResetElement();
		for (int i = 0; i < m_renderers.Count; i++)
		{
			MaterialTool.SetMaterialColor(m_renderers[i], m_data.m_propertyNameForColor, m_alphas[i]);
		}
		for (int j = 0; j < m_trailsParticleSystems.Count; j++)
		{
			m_trailModule0 = m_trailsParticleSystems[j].trails;
			m_trailModule0.colorOverTrail = m_trailsAlphas[j];
		}
		StopParticle();
		if (m_animation != null)
		{
			m_animation.Stop();
		}
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override object RebirthWriteData()
	{
		return JsonUtility.ToJson(new RD_ParticleSystemAlphaEnemy_DATA
		{
			m_animation = m_animation.GetAnimData(),
			particles = particles.GetParticlesData()
		});
	}

	[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
	public override void RebirthReadData(object rd_data)
	{
		m_rebirthData = JsonUtility.FromJson<RD_ParticleSystemAlphaEnemy_DATA>(rd_data as string);
		m_animation.SetAnimData(m_rebirthData.m_animation, ProcessState.Pause);
		particles.SetParticlesData(m_rebirthData.particles, ProcessState.Pause);
	}

	[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
	public override void RebirthStartGame(object rd_data)
	{
		if (m_rebirthData != null)
		{
			m_animation.SetAnimData(m_rebirthData.m_animation, ProcessState.UnPause);
			particles.SetParticlesData(m_rebirthData.particles, ProcessState.UnPause);
		}
		m_rebirthData = null;
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
		m_rebirthData = Bson.ToObject<RD_ParticleSystemAlphaEnemy_DATA>(rd_data);
		m_animation.SetAnimData(m_rebirthData.m_animation, ProcessState.Pause);
		particles.SetParticlesData(m_rebirthData.particles, ProcessState.Pause);
	}

	public override byte[] RebirthWriteByteData()
	{
		return Bson.ToBson(new RD_ParticleSystemAlphaEnemy_DATA
		{
			m_animation = m_animation.GetAnimData(),
			particles = particles.GetParticlesData()
		});
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
		if (m_rebirthData != null)
		{
			m_animation.SetAnimData(m_rebirthData.m_animation, ProcessState.UnPause);
			particles.SetParticlesData(m_rebirthData.particles, ProcessState.UnPause);
		}
		m_rebirthData = null;
	}
}
