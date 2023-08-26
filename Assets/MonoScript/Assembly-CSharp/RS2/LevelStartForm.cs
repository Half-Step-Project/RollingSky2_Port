using System;
using System.Collections.Generic;
using Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace RS2
{
	public class LevelStartForm : UGUIForm
	{
		public class ShowData
		{
			public bool showRebirth = true;
		}

		private ShowData showData;

		public GameObject openBtn;

		public GameObject block;

		public Transform root;

		public GameObject activeIcon;

		public GameObject inactiveIcon;

		public Text numText;

		public GameObject buyGo;

		public GameObject info;

		public GameObject infoEffect;

		public RectTransform adapterRoot;

		public GameObject rebirthRoot;

		public GuideBuffRoot guideBuffRoot;

		public RebirthBuffRoot rebirthBuffRoot;

		public ShieldBuffRoot shieldBuffRoot;

		public Text m_CoolPlayTxt;

		private MonoTimer m_timer;

		private List<object> loadedAsserts = new List<object>();

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			if (DeviceManager.Instance.IsNeedSpecialAdapte())
			{
				MonoSingleton<GameTools>.Instacne.AdapteSpecialScreen(adapterRoot, -45);
			}
			if (m_CoolPlayTxt != null)
			{
				m_CoolPlayTxt.text = "";
				m_CoolPlayTxt.gameObject.SetActive(false);
			}
		}

		protected override void OnUnload()
		{
			base.OnUnload();
			for (int i = 0; i < loadedAsserts.Count; i++)
			{
				Mod.Resource.UnloadAsset(loadedAsserts[i]);
			}
			loadedAsserts.Clear();
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			showData = userData as ShowData;
			if (block != null)
			{
				EventTriggerListener eventTriggerListener = EventTriggerListener.Get(block);
				eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(OnClickBlockButton));
			}
			if (infoEffect != null)
			{
				infoEffect.SetActive(!PlayerDataModule.Instance.PlayerLocalVideoAwardData.HasClickGuideInfo);
			}
			Mod.Event.Subscribe(EventArgs<ClickGameStartButtonEventArgs>.EventId, OnClickGameStart);
			Mod.Event.Subscribe(EventArgs<BuyOutRebirthUnLockEventArgs>.EventId, OnBuyOutUnLock);
			Mod.Event.Subscribe(EventArgs<GameStartButtonActiveEventArgs>.EventId, OnGameStartButtonActiveHandle);
			Refresh();
			PlayerDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule);
			int curLevelId = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).CurLevelId;
			dataModule.GetPlayerLevelData(curLevelId);
			info.SetActive(false);
			Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule).HasUseRebirthItem = false;
			rebirthRoot.SetActive(showData.showRebirth);
			UpdateBuffRoots();
			if (m_CoolPlayTxt != null)
			{
				EventTriggerListener eventTriggerListener2 = EventTriggerListener.Get(m_CoolPlayTxt.gameObject);
				eventTriggerListener2.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener2.onClick, new EventTriggerListener.VoidDelegate(OnCoolPlayClickHandler));
			}
			if (!PlayerDataModule.Instance.CoolPlayPagageData.IsEnable() || !(m_CoolPlayTxt != null))
			{
				return;
			}
			m_CoolPlayTxt.gameObject.SetActive(true);
			m_timer = new MonoTimer(1f, true);
			m_timer.Elapsed += delegate
			{
				int num = (int)((float)PlayerDataModule.Instance.CoolPlayPagageData.LeftTime() * 0.001f);
				if (num <= 0)
				{
					m_CoolPlayTxt.gameObject.SetActive(false);
					m_timer.Stop();
				}
				else
				{
					m_CoolPlayTxt.text = MonoSingleton<GameTools>.Instacne.TimeFormat_HH_MM_SS(num);
				}
			};
			m_timer.FireElapsedOnStop = false;
			m_timer.Start();
		}

		private void OnGameStartButtonActiveHandle(object sender, Foundation.EventArgs e)
		{
			GameStartButtonActiveEventArgs gameStartButtonActiveEventArgs = e as GameStartButtonActiveEventArgs;
			if (gameStartButtonActiveEventArgs != null && adapterRoot != null)
			{
				adapterRoot.gameObject.SetActive(gameStartButtonActiveEventArgs.mActive);
			}
		}

		private void OnCoolPlayClickHandler(GameObject go)
		{
			new CoolPlayData
			{
				Type = CoolPlayData.OpenType.INFO,
				ShopId = GameCommon.COOLPLAY_PACKAGE,
				CallBack = null
			};
		}

		private void UpdateBuffRoots()
		{
			if (PlayerDataModule.Instance.BufferIsEnable(GameCommon.GuideLine))
			{
				guideBuffRoot.Show();
			}
			else
			{
				guideBuffRoot.Hide();
			}
			if (PlayerDataModule.Instance.BufferIsEnable(GameCommon.ORIGIN_REBIRTH_FREE))
			{
				rebirthBuffRoot.Show();
			}
			else
			{
				rebirthBuffRoot.Hide();
			}
			if (PlayerDataModule.Instance.BufferIsEnable(GameCommon.START_FREE_SHIELD))
			{
				shieldBuffRoot.Show();
			}
			else
			{
				shieldBuffRoot.Hide();
			}
		}

		protected override void OnClose(object userData)
		{
			base.OnClose(userData);
			if (block != null)
			{
				EventTriggerListener eventTriggerListener = EventTriggerListener.Get(block);
				eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(OnClickBlockButton));
			}
			if (m_CoolPlayTxt != null)
			{
				EventTriggerListener eventTriggerListener2 = EventTriggerListener.Get(m_CoolPlayTxt.gameObject);
				eventTriggerListener2.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener2.onClick, new EventTriggerListener.VoidDelegate(OnCoolPlayClickHandler));
			}
			Mod.Event.Unsubscribe(EventArgs<ClickGameStartButtonEventArgs>.EventId, OnClickGameStart);
			Mod.Event.Unsubscribe(EventArgs<BuyOutRebirthUnLockEventArgs>.EventId, OnBuyOutUnLock);
			Mod.Event.Unsubscribe(EventArgs<GameStartButtonActiveEventArgs>.EventId, OnGameStartButtonActiveHandle);
			if (m_timer != null)
			{
				m_timer.Stop();
			}
			m_timer = null;
		}

		protected override void OnTick(float elapseSeconds, float realElapseSeconds)
		{
			base.OnTick(elapseSeconds, realElapseSeconds);
			if (GameController.Instance != null && GameController.Instance.M_gameState == GameController.GAMESTATE.Runing)
			{
				bool flag = base.UIForm != null;
			}
		}

		private void Refresh()
		{
			PlayerDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule);
			int curLevelId = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).CurLevelId;
			if (dataModule.GetPlayerLevelData(curLevelId).GetBuyOutRebirth() > 0)
			{
				block.SetActive(true);
				activeIcon.SetActive(true);
				inactiveIcon.SetActive(false);
			}
			else
			{
				block.SetActive(false);
				activeIcon.SetActive(false);
				inactiveIcon.SetActive(true);
			}
		}

		public void OpenBtnClickHandler(GameObject obj)
		{
			Mod.UI.OpenUIForm(UIFormId.BuyOutRebirthForm, 0);
		}

		public void OnClickBlockButton(GameObject obj)
		{
			Mod.UI.OpenUIForm(UIFormId.BuyOutRebirthIntroductionForm);
		}

		private void OnClickGameStart(object sender, Foundation.EventArgs e)
		{
		}

		private void OnBuyOutUnLock(object sender, Foundation.EventArgs e)
		{
			Refresh();
		}

		public void OnClickShowInfo()
		{
			PlayerDataModule.Instance.PlayerLocalVideoAwardData.HasClickGuideInfo = true;
			info.SetActive(true);
			if (infoEffect != null)
			{
				infoEffect.SetActive(false);
			}
		}

		public void OnClickCloseInfo()
		{
			info.SetActive(false);
		}

		public void OnClickRebirthBuff()
		{
			if (rebirthBuffRoot.IsTipShow)
			{
				rebirthBuffRoot.HideTips();
				return;
			}
			rebirthBuffRoot.ShowTips();
			shieldBuffRoot.HideTips();
			guideBuffRoot.HideTips();
		}

		public void OnClickGuideBuff()
		{
			if (guideBuffRoot.IsTipShow)
			{
				guideBuffRoot.HideTips();
				return;
			}
			rebirthBuffRoot.HideTips();
			shieldBuffRoot.HideTips();
			guideBuffRoot.ShowTips();
		}

		public void OnClickShieldBuff()
		{
			if (shieldBuffRoot.IsTipShow)
			{
				shieldBuffRoot.HideTips();
				return;
			}
			rebirthBuffRoot.HideTips();
			shieldBuffRoot.ShowTips();
			guideBuffRoot.HideTips();
		}
	}
}
