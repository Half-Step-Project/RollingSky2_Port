using System.Collections.Generic;
using UnityEngine;

namespace RS2
{
	public class TutorialManager
	{
		private TutorialLocalData m_tutorialData;

		private List<BaseTutorialStage> m_stageList = new List<BaseTutorialStage>();

		private static TutorialManager m_instance;

		private BaseTutorialStage m_currentStage;

		private List<Transform> m_UIParents = new List<Transform>();

		private string[] uiParentPath = new string[4] { "Framework/UI/UI Group - First", "Framework/UI/UI Group - Second", "Framework/UI/UI Group - Third", "Framework/UI/UI Group - Forth" };

		public static TutorialManager Instance
		{
			get
			{
				if (m_instance == null)
				{
					m_instance = new TutorialManager();
				}
				return m_instance;
			}
		}

		private TutorialManager()
		{
		}

		public void Init()
		{
			for (int i = 0; i < 4; i++)
			{
				Transform transform = GameObject.Find(uiParentPath[i]).transform;
				m_UIParents.Add(transform);
			}
			m_tutorialData = new TutorialLocalData();
			m_tutorialData.Init();
			bool num = m_tutorialData.IsTutorialStageFinish(TutorialStageId.STAGE_TUTORIAL_LEVEL);
			int levelMaxProgress = PlayerDataModule.Instance.GetLevelMaxProgress(10000);
			if (!num)
			{
				int levelMaxProgress2 = PlayerDataModule.Instance.GetLevelMaxProgress(GameCommon.FIRST_LEVEL);
				if (levelMaxProgress < 100 && levelMaxProgress2 < 1)
				{
					BaseTutorialStage baseTutorialStage = new TutorialLevelStage();
					baseTutorialStage.Init(TutorialStageId.STAGE_TUTORIAL_LEVEL);
					m_stageList.Add(baseTutorialStage);
				}
				else
				{
					m_tutorialData.SetTutorialStageFinish(TutorialStageId.STAGE_TUTORIAL_LEVEL);
				}
			}
			m_tutorialData.SetTutorialStageFinish(TutorialStageId.STAGE_FIRST_LEVEL);
			m_tutorialData.SetTutorialStageFinish(TutorialStageId.STAGE_FIRST_LEVEL_BUFF);
			m_tutorialData.SetTutorialStageFinish(TutorialStageId.STAGE_SECOND_LEVEL_GUIDLINE);
			m_tutorialData.SetTutorialStageFinish(TutorialStageId.STAGE_BUFFER);
			m_tutorialData.SetTutorialStageFinish(TutorialStageId.STAGE_LEVEL_DIFFICULT);
			int playerLevel = PlayerDataModule.Instance.GetPlayerLevel();
			int num2;
			bool flag;
			if (PlayerDataModule.Instance.GetPlayerStarLevel() >= 1)
			{
				num2 = 1;
			}
			else
				flag = playerLevel >= 2;
			if (m_stageList.Count > 0)
			{
				m_currentStage = m_stageList[0];
			}
		}

		public void SwitchStage(int currentStageId)
		{
			if (GetCurrentStageId() != currentStageId)
			{
				m_currentStage = m_stageList.Find((BaseTutorialStage x) => x.StageId == currentStageId);
				StartCurrentStage();
			}
		}

		public BaseTutorialStage GetStage(int stageId)
		{
			return m_stageList.Find((BaseTutorialStage x) => x.StageId == stageId);
		}

		public BaseTutorialStage GetCurrentStage()
		{
			return m_currentStage;
		}

		public void StartCurrentStage()
		{
			if (m_currentStage != null)
			{
				m_currentStage.Start();
			}
		}

		public void EndCurrentStage()
		{
			if (m_currentStage != null)
			{
				m_currentStage.End();
			}
		}

		public int GetCurrentStageId()
		{
			if (m_currentStage != null)
			{
				return m_currentStage.StageId;
			}
			return -1;
		}

		public bool IsGoingTutorial()
		{
			if (m_currentStage != null)
			{
				return m_currentStage.IsTutoiralGoing;
			}
			return false;
		}

		public void SetTutorialStageFlag(int stageId)
		{
			m_tutorialData.SetTutorialStageFinish(stageId);
		}

		public bool IsTutorialCustPower()
		{
			int currentStageId = GetCurrentStageId();
			if (currentStageId == TutorialStageId.STAGE_TUTORIAL_LEVEL || currentStageId == TutorialStageId.STAGE_FIRST_LEVEL)
			{
				return false;
			}
			return true;
		}

		public bool IsTutorialStageFinish(int stageId)
		{
			return m_tutorialData.IsTutorialStageFinish(stageId);
		}

		public void ShowUIGroup(params int[] indexs)
		{
			for (int i = 0; i < indexs.Length; i++)
			{
				if (indexs[i] < m_UIParents.Count)
				{
					m_UIParents[indexs[i]].gameObject.SetActive(true);
				}
			}
		}

		public void HideUIGroup(params int[] indexs)
		{
			for (int i = 0; i < indexs.Length; i++)
			{
				if (indexs[i] < m_UIParents.Count)
				{
					m_UIParents[indexs[i]].gameObject.SetActive(false);
				}
			}
		}

		public bool IsForceTutorialEnd()
		{
			return IsTutorialStageFinish(TutorialStageId.STAGE_TAPTAP);
		}
	}
}
