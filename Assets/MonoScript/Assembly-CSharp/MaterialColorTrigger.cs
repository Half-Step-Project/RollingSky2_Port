using System;
using Foundation;
using UnityEngine;

public class MaterialColorTrigger : BaseTriggerBox
{
	[Serializable]
	public struct MaterialData : IReadWriteBytes
	{
		public MaterialTheme _materialTheme;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			_materialTheme = (MaterialTheme)bytes.GetInt32(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			return ((int)_materialTheme).GetBytes();
		}
	}

	public MaterialData data;

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
	}

	public override void TriggerEnter(BaseRole ball)
	{
	}

	public override void Read(string info)
	{
		data = JsonUtility.FromJson<MaterialData>(info);
	}

	public override string Write()
	{
		return JsonUtility.ToJson(data);
	}

	public override void ReadBytes(byte[] bytes)
	{
		data = StructTranslatorUtility.ToStructure<MaterialData>(bytes);
	}

	public override byte[] WriteBytes()
	{
		return StructTranslatorUtility.ToByteArray(data);
	}
}
