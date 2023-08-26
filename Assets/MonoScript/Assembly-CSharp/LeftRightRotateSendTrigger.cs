using System;
using Foundation;
using RS2;
using UnityEngine;

public class LeftRightRotateSendTrigger : BaseTriggerBox
{
	[Serializable]
	public struct Data
	{
		public LeftRightRotateSendData[] m_datas;

		public static Data DefaultValue
		{
			get
			{
				Data result = default(Data);
				result.m_datas = new LeftRightRotateSendData[1]
				{
					new LeftRightRotateSendData
					{
						m_state = LeftRightState.Left,
						m_groupID = 1
					}
				};
				return result;
			}
		}
	}

	public Data m_data;

	public override void TriggerEnter(BaseRole ball)
	{
		base.TriggerEnter(ball);
		LeftRightRotateEventArg args = Mod.Reference.Acquire<LeftRightRotateEventArg>().Initialize(m_data);
		Mod.Event.FireNow(EventArgs<LeftRightRotateEventArg>.EventId, args);
	}

	public override string Write()
	{
		return JsonUtility.ToJson(m_data);
	}

	public override void Read(string info)
	{
		m_data = JsonUtility.FromJson<Data>(info);
	}

	public override void ReadBytes(byte[] bytes)
	{
		m_data = Bson.ToObject<Data>(bytes);
	}

	public override byte[] WriteBytes()
	{
		return Bson.ToBson(m_data);
	}

	public override void SetDefaultValue(object[] objs)
	{
		m_data = (Data)objs[0];
	}
}
