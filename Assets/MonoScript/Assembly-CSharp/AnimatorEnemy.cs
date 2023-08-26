using System;
using System.IO;
using Foundation;
using UnityEngine;

public class AnimatorEnemy : BaseEnemy
{
	[Serializable]
	public struct ElementData : IReadWriteBytes
	{
		[Header("动画状态名称")]
		public string m_animationStateName;

		[Header("放入地图时，触发开关")]
		public bool m_enableInit;

		[Header("点击start按钮时，触发开关")]
		public bool m_enableInitElement;

		[Header("距离触发，触发开关")]
		public bool m_enableDistance;

		public float m_begin;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			m_animationStateName = bytes.GetStringWithSize(ref startIndex);
			m_enableInit = bytes.GetBoolean(ref startIndex);
			m_enableInitElement = bytes.GetBoolean(ref startIndex);
			m_enableDistance = bytes.GetBoolean(ref startIndex);
			m_begin = bytes.GetSingle(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(m_animationStateName.GetBytesWithSize(), ref offset);
				memoryStream.WriteByteArray(m_enableInit.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_enableInitElement.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_enableDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_begin.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	public ElementData m_data;

	public Animator m_animator;

	private float m_distance;

	private bool m_isCanDistance;

	private static readonly float backwardDisntace = -12f;

	private RD_AnimatorEnemy_DATA m_rebirthData;

	public override bool IfRebirthRecord
	{
		get
		{
			return true;
		}
	}

	public override void Initialize()
	{
		if (m_animator == null)
		{
			m_animator = base.gameObject.GetComponentInChildren<Animator>();
		}
		if (m_data.m_enableInit && m_animator != null)
		{
			m_animator.Play(m_data.m_animationStateName, 0, 0f);
		}
		base.Initialize();
		if (effectChild != null)
		{
			effectChild.gameObject.SetActive(true);
		}
		PlayParticle();
	}

	public override void InitElement()
	{
		if (m_animator == null)
		{
			m_animator = base.gameObject.GetComponentInChildren<Animator>();
		}
		if (m_data.m_enableInitElement && m_animator != null)
		{
			m_animator.Play(m_data.m_animationStateName, 0, 0f);
		}
		base.InitElement();
	}

	public override void UpdateElement()
	{
		base.UpdateElement();
		if (m_data.m_enableDistance && m_animator != null)
		{
			if (m_isCanDistance)
			{
				if (Application.isPlaying)
				{
					m_distance = base.transform.parent.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z;
				}
				if (m_distance >= m_data.m_begin)
				{
					m_animator.Play(m_data.m_animationStateName, 0, 0f);
				}
			}
			m_isCanDistance = false;
		}
		if (effectChild != null && effectChild.gameObject.activeSelf && base.transform.parent.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z >= backwardDisntace)
		{
			effectChild.gameObject.SetActive(false);
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
		m_isCanDistance = true;
		if (effectChild != null)
		{
			effectChild.gameObject.SetActive(true);
		}
		StopParticle();
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override object RebirthWriteData()
	{
		return JsonUtility.ToJson(new RD_AnimatorEnemy_DATA
		{
			m_animator = m_animator.GetAnimData(),
			m_isCanDistance = (m_isCanDistance ? 1 : 0),
			particles = particles.GetParticlesData(),
			commonState = commonState
		});
	}

	[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
	public override void RebirthReadData(object rd_data)
	{
		m_rebirthData = JsonUtility.FromJson<RD_AnimatorEnemy_DATA>(rd_data as string);
		m_animator.SetAnimData(m_rebirthData.m_animator, ProcessState.Pause);
		m_isCanDistance = m_rebirthData.m_isCanDistance == 1;
		particles.SetParticlesData(m_rebirthData.particles, ProcessState.Pause);
		commonState = m_rebirthData.commonState;
	}

	[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
	public override void RebirthStartGame(object rd_data)
	{
		if (m_rebirthData != null)
		{
			m_animator.SetAnimData(m_rebirthData.m_animator, ProcessState.UnPause);
			particles.SetParticlesData(m_rebirthData.particles, ProcessState.UnPause);
		}
		m_rebirthData = null;
	}

	public override byte[] RebirthWriteByteData()
	{
		return Bson.ToBson(new RD_AnimatorEnemy_DATA
		{
			m_animator = m_animator.GetAnimData(),
			m_isCanDistance = (m_isCanDistance ? 1 : 0),
			particles = particles.GetParticlesData(),
			commonState = commonState
		});
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
		m_rebirthData = Bson.ToObject<RD_AnimatorEnemy_DATA>(rd_data);
		m_animator.SetAnimData(m_rebirthData.m_animator, ProcessState.Pause);
		m_isCanDistance = m_rebirthData.m_isCanDistance == 1;
		particles.SetParticlesData(m_rebirthData.particles, ProcessState.Pause);
		commonState = m_rebirthData.commonState;
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
		if (m_rebirthData != null)
		{
			m_animator.SetAnimData(m_rebirthData.m_animator, ProcessState.UnPause);
			particles.SetParticlesData(m_rebirthData.particles, ProcessState.UnPause);
		}
		m_rebirthData = null;
	}
}
