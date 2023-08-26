using System;
using System.IO;
using Foundation;
using RS2;
using UnityEngine;

public class AnimEnemyPro : BaseEnemy, IRebirth
{
	[Serializable]
	public struct EnemyData : IReadWriteBytes
	{
		public float BeginDistance;

		public float ResetDistance;

		public float BaseBallSpeed;

		public bool IfAutoPlay;

		public bool IfLoop;

		public float AudioPlayTime;

		public Vector3 BeginPos;

		public bool IfHideBegin;

		public bool IfStartPlay;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			BeginDistance = bytes.GetSingle(ref startIndex);
			ResetDistance = bytes.GetSingle(ref startIndex);
			BaseBallSpeed = bytes.GetSingle(ref startIndex);
			IfAutoPlay = bytes.GetBoolean(ref startIndex);
			IfLoop = bytes.GetBoolean(ref startIndex);
			AudioPlayTime = bytes.GetSingle(ref startIndex);
			BeginPos = bytes.GetVector3(ref startIndex);
			IfHideBegin = bytes.GetBoolean(ref startIndex);
			IfStartPlay = bytes.GetBoolean(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(BeginDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(ResetDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(BaseBallSpeed.GetBytes(), ref offset);
				memoryStream.WriteByteArray(IfAutoPlay.GetBytes(), ref offset);
				memoryStream.WriteByteArray(IfLoop.GetBytes(), ref offset);
				memoryStream.WriteByteArray(AudioPlayTime.GetBytes(), ref offset);
				memoryStream.WriteByteArray(BeginPos.GetBytes(), ref offset);
				memoryStream.WriteByteArray(IfHideBegin.GetBytes(), ref offset);
				memoryStream.WriteByteArray(IfStartPlay.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	private class RebirthData
	{
		public string m_animationName = string.Empty;

		public float m_animationTime;

		public CommonState m_commonState;

		public bool m_isShowMeshRenderer = true;
	}

	public bool DebugShowRole;

	public float DebugRoleSpeed;

	[Range(0f, 1f)]
	public float DebugPercent;

	private float debugPercent;

	public EnemyData data;

	private Animation anim;

	private GameElementSoundPlayer soundEventPlayer;

	private ParticleSystem[] additionParticles;

	private MeshRenderer[] meshRenders;

	private Transform[] meshRenderersTransforms;

	private string m_currentAnimationName = string.Empty;

	private bool m_isShowMeshRenderer;

	private RD_AnimEnemyPro_DATA m_rebirthData;

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
		anim = GetComponentInChildren<Animation>();
		if ((bool)anim)
		{
			if (audioSource != null)
			{
				soundEventPlayer = anim.gameObject.GetComponent<GameElementSoundPlayer>();
				if (soundEventPlayer == null)
				{
					soundEventPlayer = anim.gameObject.AddComponent<GameElementSoundPlayer>();
					soundEventPlayer.gameElement = this;
					soundEventPlayer.RegistAudioEvent(anim.GetClip("anim01"), data.AudioPlayTime);
				}
			}
			if (data.BaseBallSpeed > 0f)
			{
				try
				{
					float speed = Railway.theRailway.SpeedForward / data.BaseBallSpeed;
					anim["anim01"].speed = speed;
					if (data.IfAutoPlay)
					{
						anim["anim01"].normalizedTime = 0f;
						m_currentAnimationName = "anim01";
						anim.Play();
					}
				}
				catch (Exception)
				{
					Debug.LogError(base.name);
				}
			}
			additionParticles = anim.transform.GetComponentsInChildren<ParticleSystem>();
			meshRenders = anim.GetComponentsInChildren<MeshRenderer>();
			ShowMeshRenders(meshRenders, !data.IfHideBegin);
			if (meshRenders != null)
			{
				meshRenderersTransforms = new Transform[meshRenders.Length];
				for (int i = 0; i < meshRenders.Length; i++)
				{
					meshRenderersTransforms[i] = meshRenders[i].gameObject.transform;
				}
			}
			m_isShowMeshRenderer = !data.IfHideBegin;
			PlayParticle(additionParticles, false);
		}
		if (data.IfStartPlay)
		{
			Mod.Event.Subscribe(EventArgs<GameStartEventArgs>.EventId, OnStartGame);
		}
	}

	public override void ResetElement()
	{
		base.ResetElement();
		OnTriggerStop();
		ShowMeshRenders(meshRenders, !data.IfHideBegin);
		m_isShowMeshRenderer = !data.IfHideBegin;
		m_currentAnimationName = string.Empty;
		if (data.IfStartPlay)
		{
			Mod.Event.Unsubscribe(EventArgs<GameStartEventArgs>.EventId, OnStartGame);
		}
	}

	public override void UpdateElement()
	{
		float num = 0f;
		num = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z;
		if (commonState == CommonState.None)
		{
			if (num >= data.BeginDistance)
			{
				OnTriggerPlay();
				commonState = CommonState.Active;
			}
		}
		else if (commonState == CommonState.Active && num >= data.ResetDistance)
		{
			OnTriggerStop();
			commonState = CommonState.End;
		}
		if (!m_isShowMeshRenderer)
		{
			return;
		}
		int num2 = ((meshRenders != null) ? meshRenders.Length : 0);
		for (int i = 0; i < num2; i++)
		{
			MeshRenderer meshRenderer = meshRenders[i];
			Vector3 lossyScale = meshRenderersTransforms[i].lossyScale;
			bool flag = Mathf.Abs(lossyScale.x) >= 0.01f || Mathf.Abs(lossyScale.y) >= 0.01f || Mathf.Abs(lossyScale.z) >= 0.01f;
			if (meshRenderer.enabled != flag)
			{
				meshRenderer.enabled = flag;
			}
		}
	}

	public override void OnTriggerPlay()
	{
		if ((bool)anim)
		{
			if (data.IfLoop)
			{
				anim.wrapMode = WrapMode.Loop;
			}
			else
			{
				anim.wrapMode = WrapMode.ClampForever;
			}
			anim["anim01"].normalizedTime = 0f;
			m_currentAnimationName = "anim01";
			anim.Play();
		}
		ShowMeshRenders(meshRenders, true);
		m_isShowMeshRenderer = true;
		int @int = Mod.Setting.GetInt("Setting.QualityLevel");
		if (!DeviceManager.Instance.IsLowEndQualityLevel(@int))
		{
			PlayParticle(additionParticles, true);
			PlayParticle();
		}
	}

	public override void OnTriggerStop()
	{
		if ((bool)anim)
		{
			anim.Play();
			anim["anim01"].normalizedTime = 0f;
			anim.Sample();
			anim.Stop();
			m_currentAnimationName = string.Empty;
		}
		PlayParticle(additionParticles, false);
		StopParticle();
	}

	private void OnStartGame(object sender, Foundation.EventArgs e)
	{
		GameStartEventArgs gameStartEventArgs = e as GameStartEventArgs;
		if (gameStartEventArgs != null && gameStartEventArgs.StartType == GameStartEventArgs.GameStartType.Normal && commonState == CommonState.None)
		{
			OnTriggerPlay();
			commonState = CommonState.Active;
		}
	}

	public override void Read(string info)
	{
		data = JsonUtility.FromJson<EnemyData>(info);
		base.transform.Find("triggerPoint").position = data.BeginPos;
	}

	public override string Write()
	{
		Transform transform = base.transform.Find("triggerPoint");
		data.BeginPos = transform.position;
		data.BeginDistance = base.transform.parent.InverseTransformPoint(transform.position).z - base.transform.parent.InverseTransformPoint(base.transform.position).z;
		return JsonUtility.ToJson(data);
	}

	public override void ReadBytes(byte[] bytes)
	{
		if (bytes != null)
		{
			data = StructTranslatorUtility.ToStructure<EnemyData>(bytes);
			base.transform.Find("triggerPoint").position = data.BeginPos;
		}
	}

	public override byte[] WriteBytes()
	{
		Transform transform = base.transform.Find("triggerPoint");
		data.BeginPos = transform.position;
		data.BeginDistance = base.transform.parent.InverseTransformPoint(transform.position).z - base.transform.parent.InverseTransformPoint(base.transform.position).z;
		return StructTranslatorUtility.ToByteArray(data);
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
		OnTriggerStop();
	}

	public override void OnDrawGizmos()
	{
	}

	private void OnTriggerEnter(Collider collider)
	{
		TriggerEnter(BaseRole.theBall);
	}

	public bool IsRecordRebirth()
	{
		return true;
	}

	public object GetRebirthData(Vector3 position, Quaternion rotation, Vector3 scale)
	{
		RebirthData rebirthData = new RebirthData();
		string empty = string.Empty;
		empty = m_currentAnimationName;
		if (!string.IsNullOrEmpty(empty))
		{
			rebirthData.m_animationName = empty;
			rebirthData.m_animationTime = anim[rebirthData.m_animationName].time;
		}
		rebirthData.m_commonState = commonState;
		rebirthData.m_isShowMeshRenderer = m_isShowMeshRenderer;
		return rebirthData;
	}

	public void ResetBySavePointData(object obj)
	{
		RebirthData rebirthData = obj as RebirthData;
		if (rebirthData != null)
		{
			m_currentAnimationName = rebirthData.m_animationName;
			if (!string.IsNullOrEmpty(rebirthData.m_animationName))
			{
				AnimationState animationState = anim[rebirthData.m_animationName];
				animationState.time = rebirthData.m_animationTime;
				anim.Play(animationState.name);
				anim.Sample();
				anim.Stop();
			}
			commonState = rebirthData.m_commonState;
			m_isShowMeshRenderer = rebirthData.m_isShowMeshRenderer;
			ShowMeshRenders(meshRenders, m_isShowMeshRenderer);
		}
	}

	public void StartRunningForRebirthData(object obj)
	{
		RebirthData rebirthData = obj as RebirthData;
		if (rebirthData != null)
		{
			m_currentAnimationName = rebirthData.m_animationName;
			if (!string.IsNullOrEmpty(rebirthData.m_animationName))
			{
				AnimationState animationState = anim[rebirthData.m_animationName];
				animationState.time = rebirthData.m_animationTime;
				anim.Play(animationState.name);
			}
			commonState = rebirthData.m_commonState;
			m_isShowMeshRenderer = rebirthData.m_isShowMeshRenderer;
		}
	}

	[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
	public override void RebirthReadData(object rd_data)
	{
		m_rebirthData = JsonUtility.FromJson<RD_AnimEnemyPro_DATA>(rd_data as string);
		commonState = m_rebirthData.commonState;
		m_currentAnimationName = m_rebirthData.m_currentAnimationName;
		m_isShowMeshRenderer = m_rebirthData.m_isShowMeshRenderer;
		anim.SetAnimData(m_rebirthData.animData, ProcessState.Pause);
		if (additionParticles != null && additionParticles.Length != 0)
		{
			if (additionParticles[0].main.startDelayMultiplier > 0f)
			{
				additionParticles.SetParticlesData(m_rebirthData.additionParticlesData, ProcessState.Stop);
			}
			else
			{
				additionParticles.SetParticlesData(m_rebirthData.additionParticlesData, ProcessState.Pause);
			}
		}
		if (particles != null && particles.Length != 0)
		{
			if (particles[0].main.startDelayMultiplier > 0f)
			{
				particles.SetParticlesData(m_rebirthData.particles, ProcessState.Stop);
			}
			else
			{
				particles.SetParticlesData(m_rebirthData.particles, ProcessState.Pause);
			}
		}
		ShowMeshRenders(meshRenders, m_isShowMeshRenderer);
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override object RebirthWriteData()
	{
		return JsonUtility.ToJson(new RD_AnimEnemyPro_DATA
		{
			commonState = commonState,
			m_currentAnimationName = m_currentAnimationName,
			m_isShowMeshRenderer = m_isShowMeshRenderer,
			animData = anim.GetAnimData(m_currentAnimationName),
			additionParticlesData = additionParticles.GetParticlesData(),
			particles = particles.GetParticlesData()
		});
	}

	[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
	public override void RebirthStartGame(object rd_data)
	{
		anim.SetAnimData(m_rebirthData.animData, ProcessState.UnPause);
		if (additionParticles != null && additionParticles.Length != 0)
		{
			if (additionParticles[0].main.startDelayMultiplier > 0f)
			{
				additionParticles.SetParticlesData(m_rebirthData.additionParticlesData, ProcessState.Stop);
			}
			else
			{
				additionParticles.SetParticlesData(m_rebirthData.additionParticlesData, ProcessState.UnPause);
			}
		}
		if (particles != null && particles.Length != 0)
		{
			if (particles[0].main.startDelayMultiplier > 0f)
			{
				particles.SetParticlesData(m_rebirthData.particles, ProcessState.Stop);
			}
			else
			{
				particles.SetParticlesData(m_rebirthData.particles, ProcessState.UnPause);
			}
		}
		m_rebirthData = null;
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
		m_rebirthData = Bson.ToObject<RD_AnimEnemyPro_DATA>(rd_data);
		commonState = m_rebirthData.commonState;
		m_currentAnimationName = m_rebirthData.m_currentAnimationName;
		m_isShowMeshRenderer = m_rebirthData.m_isShowMeshRenderer;
		anim.SetAnimData(m_rebirthData.animData, ProcessState.Pause);
		if (additionParticles != null && additionParticles.Length != 0)
		{
			if (additionParticles[0].main.startDelayMultiplier > 0f)
			{
				additionParticles.SetParticlesData(m_rebirthData.additionParticlesData, ProcessState.Stop);
			}
			else
			{
				additionParticles.SetParticlesData(m_rebirthData.additionParticlesData, ProcessState.Pause);
			}
		}
		if (particles != null && particles.Length != 0)
		{
			if (particles[0].main.startDelayMultiplier > 0f)
			{
				particles.SetParticlesData(m_rebirthData.particles, ProcessState.Stop);
			}
			else
			{
				particles.SetParticlesData(m_rebirthData.particles, ProcessState.Pause);
			}
		}
		ShowMeshRenders(meshRenders, m_isShowMeshRenderer);
	}

	public override byte[] RebirthWriteByteData()
	{
		return Bson.ToBson(new RD_AnimEnemyPro_DATA
		{
			commonState = commonState,
			m_currentAnimationName = m_currentAnimationName,
			m_isShowMeshRenderer = m_isShowMeshRenderer,
			animData = anim.GetAnimData(m_currentAnimationName),
			additionParticlesData = additionParticles.GetParticlesData(),
			particles = particles.GetParticlesData()
		});
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
		anim.SetAnimData(m_rebirthData.animData, ProcessState.UnPause);
		if (additionParticles != null && additionParticles.Length != 0)
		{
			if (additionParticles[0].main.startDelayMultiplier > 0f)
			{
				additionParticles.SetParticlesData(m_rebirthData.additionParticlesData, ProcessState.Stop);
			}
			else
			{
				additionParticles.SetParticlesData(m_rebirthData.additionParticlesData, ProcessState.UnPause);
			}
		}
		if (particles != null && particles.Length != 0)
		{
			if (particles[0].main.startDelayMultiplier > 0f)
			{
				particles.SetParticlesData(m_rebirthData.particles, ProcessState.Stop);
			}
			else
			{
				particles.SetParticlesData(m_rebirthData.particles, ProcessState.UnPause);
			}
		}
		m_rebirthData = null;
	}

	protected void Update()
	{
		if (GameController.Instance.IsGameStateChanged())
		{
			ShowMeshRenders(meshRenders, m_isShowMeshRenderer);
		}
	}
}
