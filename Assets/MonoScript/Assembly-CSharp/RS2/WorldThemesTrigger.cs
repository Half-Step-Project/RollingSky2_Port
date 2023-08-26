using System;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class WorldThemesTrigger : BaseTriggerBox
	{
		[Serializable]
		public struct TriggerData : IReadWriteBytes
		{
			public int m_themeIndex;

			public void ReadBytes(byte[] bytes)
			{
				int startIndex = 0;
				m_themeIndex = bytes.GetInt32(ref startIndex);
			}

			public byte[] WriteBytes()
			{
				return m_themeIndex.GetBytes();
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
			Mod.Event.Fire(this, Mod.Reference.Acquire<GameWorldThemeChangeEventArgs>().Initialize(data.m_themeIndex));
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
}
