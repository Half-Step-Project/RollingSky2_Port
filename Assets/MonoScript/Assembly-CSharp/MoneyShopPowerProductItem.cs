using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.UI;

public class MoneyShopPowerProductItem : UILoopItem
{
	public Image m_goodsIcon;

	public Text m_goodsNumTxt;

	public Button m_buyBtn;

	public Text m_buyBtnTxt;

	public Button adBtn;

	public GameObject m_hotObject;

	public GameObject m_ClockFlag;

	public GameObject m_adFlag;

	public Text m_adLeftTime;

	public GameObject adPrepare;

	public GameObject bgGreen;

	private bool m_isCdEnd = true;

	private bool m_isAdItem;

	private uint adTimerId;

	private long m_adPowerStamp;

	private ShopItemData m_itemData;

	private List<object> m_loadedAsserts = new List<object>();

	private bool m_isInit;

	private int preCanShowAds = -1;

	private void Awake()
	{
		Mod.Event.Subscribe(EventArgs<GameGoodsNumChangeEventArgs>.EventId, OnGoodsChanged);
	}

	private void OnDestroy()
	{
		Mod.Event.Unsubscribe(EventArgs<GameGoodsNumChangeEventArgs>.EventId, OnGoodsChanged);
	}

	private void Update()
	{
		if (!m_isCdEnd && m_isAdItem)
		{
			UpdateCdTime(ref m_isCdEnd);
			if (m_isCdEnd)
			{
				SetAdShowState();
			}
		}
	}

	private void OnGoodsChanged(object sender, EventArgs e)
	{
		if (m_isInit && m_itemData != null && (e as GameGoodsNumChangeEventArgs).GoodsId == 6)
		{
			RefreshPriceText();
		}
	}

