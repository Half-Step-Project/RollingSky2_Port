using System;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class OpenFollowTrigger : BaseTriggerBox
	{
		[Serializable]
		public struct TriggerData : IReadWriteBytes
		{
			public float m_followSpeed;

			public void ReadBytes(byte[] bytes)
			{
				int startIndex = 0;
				m_followSpeed = bytes.GetSingle(ref startIndex);
			}

			public byte[] WriteBytes()
			{
				return m_followSpeed.GetBytes();
			}
		}

		public TriggerData m_data;

		public override bool IfRebirthRecord
		{
			get
			{
				return false;
			}
		}

		public override void SetDefaultValue(object[] objs)
		{
			m_data = (TriggerData)objs[0];
		}

		public override void TriggerEnter(BaseRole ball)
		{
			FollowData followData = new FollowData();
			followData.m_followSpeed = m_data.m_followSpeed;
			FollowData getFollowData = CameraController.theCamera.GetFollowData;
			if (getFollowData == null)
			{
				followData.m_cameraPoint = ball.transform.InverseTransformPoint(CameraController.theCamera.gameObject.transform.position);
			}
			else
			{
				followData.m_cameraPoint = getFollowData.m_cameraPoint;
			}
			Mod.Event.FireNow(this, Mod.Reference.Acquire<FollowOpenEventArgs>().Initialize(followData));
		}

		public override void Read(string info)
		{
			m_data = JsonUtility.FromJson<TriggerData>(info);
		}

		public override string Write()
		{
			return JsonUtility.ToJson(m_data);
		}

		public override void ReadBytes(byte[] bytes)
		{
			m_data = StructTranslatorUtility.ToStructure<TriggerData>(bytes);
		}

		public override byte[] WriteBytes()
		{
			return StructTranslatorUtility.ToByteArray(m_data);
		}
	}
}
