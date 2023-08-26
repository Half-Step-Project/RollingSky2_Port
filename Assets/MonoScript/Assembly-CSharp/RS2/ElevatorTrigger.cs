using System;
using System.IO;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class ElevatorTrigger : BaseTriggerBox
	{
		[Serializable]
		public struct TriggerData : IReadWriteBytes
		{
			public float ValidDistance;

			public bool IfUp;

			public int ValidIndex;

			public void ReadBytes(byte[] bytes)
			{
				int startIndex = 0;
				ValidDistance = bytes.GetSingle(ref startIndex);
				IfUp = bytes.GetBoolean(ref startIndex);
				ValidIndex = bytes.GetInt32(ref startIndex);
			}

			public byte[] WriteBytes()
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					int offset = 0;
					memoryStream.WriteByteArray(ValidDistance.GetBytes(), ref offset);
					memoryStream.WriteByteArray(IfUp.GetBytes(), ref offset);
					memoryStream.WriteByteArray(ValidIndex.GetBytes(), ref offset);
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

		public override void SetDefaultValue(object[] objs)
		{
			data.ValidDistance = (float)objs[0];
			data.IfUp = (bool)objs[1];
			data.ValidIndex = (int)objs[2];
		}

		public override void TriggerEnter(BaseRole ball)
		{
			ElevatorMoveEventArgs elevatorMoveEventArgs = Mod.Reference.Acquire<ElevatorMoveEventArgs>();
			elevatorMoveEventArgs.Initialize(data.ValidDistance, point, base.groupTransform, data.IfUp, ball.transform.position, data.ValidIndex);
			Mod.Event.FireNow(this, elevatorMoveEventArgs);
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
