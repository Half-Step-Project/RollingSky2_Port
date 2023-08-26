using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelEnterDebugSeriesItem : UILoopItem
{
	public Text m_title;

	public GameObject m_levelEnterObj;

	public GameObject m_levelContainer;

	private int m_levelSeriesId = -1;

	private List<LevelEnterDebugItemController> m_levelEnterList = new List<LevelEnterDebugItemController>();

	public override void Data(object data)
	{
		m_levelSeriesId = (int)data;
		m_title.text = string.Format("Series_{0}", m_levelSeriesId);
		GameObject gameObject = null;
		LevelEnterDebugItemController levelEnterDebugItemController = null;
		m_levelEnterList.Clear();
		int i = 0;
		for (int childCount = m_levelContainer.transform.childCount; i < childCount; i++)
		{
			Object.Destroy(m_levelContainer.transform.GetChild(i).gameObject);
		}
		List<int> allLevelsBySeriesId = MonoSingleton<GameTools>.Instacne.GetAllLevelsBySeriesId(m_levelSeriesId);
		for (int j = 0; j < allLevelsBySeriesId.Count; j++)
		{
			gameObject = Object.Instantiate(m_levelEnterObj.gameObject);
			levelEnterDebugItemController = gameObject.GetComponent<LevelEnterDebugItemController>();
			gameObject.SetActive(true);
			m_levelEnterList.Add(levelEnterDebugItemController);
			gameObject.transform.SetParent(m_levelContainer.transform);
			gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, 0f);
			gameObject.transform.localScale = Vector3.one;
			levelEnterDebugItemController.Init();
			levelEnterDebugItemController.SetData(allLevelsBySeriesId[j]);
		}
	}

	public override object GetData()
	{
		return null;
	}

	public override void SetSelected(bool selected)
	{
	}
}
