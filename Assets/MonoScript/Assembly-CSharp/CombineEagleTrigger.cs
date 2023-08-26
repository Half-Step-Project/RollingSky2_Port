using System;
using Foundation;
using UnityEngine;

public class CombineEagleTrigger : BaseTriggerBox
{
	[Serializable]
	public struct CombineEagleTriggerData : IReadWriteBytes
	{
		public float m_depart;

		public void ReadBytes(byte[] bytes)
		{
			m_depart = bytes.GetSingle();
		}

		public byte[] WriteBytes()
		{
			return m_depart.GetBytes();
		}
	}

	public CombineEagleTriggerData m_data;

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
		if (petController != null)
		{
			bool flag = petController.m_petBase != null;
		}
	}

	public override void SetDefaultValue(object[] objs)
	{
		m_data.m_depart = (float)objs[0];
	}

	public override void Read(string info)
	{
		m_data = JsonUtility.FromJson<CombineEagleTriggerData>(info);
	}

	public override string Write()
	{
		return JsonUtility.ToJson(m_data);
	}

	public override void ReadBytes(byte[] bytes)
	{
		m_data = StructTranslatorUtility.ToStructure<CombineEagleTriggerData>(bytes);
	}

	public override byte[] WriteBytes()
	{
		return StructTranslatorUtility.ToByteArray(m_data);
	}
}
