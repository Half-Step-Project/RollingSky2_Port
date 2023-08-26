using System;
using System.IO;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class TriggerFragmentTrigger : BaseTriggerBox, IElementRebirth
	{
		[Serializable]
		public struct TriggerFragmentTriggerData : IReadWriteBytes
		{
			public float m_beginDistance;

			public float m_resetDistance;

			public void ReadBytes(byte[] bytes)
			{
				int startIndex = 0;
				m_beginDistance = bytes.GetSingle(ref startIndex);
				m_resetDistance = bytes.GetSingle(ref startIndex);
			}

			public byte[] WriteBytes()
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					int offset = 0;
					memoryStream.WriteByteArray(m_beginDistance.GetBytes(), ref offset);
					memoryStream.WriteByteArray(m_resetDistance.GetBytes(), ref offset);
					memoryStream.Flush();
					memoryStream.Seek(0L, SeekOrigin.Begin);
					return memoryStream.ToArray();
				}
			}
		}

		public TriggerFragmentTriggerData m_data;

		private Animation m_animation;

		[Range(0f, 1f)]
		public float m_currentDebugAnimationTime;

		private float m_lastDebugAnimationTime;

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
			m_animation = base.transform.Find("model/Home_HuaKai_LuJing").gameObject.GetComponent<Animation>();
			PlayAnim(m_animation, false);
			commonState = CommonState.None;
		}

		public override void UpdateElement()
		{
			float num = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z;
			if (commonState == CommonState.None)
			{
				if (num >= m_data.m_beginDistance)
				{
					OnTriggerPlay();
					commonState = CommonState.Active;
				}
			}
			else if (commonState == CommonState.Active && num >= m_data.m_resetDistance)
			{
				OnTriggerStop();
				commonState = CommonState.End;
			}
		}

		public override void ResetElement()
		{
			base.ResetElement();
			commonState = CommonState.None;
		}

		public override void TriggerEnter(BaseRole ball)
		{
			if (commonState == CommonState.Active)
			{
				Mod.Event.FireNow(this, Mod.Reference.Acquire<GainedDropEventArgs>().Initialize(new DropData(m_uuId, DropType.TRIGGERFRAGMENT, 1)));
				commonState = CommonState.End;
				PlayAnim(m_animation, false);
			}
		}

		public override void OnTriggerPlay()
		{
			base.OnTriggerPlay();
			PlayAnim(m_animation, true);
		}

		public override void OnTriggerStop()
		{
			PlayAnim(m_animation, false);
			base.OnTriggerStop();
		}

		public override string Write()
		{
			return JsonUtility.ToJson(m_data);
		}

		public override void Read(string info)
		{
			m_data = JsonUtility.FromJson<TriggerFragmentTriggerData>(info);
		}

		public override void ReadBytes(byte[] bytes)
		{
			m_data = StructTranslatorUtility.ToStructure<TriggerFragmentTriggerData>(bytes);
		}

		public override byte[] WriteBytes()
		{
			return StructTranslatorUtility.ToByteArray(m_data);
		}

		public override void SetDefaultValue(object[] objs)
		{
			m_data = (TriggerFragmentTriggerData)objs[0];
		}

		public override void OnDrawGizmos()
		{
			base.OnDrawGizmos();
			Vector3 position = base.gameObject.transform.position;
			Vector3 from = base.gameObject.transform.TransformPoint(new Vector3(0f, 0f, m_data.m_beginDistance));
			Gizmos.color = Color.red;
			Gizmos.DrawLine(from, position);
			Gizmos.color = Color.white;
		}

		[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
		public override void RebirthReadDataForDrop(object rd_data)
		{
			base.RebirthReadDataForDrop(rd_data);
			commonState = CommonState.End;
		}

		public override void RebirthReadByteDataForDrop(byte[] rd_data)
		{
			base.RebirthReadByteDataForDrop(rd_data);
			commonState = CommonState.End;
		}
	}
}
