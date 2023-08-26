using System;
using Foundation;
using UnityEngine;

public class ForceHorizonTrigger : BaseTriggerBox
{
	[Serializable]
	public struct TriggerData : IReadWriteBytes
	{
		public Vector3 EndPos;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			EndPos = bytes.GetVector3(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			return EndPos.GetBytes();
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
		data.EndPos = base.transform.position;
		base.transform.Find("endPoint").transform.position = data.EndPos;
	}

	public override void TriggerEnter(BaseRole ball)
	{
		if ((bool)ball)
		{
			BaseRole.ForceHorizonData forceHorizonData = new BaseRole.ForceHorizonData();
			forceHorizonData.BeginPos = ball.transform.position;
			forceHorizonData.EndPos = data.EndPos;
			forceHorizonData.GridTrans = base.groupTransform;
			ball.SetForceHorizonData(forceHorizonData);
		}
	}

	public override void Read(string info)
	{
		data = JsonUtility.FromJson<TriggerData>(info);
		base.transform.Find("endPoint").transform.position = data.EndPos;
	}

	public override string Write()
	{
		data.EndPos = base.transform.Find("endPoint").transform.position;
		return JsonUtility.ToJson(data);
	}

	public override void ReadBytes(byte[] bytes)
	{
		data = StructTranslatorUtility.ToStructure<TriggerData>(bytes);
		base.transform.Find("endPoint").transform.position = data.EndPos;
	}

	public override byte[] WriteBytes()
	{
		data.EndPos = base.transform.Find("endPoint").transform.position;
		return StructTranslatorUtility.ToByteArray(data);
	}

	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Transform transform = base.transform.Find("endPoint");
		if ((bool)transform)
		{
			Color color = Gizmos.color;
			Gizmos.color = Color.red;
			Gizmos.DrawLine(base.transform.position, transform.position);
			Gizmos.DrawCube(transform.position, new Vector3(0.2f, 0.2f, 0.2f));
			Gizmos.color = color;
		}
	}
}
