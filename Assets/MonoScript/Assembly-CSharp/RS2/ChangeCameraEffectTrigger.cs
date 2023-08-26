using System;
using System.IO;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class ChangeCameraEffectTrigger : BaseTriggerBox
	{
		[Serializable]
		public struct TriggerData : IReadWriteBytes
		{
			public bool IfShowEffect;

			public int ParticleIndex;

			public void ReadBytes(byte[] bytes)
			{
				int startIndex = 0;
				IfShowEffect = bytes.GetBoolean(ref startIndex);
				ParticleIndex = bytes.GetInt32(ref startIndex);
			}

			public byte[] WriteBytes()
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					int offset = 0;
					memoryStream.WriteByteArray(IfShowEffect.GetBytes(), ref offset);
					memoryStream.WriteByteArray(ParticleIndex.GetBytes(), ref offset);
					memoryStream.Flush();
					memoryStream.Seek(0L, SeekOrigin.Begin);
					return memoryStream.ToArray();
				}
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
				Mod.Event.Fire(this, Mod.Reference.Acquire<CameraParticlePlayEventArgs>().Initialize(data.IfShowEffect, data.ParticleIndex));
				commonState = CommonState.Active;
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
}
