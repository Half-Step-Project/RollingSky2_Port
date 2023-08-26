using System;
using UnityEngine;

[Serializable]
public class FollowData
{
	public float m_followSpeed;

	public Vector3 m_cameraPoint;

	public FollowData Clone()
	{
		return new FollowData
		{
			m_followSpeed = m_followSpeed,
			m_cameraPoint = m_cameraPoint
		};
	}
}
