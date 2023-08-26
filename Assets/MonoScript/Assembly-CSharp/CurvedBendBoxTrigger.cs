using System;
using System.IO;
using Foundation;
using UnityEngine;

public class CurvedBendBoxTrigger : BaseTriggerBox
{
	[Serializable]
	public struct TileData : IReadWriteBytes
	{
		public float m_xAxisBendSize;

		public float m_xBias;

		public float m_yAxisBendSize;

		public float m_yBias;

		public float m_duration;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			m_xAxisBendSize = bytes.GetSingle(ref startIndex);
			m_xBias = bytes.GetSingle(ref startIndex);
			m_yAxisBendSize = bytes.GetSingle(ref startIndex);
			m_yBias = bytes.GetSingle(ref startIndex);
			m_duration = bytes.GetSingle(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(m_xAxisBendSize.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_xBias.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_yAxisBendSize.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_yBias.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_duration.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	public TileData m_tileData;

	public override bool IfRebirthRecord
	{
		get
		{
			return false;
		}
	}

	public override void TriggerEnter(BaseRole ball)
	{
		base.TriggerEnter(ball);
		CurvedBendData to = default(CurvedBendData);
		to.m_xAxisBendSize = m_tileData.m_xAxisBendSize;
		to.m_yAxisBendSize = m_tileData.m_yAxisBendSize;
		to.m_xBias = m_tileData.m_xBias;
		to.m_yBias = m_tileData.m_yBias;
		CameraController.theCamera.PlayCurvedBend(to, m_tileData.m_duration, 0f);
	}

	public override void SetDefaultValue(object[] objs)
	{
		base.SetDefaultValue(objs);
		m_tileData = default(TileData);
		m_tileData.m_xAxisBendSize = (float)objs[0];
		m_tileData.m_xBias = (float)objs[1];
		m_tileData.m_yAxisBendSize = (float)objs[2];
		m_tileData.m_yBias = (float)objs[3];
		m_tileData.m_duration = (float)objs[4];
	}

	public override void ResetElement()
	{
		base.ResetElement();
	}

	public override void Read(string info)
	{
		if (!string.IsNullOrEmpty(info))
		{
			m_tileData = JsonUtility.FromJson<TileData>(info);
		}
	}

	public override string Write()
	{
		string empty = string.Empty;
		return JsonUtility.ToJson(m_tileData);
	}

	public override void ReadBytes(byte[] bytes)
	{
		m_tileData = StructTranslatorUtility.ToStructure<TileData>(bytes);
	}

	public override byte[] WriteBytes()
	{
		return StructTranslatorUtility.ToByteArray(m_tileData);
	}
}
