using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.UI;

public class MoneyShopMoneyProductItem : UILoopItem
{
	public Image m_goodsIcon;

	public Text m_goodsNumTxt;

	public Button m_buyBtn;

	public Text m_buyBtnTxt;

	public GameObject m_goodsSale;

	public GameObject m_hotObject;

	public GameObject[] m_ten = new GameObject[10];

	public GameObject[] m_ge = new GameObject[10];

	public Button m_adBtn;

	public Text m_adLeftTime;

	public GameObject m_ClockFlag;

	public GameObject m_adFlag;

	public Text m_adPrepareTxt;

	public GameObject bgGreen;

	private ShopItemData m_itemData;

	private List<object> m_loadedAsserts = new List<object>();

	private bool m_isInit;

	private uint adTimerId;

	private long m_adKeyStamp;

	private bool m_isCdEnd = true;

	private bool m_isAdItem;

	private int preCanShowAds = -1;

	public override void Data(object data)
	{
		m_isInit = true;
		base.Data(data);
		m_itemData = (ShopItemData)data;
		m_buyBtnTxt.text = m_itemData.price;
		m_goodsNumTxt.text = m_itemData.showInfo;
		ShowDiscount(m_itemData.discount);
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
			m_adKeyStamp = PlayerDataModule.Instance.GetAdKeyTimeStamp();
			UpdateCdTime(ref m_isCdEnd);
			adTimerId = TimerHeap.AddTimer((uint)(GameCommon.COMMON_AD_REFRESHTIME * 1000f), (uint)(GameCommon.COMMON_AD_REFRESHTIME * 1000f), SetAdButtonState);
			m_adBtn.gameObject.SetActive(true);
			m_buyBtn.gameObject.SetActive(false);
			SetAdShowState();
			SetAdButtonState();
		}
		else
		{
			m_buyBtn.gameObject.SetActive(true);
			m_isAdItem = false;
			m_adBtn.gameObject.SetActive(false);
		}
		InfocUtils.Report_rollingsky2_games_neigou(m_itemData.itemId, 1);
	}

	public void OnAdBtnHandler()
	{
		if (m_adKeyStamp - PlayerDataModule.Instance.ServerTime <= 0 && MonoSingleton<GameTools>.Instacne.CanShowAd(ADScene.NONE))
		{
			InfocUtils.Report_rollingsky2_games_ads(12, 0, 1, 0, 3, 0);
			MonoSingleton<GameTools>.Instacne.PlayerVideoAd(ADScene.NONE, delegate(ADScene x)
			{
				int goodsdTeamid = m_itemData.goodsdTeamid;
				int count = m_itemData.count;
				OnAdSuccess(goodsdTeamid, count, x);
			});
		}
	}

	private void OnAdSuccess(int goodsTeamId, int count, ADScene adScen = ADScene.NONE)
	{
		m_adKeyStamp = PlayerDataModule.Instance.ServerTime + m_itemData.buyCd * 1000;
		PlayerDataModule.Instance.SaveADKeyTimeStamp(m_adKeyStamp);
		Mod.Event.FireNow(this, Mod.Reference.Acquire<PauseResponseGoodsNumChangeEventArgs>().Initialize(true));
		Dictionary<int, int>.Enumerator enumerator = MonoSingleton<GameTools>.Instacne.DealGoodsTeamById(goodsTeamId, count).GetEnumerator();
		while (enumerator.MoveNext())
		{
			PlayerDataModule.Instance.ChangePlayerGoodsNum(enumerator.Current.Key, enumerator.Current.Value, AssertChangeType.AD);
		}
		if (!Mod.UI.UIFormIsOpen(UIFormId.MoneyShopForm))
		{
			return;
		}
		UpdateCdTime(ref m_isCdEnd);
		SetAdShowState();
		GetGoodsData getGoodsData = new GetGoodsData();
		getGoodsData.GoodsTeamId = goodsTeamId;
		getGoodsData.GoodsTeamNum = count;
		getGoodsData.GoodsTeam = true;
		getGoodsData.ShowExpound = false;
		getGoodsData.closeCallback = delegate(GetGoodsForm form)
		{
			GetAwardForm form2 = GetAwardForm.Form;
			if ((bool)form2)
			{
				GameObject[] array = GameObject.FindGameObjectsWithTag("ItemMoveTarget");
				UIMoveTarget uIMoveTarget = null;
				GameObject[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					UIMoveTarget component = array2[i].GetComponent<UIMoveTarget>();
					if (!(component == null) && component.id == 203)
					{
						uIMoveTarget = component;
					}
				}
				foreach (KeyValuePair<int, int> item in MonoSingleton<GameTools>.Instacne.DealGoodsTeamById(goodsTeamId, count))
				{
					if (item.Key == 6 && uIMoveTarget != null)
					{
						double playGoodsNum = PlayerDataModule.Instance.GetPlayGoodsNum(6);
						uIMoveTarget.SetData(playGoodsNum - (double)item.Value, playGoodsNum, -1);
						RectTransform component2 = form.m_awardIcon.GetComponent<RectTransform>();
						MonoSingleton<GameTools>.Instacne.DisableInputForAWhile(5000u);
						form2.StartMove(component2.position, component2.sizeDelta * 0.65f, "diamound", item.Key, uIMoveTarget, item.Value, delegate
						{
							MonoSingleton<GameTools>.Instacne.EnableInput();
						});
					}
				}
			}
			Mod.Event.FireNow(this, Mod.Reference.Acquire<PauseResponseGoodsNumChangeEventArgs>().Initialize(false));
		};
		Mod.UI.OpenUIForm(UIFormId.GetGoodsForm, getGoodsData);
		InfocUtils.Report_rollingsky2_games_ads(12, 0, 1, 0, 4, 0);
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

	private void UpdateCdTime(ref bool isCdEnd)
	{
		long num = m_adKeyStamp - PlayerDataModule.Instance.ServerTime;
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

	public override object GetData()
	{
		return base.GetData();
	}

	public override void SetSelected(bool selected)
	{
		base.SetSelected(selected);
	}

	private void OnClickBuyButton(GameObject obj)
	{
		MonoSingleton<GameTools>.Instacne.CommonBuyOperate(m_itemData.itemId, UIFormId.MoneyShopForm);
		Mod.Event.FireNow(this, Mod.Reference.Acquire<PauseResponseGoodsNumChangeEventArgs>().Initialize(true));
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

	private void SetAdButtonState()
	{
		int num = (MonoSingleton<GameTools>.Instacne.CanShowAd(ADScene.MainView) ? 1 : 0);
		if (preCanShowAds == num)
		{
			return;
		}
		preCanShowAds = num;
		m_adBtn.interactable = ((num == 1) ? true : false);
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
			m_adPrepareTxt.gameObject.SetActive(false);
			m_adLeftTime.gameObject.SetActive(true);
		}
		else
		{
			m_adPrepareTxt.gameObject.SetActive(true);
			m_ClockFlag.SetActive(false);
			m_adFlag.SetActive(false);
			m_adLeftTime.gameObject.SetActive(false);
			bgGreen.SetActive(false);
		}
	}

	public static bool HasInternet()
	{
		return Application.internetReachability != NetworkReachability.NotReachable;
	}

	private void ShowDiscount(int discount)
	{
		if (discount > 0)
		{
			m_goodsSale.SetActive(true);
			ShowDiscountNum(discount / 10, discount % 10);
		}
		else
		{
			m_goodsSale.SetActive(false);
		}
	}

	private void ShowDiscountNum(int ten, int ge)
	{
		for (int i = 0; i < 10; i++)
		{
			if (ten == 0)
			{
				m_ten[i].SetActive(false);
			}
			else if (i == ten)
			{
				m_ten[i].SetActive(true);
			}
			else
			{
				m_ten[i].SetActive(false);
			}
			if (i == ge)
			{
				m_ge[i].SetActive(true);
			}
			else
			{
				m_ge[i].SetActive(false);
			}
		}
	}

	private void ShowHot(int active)
	{
		if ((bool)m_hotObject)
		{
			m_hotObject.SetActive(active == 1);
		}
	}
}
