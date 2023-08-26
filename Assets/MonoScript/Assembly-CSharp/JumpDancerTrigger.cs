using System;
using System.IO;
using Foundation;
using UnityEngine;

public class JumpDancerTrigger : BaseTriggerBox
{
	[Serializable]
	public struct TriggerData : IReadWriteBytes
	{
		public float JumpDistance;

		public float JumpHeight;

		public float BeginDistance;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			JumpDistance = bytes.GetSingle(ref startIndex);
			JumpHeight = bytes.GetSingle(ref startIndex);
			BeginDistance = bytes.GetSingle(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(JumpDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(JumpHeight.GetBytes(), ref offset);
				memoryStream.WriteByteArray(BeginDistance.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	public static readonly string WaitAnimName = "anim01";

	public static readonly string CollideAnimName = "anim02";

	public TriggerData data;

	private Animation anim;

	[Range(0f, 2f)]
	public float debugPercent;

	private JumpUtil debugJumpInfo;

	public override bool IfRebirthRecord
	{
		get
		{
			return false;
		}
	}

	public override void SetDefaultValue(object[] objs)
	{
		if (objs != null)
		{
			data.JumpDistance = (float)objs[0];
			data.JumpHeight = (float)objs[1];
		}
	}

	public override void Initialize()
	{
		base.Initialize();
		anim = base.transform.GetComponentInChildren<Animation>();
		commonState = CommonState.None;
	}

	public override void ResetElement()
	{
		base.ResetElement();
		commonState = CommonState.None;
		PlayAnim(anim, false);
	}

	public override void UpdateElement()
	{
		if (commonState == CommonState.None)
		{
			if (base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z >= data.BeginDistance)
			{
				PlayAnim(anim, WaitAnimName, true);
				commonState = CommonState.Begin;
			}
		}
		else if (commonState != CommonState.Begin)
		{
			CommonState commonState2 = commonState;
			int num = 2;
		}
	}

	public override void TriggerEnter(BaseRole ball)
	{
		if (commonState == CommonState.Begin)
		{
			Vector3 position = ball.transform.position;
			Vector3 forward = ball.transform.forward;
			Vector3 endPos = position + forward * data.JumpDistance;
			ball.CallBeginJump(position, endPos, forward, data.JumpHeight, BaseRole.JumpType.Dance);
			Vector3 position2 = base.transform.position;
			base.transform.position = new Vector3(position.x, position2.y, position.z);
			PlayAnim(anim, CollideAnimName, true);
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

	public override void OnDrawGizmos()
	{
	}
}