	public override void Data(object data)
	{
		m_isInit = true;
		base.Data(data);
		m_itemData = (ShopItemData)data;
		RefreshPriceText();
		m_goodsNumTxt.text = m_itemData.showInfo;
		ShowHot(m_itemData.hot);
		string _goodsIconName = m_itemData.iconId.ToString();
		Mod.Resource.LoadAsset(AssetUtility.GetGameIconAsset(_goodsIconName), new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
		{
			if (!m_isInit)
			{
				OnRelease();
			}
			else if (m_goodsIcon != null && asset != null)
			{
				m_goodsIcon.sprite = MonoSingleton<GameTools>.Instacne.CreateSpriteByTexture2D(asset as Texture2D);
				m_goodsIcon.SetNativeSize();
				m_loadedAsserts.Add(asset);
			}
		}, delegate(string assetName, string errorMessage, object data2)
		{
			Log.Error(string.Format("Can not load sprite '{0}' from '{1}' with error message '{2}'.", _goodsIconName, assetName, errorMessage));
		}));
		EventTriggerListener.Get(m_buyBtn.gameObject).onClick = OnClickBuyButton;
		if (m_itemData.buyType == 3)
		{
			m_isAdItem = true;
			adTimerId = TimerHeap.AddTimer((uint)(GameCommon.COMMON_AD_REFRESHTIME * 1000f), (uint)(GameCommon.COMMON_AD_REFRESHTIME * 1000f), SetAdButtonState);
			adBtn.gameObject.SetActive(true);
			m_buyBtn.gameObject.SetActive(false);
			m_adPowerStamp = PlayerDataModule.Instance.GetAdPowerTimeStamp();
			UpdateCdTime(ref m_isCdEnd);
			SetAdShowState();
			SetAdButtonState();
		}
		else
		{
			m_isAdItem = false;
			adBtn.gameObject.SetActive(false);
			m_buyBtn.gameObject.SetActive(true);
			InfocUtils.Report_rollingsky2_games_neigou(m_itemData.itemId, 1);
		}
	}

	public override object GetData()
	{
		return base.GetData();
	}

	public override void SetSelected(bool selected)
	{
		base.SetSelected(selected);
	}

	private void RefreshPriceText()
	{
		double playGoodsNum = Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).GetPlayGoodsNum(6);
		if ((double)int.Parse(m_itemData.price) > playGoodsNum)
		{
			m_buyBtnTxt.color = GameCommon.COMMON_RED;
		}
		else
		{
			m_buyBtnTxt.color = new Color(1f, 0.95f, 0.6f, 1f);
		}
		m_buyBtnTxt.text = m_itemData.price;
	}

	private void SetAdShowState()
	{
		if (m_isCdEnd)
		{
			m_ClockFlag.SetActive(false);
			m_adFlag.SetActive(true);
			bgGreen.SetActive(true);
		}
		else
		{
			m_ClockFlag.SetActive(true);
			m_adFlag.SetActive(false);
			bgGreen.SetActive(false);
		}
	}

	private void SetAdButtonState()
	{
		int num = (MonoSingleton<GameTools>.Instacne.CanShowAd(ADScene.MainView) ? 1 : 0);
		if (preCanShowAds == num)
		{
			return;
		}
		preCanShowAds = num;
		adBtn.interactable = ((num == 1) ? true : false);
		if (num == 1)
		{
			if (m_isCdEnd)
			{
				m_ClockFlag.SetActive(false);
				m_adFlag.SetActive(true);
				bgGreen.SetActive(true);
			}
			else
			{
				m_ClockFlag.SetActive(true);
				m_adFlag.SetActive(false);
				bgGreen.SetActive(false);
			}
			adPrepare.gameObject.SetActive(false);
			m_adLeftTime.gameObject.SetActive(true);
		}
		else
		{
			adPrepare.gameObject.SetActive(true);
			m_ClockFlag.SetActive(false);
			m_adFlag.SetActive(false);
			m_adLeftTime.gameObject.SetActive(false);
			bgGreen.SetActive(false);
		}
	}

	private void OnClickBuyButton(GameObject obj)
	{
		InfocUtils.Report_rollingsky2_games_neigou(m_itemData.itemId, 2);
		OnBuy();
	}

	private void OnBuy()
	{
		PlayerDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule);
		double playGoodsNum = dataModule.GetPlayGoodsNum(6);
		int num = int.Parse(m_itemData.price);
		if (playGoodsNum < (double)num)
		{
			(Mod.UI.GetUIForm(UIFormId.MoneyShopForm) as MoneyShopForm).Refresh(MoneyShopForm.MoneyType.Money);
			InfocUtils.Report_rollingsky2_games_neigou(m_itemData.itemId, 4);
			return;
		}
		dataModule.ChangePlayerGoodsNum(6, -num);
		foreach (KeyValuePair<int, int> item in MonoSingleton<GameTools>.Instacne.DealGoodsTeamById(m_itemData.goodsdTeamid))
		{
			dataModule.ChangePlayerGoodsNum(item.Key, item.Value);
		}
		Singleton<DataModuleManager>.Instance.GetDataModule<ShopDataModule>(DataNames.ShopDataModule).AddBuyCountByID(m_itemData.id);
		GetGoodsData getGoodsData = new GetGoodsData();
		getGoodsData.Buy = true;
		getGoodsData.GoodsTeam = true;
		getGoodsData.GoodsTeamId = m_itemData.goodsdTeamid;
		getGoodsData.GoodsTeamNum = 1;
		Mod.UI.OpenUIForm(UIFormId.GetGoodsForm, getGoodsData);
		InfocUtils.Report_rollingsky2_games_neigou(m_itemData.itemId, 3);
	}

	public void OnClickAdButton()
	{
		if (m_adPowerStamp - PlayerDataModule.Instance.ServerTime <= 0 && MonoSingleton<GameTools>.Instacne.CanShowAd(ADScene.NONE))
		{
			MonoSingleton<GameTools>.Instacne.PlayerVideoAd(ADScene.NONE, delegate(ADScene x)
			{
				OnAdSuccess(m_itemData.goodsdTeamid, m_itemData.count, x);
			});
		}
	}

	private void OnAdSuccess(int goodsTeamId, int count, ADScene adScen = ADScene.NONE)
	{
		m_adPowerStamp = PlayerDataModule.Instance.ServerTime + m_itemData.buyCd * 1000;
		PlayerDataModule.Instance.SaveADPowerTimeStamp(m_adPowerStamp);
		Dictionary<int, int>.Enumerator enumerator = MonoSingleton<GameTools>.Instacne.DealGoodsTeamById(goodsTeamId, count).GetEnumerator();
		while (enumerator.MoveNext())
		{
			PlayerDataModule.Instance.ChangePlayerGoodsNum(enumerator.Current.Key, enumerator.Current.Value, AssertChangeType.AD);
		}
		if (Mod.UI.UIFormIsOpen(UIFormId.MoneyShopForm))
		{
			UpdateCdTime(ref m_isCdEnd);
			SetAdShowState();
			GetGoodsData getGoodsData = new GetGoodsData();
			getGoodsData.GoodsTeamId = goodsTeamId;
			getGoodsData.GoodsTeamNum = count;
			getGoodsData.GoodsTeam = true;
			getGoodsData.ShowExpound = false;
			Mod.UI.OpenUIForm(UIFormId.GetGoodsForm, getGoodsData);
		}
	}

	private void UpdateCdTime(ref bool isCdEnd)
	{
		long num = m_adPowerStamp - PlayerDataModule.Instance.ServerTime;
		if (num > 0)
		{
			isCdEnd = false;
			m_adLeftTime.text = MonoSingleton<GameTools>.Instacne.TimeFormat_HH_MM_SS(num / 1000);
		}
		else
		{
			m_adLeftTime.text = "";
			isCdEnd = true;
		}
	}

	public override void OnRelease()
	{
		m_isInit = false;
		for (int i = 0; i < m_loadedAsserts.Count; i++)
		{
			Mod.Resource.UnloadAsset(m_loadedAsserts[i]);
		}
		m_loadedAsserts.Clear();
		TimerHeap.DelTimer(adTimerId);
	}

	private void ShowHot(int active)
	{
		if ((bool)m_hotObject)
		{
			m_hotObject.SetActive(active == 1);
		}
	}
}
