using System;
using Foundation;
using RS2;
using UnityEngine;

public class SwitchSendTrigger : BaseTriggerBox
{
	[Serializable]
	public struct Data
	{
		public SwitchSendData[] m_sendDatas;

		public static Data DefaultValue
		{
			get
			{
				Data result = default(Data);
				result.m_sendDatas = new SwitchSendData[1] { SwitchSendData.DefaultValue };
				return result;
			}
		}
	}

	public Data m_data;

	public override void TriggerEnter(BaseRole ball)
	{
		base.TriggerEnter(ball);
		SwitchEventArgs args = Mod.Reference.Acquire<SwitchEventArgs>().Initialize(m_data);
		Mod.Event.FireNow(EventArgs<SwitchEventArgs>.EventId, args);
	}

	public override string Write()
	{
		return JsonUtility.ToJson(m_data);
	}

	public override void Read(string info)
	{
		m_data = JsonUtility.FromJson<Data>(info);
	}

	public override byte[] WriteBytes()
	{
		return Bson.ToBson(m_data);
	}

	public override void ReadBytes(byte[] bytes)
	{
		m_data = Bson.ToObject<Data>(bytes);
	}

	public override void SetDefaultValue(object[] objs)
	{
		m_data = (Data)objs[0];
	}
}
