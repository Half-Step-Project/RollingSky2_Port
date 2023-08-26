using System;
using Foundation;
using RS2;
using UnityEngine;

public class RoleOutFireTrigger : BaseTriggerBox
{
	[Serializable]
	public struct Data
	{
		public int groupID;

		public static Data DefaultValue
		{
			get
			{
				Data result = default(Data);
				result.groupID = 1;
				return result;
			}
		}
	}

	[Serializable]
	public class RebirthData
	{
		public RD_ElementTransform_DATA trans;
	}

	public Data mData;

	private RebirthData mRebirthData;

	public override void Initialize()
	{
	}

	public override void UpdateElement()
	{
	}

	public override void ResetElement()
	{
	}

	public override void TriggerEnter(BaseRole ball)
	{
		OutFireEventArgs args = Mod.Reference.Acquire<OutFireEventArgs>().Initialize(new GroupSendData
		{
			groupID = mData.groupID
		});
		Mod.Event.FireNow(EventArgs<OutFireEventArgs>.EventId, args);
	}

	public override string Write()
	{
		return JsonUtility.ToJson(mData);
	}

	public override byte[] WriteBytes()
	{
		return Bson.ToBson(mData);
	}

	public override void Read(string info)
	{
		mData = JsonUtility.FromJson<Data>(info);
	}

	public override void ReadBytes(byte[] bytes)
	{
		mData = Bson.ToObject<Data>(bytes);
	}

	public override void SetDefaultValue(object[] objs)
	{
		mData = (Data)objs[0];
	}

	public override object RebirthWriteData()
	{
		return JsonUtility.ToJson(new RebirthData
		{
			trans = base.gameObject.transform.GetTransData()
		});
	}

	public override byte[] RebirthWriteByteData()
	{
		return Bson.ToBson(new RebirthData
		{
			trans = base.gameObject.transform.GetTransData()
		});
	}

	public override void RebirthReadData(object rd_data)
	{
		if (rd_data != null)
		{
			string text = (string)rd_data;
			if (!string.IsNullOrEmpty(text))
			{
				mRebirthData = JsonUtility.FromJson<RebirthData>(text);
				base.gameObject.transform.SetTransData(mRebirthData.trans);
			}
		}
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
		if (rd_data != null)
		{
			mRebirthData = Bson.ToObject<RebirthData>(rd_data);
			base.gameObject.transform.SetTransData(mRebirthData.trans);
		}
	}

	public override void RebirthStartGame(object rd_data)
	{
		if (mRebirthData != null)
		{
			mRebirthData = null;
		}
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
		if (mRebirthData != null)
		{
			mRebirthData = null;
		}
	}
}
