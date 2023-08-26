using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.UI;

public class ExchangeProductItem : MonoBehaviour, IScrollItem
{
	public enum State
	{
		Normal,
		Ad,
		UnAd,
		AdCd
	}

	[Label]
	public int mItemIndex;

	[Label]
	public int mTableIndex;

	[Label]
	public int mTableID;

	public Image mGoldBg;

	public Image mReputaionBg;

	public Text mGoodsNameTxt;

	public Image mGoodsIcon;

	public Text mGoodsTxt;

	public Button mBuyBtn;

	public Image mBuyIcon;

	public Text mBuyTxt;

	private Color mBuyTextColor = Color.white;

	public Button mAdBtn;

	public SetUIGrey mAdGrey;

	public Text mAdGreyTxt;

	[Label]
	public State mState;

	public UIMoveTarget goldMoveTarget;

	private ExchangeStore_table mData;

	private IScroll mScroll;

	private List<object> m_loadedAsserts = new List<object>();

	private bool m_isInit;

	private float mTime;

	private uint adTimerId;

	private ExchangeStoreDataModule GetDataModule
	{
		get
		{
			return Singleton<DataModuleManager>.Instance.GetDataModule<ExchangeStoreDataModule>(DataNames.ExchangeStoreDataModule);
		}
	}

	public void SetScroll(IScroll scroll)
	{
		mScroll = scroll;
	}

	public void SetData(int itemIndex, int tableIndex, int tableID)
	{
		mItemIndex = itemIndex;
		mTableIndex = tableIndex;
		mTableID = tableID;
		mData = GetDataModule.mSaveData.mProductDatas[mTableIndex].mTable;
	}

	public void OnInit()
	{
	}

	public void OnOpen()
	{
		if ((bool)mGoodsNameTxt)
		{
			mGoodsNameTxt.text = Mod.Localization.GetInfoById(mData.ShowInfoID);
		}
		if ((bool)mGoodsTxt)
		{
			double count = GetCount();
			mGoodsTxt.text = MonoSingleton<GameTools>.Instacne.DoubleToFormatString(count);
		}
		if ((bool)mBuyBtn)
		{
			mBuyBtn.onClick.AddListener(OnClickBuyBtn);
		}
		if ((bool)mAdBtn)
		{
			mAdBtn.onClick.AddListener(OnClickAdBtn);
		}
		if (mData.Type == 1)
		{
			if ((bool)mGoldBg)
			{
				mGoldBg.gameObject.SetActive(true);
			}
			if ((bool)mReputaionBg)
			{
				mReputaionBg.gameObject.SetActive(false);
			}
		}
		else if (mData.Type == 2)
		{
			if ((bool)mGoldBg)
			{
				mGoldBg.gameObject.SetActive(false);
			}
			if ((bool)mReputaionBg)
			{
				mReputaionBg.gameObject.SetActive(true);
			}
		}
		LoadIcon();
		mTime = 0f;
		if (mBuyTxt != null)
		{
			mBuyTextColor = mBuyTxt.color;
		}
		RefreshBuyText();
		OnRefresh();
		adTimerId = TimerHeap.AddTimer((uint)(GameCommon.COMMON_AD_REFRESHTIME * 1000f), (uint)(GameCommon.COMMON_AD_REFRESHTIME * 1000f), SetAdButtonState);
		Mod.Event.Subscribe(EventArgs<GameGoodsNumChangeEventArgs>.EventId, OnPlayerAssetChange);
	}

	private void SetAdButtonState()
	{
		if (!GetDataModule.IsAd(mTableIndex) || GetDataModule.IsOnCDTime(mTableIndex))
		{
			return;
		}
		if (MonoSingleton<GameTools>.Instacne.CanShowAd(ADScene.NONE))
		{
			if ((bool)mAdBtn)
			{
				mAdBtn.gameObject.SetActive(true);
			}
			if ((bool)mBuyBtn)
			{
				mBuyBtn.gameObject.SetActive(false);
			}
			if ((bool)mAdGrey)
			{
				mAdGrey.SetGrey(false);
			}
			return;
		}
		if ((bool)mAdBtn)
		{
			mAdBtn.gameObject.SetActive(true);
		}
		if ((bool)mBuyBtn)
		{
			mBuyBtn.gameObject.SetActive(false);
		}
		if ((bool)mAdGrey)
		{
			mAdGrey.SetGrey(true);
		}
		if ((bool)mAdGreyTxt)
		{
			mAdGreyTxt.text = Mod.Localization.GetInfoById(211);
		}
	}

