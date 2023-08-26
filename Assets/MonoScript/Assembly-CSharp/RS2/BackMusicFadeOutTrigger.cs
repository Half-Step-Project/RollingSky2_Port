using System;
using System.IO;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class BackMusicFadeOutTrigger : BaseTriggerBox
	{
		[Serializable]
		public struct TriggerData : IReadWriteBytes
		{
			public float DelayTime;

			public float FadeOutTime;

			public float BeginVolume;

			public float EndVolume;

			public void ReadBytes(byte[] bytes)
			{
				int startIndex = 0;
				DelayTime = bytes.GetSingle(ref startIndex);
				FadeOutTime = bytes.GetSingle(ref startIndex);
				BeginVolume = bytes.GetSingle(ref startIndex);
				EndVolume = bytes.GetSingle(ref startIndex);
			}

			public byte[] WriteBytes()
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					int offset = 0;
					memoryStream.WriteByteArray(DelayTime.GetBytes(), ref offset);
					memoryStream.WriteByteArray(FadeOutTime.GetBytes(), ref offset);
					memoryStream.WriteByteArray(BeginVolume.GetBytes(), ref offset);
					memoryStream.WriteByteArray(EndVolume.GetBytes(), ref offset);
					memoryStream.Flush();
					memoryStream.Seek(0L, SeekOrigin.Begin);
					return memoryStream.ToArray();
				}
			}
		}

		public TriggerData data;

		public override void TriggerEnter(BaseRole ball)
		{
			BackMusicFadeOutEventArgs args = Mod.Reference.Acquire<BackMusicFadeOutEventArgs>().Initialize(data.DelayTime, data.FadeOutTime, data.BeginVolume, data.EndVolume);
			Mod.Event.Fire(this, args);
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
