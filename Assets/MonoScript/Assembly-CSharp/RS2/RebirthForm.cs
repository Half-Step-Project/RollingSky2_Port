using System;
using System.Collections.Generic;
using Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace RS2
{
	public class RebirthForm : UGUIForm
	{
		private Text m_savemeText;

		private Image m_timeSlider;

		private Text m_levelProgressText;

		private Text m_heartRebirthCountText;

		private Text m_heartRebirthText;

		private Button m_heartRebirthButton;

		private Button m_closeButton;

		public float m_currentTime;

		public float m_continueTime = 10f;

		private float m_laveTime = 10f;

		public float m_progress;

		private int m_needResurrectionHeartCount = 1;

		private int m_needkeyCount = 1;

		private int m_keyToResurrectionHeart = 5;

		private int m_adToResurrectionHeart = 3;

		public bool m_isDetectAd;

		public float m_detectAdTime;

		public float m_detectAdIntervalTime = 1f;

		private int m_keyToResurrectionHeartProductID = 10001;

		private bool m_isPause;

		private int ResurrectionHeartNumber
		{
			get
			{
				return (int)Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).GetPlayGoodsNum(2);
			}
		}

		private int KeyNumber
		{
			get
			{
				return (int)Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).GetPlayGoodsNum(6);
			}
		}

		private bool IsAdVideoCanShow
		{
			get
			{
				return MonoSingleton<GameTools>.Instacne.CanShowAd(ADScene.ReBirthView);
			}
		}

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			Dictionary<string, GameObject> dictionary = ViewTools.CollectAllGameObjects(base.gameObject);
			m_savemeText = dictionary["saveText"].GetComponent<Text>();
			m_timeSlider = dictionary["timeSliderFill"].GetComponent<Image>();
			m_levelProgressText = dictionary["levelProgressText"].GetComponent<Text>();
			m_heartRebirthCountText = dictionary["heartRebirthCountText"].GetComponent<Text>();
			m_heartRebirthText = dictionary["heartRebirthText"].GetComponent<Text>();
			m_heartRebirthButton = dictionary["heartRebirthButton"].GetComponent<Button>();
			m_closeButton = dictionary["closeButton"].GetComponent<Button>();
			m_needkeyCount = 1;
			m_keyToResurrectionHeart = 5;
			Shops_shopTable shops_shopTable = Mod.DataTable.Get<Shops_shopTable>().Get(m_keyToResurrectionHeartProductID);
			m_needkeyCount = int.Parse(shops_shopTable.Price);
			GoodsTeam_goodsTeamTable goodsTeam_goodsTeamTable = Mod.DataTable.Get<GoodsTeam_goodsTeamTable>().Get(shops_shopTable.GoodsTeamid);
			m_keyToResurrectionHeart = int.Parse(goodsTeam_goodsTeamTable.GoodsCount) * shops_shopTable.Count;
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			RefreshLanguage();
			m_isPause = false;
			AddEventListener();
			ResetWindow(true);
			if (ResurrectionHeartNumber >= m_needResurrectionHeartCount)
			{
				string info = string.Format("scenes=2&scenes_source=0&ad_source=1&action=1&source=0");
				InfocUtils.ReportInfoc(InfocTable.rollingsky2_games_ads, info);
			}
		}

		protected override void OnClose(object userData)
		{
			base.OnClose(userData);
			RemoveEventListener();
		}

		protected override void OnTick(float elapseSeconds, float realElapseSeconds)
		{
			if (m_isPause)
			{
				return;
			}
			base.OnTick(elapseSeconds, realElapseSeconds);
			m_currentTime += elapseSeconds;
			if (m_currentTime > m_continueTime)
			{
				BackToResultForm();
				return;
			}
			m_laveTime = m_continueTime - m_currentTime;
			m_progress = m_laveTime / m_continueTime;
			m_timeSlider.fillAmount = m_progress;
			if (!m_isDetectAd)
			{
				return;
			}
			m_detectAdTime += elapseSeconds;
			if (m_detectAdTime >= m_detectAdIntervalTime)
			{
				if (IsAdVideoCanShow)
				{
					ResetWindow(false);
					m_isDetectAd = false;
				}
				m_detectAdTime = 0f;
			}
		}

		protected override void OnCover()
		{
			base.OnCover();
			m_isPause = true;
		}

		protected override void OnReveal()
		{
			base.OnReveal();
			m_isPause = false;
		}

		private void AddEventListener()
		{
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(m_heartRebirthButton.gameObject);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(OnClickHeartRebirthButtonBySelect));
			EventTriggerListener eventTriggerListener2 = EventTriggerListener.Get(m_closeButton.gameObject);
			eventTriggerListener2.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener2.onClick, new EventTriggerListener.VoidDelegate(OnClickCloseButton));
			Mod.Event.Subscribe(EventArgs<GameGoodsNumChangeEventArgs>.EventId, OnPlayerAssetChange);
		}

		private void RemoveEventListener()
		{
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(m_heartRebirthButton.gameObject);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(OnClickHeartRebirthButtonBySelect));
			EventTriggerListener eventTriggerListener2 = EventTriggerListener.Get(m_closeButton.gameObject);
			eventTriggerListener2.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener2.onClick, new EventTriggerListener.VoidDelegate(OnClickCloseButton));
			Mod.Event.Unsubscribe(EventArgs<GameGoodsNumChangeEventArgs>.EventId, OnPlayerAssetChange);
		}

		private void OnPlayerAssetChange(object sender, Foundation.EventArgs e)
		{
			GameGoodsNumChangeEventArgs gameGoodsNumChangeEventArgs = e as GameGoodsNumChangeEventArgs;
			if (gameGoodsNumChangeEventArgs != null && gameGoodsNumChangeEventArgs.GoodsId == 2)
			{
				m_heartRebirthCountText.text = ResurrectionHeartNumber.ToString();
			}
		}

		private void OnClickHeartRebirthButton(GameObject obj)
		{
			InsideGameDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule);
			if (dataModule.CurrentRebirthBoxData != null)
			{
				dataModule.m_isOriginRebirth = true;
				dataModule.m_rebirthProgress = dataModule.CurrentRebirthBoxData.m_rebirthProgressData.m_levelProgress;
			}
			RangeRebirth(-m_needResurrectionHeartCount);
			dataModule.m_theBoardRebirthCount++;
			Mod.Event.Fire(this, Mod.Reference.Acquire<GameRebirthResetEventArgs>());
			Mod.UI.CloseUIForm(UIFormId.RebirthForm);
			Mod.UI.CloseUIForm(UIFormId.CommonAlertForm);
			InfocUtils.Report_rollingsky2_games_currency(2, 2, 4);
		}

		private void OnClickExchangeHeartButton(GameObject obj)
		{
			Shops_shopTable _shopTable = Mod.DataTable.Get<Shops_shopTable>().Get(m_keyToResurrectionHeartProductID);
			CommonAlertData commonAlertData = new CommonAlertData();
			commonAlertData.showType = CommonAlertData.AlertShopType.BUY_SHOPITEM;
			commonAlertData.shopItemId = _shopTable.Id;
			commonAlertData.lableContent = Mod.Localization.GetInfoById(21);
			commonAlertData.alertContent = Mod.Localization.GetInfoById(58);
			commonAlertData.callBackFunc = delegate
			{
				InfocUtils.Report_rollingsky2_games_neigou(_shopTable.Id, 2, Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).CurLevelId);
				if (KeyNumber >= m_needkeyCount)
				{
					OnBuy();
					InfocUtils.Report_rollingsky2_games_neigou(_shopTable.Id, 3, Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).CurLevelId);
					Mod.UI.CloseUIForm(UIFormId.CommonAlertForm);
					ResetWindow(false);
				}
				else
				{
					InfocUtils.Report_rollingsky2_games_neigou(_shopTable.Id, 4, Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).CurLevelId);
					ResetWindow(false);
				}
			};
			Mod.UI.OpenUIForm(UIFormId.CommonAlertForm, commonAlertData);
		}

		private void OnClickAdButton(GameObject obj)
		{
			if (IsAdVideoCanShow)
			{
				Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule).m_theBoardShowRebirthAdCount++;
				m_isPause = true;
				MonoSingleton<GameTools>.Instacne.PlayerVideoAd(ADScene.ReBirthView, OnAdSuccessCallBack, OnAdFailCallBack);
				string info = string.Format("scenes=2&scenes_source=0&ad_source=1&action=3&source=0");
				InfocUtils.ReportInfoc(InfocTable.rollingsky2_games_ads, info);
			}
		}

		private void OnClickAdReadyButton(GameObject obj)
		{
		}

		private void OnClickCloseButton(GameObject obj)
		{
			BackToResultForm();
		}

		private void OnClickHeartRebirthButtonBySelect(GameObject obj)
		{
			if (ResurrectionHeartNumber >= m_needResurrectionHeartCount)
			{
				OnClickHeartRebirthButton(obj);
			}
			else
			{
				OnClickExchangeHeartButton(obj);
			}
		}

		private void BackToMenuForm()
		{
			Mod.UI.CloseUIForm(UIFormId.RebirthForm);
			Mod.Event.Fire(this, Mod.Reference.Acquire<GameExitEventArgs>());
		}

		private void BackToResultForm()
		{
			Mod.UI.CloseUIForm(UIFormId.RebirthForm);
			Mod.Event.Fire(this, Mod.Reference.Acquire<GameOverEventArgs>());
		}

		private void OnAdSuccessCallBack(ADScene adScene)
		{
			if (adScene == ADScene.ReBirthView)
			{
				m_isPause = false;
				m_currentTime = 0f;
				RangeRebirth(m_adToResurrectionHeart);
				ResetWindow(false);
				string info = string.Format("scenes=2&scenes_source=0&ad_source=1&action=4&source=0");
				InfocUtils.ReportInfoc(InfocTable.rollingsky2_games_ads, info);
			}
		}

		private void OnAdFailCallBack(ADScene adScene)
		{
			if (adScene == ADScene.ReBirthView)
			{
				m_isPause = false;
				m_currentTime = 0f;
				ResetWindow(false);
			}
		}

		private void RefreshLanguage()
		{
			InsideGameDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule);
			m_levelProgressText.text = Mod.Localization.GetInfoById(49) + ": " + dataModule.CurrentRebirthBoxData.m_rebirthProgressData.m_levelProgress + "%";
			m_heartRebirthText.text = string.Format(Mod.Localization.GetInfoById(44), -m_needResurrectionHeartCount);
			m_savemeText.text = Mod.Localization.GetInfoById(48);
		}

		private void ResetWindow(bool resetData)
		{
			if (resetData)
			{
				m_currentTime = 0f;
				m_detectAdTime = 0f;
				m_isDetectAd = false;
			}
			m_heartRebirthCountText.text = ResurrectionHeartNumber.ToString();
			m_heartRebirthButton.gameObject.SetActive(true);
		}

		private void RangeRebirth(int num)
		{
			Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).ChangePlayerGoodsNum(2, num);
		}

		private void RangeKey(int num)
		{
			Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).ChangePlayerGoodsNum(6, num);
		}

		private void OnBuy()
		{
			RangeKey(-m_needkeyCount);
			RangeRebirth(m_keyToResurrectionHeart);
			InfocUtils.Report_rollingsky2_games_currency(2, 1, 1);
		}
	}
}
