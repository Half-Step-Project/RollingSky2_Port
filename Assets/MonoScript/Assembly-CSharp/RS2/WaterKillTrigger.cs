using System;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class WaterKillTrigger : BaseTriggerBox
	{
		[Serializable]
		public struct TriggerData : IReadWriteBytes
		{
			public float ValidDistance;

			public void ReadBytes(byte[] bytes)
			{
				int startIndex = 0;
				ValidDistance = bytes.GetSingle(ref startIndex);
			}

			public byte[] WriteBytes()
			{
				return ValidDistance.GetBytes();
			}
		}

		private Transform effect;

		private ParticleSystem[] particles;

		public TriggerData data;

		public override bool IfRebirthRecord
		{
			get
			{
				return false;
			}
		}

		public override void Initialize()
		{
			base.Initialize();
			effect = base.transform.Find("effect");
			if ((bool)effect)
			{
				particles = effect.GetComponentsInChildren<ParticleSystem>();
				StopParticle();
			}
			Mod.Event.Subscribe(EventArgs<GameFailEventArgs>.EventId, OnWaterKillDropDie);
		}

		public override void ResetElement()
		{
			base.transform.position = StartPos;
			StopParticle();
			Mod.Event.Unsubscribe(EventArgs<GameFailEventArgs>.EventId, OnWaterKillDropDie);
		}

		private void OnWaterKillDropDie(object sender, Foundation.EventArgs e)
		{
			GameFailEventArgs gameFailEventArgs = e as GameFailEventArgs;
			if (gameFailEventArgs != null && gameFailEventArgs.FailType == GameFailEventArgs.GameFailType.Special)
			{
				Vector3 diePosition = gameFailEventArgs.DiePosition;
				float num = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z;
				if (num > 0f && num <= data.ValidDistance)
				{
					base.transform.position = diePosition;
					PlayParticle();
					BaseRole.theBall.SetSpecialDieFlag(true, false);
				}
			}
		}

		private void OnDestroy()
		{
			Mod.Event.Unsubscribe(EventArgs<GameFailEventArgs>.EventId, OnWaterKillDropDie);
		}

		public override void Read(string info)
		{
			if (!string.IsNullOrEmpty(info))
			{
				data = JsonUtility.FromJson<TriggerData>(info);
			}
		}

		public override string Write()
		{
			return JsonUtility.ToJson(data);
		}

		public override void ReadBytes(byte[] bytes)
		{
			data = StructTranslatorUtility.ToStructure<TriggerData>(bytes);
		}

		public override byte[] WriteBytes()
		{
			return StructTranslatorUtility.ToByteArray(data);
		}

		public override void SetDefaultValue(object[] objs)
		{
			if (objs != null)
			{
				data.ValidDistance = (float)objs[0];
			}
		}

		public override void OnDrawGizmos()
		{
			base.OnDrawGizmos();
			Color color = Gizmos.color;
			Gizmos.color = Color.white;
			Gizmos.DrawSphere(base.transform.position, 0.5f);
			Gizmos.color = color;
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
	}
}
