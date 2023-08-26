using System;
using System.Collections.Generic;
using System.IO;
using Foundation;
using UnityEngine;

public class RelativeDisplacementMotionTriggerBox : BaseTriggerBox, IBrushTrigger, IRebirth
{
	public enum ElementState
	{
		Null,
		Wait,
		Run,
		End,
		Die
	}

	[Serializable]
	public struct ElementData : IReadWriteBytes
	{
		public string m_defaultAnimationName;

		public NodeData m_startData;

		public NodeData m_endData;

		public NodeData[] m_runData;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			m_defaultAnimationName = bytes.GetStringWithSize(ref startIndex);
			m_startData.ReadBytes(bytes, ref startIndex);
			m_endData.ReadBytes(bytes, ref startIndex);
			int @int = bytes.GetInt32(ref startIndex);
			m_runData = new NodeData[@int];
			for (int i = 0; i < @int; i++)
			{
				m_runData[i].ReadBytes(bytes, ref startIndex);
			}
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(m_defaultAnimationName.GetBytesWithSize(), ref offset);
				memoryStream.WriteByteArray(m_startData.WriteBytes(), ref offset);
				memoryStream.WriteByteArray(m_endData.WriteBytes(), ref offset);
				int value = m_runData.Length;
				memoryStream.WriteByteArray(value.GetBytes(), ref offset);
				NodeData[] runData = m_runData;
				foreach (NodeData nodeData in runData)
				{
					memoryStream.WriteByteArray(nodeData.WriteBytes(), ref offset);
				}
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	[Serializable]
	public struct NodeData
	{
		[HideInInspector]
		public string m_colliderName;

		[Header("animatorState 动画名称 ")]
		public string m_animationName;

		[Header("特效路径 当前对象/    -    ps. effect/a/b ")]
		public string m_prarticleSystemName;

		[HideInInspector]
		public Vector3 m_localPosition;

		[HideInInspector]
		public Quaternion m_localRotate;

		[HideInInspector]
		public Vector3 m_localSize;

		[HideInInspector]
		public Vector3 m_colliderCenter;

		[HideInInspector]
		public Vector3 m_colliderSize;

