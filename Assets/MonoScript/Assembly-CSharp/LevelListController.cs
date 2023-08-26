using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;

public class LevelListController : MonoBehaviour
{
	public List<LevelItem> m_levelDic = new List<LevelItem>();

	public GameObject cup0Tips;

	public GameObject cup1Tips;

	public RectTransform bg;

	private int m_levelSeriesId = -1;

	private List<LevelMetaTableData> m_levelMetaDataList = new List<LevelMetaTableData>();

	private LevelMetaTableData m_currentMetaData;

	private Vector3 m_currentBackPos = Vector3.zero;

	private LevelItem currentLevel;

	private int m_lastLevelId = -1;

	private PlayerLocalLevelData m_CurrentlevelLogicData;

	private int m_totalFrame;

	private GameObject nsIconR;

	private GameObject nsIconL;

	public void Init(LevelMetaTableData metaData)
	{
		m_currentMetaData = metaData;
		m_levelSeriesId = m_currentMetaData.SeriesId;
		if (m_levelSeriesId <= 0)
		{
			return;
		}
		if (m_levelSeriesId == 7)
		{
			base.gameObject.SetActive(false);
			return;
		}
		base.gameObject.SetActive(true);
		InitMetaData();
		SetLevelData();
		AddEventListener();
		if (m_currentMetaData.LevelId > 0)
		{
			SwitchLevel(m_currentMetaData.LevelId);
		}
		else
		{
			SwitchLevel(FindDefaultLevelId());
		}
	}

	public void OnShow()
	{
		foreach (LevelItem item in m_levelDic)
		{
			item.OnShow();
		}
		int num = 0;
		foreach (LevelItem item2 in m_levelDic)
		{
			if (!item2.gameObject.activeSelf)
			{
				num++;
			}
		}
		bg.sizeDelta = new Vector2(630 - 210 * num, bg.sizeDelta.y);
		if (nsIconR == null)
		{
			nsIconR = bg.transform.parent.Find("nsIconR").gameObject;
		}
		if (nsIconL == null)
		{
			nsIconL = bg.transform.parent.Find("nsIconL").gameObject;
		}
		nsIconR.SetActive(false);
		nsIconL.SetActive(false);
	}

	public void OnHide()
	{
		foreach (LevelItem item in m_levelDic)
		{
			item.OnHide();
		}
	}

	private void AddEventListener()
	{
		Mod.Event.Subscribe(EventArgs<LevelUnLockEventArgs>.EventId, OnLevelUnLockChange);
	}

	private void RemoveEventListener()
	{
		Mod.Event.Unsubscribe(EventArgs<LevelUnLockEventArgs>.EventId, OnLevelUnLockChange);
	}

	private void InitMetaData()
	{
		m_levelMetaDataList = PlayerDataModule.Instance.GloableLevelLableData.FindAll((LevelMetaTableData x) => x.SeriesId == m_levelSeriesId);
	}

	public void SwitchLevel(int currentLevelId)
	{
		if (m_lastLevelId != currentLevelId)
		{
			m_lastLevelId = currentLevelId;
			ShowContentByLevelId(currentLevelId);
		}
	}

	public void Release()
	{
		RemoveEventListener();
		m_levelSeriesId = -1;
		List<LevelItem>.Enumerator enumerator = m_levelDic.GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.Release();
		}
	}

	private void SetLevelData()
	{
		List<LevelMetaTableData> list = m_levelMetaDataList.FindAll((LevelMetaTableData x) => x.IsShowInSelect);
		LevelItem levelItem = null;
		LevelMetaTableData levelMetaTableData = null;
		for (int i = 0; i < list.Count && i < m_levelDic.Count; i++)
		{
			levelItem = m_levelDic[i];
			levelMetaTableData = list[i];
			levelItem.gameObject.name = levelMetaTableData.LevelId.ToString();
			levelItem.SetData(levelMetaTableData, this);
			if (MonoSingleton<GameTools>.Instacne.IsPreparing(levelMetaTableData.LevelId))
			{
				levelItem.gameObject.SetActive(false);
			}
			else
			{
				levelItem.gameObject.SetActive(true);
			}
		}
	}

	private void ShowContentByLevelId(int levelId)
	{
		List<LevelItem>.Enumerator enumerator = m_levelDic.GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (enumerator.Current.MetaData.LevelId == levelId)
			{
				currentLevel = enumerator.Current;
			}
			else
			{
				enumerator.Current.SetItemSelected(false);
			}
		}
		if (currentLevel != null)
		{
			currentLevel.SetItemSelected(true);
		}
	}

	private int FindDefaultLevelId()
	{
		int num = -1;
		int num2 = -1;
		List<LevelMetaTableData> list = m_levelMetaDataList.FindAll((LevelMetaTableData x) => x.IsShowInSelect);
		for (int i = 0; i < list.Count; i++)
		{
			if (!MonoSingleton<GameTools>.Instacne.IsPreparing(list[i].LevelId))
			{
				num2 = list[i].LevelId;
				if (PlayerDataModule.Instance.GetLevelMaxProgress(list[i].LevelId) < 100)
				{
					num = list[i].LevelId;
					break;
				}
			}
		}
		if (num == -1)
		{
			num = num2;
		}
		return num;
	}

	private void OnLevelUnLockChange(object sender, EventArgs e)
	{
		bool flag = e is LevelUnLockEventArgs;
	}

	public bool CompatibleOldVersionUnLock()
	{
		foreach (LevelItem item in m_levelDic)
		{
			if (item.CompatibleOldVersionUnLock())
			{
				return true;
			}
		}
		return false;
	}

	public void EnterLevel()
	{
		currentLevel.EnterLevelHandler();
	}

	public void OnClickCup0()
	{
		cup0Tips.SetActive(true);
		currentLevel.ClickTips0();
	}

	public void OnClickCup1()
	{
		cup1Tips.SetActive(true);
		currentLevel.ClickTips1();
	}
}