	public void OnTick(float elapseSeconds, float realElapseSeconds)
	{
		mTime += elapseSeconds;
		if (!(mTime >= 1f))
		{
			return;
		}
		State state = mState;
		if (state == State.AdCd)
		{
			long cdRemainingTime = GetDataModule.GetCdRemainingTime(mTableIndex);
			if (cdRemainingTime >= 0)
			{
				if ((bool)mAdGreyTxt)
				{
					mAdGreyTxt.text = MonoSingleton<GameTools>.Instacne.TimeFormat_HH_MM_SS(cdRemainingTime / 1000);
				}
				if ((bool)mAdGrey)
				{
					mAdGrey.SetGrey(true);
				}
			}
			else
			{
				OnRefresh();
			}
		}
		mTime = 0f;
	}

	public void OnClose()
	{
		if ((bool)mBuyBtn)
		{
			mBuyBtn.onClick.RemoveListener(OnClickBuyBtn);
		}
		if ((bool)mAdBtn)
		{
			mAdBtn.onClick.RemoveListener(OnClickAdBtn);
		}
		TimerHeap.DelTimer(adTimerId);
		Mod.Event.Unsubscribe(EventArgs<GameGoodsNumChangeEventArgs>.EventId, OnPlayerAssetChange);
	}

	public void OnRefresh()
	{
		if (!Mod.UI.UIFormIsOpen(UIFormId.ExchangeStoreForm))
		{
			return;
		}
		mState = State.Normal;
		if (GetDataModule.IsAd(mTableIndex))
		{
			if (GetDataModule.IsOnCDTime(mTableIndex))
			{
				if ((bool)mAdBtn)
				{
					mAdBtn.gameObject.SetActive(true);
				}
				if ((bool)mBuyBtn)
				{
					mBuyBtn.gameObject.SetActive(false);
				}
				if ((bool)mAdGrey)
				{
					mAdGrey.SetGrey(true);
				}
				long cdRemainingTime = GetDataModule.GetCdRemainingTime(mTableIndex);
				if ((bool)mAdGreyTxt)
				{
					mAdGreyTxt.text = MonoSingleton<GameTools>.Instacne.TimeFormat_HH_MM_SS(cdRemainingTime / 1000);
				}
				mState = State.AdCd;
			}
			else if (MonoSingleton<GameTools>.Instacne.CanShowAd(ADScene.NONE))
			{
				if ((bool)mAdBtn)
				{
					mAdBtn.gameObject.SetActive(true);
				}
				if ((bool)mBuyBtn)
				{
					mBuyBtn.gameObject.SetActive(false);
				}
				if ((bool)mAdGrey)
				{
					mAdGrey.SetGrey(false);
				}
				mState = State.Ad;
			}
			else
			{
				if ((bool)mAdBtn)
				{
					mAdBtn.gameObject.SetActive(true);
				}
				if ((bool)mBuyBtn)
				{
					mBuyBtn.gameObject.SetActive(false);
				}
				if ((bool)mAdGrey)
				{
					mAdGrey.SetGrey(true);
				}
				if ((bool)mAdGreyTxt)
				{
					mAdGreyTxt.text = Mod.Localization.GetInfoById(211);
				}
				mState = State.UnAd;
			}
		}
		else
		{
			if ((bool)mAdBtn)
			{
				mAdBtn.gameObject.SetActive(false);
			}
			if ((bool)mBuyBtn)
			{
				mBuyBtn.gameObject.SetActive(true);
			}
			mState = State.Normal;
		}
	}

	public void OnRelease()
	{
		m_isInit = false;
		for (int i = 0; i < m_loadedAsserts.Count; i++)
		{
			Mod.Resource.UnloadAsset(m_loadedAsserts[i]);
		}
		m_loadedAsserts.Clear();
	}

	private void OnPlayerAssetChange(object sender, EventArgs e)
	{
		if (Mod.UI.UIFormIsOpen(UIFormId.ExchangeStoreForm))
		{
			GameGoodsNumChangeEventArgs gameGoodsNumChangeEventArgs = e as GameGoodsNumChangeEventArgs;
			if (gameGoodsNumChangeEventArgs != null && gameGoodsNumChangeEventArgs.GoodsId == 6)
			{
				RefreshBuyText();
			}
		}
	}

