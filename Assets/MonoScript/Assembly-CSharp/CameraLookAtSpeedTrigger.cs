using System;
using Foundation;
using UnityEngine;

public class CameraLookAtSpeedTrigger : BaseTriggerBox
{
	[Serializable]
	public struct ElementData : IReadWriteBytes
	{
		public float m_lookAtSlerpSpeed;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			m_lookAtSlerpSpeed = bytes.GetSingle(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			return m_lookAtSlerpSpeed.GetBytes();
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

	public override void TriggerEnter(BaseRole ball)
	{
		CameraLookAt.Instance.m_lookAtSlerpSpeed = m_data.m_lookAtSlerpSpeed;
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

	public override void Read(string info)
	{
		m_data = JsonUtility.FromJson<ElementData>(info);
	}

	public override string Write()
	{
		return JsonUtility.ToJson(m_data);
	}

	public override void SetDefaultValue(object[] objs)
	{
		m_data = (ElementData)objs[0];
	}
}
