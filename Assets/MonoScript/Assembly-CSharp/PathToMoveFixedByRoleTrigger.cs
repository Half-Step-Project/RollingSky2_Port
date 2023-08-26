using System;
using Foundation;
using UnityEngine;
using User.TileMap;

using Grid = User.TileMap.Grid;

public class PathToMoveFixedByRoleTrigger : BaseTriggerBox, IPathFixRebirth
{
	[Serializable]
	public struct PathToMoveFixedByRoleTriggerData : IReadWriteBytes
	{
		public PathToMoveTransform[] m_transfroms;

		public PathToMoveTransform[] m_bezierTransfroms;

		public bool m_isFinishedStop;

		public float m_rotationSpeed;

		public int m_smooth;

		public PathToMoveFixedByRoleTriggerData Copy()
		{
			PathToMoveFixedByRoleTriggerData result = default(PathToMoveFixedByRoleTriggerData);
			result.m_transfroms = new PathToMoveTransform[m_transfroms.Length];
			for (int i = 0; i < m_transfroms.Length; i++)
			{
				if (m_transfroms[i] != null)
				{
					result.m_transfroms[i] = m_transfroms[i].Copy();
				}
			}
			if (m_bezierTransfroms != null && m_bezierTransfroms.Length != 0)
			{
				result.m_bezierTransfroms = new PathToMoveTransform[m_bezierTransfroms.Length];
				for (int j = 0; j < m_bezierTransfroms.Length; j++)
				{
					if (m_bezierTransfroms[j] != null)
					{
						result.m_bezierTransfroms[j] = m_bezierTransfroms[j].Copy();
					}
				}
			}
			result.m_isFinishedStop = m_isFinishedStop;
			result.m_rotationSpeed = Mathf.Clamp(m_rotationSpeed, 5f, 30f);
			result.m_smooth = Mathf.Max(10, m_smooth);
			return result;
		}

		public void ReadBytes(byte[] bytes)
		{
			PathToMoveFixedByRoleTriggerData pathToMoveFixedByRoleTriggerData = (this = JsonUtility.FromJson<PathToMoveFixedByRoleTriggerData>(bytes.GetString()));
		}

		public byte[] WriteBytes()
		{
			return JsonUtility.ToJson(this).GetBytes();
		}
	}

	public PathToMoveFixedByRoleTriggerData m_data;

	public override bool IfRebirthRecord
	{
		get
		{
			return false;
		}
	}

	public override void TriggerEnter(BaseRole ball)
	{
		PathToMoveFixedByRoleTriggerData pathData = GetPathData();
		ball.CallChangeToPathToMoveFixed(pathData, m_uuId);
	}

	public override void SetDefaultValue(object[] objs)
	{
		m_data = (PathToMoveFixedByRoleTriggerData)objs[0];
	}

	public override void Read(string info)
	{
		m_data = JsonUtility.FromJson<PathToMoveFixedByRoleTriggerData>(info);
	}

	public override string Write()
	{
		if (m_data.m_smooth <= 0)
		{
			Debug.LogError("PathToMoveFixedByRoleTrigger.smooth <=0");
		}
		Grid componentInParent = base.transform.GetComponentInParent<Grid>();
		if (componentInParent != null)
		{
			PathToMoveTransform[] array = new PathToMoveTransform[m_data.m_transfroms.Length];
			for (int i = 0; i < array.Length; i++)
			{
				if (m_data.m_transfroms[i] != null)
				{
					array[i] = new PathToMoveTransform();
					Vector3 position = base.transform.TransformPoint(m_data.m_transfroms[i].m_position);
					array[i].m_position = componentInParent.transform.InverseTransformPoint(position);
					array[i].m_rotation = m_data.m_transfroms[i].m_rotation;
					array[i].m_size = m_data.m_transfroms[i].m_size;
				}
			}
			m_data.m_bezierTransfroms = ThreeBezier.GetPathByPathToMoveTransform(array, m_data.m_smooth);
		}
		return JsonUtility.ToJson(m_data);
	}

	public override void ReadBytes(byte[] bytes)
	{
		m_data = StructTranslatorUtility.ToStructure<PathToMoveFixedByRoleTriggerData>(bytes);
	}

	public override byte[] WriteBytes()
	{
		if (m_data.m_smooth <= 0)
		{
			Debug.LogError("PathToMoveFixedByRoleTrigger.smooth <=0");
		}
		Grid componentInParent = base.transform.GetComponentInParent<Grid>();
		if (componentInParent != null)
		{
			PathToMoveTransform[] array = new PathToMoveTransform[m_data.m_transfroms.Length];
			for (int i = 0; i < array.Length; i++)
			{
				if (m_data.m_transfroms[i] != null)
				{
					array[i] = new PathToMoveTransform();
					Vector3 position = base.transform.TransformPoint(m_data.m_transfroms[i].m_position);
					array[i].m_position = componentInParent.transform.InverseTransformPoint(position);
					array[i].m_rotation = m_data.m_transfroms[i].m_rotation;
					array[i].m_size = m_data.m_transfroms[i].m_size;
				}
			}
			m_data.m_bezierTransfroms = ThreeBezier.GetPathByPathToMoveTransform(array, m_data.m_smooth);
		}
		return StructTranslatorUtility.ToByteArray(m_data);
	}

	public override void OnDrawGizmos()
	{
		ThreeBezier.DrawGizmos(base.gameObject, m_data.m_transfroms, m_data.m_smooth, true);
		base.OnDrawGizmos();
	}

	public PathToMoveFixedByRoleTriggerData GetPathData()
	{
		PathToMoveFixedByRoleTriggerData result = m_data.Copy();
		if (m_data.m_bezierTransfroms == null || m_data.m_bezierTransfroms.Length == 0)
		{
			for (int i = 0; i < result.m_transfroms.Length; i++)
			{
				if (result.m_transfroms[i] != null)
				{
					result.m_transfroms[i] = new PathToMoveTransform();
					result.m_transfroms[i].m_position = base.transform.TransformPoint(m_data.m_transfroms[i].m_position);
					result.m_transfroms[i].m_rotation = m_data.m_transfroms[i].m_rotation;
					result.m_transfroms[i].m_size = m_data.m_transfroms[i].m_size;
				}
			}
		}
		return result;
	}
}
