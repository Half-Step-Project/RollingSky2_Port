using System;
using System.IO;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class FreeMoveTrigger : BaseTriggerBox
	{
		[Serializable]
		public struct TriggerData : IReadWriteBytes
		{
			public float ValidDistance;

			public bool IfTriggerControl;

			public float BeginDistance;

			public float SpeedScaler;

			public float MoveScaler;

			public void ReadBytes(byte[] bytes)
			{
				int startIndex = 0;
				ValidDistance = bytes.GetSingle(ref startIndex);
				IfTriggerControl = bytes.GetBoolean(ref startIndex);
				BeginDistance = bytes.GetSingle(ref startIndex);
				SpeedScaler = bytes.GetSingle(ref startIndex);
				MoveScaler = bytes.GetSingle(ref startIndex);
			}

			public byte[] WriteBytes()
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					int offset = 0;
					memoryStream.WriteByteArray(ValidDistance.GetBytes(), ref offset);
					memoryStream.WriteByteArray(IfTriggerControl.GetBytes(), ref offset);
					memoryStream.WriteByteArray(BeginDistance.GetBytes(), ref offset);
					memoryStream.WriteByteArray(SpeedScaler.GetBytes(), ref offset);
					memoryStream.WriteByteArray(MoveScaler.GetBytes(), ref offset);
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
			FreeMoveData moveData = new FreeMoveData(data.ValidDistance, base.transform.position, base.groupTransform, data.IfTriggerControl, data.BeginDistance, data.SpeedScaler, data.MoveScaler);
			Mod.Event.Fire(this, Mod.Reference.Acquire<TileFreeMoveCallEventArgs>().Initialize(moveData));
		}

		public override void SetDefaultValue(object[] objs)
		{
			data.ValidDistance = (float)objs[0];
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
