using System;
using System.IO;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class ViolinDanceTrigger : BaseTriggerBox
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
					memoryStream.Flush();
					memoryStream.Seek(0L, SeekOrigin.Begin);
					return memoryStream.ToArray();
				}
			}
		}

		private class RebirthData
		{
			public bool m_animationPlaying;

			public string m_animationName = string.Empty;

			public float m_animationTime;
		}

		[Range(0f, 1f)]
		public float DebugPercent;

		private float debugPercent;

		public EnemyData data;

		private Animation anim;

		private GameElementSoundPlayer soundEventPlayer;

		private ParticleSystem[] additionParticles;

		private MeshRenderer[] meshRenders;

		private bool m_currentMeshRenderEnable = true;

		private RD_ViolinDanceTrigger_DATA m_rebirthData;

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
			if (!anim)
			{
				return;
			}
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
				float speed = Railway.theRailway.SpeedForward / data.BaseBallSpeed;
				anim["anim01"].speed = speed;
				if (data.IfAutoPlay)
				{
					anim.Play();
				}
			}
			additionParticles = anim.transform.GetComponentsInChildren<ParticleSystem>();
			meshRenders = anim.GetComponentsInChildren<MeshRenderer>();
			m_currentMeshRenderEnable = !data.IfHideBegin;
			ShowMeshRenders(meshRenders, !data.IfHideBegin);
			PlayParticle(additionParticles, false);
		}

		public override void ResetElement()
		{
			base.ResetElement();
			OnTriggerStop();
			m_currentMeshRenderEnable = !data.IfHideBegin;
			ShowMeshRenders(meshRenders, !data.IfHideBegin);
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
				anim.Play();
			}
			m_currentMeshRenderEnable = true;
			ShowMeshRenders(meshRenders, true);
			PlayParticle(additionParticles, true);
		}

		public override void OnTriggerStop()
		{
			if ((bool)anim)
			{
				anim.Play();
				anim["anim01"].normalizedTime = 0f;
				anim.Sample();
				anim.Stop();
			}
			PlayParticle(additionParticles, false);
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
			data = StructTranslatorUtility.ToStructure<EnemyData>(bytes);
			base.transform.Find("triggerPoint").position = data.BeginPos;
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
			base.OnDrawGizmos();
			Transform transform = base.transform.Find("triggerPoint");
			if ((bool)transform)
			{
				Color color = Gizmos.color;
				Gizmos.color = Color.green;
				Gizmos.DrawCube(transform.position, new Vector3(1f, 0.1f, 0.1f));
				Gizmos.color = color;
			}
			if (debugPercent != DebugPercent)
			{
				if (anim == null)
				{
					anim = GetComponentInChildren<Animation>();
				}
				SetAnimPercent(anim, DebugPercent);
				debugPercent = DebugPercent;
			}
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
			rebirthData.m_animationPlaying = anim.isPlaying;
			if (rebirthData.m_animationPlaying)
			{
				foreach (AnimationState item in anim)
				{
					if (anim.IsPlaying(item.name))
					{
						empty = item.name;
						break;
					}
				}
				if (!string.IsNullOrEmpty(empty))
				{
					rebirthData.m_animationName = empty;
					rebirthData.m_animationTime = anim[rebirthData.m_animationName].time;
				}
			}
			return rebirthData;
		}

		public void ResetBySavePointData(object obj)
		{
			RebirthData rebirthData = obj as RebirthData;
			if (rebirthData != null && rebirthData.m_animationPlaying)
			{
				AnimationState animationState = anim[rebirthData.m_animationName];
				animationState.time = rebirthData.m_animationTime;
				anim.Play(animationState.name);
				anim.Sample();
				anim.Stop();
				commonState = CommonState.Active;
			}
		}

		public void StartRunningForRebirthData(object obj)
		{
			RebirthData rebirthData = obj as RebirthData;
			if (rebirthData != null && rebirthData.m_animationPlaying)
			{
				AnimationState animationState = anim[rebirthData.m_animationName];
				animationState.time = rebirthData.m_animationTime;
				anim.Play(animationState.name);
				commonState = CommonState.Active;
			}
		}

		[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
		public override object RebirthWriteData()
		{
			return JsonUtility.ToJson(new RD_ViolinDanceTrigger_DATA
			{
				anim = anim.GetAnimData(),
				additionParticles = additionParticles.GetParticlesData(),
				commonState = commonState,
				meshRendersEnable = (m_currentMeshRenderEnable ? 1 : 0)
			});
		}

		[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
		public override void RebirthReadData(object rd_data)
		{
			m_rebirthData = JsonUtility.FromJson<RD_ViolinDanceTrigger_DATA>(rd_data as string);
			m_currentMeshRenderEnable = m_rebirthData.meshRendersEnable == 1;
			anim.SetAnimData(m_rebirthData.anim, ProcessState.Pause);
			additionParticles.SetParticlesData(m_rebirthData.additionParticles, ProcessState.Pause);
			commonState = m_rebirthData.commonState;
			ShowMeshRenders(meshRenders, m_currentMeshRenderEnable);
		}

		[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
		public override void RebirthStartGame(object rd_data)
		{
			if (m_rebirthData != null)
			{
				anim.SetAnimData(m_rebirthData.anim, ProcessState.UnPause);
				additionParticles.SetParticlesData(m_rebirthData.additionParticles, ProcessState.UnPause);
			}
			m_rebirthData = null;
		}

		public override void RebirthReadByteData(byte[] rd_data)
		{
			m_rebirthData = Bson.ToObject<RD_ViolinDanceTrigger_DATA>(rd_data);
			m_currentMeshRenderEnable = m_rebirthData.meshRendersEnable == 1;
			anim.SetAnimData(m_rebirthData.anim, ProcessState.Pause);
			additionParticles.SetParticlesData(m_rebirthData.additionParticles, ProcessState.Pause);
			commonState = m_rebirthData.commonState;
			ShowMeshRenders(meshRenders, m_currentMeshRenderEnable);
		}

		public override byte[] RebirthWriteByteData()
		{
			return Bson.ToBson(new RD_ViolinDanceTrigger_DATA
			{
				anim = anim.GetAnimData(),
				additionParticles = additionParticles.GetParticlesData(),
				commonState = commonState,
				meshRendersEnable = (m_currentMeshRenderEnable ? 1 : 0)
			});
		}

		public override void RebirthStartByteGame(byte[] rd_data)
		{
			if (m_rebirthData != null)
			{
				anim.SetAnimData(m_rebirthData.anim, ProcessState.UnPause);
				additionParticles.SetParticlesData(m_rebirthData.additionParticles, ProcessState.UnPause);
			}
			m_rebirthData = null;
		}
	}
}