	private void OnClickBuyBtn()
	{
		if (IsCanBuy())
		{
			PlayerDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule);
			dataModule.ChangePlayerGoodsNum(6, -mData.Price, AssertChangeType.SHOP_KEY);
			double count = GetCount();
			int goodID = GetGoodID();
			if (goodID != 3)
			{
				dataModule.ChangePlayerGoodsNum(goodID, count, AssertChangeType.SHOP_KEY);
			}
			GetDataModule.BuySuccess(mTableIndex, ExchangeStoreDataModule.BuyType.Normal);
			OnRefresh();
			OpenGetGoodsForm(goodID, count, ExchangeStoreDataModule.BuyType.Normal);
		}
	}

	private void OnClickAdBtn()
	{
		if (mState != State.Ad)
		{
			return;
		}
		if (!MonoSingleton<GameTools>.Instacne.CanShowAd(ADScene.NONE))
		{
			OnRefresh();
			return;
		}
		int num = 0;
		if (mData.Type == 1)
		{
			num = 28;
		}
		else if (mData.Type == 2)
		{
			num = 29;
		}
		if (num != 0)
		{
			InfocUtils.Report_rollingsky2_games_ads(num, 0, 1, 0, 3, 0);
		}
		MonoSingleton<GameTools>.Instacne.PlayerVideoAd(ADScene.NONE, OnAdSuccess, OnAdFail, OnAdFail);
	}

	private void OnAdSuccess(ADScene adScen = ADScene.NONE)
	{
		int num = 0;
		if (mData.Type == 1)
		{
			num = 28;
		}
		else if (mData.Type == 2)
		{
			num = 29;
		}
		if (num != 0)
		{
			InfocUtils.Report_rollingsky2_games_ads(num, 0, 1, 0, 4, 0);
		}
		PlayerDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule);
		double count = GetCount();
		int goodID = GetGoodID();
		if (goodID != 3)
		{
			dataModule.ChangePlayerGoodsNum(goodID, count, AssertChangeType.SHOP_KEY);
		}
		GetDataModule.BuySuccess(mTableIndex, ExchangeStoreDataModule.BuyType.AD);
		OpenGetGoodsForm(goodID, count, ExchangeStoreDataModule.BuyType.AD);
		if (Mod.UI.UIFormIsOpen(UIFormId.ExchangeStoreForm) && mScroll != null)
		{
			mScroll.OnRefresh();
		}
	}

	private void OnAdFail(ADScene adScen = ADScene.NONE)
	{
		if (Mod.UI.UIFormIsOpen(UIFormId.ExchangeStoreForm) && mScroll != null)
		{
			mScroll.OnRefresh();
		}
	}

	private void LoadIcon()
	{
		m_isInit = true;
		string _goodsIconName = mData.IconID.ToString();
		Mod.Resource.LoadAsset(AssetUtility.GetGameIconAsset(_goodsIconName), new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
		{
			if (!m_isInit)
			{
				OnRelease();
			}
			else if (mGoodsIcon != null && asset != null)
			{
				mGoodsIcon.sprite = MonoSingleton<GameTools>.Instacne.CreateSpriteByTexture2D(asset as Texture2D);
				m_loadedAsserts.Add(asset);
			}
		}, delegate(string assetName, string errorMessage, object data2)
		{
			Log.Error(string.Format("Can not load sprite '{0}' from '{1}' with error message '{2}'.", _goodsIconName, assetName, errorMessage));
		}));
	}

	private bool IsCanBuy()
	{
		return Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).GetPlayGoodsNum(6) >= (double)mData.Price;
	}

	private void OpenGetGoodsForm(int goodID, double count, ExchangeStoreDataModule.BuyType buyType)
	{
		if (Mod.UI.UIFormIsOpen(UIFormId.GetGoodsForm))
		{
			Mod.UI.CloseUIForm(UIFormId.GetGoodsForm);
		}
		GetGoodsData getGoodsData = new GetGoodsData();
		getGoodsData.GoodsTeam = false;
		getGoodsData.GoodsId = goodID;
		getGoodsData.GoodsNum = count;
		if (goodID == 3)
		{
			getGoodsData.closeCallback = delegate(GetGoodsForm form)
			{
				GetAwardForm form2 = GetAwardForm.Form;
				if ((bool)form2)
				{
					double playGoodsNum = PlayerDataModule.Instance.GetPlayGoodsNum(goodID);
					goldMoveTarget.SetData(playGoodsNum, playGoodsNum + count, -1);
					form2.StartMove(form.m_awardIcon.GetComponent<RectTransform>(), goodID, goldMoveTarget, (int)count, delegate
					{
						PlayerDataModule.Instance.ChangePlayerGoodsNum(goodID, count, AssertChangeType.SHOP_KEY);
					});
				}
			};
		}
		Mod.UI.OpenUIForm(UIFormId.GetGoodsForm, getGoodsData);
	}

	private int GetGoodID()
	{
		int result = 0;
		switch (mData.Type)
		{
		case 1:
			result = 3;
			break;
		case 2:
			result = GameCommon.REPUTATION_ID;
			break;
		}
		return result;
	}

	private double GetCount()
	{
		double num = 0.0;
		switch (mData.Type)
		{
		case 1:
			num = PlayerDataModule.Instance.GetOffLineProductionGoldByTime(1000L);
			break;
		case 2:
			num = PlayerDataModule.Instance.GetCurrentProductReputaionSpeed();
			break;
		}
		return num * (double)mData.Coin_time;
	}

	private void RefreshBuyText()
	{
		if ((bool)mBuyTxt)
		{
			mBuyTxt.text = mData.Price.ToString();
			mBuyTxt.color = (IsCanBuy() ? mBuyTextColor : GameCommon.COMMON_RED);
		}
	}
}
