using UnityEngine;

public class EasingQuaternionByDistance
{
	public EasingVector3ByDistanceData m_data;

	private float m_currentDistance;

	public bool IsPlaying
	{
		get
		{
			if (m_currentDistance >= m_data.m_startDistance)
			{
				return m_currentDistance <= m_data.m_endDistance;
			}
			return false;
		}
	}

	public EasingQuaternionByDistance(EasingVector3ByDistanceData data)
	{
		m_data = data;
		if (m_data.m_pingPongCount <= 0)
		{
			m_data.m_pingPongCount = 1;
		}
		m_currentDistance = 0f;
	}

	public Quaternion Run(float currentDistance)
	{
		m_currentDistance = currentDistance;
		return Easing.EasingQuaternion(Progress(currentDistance), m_data.m_from, m_data.m_to, 1f, m_data.m_easeType);
	}

	private float Progress(float currentDistance)
	{
		float result = 0f;
		switch (m_data.m_easeProcess)
		{
		case EaseProcess.Once:
			result = OnOnce(currentDistance, m_data.m_startDistance, m_data.m_endDistance);
			break;
		case EaseProcess.PingPong:
			result = OnPingPong(currentDistance, m_data.m_startDistance, m_data.m_endDistance, m_data.m_pingPongCount);
			break;
		}
		return result;
	}

	private float OnOnce(float currentDistance, float startDistance, float endDistance)
	{
		float result = 0f;
		if (currentDistance < startDistance)
		{
			result = 0f;
		}
		else if (currentDistance >= startDistance && currentDistance <= endDistance)
		{
			result = (m_data.m_startDistance - currentDistance) / (m_data.m_startDistance - m_data.m_endDistance);
		}
		else if (currentDistance > endDistance)
		{
			result = 1f;
		}
		return result;
	}

	private float OnPingPong(float currentDistance, float startDistance, float endDistance, int pingPongCount = 1)
	{
		float result = 0f;
		if (currentDistance < startDistance || currentDistance > endDistance)
		{
			result = 0f;
		}
		else if (currentDistance >= startDistance && currentDistance <= endDistance)
		{
			float num = (m_data.m_endDistance - m_data.m_startDistance) / (float)(2 * pingPongCount);
			float num2 = m_data.m_startDistance - currentDistance;
			float num3 = num2 % num;
			bool flag = num3 == 0f;
			int num4 = Mathf.FloorToInt(num2 / num);
			float num5 = num3 / num;
			result = ((!flag) ? ((num4 % 2 == 0) ? (1f + num5) : (0f - num5)) : ((float)((num4 % 2 != 0) ? 1 : 0)));
		}
		return result;
	}
}
