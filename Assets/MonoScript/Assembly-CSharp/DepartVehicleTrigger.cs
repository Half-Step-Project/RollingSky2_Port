using System;
using Foundation;
using UnityEngine;

public class DepartVehicleTrigger : BaseTriggerBox
{
	[Serializable]
	public struct TriggerData : IReadWriteBytes
	{
		public bool IfDropDie;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			IfDropDie = bytes.GetBoolean(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			return IfDropDie.GetBytes();
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
		if (data.IfDropDie)
		{
			if (!ball.IsInvincible)
			{
				ball.DepartVehicle();
				ball.BeginDropDie();
			}
		}
		else
		{
			ball.DepartVehicle();
			ball.ChangeToMove();
		}
	}

	public override string Write()
	{
		return JsonUtility.ToJson(data);
	}

	public override void Read(string info)
	{
		data = JsonUtility.FromJson<TriggerData>(info);
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
