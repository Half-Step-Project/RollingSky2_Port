using System;
using Foundation;
using UnityEngine;

public class RoleAnimatiorTrigger : BaseTriggerBox
{
	[Serializable]
	public struct RoleAnimatiorTriggerData : IReadWriteBytes
	{
		public BaseRole.AnimType m_animType;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			m_animType = (BaseRole.AnimType)bytes.GetInt32(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			return ((int)m_animType).GetBytes();
		}
	}

	public RoleAnimatiorTriggerData m_data;

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
		commonState = CommonState.None;
	}

	public override void ResetElement()
	{
		base.ResetElement();
		commonState = CommonState.None;
	}

	public override void TriggerEnter(BaseRole ball)
	{
		if (commonState == CommonState.None)
		{
			base.TriggerEnter(ball);
			ball.TriggerRolePlayAnim(m_data.m_animType);
			commonState = CommonState.Active;
		}
	}

	public override void SetDefaultValue(object[] objs)
	{
		base.SetDefaultValue(objs);
		m_data = (RoleAnimatiorTriggerData)objs[0];
	}

	public override string Write()
	{
		return JsonUtility.ToJson(m_data);
	}

	public override void Read(string info)
	{
		m_data = JsonUtility.FromJson<RoleAnimatiorTriggerData>(info);
	}

	public override void ReadBytes(byte[] bytes)
	{
		m_data = StructTranslatorUtility.ToStructure<RoleAnimatiorTriggerData>(bytes);
	}

	public override byte[] WriteBytes()
	{
		return StructTranslatorUtility.ToByteArray(m_data);
	}
}
