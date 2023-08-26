using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace RS2
{
	public class GiftPackageBuyForm : UGUIForm
	{
		private GameObject back;

		private Text m_titleTxt;

		public Text m_giftPackageNameTxt;

		public Text m_giftPackageDesc;

		public GameObject m_closeBtn;

		public Text m_packageNumTxt;

		public Image m_packageIcon;

		public Text m_originalPriceTxt;

		public Text m_discountPriceTxt;

		public Text m_limitBuyCountTxt;

		public GameObject m_buyBtn;

		private int m_giftPackageId;

		private AssetLoadCallbacks m_assetLoadCallBack;

		private List<object> loadedAsserts = new List<object>();

		public RewardItemController m_rewardItem;

		public GameObject m_rewardContent;

		private List<RewardItemController> m_rewardList = new List<RewardItemController>();

		public DiscountController m_discountController;

		public Transform m_originalPricePos;

		public Vector3 m_discountOriginalPricePos;

		private bool m_isInLimitCount = true;

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

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			m_giftPackageId = (int)userData;
			Shops_shopTable shops_shopTable = Mod.DataTable.Get<Shops_shopTable>()[m_giftPackageId];
			int goodsTeamid = shops_shopTable.GoodsTeamid;
			GoodsTeam_goodsTeamTable goodsTeam_goodsTeamTable = Mod.DataTable.Get<GoodsTeam_goodsTeamTable>()[goodsTeamid];
			AddEventListener();
			int num = 0;
			m_packageNumTxt.text = string.Format("x{0}", shops_shopTable.Count);
			num = MonoSingleton<GameTools>.Instacne.GoodsTeamIconId(goodsTeam_goodsTeamTable.Id);
			m_giftPackageNameTxt.text = MonoSingleton<GameTools>.Instacne.GetGoodsTeamName(goodsTeam_goodsTeamTable.Id);
			string productRealPrice = MonoSingleton<GameTools>.Instacne.GetProductRealPrice(m_giftPackageId);
			m_originalPriceTxt.text = productRealPrice;
			m_discountController.SetDiscount(shops_shopTable.Discount);
			float num2 = 0f;
			if (shops_shopTable.Discount > 0)
			{
				m_originalPriceTxt.transform.localPosition = m_discountOriginalPricePos;
				m_discountPriceTxt.gameObject.SetActive(true);
				if (shops_shopTable.BuyType == 1)
				{
					string text = Regex.Replace(productRealPrice, "[^0-9.-]", "");
					int length = productRealPrice.IndexOf(text);
					productRealPrice.Substring(0, length);
					CultureInfo cultureInfo = (CultureInfo)CultureInfo.CurrentCulture.Clone();
					cultureInfo.NumberFormat.CurrencyDecimalSeparator = ".";
					num2 = float.Parse(text, NumberStyles.Any, cultureInfo);
					num2 = num2 * 100f / (float)shops_shopTable.Discount;
				}
				else
				{
					num2 = int.Parse(shops_shopTable.Price);
					num2 = num2 * 100f / (float)shops_shopTable.Discount;
				}
				int num3 = (int)num2;
				if (num2 - (float)num3 > 0.001f)
				{
					m_discountPriceTxt.text = num2.ToString("0.00");
				}
				else
				{
					m_discountPriceTxt.text = num3.ToString();
				}
			}
			else
			{
				m_originalPriceTxt.transform.localPosition = m_originalPricePos.localPosition;
				m_discountPriceTxt.gameObject.SetActive(false);
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
			GameObject gameObject = null;
			RewardItemController rewardItemController = null;
			m_rewardList.Clear();
			Dictionary<int, int>.Enumerator enumerator = MonoSingleton<GameTools>.Instacne.DealGoodsTeamById(goodsTeam_goodsTeamTable.Id).GetEnumerator();
			int i = 0;
			for (int childCount = m_rewardContent.transform.childCount; i < childCount; i++)
			{
				UnityEngine.Object.Destroy(m_rewardContent.transform.GetChild(i).gameObject);
			}
			while (enumerator.MoveNext())
			{
				gameObject = UnityEngine.Object.Instantiate(m_rewardItem.gameObject);
				rewardItemController = gameObject.GetComponent<RewardItemController>();
				gameObject.SetActive(true);
				m_rewardList.Add(rewardItemController);
				gameObject.transform.SetParent(m_rewardContent.transform);
				gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, 0f);
				gameObject.transform.localScale = Vector3.one;
				rewardItemController.Init();
				rewardItemController.SetGoodsId(enumerator.Current.Key, enumerator.Current.Value * shops_shopTable.Count);
			}
			m_rewardContent.SetActive(true);
			string assetName = num.ToString();
			Mod.Resource.LoadAsset(AssetUtility.GetGameIconAsset(assetName), m_assetLoadCallBack);
		}

		protected override void OnClose(object userData)
		{
			base.OnClose(userData);
			RemoveEventListener();
		}

		protected override void OnUnload()
		{
			base.OnUnload();
			for (int i = 0; i < m_rewardList.Count; i++)
			{
				m_rewardList[i].Release();
			}
			m_rewardList.Clear();
			for (int j = 0; j < loadedAsserts.Count; j++)
			{
				Mod.Resource.UnloadAsset(loadedAsserts[j]);
			}
			loadedAsserts.Clear();
		}

		private void InitUI()
		{
			Dictionary<string, GameObject> dictionary = ViewTools.CollectAllGameObjects(base.gameObject);
			back = dictionary["back"];
			m_discountOriginalPricePos = m_originalPriceTxt.transform.localPosition;
		}

		public void AddEventListener()
		{
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(m_closeBtn);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(CloseHandler));
			EventTriggerListener eventTriggerListener2 = EventTriggerListener.Get(m_buyBtn);
			eventTriggerListener2.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener2.onClick, new EventTriggerListener.VoidDelegate(Buyhandler));
			Mod.Event.Subscribe(EventArgs<BuySuccessEventArgs>.EventId, BuyPackageSuccessHandler);
		}

		private void Buyhandler(GameObject go)
		{
			if (m_isInLimitCount)
			{
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
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(m_closeBtn);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(CloseHandler));
			EventTriggerListener eventTriggerListener2 = EventTriggerListener.Get(m_buyBtn);
			eventTriggerListener2.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener2.onClick, new EventTriggerListener.VoidDelegate(Buyhandler));
			Mod.Event.Unsubscribe(EventArgs<BuySuccessEventArgs>.EventId, BuyPackageSuccessHandler);
		}

		private void CloseHandler(GameObject go)
		{
			Mod.UI.CloseUIForm(UIFormId.GiftPackageBuyForm);
		}

		private void BroadResult(int goodsNum)
		{
			BroadCastData broadCastData = new BroadCastData();
			broadCastData.GoodId = 2;
			broadCastData.Type = BroadCastType.GOODS;
			broadCastData.Info = string.Format("+{0}", goodsNum);
			MonoSingleton<BroadCastManager>.Instacne.BroadCast(broadCastData);
		}

		private void BuyPackageSuccessHandler(object sender, Foundation.EventArgs e)
		{
			if (e is BuySuccessEventArgs)
			{
				Mod.UI.CloseUIForm(UIFormId.GiftPackageBuyForm);
			}
		}

		private void ProgressInfo(int action)
		{
			InfocUtils.Report_rollingsky2_games_neigou(m_giftPackageId, action);
		}
	}
}
