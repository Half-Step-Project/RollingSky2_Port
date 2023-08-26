using System;
using System.Collections.Generic;
using System.Text;
using GoogleMobileAds.Api;
using UnityEngine;

public class CMPBUAdsManager : MonoBehaviour
{
	public enum CMPBUBannerViewPosition
	{
		Top,
		Bottom
	}

	public enum CMPBUBannerViewSize
	{
		CMPBUBannerViewSize_Banner600_90,
		CMPBUBannerViewSize_Banner600_100,
		CMPBUBannerViewSize_Banner600_150,
		CMPBUBannerViewSize_Banner600_260,
		CMPBUBannerViewSize_Banner600_286,
		CMPBUBannerViewSize_Banner600_300,
		CMPBUBannerViewSize_Banner600_388,
		CMPBUBannerViewSize_Banner600_400,
		CMPBUBannerViewSize_Banner600_500,
		CMPBUBannerViewSize_Feed228_150,
		CMPBUBannerViewSize_Feed690_388,
		CMPBUBannerViewSize_Interstitial600_400,
		CMPBUBannerViewSize_Interstitial600_600,
		CMPBUBannerViewSize_Interstitial600_900,
		CMPBUBannerViewSize_DrawFullScreen
	}

	public enum CMPBUStaticInterstitialSize
	{
		CMPBUStaticInterstitialSize600_400,
		CMPBUStaticInterstitialSize600_600,
		CMPBUStaticInterstitialSize600_900
	}

	private static CMPBUAdsManager instance;

	private const string GameObject_Name = "CMPBUAdsUtil";

	public const string cmpBUAdsSdkVersion = "1.3.6";

	private static string s_cmplayadsTableName;

	private int retryCount;

	private int ISRetryCount;

	private int StaticISRetryCount;

	private string RVAdUnitId = "";

	private string ISAdUnitId = "";

	private string StaticISAdUnitId = "";

	private string BannerAdUnitId = "";

	private string NativeAdUnitId = "";

	private bool isBannerAdLoaded;

	private bool isBannerAdLoading;

	private bool isNativeAdLoading;

	private bool isInterstitialAdLoading;

	private bool isStaticInterstitialAdLoading;

	private bool isRewardedVideoAdLoading;

	private long requestBannerTime;

	private long requestISTime;

	private long requestStaticISTime;

	private long requestRVTime;

	private long lastCallRequestBannerTime;

	private long lastCallRequestISTime;

	private long lastCallRequestStaticISTime;

	private long lastCallRequestRVTime;

	private long lastCallShowBannerTime;

	private long lastCallShowISTime;

	private long lastCallShowStaticISTime;

	private long lastCallShowRVTime;

	private long bannerScene1;

	private int bannerCid;

	private int bannerCompletion;

	private int bannerRetry_show;

	private long interstitialScene1;

	private int interstitialCid;

	private int interstitialCompletion;

	private int interstitialRetry_show;

	private long staticInterstitialScene1;

	private int staticInterstitialCid;

	private int staticInterstitialCompletion;

	private int staticInterstitialRetry_show;

	private long rewardVideoScene1;

	private int rewardVideoCid;

	private int rewardVideoCompletion;

	private int rewardVideoRetry_show;

	private bool isRewardVideoShowing;

	private CMPBUStaticInterstitialSize staticISSize;

	private static float s_timerInterval;

	private static bool s_isStartAdTimer;

	public static CMPBUAdsManager Instance
	{
		get
		{
			return instance;
		}
	}

	public event EventHandler<CMPBUAdsEventArgs> CMPBUAdsStaticInterstitialOnAdLoaded;

	public event EventHandler<AdFailedToLoadEventArgs> CMPBUAdsStaticInterstitialOnAdFailedToLoad;

	public event EventHandler<CMPBUAdsEventArgs> CMPBUAdsStaticInterstitialOnAdOpening;

	public event EventHandler<CMPBUAdsEventArgs> CMPBUAdsStaticInterstitialOnAdClosed;

	public event EventHandler<CMPBUAdsEventArgs> CMPBUAdsStaticInterstitialOnAdClick;

	public event EventHandler<CMPBUAdsEventArgs> CMPBUAdsStaticInterstitialShowFailed;

	public event EventHandler<CMPBUAdsEventArgs> CMPBUAdsInterstitialOnAdLoaded;

	public event EventHandler<AdFailedToLoadEventArgs> CMPBUAdsInterstitialOnAdFailedToLoad;

	public event EventHandler<CMPBUAdsEventArgs> CMPBUAdsInterstitialOnAdOpening;

	public event EventHandler<CMPBUAdsEventArgs> CMPBUAdsInterstitialOnAdClosed;

	public event EventHandler<CMPBUAdsEventArgs> CMPBUAdsInterstitialOnAdClick;

	public event EventHandler<CMPBUAdsEventArgs> CMPBUAdsInterstitialShowFailed;

	public event EventHandler<CMPBUAdsEventArgs> CMPBUAdsRewardVideoOnAdLoaded;

	public event EventHandler<AdErrorEventArgs> CMPBUAdsRewardVideoDidFailToLoad;

	public event EventHandler<CMPBUAdsEventArgs> CMPBUAdsRewardVideoOnAdOpening;

	public event EventHandler<CMPBUAdsEventArgs> CMPBUAdsRewardVideoOnAdClosed;

