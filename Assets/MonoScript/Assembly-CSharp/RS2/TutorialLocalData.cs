using System;
using System.Collections.Generic;
using UnityEngine;

namespace RS2
{
	[Serializable]
	public class TutorialLocalData
	{
		public List<TutorialStageData> m_stageData = new List<TutorialStageData>();

		public bool IsTutorialStageFinish(int stageId)
		{
			TutorialStageData tutorialStageData = m_stageData.Find((TutorialStageData x) => x.m_id == stageId);
			if (tutorialStageData != null)
			{
				return tutorialStageData.m_flag == 1;
			}
			return false;
		}

		public void SetTutorialStageFinish(int stageId)
		{
			TutorialStageData tutorialStageData = m_stageData.Find((TutorialStageData x) => x.m_id == stageId);
			if (tutorialStageData != null)
			{
				tutorialStageData.m_flag = 1;
			}
			else
			{
				tutorialStageData = new TutorialStageData();
				tutorialStageData.m_id = stageId;
				tutorialStageData.m_flag = 1;
				m_stageData.Add(tutorialStageData);
			}
			SaveToLocal();
		}

		public void Init()
		{
			string config = EncodeConfig.getConfig(PlayerLocalDatakey.PLAYERTUTORIALGLOABLEDATA);
			if (config.Length > 0)
			{
				JsonUtility.FromJsonOverwrite(config, this);
			}
		}

		public void SaveToLocal()
		{
			string values = JsonUtility.ToJson(this);
			EncodeConfig.setConfig(PlayerLocalDatakey.PLAYERTUTORIALGLOABLEDATA, values);
		}
	}
}
