using System;
using System.IO;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class AnimEffectShowEnemy : BaseEnemy
	{
		[Serializable]
		public struct EnemyData : IReadWriteBytes
		{
			public float ShowDistance;

			public float BeginDistance;

			public float ResetDistance;

			public float BaseBallSpeed;

			public bool IfAutoPlay;

			public bool IfLoop;

			public float AudioPlayTime;

			public Vector3 ShowPos;

			public Vector3 BeginPos;

			public void ReadBytes(byte[] bytes)
			{
				int startIndex = 0;
				ShowDistance = bytes.GetSingle(ref startIndex);
				BeginDistance = bytes.GetSingle(ref startIndex);
				ResetDistance = bytes.GetSingle(ref startIndex);
				BaseBallSpeed = bytes.GetSingle(ref startIndex);
				IfAutoPlay = bytes.GetBoolean(ref startIndex);
				IfLoop = bytes.GetBoolean(ref startIndex);
				AudioPlayTime = bytes.GetSingle(ref startIndex);
				ShowPos = bytes.GetVector3(ref startIndex);
				BeginPos = bytes.GetVector3(ref startIndex);
			}

			public byte[] WriteBytes()
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					int offset = 0;
					memoryStream.WriteByteArray(ShowDistance.GetBytes(), ref offset);
					memoryStream.WriteByteArray(BeginDistance.GetBytes(), ref offset);
					memoryStream.WriteByteArray(ResetDistance.GetBytes(), ref offset);
					memoryStream.WriteByteArray(BaseBallSpeed.GetBytes(), ref offset);
					memoryStream.WriteByteArray(IfAutoPlay.GetBytes(), ref offset);
					memoryStream.WriteByteArray(IfLoop.GetBytes(), ref offset);
					memoryStream.WriteByteArray(AudioPlayTime.GetBytes(), ref offset);
					memoryStream.WriteByteArray(ShowPos.GetBytes(), ref offset);
					memoryStream.WriteByteArray(BeginPos.GetBytes(), ref offset);
					memoryStream.Flush();
					memoryStream.Seek(0L, SeekOrigin.Begin);
					return memoryStream.ToArray();
				}
			}
		}

		[Range(0f, 1f)]
		public float DebugPercent;

		private float debugPercent;

		public EnemyData data;

		private Animation anim;

		private GameElementSoundPlayer soundEventPlayer;

		private ParticleSystem[] additionParticles;

		private Transform modelPoint;

		private RD_AnimEffectShowEnemy_DATA m_rebirthData;

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
			modelPoint = base.transform.Find("model");
			anim = modelPoint.GetComponent<Animation>();
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
					float speed = Railway.theRailway.SpeedForward / data.BaseBallSpeed;
					anim["anim01"].speed = speed;
					if (data.IfAutoPlay)
					{
						anim.Play();
					}
				}
				additionParticles = anim.transform.GetComponentsInChildren<ParticleSystem>();
				PlayParticle(additionParticles, false);
			}
			HideElement();
		}

		public override void ResetElement()
		{
			base.ResetElement();
			OnTriggerStop();
			HideElement();
		}

		public override void UpdateElement()
		{
			float num = 0f;
			num = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z;
			if (commonState == CommonState.None)
			{
				if (num >= data.ShowDistance)
				{
					ShowElement();
					commonState = CommonState.Active;
				}
			}
			else if (commonState == CommonState.Active)
			{
				if (num >= data.BeginDistance)
				{
					OnTriggerPlay();
					commonState = CommonState.InActive;
				}
			}
			else if (commonState == CommonState.InActive && num >= data.ResetDistance)
			{
				OnTriggerStop();
				commonState = CommonState.End;
			}
		}

		private void ShowElement()
		{
			modelPoint.gameObject.SetActive(true);
		}

		private void HideElement()
		{
			modelPoint.gameObject.SetActive(false);
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
				anim.Play();
			}
			PlayParticle(additionParticles, true);
			PlayParticle();
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
			StopParticle();
		}

		public override void Read(string info)
		{
			data = JsonUtility.FromJson<EnemyData>(info);
			base.transform.Find("triggerPoint").position = data.BeginPos;
			base.transform.Find("showPoint").position = data.ShowPos;
		}

		public override string Write()
		{
			Transform transform = base.transform.Find("triggerPoint");
			data.BeginPos = transform.position;
			data.BeginDistance = base.transform.parent.InverseTransformPoint(transform.position).z - base.transform.parent.InverseTransformPoint(base.transform.position).z;
			Transform transform2 = base.transform.Find("showPoint");
			data.ShowPos = transform2.position;
			data.ShowDistance = base.transform.parent.InverseTransformPoint(transform2.position).z - base.transform.parent.InverseTransformPoint(base.transform.position).z;
			return JsonUtility.ToJson(data);
		}

		public override void ReadBytes(byte[] bytes)
		{
			data = StructTranslatorUtility.ToStructure<EnemyData>(bytes);
			base.transform.Find("triggerPoint").position = data.BeginPos;
			base.transform.Find("showPoint").position = data.ShowPos;
		}

		public override byte[] WriteBytes()
		{
			Transform transform = base.transform.Find("triggerPoint");
			data.BeginPos = transform.position;
			data.BeginDistance = base.transform.parent.InverseTransformPoint(transform.position).z - base.transform.parent.InverseTransformPoint(base.transform.position).z;
			Transform transform2 = base.transform.Find("showPoint");
			data.ShowPos = transform2.position;
			data.ShowDistance = base.transform.parent.InverseTransformPoint(transform2.position).z - base.transform.parent.InverseTransformPoint(base.transform.position).z;
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
			Transform transform2 = base.transform.Find("showPoint");
			if ((bool)transform2)
			{
				Color color2 = Gizmos.color;
				Gizmos.color = Color.red;
				Gizmos.DrawCube(transform2.position, new Vector3(1f, 0.1f, 0.1f));
				Gizmos.color = color2;
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

		[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
		public override void RebirthReadData(object rd_data)
		{
			m_rebirthData = JsonUtility.FromJson<RD_AnimEffectShowEnemy_DATA>(rd_data as string);
			commonState = m_rebirthData.commonState;
			modelPoint.SetTransData(m_rebirthData.modelPoint);
			anim.SetAnimData(m_rebirthData.anim, ProcessState.Pause);
			additionParticles.SetParticlesData(m_rebirthData.additionParticles, ProcessState.Pause);
		}

		[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
		public override object RebirthWriteData()
		{
			return JsonUtility.ToJson(new RD_AnimEffectShowEnemy_DATA
			{
				commonState = commonState,
				anim = anim.GetAnimData(),
				additionParticles = additionParticles.GetParticlesData(),
				modelPoint = modelPoint.GetTransData()
			});
		}

		[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
		public override void RebirthStartGame(object rd_data)
		{
			anim.SetAnimData(m_rebirthData.anim, ProcessState.UnPause);
			additionParticles.SetParticlesData(m_rebirthData.additionParticles, ProcessState.UnPause);
			m_rebirthData = null;
		}

		public override void RebirthReadByteData(byte[] rd_data)
		{
			m_rebirthData = Bson.ToObject<RD_AnimEffectShowEnemy_DATA>(rd_data);
			commonState = m_rebirthData.commonState;
			modelPoint.SetTransData(m_rebirthData.modelPoint);
			anim.SetAnimData(m_rebirthData.anim, ProcessState.Pause);
			additionParticles.SetParticlesData(m_rebirthData.additionParticles, ProcessState.Pause);
		}

		public override byte[] RebirthWriteByteData()
		{
			return Bson.ToBson(new RD_AnimEffectShowEnemy_DATA
			{
				commonState = commonState,
				anim = anim.GetAnimData(),
				additionParticles = additionParticles.GetParticlesData(),
				modelPoint = modelPoint.GetTransData()
			});
		}

		public override void RebirthStartByteGame(byte[] rd_data)
		{
			anim.SetAnimData(m_rebirthData.anim, ProcessState.UnPause);
			additionParticles.SetParticlesData(m_rebirthData.additionParticles, ProcessState.UnPause);
			m_rebirthData = null;
		}
	}
}