	public event EventHandler<Reward> CMPBUAdsRewardVideoOnAdRewarded;

	public event EventHandler<CMPBUAdsEventArgs> CMPBUAdsRewardVideoOnAdClick;

	public event EventHandler<AdErrorEventArgs> CMPBUAdsRewardVideoShowFailed;

	public event EventHandler<EventArgs> CMPBUAdsBannerOnAdLoaded;

	public event EventHandler<AdFailedToLoadEventArgs> CMPBUAdsBannerOnAdFailedToLoad;

	public event EventHandler<EventArgs> CMPBUAdsBannerOnAdOpening;

	public event EventHandler<EventArgs> CMPBUAdsBannerOnAdClosed;

	public event EventHandler<EventArgs> CMPBUAdsBannerOnAdLeavingApplication;

	public event EventHandler<EventArgs> CMPBUAdsNativeOnAdLoaded;

	public event EventHandler<AdFailedToLoadEventArgs> CMPBUAdsNativeOnAdFailedToLoad;

	public event EventHandler<EventArgs> CMPBUAdsNativeOnAdOpening;

	public event EventHandler<EventArgs> CMPBUAdsNativeOnAdClosed;

	public event EventHandler<EventArgs> CMPBUAdsNativeOnAdLeavingApplication;

	private void Start()
	{
		RegisteredNetWorkChangeReceiver();
		RegisteredCloudDataRefreshReceiver();
	}

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
			if (!base.gameObject.name.Equals("CMPBUAdsUtil"))
			{
				base.gameObject.name = "CMPBUAdsUtil";
			}
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
	}

	public static void Initialize(string appid, bool isAgreedGDPRPolicy, int timerInterval = 15)
	{
		s_timerInterval = timerInterval;
		if (appid == null || appid == "")
		{
			Debug.Log("CMPBUAdsManager Initialize error: please input correct appid");
		}
		else if (instance == null)
		{
			Debug.Log("Initialize admobManager");
			GameObject gameObject = GameObject.Find("CMPBUAdsUtil");
			if (gameObject == null)
			{
				gameObject = new GameObject("AdmobUtil");
			}
			if (gameObject != null)
			{
				gameObject.AddComponent<CMPBUAdsManager>();
			}
		}
	}

	private void StartBUAdTimer()
	{
		if (s_timerInterval > 0f && !s_isStartAdTimer)
		{
			s_isStartAdTimer = true;
			InvokeRepeating("TryRequestBUAds", 0f, s_timerInterval);
		}
	}

	private void TryRequestBUAds()
	{
		if (!IsRewardedVideoReady() || !IsInterstitialReady())
		{
			if (!IsRewardedVideoReady())
			{
				RequestRewardedVideoAd(RVAdUnitId);
			}
			if (!IsInterstitialReady())
			{
				RequestInterstitialAd(ISAdUnitId);
			}
		}
		else if (s_isStartAdTimer)
		{
			s_isStartAdTimer = false;
			CancelInvoke("TryRequestBUAds");
		}
	}

	public static void SetCMPlayAdsTableName(string tableName)
	{
		s_cmplayadsTableName = tableName;
	}

	public void ResetInterstitialAdUnitId(string adUnitId)
	{
		ISAdUnitId = adUnitId;
	}

	public void ResetRewardVideoAdUnitId(string adUnitId)
	{
		RVAdUnitId = adUnitId;
	}

	public void ResetBannerAdUnitId(string adUnitId)
	{
		BannerAdUnitId = adUnitId;
	}

	public void ResetNativeAdUnitId(string adUnitId)
	{
		NativeAdUnitId = adUnitId;
	}

	public void setStaticInterstitialSize(CMPBUStaticInterstitialSize size)
	{
	}

	public void RequestStaticInterstitialAd(string adUnitId, CMPBUStaticInterstitialSize size = CMPBUStaticInterstitialSize.CMPBUStaticInterstitialSize600_400, bool isRetry = false)
	{
		staticISSize = size;
		ReportAdmobAd("", 8, 101, true, 0, false, 0L);
		lastCallRequestStaticISTime = GetTimeStamp();
		if (isRetry)
		{
			StaticISRetryCount++;
		}
		else
		{
			StaticISRetryCount = 0;
		}
		if (adUnitId == null || adUnitId == "")
		{
			Debug.Log("CMPBUAdsManager Request static InterstitialAd error: please input correct adUnitId");
			return;
		}
		if (IsStaticInterstitialReady())
		{
			Debug.Log("CMPBUAdsManager Request static InterstitialAd warning: is loaded");
			ReportAdmobAd("", 1, 105, true, 0, false, 0L);
			return;
		}
		if (isStaticInterstitialAdLoading)
		{
			Debug.Log("CMPBUAdsManager Request static InterstitialAd warning: is loading");
			ReportAdmobAd("", 1, 106, true, 0, false, 0L);
			return;
		}
		setStaticInterstitialSize(size);
		StaticISAdUnitId = adUnitId;
		requestStaticISTime = GetTimeStamp();
		isStaticInterstitialAdLoading = true;
		ReportAdmobAd("", 8, 8, true, 0, false, 0L);
	}

	private void requestSingleStaticInterstitialAd(string adUnitId, bool isRetry = false)
	{
		ReportAdmobAd("", 8, 101, true, 0, false, 0L);
		lastCallRequestStaticISTime = GetTimeStamp();
		if (isRetry)
		{
			StaticISRetryCount++;
		}
		else
		{
			StaticISRetryCount = 0;
		}
		requestStaticISTime = GetTimeStamp();
		isStaticInterstitialAdLoading = true;
		ReportAdmobAd("", 8, 8, true, 0, false, 0L);
		setStaticInterstitialSize(staticISSize);
	}

	public bool IsStaticInterstitialAdLoading()
	{
		return isStaticInterstitialAdLoading;
	}

	public bool IsStaticInterstitialReady()
	{
		return false;
	}

	public bool IsStaticInterstitialReadyWithAdUnitId(string adUnitId)
	{
		return false;
	}

	public bool ShowStaticInterstitialAd(long scene1 = 0L, int cid = 0, int completion = 0, int retry_show = 0)
	{
		staticInterstitialScene1 = scene1;
		staticInterstitialCid = cid;
		staticInterstitialCompletion = completion;
		staticInterstitialRetry_show = retry_show;
		ReportAdShowChance(8, scene1, cid, completion, retry_show);
		lastCallShowStaticISTime = GetTimeStamp();
		if (IsStaticInterstitialReady())
		{
			return true;
		}
		int failure = 2;
		if (isStaticInterstitialAdLoading)
		{
			failure = 1;
		}
		ReportAdmobAd("", 8, 10, true, failure, false, scene1, cid, completion, retry_show);
		return false;
	}

	public bool ShowStaticInterstitialAdWithAdUnitId(string adUnitId, long scene1 = 0L, int cid = 0, int completion = 0, int retry_show = 0)
	{
		staticInterstitialScene1 = scene1;
		staticInterstitialCid = cid;
		staticInterstitialCompletion = completion;
		staticInterstitialRetry_show = retry_show;
		ReportAdShowChance(8, scene1, cid, completion, retry_show);
		lastCallShowStaticISTime = GetTimeStamp();
		if (IsStaticInterstitialReadyWithAdUnitId(adUnitId))
		{
			return true;
		}
		int failure = 2;
		if (isStaticInterstitialAdLoading)
		{
			failure = 1;
		}
		ReportAdmobAd("", 8, 10, true, failure, false, scene1, cid, completion, retry_show);
		return false;
	}

	public void RequestInterstitialAd(string adUnitId, bool isRetry = false)
	{
		ReportAdmobAd("", 1, 101, true, 0, false, 0L);
		lastCallRequestISTime = GetTimeStamp();
		if (isRetry)
		{
			ISRetryCount++;
		}
		else
		{
			ISRetryCount = 0;
		}
		if (adUnitId == null || adUnitId == "")
		{
			Debug.Log("CMPBUAdsManager RequestInterstitialAd error: please input correct adUnitId");
			return;
		}
		if (IsInterstitialReady())
		{
			Debug.Log("CMPBUAdsManager RequestInterstitialAd warning: is loaded");
			ReportAdmobAd("", 1, 105, true, 0, false, 0L);
			return;
		}
		if (isInterstitialAdLoading)
		{
			Debug.Log("CMPBUAdsManager RequestInterstitialAd warning: is loading");
			ReportAdmobAd("", 1, 106, true, 0, false, 0L);
			return;
		}
		ISAdUnitId = adUnitId;
		requestISTime = GetTimeStamp();
		isInterstitialAdLoading = true;
		ReportAdmobAd("", 1, 8, true, 0, false, 0L);
	}

	private void requestSingleInterstitialAd(string adUnitId, bool isRetry = false)
	{
		ReportAdmobAd("", 1, 101, true, 0, false, 0L);
		lastCallRequestISTime = GetTimeStamp();
		if (isRetry)
		{
			ISRetryCount++;
		}
		else
		{
			ISRetryCount = 0;
		}
		requestISTime = GetTimeStamp();
		isInterstitialAdLoading = true;
		ReportAdmobAd("", 1, 8, true, 0, false, 0L);
	}

	public bool IsInterstitialAdLoading()
	{
		return isInterstitialAdLoading;
	}

	public bool IsInterstitialReady()
	{
		return false;
	}

	public bool IsInterstitialReadyWithAdUnitId(string adUnitId)
	{
		return false;
	}

	public bool ShowInterstitialAd(long scene1 = 0L, int cid = 0, int completion = 0, int retry_show = 0)
	{
		interstitialScene1 = scene1;
		interstitialCid = cid;
		interstitialCompletion = completion;
		interstitialRetry_show = retry_show;
		ReportAdShowChance(1, scene1, cid, completion, retry_show);
		lastCallShowISTime = GetTimeStamp();
		if (IsInterstitialReady())
		{
			return true;
		}
		int failure = 2;
		if (isInterstitialAdLoading)
		{
			failure = 1;
		}
		ReportAdmobAd("", 1, 10, true, failure, false, scene1, cid, completion, retry_show);
		return false;
	}

	public bool ShowInterstitialAdWithAdUnitId(string adUnitId, long scene1 = 0L, int cid = 0, int completion = 0, int retry_show = 0)
	{
		interstitialScene1 = scene1;
		interstitialCid = cid;
		interstitialCompletion = completion;
		interstitialRetry_show = retry_show;
		ReportAdShowChance(1, scene1, cid, completion, retry_show);
		lastCallShowISTime = GetTimeStamp();
		if (IsInterstitialReadyWithAdUnitId(adUnitId))
		{
			return true;
		}
		int failure = 2;
		if (isInterstitialAdLoading)
		{
			failure = 1;
		}
		ReportAdmobAd("", 1, 10, true, failure, false, scene1, cid, completion, retry_show);
		return false;
	}

	private void RequestSingleRewardedVideoAd(string adUnitId, bool isRetry = false)
	{
		ReportAdmobAd("", 5, 101, true, 0, false, 0L);
		lastCallRequestRVTime = GetTimeStamp();
		if (adUnitId == null || adUnitId == "")
		{
			Debug.Log("CMPBUAdsManager RequestRewardedVideoAd error: please input correct adUnitId");
			return;
		}
		if (isRetry)
		{
			retryCount++;
		}
		else
		{
			retryCount = 0;
		}
		requestRVTime = GetTimeStamp();
		isRewardedVideoAdLoading = true;
		ReportAdmobAd("", 5, 8, true, 0, false, 0L);
	}

	public void RequestRewardedVideoAd(string adUnitId, bool isRetry = false)
	{
		ReportAdmobAd("", 5, 101, true, 0, false, 0L);
		lastCallRequestRVTime = GetTimeStamp();
		if (adUnitId == null || adUnitId == "")
		{
			Debug.Log("CMPBUAdsManager RequestRewardedVideoAd error: please input correct adUnitId");
			return;
		}
		if (IsRewardedVideoReady())
		{
			Debug.Log("CMPBUAdsManager RequestRewardedVideoAd warning: is loaded");
			ReportAdmobAd("", 5, 105, true, 0, false, 0L);
			return;
		}
		if (isRewardedVideoAdLoading)
		{
			Debug.Log("CMPBUAdsManager RequestRewardedVideoAd warning: is loading");
			ReportAdmobAd("", 5, 106, true, 0, false, 0L);
			return;
		}
		if (isRetry)
		{
			retryCount++;
		}
		else
		{
			retryCount = 0;
		}
		RVAdUnitId = adUnitId;
		requestRVTime = GetTimeStamp();
		isRewardedVideoAdLoading = true;
		ReportAdmobAd("", 5, 8, true, 0, false, 0L);
	}

	public bool ShowRewardedVideoAd(long scene1 = 0L, int cid = 0, int completion = 0, int retry_show = 0)
	{
		rewardVideoScene1 = scene1;
		rewardVideoCid = cid;
		rewardVideoCompletion = completion;
		rewardVideoRetry_show = retry_show;
		ReportAdShowChance(5, scene1, cid, completion, retry_show);
		lastCallShowRVTime = GetTimeStamp();
		if (IsRewardedVideoReady())
		{
			return true;
		}
		int failure = 2;
		if (isRewardedVideoAdLoading)
		{
			failure = 1;
		}
		ReportAdmobAd("", 5, 10, true, failure, false, scene1, cid, completion, retry_show);
		RequestRewardedVideoAd(RVAdUnitId);
		return false;
	}

	public bool ShowRewardedVideoAdWithAdUnitId(string adUnitId, long scene1 = 0L, int cid = 0, int completion = 0, int retry_show = 0)
	{
		rewardVideoScene1 = scene1;
		rewardVideoCid = cid;
		rewardVideoCompletion = completion;
		rewardVideoRetry_show = retry_show;
		ReportAdShowChance(5, scene1, cid, completion, retry_show);
		lastCallShowRVTime = GetTimeStamp();
		if (IsRewardedVideoReadyWithAdUnitId(adUnitId))
		{
			return true;
		}
		int failure = 2;
		if (isRewardedVideoAdLoading)
		{
			failure = 1;
		}
		ReportAdmobAd("", 5, 10, true, failure, false, scene1, cid, completion, retry_show);
		RequestRewardedVideoAd(RVAdUnitId);
		return false;
	}

	public bool IsRewardedVideoAdLoading()
	{
		return isRewardedVideoAdLoading;
	}

	public bool IsRewardedVideoReady()
	{
		return false;
	}

	public bool IsRewardedVideoReadyWithAdUnitId(string adUnitId)
	{
		return false;
	}

	public void SetCustomBannerPosition(string leftMargin, string bottomMargin)
	{
		string text = leftMargin + "|" + bottomMargin;
	}

	public void SetBannerRefreshInterval(int interval)
	{
	}

	public void RequestBanner(string adUnitId, CMPBUBannerViewSize size = CMPBUBannerViewSize.CMPBUBannerViewSize_Banner600_100, CMPBUBannerViewPosition postion = CMPBUBannerViewPosition.Bottom, int refreshInterval = 0)
	{
		SetBannerRefreshInterval(refreshInterval);
		if (adUnitId == null || adUnitId == "")
		{
			Debug.Log("CMPBUAdsManager RequestBanner error: please input correct adUnitId");
		}
		BannerAdUnitId = adUnitId;
		ReportAdmobAd("", 6, 101, true, 0, false, 0L);
		if (isBannerAdLoading)
		{
			Debug.Log("CMPBUAdsManager RequestBanner warning:banner is loading");
			ReportAdmobAd("", 6, 106, true, 0, false, 0L);
			return;
		}
		isBannerAdLoading = true;
		ReportAdmobAd("", 6, 8, true, 0, false, 0L);
		Convert.ToString((int)size);
		Convert.ToString((int)postion);
	}

	public bool IsBannerReady()
	{
		return false;
	}

	public void ShowBanner(long scene1 = 0L, int cid = 0, int completion = 0, int retry_show = 0)
	{
		if (!IsBannerReady())
		{
			int failure = 2;
			if (isBannerAdLoading)
			{
				failure = 1;
			}
			ReportAdmobAd("", 6, 10, true, failure, false, scene1, cid, completion, retry_show);
		}
	}

	public void HiddenBanner()
	{
	}

	public void RequestNativeAd(string adUnitId)
	{
		if (adUnitId == null || adUnitId == "")
		{
			Debug.Log("CMPBUAdsManager RequestNative error: please input correct adUnitId");
		}
		NativeAdUnitId = adUnitId;
		ReportAdmobAd("", 7, 101, true, 0, false, 0L);
		if (isNativeAdLoading)
		{
			Debug.Log("CMPBUAdsManager RequestNative warning:banner is loading");
			ReportAdmobAd("", 7, 106, true, 0, false, 0L);
		}
		else if (IsNativeAdReady())
		{
			Debug.Log("CMPBUAdsManager RequestNativeAd warning: is loaded");
			ReportAdmobAd("", 7, 105, true, 0, false, 0L);
		}
		else
		{
			isNativeAdLoading = true;
			ReportAdmobAd("", 7, 8, true, 0, false, 0L);
		}
	}

	public void ShowNativeAd(float leftMargin, float bottomMargin, float width, float height, long scene1 = 0L, int cid = 0, int completion = 0, int retry_show = 0)
	{
		if (!IsNativeAdReady())
		{
			int failure = 2;
			if (isNativeAdLoading)
			{
				failure = 1;
			}
			ReportAdmobAd("", 7, 10, true, failure, false, scene1, cid, completion, retry_show);
		}
	}

	public bool IsNativeAdReady()
	{
		return false;
	}

	public void HiddenNativeAd()
	{
	}

	public void CloseNativeAd()
	{
	}

	public void CMPBUStaticInterstitialDidLoad(string slotid)
	{
		ReportAdmobAd("", 8, 103, true, 0, false, 0L);
		if (this.CMPBUAdsStaticInterstitialOnAdLoaded != null)
		{
			this.CMPBUAdsStaticInterstitialOnAdLoaded(this, new CMPBUAdsEventArgs(slotid));
		}
		isStaticInterstitialAdLoading = false;
	}

	public void CMPBUStaticInterstitialDidOpen(string slotid)
	{
		ReportAdmobAd("", 8, 1, true, 0, false, 0L);
		if (this.CMPBUAdsStaticInterstitialOnAdOpening != null)
		{
			this.CMPBUAdsStaticInterstitialOnAdOpening(this, new CMPBUAdsEventArgs(slotid));
		}
	}

	public void CMPBUStaticInterstitialDidClose(string slotid)
	{
		ReportAdmobAd("", 8, 7, true, 0, false, 0L);
		if (this.CMPBUAdsStaticInterstitialOnAdClosed != null)
		{
			this.CMPBUAdsStaticInterstitialOnAdClosed(this, new CMPBUAdsEventArgs(slotid));
		}
		if (slotid != null && slotid != "")
		{
			requestSingleStaticInterstitialAd(slotid);
		}
		else
		{
			requestSingleStaticInterstitialAd(ISAdUnitId);
		}
	}

	public void CMPBUStaticInterstitialDidClick(string slotid)
	{
		ReportAdmobAd("", 8, 2, true, 0, false, 0L);
		if (this.CMPBUAdsStaticInterstitialOnAdClick != null)
		{
			this.CMPBUAdsStaticInterstitialOnAdClick(this, new CMPBUAdsEventArgs(slotid));
		}
	}

	public void CMPBUStaticInterstitialDidFailToLoad(string slotid)
	{
		isStaticInterstitialAdLoading = false;
		int failure = 4;
		if (this.CMPBUAdsStaticInterstitialOnAdFailedToLoad != null)
		{
			AdFailedToLoadEventArgs adFailedToLoadEventArgs = new AdFailedToLoadEventArgs();
			adFailedToLoadEventArgs.Message = slotid;
			this.CMPBUAdsStaticInterstitialOnAdFailedToLoad(this, adFailedToLoadEventArgs);
		}
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			failure = 2;
		}
		ReportAdmobAd("", 8, 104, true, failure, false, 0L);
		if (StaticISRetryCount < 3)
		{
			if (slotid != null && slotid != "")
			{
				requestSingleStaticInterstitialAd(slotid, true);
			}
			else
			{
				RequestStaticInterstitialAd(ISAdUnitId, staticISSize, true);
			}
		}
	}

	public void CMPBUInterstitialDidLoad(string slotid)
	{
		if (s_isStartAdTimer && IsRewardedVideoReady())
		{
			s_isStartAdTimer = false;
			CancelInvoke("TryRequestBUAds");
		}
		Debug.Log("CMPWMInterstitialDidLoad");
		ReportAdmobAd("", 1, 103, true, 0, false, 0L);
		if (this.CMPBUAdsInterstitialOnAdLoaded != null)
		{
			this.CMPBUAdsInterstitialOnAdLoaded(this, new CMPBUAdsEventArgs(slotid));
		}
		isInterstitialAdLoading = false;
	}

	public void CMPBUInterstitialDidOpen(string slotid)
	{
		Debug.Log("CMPWMInterstitialDidOpen");
		ReportAdmobAd("", 1, 1, true, 0, false, 0L);
		if (this.CMPBUAdsInterstitialOnAdOpening != null)
		{
			this.CMPBUAdsInterstitialOnAdOpening(this, new CMPBUAdsEventArgs(slotid));
		}
	}

	public void CMPBUInterstitialDidClose(string slotid)
	{
		Debug.Log("CMPWMInterstitialDidClose");
		ReportAdmobAd("", 1, 7, true, 0, false, 0L);
		if (this.CMPBUAdsInterstitialOnAdClosed != null)
		{
			this.CMPBUAdsInterstitialOnAdClosed(this, new CMPBUAdsEventArgs(slotid));
		}
		if (slotid != null && slotid != "")
		{
			requestSingleInterstitialAd(slotid);
		}
		else
		{
			requestSingleInterstitialAd(ISAdUnitId);
		}
	}

	public void CMPBUInterstitialDidClick(string slotid)
	{
		Debug.Log("CMPWMInterstitialDidClick");
		ReportAdmobAd("", 1, 2, true, 0, false, 0L);
		if (this.CMPBUAdsInterstitialOnAdClick != null)
		{
			this.CMPBUAdsInterstitialOnAdClick(this, new CMPBUAdsEventArgs(slotid));
		}
	}

	public void CMPBUInterstitialDidFailToLoad(string slotid)
	{
		isInterstitialAdLoading = false;
		int failure = 4;
		AdFailedToLoadEventArgs adFailedToLoadEventArgs = new AdFailedToLoadEventArgs();
		if (slotid != null)
		{
			adFailedToLoadEventArgs.Message = slotid;
		}
		else
		{
			adFailedToLoadEventArgs.Message = "no fill";
		}
		if (this.CMPBUAdsInterstitialOnAdFailedToLoad != null)
		{
			this.CMPBUAdsInterstitialOnAdFailedToLoad(this, adFailedToLoadEventArgs);
		}
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			failure = 2;
		}
		ReportAdmobAd("", 1, 104, true, failure, false, 0L);
		if (ISRetryCount < 3)
		{
			if (slotid != null && slotid != "")
			{
				requestSingleInterstitialAd(slotid, true);
			}
			else
			{
				RequestInterstitialAd(ISAdUnitId, true);
			}
		}
		else
		{
			StartBUAdTimer();
		}
	}

	public void CMPBURewardVideoDidLoad(string slotid)
	{
		if (s_isStartAdTimer && IsInterstitialReady())
		{
			s_isStartAdTimer = false;
			CancelInvoke("TryRequestBUAds");
		}
		ReportAdmobAd("", 5, 103, true, 0, false, 0L);
		if (this.CMPBUAdsRewardVideoOnAdLoaded != null)
		{
			this.CMPBUAdsRewardVideoOnAdLoaded(this, new CMPBUAdsEventArgs(slotid));
		}
		isRewardedVideoAdLoading = false;
	}

	public void CMPBURewardVideoDidShow(string slotid)
	{
		isRewardVideoShowing = true;
		ReportAdmobAd("", 5, 3, true, 0, false, 0L);
		if (this.CMPBUAdsRewardVideoOnAdOpening != null)
		{
			this.CMPBUAdsRewardVideoOnAdOpening(this, new CMPBUAdsEventArgs(slotid));
		}
	}

	public void CMPBURewardVideoShowFailed(string slotid)
	{
		if (this.CMPBUAdsRewardVideoShowFailed != null)
		{
			AdErrorEventArgs adErrorEventArgs = new AdErrorEventArgs();
			if (slotid != null)
			{
				adErrorEventArgs.Message = slotid;
			}
			else
			{
				adErrorEventArgs.Message = "no fill";
			}
			this.CMPBUAdsRewardVideoShowFailed(this, adErrorEventArgs);
		}
	}

	public void CMPBURewardVideoDidClose(string slotid)
	{
		isRewardVideoShowing = false;
		ReportAdmobAd("", 5, 7, true, 0, false, 0L);
		if (this.CMPBUAdsRewardVideoOnAdClosed != null)
		{
			this.CMPBUAdsRewardVideoOnAdClosed(this, new CMPBUAdsEventArgs(slotid));
		}
		if (slotid != null && slotid != "")
		{
			RequestSingleRewardedVideoAd(slotid);
		}
		else
		{
			RequestRewardedVideoAd(RVAdUnitId);
		}
	}

	public void CMPBURewardVideoDidClick(string slotid)
	{
		ReportAdmobAd("", 5, 2, true, 0, false, 0L);
		if (this.CMPBUAdsRewardVideoOnAdClick != null)
		{
			this.CMPBUAdsRewardVideoOnAdClick(this, new CMPBUAdsEventArgs(slotid));
		}
	}

	public void CMPBURewardVideoDidFailToLoad(string slotid)
	{
		isRewardedVideoAdLoading = false;
		int failure = 4;
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			failure = 2;
		}
		ReportAdmobAd("", 5, 104, true, failure, false, 0L);
		AdErrorEventArgs adErrorEventArgs = new AdErrorEventArgs();
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			adErrorEventArgs.Message = "1";
		}
		else
		{
			adErrorEventArgs.Message = "2";
		}
		if (this.CMPBUAdsRewardVideoDidFailToLoad != null)
		{
			this.CMPBUAdsRewardVideoDidFailToLoad(this, adErrorEventArgs);
		}
		if (retryCount < 3)
		{
			if (slotid != null && slotid != "")
			{
				RequestSingleRewardedVideoAd(slotid, true);
			}
			else
			{
				RequestRewardedVideoAd(RVAdUnitId, true);
			}
		}
		else
		{
			StartBUAdTimer();
		}
	}

	public void CMPBURewardVideoDidReward(string slotid)
	{
		ReportAdmobAd("", 5, 4, true, 0, false, 0L);
		if (this.CMPBUAdsRewardVideoOnAdRewarded != null)
		{
			Reward reward = new Reward();
			reward.Type = ((slotid != null) ? slotid : "");
			reward.Amount = 1.0;
			this.CMPBUAdsRewardVideoOnAdRewarded(this, reward);
		}
	}

	private void CMPBUBannerAdLoaded(string message)
	{
		ReportAdmobAd("", 6, 103, true, 0, false, 0L);
		isBannerAdLoading = false;
		if (this.CMPBUAdsBannerOnAdLoaded != null)
		{
			this.CMPBUAdsBannerOnAdLoaded(this, new EventArgs());
		}
	}

	private void CMPBUBannerAdFailedToLoad(string message)
	{
		isBannerAdLoading = false;
		int failure = 4;
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			failure = 2;
		}
		ReportAdmobAd("", 6, 104, true, failure, false, 0L);
		AdFailedToLoadEventArgs adFailedToLoadEventArgs = new AdFailedToLoadEventArgs();
		if (message != null)
		{
			adFailedToLoadEventArgs.Message = message;
		}
		else
		{
			adFailedToLoadEventArgs.Message = "no fill";
		}
		if (this.CMPBUAdsBannerOnAdFailedToLoad != null)
		{
			this.CMPBUAdsBannerOnAdFailedToLoad(this, adFailedToLoadEventArgs);
		}
	}

	private void CMPBUBannerAdOpened(string message)
	{
		ReportAdmobAd("", 6, 1, true, 0, false, 0L);
		if (this.CMPBUAdsBannerOnAdOpening != null)
		{
			this.CMPBUAdsBannerOnAdOpening(this, new EventArgs());
		}
	}

	private void CMPBUBannerAdClosed(string message)
	{
		ReportAdmobAd("", 6, 7, true, 0, false, 0L);
		if (this.CMPBUAdsBannerOnAdClosed != null)
		{
			this.CMPBUAdsBannerOnAdClosed(this, new EventArgs());
		}
	}

	private void CMPBUBannerAdLeavingApplication(string message)
	{
		ReportAdmobAd("", 6, 2, true, 0, false, 0L);
		if (this.CMPBUAdsBannerOnAdLeavingApplication != null)
		{
			this.CMPBUAdsBannerOnAdLeavingApplication(this, new EventArgs());
		}
	}

	private void CMPBUNativeAdLoaded(string message)
	{
		ReportAdmobAd("", 7, 103, true, 0, false, 0L);
		isNativeAdLoading = false;
		if (this.CMPBUAdsNativeOnAdLoaded != null)
		{
			this.CMPBUAdsNativeOnAdLoaded(this, new EventArgs());
		}
	}

	private void CMPBUNativeAdFailedToLoad(string message)
	{
		isNativeAdLoading = false;
		int failure = 4;
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			failure = 2;
		}
		ReportAdmobAd("", 7, 104, true, failure, false, 0L);
		AdFailedToLoadEventArgs adFailedToLoadEventArgs = new AdFailedToLoadEventArgs();
		if (message != null)
		{
			adFailedToLoadEventArgs.Message = message;
		}
		else
		{
			adFailedToLoadEventArgs.Message = "no fill";
		}
		if (this.CMPBUAdsNativeOnAdFailedToLoad != null)
		{
			this.CMPBUAdsNativeOnAdFailedToLoad(this, adFailedToLoadEventArgs);
		}
	}

	private void CMPBUNativeAdOpened(string message)
	{
		ReportAdmobAd("", 7, 1, true, 0, false, 0L);
		if (this.CMPBUAdsNativeOnAdOpening != null)
		{
			this.CMPBUAdsNativeOnAdOpening(this, new EventArgs());
		}
	}

	private void CMPBUNativeAdClosed(string message)
	{
		ReportAdmobAd("", 7, 7, true, 0, false, 0L);
		if (this.CMPBUAdsNativeOnAdClosed != null)
		{
			this.CMPBUAdsNativeOnAdClosed(this, new EventArgs());
		}
	}

	private void CMPBUNativeAdLeavingApplication(string message)
	{
		ReportAdmobAd("", 7, 2, true, 0, false, 0L);
		if (this.CMPBUAdsNativeOnAdLeavingApplication != null)
		{
			this.CMPBUAdsNativeOnAdLeavingApplication(this, new EventArgs());
		}
	}

	public void ReportAdmobAd(string adapterName, int scene, int action, bool commercial, int failure, bool remove_ads, long scene1 = 0L, int cid = 0, int completion = 0, int retry_show = 0)
	{
		int ad_source = 10;
		string adapterName2 = "";
		switch (scene)
		{
		case 1:
			adapterName2 = "WMInterstitialCustomEvent";
			break;
		case 5:
			adapterName2 = "WMRewardVideoCustomEvent";
			break;
		}
		ReportAdmobAdData(adapterName2, scene, action, ad_source, commercial, failure, remove_ads, scene1, cid, completion, retry_show);
	}

	public void ReportAdmobAdData(string adapterName, int scene, int action, int ad_source, bool commercial, int failure, bool remove_ads, long scene1 = 0L, int cid = 0, int completion = 0, int retry_show = 0)
	{
		long num = 0L;
		int num2 = 0;
		long timeStamp = GetTimeStamp();
		switch (action)
		{
		case 10:
			num2 = 2;
			if (scene == 1 && isInterstitialAdLoading)
			{
				num2 = 1;
			}
			else if (scene == 5 && isRewardedVideoAdLoading)
			{
				num2 = 1;
			}
			else if (scene == 6 && isBannerAdLoading)
			{
				num2 = 1;
			}
			else if (scene == 7 && isNativeAdLoading)
			{
				num2 = 1;
			}
			break;
		case 103:
		case 104:
			switch (scene)
			{
			case 1:
				num = timeStamp - requestISTime;
				break;
			case 5:
				num = timeStamp - requestRVTime;
				break;
			case 6:
				num = timeStamp - requestBannerTime;
				break;
			}
			break;
		case 101:
			if (scene == 1 && lastCallRequestISTime > 0)
			{
				num = timeStamp - lastCallRequestISTime;
			}
			else if (scene == 5 && lastCallRequestRVTime > 0)
			{
				num = timeStamp - lastCallRequestRVTime;
			}
			else if (scene == 6 && lastCallRequestBannerTime > 0)
			{
				num = timeStamp - lastCallRequestBannerTime;
			}
			break;
		case 102:
			if (scene == 1 && lastCallShowISTime > 0)
			{
				num = timeStamp - lastCallShowISTime;
			}
			else if (scene == 5 && lastCallShowRVTime > 0)
			{
				num = timeStamp - lastCallShowRVTime;
			}
			else if (scene == 6 && lastCallShowBannerTime > 0)
			{
				num = timeStamp - lastCallShowBannerTime;
			}
			break;
		}
		long num3 = (int)GetTimeStamp();
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["uptime2"] = num3;
		dictionary["player_time"] = num3.ToString();
		dictionary["scene"] = scene;
		dictionary["action"] = action;
		dictionary["ad_source"] = ad_source;
		dictionary["commercial"] = (commercial ? 1 : 0);
		dictionary["failure"] = failure;
		dictionary["remove_ads"] = (remove_ads ? 1 : 0);
		dictionary["reserve1"] = (int)num;
		dictionary["reserve2"] = num2;
		dictionary["reserve3"] = ((adapterName != null) ? adapterName : "");
		dictionary["reserve4"] = "1.3.6";
		dictionary["scene1"] = scene1;
		dictionary["cid"] = cid;
		dictionary["completion"] = completion;
		dictionary["retry_show"] = retry_show;
		dictionary["mediation_type"] = 10;
		string text = "";
		text = ((s_cmplayadsTableName == null || !(s_cmplayadsTableName != "")) ? "" : s_cmplayadsTableName);
		ReportInfocDictionaryData(text, dictionary);
	}

	public void ReportInfocDictionaryData(string tableName, Dictionary<string, object> reportDict)
	{
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		foreach (KeyValuePair<string, object> item in reportDict)
		{
			if (num > 0)
			{
				stringBuilder.Append("&");
			}
			stringBuilder.Append(item.Key);
			stringBuilder.Append("=");
			stringBuilder.Append(item.Value);
			num++;
		}
		ReportData(tableName, stringBuilder.ToString());
	}

	public long GetTimeStamp(bool bSecondsflag = true)
	{
		TimeSpan timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
		if (bSecondsflag)
		{
			return Convert.ToInt64(timeSpan.TotalSeconds);
		}
		return Convert.ToInt64(timeSpan.TotalMilliseconds);
	}

	public void ReportAdShowChance(int scene, long scene1, int cid, int completion, int retry_show)
	{
		ReportAdmobAd("", scene, 102, true, 0, false, scene1, cid, completion, retry_show);
	}

	public void ReportData(string tableName, string data)
	{
	}

	private void RegisteredCloudDataRefreshReceiver()
	{
	}

	public void onCloudDataRefreshNotify()
	{
	}

	public void RegisteredNetWorkChangeReceiver()
	{
	}

	public void onNetWorkAviable(string networkType)
	{
		Debug.Log("CMPBUAdsManager onNetWorkAviable networkType:" + networkType);
		if (isRewardVideoShowing && Application.internetReachability == NetworkReachability.NotReachable && this.CMPBUAdsRewardVideoShowFailed != null)
		{
			AdErrorEventArgs adErrorEventArgs = new AdErrorEventArgs();
			adErrorEventArgs.Message = "1";
			this.CMPBUAdsRewardVideoShowFailed(this, adErrorEventArgs);
		}
		if (!IsInterstitialReady())
		{
			RequestInterstitialAd(ISAdUnitId);
		}
		if (!IsRewardedVideoReady())
		{
			RequestRewardedVideoAd(RVAdUnitId);
		}
	}
}
