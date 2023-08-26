using System;
using Foundation;
using RS2;
using UnityEngine;

public class CrownFragmentTrigger : BaseTriggerBox, IAwardFragement, IAward
{
	[Serializable]
	public struct Data
	{
		[Label]
		public int sortID;

		[Label]
		public int needCount;

		[Label]
		public int completeSortID;

		public static Data DefaultValue
		{
			get
			{
				Data result = default(Data);
				result.sortID = 0;
				result.needCount = 0;
				result.completeSortID = 0;
				return result;
			}
		}
	}

	public Data mData;

	public override void TriggerEnter(BaseRole ball)
	{
		ball.GainCrownFragment(m_uuId);
		Mod.Event.FireNow(this, ChangeRoleIntroductionIdentifierEventArgs.Make(true, 16, mData.needCount));
	}

	public int GetNeedFragementCount()
	{
		return mData.needCount;
	}

	public void SetNeedFragementCount(int count)
	{
		mData.needCount = count;
	}

	public void SetCompleteSortID(int sortID)
	{
		mData.completeSortID = sortID;
	}

	public int GetCompleteSortID()
	{
		return mData.completeSortID;
	}

	public DropType GetCompleteDropType()
	{
		return DropType.CROWN;
	}

	public int GetAwardSortID()
	{
		return mData.sortID;
	}

	public void SetAwardSortID(int id)
	{
		mData.sortID = id;
	}

	public DropType GetDropType()
	{
		return DropType.CROWNFRAGMENT;
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
		if (objs != null && objs[0] is Data)
		{
			mData = (Data)objs[0];
		}
		else
		{
			mData = Data.DefaultValue;
		}
	}
}