		public void ReadBytes(byte[] bytes, ref int startIndex)
		{
			m_colliderName = bytes.GetStringWithSize(ref startIndex);
			m_animationName = bytes.GetStringWithSize(ref startIndex);
			m_prarticleSystemName = bytes.GetStringWithSize(ref startIndex);
			m_localPosition = bytes.GetVector3(ref startIndex);
			m_localRotate = bytes.GetQuaternion(ref startIndex);
			m_localSize = bytes.GetVector3(ref startIndex);
			m_colliderCenter = bytes.GetVector3(ref startIndex);
			m_colliderSize = bytes.GetVector3(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(m_colliderName.GetBytesWithSize(), ref offset);
				memoryStream.WriteByteArray(m_animationName.GetBytesWithSize(), ref offset);
				memoryStream.WriteByteArray(m_prarticleSystemName.GetBytesWithSize(), ref offset);
				memoryStream.WriteByteArray(m_localPosition.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_localRotate.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_localSize.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_colliderCenter.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_colliderSize.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	public struct RebirthData
	{
		public int m_animationHash;

		public float m_animationNormalizedTime;

		public string m_prarticleSystemName;

		public string m_prarticleSystemTime;
	}

	[HideInInspector]
	public Transform m_modelTransform;

	public Animator m_animator;

	[HideInInspector]
	public BoxCollider m_startCollider;

	[HideInInspector]
	public BoxCollider m_endCollider;

	[HideInInspector]
	public Transform m_runColliderTransform;

	public BoxCollider[] m_runColliders;

	private Dictionary<string, ParticleSystem> m_particleSystems = new Dictionary<string, ParticleSystem>();

	private Vector3 m_startInverse = Vector3.zero;

	private Vector3 m_targetPosition = Vector3.zero;

	private Vector3 m_railwayInverse = Vector3.zero;

	public ElementData m_data;

	public ElementState m_state;

	private RD_RelativeDisplacementMotionTriggerBox_DATA m_rebirthData;

	public override bool CanRecycle
	{
		get
		{
			return false;
		}
	}

	public override bool IfRebirthRecord
	{
		get
		{
			return true;
		}
	}

	public override void Initialize()
	{
		RefreshGameObjectData();
		RefreshPrarticleSystems();
		RefreshStartAddEndColliders();
		RefreshRunColliders();
		if (m_animator != null)
		{
			m_animator.enabled = true;
		}
		m_startInverse = base.gameObject.transform.InverseTransformPoint(m_modelTransform.transform.position);
		m_state = ElementState.Wait;
	}

	public override void LateInitialize()
	{
		PlayAnimation(m_data.m_defaultAnimationName);
		base.LateInitialize();
	}

	public override void UpdateElement()
	{
		if (m_state == ElementState.Run && m_modelTransform != null)
		{
			m_targetPosition = Railway.theRailway.transform.TransformPoint(m_railwayInverse);
			m_modelTransform.transform.position = m_targetPosition;
		}
		base.UpdateElement();
	}

	public override void ResetElement()
	{
		m_targetPosition = base.gameObject.transform.TransformPoint(m_startInverse);
		m_modelTransform.transform.position = m_targetPosition;
		m_state = ElementState.Null;
		if (m_animator != null)
		{
			m_animator.enabled = false;
		}
		base.ResetElement();
	}

	public override string Write()
	{
		WriteNodeData(ref m_data.m_startData, m_startCollider);
		WriteNodeData(ref m_data.m_endData, m_endCollider);
		if (m_data.m_runData != null)
		{
			for (int i = 0; i < m_data.m_runData.Length; i++)
			{
				WriteNodeData(ref m_data.m_runData[i], m_runColliders[i]);
			}
		}
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
		WriteNodeData(ref m_data.m_startData, m_startCollider);
		WriteNodeData(ref m_data.m_endData, m_endCollider);
		if (m_data.m_runData != null)
		{
			for (int i = 0; i < m_data.m_runData.Length; i++)
			{
				WriteNodeData(ref m_data.m_runData[i], m_runColliders[i]);
			}
		}
		return StructTranslatorUtility.ToByteArray(m_data);
	}

	public override void SetDefaultValue(object[] objs)
	{
		m_data = (ElementData)objs[0];
	}

	public void TriggerEnter(BaseRole ball, Collider collider)
	{
		if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
		{
			m_state = ElementState.Die;
			return;
		}
		if (collider == m_startCollider)
		{
			m_state = ElementState.Run;
			PlayAnimation(m_data.m_startData);
			PlayParticleSystem(m_data.m_startData);
			m_railwayInverse = Railway.theRailway.transform.InverseTransformPoint(m_modelTransform.transform.position);
			return;
		}
		if (collider == m_endCollider)
		{
			m_state = ElementState.End;
			PlayAnimation(m_data.m_endData);
			PlayParticleSystem(m_data.m_endData);
			return;
		}
		for (int i = 0; i < m_runColliders.Length; i++)
		{
			if (m_runColliders[i] == collider)
			{
				PlayAnimation(m_data.m_runData[i]);
				PlayParticleSystem(m_data.m_runData[i]);
				break;
			}
		}
	}

	private void PlayAnimation(NodeData data)
	{
		if (!string.IsNullOrEmpty(data.m_animationName))
		{
			PlayAnimation(data.m_animationName);
		}
	}

	private void PlayAnimation(string animationName)
	{
		if (m_animator != null)
		{
			m_animator.Play(animationName, 0, 0f);
		}
	}

	private void PlayParticleSystem(NodeData data)
	{
		ParticleSystem value = null;
		if (m_particleSystems.TryGetValue(data.m_prarticleSystemName, out value) && value != null)
		{
			value.Play(true);
		}
	}

	public void RefreshGameObjectData()
	{
		if (m_modelTransform == null)
		{
			m_modelTransform = base.gameObject.transform.Find("model");
			if (m_modelTransform != null)
			{
				m_animator = m_modelTransform.GetComponentInChildren<Animator>();
			}
		}
		if (m_startCollider == null)
		{
			m_startCollider = base.gameObject.transform.Find("startCollider").GetComponent<BoxCollider>();
		}
		if (m_endCollider == null)
		{
			m_endCollider = base.gameObject.transform.Find("endCollider").GetComponent<BoxCollider>();
		}
		if (m_runColliderTransform == null)
		{
			m_runColliderTransform = base.gameObject.transform.Find("run");
		}
	}

	private void RefreshPrarticleSystems()
	{
		if (m_particleSystems != null && m_particleSystems.Count != 0)
		{
			return;
		}
		List<NodeData> list = new List<NodeData>();
		list.Add(m_data.m_startData);
		list.Add(m_data.m_endData);
		if (m_data.m_runData != null)
		{
			list.AddRange(m_data.m_runData);
		}
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			if (string.IsNullOrEmpty(list[i].m_prarticleSystemName))
			{
				continue;
			}
			Transform transform = base.gameObject.transform.Find(list[i].m_prarticleSystemName);
			if (transform != null)
			{
				ParticleSystem component = transform.gameObject.GetComponent<ParticleSystem>();
				if (component != null)
				{
					m_particleSystems[list[i].m_prarticleSystemName] = component;
				}
			}
		}
	}

	public void RefreshStartAddEndColliders()
	{
		if (m_runColliderTransform == null)
		{
			m_runColliderTransform = base.gameObject.transform.Find("run");
		}
		m_startCollider.center = m_data.m_startData.m_colliderCenter;
		m_startCollider.size = m_data.m_startData.m_colliderSize;
		m_startCollider.gameObject.transform.localPosition = m_data.m_startData.m_localPosition;
		m_startCollider.gameObject.transform.localRotation = m_data.m_startData.m_localRotate;
		m_startCollider.gameObject.transform.localScale = m_data.m_startData.m_localSize;
		m_endCollider.center = m_data.m_endData.m_colliderCenter;
		m_endCollider.size = m_data.m_endData.m_colliderSize;
		m_endCollider.gameObject.transform.localPosition = m_data.m_endData.m_localPosition;
		m_endCollider.gameObject.transform.localRotation = m_data.m_endData.m_localRotate;
		m_endCollider.gameObject.transform.localScale = m_data.m_endData.m_localSize;
	}

	public void RefreshRunColliders()
	{
		if (m_runColliderTransform == null)
		{
			m_runColliderTransform = base.gameObject.transform.Find("run");
		}
		if (m_data.m_runData == null)
		{
			return;
		}
		m_runColliders = m_runColliderTransform.GetComponentsInChildren<BoxCollider>();
		for (int i = 0; i < m_runColliders.Length; i++)
		{
			BoxCollider boxCollider = m_runColliders[i];
			if (i < m_data.m_runData.Length)
			{
				boxCollider.center = m_data.m_runData[i].m_colliderCenter;
				boxCollider.size = m_data.m_runData[i].m_colliderSize;
				boxCollider.isTrigger = true;
				boxCollider.transform.localPosition = m_data.m_runData[i].m_localPosition;
				boxCollider.transform.localRotation = m_data.m_runData[i].m_localRotate;
				boxCollider.transform.localScale = m_data.m_runData[i].m_localSize;
				boxCollider.enabled = true;
			}
			else
			{
				boxCollider.enabled = false;
			}
		}
	}

	private void WriteNodeData(ref NodeData nodeData, BoxCollider collider)
	{
		nodeData.m_localPosition = collider.transform.localPosition;
		nodeData.m_localRotate = collider.transform.localRotation;
		nodeData.m_localSize = collider.transform.localScale;
		nodeData.m_colliderCenter = collider.center;
		nodeData.m_colliderSize = collider.size;
	}

	[ContextMenu("Add a run collider")]
	public void OnAddARunCollider()
	{
		if (m_runColliders == null)
		{
			m_runColliders = new BoxCollider[0];
		}
		GameObject obj = new GameObject("run" + m_runColliders.Length);
		BoxCollider boxCollider = obj.AddComponent<BoxCollider>();
		boxCollider.center = new Vector3(0f, 2.5f, 0f);
		boxCollider.size = new Vector3(10f, 5f, 0.5f);
		boxCollider.isTrigger = true;
		obj.layer = LayerMask.NameToLayer("Ground");
		obj.transform.parent = m_runColliderTransform;
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localRotation = Quaternion.identity;
		obj.transform.localScale = Vector3.one;
	}

	[ContextMenu("write info")]
	private void OnWriteInfo()
	{
		Debug.Log(Write());
	}

	public bool IsRecordRebirth()
	{
		return true;
	}

	public object GetRebirthData(Vector3 position, Quaternion rotation, Vector3 scale)
	{
		RebirthData rebirthData = default(RebirthData);
		if (m_animator != null)
		{
			AnimatorStateInfo currentAnimatorStateInfo = m_animator.GetCurrentAnimatorStateInfo(0);
			rebirthData.m_animationHash = currentAnimatorStateInfo.shortNameHash;
			rebirthData.m_animationNormalizedTime = currentAnimatorStateInfo.normalizedTime;
		}
		return rebirthData;
	}

	public void ResetBySavePointData(object obj)
	{
		RebirthData rebirthData = (RebirthData)obj;
		if (m_animator != null)
		{
			m_animator.Play(rebirthData.m_animationHash, 0, rebirthData.m_animationNormalizedTime);
			m_animator.Update(rebirthData.m_animationNormalizedTime);
			m_animator.enabled = false;
		}
	}

	public void StartRunningForRebirthData(object obj)
	{
		if (m_animator != null)
		{
			m_animator.enabled = true;
		}
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override object RebirthWriteData()
	{
		RD_RelativeDisplacementMotionTriggerBox_DATA rD_RelativeDisplacementMotionTriggerBox_DATA = new RD_RelativeDisplacementMotionTriggerBox_DATA();
		rD_RelativeDisplacementMotionTriggerBox_DATA.m_state = m_state;
		rD_RelativeDisplacementMotionTriggerBox_DATA.m_railwayInverse = m_railwayInverse;
		rD_RelativeDisplacementMotionTriggerBox_DATA.m_modelTransform = m_modelTransform.GetTransData();
		rD_RelativeDisplacementMotionTriggerBox_DATA.m_animator = m_animator.GetAnimData();
		rD_RelativeDisplacementMotionTriggerBox_DATA.particles = new RD_ElementParticle_DATA[m_particleSystems.Count];
		int num = 0;
		foreach (KeyValuePair<string, ParticleSystem> particleSystem in m_particleSystems)
		{
			rD_RelativeDisplacementMotionTriggerBox_DATA.particles[num] = particleSystem.Value.GetParticleData();
			num++;
		}
		return JsonUtility.ToJson(rD_RelativeDisplacementMotionTriggerBox_DATA);
	}

	[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
	public override void RebirthReadData(object rd_data)
	{
		m_rebirthData = JsonUtility.FromJson<RD_RelativeDisplacementMotionTriggerBox_DATA>(rd_data as string);
		m_state = m_rebirthData.m_state;
		m_railwayInverse = m_rebirthData.m_railwayInverse;
		m_modelTransform.SetTransData(m_rebirthData.m_modelTransform);
		m_animator.SetAnimData(m_rebirthData.m_animator, ProcessState.Pause);
		int num = 0;
		foreach (KeyValuePair<string, ParticleSystem> particleSystem in m_particleSystems)
		{
			particleSystem.Value.SetParticleData(m_rebirthData.particles[num], ProcessState.Pause);
			num++;
		}
	}

	[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
	public override void RebirthStartGame(object rd_data)
	{
		if (m_rebirthData != null)
		{
			m_animator.SetAnimData(m_rebirthData.m_animator, ProcessState.UnPause);
			int num = 0;
			foreach (KeyValuePair<string, ParticleSystem> particleSystem in m_particleSystems)
			{
				particleSystem.Value.SetParticleData(m_rebirthData.particles[num], ProcessState.UnPause);
				num++;
			}
		}
		m_rebirthData = null;
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
		m_rebirthData = Bson.ToObject<RD_RelativeDisplacementMotionTriggerBox_DATA>(rd_data);
		m_state = m_rebirthData.m_state;
		m_railwayInverse = m_rebirthData.m_railwayInverse;
		m_modelTransform.SetTransData(m_rebirthData.m_modelTransform);
		m_animator.SetAnimData(m_rebirthData.m_animator, ProcessState.Pause);
		int num = 0;
		foreach (KeyValuePair<string, ParticleSystem> particleSystem in m_particleSystems)
		{
			particleSystem.Value.SetParticleData(m_rebirthData.particles[num], ProcessState.Pause);
			num++;
		}
	}

	public override byte[] RebirthWriteByteData()
	{
		RD_RelativeDisplacementMotionTriggerBox_DATA rD_RelativeDisplacementMotionTriggerBox_DATA = new RD_RelativeDisplacementMotionTriggerBox_DATA();
		rD_RelativeDisplacementMotionTriggerBox_DATA.m_state = m_state;
		rD_RelativeDisplacementMotionTriggerBox_DATA.m_railwayInverse = m_railwayInverse;
		rD_RelativeDisplacementMotionTriggerBox_DATA.m_modelTransform = m_modelTransform.GetTransData();
		rD_RelativeDisplacementMotionTriggerBox_DATA.m_animator = m_animator.GetAnimData();
		rD_RelativeDisplacementMotionTriggerBox_DATA.particles = new RD_ElementParticle_DATA[m_particleSystems.Count];
		int num = 0;
		foreach (KeyValuePair<string, ParticleSystem> particleSystem in m_particleSystems)
		{
			rD_RelativeDisplacementMotionTriggerBox_DATA.particles[num] = particleSystem.Value.GetParticleData();
			num++;
		}
		return Bson.ToBson(rD_RelativeDisplacementMotionTriggerBox_DATA);
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
		if (m_rebirthData != null)
		{
			m_animator.SetAnimData(m_rebirthData.m_animator, ProcessState.UnPause);
			int num = 0;
			foreach (KeyValuePair<string, ParticleSystem> particleSystem in m_particleSystems)
			{
				particleSystem.Value.SetParticleData(m_rebirthData.particles[num], ProcessState.UnPause);
				num++;
			}
		}
		m_rebirthData = null;
	}
}
