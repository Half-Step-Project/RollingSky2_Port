using System;
using System.IO;
using Foundation;
using UnityEngine;
using User.TileMap;

using Grid = User.TileMap.Grid;

public class PathToMoveByRoleTrigger : BaseTriggerBox, IPathRebirth
{
	[Serializable]
	public struct PathToMoveByRoleTriggerData : IReadWriteBytes
	{
		public Vector3[] m_positions;

		public bool m_isFinishedStop;

		public int m_smooth;

		public bool m_ifHaveEffect;

		public Vector3[] m_bezierPositions;

		public PathToMoveByRoleTriggerData Copy()
		{
			PathToMoveByRoleTriggerData result = default(PathToMoveByRoleTriggerData);
			result.m_positions = new Vector3[m_positions.Length];
			for (int i = 0; i < result.m_positions.Length; i++)
			{
				result.m_positions[i] = m_positions[i];
			}
			if (m_bezierPositions != null && m_bezierPositions.Length != 0)
			{
				result.m_bezierPositions = new Vector3[m_bezierPositions.Length];
				for (int j = 0; j < m_bezierPositions.Length; j++)
				{
					result.m_bezierPositions[j] = m_bezierPositions[j];
				}
			}
			result.m_isFinishedStop = m_isFinishedStop;
			result.m_smooth = Mathf.Max(10, m_smooth);
			result.m_ifHaveEffect = m_ifHaveEffect;
			return result;
		}

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			ReadBytes(bytes, ref startIndex);
		}

		public void ReadBytes(byte[] bytes, ref int startIndex)
		{
			m_positions = bytes.GetVector3Array(ref startIndex);
			m_isFinishedStop = bytes.GetBoolean(ref startIndex);
			m_smooth = bytes.GetInt32(ref startIndex);
			m_ifHaveEffect = bytes.GetBoolean(ref startIndex);
			m_bezierPositions = bytes.GetVector3Array(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(m_positions.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_isFinishedStop.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_smooth.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_ifHaveEffect.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_bezierPositions.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	public PathToMoveByRoleTriggerData m_data;

	public override bool IfRebirthRecord
	{
		get
		{
			return false;
		}
	}

	public override void TriggerEnter(BaseRole ball)
	{
		PathToMoveByRoleTriggerData pathData = GetPathData();
		ball.CallChangeToPathToMove(pathData, m_uuId);
	}

	public override void SetDefaultValue(object[] objs)
	{
		m_data = (PathToMoveByRoleTriggerData)objs[0];
	}

	public override void Read(string info)
	{
		m_data = JsonUtility.FromJson<PathToMoveByRoleTriggerData>(info);
	}

	public override string Write()
	{
		if (m_data.m_smooth <= 0)
		{
			Debug.LogError("PathToMoveByRoleTriggerData.smooth <=0");
		}
		Grid componentInParent = base.transform.GetComponentInParent<Grid>();
		if (componentInParent != null)
		{
			Vector3[] array = new Vector3[m_data.m_positions.Length];
			for (int i = 0; i < m_data.m_positions.Length; i++)
			{
				Vector3 position = base.transform.TransformPoint(m_data.m_positions[i]);
				array[i] = componentInParent.transform.InverseTransformPoint(position);
			}
			Vector3[] pathByPositions = ThreeBezier.GetPathByPositions(array, m_data.m_smooth);
			if (pathByPositions.Length < 500)
			{
				m_data.m_bezierPositions = pathByPositions;
			}
		}
		return JsonUtility.ToJson(m_data);
	}

	public override void ReadBytes(byte[] bytes)
	{
		m_data = StructTranslatorUtility.ToStructure<PathToMoveByRoleTriggerData>(bytes);
	}

	public override byte[] WriteBytes()
	{
		if (m_data.m_smooth <= 0)
		{
			Debug.LogError("PathToMoveByRoleTriggerData.smooth <=0");
		}
		Grid componentInParent = base.transform.GetComponentInParent<Grid>();
		if (componentInParent != null)
		{
			Vector3[] array = new Vector3[m_data.m_positions.Length];
			for (int i = 0; i < m_data.m_positions.Length; i++)
			{
				Vector3 position = base.transform.TransformPoint(m_data.m_positions[i]);
				array[i] = componentInParent.transform.InverseTransformPoint(position);
			}
			Vector3[] pathByPositions = ThreeBezier.GetPathByPositions(array, m_data.m_smooth);
			if (pathByPositions.Length < 500)
			{
				m_data.m_bezierPositions = pathByPositions;
			}
		}
		return StructTranslatorUtility.ToByteArray(m_data);
	}

	public override void OnDrawGizmos()
	{
		ThreeBezier.DrawGizmos(base.gameObject, m_data.m_positions, m_data.m_smooth);
		base.OnDrawGizmos();
	}

	public PathToMoveByRoleTriggerData GetPathData()
	{
		PathToMoveByRoleTriggerData result = m_data.Copy();
		if (m_data.m_bezierPositions == null || m_data.m_bezierPositions.Length == 0)
		{
			for (int i = 0; i < result.m_positions.Length; i++)
			{
				result.m_positions[i] = base.transform.TransformPoint(result.m_positions[i]);
			}
		}
		return result;
	}
}
