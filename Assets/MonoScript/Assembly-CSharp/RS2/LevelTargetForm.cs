using System;
using System.Collections.Generic;
using Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace RS2
{
	public class LevelTargetForm : UGUIForm
	{
		private Image back;

		private Text titleTxt;

		private Text targetContentTxt;

		private UILoopList uiLoopList;

		private GameObject closeBtn;

		private Text refresTargetTxt;

		private GameObject refreshTargetBtn;

		private Camera mainCamera;

		private int levelId = -1;

		private RectTransform step_0;

		private RectTransform step_1;

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			Dictionary<string, GameObject> dictionary = ViewTools.CollectAllGameObjects(base.gameObject);
			back = dictionary["back"].GetComponent<Image>();
			titleTxt = dictionary["titleTxt"].GetComponent<Text>();
			targetContentTxt = dictionary["targetContentTxt"].GetComponent<Text>();
			refresTargetTxt = dictionary["refresTargetTxt"].GetComponent<Text>();
			refreshTargetBtn = dictionary["refreshTargetBtn"];
			step_0 = dictionary["step_0"].GetComponent<RectTransform>();
			step_1 = dictionary["step_1"].GetComponent<RectTransform>();
			closeBtn = dictionary["closeBtn"];
			closeBtn.SetActive(false);
			uiLoopList = dictionary["content"].GetComponent<UILoopList>();
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			AddEventListener();
			if (userData != null)
			{
				levelId = (int)userData;
			}
			else
			{
				levelId = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).CurLevelId;
			}
			SetData();
			ShowScenePostEffect();
			DealWithTutorial();
		}

		protected override void OnClose(object userData)
		{
			base.OnClose(userData);
			RemoveEventListener();
			CloseScenePostEffect();
			levelId = -1;
		}

		private void AddEventListener()
		{
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(closeBtn);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(CloseHandler));
			EventTriggerListener eventTriggerListener2 = EventTriggerListener.Get(refreshTargetBtn);
			eventTriggerListener2.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener2.onClick, new EventTriggerListener.VoidDelegate(RefreshHandler));
		}

		private void RemoveEventListener()
		{
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(closeBtn);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(CloseHandler));
			EventTriggerListener eventTriggerListener2 = EventTriggerListener.Get(refreshTargetBtn);
			eventTriggerListener2.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener2.onClick, new EventTriggerListener.VoidDelegate(RefreshHandler));
		}

		private void RefreshHandler(GameObject go)
		{
			MonoSingleton<GameTools>.Instacne.PlayerVideoAd(ADScene.LevelTargetRefresh, OnAddSuccess);
		}

		private void OnAddSuccess(ADScene adScene)
		{
			RefreshTarget();
			SetData();
		}

		private void RefreshTarget()
		{
			int level = 0;
			if (levelId == 3 || levelId == 4 || levelId == 5)
			{
				level = levelId;
			}
			InfocUtils.Report_rollingsky2_games_ads(4, 0, 1, level, 3, 0);
			Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).PlayerLevelTargetData.RefreshTargets();
		}

		private void CloseHandler(GameObject obj)
		{
			Mod.UI.CloseUIForm(UIFormId.LevelTargetForm);
		}

		private void SetData()
		{
			titleTxt.text = Mod.Localization.GetInfoById(10005);
			LevelGoalItemData levelGoalItemData = null;
			List<LevelGoalItemData> list = new List<LevelGoalItemData>();
			string[] array = null;
			LevelTarget_levelTargetTable levelTarget_levelTargetTable = null;
			PlayerDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule);
			DateTime now = DateTime.Now;
			targetContentTxt.text = Mod.Localization.GetInfoById(10009) + "<color=#8BDDAAFF>" + dataModule.PlayerLevelTargetData.hadFinishTargetNum + "/" + GameCommon.levelTargetMaxCanFinNum + "</color>";
			List<int> currentCanFinishTargets = dataModule.PlayerLevelTargetData.GetCurrentCanFinishTargets();
			if (currentCanFinishTargets.Count > 0)
			{
				int i = 0;
				for (int count = currentCanFinishTargets.Count; i < count; i++)
				{
					levelTarget_levelTargetTable = Mod.DataTable.Get<LevelTarget_levelTargetTable>()[currentCanFinishTargets[i]];
					if (levelTarget_levelTargetTable != null)
					{
						levelGoalItemData = new LevelGoalItemData();
						levelGoalItemData.targetDesc = string.Format(Mod.Localization.GetInfoById(levelTarget_levelTargetTable.Desc), levelTarget_levelTargetTable.TargetNum);
						levelGoalItemData.targetNum = levelTarget_levelTargetTable.TargetNum;
						levelGoalItemData.targetId = currentCanFinishTargets[i];
						levelGoalItemData.startLevels = levelTarget_levelTargetTable.StartLevel;
						levelGoalItemData.levelId = levelId;
						levelGoalItemData.awardList.Clear();
						array = levelTarget_levelTargetTable.AwardIds.Split('|');
						for (int j = 0; j < array.Length; j++)
						{
							levelGoalItemData.awardList.Add(int.Parse(array[j]));
						}
					}
					list.Add(levelGoalItemData);
				}
			}
			uiLoopList.Data(list);
		}

		private void ShowScenePostEffect()
		{
			if (mainCamera == null)
			{
				mainCamera = Camera.main;
			}
			CommandBufferBlur commandBufferBlur = mainCamera.GetComponent<CommandBufferBlur>();
			if (commandBufferBlur == null)
			{
				commandBufferBlur = mainCamera.gameObject.AddComponent<CommandBufferBlur>();
			}
			commandBufferBlur.enabled = true;
		}

		private void CloseScenePostEffect()
		{
			if (mainCamera == null)
			{
				mainCamera = Camera.main;
			}
			CommandBufferBlur component = mainCamera.GetComponent<CommandBufferBlur>();
			if (component != null)
			{
				component.enabled = false;
			}
		}

		private void DealWithTutorial()
		{
			if (!PlayerDataModule.Instance.PlayerLevelTargetData.IsFinishTutorial())
			{
				CommonTutorialData commonTutorialData = new CommonTutorialData(1);
				CommonTutorialStepData commonTutorialStepData = new CommonTutorialStepData();
				commonTutorialStepData.tutorialContent = "点击屏幕结束第一步";
				commonTutorialStepData.position = new Rect(step_0.anchoredPosition.x, step_0.anchoredPosition.y, step_0.sizeDelta.x, step_0.sizeDelta.y);
				commonTutorialStepData.changeRect = false;
				commonTutorialData.AddStep(commonTutorialStepData);
				commonTutorialStepData = new CommonTutorialStepData();
				commonTutorialStepData.tutorialContent = "点击屏幕结束第er步";
				commonTutorialStepData.position = new Rect(step_1.anchoredPosition.x, step_1.anchoredPosition.y, step_1.sizeDelta.x, step_1.sizeDelta.y);
				commonTutorialStepData.changeRect = true;
				commonTutorialStepData.stepAction = delegate
				{
					MonoSingleton<GameTools>.Instacne.PlayerVideoAd(ADScene.LevelTargetRefresh, OnAddSuccess);
				};
				commonTutorialData.AddStep(commonTutorialStepData);
				Mod.UI.OpenUIForm(UIFormId.CommonTutorialForm, commonTutorialData);
			}
		}
	}
}
