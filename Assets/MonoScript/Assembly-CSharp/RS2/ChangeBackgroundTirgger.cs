using System;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class ChangeBackgroundTirgger : BaseTriggerBox
	{
		[Serializable]
		public struct TriggerData : IReadWriteBytes
		{
			public int ShowIndex;

			public void ReadBytes(byte[] bytes)
			{
				int startIndex = 0;
				ShowIndex = bytes.GetInt32(ref startIndex);
			}

			public byte[] WriteBytes()
			{
				return ShowIndex.GetBytes();
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
			Mod.Event.FireNow(this, Mod.Reference.Acquire<ChangeBackgroundEventArgs>().Initialize(data.ShowIndex));
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
}
