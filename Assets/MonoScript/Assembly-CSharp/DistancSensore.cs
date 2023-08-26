public class DistancSensore
{
	public enum DistancSensoreType
	{
		Once,
		Continued
	}

	private float m_startDistance;

	private float m_endDistance;

	private DistancSensoreType m_type;

	public bool m_isTriggerStay;

	public DistancSensore(float distance, DistancSensoreType type = DistancSensoreType.Once)
	{
		m_startDistance = distance;
		m_endDistance = float.MaxValue;
		m_isTriggerStay = false;
		m_type = type;
	}

	public DistancSensore(float startDistance, float endDistance, DistancSensoreType type = DistancSensoreType.Once)
	{
		m_startDistance = startDistance;
		m_endDistance = endDistance;
		m_isTriggerStay = false;
		m_type = type;
	}

	public bool Run(float currentDistance)
	{
		bool result = false;
		switch (m_type)
		{
		case DistancSensoreType.Once:
			result = OnOnce(currentDistance);
			break;
		case DistancSensoreType.Continued:
			result = OnContinued(currentDistance);
			break;
		}
		return result;
	}

	private bool OnOnce(float currentDistance)
	{
		if (currentDistance >= m_startDistance && !m_isTriggerStay && currentDistance <= m_endDistance)
		{
			m_isTriggerStay = true;
			return true;
		}
		return false;
	}

	private bool OnContinued(float currentDistance)
	{
		m_isTriggerStay = currentDistance >= m_startDistance && currentDistance <= m_endDistance;
		return m_isTriggerStay;
	}

	public bool IsBeyondFarthestDistance(float currentDistance)
	{
		return currentDistance > m_endDistance;
	}
}
