using System;
using UnityEngine;

[Serializable]
public struct EasingVector3ByDistanceData
{
	public Vector3 m_from;

	public Vector3 m_to;

	public float m_startDistance;

	public float m_endDistance;

	public EaseType m_easeType;

	public EaseProcess m_easeProcess;

	public int m_pingPongCount;

	public static EasingVector3ByDistanceData DefaultValue
	{
		get
		{
			EasingVector3ByDistanceData result = default(EasingVector3ByDistanceData);
			result.m_from = new Vector3(0f, 0f, -10f);
			result.m_to = new Vector3(0f, 0f, 10f);
			result.m_startDistance = -10f;
			result.m_endDistance = 10f;
			result.m_easeType = EaseType.Linear;
			result.m_easeProcess = EaseProcess.Once;
			result.m_pingPongCount = 1;
			return result;
		}
	}
}
