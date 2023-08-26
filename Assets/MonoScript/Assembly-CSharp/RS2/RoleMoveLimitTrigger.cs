using System;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class RoleMoveLimitTrigger : BaseTriggerBox
	{
		[Serializable]
		public struct TriggerData : IReadWriteBytes
		{
			public float MoveLimit;

			public void ReadBytes(byte[] bytes)
			{
				int startIndex = 0;
				MoveLimit = bytes.GetSingle(ref startIndex);
			}

			public byte[] WriteBytes()
			{
				return MoveLimit.GetBytes();
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
			data.MoveLimit = 10f;
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
				ball.RoleMoveLimit = data.MoveLimit;
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
