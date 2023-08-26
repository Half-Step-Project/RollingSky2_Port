using System;
using Foundation;
using RS2;
using UnityEngine;

public class PathToMoveByDiamondAwardSendTrigger : BaseTriggerBox
{
	[Serializable]
	public struct Data
	{
		public int m_groupID;

		public static Data DefaultValue
		{
			get
			{
				Data result = default(Data);
				result.m_groupID = 1;
				return result;
			}
		}
	}

	public Data m_data = Data.DefaultValue;

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

	public override void TriggerEnter(BaseRole ball)
	{
		base.TriggerEnter(ball);
		PathToMoveByDiamondAwardEventArgs args = Mod.Reference.Acquire<PathToMoveByDiamondAwardEventArgs>().Initialize(m_data.m_groupID);
		Mod.Event.FireNow(EventArgs<PathToMoveByDiamondAwardEventArgs>.EventId, args);
	}
}
