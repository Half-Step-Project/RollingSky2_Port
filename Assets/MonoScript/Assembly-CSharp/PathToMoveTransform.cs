using System;
using UnityEngine;

[Serializable]
public class PathToMoveTransform
{
	public Vector3 m_position = Vector3.zero;

	public Quaternion m_rotation = Quaternion.identity;

	public Vector3 m_size = Vector3.one;

	public PathToMoveTransform Copy()
	{
		return new PathToMoveTransform
		{
			m_position = m_position,
			m_rotation = m_rotation,
			m_size = m_size
		};
	}
}
