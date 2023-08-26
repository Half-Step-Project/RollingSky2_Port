using System;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class ChangeRuntimeAnimatorControllerTrigger : BaseTriggerBox
	{
		[Serializable]
		public struct ElementData : IReadWriteBytes
		{
			public int m_changeIndex;

			public void ReadBytes(byte[] bytes)
			{
				int startIndex = 0;
				m_changeIndex = bytes.GetInt32(ref startIndex);
			}

			public byte[] WriteBytes()
			{
				return m_changeIndex.GetBytes();
			}
		}

		public ElementData m_data;

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
		}

		public override void ResetElement()
		{
			base.ResetElement();
		}

		public override void TriggerEnter(BaseRole ball)
		{
			BaseRole.theBall.ChangeRuntimeAnimatorControllerByIndex(m_data.m_changeIndex);
		}

		public override void SetDefaultValue(object[] objs)
		{
			m_data = (ElementData)objs[0];
		}

		public override void Read(string info)
		{
			m_data = JsonUtility.FromJson<ElementData>(info);
		}

		public override string Write()
		{
			return JsonUtility.ToJson(m_data);
		}

		public override void ReadBytes(byte[] bytes)
		{
			if (bytes != null)
			{
				m_data = StructTranslatorUtility.ToStructure<ElementData>(bytes);
			}
		}

		public override byte[] WriteBytes()
		{
			return StructTranslatorUtility.ToByteArray(m_data);
		}
	}
}
