using System;

[Serializable]
public struct AnimPlayContinueDistanceData
{
	public string m_animName;

	public float m_start;

	public float m_end;

	public static AnimPlayContinueDistanceData DefaultValue
	{
		get
		{
			AnimPlayContinueDistanceData result = default(AnimPlayContinueDistanceData);
			result.m_animName = string.Empty;
			result.m_start = -10f;
			result.m_end = 10f;
			return result;
		}
	}
}
