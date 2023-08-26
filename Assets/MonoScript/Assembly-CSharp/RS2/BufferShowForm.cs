using System;
using System.Collections.Generic;
using Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace RS2
{
	public class BufferShowForm : UGUIForm
	{
		public Text titleTxt;

		public UILoopList uiLoopList;

		public GameObject closeBtn;

		public RectTransform step_0;

		public RectTransform step_1;

		public GiftPackageController m_GiftPackageController;

		private int m_recommandGiftId = -1;

		private const int LeaveBufferShowFormPluginAdId = 9;

		private static int m_leaveBufferShowFormTotalTime;

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			m_GiftPackageController.Init();
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			m_GiftPackageController.OnOpen();
			AddEventListener();
			SetData();
			InfocUtils.Report_rollingsky2_games_pageshow(10, 7, 1);
		}

		protected override void OnClose(object userData)
		{
			base.OnClose(userData);
			m_GiftPackageController.OnReset();
			RemoveEventListener();
		}

		private void AddEventListener()
		{
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(closeBtn);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(CloseHandler));
			Mod.Event.Subscribe(EventArgs<GameGoodsNumChangeEventArgs>.EventId, OnPlayerAssetChange);
		}

		private void OnPlayerAssetChange(object sender, Foundation.EventArgs e)
		{
			GameGoodsNumChangeEventArgs gameGoodsNumChangeEventArgs = e as GameGoodsNumChangeEventArgs;
			if (gameGoodsNumChangeEventArgs != null && (gameGoodsNumChangeEventArgs.GoodsId == GameCommon.START_FREE_SHIELD || gameGoodsNumChangeEventArgs.GoodsId == GameCommon.EVERY_DAY_GIVE_POWER || gameGoodsNumChangeEventArgs.GoodsId == GameCommon.ORIGIN_REBIRTH_FREE))
			{
				SetData();
			}
		}

		private void GiftBuyHandler(GameObject go)
		{
			Mod.UI.OpenUIForm(UIFormId.GiftPackageBuyForm, m_recommandGiftId);
		}

		private void RemoveEventListener()
		{
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(closeBtn);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(CloseHandler));
			EventTriggerListener eventTriggerListener2 = EventTriggerListener.Get(m_GiftPackageController.gameObject);
			eventTriggerListener2.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener2.onClick, new EventTriggerListener.VoidDelegate(GiftBuyHandler));
			Mod.Event.Unsubscribe(EventArgs<GameGoodsNumChangeEventArgs>.EventId, OnPlayerAssetChange);
		}

		private void OnAddSuccess(ADScene adScene)
		{
		}

		private void CloseHandler(GameObject obj)
		{
			Mod.UI.CloseUIForm(UIFormId.BufferShowForm);
		}

		private void SetData()
		{
			List<int> list = new List<int>();
			list.Add(GameCommon.START_FREE_SHIELD);
			list.Add(GameCommon.EVERY_DAY_GIVE_POWER);
			list.Add(GameCommon.ORIGIN_REBIRTH_FREE);
			uiLoopList.Data(list);
			m_recommandGiftId = GetRecommandPackageID();
			if (m_recommandGiftId > 0)
			{
				m_GiftPackageController.SetData(m_recommandGiftId);
				m_GiftPackageController.gameObject.SetActive(true);
			}
			else
			{
				m_GiftPackageController.gameObject.SetActive(false);
			}
		}

		protected override void OnUnload()
		{
			base.OnUnload();
			m_GiftPackageController.Release();
			for (int i = 0; i < uiLoopList.transform.childCount; i++)
			{
				uiLoopList.transform.GetChild(i).GetComponent<BufferShowItemController>().OnRelease();
			}
		}

		private int GetRecommandPackageID()
		{
			int num = (int)PlayerDataModule.Instance.GetPlayGoodsNum(GameCommon.ORIGIN_REBIRTH_FREE);
			int num2 = (int)PlayerDataModule.Instance.GetPlayGoodsNum(GameCommon.START_FREE_SHIELD);
			int num3 = (int)PlayerDataModule.Instance.GetPlayGoodsNum(GameCommon.EVERY_DAY_GIVE_POWER);
			if (num <= 0 && num2 <= 0 && num3 <= 0)
			{
				return GameCommon.FULL_BUFF_GIFT;
			}
			if (num > 0 && num3 <= 0 && num2 <= 0)
			{
				return 4401;
			}
			if (num3 > 0 && num <= 0 && num2 <= 0)
			{
				return 4402;
			}
			if (num2 > 0 && num <= 0 && num3 <= 0)
			{
				return 4403;
			}
			if (num <= 0)
			{
				return GameCommon.REBIRTH_BUFF_GIFT_NO_DISOCOUNT;
			}
			if (num2 <= 0)
			{
				return GameCommon.SHIED_BUFF_GIFT_NO_DISOCOUNT;
			}
			if (num3 <= 0)
			{
				return GameCommon.POWER_BUFF_GIFT_NO_DISOCOUNT;
			}
			return -1;
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

		private bool DealBufferShowPluginAd()
		{
			m_leaveBufferShowFormTotalTime++;
			int num = (int)PlayerDataModule.Instance.GetPlayGoodsNum(GameCommon.ORIGIN_REBIRTH_FREE);
			int num2 = (int)PlayerDataModule.Instance.GetPlayGoodsNum(GameCommon.START_FREE_SHIELD);
			int num3 = (int)PlayerDataModule.Instance.GetPlayGoodsNum(GameCommon.EVERY_DAY_GIVE_POWER);
			if (num > 0 && num2 > 0 && num3 > 0)
			{
				return false;
			}
			int id = MonoSingleton<GameTools>.Instacne.ComputerScreenPluginAdId(9);
			ScreenPluginsAd_table screenPluginsAd_table = Mod.DataTable.Get<ScreenPluginsAd_table>()[id];
			bool flag = false;
			if (screenPluginsAd_table != null)
			{
				flag |= m_leaveBufferShowFormTotalTime >= screenPluginsAd_table.TriggerNum;
				flag &= !PlayerDataModule.Instance.PluginAdController.CommonBuyRecord.Contains(UIFormId.BufferShowForm);
				if (flag)
				{
					flag &= PlayerDataModule.Instance.PluginAdController.IsScreenAdRead();
					if (flag)
					{
						PluginAdData pluginAdData = new PluginAdData();
						pluginAdData.PluginId = 9;
						pluginAdData.EndHandler = delegate
						{
							ClearBufferShowPluginAdData();
							ClearBufferShowPluginAdCountData();
						};
						Mod.UI.OpenUIForm(UIFormId.ScreenPluginsForm, pluginAdData);
					}
				}
			}
			ClearBufferShowPluginAdData();
			return flag;
		}

		private void ClearBufferShowPluginAdData()
		{
			PlayerDataModule.Instance.PluginAdController.CommonBuyRecord.RemoveAll((UIFormId x) => x == UIFormId.BufferShowForm);
		}

		private void ClearBufferShowPluginAdCountData()
		{
			m_leaveBufferShowFormTotalTime = 0;
		}
	}
}
