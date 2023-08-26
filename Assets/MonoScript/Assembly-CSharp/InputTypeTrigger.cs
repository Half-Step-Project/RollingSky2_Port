using System;
using Foundation;
using UnityEngine;

public class InputTypeTrigger : BaseTriggerBox
{
	[Serializable]
	public struct TriggerData : IReadWriteBytes
	{
		public BaseRole.InputType InputType;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			InputType = (BaseRole.InputType)bytes.GetInt32(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			return ((int)InputType).GetBytes();
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

	public override void TriggerEnter(BaseRole ball)
	{
		ball.ChangeInputType(data.InputType);
	}

	public override void Read(string info)
	{
		if (!string.IsNullOrEmpty(info))
		{
			data = JsonUtility.FromJson<TriggerData>(info);
		}
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
