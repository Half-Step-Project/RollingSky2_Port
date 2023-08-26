using System;
using UnityEngine;

[Serializable]
public struct PathToMoveData
{
	public Vector3[] m_positions;

	public int m_smooth;

	public Vector3[] m_bezierPositions;

	public static PathToMoveData DefaultValue
	{
		get
		{
			PathToMoveData result = default(PathToMoveData);
			result.m_positions = new Vector3[4]
			{
				new Vector3(0f, 0f, 0f),
				new Vector3(0f, 1f, 0.3f),
				new Vector3(0f, 1f, 0.7f),
				new Vector3(0f, 0f, 1f)
			};
			result.m_smooth = 20;
			return result;
		}
	}

	public void RefreshBezierPositions(GameObject target, bool needRefresh = false)
	{
		if (m_bezierPositions == null || m_bezierPositions.Length == 0 || needRefresh)
		{
			Vector3[] array = new Vector3[m_positions.Length];
			for (int i = 0; i < m_positions.Length; i++)
			{
				array[i] = target.transform.TransformPoint(m_positions[i]);
			}
			m_bezierPositions = ThreeBezier.GetPathByPositions(array, m_smooth);
		}
	}
}
