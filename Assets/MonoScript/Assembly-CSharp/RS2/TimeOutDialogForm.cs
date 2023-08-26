using System;
using System.Collections;
using System.Collections.Generic;
using Foundation;
using My.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RS2
{
	public class TimeOutDialogForm : UGUIForm
	{
		public class ShowData
		{
			public TimeOutOpenFrom openFrom = TimeOutOpenFrom.NONE;

			public bool isFirst;

			public bool isRebirth;
		}

		private float TIME_TO_COUNT = GameCommon.powerCdTime;

		public static int DEFAULT_ADD_BALLS = GameCommon.powerRecoverNum;

		private static bool isReplay = false;

		private const int SHOP_ITEM_ID = 10002;

		private const int SHOP_ITEM_TIME_ID = 10003;

		private const int SHOP_ITEM_TIME_ID2 = 10004;

		public GameObject timeMenuBackBtn;

		public Button getLivesAdBtn;

		public GameObject getLivesShopBtn;

		public GameObject addLiveContainer;

		public Text addLiveNumTxt;

		public Text getLivesShopDiscountLabel;

		public Text getLivesShopLabel;

		public Text getLivesNumLabel;

		public Text remainLivesLabel;

		public GameObject clockBanner;

		public Text clockTimeLabel;

		public Image clockBg;

		public Text getLivesAdLabel;

		public GameObject Effect_zhizhen_start;

		public GameObject Effect_zhizhen_end;

		private bool isAddBallByAd;

		private bool m_isRealMoneyBuy;

		public ParticleSystemRenderer particles_003_hudunUI;

		private PlayerDataModule playerData;

		private float restTime;

		private uint timerId;

		private uint timerEndId;

		private uint adTimerId;

		private UnityAction buyItemAction;

		private ShowData showData;

		private ADScene adScene;

		public GameObject m_getTimeLiveBtn;

		public Text m_timeLivesNumLabel;

		private UnityAction m_AutoBuyTimeItemAction;

		private UnityAction m_giveTimeItemAction;

		private UnityAction m_buyTimeItemAction;

		public Transform m_topLeft;

		public GameObject effectCamera;

		public GameObject m_ClockEffect;

		public List<ParticleSystemRenderer> m_particleSystems = new List<ParticleSystemRenderer>();

		public GiftPackageController m_giftPackageController;

		public SetUIGrey m_adGray;

		public Text m_adAlertTxt;

		public GameObject m_adIcon;

		public float m_adIconSourcePosX;

		public static int CurrentLevelId { private get; set; }

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			Init();
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			showData = userData as ShowData;
			adScene = ADScene.NONE;
			if (showData.openFrom == TimeOutOpenFrom.MenuForm)
			{
				adScene = ADScene.MainView;
			}
			else if (showData.openFrom == TimeOutOpenFrom.ResultForm)
			{
				adScene = ADScene.ResultView;
			}
			TIME_TO_COUNT = MonoSingleton<GameTools>.Instacne.GetPowerCDByAd();
			Begin();
			string info = string.Format("scenes=1&scenes_source={0}&ad_source=0&action=1&source=0&level=0", (int)adScene);
			InfocUtils.ReportInfoc(InfocTable.rollingsky2_games_ads, info);
			if (Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).PlayerRecordData.firstEnter_TimeOutForm == 0)
			{
				Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).PlayerRecordData.firstEnter_TimeOutForm = 1;
				Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).PlayerRecordData.SaveToLocal();
				if (MonoSingleton<GameTools>.Instacne.IsFirstLanuchGameIn24Hour())
				{
					InfocUtils.Report_rollingsky2_games_gamefiller(4);
				}
			}
			if ((bool)m_giftPackageController)
			{
				m_giftPackageController.OnOpen();
				m_giftPackageController.SetData(GameCommon.powerRecoveryGiftPackageShopID);
			}
			HandleAd();
		}

		private void HandleAd()
		{
			if (My.Core.Singleton<ADHelper>.Instance.RewardeVideoCanShow(adScene) || My.Core.Singleton<ADHelper>.Instance.RecommandVideoCanShow(adScene, true))
			{
				string info = string.Format("scenes=1&scenes_source={0}&ad_source=0&action=2&source=0level=0", (int)adScene);
				InfocUtils.ReportInfoc(InfocTable.rollingsky2_games_ads, info);
			}
		}

		protected override void OnClose(object userData)
		{
			base.OnClose(userData);
			if ((bool)m_giftPackageController)
			{
				m_giftPackageController.OnReset();
			}
			Reset();
		}

		protected override void OnUnload()
		{
			base.OnUnload();
			if ((bool)m_giftPackageController)
			{
				m_giftPackageController.Release();
			}
		}

		public void Init()
		{
			Effect_zhizhen_start.SetActive(false);
			Effect_zhizhen_end.SetActive(false);
			if (DeviceManager.Instance.IsNeedSpecialAdapte())
			{
				MonoSingleton<GameTools>.Instacne.AdapteSpecialScreen(m_topLeft as RectTransform);
			}
			addLiveNumTxt.text = string.Format("+{0}", DEFAULT_ADD_BALLS);
			getLivesAdLabel.text = string.Format(Mod.Localization.GetInfoById(62), GameCommon.powerRecoverNum_byAd);
			if ((bool)m_giftPackageController)
			{
				m_giftPackageController.Init();
			}
			BuyItemInfo();
			InitFunction();
		}

		private void BuyItemInfo()
		{
			Shops_shopTable shops_shopTable = Mod.DataTable.Get<Shops_shopTable>()[10002];
			if (shops_shopTable != null)
			{
				Dictionary<int, int>.Enumerator enumerator = MonoSingleton<GameTools>.Instacne.DealGoodsTeamById(shops_shopTable.GoodsTeamid).GetEnumerator();
				int powerNum = 0;
				int itemPrice2 = 0;
				while (enumerator.MoveNext())
				{
					powerNum = enumerator.Current.Value;
					getLivesNumLabel.text = string.Format(Mod.Localization.GetInfoById(61), powerNum);
				}
				itemPrice2 = int.Parse(shops_shopTable.Price);
				getLivesShopDiscountLabel.text = MonoSingleton<GameTools>.Instacne.GetProductRealPrice(10002);
				getLivesShopLabel.text = float.Parse(shops_shopTable.Price).ToString();
				buyItemAction = delegate
				{
					playerData = Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule);
					CommonAlertData commonAlertData2 = new CommonAlertData
					{
						showType = CommonAlertData.AlertShopType.BUY_SHOPITEM
					};
					int levelId2 = 0;
					if (showData.openFrom == TimeOutOpenFrom.ResultForm)
					{
						levelId2 = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).CurLevelId;
					}
					commonAlertData2.shopItemId = 10002;
					commonAlertData2.lableContent = Mod.Localization.GetInfoById(21);
					commonAlertData2.alertContent = Mod.Localization.GetInfoById(59);
					commonAlertData2.callBackFunc = delegate
					{
						double playGoodsNum = playerData.GetPlayGoodsNum(6);
						InfocUtils.Report_rollingsky2_games_neigou(10002, 2, levelId2);
						if (playGoodsNum >= (double)itemPrice2)
						{
							InfocUtils.Report_rollingsky2_games_neigou(10002, 3, levelId2);
							AddPowerNum(powerNum, AssertChangeType.SHOP_KEY);
							playerData.ChangePlayerGoodsNum(6, -1 * itemPrice2);
							Mod.UI.CloseUIForm(UIFormId.CommonAlertForm);
							InfocUtils.Report_rollingsky2_games_currency(1, 1, 6);
						}
						else
						{
							InfocUtils.Report_rollingsky2_games_neigou(10002, 4, levelId2);
						}
					};
					Mod.UI.OpenUIForm(UIFormId.CommonAlertForm, commonAlertData2);
				};
			}
			shops_shopTable = Mod.DataTable.Get<Shops_shopTable>()[10003];
			if (shops_shopTable != null)
			{
				Dictionary<int, int>.Enumerator enumerator2 = MonoSingleton<GameTools>.Instacne.DealGoodsTeamById(shops_shopTable.GoodsTeamid).GetEnumerator();
				while (enumerator2.MoveNext())
				{
					int key = enumerator2.Current.Key;
				}
				m_AutoBuyTimeItemAction = delegate
				{
					playerData = Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule);
					CommonAlertData userData = new CommonAlertData
					{
						showType = CommonAlertData.AlertShopType.BUY_SHOPITEM,
						shopItemId = 10003,
						lableContent = Mod.Localization.GetInfoById(21),
						alertContent = Mod.Localization.GetInfoById(71),
						callBackFunc = delegate
						{
							Mod.UI.CloseUIForm(UIFormId.CommonAlertForm);
							MonoSingleton<GameTools>.Instacne.CommonBuyOperate(10003);
						}
					};
					Mod.UI.OpenUIForm(UIFormId.CommonAlertForm, userData);
				};
			}
			shops_shopTable = Mod.DataTable.Get<Shops_shopTable>()[10004];
			if (shops_shopTable == null)
			{
				return;
			}
			Dictionary<int, int>.Enumerator enumerator3 = MonoSingleton<GameTools>.Instacne.DealGoodsTeamById(shops_shopTable.GoodsTeamid).GetEnumerator();
			int goodsNUm = 0;
			int itemPrice = 0;
			Goods_goodsTable goods_goodsTable = null;
			int goodsId = 0;
			while (enumerator3.MoveNext())
			{
				goodsId = enumerator3.Current.Key;
				goods_goodsTable = Mod.DataTable.Get<Goods_goodsTable>()[goodsId];
				if (goods_goodsTable != null && goods_goodsTable.FunctionType == 1)
				{
					int functionNum = goods_goodsTable.FunctionNum;
					m_timeLivesNumLabel.text = string.Format(Mod.Localization.GetInfoById(66), goods_goodsTable.FunctionNum);
				}
				goodsNUm = enumerator3.Current.Value;
			}
			itemPrice = int.Parse(shops_shopTable.Price);
			m_buyTimeItemAction = delegate
			{
				playerData = Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule);
				CommonAlertData commonAlertData = new CommonAlertData
				{
					showType = CommonAlertData.AlertShopType.BUY_SHOPITEM,
					shopItemId = 10004,
					lableContent = Mod.Localization.GetInfoById(21)
				};
				int levelId = 0;
				if (showData.openFrom == TimeOutOpenFrom.ResultForm)
				{
					levelId = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).CurLevelId;
				}
				commonAlertData.callBackFunc = delegate
				{
					int num = (int)playerData.GetPlayGoodsNum(6);
					InfocUtils.Report_rollingsky2_games_neigou(10004, 2, levelId);
					if (num >= itemPrice)
					{
						InfocUtils.Report_rollingsky2_games_neigou(10004, 3, levelId);
						playerData.ChangePlayerGoodsNum(6, -1 * itemPrice);
						playerData.ChangePlayerGoodsNum(goodsId, goodsNUm);
						OnBuyTimePowerSuccess(goodsId, goodsNUm, true);
						InfocUtils.Report_rollingsky2_games_currency(goodsId, 1, 0);
						Mod.UI.CloseUIForm(UIFormId.CommonAlertForm);
					}
					else
					{
						InfocUtils.Report_rollingsky2_games_neigou(10004, 4, levelId);
					}
				};
				Mod.UI.OpenUIForm(UIFormId.CommonAlertForm, commonAlertData);
			};
		}

		private void InitFunction()
		{
			m_giveTimeItemAction = delegate
			{
				Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).ChangePlayerGoodsNum(GameCommon.timeOutFormGiveGoodsId, 1.0);
				GetGoodsData userData = new GetGoodsData
				{
					GoodsId = GameCommon.timeOutFormGiveGoodsId,
					GoodsNum = 1.0,
					Buy = false,
					CallBackFunc = delegate
					{
						ReGetPowerHandler();
					}
				};
				Mod.UI.OpenUIForm(UIFormId.GetGoodsForm, userData);
			};
		}

		public void Begin()
		{
			AddEventListener();
			addLiveContainer.SetActive(false);
			clockTimeLabel.gameObject.SetActive(true);
			ShowLivesLeft();
			playerData = Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule);
			TimerHeap.DelTimer(timerEndId);
			Effect_zhizhen_end.SetActive(false);
			Effect_zhizhen_start.SetActive(true);
			m_ClockEffect.SetActive(false);
			SetClockData();
			isReplay = false;
			Show();
		}

		private void SetClockData()
		{
			TimerHeap.DelTimer(timerId);
			playerData.RecordPower();
			restTime = (float)playerData.PowerCdGoing() * 0.001f;
			timerId = TimerHeap.AddTimer(0u, 100u, TimerHandler);
		}

		private void Reset()
		{
			RemoveEventListener();
			TimerHeap.DelTimer(timerId);
			TimerHeap.DelTimer(timerEndId);
			TimerHeap.DelTimer(adTimerId);
			isAddBallByAd = false;
			m_isRealMoneyBuy = false;
			StopAllCoroutines();
		}

		private void SetAdButtonState()
		{
			if (PlayerDataModule.Instance.PlayerRecordData.WatchPowerAdNum > 0 && MonoSingleton<GameTools>.Instacne.CanShowAd(adScene))
			{
				m_adGray.SetGrey(false);
				TimerHeap.DelTimer(adTimerId);
				m_adAlertTxt.gameObject.SetActive(false);
				m_adIcon.transform.localPosition = Vector3.zero;
			}
			else
			{
				m_adGray.SetGrey(true);
				m_adAlertTxt.gameObject.SetActive(true);
				m_adIcon.transform.localPosition = new Vector3(m_adIconSourcePosX, 0f, 0f);
			}
		}

		public void Show()
		{
			PlayerDataModule.Instance.PlayerRecordData.TimeOutFormOpenTime++;
			SetAdButtonState();
			adTimerId = TimerHeap.AddTimer((uint)(GameCommon.COMMON_AD_REFRESHTIME * 1000f), (uint)(GameCommon.COMMON_AD_REFRESHTIME * 1000f), SetAdButtonState);
		}

		private void AddEventListener()
		{
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(timeMenuBackBtn);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(OnMenuBackBtnClick));
			EventTriggerListener eventTriggerListener2 = EventTriggerListener.Get(getLivesAdBtn.gameObject);
			eventTriggerListener2.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener2.onClick, new EventTriggerListener.VoidDelegate(OnLivesAdBtnClick));
			EventTriggerListener eventTriggerListener3 = EventTriggerListener.Get(getLivesShopBtn);
			eventTriggerListener3.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener3.onClick, new EventTriggerListener.VoidDelegate(OnLivesShopBtnClick));
			EventTriggerListener eventTriggerListener4 = EventTriggerListener.Get(m_getTimeLiveBtn);
			eventTriggerListener4.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener4.onClick, new EventTriggerListener.VoidDelegate(OnTimeLiveClick));
			Mod.Event.Subscribe(EventArgs<GameGoodsNumChangeEventArgs>.EventId, OnPlayerAssetChange);
			Mod.Event.Subscribe(EventArgs<UIMod.CloseCompleteEventArgs>.EventId, OnCloseUIFormSuccess);
		}

		private void OnCloseUIFormSuccess(object sender, Foundation.EventArgs e)
		{
			string assetName = ((UIMod.CloseCompleteEventArgs)e).AssetName;
			string uIFormAsset = AssetUtility.GetUIFormAsset(Mod.DataTable.Get<UIForms_uiformTable>().Get(27).AssetName);
			if (assetName.Equals(uIFormAsset) && m_isRealMoneyBuy)
			{
				ReGetPowerHandler();
			}
		}

		private void OnTimeLiveClick(GameObject go)
		{
			if (m_buyTimeItemAction != null)
			{
				m_buyTimeItemAction();
			}
		}

		private void RemoveEventListener()
		{
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(timeMenuBackBtn);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(OnMenuBackBtnClick));
			EventTriggerListener eventTriggerListener2 = EventTriggerListener.Get(getLivesAdBtn.gameObject);
			eventTriggerListener2.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener2.onClick, new EventTriggerListener.VoidDelegate(OnLivesAdBtnClick));
			EventTriggerListener eventTriggerListener3 = EventTriggerListener.Get(getLivesShopBtn);
			eventTriggerListener3.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener3.onClick, new EventTriggerListener.VoidDelegate(OnLivesShopBtnClick));
			EventTriggerListener eventTriggerListener4 = EventTriggerListener.Get(m_getTimeLiveBtn);
			eventTriggerListener4.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener4.onClick, new EventTriggerListener.VoidDelegate(OnTimeLiveClick));
			Mod.Event.Unsubscribe(EventArgs<GameGoodsNumChangeEventArgs>.EventId, OnPlayerAssetChange);
			Mod.Event.Unsubscribe(EventArgs<UIMod.CloseCompleteEventArgs>.EventId, OnCloseUIFormSuccess);
		}

		private void OnPlayerAssetChange(object sender, Foundation.EventArgs e)
		{
			GameGoodsNumChangeEventArgs gameGoodsNumChangeEventArgs = e as GameGoodsNumChangeEventArgs;
			if (gameGoodsNumChangeEventArgs == null || gameGoodsNumChangeEventArgs.GoodsId != 1)
			{
				return;
			}
			ShowLivesLeft();
			TimerHeap.DelTimer(timerId);
			clockTimeLabel.text = "";
			clockTimeLabel.gameObject.SetActive(false);
			if (gameGoodsNumChangeEventArgs.ChangeType == AssertChangeType.TIME)
			{
				if (base.gameObject.activeSelf)
				{
					m_ClockEffect.SetActive(true);
					StartCoroutine(TimeEndHandler(2f, (int)gameGoodsNumChangeEventArgs.ChangeNum, false));
				}
			}
			else if (gameGoodsNumChangeEventArgs.ChangeType == AssertChangeType.AD)
			{
				ShowGetPowerForm((int)gameGoodsNumChangeEventArgs.ChangeNum, false);
			}
			else if (gameGoodsNumChangeEventArgs.ChangeType == AssertChangeType.SHOP_KEY)
			{
				ShowGetPowerForm((int)gameGoodsNumChangeEventArgs.ChangeNum, true);
			}
			else if (gameGoodsNumChangeEventArgs.ChangeType == AssertChangeType.SHOP_REALMONEY)
			{
				m_isRealMoneyBuy = true;
				Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).ClearPowerCDStartTimestamp();
			}
		}

		private IEnumerator TimeEndHandler(float timedelty, int changeNum, bool isBuy)
		{
			yield return new WaitForSeconds(timedelty);
			ShowGetPowerForm(changeNum, isBuy);
		}

		private void ShowLivesLeft()
		{
			int num = (int)Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).GetPlayGoodsNum(1);
			remainLivesLabel.text = string.Format(Mod.Localization.GetInfoById(12), num);
		}

		private void SetClock(float restTime)
		{
			clockTimeLabel.text = string.Format("{0}s", Mathf.CeilToInt(restTime));
			float fillAmount = 1f - restTime / TIME_TO_COUNT;
			clockBg.fillAmount = fillAmount;
		}

		public void OnMenuBackBtnClick(GameObject gameObj)
		{
			Mod.UI.CloseUIForm(UIFormId.TimeOutDialogForm);
			if (showData.openFrom == TimeOutOpenFrom.ResultForm)
			{
				HomeForm.CurrentSeriesId = PlayerDataModule.Instance.LastEndterLevelData.SeriesId;
				Mod.UI.CloseUIForm(UIFormId.ResultForm);
				Mod.Event.Fire(this, Mod.Reference.Acquire<GameExitEventArgs>());
			}
		}

		public void OnLivesAdBtnClick(GameObject gameObj)
		{
			if (MonoSingleton<GameTools>.Instacne.CanShowAd(adScene) && PlayerDataModule.Instance.PlayerRecordData.WatchPowerAdNum > 0)
			{
				string info = string.Format("scenes=1&scenes_source={0}&ad_source=0&action=3&source=0&level=0", (int)adScene);
				InfocUtils.ReportInfoc(InfocTable.rollingsky2_games_ads, info);
				if (!(Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).GetPlayGoodsNum(1) > 0.0))
				{
					MonoSingleton<GameTools>.Instacne.PlayerVideoAd(adScene, OnAdSuccess);
				}
			}
		}

		public void OnLivesShopBtnClick(GameObject gameObj)
		{
			if (buyItemAction != null)
			{
				buyItemAction();
			}
		}

		private void TimerHandler()
		{
			restTime -= 0.1f;
			if (restTime < 0f)
			{
				SetClock(0f);
				Effect_zhizhen_start.SetActive(false);
				TimerHeap.DelTimer(timerId);
			}
			else
			{
				SetClock(restTime);
			}
		}

		private void AddPowerNum(int val, AssertChangeType type)
		{
			Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).ChangePlayerGoodsNum(1, val, type);
			Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).ClearPowerCDStartTimestamp();
		}

		private void CloseUI()
		{
			Mod.UI.CloseUIForm(UIFormId.TimeOutDialogForm);
		}

		private void OnAdSuccess(ADScene adScen = ADScene.LevelLockView)
		{
			PlayerDataModule.Instance.PlayerRecordData.WatchPowerAdNum--;
			if (Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).PlayerRecordData.firstPlayAD_GetPower == 0)
			{
				Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).PlayerRecordData.firstPlayAD_GetPower = 1;
				Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).PlayerRecordData.SaveToLocal();
				if (MonoSingleton<GameTools>.Instacne.IsFirstLanuchGameIn24Hour())
				{
					InfocUtils.Report_rollingsky2_games_gamefiller(5);
				}
			}
			string info = string.Format("scenes=1&scenes_source={0}&ad_source=0&action=4&source=0&level=0", (int)adScene);
			InfocUtils.ReportInfoc(InfocTable.rollingsky2_games_ads, info);
			isAddBallByAd = true;
			AddPowerNum(GameCommon.powerRecoverNum_byAd, AssertChangeType.AD);
		}

		private void OnBuyTimePowerSuccess(int goodsId, int goodsNum, bool isBuy)
		{
			GetGoodsData getGoodsData = new GetGoodsData();
			getGoodsData.GoodsId = goodsId;
			getGoodsData.GoodsNum = goodsNum;
			getGoodsData.Buy = isBuy;
			getGoodsData.CallBackFunc = delegate
			{
				ReGetPowerHandler();
			};
			Mod.UI.OpenUIForm(UIFormId.GetGoodsForm, getGoodsData);
		}

		private void ReGetPowerHandler()
		{
			CloseUI();
			if (CurrentLevelId <= 0)
			{
				return;
			}
			if (showData.openFrom == TimeOutOpenFrom.ResultForm)
			{
				if (showData.isRebirth)
				{
					ReplayFromRebirthPoint(showData.isFirst);
				}
				else
				{
					ReplayLevelFromStart();
				}
			}
			else
			{
				EnterLevelById(CurrentLevelId);
			}
		}

		private void ShowGetPowerForm(int goodsNum, bool isBuy)
		{
			GetGoodsData getGoodsData = new GetGoodsData();
			getGoodsData.GoodsId = 1;
			getGoodsData.GoodsNum = goodsNum;
			getGoodsData.Buy = isBuy;
			getGoodsData.CallBackFunc = delegate
			{
				ReGetPowerHandler();
			};
			Mod.UI.OpenUIForm(UIFormId.GetGoodsForm, getGoodsData);
		}

		private void OnAdFailed()
		{
			BroadCastData broadCastData = new BroadCastData();
			broadCastData.Type = BroadCastType.INFO;
			broadCastData.Info = "Play failed,try again later!";
			MonoSingleton<BroadCastManager>.Instacne.BroadCast(broadCastData);
		}

		private void NoAdPlayer()
		{
			BroadCastData broadCastData = new BroadCastData();
			broadCastData.Type = BroadCastType.INFO;
			broadCastData.Info = "Preparing,try again later!";
			MonoSingleton<BroadCastManager>.Instacne.BroadCast(broadCastData);
		}

		private void EnterLevelById(int levelId)
		{
			MenuProcedure menuProcedure = Mod.Procedure.Current as MenuProcedure;
			if (menuProcedure != null)
			{
				Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).CurLevelId = levelId;
				int sceneIdByLevelId = MonoSingleton<GameTools>.Instacne.GetSceneIdByLevelId(levelId);
				menuProcedure.WillToScene(sceneIdByLevelId);
			}
		}

		private void ReplayLevelFromStart()
		{
			if (!isReplay)
			{
				isReplay = true;
				Mod.UI.CloseLoadedUIForms();
				Mod.Event.Fire(this, Mod.Reference.Acquire<GameResetEventArgs>());
				Mod.UI.OpenUIForm(UIFormId.TutorialGameStartForm);
				Mod.UI.OpenUIForm(UIFormId.RuntimeForm);
				new LevelStartForm.ShowData().showRebirth = true;
			}
		}

		private void ReplayFromRebirthPoint(bool isFirst = false)
		{
			InsideGameDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule);
			dataModule.SetBuyOutRebirthData(isFirst);
			if (isFirst)
			{
				dataModule.ResetFirstBuyOutRebirth();
			}
			Mod.Event.Fire(this, Mod.Reference.Acquire<GameOriginRebirthResetEventArgs>());
			Mod.UI.CloseUIForm(UIFormId.ResultForm);
		}

		private IEnumerator OnApplicationPause(bool pauseState)
		{
			yield return null;
			if (!pauseState)
			{
				SetClockData();
			}
		}

		private void PauseMusic()
		{
		}

		private void ResumeMusic()
		{
		}

		protected override void OnCover()
		{
			base.OnCover();
			effectCamera.SetActive(false);
			for (int i = 0; i < m_particleSystems.Count; i++)
			{
				m_particleSystems[i].sortingOrder = 100;
			}
		}

		protected override void OnReveal()
		{
			base.OnReveal();
			effectCamera.SetActive(true);
			for (int i = 0; i < m_particleSystems.Count; i++)
			{
				m_particleSystems[i].sortingOrder = 30000;
			}
		}

		private void OnDestroy()
		{
			Reset();
		}
	}
}
