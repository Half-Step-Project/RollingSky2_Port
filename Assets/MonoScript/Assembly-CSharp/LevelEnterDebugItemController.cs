using System;
using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.UI;

public class LevelEnterDebugItemController : MonoBehaviour
{
	public GameObject m_LevelItem;

	public Text m_levelName;

	private int m_levelId;

	public void SetData(int levelId)
	{
		m_levelId = levelId;
		string levelNameById = MonoSingleton<GameTools>.Instacne.GetLevelNameById(m_levelId);
		m_levelName.text = string.Format("{0}({1})", levelNameById, m_levelId);
	}

	private void AddEventListener()
	{
		EventTriggerListener.Get(m_LevelItem).onClick = OnClickCloseButton;
	}

	private void OnClickCloseButton(GameObject go)
	{
		EnterLevelById(m_levelId);
	}

	private void EnterLevelById(int levelId)
	{
		Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).CurLevelId = levelId;
		MenuProcedure obj = (MenuProcedure)Mod.Procedure.Current;
		int sceneIdByLevelId = MonoSingleton<GameTools>.Instacne.GetSceneIdByLevelId(levelId);
		obj.WillToScene(sceneIdByLevelId);
		Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).CurrentEnterLevelType = LevelEnterType.MENU;
	}

	private void RemoveEventListener()
	{
		EventTriggerListener eventTriggerListener = EventTriggerListener.Get(m_LevelItem);
		eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(OnClickCloseButton));
	}

	public void Init()
	{
		AddEventListener();
	}

	public void Reset()
	{
		RemoveEventListener();
	}
}
