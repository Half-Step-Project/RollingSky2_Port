using System;
using Foundation;
using UnityEngine;

public class FlashMoveTrigger : BaseTriggerBox
{
	[Serializable]
	public struct TriggerData : IReadWriteBytes
	{
		public Vector3 TargetPos;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			TargetPos = bytes.GetVector3(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			return TargetPos.GetBytes();
		}
	}

	public TriggerData data;

	private Transform triggerPoint;

	public override bool IfRebirthRecord
	{
		get
		{
			return false;
		}
	}

	public override void SetDefaultValue(object[] objs)
	{
		Transform transform = TryGetTriggerPoint();
		if ((bool)transform)
		{
			transform.position = base.transform.position;
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
			Railway theRailway = Railway.theRailway;
			Vector3 forward = base.transform.forward;
			Vector3 position = theRailway.transform.position;
			float num = Vector3.Dot(data.TargetPos - position, forward);
			theRailway.transform.position = theRailway.transform.position + forward * num;
			commonState = CommonState.Active;
		}
	}

	public override void Read(string info)
	{
		data = JsonUtility.FromJson<TriggerData>(info);
		Transform transform = TryGetTriggerPoint();
		if ((bool)transform)
		{
			transform.position = data.TargetPos;
		}
	}

	public override string Write()
	{
		Transform transform = TryGetTriggerPoint();
		if ((bool)transform)
		{
			data.TargetPos = transform.position;
		}
		return JsonUtility.ToJson(data);
	}

	public override void ReadBytes(byte[] bytes)
	{
		data = StructTranslatorUtility.ToStructure<TriggerData>(bytes);
		Transform transform = TryGetTriggerPoint();
		if ((bool)transform)
		{
			transform.position = data.TargetPos;
		}
	}

	public override byte[] WriteBytes()
	{
		Transform transform = TryGetTriggerPoint();
		if ((bool)transform)
		{
			data.TargetPos = transform.position;
		}
		return StructTranslatorUtility.ToByteArray(data);
	}

	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Transform transform = TryGetTriggerPoint();
		if ((bool)transform)
		{
			Gizmos.DrawCube(transform.position, Vector3.one * 0.5f);
		}
	}

	private Transform TryGetTriggerPoint()
	{
		if (triggerPoint == null)
		{
			triggerPoint = base.transform.Find("triggerPoint");
		}
		return triggerPoint;
	}
}
