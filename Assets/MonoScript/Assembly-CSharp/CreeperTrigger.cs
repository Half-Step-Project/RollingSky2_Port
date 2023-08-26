using System;
using System.IO;
using Foundation;
using UnityEngine;

public class CreeperTrigger : BaseTriggerBox
{
	[Serializable]
	public struct TriggerData : IReadWriteBytes
	{
		public float BeginDistance;

		public float MoveDistance;

		public float SpeedScaler;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			BeginDistance = bytes.GetSingle(ref startIndex);
			MoveDistance = bytes.GetSingle(ref startIndex);
			SpeedScaler = bytes.GetSingle(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(BeginDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(MoveDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(SpeedScaler.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	public TriggerData data;

	public Material creeperMat;

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
		creeperMat = base.transform.Find("model").GetComponentInChildren<MeshRenderer>().material;
		PlayByPercent(0f);
	}

	public override void ResetElement()
	{
		base.ResetElement();
		PlayByPercent(0f);
	}

	public override void SetDefaultValue(object[] objs)
	{
		if (objs != null)
		{
			data.BeginDistance = (float)objs[0];
			data.MoveDistance = (float)objs[1];
			data.SpeedScaler = (float)objs[2];
		}
	}

	public override void UpdateElement()
	{
		float distance = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z - data.BeginDistance;
		PlayByPercent(GetPercent(distance));
	}

	public override void PlayByPercent(float percent)
	{
		if ((bool)creeperMat)
		{
			creeperMat.SetFloat("_FadeIn", percent);
		}
	}

	public override float GetPercent(float distance)
	{
		return Mathf.Clamp(distance * data.SpeedScaler / data.MoveDistance, 0f, 1f);
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
