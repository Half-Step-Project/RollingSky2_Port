using System;
using System.Collections;
using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.UI;

public class CommonAlertItemBuyController : CommonAlertItemController
{
	public CommonAlertData.AlertShopType m_showType = CommonAlertData.AlertShopType.BUY_SHOPITEM;

	private CommonAlertData m_AlertData;

	public GameObject buyBtn;

	public GameObject discountBuyBtn;

	public Text numTxt;

	public Text priceTxt;

	public Text priceConterTxt;

	public Text numAlertTxt;

	public Image moneyIcon;

	public Image goodsIcon;

	public Image discountMoneyIcon;

	public Text discountRealTxt;

	public Text discountPriceTxt;

	public Text m_goodsTeamDesc;

	public List<GameObject> tenList = new List<GameObject>();

	public List<GameObject> geList = new List<GameObject>();

	private List<object> loadedAsserts = new List<object>();

	private int m_currentGoldId = -1;

	private int m_needNm;

	private bool m_isReleased;

	private void OnDestroy()
	{
		m_isReleased = true;
	}

	private void ChangePriceColor(int itemPrice)
	{
		if (PlayerDataModule.Instance.GetPlayGoodsNum(m_currentGoldId) >= (double)itemPrice)
		{
			priceTxt.color = new Color(1f, 0.95f, 0.6f, 1f);
			discountRealTxt.color = new Color(1f, 0.95f, 0.6f, 1f);
		}
		else
		{
			priceTxt.color = GameCommon.COMMON_RED;
			discountRealTxt.color = GameCommon.COMMON_RED;
		}
	}

