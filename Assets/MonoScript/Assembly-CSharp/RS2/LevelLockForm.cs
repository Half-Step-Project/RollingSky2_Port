using System;
using System.Collections.Generic;
using Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace RS2
{
	public class LevelLockForm : UGUIForm
	{
		private GameObject closeBtn;

		private GameObject keyUnLockBtn;

		private GameObject goldUnLockBtn;

		private GameObject ticketsUnLockBtn;

		private Button keyUnLockButton;

		private GameObject helpBtn;

		private GameObject adIcon;

		private GameObject goodsContent;

		private GameObject chengjiuLockContainer;

		private Text keyNumTxt;

		private Text keyDescTxt;

		private Text goldNumTxt;

		private Text goldDescTxt;

		private Text ticketsDescTxt;

		private Text ticketsNumTxt;

		private Text keyTotalNumTxt;

		private Text redHeartNumTxt;

		private Text totalgoldNumTxt;

		private GameObject goldBuyBtn;

		private GameObject keyBuyBtn;

		private Text openTimeTxt;

		private Text titleTxt;

		private Text daimondUnlockTimeTxt;

		private int currentLevelId = -1;

		private int ticketState;

		private Button ticketsUnLockButton;

		private LevelLockData currentData;

		private Dictionary<int, int> unLockDic = new Dictionary<int, int>();

		public Color m_NormalColor;

		public Color m_FullColor;

		private bool m_isOpenAlert;

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			Dictionary<string, GameObject> dictionary = ViewTools.CollectAllGameObjects(base.gameObject);
			closeBtn = dictionary["back"];
			keyUnLockBtn = dictionary["keyUnLockBtn"];
			keyUnLockBtn.SetActive(false);
			keyUnLockButton = keyUnLockBtn.GetComponent<Button>();
			goldUnLockBtn = dictionary["goldUnLockBtn"];
			goldUnLockBtn.SetActive(false);
			ticketsUnLockBtn = dictionary["ticketsUnLockBtn"];
			ticketsUnLockButton = ticketsUnLockBtn.GetComponent<Button>();
			helpBtn = dictionary["helpBtn"];
			keyNumTxt = dictionary["keyNumTxt"].GetComponent<Text>();
			goldNumTxt = dictionary["goldNumTxt"].GetComponent<Text>();
			ticketsDescTxt = dictionary["ticketsDescTxt"].GetComponent<Text>();
			keyDescTxt = dictionary["keyDescTxt"].GetComponent<Text>();
			goldDescTxt = dictionary["goldDescTxt"].GetComponent<Text>();
			ticketsNumTxt = dictionary["ticketsNumTxt"].GetComponent<Text>();
			redHeartNumTxt = dictionary["redHeartNumTxt"].GetComponent<Text>();
			keyTotalNumTxt = dictionary["keyTotalNumTxt"].GetComponent<Text>();
			totalgoldNumTxt = dictionary["totalgoldNumTxt"].GetComponent<Text>();
			daimondUnlockTimeTxt = dictionary["daimondUnlockTimeTxt"].GetComponent<Text>();
			openTimeTxt = dictionary["openTimeTxt"].GetComponent<Text>();
			titleTxt = dictionary["titleTxt"].GetComponent<Text>();
			adIcon = dictionary["adIcon"];
			chengjiuLockContainer = dictionary["chengjiuLockContainer"];
			goodsContent = dictionary["goodsContent"];
			keyBuyBtn = dictionary["keyBuyBtn"];
			goldBuyBtn = dictionary["goldBuyBtn"];
			if (DeviceManager.Instance.IsNeedSpecialAdapte())
			{
				MonoSingleton<GameTools>.Instacne.AdapteSpecialScreen(goodsContent.transform as RectTransform);
			}
			RefreshGoodsNum();
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			currentData = (LevelLockData)userData;
			currentLevelId = currentData.currentLevelId;
			AddEventHandler();
			ShowState(currentLevelId);
			InfocUtils.Report_rollingsky2_games_pageshow(5, 4, 1, currentLevelId);
		}

		protected override void OnClose(object userData)
		{
			base.OnClose(userData);
			RemoveEventHandler();
			Reset();
		}

		private void OnGameGoodsNumChange(object sender, Foundation.EventArgs e)
		{
			if (e is GameGoodsNumChangeEventArgs)
			{
				RefreshGoodsNum();
			}
		}

		private void RefreshGoodsNum()
		{
			PlayerDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule);
			redHeartNumTxt.text = dataModule.GetPlayGoodsNum(1).ToString();
			totalgoldNumTxt.text = dataModule.GetPlayGoodsNum(3).ToString();
			keyTotalNumTxt.text = dataModule.GetPlayGoodsNum(6).ToString();
		}

		private void AddEventHandler()
		{
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(closeBtn);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(CloseBtnClickHandler));
			EventTriggerListener eventTriggerListener2 = EventTriggerListener.Get(keyUnLockBtn);
			eventTriggerListener2.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener2.onClick, new EventTriggerListener.VoidDelegate(KeyUnLockBtnClickHandler));
			EventTriggerListener eventTriggerListener3 = EventTriggerListener.Get(goldUnLockBtn);
			eventTriggerListener3.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener3.onClick, new EventTriggerListener.VoidDelegate(GoldUnLockBtnClickHandler));
			EventTriggerListener eventTriggerListener4 = EventTriggerListener.Get(ticketsUnLockBtn);
			eventTriggerListener4.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener4.onClick, new EventTriggerListener.VoidDelegate(TicketUnLockBtnClickHandler));
			Mod.Event.Subscribe(EventArgs<GameGoodsNumChangeEventArgs>.EventId, OnGameGoodsNumChange);
			EventTriggerListener eventTriggerListener5 = EventTriggerListener.Get(helpBtn);
			eventTriggerListener5.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener5.onClick, new EventTriggerListener.VoidDelegate(HelpBtnClickHandler));
			EventTriggerListener eventTriggerListener6 = EventTriggerListener.Get(goldBuyBtn);
			eventTriggerListener6.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener6.onClick, new EventTriggerListener.VoidDelegate(GoldBuyHandle));
			EventTriggerListener eventTriggerListener7 = EventTriggerListener.Get(keyBuyBtn);
			eventTriggerListener7.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener7.onClick, new EventTriggerListener.VoidDelegate(KeyBuyHandler));
		}

		private void HelpBtnClickHandler(GameObject go)
		{
			CommonHelpData commonHelpData = new CommonHelpData();
			commonHelpData.Title = "Unlock rule";
			commonHelpData.Content = Mod.Localization.GetInfoById(2001).Replace("\\n", "\n");
			Mod.UI.OpenUIForm(UIFormId.CommonHelpForm, commonHelpData);
		}

		private void RemoveEventHandler()
		{
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(closeBtn);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(CloseBtnClickHandler));
			EventTriggerListener eventTriggerListener2 = EventTriggerListener.Get(keyUnLockBtn);
			eventTriggerListener2.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener2.onClick, new EventTriggerListener.VoidDelegate(KeyUnLockBtnClickHandler));
			EventTriggerListener eventTriggerListener3 = EventTriggerListener.Get(goldUnLockBtn);
			eventTriggerListener3.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener3.onClick, new EventTriggerListener.VoidDelegate(GoldUnLockBtnClickHandler));
			EventTriggerListener eventTriggerListener4 = EventTriggerListener.Get(ticketsUnLockBtn);
			eventTriggerListener4.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener4.onClick, new EventTriggerListener.VoidDelegate(TicketUnLockBtnClickHandler));
			EventTriggerListener eventTriggerListener5 = EventTriggerListener.Get(helpBtn);
			eventTriggerListener5.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener5.onClick, new EventTriggerListener.VoidDelegate(HelpBtnClickHandler));
			Mod.Event.Unsubscribe(EventArgs<GameGoodsNumChangeEventArgs>.EventId, OnGameGoodsNumChange);
			EventTriggerListener eventTriggerListener6 = EventTriggerListener.Get(goldBuyBtn);
			eventTriggerListener6.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener6.onClick, new EventTriggerListener.VoidDelegate(GoldBuyHandle));
			EventTriggerListener eventTriggerListener7 = EventTriggerListener.Get(keyBuyBtn);
			eventTriggerListener7.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener7.onClick, new EventTriggerListener.VoidDelegate(KeyBuyHandler));
		}

		private void KeyUnLockBtnClickHandler(GameObject obj)
		{
			InfocUtils.Report_rollingsky2_games_pageshow(5, 4, 2, currentLevelId);
			PlayerDataModule pleyrDataModule = Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule);
			int unLockNum = unLockDic[6];
			m_isOpenAlert = true;
			CommonAlertData commonAlertData = new CommonAlertData();
			commonAlertData.showType = CommonAlertData.AlertShopType.UNLOCK;
			commonAlertData.lableContent = Mod.Localization.GetInfoById(20);
			commonAlertData.iconid = Mod.DataTable.Get<Goods_goodsTable>()[6].IconId;
			commonAlertData.goodsNum = unLockNum;
			commonAlertData.callBackFunc = delegate
			{
				if (pleyrDataModule.GetPlayGoodsNum(6) >= (double)unLockNum)
				{
					InfocUtils.Report_rollingsky2_games_Unlock(4, currentData.currentLevelId);
					int state = MonoSingleton<GameTools>.Instacne.GenerateUnLockState(LevelLockType.GOODS, 6);
					pleyrDataModule.ChangePlayerGoodsNum(6, -unLockNum);
					pleyrDataModule.SetLevelLockState(currentLevelId, state);
					Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule).TryLevel = false;
					Mod.UI.CloseUIForm(UIFormId.LevelLockForm);
					Mod.UI.CloseUIForm(UIFormId.CommonAlertForm);
					m_isOpenAlert = false;
					EnterLevel(LevelLockData.EnterLevelType.LOCKED);
				}
			};
			commonAlertData.closeCallBackFunc = delegate
			{
				Mod.UI.CloseUIForm(UIFormId.CommonAlertForm);
			};
			Mod.UI.OpenUIForm(UIFormId.CommonAlertForm, commonAlertData);
		}

		private void EnterLevel(LevelLockData.EnterLevelType type)
		{
			if (currentData != null && currentData.callBack != null)
			{
				currentData.callBack(type);
				currentData = null;
			}
		}

		private void GoldUnLockBtnClickHandler(GameObject obj)
		{
			PlayerDataModule pleyrDataModule = Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule);
			int num = (int)pleyrDataModule.GetPlayGoodsNum(3);
			int unLockNum = unLockDic[3];
			CommonAlertData commonAlertData = new CommonAlertData();
			if (num >= unLockNum)
			{
				commonAlertData.showType = CommonAlertData.AlertShopType.UNLOCK;
				commonAlertData.lableContent = Mod.Localization.GetInfoById(20);
				commonAlertData.iconid = Mod.DataTable.Get<Goods_goodsTable>()[3].IconId;
				commonAlertData.goodsNum = unLockNum;
				commonAlertData.callBackFunc = delegate
				{
					int state = MonoSingleton<GameTools>.Instacne.GenerateUnLockState(LevelLockType.GOODS, 3);
					pleyrDataModule.ChangePlayerGoodsNum(3, -unLockNum);
					pleyrDataModule.SetLevelLockState(currentLevelId, state);
					Mod.UI.CloseUIForm(UIFormId.LevelLockForm);
					Mod.UI.CloseUIForm(UIFormId.CommonAlertForm);
					EnterLevel(LevelLockData.EnterLevelType.LOCKED);
				};
				Mod.UI.OpenUIForm(UIFormId.CommonAlertForm, commonAlertData);
			}
		}

		private void TicketUnLockBtnClickHandler(GameObject obj)
		{
			Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule);
			if (ticketState == 1 || !currentData.m_isHadDowloaded)
			{
				Mod.UI.CloseUIForm(UIFormId.LevelLockForm);
				EnterLevel(LevelLockData.EnterLevelType.TRY);
				return;
			}
			int num = 0;
			if (currentLevelId == 3 || currentLevelId == 4 || currentLevelId == 5)
			{
				num = currentLevelId;
			}
			string info = string.Format("scenes=3&scenes_source=0&ad_source=1&action=3&source=0&level={0}", num);
			InfocUtils.ReportInfoc(InfocTable.rollingsky2_games_ads, info);
			MonoSingleton<GameTools>.Instacne.PlayerVideoAd(ADScene.LevelLockView, OnAddSuccess);
		}

		private void OnAddSuccess(ADScene scene)
		{
			int num = 0;
			if (currentLevelId == 3 || currentLevelId == 4 || currentLevelId == 5)
			{
				num = currentLevelId;
			}
			string info = string.Format("scenes=3&scenes_source=0&ad_source=1&action=4&source=0&level={0}", num);
			InfocUtils.ReportInfoc(InfocTable.rollingsky2_games_ads, info);
			num = currentLevelId;
			PlayerDataModule.Instance.SetLevelTicketNum(currentLevelId, GameCommon.tyrLevelNumPerAd);
			SwitchTicketStateByTickeyNum(GameCommon.tyrLevelNumPerAd);
			Mod.UI.CloseUIForm(UIFormId.LevelLockForm);
			if ((int)PlayerDataModule.Instance.GetPlayGoodsNum(1) <= 0)
			{
				Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).ChangePlayerGoodsNum(1, 2.0);
			}
			EnterLevel(LevelLockData.EnterLevelType.TRY);
		}

		private void CloseBtnClickHandler(GameObject obj)
		{
			Mod.UI.CloseUIForm(UIFormId.LevelLockForm);
		}

		private void ShowState(int levelId)
		{
			PlayerDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule);
			PlayerLocalLevelData playerLevelData = dataModule.GetPlayerLevelData(levelId);
			string[] array = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).GetLevelTableById(levelId).UnLockIds.Split('|');
			LevelUnLock_table levelUnLock_table = null;
			int num = 0;
			for (int i = 0; i < array.Length; i++)
			{
				levelUnLock_table = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).GetLevelUnLockById(int.Parse(array[i]));
				if (levelUnLock_table == null)
				{
					continue;
				}
				LevelLockType lockType = (LevelLockType)levelUnLock_table.LockType;
				switch (lockType)
				{
				case LevelLockType.GOODS:
					switch (levelUnLock_table.LockTypeId)
					{
					case 6:
						unLockDic.Add(6, levelUnLock_table.UnLockNum);
						keyUnLockBtn.SetActive(true);
						keyDescTxt.text = Mod.Localization.GetInfoById(20);
						break;
					case 3:
						unLockDic.Add(3, levelUnLock_table.UnLockNum);
						goldUnLockBtn.SetActive(true);
						goldDescTxt.text = Mod.Localization.GetInfoById(20);
						break;
					}
					break;
				case LevelLockType.CHENGJIU:
				{
					chengjiuLockContainer.SetActive(true);
					int playerTotalDiamondsNum = dataModule.GetPlayerTotalDiamondsNum();
					int unLockNum = levelUnLock_table.UnLockNum;
					daimondUnlockTimeTxt.text = string.Format("{0}/{1}", playerTotalDiamondsNum, unLockNum);
					if (playerTotalDiamondsNum >= unLockNum)
					{
						daimondUnlockTimeTxt.color = m_FullColor;
						int state = MonoSingleton<GameTools>.Instacne.GenerateUnLockState(lockType, 0, ChengJiuType.DIAMOND);
						dataModule.SetLevelLockState(currentLevelId, state);
						Mod.UI.CloseUIForm(UIFormId.LevelLockForm);
						EnterLevel(LevelLockData.EnterLevelType.LOCKED);
					}
					else
					{
						daimondUnlockTimeTxt.color = m_NormalColor;
					}
					break;
				}
				case LevelLockType.TIME:
					if (unLockDic.ContainsKey(3))
					{
						num = ComputerPrice(playerLevelData.levelId, unLockDic[3], levelUnLock_table.UnLockNum);
						unLockDic[3] = num;
						goldNumTxt.text = string.Format("{0}", num);
					}
					if (unLockDic.ContainsKey(6))
					{
						num = ComputerPrice(playerLevelData.levelId, unLockDic[6], levelUnLock_table.UnLockNum);
						unLockDic[6] = num;
						keyNumTxt.text = string.Format("{0}", num);
					}
					break;
				}
			}
			ticketsUnLockBtn.SetActive(true);
			int ticketNum = playerLevelData.GetTicketNum();
			SwitchTicketStateByTickeyNum(ticketNum);
		}

		private void SwitchTicketStateByTickeyNum(int ticketNum)
		{
			if (ticketNum > 0)
			{
				ticketState = 1;
				adIcon.SetActive(false);
				ticketsDescTxt.gameObject.SetActive(true);
				ticketsDescTxt.text = Mod.Localization.GetInfoById(18);
				ticketsNumTxt.gameObject.SetActive(false);
				return;
			}
			ticketState = 2;
			adIcon.SetActive(true);
			ticketsDescTxt.gameObject.SetActive(true);
			ticketsDescTxt.text = Mod.Localization.GetInfoById(18);
			ticketsNumTxt.gameObject.SetActive(true);
			ticketsNumTxt.text = string.Format("x{0}", GameCommon.tyrLevelNumPerAd);
			int num = 0;
			if (currentLevelId == 3 || currentLevelId == 4 || currentLevelId == 5)
			{
				num = currentLevelId;
			}
			string info = string.Format("scenes=3&scenes_source=0&ad_source=1&action=1&source=0&level={0}", num);
			InfocUtils.ReportInfoc(InfocTable.rollingsky2_games_ads, info);
		}

		private void Reset()
		{
			unLockDic.Clear();
			ticketState = 0;
			currentLevelId = -1;
			if (m_isOpenAlert)
			{
				Mod.UI.CloseUIForm(UIFormId.CommonAlertForm);
				m_isOpenAlert = false;
			}
		}

		private void KeyBuyHandler(GameObject go)
		{
		}

		private void GoldBuyHandle(GameObject go)
		{
		}

		private int ComputerPrice(int levelId, int orinalPrice, int unLocktime)
		{
			return orinalPrice;
		}
	}
}
