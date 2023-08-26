using System;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class RoleChangeMoodByEvent : BaseEnemy
	{
		[Serializable]
		public struct EnemyData : IReadWriteBytes
		{
			public BaseRole.MoodFaceType moodFaceType;

			public void ReadBytes(byte[] bytes)
			{
				int startIndex = 0;
				moodFaceType = (BaseRole.MoodFaceType)bytes.GetInt32(ref startIndex);
			}

			public byte[] WriteBytes()
			{
				return ((int)moodFaceType).GetBytes();
			}
		}

		public EnemyData data;

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
				Mod.Event.Fire(this, Mod.Reference.Acquire<RoleChangeMoodEventArgs>().Initialize(RoleChangeMoodEventArgs.TargetType.Role, data.moodFaceType));
				commonState = CommonState.Active;
			}
		}

		public override void CoupleTriggerEnter(BaseCouple couple, Collider collider)
		{
			if (commonState == CommonState.None)
			{
				commonState = CommonState.Active;
			}
		}

		public override void Read(string info)
		{
			data = JsonUtility.FromJson<EnemyData>(info);
		}

		public override string Write()
		{
			return JsonUtility.ToJson(data);
		}

		public override void ReadBytes(byte[] bytes)
		{
			data = StructTranslatorUtility.ToStructure<EnemyData>(bytes);
		}

		public override byte[] WriteBytes()
		{
			return StructTranslatorUtility.ToByteArray(data);
		}
	}
}