	public override void Init(CommonAlertData alertData)
	{
		m_AlertData = alertData;
		AddEventHandler();
		m_isReleased = false;
		if (alertData.shopItemId <= 0)
		{
			return;
		}
		Shops_shopTable shops_shopTable = Mod.DataTable.Get<Shops_shopTable>()[alertData.shopItemId];
		if (shops_shopTable == null)
		{
			return;
		}
		GoodsTeam_goodsTeamTable goodsTeam_goodsTeamTable = Mod.DataTable.Get<GoodsTeam_goodsTeamTable>()[shops_shopTable.GoodsTeamid];
		if (goodsTeam_goodsTeamTable != null && goodsTeam_goodsTeamTable.Desc > 0)
		{
			m_goodsTeamDesc.text = Mod.Localization.GetInfoById(goodsTeam_goodsTeamTable.Desc);
		}
		else
		{
			m_goodsTeamDesc.text = "";
		}
		Dictionary<int, int>.Enumerator enumerator = MonoSingleton<GameTools>.Instacne.DealGoodsTeamById(shops_shopTable.GoodsTeamid).GetEnumerator();
		int num = 0;
		int num2 = 0;
		string text = "";
		while (enumerator.MoveNext())
		{
			num = enumerator.Current.Value;
		}
		bool flag = false;
		int num3 = 10006;
		switch (shops_shopTable.BuyType)
		{
		case 4:
			m_currentGoldId = 3;
			num3 = 10000;
			num2 = int.Parse(shops_shopTable.Price);
			text = num2.ToString();
			break;
		case 2:
			m_currentGoldId = 6;
			num3 = 10006;
			num2 = int.Parse(shops_shopTable.Price);
			text = num2.ToString();
			break;
		case 1:
			flag = true;
			num = shops_shopTable.Count;
			text = MonoSingleton<GameTools>.Instacne.GetProductRealPrice(alertData.shopItemId);
			break;
		}
		if (alertData.isBuyBySinglePrice)
		{
			num2 = num2 / num * alertData.itemNum;
			numTxt.text = string.Format("X{0}", alertData.itemNum);
			text = num2.ToString();
		}
		else
		{
			numTxt.text = string.Format("X{0}", num);
		}
		if (flag)
		{
			priceTxt.gameObject.SetActive(false);
			priceConterTxt.gameObject.SetActive(true);
			priceConterTxt.text = text;
		}
		else
		{
			priceTxt.gameObject.SetActive(true);
			priceConterTxt.gameObject.SetActive(false);
			priceTxt.text = text;
		}
		discountRealTxt.text = num2.ToString();
		m_needNm = num2;
		if (!flag)
		{
			ChangePriceColor(num2);
		}
		if (shops_shopTable.Discount > 0)
		{
			buyBtn.SetActive(false);
			discountBuyBtn.SetActive(true);
			int num4 = 100 - shops_shopTable.Discount;
			int ten = num4 / 10;
			int ge = num4 % 10;
			ShowDiscountNum(ten, ge);
			float num5 = num2 * 100 / shops_shopTable.Discount;
			int num6 = (int)num5;
			if (num5 - (float)num6 > 0.001f)
			{
				discountPriceTxt.text = num5.ToString("0.00");
			}
			else
			{
				discountPriceTxt.text = num6.ToString();
			}
		}
		else
		{
			buyBtn.SetActive(true);
			discountBuyBtn.SetActive(false);
		}
		string empty = string.Empty;
		empty = ((!string.IsNullOrEmpty(alertData.alertContent)) ? alertData.alertContent : ((shops_shopTable.Name != -1) ? Mod.Localization.GetInfoById(shops_shopTable.Name) : Mod.Localization.GetInfoById(39)));
		numAlertTxt.text = empty;
		if (!flag)
		{
			moneyIcon.gameObject.SetActive(true);
			string moneyspriteName = num3.ToString();
			Mod.Resource.LoadAsset(AssetUtility.GetGameIconAsset(moneyspriteName), new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
			{
				if (!(base.gameObject == null) && !(moneyIcon == null) && !(discountMoneyIcon == null) && !(MonoSingleton<GameTools>.Instacne == null))
				{
					if (m_isReleased)
					{
						Mod.Resource.UnloadAsset(asset);
					}
					else
					{
						if (moneyIcon != null)
						{
							moneyIcon.gameObject.SetActive(true);
							moneyIcon.sprite = MonoSingleton<GameTools>.Instacne.CreateSpriteByTexture2D(asset as Texture2D);
						}
						if (discountMoneyIcon != null)
						{
							discountMoneyIcon.gameObject.SetActive(true);
							discountMoneyIcon.sprite = MonoSingleton<GameTools>.Instacne.CreateSpriteByTexture2D(asset as Texture2D);
						}
						loadedAsserts.Add(asset);
					}
				}
			}, delegate(string assetName, string errorMessage, object data2)
			{
				Log.Error(string.Format("Can not load sprite '{0}' from '{1}' with error message '{2}'.", moneyspriteName, assetName, errorMessage));
			}));
		}
		else
		{
			moneyIcon.gameObject.SetActive(false);
		}
		string goodsSpriteName = shops_shopTable.IconId.ToString();
		Mod.Resource.LoadAsset(AssetUtility.GetGameIconAsset(goodsSpriteName), new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
		{
			if (!(base.gameObject == null) && !(goodsIcon == null) && !(MonoSingleton<GameTools>.Instacne == null))
			{
				if (m_isReleased)
				{
					Mod.Resource.UnloadAsset(asset);
				}
				else
				{
					if (goodsIcon != null)
					{
						goodsIcon.gameObject.SetActive(true);
						goodsIcon.sprite = MonoSingleton<GameTools>.Instacne.CreateSpriteByTexture2D(asset as Texture2D);
						goodsIcon.SetNativeSize();
					}
					loadedAsserts.Add(asset);
				}
			}
		}, delegate(string assetName, string errorMessage, object data2)
		{
			Log.Error(string.Format("Can not load sprite '{0}' from '{1}' with error message '{2}'.", goodsSpriteName, assetName, errorMessage));
		}));
	}

	private void AddEventHandler()
	{
		EventTriggerListener eventTriggerListener = EventTriggerListener.Get(buyBtn);
		eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(BuyBtnClickHandler));
		EventTriggerListener eventTriggerListener2 = EventTriggerListener.Get(discountBuyBtn);
		eventTriggerListener2.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener2.onClick, new EventTriggerListener.VoidDelegate(BuyBtnClickHandler));
		Mod.Event.Subscribe(EventArgs<GameGoodsNumChangeEventArgs>.EventId, OnPlayerAssetChange);
	}

	private void RemoveEventHandler()
	{
		EventTriggerListener eventTriggerListener = EventTriggerListener.Get(buyBtn);
		eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(BuyBtnClickHandler));
		EventTriggerListener eventTriggerListener2 = EventTriggerListener.Get(discountBuyBtn);
		eventTriggerListener2.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener2.onClick, new EventTriggerListener.VoidDelegate(BuyBtnClickHandler));
		Mod.Event.Unsubscribe(EventArgs<GameGoodsNumChangeEventArgs>.EventId, OnPlayerAssetChange);
	}

	private void OnPlayerAssetChange(object sender, Foundation.EventArgs e)
	{
		GameGoodsNumChangeEventArgs gameGoodsNumChangeEventArgs = e as GameGoodsNumChangeEventArgs;
		if (gameGoodsNumChangeEventArgs != null && gameGoodsNumChangeEventArgs.GoodsId == m_currentGoldId)
		{
			ChangePriceColor(m_needNm);
		}
	}

	private void BuyBtnClickHandler(GameObject obj)
	{
		if (m_AlertData != null)
		{
			m_AlertData.callBackFunc();
		}
	}

	private IEnumerator DynamicLayout()
	{
		yield return new WaitForEndOfFrame();
		goodsIcon.rectTransform.localPosition = new Vector2(numTxt.rectTransform.localPosition.x + numTxt.rectTransform.rect.width + goodsIcon.rectTransform.rect.width * 0.5f, numTxt.rectTransform.localPosition.y);
		yield return null;
	}

	private void ShowDiscountNum(int ten, int ge)
	{
		for (int i = 0; i < 10; i++)
		{
			if (i == ten)
			{
				tenList[i].SetActive(true);
			}
			else
			{
				tenList[i].SetActive(false);
			}
			if (i == ge)
			{
				geList[i].SetActive(true);
			}
			else
			{
				geList[i].SetActive(false);
			}
		}
	}

	public override void Release()
	{
		for (int i = 0; i < loadedAsserts.Count; i++)
		{
			Mod.Resource.UnloadAsset(loadedAsserts[i]);
		}
		loadedAsserts.Clear();
	}

	public override void Reset()
	{
		m_currentGoldId = -1;
		RemoveEventHandler();
	}

	public override CommonAlertData.AlertShopType GetAlertType()
	{
		return m_showType;
	}
}
