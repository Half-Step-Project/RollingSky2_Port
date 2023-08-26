using System;
using Foundation;
using RS2;
using UnityEngine;

public class PathGuideSendTrigger : BaseTriggerBox
{
	[Serializable]
	public struct Data
	{
		public GroupsSendData groupsSendData;

		public static Data DefaultValue
		{
			get
			{
				Data result = default(Data);
				result.groupsSendData = GroupsSendData.DefaultValue;
				return result;
			}
		}
	}

	public Data mData;

	public override void TriggerEnter(BaseRole ball)
	{
		base.TriggerEnter(ball);
		PathGuideSendEventArgs args = Mod.Reference.Acquire<PathGuideSendEventArgs>().Initialize(mData.groupsSendData);
		Mod.Event.FireNow(EventArgs<PathGuideSendEventArgs>.EventId, args);
	}

	public override string Write()
	{
		return JsonUtility.ToJson(mData);
	}

	public override void Read(string info)
	{
		if (!string.IsNullOrEmpty(info))
		{
			mData = JsonUtility.FromJson<Data>(info);
		}
	}

	public override byte[] WriteBytes()
	{
		return Bson.ToBson(mData);
	}

	public override void ReadBytes(byte[] bytes)
	{
		if (bytes != null)
		{
			mData = Bson.ToObject<Data>(bytes);
		}
	}

	public override void SetDefaultValue(object[] objs)
	{
		if (objs != null && objs.Length != 0)
		{
			mData = (Data)objs[0];
		}
	}
}
