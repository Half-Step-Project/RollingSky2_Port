using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelItemTypeController : MonoBehaviour
{
	public GameObject m_normalStyle;

	public GameObject m_selectStyle;

	public Text m_content;

	private LevelDifficulty m_type = LevelDifficulty.NONE;

	private UnityAction<LevelDifficulty, int> m_clickAction;

	public LevelDifficulty Type
	{
		get
		{
			return m_type;
		}
	}

	public void SetData(LevelDifficulty type, UnityAction<LevelDifficulty, int> clickAction)
	{
		m_type = type;
		m_clickAction = clickAction;
		AddEventController();
	}

	private void AddEventController()
	{
		EventTriggerListener.Get(base.gameObject).onClick = ClickHandle;
	}

	private void ClickHandle(GameObject go)
	{
		SetIsSlected(true);
		if (m_clickAction != null)
		{
			m_clickAction(Type, -1);
		}
	}

	private void ShowNormal()
	{
		m_normalStyle.SetActive(true);
		m_selectStyle.SetActive(false);
	}

	private void ShowSelected()
	{
		m_normalStyle.SetActive(false);
		m_selectStyle.SetActive(true);
	}

	public void SetIsSlected(bool isSelect)
	{
		if (isSelect)
		{
			ShowSelected();
		}
		else
		{
			ShowNormal();
		}
	}

	public void Reset()
	{
		m_type = LevelDifficulty.NONE;
	}
}
