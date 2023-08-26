using System;
using System.Collections.Generic;
using Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace RS2
{
	public class GiftPackageRecommandForm : UGUIForm
	{
		public GameObject root;

		private Text m_titleTxt;

		public Text m_giftPackageNameTxt;

		public Text m_giftPackageDesc;

		public Text m_packageNumTxt;

		public Image m_packageIcon;

		public Text m_originalPriceTxt;

		public Text m_discountPriceTxt;

		public Text m_limitBuyCountTxt;

		public GameObject m_buyBtn;

		public GameObject freeBtn;

		public List<RewardItemController> rewardList_3 = new List<RewardItemController>();

		public List<RewardItemController> rewardList_4 = new List<RewardItemController>();

		public GameObject discountRoot;

		public Image adSlider;

		public Text adSliderText;

		public Text adTips;

		private int m_giftPackageId;

		private AssetLoadCallbacks m_assetLoadCallBack;

		private List<object> loadedAsserts = new List<object>();

		private bool m_isInLimitCount = true;

		private Material m_backMatical;

		private const int LeaveFormPluginAdId = 7;

		private static int m_leaveFormTotalTime;

		private bool m_isBuy;

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			m_packageIcon.gameObject.SetActive(false);
			m_assetLoadCallBack = new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
			{
				m_packageIcon.gameObject.SetActive(true);
				m_packageIcon.sprite = MonoSingleton<GameTools>.Instacne.CreateSpriteByTexture2D(asset as Texture2D);
				loadedAsserts.Add(asset);
				m_packageIcon.gameObject.SetActive(true);
			}, delegate(string assetName, string errorMessage, object data2)
			{
				Log.Error(string.Format("Can not load item '{0}' from '{1}' with error message.", assetName, errorMessage));
			});
			InitUI();
		}

		private void OnUIFormClosed(object sender, Foundation.EventArgs e)
		{
			if ((e as UIFormCloseEvent).UIFormId == UIFormId.LevelEnterForm)
			{
				root.SetActive(true);
				DoOpen();
			}
		}

		private void DoOpen()
		{
			Shops_shopTable shops_shopTable = Mod.DataTable.Get<Shops_shopTable>()[m_giftPackageId];
			int goodsTeamid = shops_shopTable.GoodsTeamid;
			GoodsTeam_goodsTeamTable goodsTeam_goodsTeamTable = Mod.DataTable.Get<GoodsTeam_goodsTeamTable>()[goodsTeamid];
			int num = 0;
			m_packageNumTxt.text = string.Format("x{0}", shops_shopTable.Count);
			num = MonoSingleton<GameTools>.Instacne.GoodsTeamIconId(goodsTeam_goodsTeamTable.Id);
			m_giftPackageNameTxt.text = MonoSingleton<GameTools>.Instacne.GetGoodsTeamName(goodsTeam_goodsTeamTable.Id);
			m_originalPriceTxt.text = MonoSingleton<GameTools>.Instacne.GetProductRealPrice(shops_shopTable.Id);
			if (shops_shopTable.Discount > 0)
			{
				discountRoot.SetActive(true);
				int num2 = (int)(10000f / (float)shops_shopTable.Discount + 0.5f);
				m_discountPriceTxt.text = string.Format("{0}%MORE", num2);
			}
			else
			{
				discountRoot.SetActive(false);
			}
			if (shops_shopTable.LimitCount <= 0)
			{
				m_limitBuyCountTxt.gameObject.SetActive(false);
				m_isInLimitCount = true;
			}
			else
			{
				m_limitBuyCountTxt.gameObject.SetActive(true);
				int giftBuyNum = PlayerDataModule.Instance.PlayerGiftPackageData.GetGiftBuyNum(m_giftPackageId);
				if (giftBuyNum >= shops_shopTable.LimitCount)
				{
					m_isInLimitCount = false;
					m_limitBuyCountTxt.color = GameCommon.COMMON_RED;
				}
				else
				{
					m_isInLimitCount = true;
					m_limitBuyCountTxt.color = Color.green;
				}
				m_limitBuyCountTxt.text = string.Format("{0}/{1}", giftBuyNum, shops_shopTable.LimitCount);
			}
			RewardItemController rewardItemController = null;
			Dictionary<int, int> dictionary = MonoSingleton<GameTools>.Instacne.DealGoodsTeamById(goodsTeam_goodsTeamTable.Id);
			Dictionary<int, int>.Enumerator enumerator = dictionary.GetEnumerator();
			m_giftPackageDesc.text = MonoSingleton<GameTools>.Instacne.GetGoodsTeamDesc(goodsTeam_goodsTeamTable.Id).Replace("\\n", "\n");
			int num3 = 0;
			int count = dictionary.Count;
			while (enumerator.MoveNext())
			{
				if (count <= 3)
				{
					rewardItemController = rewardList_3[num3];
				}
				else if (count <= 4)
				{
					rewardItemController = rewardList_4[num3];
				}
				rewardItemController.gameObject.SetActive(true);
				rewardItemController.Init();
				rewardItemController.SetGoodsId(enumerator.Current.Key, enumerator.Current.Value * shops_shopTable.Count);
				num3++;
			}
			string assetName = num.ToString();
			Mod.Resource.LoadAsset(AssetUtility.GetGameIconAsset(assetName), m_assetLoadCallBack);
			int totalAdCount = PlayerDataModule.Instance.PlayerGiftPackageData.totalAdCount;
			if (shops_shopTable.AdFreeCount != -1 && totalAdCount >= shops_shopTable.AdFreeCount)
			{
				Vector3 localPosition = m_buyBtn.transform.localPosition;
				m_buyBtn.transform.localPosition = new Vector3(5000f, localPosition.y, localPosition.z);
			}
			else
			{
				Vector3 localPosition2 = freeBtn.transform.localPosition;
				freeBtn.transform.localPosition = new Vector3(5000f, localPosition2.y, localPosition2.z);
			}
			adSlider.fillAmount = (float)totalAdCount / (float)shops_shopTable.AdFreeCount;
			adSliderText.text = totalAdCount + "/" + shops_shopTable.AdFreeCount;
			adTips.text = string.Format(Mod.Localization.GetInfoById(218), Mathf.Max(0, shops_shopTable.AdFreeCount - totalAdCount));
			InfocUtils.Report_rollingsky2_games_pageshow(12, 0, 1);
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			m_giftPackageId = (int)userData;
			AddEventListener();
			if (Mod.UI.UIFormIsOpen(UIFormId.LevelEnterForm))
			{
				root.SetActive(false);
			}
			else
			{
				DoOpen();
			}
		}

		protected override void OnClose(object userData)
		{
			base.OnClose(userData);
			RemoveEventListener();
			for (int i = 0; i < rewardList_3.Count; i++)
			{
				rewardList_3[i].gameObject.SetActive(false);
			}
			for (int j = 0; j < rewardList_4.Count; j++)
			{
				rewardList_4[j].gameObject.SetActive(false);
			}
			Mod.Event.Fire(this, Mod.Reference.Acquire<GiftRecommandCloseEvent>());
			if (!m_isBuy)
			{
				DealLeaveformPluginAd();
			}
			ClearLeaveFormPluginAdData();
			Mod.Event.Fire(this, UIFormCloseEvent.Make(UIFormId.GiftPackageRecommandForm));
			Mod.Event.Fire(this, UIPopUpFormCloseEvent.Make(UIFormId.GiftPackageRecommandForm));
		}

		protected override void OnUnload()
		{
			base.OnUnload();
			m_backMatical = null;
			for (int i = 0; i < loadedAsserts.Count; i++)
			{
				Mod.Resource.UnloadAsset(loadedAsserts[i]);
			}
			loadedAsserts.Clear();
			for (int j = 0; j < rewardList_3.Count; j++)
			{
				rewardList_3[j].Release();
			}
			for (int k = 0; k < rewardList_4.Count; k++)
			{
				rewardList_4[k].Release();
			}
		}

		private void InitUI()
		{
			for (int i = 0; i < rewardList_3.Count; i++)
			{
				rewardList_3[i].gameObject.SetActive(false);
			}
			for (int j = 0; j < rewardList_4.Count; j++)
			{
				rewardList_4[j].gameObject.SetActive(false);
			}
		}

		public void AddEventListener()
		{
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(m_buyBtn);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(Buyhandler));
			Mod.Event.Subscribe(EventArgs<BuySuccessEventArgs>.EventId, BuySuccessHandler);
			Mod.Event.Subscribe(EventArgs<UIFormCloseEvent>.EventId, OnUIFormClosed);
		}

		private void BuySuccessHandler(object sender, Foundation.EventArgs e)
		{
			BuySuccessEventArgs buySuccessEventArgs = e as BuySuccessEventArgs;
			if (buySuccessEventArgs != null && buySuccessEventArgs.ShopItemId == m_giftPackageId)
			{
				Mod.UI.CloseUIForm(UIFormId.GiftPackageRecommandForm);
			}
		}

		public void OnClickFree()
		{
			int giftIdIndex = PlayerDataModule.Instance.PlayerGiftPackageData.GetGiftIdIndex(m_giftPackageId);
			InfocUtils.Report_rollingsky2_games_ads(17 + giftIdIndex, 0, 1, 0, 4, 0);
			MonoSingleton<GameTools>.Instacne.AddGoodsByShopTable(m_giftPackageId);
			MonoSingleton<GameTools>.Instacne.ShowBuyResult(true, m_giftPackageId, 1);
		}

		private void Buyhandler(GameObject go)
		{
			if (m_isInLimitCount)
			{
				m_isBuy = true;
				MonoSingleton<GameTools>.Instacne.CommonBuyOperate(m_giftPackageId);
				return;
			}
			CommonAlertData commonAlertData = new CommonAlertData();
			commonAlertData.showType = CommonAlertData.AlertShopType.COMMON;
			commonAlertData.alertContent = "Buy items reach the max count!";
			commonAlertData.callBackFunc = delegate
			{
				Mod.UI.CloseUIForm(UIFormId.CommonAlertForm);
			};
			Mod.UI.OpenUIForm(UIFormId.CommonAlertForm, commonAlertData);
		}

		private void OnCloseUIFormSuccess(object sender, Foundation.EventArgs e)
		{
			string assetName = ((UIMod.CloseCompleteEventArgs)e).AssetName;
			string uIFormAsset = AssetUtility.GetUIFormAsset(Mod.DataTable.Get<UIForms_uiformTable>().Get(6).AssetName);
			if (assetName.Equals(uIFormAsset))
			{
				base.gameObject.SetActive(true);
			}
		}

		public void RemoveEventListener()
		{
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(m_buyBtn);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(Buyhandler));
			Mod.Event.Unsubscribe(EventArgs<BuySuccessEventArgs>.EventId, BuySuccessHandler);
			Mod.Event.Unsubscribe(EventArgs<UIFormCloseEvent>.EventId, OnUIFormClosed);
		}

		public void CloseHandler()
		{
			Mod.UI.CloseUIForm(UIFormId.GiftPackageRecommandForm);
		}

		private void BroadResult(int goodsNum)
		{
			BroadCastData broadCastData = new BroadCastData();
			broadCastData.GoodId = 2;
			broadCastData.Type = BroadCastType.GOODS;
			broadCastData.Info = string.Format("+{0}", goodsNum);
			MonoSingleton<BroadCastManager>.Instacne.BroadCast(broadCastData);
		}

		private bool DealLeaveformPluginAd()
		{
			m_leaveFormTotalTime++;
			int id = MonoSingleton<GameTools>.Instacne.ComputerScreenPluginAdId(7);
			ScreenPluginsAd_table screenPluginsAd_table = Mod.DataTable.Get<ScreenPluginsAd_table>()[id];
			bool flag = false;
			if (screenPluginsAd_table != null)
			{
				flag |= m_leaveFormTotalTime >= screenPluginsAd_table.TriggerNum;
				if (flag)
				{
					flag &= PlayerDataModule.Instance.PluginAdController.IsScreenAdRead();
					if (flag)
					{
						PluginAdData pluginAdData = new PluginAdData();
						pluginAdData.PluginId = 7;
						pluginAdData.EndHandler = delegate
						{
							ClearLeaveFormPluginAdData();
						};
						Singleton<UIPopupManager>.Instance.PopupUI(UIFormId.ScreenPluginsForm, pluginAdData);
					}
				}
			}
			return flag;
		}

		private void ClearLeaveFormPluginAdData()
		{
			m_leaveFormTotalTime = 0;
			m_isBuy = false;
		}
	}
}
