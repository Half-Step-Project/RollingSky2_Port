using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace RS2
{
	public class MoneyShopForm : UGUIForm
	{
		public enum MoneyType
		{
			Money,
			Key,
			Power,
			RemoveAd
		}

		private Text m_titleTxt;

		public MoneyShopProductItemList m_moneyProductItemList;

		public MoneyShopProductItemList m_keyProductItemList;

		public MoneyShopProductItemList m_powerProductItemList;

		public MoneyShopRemoveAdList m_removeAdItemList;

		public GiftPackageController m_giftPackageController;

		private ShopResponseData m_moneyData;

		private ShopResponseData m_keyData;

		private ShopResponseData m_powerData;

		private ShopResponseData m_removeAdData;

		public GameObject[] m_removeAdNeedActives;

		private GameObject m_closeBtn;

		private MoneyType m_currentTypeID = MoneyType.Key;

		public MoneyType CurrentMoneyType
		{
			get
			{
				return m_currentTypeID;
			}
		}

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			Dictionary<string, GameObject> dictionary = ViewTools.CollectAllGameObjects(base.gameObject);
			m_closeBtn = dictionary["closeBtn"];
			m_titleTxt = dictionary["titleTxt"].GetComponent<Text>();
			LoadData();
			if ((bool)m_giftPackageController)
			{
				m_giftPackageController.Init();
			}
			if ((bool)m_removeAdItemList)
			{
				m_removeAdItemList.OnInit(m_removeAdData);
			}
		}

		protected override void OnOpen(object userData)
		{
			m_currentTypeID = ((userData == null) ? MoneyType.Key : ((MoneyType)userData));
			base.OnOpen(userData);
			AddEventListener();
			m_moneyProductItemList.OnInit(m_moneyData);
			m_keyProductItemList.OnInit(m_keyData);
			m_powerProductItemList.OnInit(m_powerData);
			m_moneyProductItemList.OnOpen(m_moneyData);
			m_keyProductItemList.OnOpen(m_keyData);
			m_powerProductItemList.OnOpen(m_powerData);
			if ((bool)m_giftPackageController)
			{
				int giftPackageID = 1;
				if (GetGiftPackageID(out giftPackageID))
				{
					m_giftPackageController.OnOpen();
					m_giftPackageController.SetData(giftPackageID);
				}
			}
			RefreshNetWorkMoneyData();
			InternalRefresh(m_currentTypeID);
		}

		protected override void OnTick(float elapseSeconds, float realElapseSeconds)
		{
			base.OnTick(elapseSeconds, realElapseSeconds);
			if ((bool)m_removeAdItemList)
			{
				m_removeAdItemList.OnTick(elapseSeconds, realElapseSeconds);
			}
			if ((bool)m_moneyProductItemList)
			{
				m_moneyProductItemList.OnTick(elapseSeconds, realElapseSeconds);
			}
			if ((bool)m_keyProductItemList)
			{
				m_keyProductItemList.OnTick(elapseSeconds, realElapseSeconds);
			}
			if ((bool)m_powerProductItemList)
			{
				m_powerProductItemList.OnTick(elapseSeconds, realElapseSeconds);
			}
		}

		private bool GetGiftPackageID(out int giftPackageID)
		{
			giftPackageID = 0;
			switch (m_currentTypeID)
			{
			case MoneyType.Money:
				giftPackageID = GameCommon.moneyGiftPackageShopID;
				break;
			case MoneyType.Power:
				giftPackageID = GameCommon.powerGiftPackageShopID;
				break;
			case MoneyType.RemoveAd:
				giftPackageID = GameCommon.removeAdGiftPackageShopID;
				break;
			case MoneyType.Key:
				giftPackageID = GameCommon.keyGiftPackageShopID;
				break;
			}
			return giftPackageID != 0;
		}

		private void InternalRefresh(MoneyType type)
		{
			m_currentTypeID = type;
			if (m_currentTypeID == MoneyType.Money)
			{
				closeAnim = m_moneyProductItemList.GetComponentInChildren<Animator>();
				m_moneyProductItemList.gameObject.SetActive(true);
				m_keyProductItemList.gameObject.SetActive(false);
				m_powerProductItemList.gameObject.SetActive(false);
				if ((bool)m_giftPackageController)
				{
					m_giftPackageController.gameObject.SetActive(true);
				}
				if ((bool)m_removeAdItemList)
				{
					m_removeAdItemList.gameObject.SetActive(false);
				}
				SetRemoveAdNeedActives(true);
			}
			else if (m_currentTypeID == MoneyType.Key)
			{
				closeAnim = m_keyProductItemList.GetComponentInChildren<Animator>();
				m_keyProductItemList.gameObject.SetActive(true);
				m_moneyProductItemList.gameObject.SetActive(false);
				m_powerProductItemList.gameObject.SetActive(false);
				if ((bool)m_giftPackageController)
				{
					m_giftPackageController.gameObject.SetActive(true);
				}
				if ((bool)m_removeAdItemList)
				{
					m_removeAdItemList.gameObject.SetActive(false);
				}
				SetRemoveAdNeedActives(true);
			}
			else if (m_currentTypeID == MoneyType.Power)
			{
				closeAnim = m_powerProductItemList.GetComponentInChildren<Animator>();
				m_powerProductItemList.gameObject.SetActive(true);
				m_keyProductItemList.gameObject.SetActive(false);
				m_moneyProductItemList.gameObject.SetActive(false);
				if ((bool)m_giftPackageController)
				{
					m_giftPackageController.gameObject.SetActive(true);
				}
				if ((bool)m_removeAdItemList)
				{
					m_removeAdItemList.gameObject.SetActive(false);
				}
				SetRemoveAdNeedActives(true);
			}
			else if (m_currentTypeID == MoneyType.RemoveAd)
			{
				if ((bool)m_removeAdItemList)
				{
					closeAnim = m_removeAdItemList.GetComponentInChildren<Animator>();
				}
				m_powerProductItemList.gameObject.SetActive(false);
				m_keyProductItemList.gameObject.SetActive(false);
				m_moneyProductItemList.gameObject.SetActive(false);
				if ((bool)m_giftPackageController)
				{
					m_giftPackageController.gameObject.SetActive(true);
				}
				if ((bool)m_removeAdItemList)
				{
					m_removeAdItemList.gameObject.SetActive(true);
				}
				if ((bool)m_removeAdItemList)
				{
					m_removeAdItemList.OnOpen();
				}
				SetRemoveAdNeedActives(false);
			}
		}

		protected void SetRemoveAdNeedActives(bool active)
		{
			for (int i = 0; i < m_removeAdNeedActives.Length; i++)
			{
				if ((bool)m_removeAdNeedActives[i])
				{
					m_removeAdNeedActives[i].SetActive(active);
				}
			}
		}

		protected override void OnClose(object userData)
		{
			base.OnClose(userData);
			RemoveEventListener();
			m_moneyProductItemList.OnClose();
			m_keyProductItemList.OnClose();
			if ((bool)m_giftPackageController)
			{
				m_giftPackageController.OnReset();
			}
			if ((bool)m_removeAdItemList)
			{
				m_removeAdItemList.OnClose();
			}
			Mod.Event.Fire(this, UIPopUpFormCloseEvent.Make(UIFormId.MoneyShopForm));
		}

		protected override void OnUnload()
		{
			base.OnUnload();
			m_moneyProductItemList.OnRelease();
			m_keyProductItemList.OnRelease();
			m_powerProductItemList.OnRelease();
			if ((bool)m_giftPackageController)
			{
				m_giftPackageController.Release();
			}
			if ((bool)m_removeAdItemList)
			{
				m_removeAdItemList.OnRelease();
			}
		}

		private void AddEventListener()
		{
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(m_closeBtn);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(OnClickCloseButton));
			Mod.Event.Subscribe(EventArgs<BuySuccessEventArgs>.EventId, BuyPackageSuccessHandler);
		}

		private void RemoveEventListener()
		{
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(m_closeBtn);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(OnClickCloseButton));
			Mod.Event.Unsubscribe(EventArgs<BuySuccessEventArgs>.EventId, BuyPackageSuccessHandler);
		}

		public void OnClickCloseButton(GameObject obj)
		{
		}

		public void OnClickClose()
		{
		}

		private void LoadData()
		{
			Shops_shopTable[] records = Mod.DataTable.Get<Shops_shopTable>().Records;
			m_moneyData = new ShopResponseData();
			m_keyData = new ShopResponseData();
			m_powerData = new ShopResponseData();
			m_removeAdData = new ShopResponseData();
			m_moneyData.shopItemList = new List<ShopItemData>();
			m_keyData.shopItemList = new List<ShopItemData>();
			m_powerData.shopItemList = new List<ShopItemData>();
			m_removeAdData.shopItemList = new List<ShopItemData>();
			foreach (Shops_shopTable shops_shopTable in records)
			{
				if ((shops_shopTable.BuyType == 2 && shops_shopTable.Type == 5) || (shops_shopTable.BuyType == 1 && shops_shopTable.Type == 5) || (shops_shopTable.BuyType == 3 && shops_shopTable.Type == 5))
				{
					if (shops_shopTable.BuyType == 2)
					{
						ShopItemData shopItemData = new ShopItemData();
						shopItemData.Init(shops_shopTable);
						m_keyData.shopItemList.Add(shopItemData);
					}
					else if (shops_shopTable.BuyType == 1 || shops_shopTable.BuyType == 3)
					{
						ShopItemData shopItemData2 = new ShopItemData();
						shopItemData2.Init(shops_shopTable);
						m_moneyData.shopItemList.Add(shopItemData2);
					}
				}
				if (GameCommon.removeAdMoneyShopIDs.Contains(shops_shopTable.Id))
				{
					ShopItemData shopItemData3 = new ShopItemData();
					shopItemData3.Init(shops_shopTable);
					m_removeAdData.shopItemList.Add(shopItemData3);
				}
				if (shops_shopTable.Type == 7 && shops_shopTable.Sort == 2)
				{
					m_powerData.shopItemList.Add(new ShopItemData(shops_shopTable));
				}
			}
			int num = GameCommon.removeAdMoneyShopIDs.Length;
			int count = m_removeAdData.shopItemList.Count;
			List<ShopItemData> list = new List<ShopItemData>();
			for (int j = 0; j < num; j++)
			{
				int num2 = GameCommon.removeAdMoneyShopIDs[j];
				for (int k = 0; k < count; k++)
				{
					ShopItemData shopItemData4 = m_removeAdData.shopItemList[k];
					if (shopItemData4.id == num2)
					{
						list.Add(shopItemData4);
						break;
					}
				}
			}
			m_removeAdData.shopItemList = list;
			m_moneyData.shopItemList.Sort((ShopItemData x, ShopItemData y) => x.shopSort - y.shopSort);
			RefreshNetWorkMoneyData();
		}

		public void Refresh(MoneyType data)
		{
			InternalRefresh(data);
		}

		private void RefreshNetWorkMoneyData()
		{
			ProductInfo productInfo = MonoSingleton<PluginManager>.Instacne.GetProductInfo();
			for (int i = 0; i < m_moneyData.shopItemList.Count; i++)
			{
				string product_id_ios = m_moneyData.shopItemList[i].product_id_ios;
				if (productInfo == null)
				{
					continue;
				}
				for (int j = 0; j < productInfo.productinInfoList.Count; j++)
				{
					if (productInfo.productinInfoList[j].productId == product_id_ios)
					{
						m_moneyData.shopItemList[i].price = productInfo.productinInfoList[j].price;
						break;
					}
				}
			}
			for (int k = 0; k < m_removeAdData.shopItemList.Count; k++)
			{
				string product_id_ios = m_removeAdData.shopItemList[k].product_id_ios;
				if (productInfo == null)
				{
					continue;
				}
				for (int l = 0; l < productInfo.productinInfoList.Count; l++)
				{
					if (productInfo.productinInfoList[l].productId == product_id_ios)
					{
						m_removeAdData.shopItemList[k].price = productInfo.productinInfoList[l].price;
						break;
					}
				}
			}
		}

		private void BuyPackageSuccessHandler(object sender, Foundation.EventArgs e)
		{
			BuySuccessEventArgs buySuccessEventArgs = e as BuySuccessEventArgs;
			if (buySuccessEventArgs != null)
			{
				switch (m_currentTypeID)
				{
				case MoneyType.RemoveAd:
					m_removeAdItemList.OnRefresh();
					m_removeAdItemList.BuyPackageSuccessHandler(sender, buySuccessEventArgs);
					break;
				case MoneyType.Money:
				case MoneyType.Key:
				case MoneyType.Power:
					break;
				}
			}
		}
	}
}
