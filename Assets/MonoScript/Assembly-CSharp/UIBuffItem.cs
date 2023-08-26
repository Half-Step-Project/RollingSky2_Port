using UnityEngine;
using UnityEngine.UI;

public class UIBuffItem : MonoBehaviour
{
	public enum ItemState
	{
		Close,
		TimeLimit,
		Permanent
	}

	public GameObject m_closeObject;

	public GameObject m_permanentObject;

	public GameObject m_timeLimitObject;

	public Text m_timeText;

	public PlayerBufferData m_bufferData;

	public ItemState m_currentState;

	public void SwitchState(ItemState state, bool ignoreShowTime = false)
	{
		switch (state)
		{
		case ItemState.Close:
			if ((bool)m_permanentObject)
			{
				m_permanentObject.SetActive(false);
			}
			if ((bool)m_closeObject)
			{
				m_closeObject.SetActive(true);
			}
			if ((bool)m_timeText)
			{
				m_timeText.gameObject.SetActive(false);
			}
			if ((bool)m_timeLimitObject)
			{
				m_timeLimitObject.SetActive(false);
			}
			break;
		case ItemState.TimeLimit:
			if ((bool)m_timeText && !ignoreShowTime)
			{
				m_timeText.gameObject.SetActive(true);
			}
			if ((bool)m_timeLimitObject && !ignoreShowTime)
			{
				m_timeLimitObject.SetActive(true);
			}
			if ((bool)m_permanentObject)
			{
				m_permanentObject.SetActive(false);
			}
			if ((bool)m_closeObject)
			{
				m_closeObject.SetActive(false);
			}
			break;
		case ItemState.Permanent:
			if ((bool)m_permanentObject)
			{
				m_permanentObject.SetActive(true);
			}
			if ((bool)m_closeObject)
			{
				m_closeObject.SetActive(false);
			}
			if ((bool)m_timeText)
			{
				m_timeText.gameObject.SetActive(false);
			}
			if ((bool)m_timeLimitObject)
			{
				m_timeLimitObject.SetActive(false);
			}
			break;
		}
		m_currentState = state;
	}

	public void SetTime(long time)
	{
		if (m_timeText != null)
		{
			m_timeText.text = MonoSingleton<GameTools>.Instacne.CommonTimeFormatForOne(time / 1000);
		}
	}
}
