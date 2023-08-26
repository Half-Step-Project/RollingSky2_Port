using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.UI;

public class InstrumentUnlockForm : UGUIForm
{
	public class Data
	{
		public List<int> ids;

		public InstrumentPropertyType type;

		public override string ToString()
		{
			return base.ToString();
		}
	}

	public enum AdState
	{
		Null,
		Ad,
		UnAd
	}

	public enum CloseState
	{
		Null,
		Show,
		Hide
	}

	public Data mData;

	public Image mIcon;

	public Text mIconName;

	public Text mUpNote;

	[Label]
	public AdState mAdState;

	public Button mAdBtn;

	public SetUIGrey mSetUIGrey;

	[Label]
	public CloseState mCloseState;

	public Button mCloseButton;

	[Label]
	[SerializeField]
	private float mUnAdTime;

	[Label]
	[SerializeField]
	private float mUnAdTimeDuration = 1f;

	[Label]
	[SerializeField]
	private float mShowCloseTime;

	[Label]
	[SerializeField]
	private float mShowCloseDuration = 2f;

	[Label]
	[SerializeField]
	private int mCurrentID;

	[Label]
	[SerializeField]
	private int mToLV;

	private List<object> mLoadedAsserts = new List<object>();

	private bool mIsInit;

	protected override void OnInit(object userData)
	{
		base.OnInit(userData);
	}

	protected override void OnOpen(object userData)
	{
		base.OnOpen(userData);
		mData = userData as Data;
		if (mData == null)
		{
			Log.Error("InstrumentUnlockForm mData is null");
		}
		if (mData.ids.Count == 0)
		{
			Log.Error("InstrumentUnlockForm mData.ids count min 1");
		}
		mCurrentID = mData.ids[0];
		PlayerLocalInstrumentData instrumentDataById = PlayerDataModule.Instance.GetInstrumentDataById(mCurrentID);
		mToLV = instrumentDataById.GetAdDirectUpLevel();
		if (mIcon != null)
		{
			mIsInit = true;
			Mod.Resource.LoadAsset(AssetUtility.GetGameIconAsset(instrumentDataById.IconId), new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
			{
				if (mIcon != null && mIsInit)
				{
					mIcon.sprite = MonoSingleton<GameTools>.Instacne.CreateSpriteByTexture2D(asset as Texture2D);
					mLoadedAsserts.Add(asset);
				}
			}, delegate(string assetName, string errorMessage, object data2)
			{
				Log.Error(string.Format("Can not load item '{0}' from '{1}' with error message.", assetName, errorMessage));
			}));
		}
		if (mIconName != null)
		{
			mIconName.text = instrumentDataById.Name();
		}
		if (mUpNote != null)
		{
			mUpNote.text = string.Format(Mod.Localization.GetInfoById(330), mToLV);
		}
		if ((bool)mAdBtn)
		{
			mAdBtn.onClick.AddListener(OnClickAdButton);
		}
		if ((bool)mCloseButton)
		{
			mCloseButton.onClick.AddListener(OnClickCloseButton);
			mCloseButton.gameObject.SetActive(false);
			mCloseState = CloseState.Hide;
			mShowCloseTime = 0f;
		}
		OnRefresh();
	}

	protected override void OnTick(float elapseSeconds, float realElapseSeconds)
	{
		base.OnTick(elapseSeconds, realElapseSeconds);
		if (mAdState == AdState.UnAd)
		{
			mUnAdTime += elapseSeconds;
			if (mUnAdTime >= mUnAdTimeDuration)
			{
				OnRefresh();
				mUnAdTime = 0f;
			}
		}
		if (mCloseState == CloseState.Hide)
		{
			mShowCloseTime += elapseSeconds;
			if (mShowCloseTime >= mShowCloseDuration)
			{
				mCloseButton.gameObject.SetActive(true);
				mCloseState = CloseState.Show;
				mShowCloseTime = 0f;
			}
		}
	}

	protected override void OnClose(object userData)
	{
		base.OnClose(userData);
		if ((bool)mAdBtn)
		{
			mAdBtn.onClick.RemoveListener(OnClickAdButton);
		}
		if ((bool)mCloseButton)
		{
			mCloseButton.onClick.RemoveListener(OnClickCloseButton);
		}
	}

	protected override void OnUnload()
	{
		mIsInit = false;
		for (int i = 0; i < mLoadedAsserts.Count; i++)
		{
			Mod.Resource.UnloadAsset(mLoadedAsserts[i]);
		}
		mLoadedAsserts.Clear();
	}

	private void OnRefresh()
	{
		if (!Mod.UI.UIFormIsOpen(UIFormId.InstrumentUnlockForm))
		{
			return;
		}
		mAdState = AdState.Null;
		mUnAdTime = 0f;
		if (MonoSingleton<GameTools>.Instacne.CanShowAd(ADScene.NONE))
		{
			if ((bool)mSetUIGrey)
			{
				mSetUIGrey.SetGrey(false);
			}
			mAdState = AdState.Ad;
		}
		else
		{
			if ((bool)mSetUIGrey)
			{
				mSetUIGrey.SetGrey(true);
			}
			mAdState = AdState.UnAd;
		}
	}

	public void OnClickCloseButton()
	{
		Mod.UI.CloseUIForm(UIFormId.InstrumentUnlockForm);
	}

	private void OnClickAdButton()
	{
		if (mAdState == AdState.Ad && MonoSingleton<GameTools>.Instacne.CanShowAd(ADScene.NONE))
		{
			InfocUtils.Report_rollingsky2_games_ads(32, 0, 1, 0, 3, 0);
			MonoSingleton<GameTools>.Instacne.PlayerVideoAd(ADScene.NONE, OnAdSuccess, OnAdFail, OnAdFail);
		}
	}

	private void OnAdSuccess(ADScene adScen = ADScene.NONE)
	{
		if (Mod.UI.UIFormIsOpen(UIFormId.InstrumentUnlockForm))
		{
			InfocUtils.Report_rollingsky2_games_ads(32, 0, 1, 0, 4, 0);
			PlayerDataModule.Instance.ForceUpInstrumentLevel(mCurrentID, mToLV);
			OnClickCloseButton();
		}
	}

	private void OnAdFail(ADScene adScen = ADScene.NONE)
	{
		if (Mod.UI.UIFormIsOpen(UIFormId.InstrumentUnlockForm))
		{
			OnClickCloseButton();
		}
	}
}
