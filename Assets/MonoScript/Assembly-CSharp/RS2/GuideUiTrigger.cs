using System;
using System.IO;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class GuideUiTrigger : BaseTriggerBox
	{
		[Serializable]
		public struct TriggerData : IReadWriteBytes
		{
			public int UIIndex;

			public void ReadBytes(byte[] bytes)
			{
				int startIndex = 0;
				UIIndex = bytes.GetInt32(ref startIndex);
			}

			public byte[] WriteBytes()
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					int offset = 0;
					memoryStream.WriteByteArray(UIIndex.GetBytes(), ref offset);
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

		public override void TriggerEnter(BaseRole ball)
		{
			Mod.Event.Fire(this, Mod.Reference.Acquire<GuideUiEventArgs>().Initialize(data.UIIndex));
			commonState = CommonState.InActive;
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

		public override void OnDrawGizmos()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawRay(base.transform.position, Vector3.up);
		}
	}
}
