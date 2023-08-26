using System;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class FollowCameraEffectTrigger : BaseTriggerBox
	{
		[Serializable]
		public struct TriggerData : IReadWriteBytes
		{
			public float ResetDistance;

			public void ReadBytes(byte[] bytes)
			{
				int startIndex = 0;
				ResetDistance = bytes.GetSingle(ref startIndex);
			}

			public byte[] WriteBytes()
			{
				return ResetDistance.GetBytes();
			}
		}

		public TriggerData data;

		private Transform cameraTrans;

		private Vector3 beginPos;

		private ParticleSystem[] particles;

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
			cameraTrans = CameraController.theCamera.m_Camera;
			particles = base.transform.GetComponentsInChildren<ParticleSystem>();
			commonState = CommonState.None;
		}

		public override void ResetElement()
		{
			base.ResetElement();
			commonState = CommonState.None;
			cameraTrans = null;
			PlayParticle(particles, false);
		}

		public override void TriggerEnter(BaseRole ball)
		{
			if (commonState == CommonState.None)
			{
				commonState = CommonState.Active;
				base.transform.position = cameraTrans.position;
				beginPos = base.transform.position;
				PlayParticle(particles, true);
			}
		}

		public override void UpdateElement()
		{
			if (commonState == CommonState.Active)
			{
				base.transform.position = cameraTrans.position;
				if (base.transform.position.z - beginPos.z >= data.ResetDistance)
				{
					commonState = CommonState.InActive;
					PlayParticle(particles, false);
				}
			}
		}

		public override void Read(string info)
		{
			data = JsonUtility.FromJson<TriggerData>(info);
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
	}
}
