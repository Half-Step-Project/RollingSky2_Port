using System;
using System.IO;
using Foundation;
using UnityEngine;

public class PathToMoveByPetTrigger : BaseTriggerBox
{
	[Serializable]
	public struct PathToMoveByPetTriggerData : IReadWriteBytes
	{
		public float m_time;

		public bool m_isLookAtNextPoint;

		public float m_lookAtSpeed;

		public Vector3[] m_positions;

		public bool m_isFinishedStop;

		[Header("isFinishedStop=true 是才会执行")]
		public PetState m_finishedRangePetState;

		public Vector3[] m_bezierPositions;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			m_time = bytes.GetSingle(ref startIndex);
			m_isLookAtNextPoint = bytes.GetBoolean(ref startIndex);
			m_lookAtSpeed = bytes.GetSingle(ref startIndex);
			m_positions = bytes.GetVector3Array(ref startIndex);
			m_isFinishedStop = bytes.GetBoolean(ref startIndex);
			m_finishedRangePetState = (PetState)bytes.GetInt32(ref startIndex);
			m_bezierPositions = bytes.GetVector3Array(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(m_time.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_isLookAtNextPoint.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_lookAtSpeed.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_positions.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_isFinishedStop.GetBytes(), ref offset);
				memoryStream.WriteByteArray(((int)m_finishedRangePetState).GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_bezierPositions.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	public PathToMoveByPetTriggerData m_data;

	public override bool IfRebirthRecord
	{
		get
		{
			return false;
		}
	}

	public override void TriggerEnter(BaseRole ball)
	{
		PathToMoveByPetTriggerData data = default(PathToMoveByPetTriggerData);
		if (m_data.m_bezierPositions == null || m_data.m_bezierPositions.Length == 0)
		{
			data.m_positions = new Vector3[m_data.m_positions.Length];
			for (int i = 0; i < m_data.m_positions.Length; i++)
			{
				data.m_positions[i] = base.transform.TransformPoint(m_data.m_positions[i]);
			}
		}
		else
		{
			data.m_bezierPositions = m_data.m_bezierPositions;
		}
		data.m_time = m_data.m_time;
		data.m_isFinishedStop = m_data.m_isFinishedStop;
		data.m_finishedRangePetState = m_data.m_finishedRangePetState;
		data.m_isLookAtNextPoint = m_data.m_isLookAtNextPoint;
		data.m_lookAtSpeed = m_data.m_lookAtSpeed;
		PetController petController = GameController.Instance.m_petController;
		if (petController != null && petController.m_petBase != null)
		{
			petController.m_petBase.TriggerPathToMove(data);
		}
	}

	public override void SetDefaultValue(object[] objs)
	{
		m_data = (PathToMoveByPetTriggerData)objs[0];
	}

	public override void Read(string info)
	{
		m_data = JsonUtility.FromJson<PathToMoveByPetTriggerData>(info);
	}

	public override string Write()
	{
		Vector3[] array = new Vector3[m_data.m_positions.Length];
		for (int i = 0; i < m_data.m_positions.Length; i++)
		{
			array[i] = base.transform.TransformPoint(m_data.m_positions[i]);
		}
		Vector3[] pathByPositions = Bezier.GetPathByPositions(array, 10);
		if (pathByPositions.Length < 500)
		{
			m_data.m_bezierPositions = pathByPositions;
		}
		return JsonUtility.ToJson(m_data);
	}

	public override void ReadBytes(byte[] bytes)
	{
		m_data = StructTranslatorUtility.ToStructure<PathToMoveByPetTriggerData>(bytes);
	}

	public override byte[] WriteBytes()
	{
		Vector3[] array = new Vector3[m_data.m_positions.Length];
		for (int i = 0; i < m_data.m_positions.Length; i++)
		{
			array[i] = base.transform.TransformPoint(m_data.m_positions[i]);
		}
		Vector3[] pathByPositions = Bezier.GetPathByPositions(array, 10);
		if (pathByPositions.Length < 500)
		{
			m_data.m_bezierPositions = pathByPositions;
		}
		return StructTranslatorUtility.ToByteArray(m_data);
	}

	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
	}
}
