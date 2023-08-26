using System;
using System.IO;
using Foundation;
using UnityEngine;

public class InputResetTrigger : BaseTriggerBox
{
	[Serializable]
	public struct TriggerData : IReadWriteBytes
	{
		public float TargetScaler;

		public float SlideSpeed;

		public float NormalizeSpeed;

		public float Sensitivity;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			TargetScaler = bytes.GetSingle(ref startIndex);
			SlideSpeed = bytes.GetSingle(ref startIndex);
			NormalizeSpeed = bytes.GetSingle(ref startIndex);
			Sensitivity = bytes.GetSingle(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(TargetScaler.GetBytes(), ref offset);
				memoryStream.WriteByteArray(SlideSpeed.GetBytes(), ref offset);
				memoryStream.WriteByteArray(NormalizeSpeed.GetBytes(), ref offset);
				memoryStream.WriteByteArray(Sensitivity.GetBytes(), ref offset);
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
		CameraMover.targetScaler = data.TargetScaler;
		BaseRole.theBall.SlideSpeed = data.SlideSpeed;
		InputController.NormalizeSpeed = data.NormalizeSpeed;
		InputController.Sensitivity = data.Sensitivity;
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

	public override void SetDefaultValue(object[] objs)
	{
		if (objs != null)
		{
			data.TargetScaler = (float)objs[0];
			data.SlideSpeed = (float)objs[1];
			data.NormalizeSpeed = (float)objs[2];
			data.Sensitivity = (float)objs[3];
		}
	}
}
