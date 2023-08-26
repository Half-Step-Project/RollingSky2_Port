using System;
using System.IO;
using Foundation;
using RS2;
using UnityEngine;

public class WindOpenTrigger : BaseTriggerBox
{
	[Serializable]
	public struct WindData : IReadWriteBytes
	{
		[Header("风向")]
		public bool m_isRightWind;

		[Header("风的强度")]
		[Range(1f, 20f)]
		public float m_windIntensity;

		[Header("风的阻力")]
		[Range(1f, 20f)]
		public float m_windResistance;

		[Header("未操作时，风的强度")]
		[Range(1f, 20f)]
		public float m_stillWindIntensity;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			m_isRightWind = bytes.GetBoolean(ref startIndex);
			m_windIntensity = bytes.GetSingle(ref startIndex);
			m_windResistance = bytes.GetSingle(ref startIndex);
			m_stillWindIntensity = bytes.GetSingle(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(m_isRightWind.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_windIntensity.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_windResistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_stillWindIntensity.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	public WindData m_data;

	public override bool IfRebirthRecord
	{
		get
		{
			return false;
		}
	}

	public override void SetDefaultValue(object[] objs)
	{
		base.SetDefaultValue(objs);
		m_data = (WindData)objs[0];
	}

	public override void TriggerEnter(BaseRole ball)
	{
		base.TriggerEnter(ball);
		Mod.Event.FireNow(this, Mod.Reference.Acquire<WindOpenEventArgs>().Initialize(m_data));
	}

	public override void Read(string info)
	{
		m_data = JsonUtility.FromJson<WindData>(info);
	}

	public override string Write()
	{
		return JsonUtility.ToJson(m_data);
	}

	public override void ReadBytes(byte[] bytes)
	{
		m_data = StructTranslatorUtility.ToStructure<WindData>(bytes);
	}

	public override byte[] WriteBytes()
	{
		return StructTranslatorUtility.ToByteArray(m_data);
	}

	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = Color.blue;
		Vector3 vector = base.gameObject.transform.position + new Vector3(0f, 2f, 0f);
		Vector3[] array = new Vector3[8]
		{
			new Vector3(0f, 0f, 2f),
			new Vector3(-2f, 0f, 0f),
			new Vector3(-1f, 0f, 0f),
			new Vector3(-1f, 0f, -2f),
			new Vector3(1f, 0f, -2f),
			new Vector3(1f, 0f, 0f),
			new Vector3(2f, 0f, 0f),
			new Vector3(0f, 0f, 2f)
		};
		Vector3[] array2 = new Vector3[8]
		{
			new Vector3(0f, 0f, 2f),
			new Vector3(0f, -2f, 0f),
			new Vector3(0f, -1f, 0f),
			new Vector3(0f, -1f, -2f),
			new Vector3(0f, 1f, -2f),
			new Vector3(0f, 1f, 0f),
			new Vector3(0f, 2f, 0f),
			new Vector3(0f, 0f, 2f)
		};
		Vector3 vector2 = new Vector3(0f, 90f, 0f);
		vector2 = ((!m_data.m_isRightWind) ? new Vector3(0f, -90f, 0f) : new Vector3(0f, 90f, 0f));
		for (int i = 0; i < array.Length - 1; i++)
		{
			Vector3 from = vector + Quaternion.Euler(vector2) * array[i];
			Vector3 to = vector + Quaternion.Euler(vector2) * array[i + 1];
			Gizmos.DrawLine(from, to);
		}
		for (int j = 0; j < array2.Length - 1; j++)
		{
			Vector3 from2 = vector + Quaternion.Euler(vector2) * array2[j];
			Vector3 to2 = vector + Quaternion.Euler(vector2) * array2[j + 1];
			Gizmos.DrawLine(from2, to2);
		}
		Gizmos.DrawIcon(vector, "windZone.png");
	}
}
