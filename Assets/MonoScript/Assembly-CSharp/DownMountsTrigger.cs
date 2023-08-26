using System;
using Foundation;
using UnityEngine;

public class DownMountsTrigger : BaseTriggerBox
{
	[Serializable]
	public struct TriggerData : IReadWriteBytes
	{
		public float m_duration;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			m_duration = bytes.GetSingle(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			return m_duration.GetBytes();
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

	public override void TriggerEnter(BaseRole ball)
	{
		PetController petController = GameController.Instance.m_petController;
		if (petController != null && petController.m_petBase != null)
		{
			petController.m_petBase.TriggerDownMounts(m_data);
		}
	}

	public override void SetDefaultValue(object[] objs)
	{
		m_data = (TriggerData)objs[0];
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
