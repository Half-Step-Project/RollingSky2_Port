using UnityEngine.Events;

public class LevelLockData
{
	public enum EnterLevelType
	{
		NONE,
		TRY,
		LOCKED
	}

	public int currentLevelId = -1;

	public UnityAction<EnterLevelType> callBack;

	public bool m_isHadDowloaded;

	public void Reset()
	{
		currentLevelId = -1;
		callBack = null;
		m_isHadDowloaded = false;
	}
}
