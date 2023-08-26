using System;

[Serializable]
public struct AnimPlayOnceDistanceData
{
	public string m_animName;

	public float m_distance;

	public static AnimPlayOnceDistanceData DefaultValue
	{
		get
		{
			AnimPlayOnceDistanceData result = default(AnimPlayOnceDistanceData);
			result.m_animName = string.Empty;
			result.m_distance = -10f;
			return result;
		}
	}
}
