using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.UI;

public class BufferShowItemController : UILoopItem
{
	private int m_goodsId = -1;

	public Image m_icon;

	public Text m_name;

	public Text m_desc;

	public GameObject m_selectFlag;

	public GameObject activeEffect;

	public Text m_originalPriceTxt;

	public Text m_discountPriceTxt;

	public Transform m_originalPricePos;

	public Vector3 m_discountOriginalPricePos;

	public GameObject m_buyBtn;

	private int m_giftPackageId;

	private bool m_isRelease;

	private bool m_isInit;

	private List<object> loadedAsserts = new List<object>();

	public override void Data(object data)
	{
		m_isRelease = false;
		m_goodsId = (int)data;
		m_name.text = MonoSingleton<GameTools>.Instacne.GetGoodsName(m_goodsId);
		m_desc.text = MonoSingleton<GameTools>.Instacne.GetGoodsDesc(m_goodsId);
		AddEventListener();
		if (!m_isInit)
		{
			m_discountOriginalPricePos = m_originalPriceTxt.transform.localPosition;
			m_isInit = true;
		}
		if (PlayerDataModule.Instance.GetPlayGoodsNum(m_goodsId) > 0.0)
		{
			m_buyBtn.SetActive(false);
		}
		else
		{
			ShowBuyContent();
			m_buyBtn.SetActive(true);
		}
		if (PlayerDataModule.Instance.GetPlayGoodsNum(m_goodsId) > 0.0)
		{
			m_selectFlag.SetActive(true);
			activeEffect.SetActive(true);
			m_icon.color = Color.white;
		}
		else
		{
			activeEffect.SetActive(false);
			m_selectFlag.SetActive(false);
			m_icon.color = new Color(1f, 1f, 1f, 0.4f);
		}
		m_icon.gameObject.SetActive(false);
		string assetName2 = MonoSingleton<GameTools>.Instacne.GetGoodsIconIdByGoodsId(m_goodsId).ToString();
		Mod.Resource.LoadAsset(AssetUtility.GetGameIconAsset(assetName2), new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
		{
			if (m_isRelease)
			{
				Mod.Resource.UnloadAsset(asset);
			}
			else
			{
				m_icon.gameObject.SetActive(true);
				m_icon.sprite = MonoSingleton<GameTools>.Instacne.CreateSpriteByTexture2D(asset as Texture2D);
				loadedAsserts.Add(asset);
			}
		}, delegate(string assetName, string errorMessage, object data2)
		{
			Log.Error(string.Format("Can not load item '{0}' from '{1}' with error message.", assetName, errorMessage));
		}));
	}

	private void ShowBuyContent()
	{
		if (m_goodsId == GameCommon.ORIGIN_REBIRTH_FREE)
		{
			m_giftPackageId = 4303;
		}
		else if (m_goodsId == GameCommon.START_FREE_SHIELD)
		{
			m_giftPackageId = 4302;
		}
		else if (m_goodsId == GameCommon.EVERY_DAY_GIVE_POWER)
		{
			m_giftPackageId = 4301;
		}
		if (m_giftPackageId <= 0)
		{
			return;
		}
		Shops_shopTable shops_shopTable = Mod.DataTable.Get<Shops_shopTable>()[m_giftPackageId];
		int goodsTeamid = shops_shopTable.GoodsTeamid;
		GoodsTeam_goodsTeamTable goodsTeam_goodsTeamTable = Mod.DataTable.Get<GoodsTeam_goodsTeamTable>()[goodsTeamid];
		string productRealPrice = MonoSingleton<GameTools>.Instacne.GetProductRealPrice(m_giftPackageId);
		m_originalPriceTxt.text = productRealPrice;
		float num = 0f;
		if (shops_shopTable.Discount > 0)
		{
			m_originalPriceTxt.transform.localPosition = m_discountOriginalPricePos;
			m_discountPriceTxt.gameObject.SetActive(true);
			if (shops_shopTable.BuyType == 1)
			{
				string text = Regex.Replace(productRealPrice, "[^0-9.-]", "");
				int length = productRealPrice.IndexOf(text);
				productRealPrice.Substring(0, length);
				num = float.Parse(text);
				num = num * 100f / (float)shops_shopTable.Discount;
			}
			else
			{
				num = int.Parse(shops_shopTable.Price);
				num = num * 100f / (float)shops_shopTable.Discount;
			}
			int num2 = (int)num;
			if (num - (float)num2 > 0.001f)
			{
				m_discountPriceTxt.text = num.ToString("0.00");
			}
			else
			{
				m_discountPriceTxt.text = num2.ToString();
			}
		}
		else
		{
			m_originalPriceTxt.transform.localPosition = m_originalPricePos.localPosition;
			m_discountPriceTxt.gameObject.SetActive(false);
		}
	}

	private void AddEventListener()
	{
		EventTriggerListener.Get(m_buyBtn).onClick = Buyhandler;
	}

	private void RemoveEventListener()
	{
		EventTriggerListener eventTriggerListener = EventTriggerListener.Get(m_buyBtn);
		eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(Buyhandler));
	}

	private void Buyhandler(GameObject go)
	{
		if (m_giftPackageId > 0)
		{
			MonoSingleton<GameTools>.Instacne.CommonBuyOperate(m_giftPackageId, UIFormId.BufferShowForm);
		}
	}

	public override object GetData()
	{
		return m_goodsId;
	}

	public override void SetSelected(bool selected)
	{
	}

	public override void OnRelease()
	{
		m_isRelease = true;
		m_isInit = false;
		for (int i = 0; i < loadedAsserts.Count; i++)
		{
			Mod.Resource.UnloadAsset(loadedAsserts[i]);
		}
		loadedAsserts.Clear();
		RemoveEventListener();
	}
}
