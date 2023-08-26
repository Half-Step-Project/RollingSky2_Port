using System;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class CameraClipTrigger : BaseTriggerBox
	{
		[Serializable]
		public struct TriggerData : IReadWriteBytes
		{
			public float clipFar;

			public void ReadBytes(byte[] bytes)
			{
				int startIndex = 0;
				clipFar = bytes.GetSingle(ref startIndex);
			}

			public byte[] WriteBytes()
			{
				return clipFar.GetBytes();
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

		public override void SetDefaultValue(object[] objs)
		{
			if (objs != null)
			{
				data.clipFar = (float)objs[0];
			}
		}

		public override void TriggerEnter(BaseRole ball)
		{
			if (commonState == CommonState.None)
			{
				Camera.main.farClipPlane = data.clipFar;
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

		[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
		public override void RebirthReadData(object rd_data)
		{
		}

		[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
		public override object RebirthWriteData()
		{
			return string.Empty;
		}

		[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
		public override void RebirthStartGame(object rd_data)
		{
		}

		public override void RebirthReadByteData(byte[] rd_data)
		{
		}

		public override byte[] RebirthWriteByteData()
		{
			return null;
		}

		public override void RebirthStartByteGame(byte[] rd_data)
		{
		}
	}
}
