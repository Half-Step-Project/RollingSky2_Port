using System;
using System.IO;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class MagicBoxTrigger : BaseTriggerBox
	{
		[Serializable]
		public struct TriggerData : IReadWriteBytes
		{
			public float ValidDistance;

			public float RotateAngle;

			public float RotateSpeed;

			public void ReadBytes(byte[] bytes)
			{
				int startIndex = 0;
				ValidDistance = bytes.GetSingle(ref startIndex);
				RotateAngle = bytes.GetSingle(ref startIndex);
				RotateSpeed = bytes.GetSingle(ref startIndex);
			}

			public byte[] WriteBytes()
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					int offset = 0;
					memoryStream.WriteByteArray(ValidDistance.GetBytes(), ref offset);
					memoryStream.WriteByteArray(RotateAngle.GetBytes(), ref offset);
					memoryStream.WriteByteArray(RotateSpeed.GetBytes(), ref offset);
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
			Debug.Log("name:" + base.gameObject.name);
		}

		public override void ResetElement()
		{
			base.ResetElement();
		}

		public override void TriggerEnter(BaseRole ball)
		{
			MagicBoxData magicBoxData = new MagicBoxData(data.ValidDistance, point, base.groupTransform, data.RotateAngle, data.RotateSpeed);
			MagicBoxCallEventArgs args = Mod.Reference.Acquire<MagicBoxCallEventArgs>().Initialize(magicBoxData);
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
