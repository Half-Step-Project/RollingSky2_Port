using System;
using System.IO;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class DandelionEnemy : BaseEnemy
	{
		public enum State
		{
			AutoPlay,
			TriggerPlay,
			End
		}

		[Serializable]
		public struct EnemyData : IReadWriteBytes
		{
			public float TriggerDistance;

			public float ResetDistance;

			public void ReadBytes(byte[] bytes)
			{
				int startIndex = 0;
				TriggerDistance = bytes.GetSingle(ref startIndex);
				ResetDistance = bytes.GetSingle(ref startIndex);
			}

			public byte[] WriteBytes()
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					int offset = 0;
					memoryStream.WriteByteArray(TriggerDistance.GetBytes(), ref offset);
					memoryStream.WriteByteArray(ResetDistance.GetBytes(), ref offset);
					memoryStream.Flush();
					memoryStream.Seek(0L, SeekOrigin.Begin);
					return memoryStream.ToArray();
				}
			}
		}

		private static string ModelPartName = "model";

		private static string DefaultParticlesName = "effect0";

		private static string TriggerParticlesName = "effect1";

		public EnemyData data;

		private Transform modelPart;

		private Animation defaultAnim;

		private ParticleSystem[] defaultParticles;

		private ParticleSystem[] triggerParticles;

		private State currentState;

		private RD_DandelionEnemy_DATA m_rebirthData;

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
			modelPart = base.transform.Find(ModelPartName);
			if ((bool)modelPart)
			{
				defaultAnim = modelPart.GetComponentInChildren<Animation>();
				if ((bool)defaultAnim)
				{
					defaultAnim.wrapMode = WrapMode.Loop;
					PlayAnim(defaultAnim, true);
				}
				modelPart.gameObject.SetActive(true);
			}
			Transform transform = base.transform.Find(DefaultParticlesName);
			if ((bool)transform)
			{
				defaultParticles = transform.GetComponentsInChildren<ParticleSystem>();
				PlayParticle(defaultParticles, true);
			}
			transform = base.transform.Find(TriggerParticlesName);
			if ((bool)transform)
			{
				triggerParticles = transform.GetComponentsInChildren<ParticleSystem>();
				PlayParticle(triggerParticles, false);
			}
			currentState = State.AutoPlay;
		}

		public override void ResetElement()
		{
			base.ResetElement();
			if ((bool)defaultAnim)
			{
				PlayAnim(defaultAnim, false);
			}
			PlayParticle(defaultParticles, false);
			PlayParticle(triggerParticles, false);
			currentState = State.AutoPlay;
		}

		public override void UpdateElement()
		{
			float num = 0f;
			num = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z;
			if (currentState == State.AutoPlay)
			{
				if (num >= data.TriggerDistance)
				{
					PlayAnim(defaultAnim, false);
					modelPart.gameObject.SetActive(false);
					PlayParticle(defaultParticles, false);
					PlayParticle(triggerParticles, true);
					currentState = State.TriggerPlay;
				}
			}
			else if (currentState == State.TriggerPlay)
			{
				if (num >= data.ResetDistance)
				{
					PlayParticle(triggerParticles, false);
					currentState = State.End;
				}
			}
			else
			{
				State currentState2 = currentState;
				int num2 = 2;
			}
		}

		public override void TriggerEnter(BaseRole ball)
		{
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
			m_rebirthData = JsonUtility.FromJson<RD_DandelionEnemy_DATA>(rd_data as string);
			modelPart.SetTransData(m_rebirthData.modelPart);
			currentState = m_rebirthData.currentState;
			defaultAnim.SetAnimData(m_rebirthData.defaultAnim, ProcessState.Pause);
			defaultParticles.SetParticlesData(m_rebirthData.defaultParticles, ProcessState.Pause);
			triggerParticles.SetParticlesData(m_rebirthData.triggerParticles, ProcessState.Pause);
		}

		[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
		public override object RebirthWriteData()
		{
			return JsonUtility.ToJson(new RD_DandelionEnemy_DATA
			{
				modelPart = modelPart.GetTransData(),
				defaultAnim = defaultAnim.GetAnimData(),
				defaultParticles = defaultParticles.GetParticlesData(),
				triggerParticles = triggerParticles.GetParticlesData(),
				currentState = currentState
			});
		}

		[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
		public override void RebirthStartGame(object rd_data)
		{
			defaultAnim.SetAnimData(m_rebirthData.defaultAnim, ProcessState.UnPause);
			defaultParticles.SetParticlesData(m_rebirthData.defaultParticles, ProcessState.UnPause);
			triggerParticles.SetParticlesData(m_rebirthData.triggerParticles, ProcessState.UnPause);
			m_rebirthData = null;
		}

		public override void RebirthReadByteData(byte[] rd_data)
		{
			m_rebirthData = Bson.ToObject<RD_DandelionEnemy_DATA>(rd_data);
			modelPart.SetTransData(m_rebirthData.modelPart);
			currentState = m_rebirthData.currentState;
			defaultAnim.SetAnimData(m_rebirthData.defaultAnim, ProcessState.Pause);
			defaultParticles.SetParticlesData(m_rebirthData.defaultParticles, ProcessState.Pause);
			triggerParticles.SetParticlesData(m_rebirthData.triggerParticles, ProcessState.Pause);
		}

		public override byte[] RebirthWriteByteData()
		{
			return Bson.ToBson(new RD_DandelionEnemy_DATA
			{
				modelPart = modelPart.GetTransData(),
				defaultAnim = defaultAnim.GetAnimData(),
				defaultParticles = defaultParticles.GetParticlesData(),
				triggerParticles = triggerParticles.GetParticlesData(),
				currentState = currentState
			});
		}

		public override void RebirthStartByteGame(byte[] rd_data)
		{
			defaultAnim.SetAnimData(m_rebirthData.defaultAnim, ProcessState.UnPause);
			defaultParticles.SetParticlesData(m_rebirthData.defaultParticles, ProcessState.UnPause);
			triggerParticles.SetParticlesData(m_rebirthData.triggerParticles, ProcessState.UnPause);
			m_rebirthData = null;
		}
	}
}
