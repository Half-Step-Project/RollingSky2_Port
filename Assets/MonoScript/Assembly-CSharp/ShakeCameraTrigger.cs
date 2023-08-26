using System;
using System.IO;
using Foundation;
using UnityEngine;

public class ShakeCameraTrigger : BaseTriggerBox
{
	[Serializable]
	public struct TriggerData : IReadWriteBytes
	{
		public bool IfReset;

		[Range(0f, 1f)]
		public float shakeForce;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			IfReset = bytes.GetBoolean(ref startIndex);
			shakeForce = bytes.GetSingle(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(IfReset.GetBytes(), ref offset);
				memoryStream.WriteByteArray(shakeForce.GetBytes(), ref offset);
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
		CameraShake.ShakeCamera(data.IfReset ? data.shakeForce : 0.5f);
	}

	public override void Read(string info)
	{
		if (!string.IsNullOrEmpty(info))
		{
			data = JsonUtility.FromJson<TriggerData>(info);
		}
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
