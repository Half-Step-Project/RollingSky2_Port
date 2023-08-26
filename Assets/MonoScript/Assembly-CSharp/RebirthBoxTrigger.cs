using System;
using System.IO;
using Foundation;
using UnityEngine;
using User.TileMap;

public class RebirthBoxTrigger : BaseTriggerBox
{
	[Serializable]
	public struct TileData : IReadWriteBytes
	{
		[Header("check point的位置")]
		public Vector3 m_move1LocalPosition;

		[Header("光幕的位置")]
		public Vector3 m_move2LocalPosition;

		public float m_beginDistance;

		public float m_resetDistance;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			m_move1LocalPosition = bytes.GetVector3(ref startIndex);
			m_move2LocalPosition = bytes.GetVector3(ref startIndex);
			m_beginDistance = bytes.GetSingle(ref startIndex);
			m_resetDistance = bytes.GetSingle(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(m_move1LocalPosition.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_move2LocalPosition.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_beginDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_resetDistance.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	public override bool IfRebirthRecord
	{
		get
		{
			return false;
		}
	}

	public override void Initialize()
	{
		base.gameObject.SetActive(false);
	}

	public static RebirthBoxData GetRebirthBoxData(int gridID, Point point, Transform gridTransform, Transform rebirthTransform)
	{
		return null;
	}
}
