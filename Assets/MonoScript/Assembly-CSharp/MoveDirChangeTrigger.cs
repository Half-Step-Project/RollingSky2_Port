using System;
using Foundation;
using UnityEngine;

public sealed class MoveDirChangeTrigger : BaseTriggerBox
{
	[Serializable]
	public struct TriggerData : IReadWriteBytes
	{
		public Railway.RailwayMoveDir MoveDir;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			MoveDir = (Railway.RailwayMoveDir)bytes.GetSingle(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			return ((int)MoveDir).GetBytes();
		}
	}

	public TriggerData data;

	public override bool IfRebirthRecord
	{
		get
		{
			return false;
		}
	}

	public override void SetDefaultValue(object[] objs)
	{
		if (objs != null)
		{
			data.MoveDir = (Railway.RailwayMoveDir)objs[0];
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
			Railway theRailway = Railway.theRailway;
			if (theRailway.GetMoveDir() != data.MoveDir)
			{
				theRailway.ChangeMoveDirTo(data.MoveDir);
				commonState = CommonState.Active;
			}
		}
	}

	public override void Read(string info)
	{
		data = JsonUtility.FromJson<TriggerData>(info);
	}

	public override string Write()
	{
		return JsonUtility.ToJson(data);
	}

	public override void ReadBytes(byte[] bytes)
	{
		data = StructTranslatorUtility.ToStructure<TriggerData>(bytes);
	}

	public override byte[] WriteBytes()
	{
		return StructTranslatorUtility.ToByteArray(data);
	}
}
