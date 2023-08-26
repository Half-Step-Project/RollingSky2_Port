using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using GoogleMobileAds.Api;
using UnityEngine;

public class CMPAdmobManager : MonoBehaviour
{
	public enum BannerViewSize
	{
		Normal,
		MediumRectangle,
		IABBanner,
		Leaderboard,
		SmartBanner
	}

	public enum BannerViewPostion
	{
		Top,
		Bottom,
		TopLeft,
		TopRight,
		BottomLeft,
		BottomRight,
		Center
	}

	public static class AdMessage
	{
		public const string REWARD_ON_SHOW_NETWORK_NOREACHABLE = "1";

		public const string REWARD_ON_SHOW_NOFILL = "2";
	}

	public const string sdkVersion = "1.3.6";

	private static CMPAdmobManager instance = null;

	private static string admobAppid = "";

	private static string s_cmplayadsTableName = null;

	private static readonly object padlock = new object();

	private static bool isAgreeGDPR = true;

	private InterstitialAd admobInterstitialAd;

	private RewardBasedVideoAd rewardBasedVideo;

	private RewardedAd rewardedAdHigh;

	private RewardedAd rewardedAdLow;

	private BannerView bannerView;

	private int RVHighRetryCount;

	private int RVLowRetryCount;

	private int ISRetryCount;

	private string RVAdUnitId = "";

	private string RVHighAdUnitId = "";

	private string RVLowAdUnitId = "";

	private string ISAdUnitId = "";

	private string BannerAdUnitId = "";

	private const string GameObject_Name = "AdmobUtil";

	private bool isBannerAdLoaded;

	private bool isBannerAdLoading;

	private bool isInterstitialAdLoading;

	private bool isRewardedVideoHighAdLoading;

	private bool isRewardedVideoLowAdLoading;

	private long requestBannerTime;

	private long requestISTime;

	private long requestHighRVTime;

	private long requestLowRVTime;

	private long lastCallRequestBannerTime;

	private long lastCallRequestISTime;

	private long lastCallRequestHighRVTime;

	private long lastCallRequestLowRVTime;

	private long lastCallShowBannerTime;

	private long lastCallShowISTime;

	private long lastCallShowHighRVTime;

	private long lastCallShowLowRVTime;

	private long bannerScene1;

	private int bannerCid;

	private int bannerCompletion;

	private int bannerRetry_show;

	private long interstitialScene1;

	private int interstitialCid;

	private int interstitialCompletion;

	private int interstitialRetry_show;

	private long rewardVideoScene1;

	private int rewardVideoCid;

	private int rewardVideoCompletion;

	private int rewardVideoRetry_show;

	private static Type unityAds_Type = null;

	private static Type vungle_Type = null;

	private static Type applovin_Type = null;

	private static Type ironsource_Type = null;

	private static Type chartboost_Type = null;

	private static Type tapjoy_Type = null;

	private EventArgs videoCloseEventArgs;

	private Reward videoRewardedEventArgs;

	private bool hasRewarded;

	private bool hasClosed;

	private bool isRewardVideoHighOccupied;

	private bool isRewardVideoLowOccupied;

	private bool isRewardVideoShowing;

	private int lastRewardVideoShowType;

	private static float s_timerInterval = 0f;

	private static bool s_isStartAdTimer = false;

	private static bool s_hasRegisteredNofication = false;

	public static CMPAdmobManager Instance
	{
		get
		{
			return instance;
		}
	}

	public event EventHandler<EventArgs> CMPAdmobInterstitialOnAdLoaded;

	public event EventHandler<AdFailedToLoadEventArgs> CMPAdmobInterstitialOnAdFailedToLoad;

	public event EventHandler<EventArgs> CMPAdmobInterstitialOnAdOpening;

	public event EventHandler<EventArgs> CMPAdmobInterstitialOnAdClosed;

	public event EventHandler<EventArgs> CMPAdmobInterstitialOnAdLeavingApplication;

	public event EventHandler<EventArgs> CMPAdmobRewardVideoOnAdLoaded;

	public event EventHandler<AdErrorEventArgs> CMPAdmobRewardVideoOnAdFailedToLoad;

	public event EventHandler<AdErrorEventArgs> CMPAdmobRewardVideoOnAdFailedToShow;

	public event EventHandler<EventArgs> CMPAdmobRewardVideoOnAdOpening;

	public event EventHandler<EventArgs> CMPAdmobRewardVideoOnAdStarted;

	public event EventHandler<EventArgs> CMPAdmobRewardVideoOnAdClosed;

	public event EventHandler<Reward> CMPAdmobRewardVideoOnAdRewarded;

	public event EventHandler<EventArgs> CMPAdmobRewardVideoOnAdLeavingApplication;

	public event EventHandler<EventArgs> CMPAdmobBannerOnAdLoaded;

	public event EventHandler<AdFailedToLoadEventArgs> CMPAdmobBannerOnAdFailedToLoad;

	public event EventHandler<EventArgs> CMPAdmobBannerOnAdOpening;

	public event EventHandler<EventArgs> CMPAdmobBannerOnAdClosed;

	public event EventHandler<EventArgs> CMPAdmobBannerOnAdLeavingApplication;

	private void Awake()
	{
		CMPUnityThreadDispatcher.Initialize();
		if (instance == null)
		{
			instance = this;
		}
	}

	public static void Initialize(string appid, bool isAgreedGDPRPolicy, int timerInterval = 15)
	{
		s_timerInterval = timerInterval;
		checkSDKAndSetGDPRStatus(isAgreedGDPRPolicy);
		SetGDPRAgreedStaus(isAgreedGDPRPolicy);
		if (appid == null || appid == "")
		{
			Debug.Log("CMPAdmobManager Initialize error: please input correct adUnitId");
			return;
		}
		if (instance == null)
		{
			Debug.Log("Initialize admobManager");
			GameObject gameObject = GameObject.Find("AdmobUtil");
			if (gameObject == null)
			{
				gameObject = new GameObject("AdmobUtil");
			}
			if (gameObject != null)
			{
				gameObject.AddComponent<CMPAdmobManager>();
			}
		}
		else if (!s_hasRegisteredNofication)
		{
			Debug.Log("Initialize admobManager RegisteredNofication");
			s_hasRegisteredNofication = true;
			instance.RegisteredNetWorkChangeReceiver();
			instance.RegisteredCloudDataRefreshReceiver();
		}
		admobAppid = appid;
		MobileAds.Initialize(admobAppid);
	}

	private void StartAdTimer()
	{
		if (s_timerInterval > 0f && !s_isStartAdTimer)
		{
			s_isStartAdTimer = true;
			InvokeRepeating("TryRequestAdsTimed", 0f, s_timerInterval);
		}
	}

	private void TryRequestAdsTimed()
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
			CancelInvoke("TryRequestAdsTimed");
		}
	}

	public static void SetGDPRAgreedStaus(bool isAgreed)
	{
		Debug.Log("CMPAdmobManager SetGDPRAgreedStaus: " + isAgreed);
		isAgreeGDPR = isAgreed;
	}

	public static void SetCMPlayAdsTableName(string tableName)
	{
		s_cmplayadsTableName = tableName;
	}

	public static bool GetGDPRAgreedStaus()
	{
		return isAgreeGDPR;
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

	private static void checkSDKAndSetGDPRStatus(bool isAgreed)
	{
		Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
		for (int i = 0; i < assemblies.Length; i++)
		{
			Type[] types = assemblies[i].GetTypes();
			foreach (Type type in types)
			{
				if (!type.IsClass)
				{
					continue;
				}
				if (unityAds_Type == null && type.Name == "UnityAds" && type.Namespace == "GoogleMobileAds.Api.Mediation.UnityAds")
				{
					unityAds_Type = type;
					MethodInfo method = unityAds_Type.GetMethod("SetGDPRConsentMetaData");
					if (method != null)
					{
						method.Invoke(null, new object[1] { isAgreed });
					}
				}
				if (vungle_Type == null && type.Name == "Vungle" && type.Namespace == "GoogleMobileAds.Api.Mediation.Vungle")
				{
					vungle_Type = type;
					MethodInfo method2 = vungle_Type.GetMethod("UpdateConsentStatus", new Type[2]
					{
						typeof(int),
						typeof(string)
					});
					if (method2 != null)
					{
						method2.Invoke(null, new object[2]
						{
							isAgreed ? 1 : 2,
							"1.0.0"
						});
					}
				}
				if (ironsource_Type == null && type.Name == "IronSource" && type.Namespace == "GoogleMobileAds.Api.Mediation.IronSource")
				{
					ironsource_Type = type;
					MethodInfo method3 = ironsource_Type.GetMethod("SetConsent");
					if (method3 != null)
					{
						method3.Invoke(null, new object[1] { isAgreed });
					}
				}
				if (applovin_Type == null && type.Name == "AppLovin" && type.Namespace == "GoogleMobileAds.Api.Mediation.AppLovin")
				{
					applovin_Type = type;
					MethodInfo method4 = applovin_Type.GetMethod("SetHasUserConsent");
					if (method4 != null)
					{
						method4.Invoke(null, new object[1] { isAgreed });
					}
				}
				if (chartboost_Type == null && type.Name == "Chartboost" && type.Namespace == "GoogleMobileAds.Api.Mediation.Chartboost")
				{
					chartboost_Type = type;
					MethodInfo method5 = chartboost_Type.GetMethod("RestrictDataCollection");
					if (method5 != null)
					{
						method5.Invoke(null, new object[1] { !isAgreed });
					}
				}
				if (tapjoy_Type == null && type.Name == "Tapjoy" && type.Namespace == "GoogleMobileAds.Api.Mediation.Tapjoy")
				{
					tapjoy_Type = type;
					MethodInfo method6 = tapjoy_Type.GetMethod("SetUserConsent");
					if (method6 != null)
					{
						method6.Invoke(null, new object[1] { isAgreed ? "1" : "0" });
					}
				}
			}
		}
	}

	private void initAdmobRewardedVideo()
	{
	}

	private void OnDeleteRewardLoaded(object sender, EventArgs args)
	{
		CMPUnityThreadDispatcher.RunOnMainThread(delegate
		{
			HandleRewardBasedVideoLoaded(sender, args);
		});
	}

	private void OnDeleteRewardFailedToLoad(object sender, AdErrorEventArgs args)
	{
		CMPUnityThreadDispatcher.RunOnMainThread(delegate
		{
			HandleRewardBasedVideoFailedToLoad(sender, args);
		});
	}

	private void OnDeleteRewardOpened(object sender, EventArgs args)
	{
		CMPUnityThreadDispatcher.RunOnMainThread(delegate
		{
			HandleRewardBasedVideoOpened(sender, args);
		});
	}

	private void OnDeleteRewardRewarded(object sender, Reward args)
	{
		CMPUnityThreadDispatcher.RunOnMainThread(delegate
		{
			HandleRewardBasedVideoRewarded(sender, args);
		});
	}

	private void OnDeleteRewardFailedToShow(object sender, AdErrorEventArgs args)
	{
		CMPUnityThreadDispatcher.RunOnMainThread(delegate
		{
			HandleRewardBasedVideoFailedToShow(sender, args);
		});
	}

	private void OnDeleteRewardClosed(object sender, EventArgs args)
	{
		CMPUnityThreadDispatcher.RunOnMainThread(delegate
		{
			HandleRewardBasedVideoClosed(sender, args);
		});
	}

	private void Start()
	{
		if (admobAppid != null && admobAppid != "" && !s_hasRegisteredNofication)
		{
			s_hasRegisteredNofication = true;
			RegisteredNetWorkChangeReceiver();
			RegisteredCloudDataRefreshReceiver();
		}
	}

	public void RequestBanner(string adUnitId, BannerViewSize size = BannerViewSize.Normal, BannerViewPostion postion = BannerViewPostion.Bottom)
	{
		BannerAdUnitId = adUnitId;
		long duringTime = 0L;
		if (lastCallRequestBannerTime > 0)
		{
			duringTime = GetTimeStamp() - lastCallRequestBannerTime;
		}
		ReportAdmobAd("", 6, 101, true, 0, false, duringTime, 0L);
		lastCallRequestBannerTime = GetTimeStamp();
		if (adUnitId == null || adUnitId == "")
		{
			Debug.Log("CMPAdmobManager RequestBanner error: please input correct adUnitId");
			return;
		}
		if (isBannerAdLoading)
		{
			Debug.Log("CMPAdmobManager RequestBanner warning:banner is loading");
			ReportAdmobAd("", 6, 106, true, 0, false, 0L, 0L);
			return;
		}
		AdSize adSize = AdSize.Banner;
		switch (size)
		{
		case BannerViewSize.Normal:
			adSize = AdSize.Banner;
			break;
		case BannerViewSize.MediumRectangle:
			adSize = AdSize.MediumRectangle;
			break;
		case BannerViewSize.IABBanner:
			adSize = AdSize.IABBanner;
			break;
		case BannerViewSize.Leaderboard:
			adSize = AdSize.Leaderboard;
			break;
		case BannerViewSize.SmartBanner:
			adSize = AdSize.SmartBanner;
			break;
		}
		if (bannerView != null)
		{
			bannerView.Destroy();
		}
		bannerView = new BannerView(adUnitId, adSize, (AdPosition)postion);
		bannerView.OnAdLoaded += OnDeleteHandleOnBannerAdLoaded;
		bannerView.OnAdFailedToLoad += OnDeleteHandleOnBannerAdFailedToLoad;
		bannerView.OnAdOpening += OnDeleteHandleOnBannerAdOpened;
		bannerView.OnAdClosed += OnDeleteHandleOnBannerAdClosed;
		bannerView.OnAdLeavingApplication += OnDeletHandleOnBannerAdLeavingApplication;
		AdRequest request = CreateAdRequest();
		bannerView.LoadAd(request);
		requestBannerTime = GetTimeStamp();
		isBannerAdLoaded = false;
		isBannerAdLoading = true;
		ReportAdmobAd("", 6, 8, true, 0, false, 0L, 0L);
	}

	private void OnDeleteHandleOnBannerAdLoaded(object sender, EventArgs args)
	{
		CMPUnityThreadDispatcher.RunOnMainThread(delegate
		{
			HandleOnBannerAdLoaded(sender, args);
		});
	}

	private void OnDeleteHandleOnBannerAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		CMPUnityThreadDispatcher.RunOnMainThread(delegate
		{
			HandleOnBannerAdFailedToLoad(sender, args);
		});
	}

	private void OnDeleteHandleOnBannerAdOpened(object sender, EventArgs args)
	{
		CMPUnityThreadDispatcher.RunOnMainThread(delegate
		{
			HandleOnBannerAdOpened(sender, args);
		});
	}

	private void OnDeleteHandleOnBannerAdClosed(object sender, EventArgs args)
	{
		CMPUnityThreadDispatcher.RunOnMainThread(delegate
		{
			HandleOnBannerAdClosed(sender, args);
		});
	}

	private void OnDeletHandleOnBannerAdLeavingApplication(object sender, EventArgs args)
	{
		CMPUnityThreadDispatcher.RunOnMainThread(delegate
		{
			HandleOnBannerAdLeavingApplication(sender, args);
		});
	}

	public bool IsBannerReady()
	{
		if (bannerView != null)
		{
			return isBannerAdLoaded;
		}
		return false;
	}

	public void ShowBannerView(long scene1 = 0L, int cid = 0, int completion = 0, int retry_show = 0)
	{
		bannerScene1 = scene1;
		bannerCid = cid;
		bannerCompletion = completion;
		bannerRetry_show = retry_show;
		long durationTime = 0L;
		if (lastCallShowBannerTime > 0)
		{
			durationTime = GetTimeStamp() - lastCallShowBannerTime;
		}
		ReportAdShowChance(6, scene1, cid, completion, retry_show, durationTime);
		lastCallShowBannerTime = GetTimeStamp();
		if (bannerView != null && isBannerAdLoaded)
		{
			bannerView.Show();
			return;
		}
		int failure = 2;
		if (isBannerAdLoading)
		{
			failure = 1;
		}
		ReportAdmobAd("", 6, 10, true, failure, false, 0L, scene1, cid, completion, retry_show);
	}

	public void HiddenBannerView()
	{
		if (bannerView != null)
		{
			bannerView.Hide();
		}
	}

	private AdRequest CreateAdRequest()
	{
		AdRequest.Builder builder = new AdRequest.Builder();
		if (!isAgreeGDPR)
		{
			builder.AddExtra("npa", "1");
		}
		return builder.Build();
	}

	public void RequestInterstitialAd(string adUnitId, bool isRetry = false)
	{
		long duringTime = 0L;
		if (lastCallRequestISTime > 0)
		{
			duringTime = GetTimeStamp() - lastCallRequestISTime;
		}
		ReportAdmobAd("", 1, 101, true, 0, false, duringTime, 0L);
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
			Debug.Log("CMPAdmobManager RequestInterstitialAd error: please input correct adUnitId");
			return;
		}
		if (admobInterstitialAd != null && admobInterstitialAd.IsLoaded())
		{
			Debug.Log("CMPAdmobManager RequestInterstitialAd warning: is loaded");
			ReportAdmobAd("", 1, 105, true, 0, false, 0L, 0L);
			return;
		}
		if (isInterstitialAdLoading)
		{
			Debug.Log("CMPAdmobManager RequestInterstitialAd warning: is loading");
			ReportAdmobAd("", 1, 106, true, 0, false, 0L, 0L);
			return;
		}
		ISAdUnitId = adUnitId;
		admobInterstitialAd = new InterstitialAd(adUnitId);
		admobInterstitialAd.OnAdLoaded += OnDeleteInterstitialOnAdLoaded;
		admobInterstitialAd.OnAdFailedToLoad += OnDeleteInterstitialOnAdFailedToLoad;
		admobInterstitialAd.OnAdOpening += OnDeleteInterstitialOnAdOpened;
		admobInterstitialAd.OnAdClosed += OnDeleteInterstitialOnAdClosed;
		admobInterstitialAd.OnAdLeavingApplication += OnDeleteInterstitialOnAdLeavingApplication;
		AdRequest request = CreateAdRequest();
		admobInterstitialAd.LoadAd(request);
		requestISTime = GetTimeStamp();
		isInterstitialAdLoading = true;
		ReportAdmobAd("", 1, 8, true, 0, false, 0L, 0L);
	}

	private void OnDeleteInterstitialOnAdLoaded(object sender, EventArgs args)
	{
		CMPUnityThreadDispatcher.RunOnMainThread(delegate
		{
			HandleOnAdLoaded(sender, args);
		});
	}

	private void OnDeleteInterstitialOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		CMPUnityThreadDispatcher.RunOnMainThread(delegate
		{
			HandleOnAdFailedToLoad(sender, args);
		});
	}

	private void OnDeleteInterstitialOnAdOpened(object sender, EventArgs args)
	{
		CMPUnityThreadDispatcher.RunOnMainThread(delegate
		{
			HandleOnAdOpened(sender, args);
		});
	}

	private void OnDeleteInterstitialOnAdClosed(object sender, EventArgs args)
	{
		CMPUnityThreadDispatcher.RunOnMainThread(delegate
		{
			HandleOnAdClosed(sender, args);
		});
	}

	private void OnDeleteInterstitialOnAdLeavingApplication(object sender, EventArgs args)
	{
		CMPUnityThreadDispatcher.RunOnMainThread(delegate
		{
			HandleOnAdLeavingApplication(sender, args);
		});
	}

	public bool IsInterstitialAdLoading()
	{
		return isInterstitialAdLoading;
	}

	public bool IsInterstitialReady()
	{
		if (admobInterstitialAd != null)
		{
			return admobInterstitialAd.IsLoaded();
		}
		return false;
	}

	public bool ShowInterstitialAd(long scene1 = 0L, int cid = 0, int completion = 0, int retry_show = 0)
	{
		interstitialScene1 = scene1;
		interstitialCid = cid;
		interstitialCompletion = completion;
		interstitialRetry_show = retry_show;
		long durationTime = 0L;
		if (lastCallShowISTime > 0)
		{
			durationTime = GetTimeStamp() - lastCallShowISTime;
		}
		ReportAdShowChance(1, scene1, cid, completion, retry_show, durationTime);
		lastCallShowISTime = GetTimeStamp();
		if (admobInterstitialAd != null && admobInterstitialAd.IsLoaded())
		{
			admobInterstitialAd.Show();
			return true;
		}
		int failure = 2;
		if (isInterstitialAdLoading)
		{
			failure = 1;
		}
		ReportAdmobAd("", 1, 10, true, failure, false, 0L, scene1, cid, completion, retry_show);
		RequestInterstitialAd(ISAdUnitId);
		return false;
	}

	public void RequestRewardedVideoAd(string adUnitId, bool isRetry = false)
	{
		if (adUnitId == null || adUnitId == "")
		{
			Debug.Log("CMPAdmobManager RequestRewardedVideoAd error: please input correct adUnitId");
			return;
		}
		RVAdUnitId = adUnitId;
		if (adUnitId.Contains("|"))
		{
			string[] array = adUnitId.Split('|');
			if (array != null && array.Length != 0)
			{
				if (array.Length > 1)
				{
					RVHighAdUnitId = array[0];
					RVLowAdUnitId = array[1];
				}
				else
				{
					RVHighAdUnitId = array[0];
					RVLowAdUnitId = array[0];
				}
			}
		}
		else
		{
			RVLowAdUnitId = adUnitId;
			RVHighAdUnitId = adUnitId;
		}
		Debug.Log("CMPAdmobManager RequestRewardedVideoAd  adUnitId:" + adUnitId + "    RVHighAdUnitId:" + RVHighAdUnitId + "    RVLowAdUnitId:" + RVLowAdUnitId);
		RequestHighRewardedVideoAd(RVHighAdUnitId);
		RequestLowRewardedVideoAd(RVLowAdUnitId);
	}

	private void RequestHighRewardedVideoAd(string adUnitId, bool isRetry = false)
	{
		Debug.Log("CMPAdmobManager RequestHighRewardedVideoAd adUnitId:" + adUnitId);
		long duringTime = 0L;
		if (lastCallRequestHighRVTime > 0)
		{
			duringTime = GetTimeStamp() - lastCallRequestHighRVTime;
		}
		ReportAdmobAd("", 5, 101, true, 0, false, duringTime, 0L);
		lastCallRequestHighRVTime = GetTimeStamp();
		if (rewardedAdHigh != null && rewardedAdHigh.IsLoaded())
		{
			Debug.Log("CMPAdmobManager RequestHighRewardedVideoAd warning: is loaded");
			ReportAdmobAd("", 5, 105, true, 0, false, 0L, 0L);
			return;
		}
		if (isRewardVideoHighOccupied)
		{
			Debug.Log("CMPAdmobManager RequestHighRewardedVideoAd error: reward video is occupied");
			ReportAdmobAd("", 5, 105, true, 0, false, 0L, 0L);
			return;
		}
		if (isRewardedVideoHighAdLoading)
		{
			Debug.Log("CMPAdmobManager RequestHighRewardedVideoAd warning: is loading");
			ReportAdmobAd("", 5, 106, true, 0, false, 0L, 0L);
			return;
		}
		if (isRetry)
		{
			RVHighRetryCount++;
		}
		else
		{
			RVHighRetryCount = 0;
		}
		rewardedAdHigh = new RewardedAd(adUnitId);
		rewardedAdHigh.OnAdLoaded += OnDeleteRewardLoaded;
		rewardedAdHigh.OnAdFailedToLoad += OnDeleteRewardFailedToLoad;
		rewardedAdHigh.OnAdOpening += OnDeleteRewardOpened;
		rewardedAdHigh.OnAdFailedToShow += OnDeleteRewardFailedToShow;
		rewardedAdHigh.OnUserEarnedReward += OnDeleteRewardRewarded;
		rewardedAdHigh.OnAdClosed += OnDeleteRewardClosed;
		AdRequest request = CreateAdRequest();
		rewardedAdHigh.LoadAd(request);
		Debug.Log("CMPAdmobManager RequestHighRewardedVideoAd rewardedAdHigh.LoadAd");
		requestHighRVTime = GetTimeStamp();
		isRewardedVideoHighAdLoading = true;
		ReportAdmobAd("", 5, 8, true, 0, false, 0L, 0L);
	}

	private void RequestLowRewardedVideoAd(string adUnitId, bool isRetry = false)
	{
		Debug.Log("CMPAdmobManager RequestLowRewardedVideoAd adUnitId:" + adUnitId);
		long duringTime = 0L;
		if (lastCallRequestLowRVTime > 0)
		{
			duringTime = GetTimeStamp() - lastCallRequestLowRVTime;
		}
		ReportAdmobAd("", 5, 101, true, 0, false, duringTime, 0L);
		lastCallRequestLowRVTime = GetTimeStamp();
		if (rewardedAdLow != null && rewardedAdLow.IsLoaded())
		{
			Debug.Log("CMPAdmobManager RequestLowRewardedVideoAd warning: is loaded");
			ReportAdmobAd("", 5, 105, true, 0, false, 0L, 0L);
			return;
		}
		if (isRewardVideoLowOccupied)
		{
			Debug.Log("CMPAdmobManager RequestLowRewardedVideoAd error: reward video is occupied");
			ReportAdmobAd("", 5, 105, true, 0, false, 0L, 0L);
			return;
		}
		if (isRewardedVideoLowAdLoading)
		{
			Debug.Log("CMPAdmobManager RequestLowRewardedVideoAd warning: is loading");
			ReportAdmobAd("", 5, 106, true, 0, false, 0L, 0L);
			return;
		}
		if (isRetry)
		{
			RVLowRetryCount++;
		}
		else
		{
			RVLowRetryCount = 0;
		}
		rewardedAdLow = new RewardedAd(adUnitId);
		rewardedAdLow.OnAdLoaded += OnDeleteRewardLoaded;
		rewardedAdLow.OnAdFailedToLoad += OnDeleteRewardFailedToLoad;
		rewardedAdLow.OnAdOpening += OnDeleteRewardOpened;
		rewardedAdLow.OnAdFailedToShow += OnDeleteRewardFailedToShow;
		rewardedAdLow.OnUserEarnedReward += OnDeleteRewardRewarded;
		rewardedAdLow.OnAdClosed += OnDeleteRewardClosed;
		AdRequest request = CreateAdRequest();
		rewardedAdLow.LoadAd(request);
		Debug.Log("CMPAdmobManager RequestLowRewardedVideoAd rewardedAdLow.LoadAd");
		requestLowRVTime = GetTimeStamp();
		isRewardedVideoLowAdLoading = true;
		ReportAdmobAd("", 5, 8, true, 0, false, 0L, 0L);
	}

	public bool ShowRewardedVideoAd(long scene1 = 0L, int cid = 0, int completion = 0, int retry_show = 0)
	{
		rewardVideoScene1 = scene1;
		rewardVideoCid = cid;
		rewardVideoCompletion = completion;
		rewardVideoRetry_show = retry_show;
		if (IsRewardedVideoReady())
		{
			if (lastRewardVideoShowType == 0)
			{
				if (rewardedAdHigh != null && rewardedAdHigh.IsLoaded())
				{
					showHighRewardedVideoAd(scene1, cid, completion, retry_show);
				}
				else if (rewardedAdLow != null && rewardedAdLow.IsLoaded())
				{
					showLowRewardedVideoAd(scene1, cid, completion, retry_show);
				}
			}
			else if (lastRewardVideoShowType == 1)
			{
				if (rewardedAdHigh != null && rewardedAdHigh.IsLoaded())
				{
					showHighRewardedVideoAd(scene1, cid, completion, retry_show);
				}
				else if (rewardedAdLow != null && rewardedAdLow.IsLoaded())
				{
					showLowRewardedVideoAd(scene1, cid, completion, retry_show);
				}
			}
			else if (lastRewardVideoShowType == 2)
			{
				if (rewardedAdLow != null && rewardedAdLow.IsLoaded())
				{
					showLowRewardedVideoAd(scene1, cid, completion, retry_show);
				}
				else if (rewardedAdHigh != null && rewardedAdHigh.IsLoaded())
				{
					showHighRewardedVideoAd(scene1, cid, completion, retry_show);
				}
			}
			else if (rewardedAdHigh != null && rewardedAdHigh.IsLoaded())
			{
				showHighRewardedVideoAd(scene1, cid, completion, retry_show);
			}
			else if (rewardedAdLow != null && rewardedAdLow.IsLoaded())
			{
				showLowRewardedVideoAd(scene1, cid, completion, retry_show);
			}
			return true;
		}
		int failure = 2;
		if (IsRewardedVideoAdLoading())
		{
			failure = 1;
		}
		ReportAdmobAd("", 5, 10, true, failure, false, 0L, scene1, cid, completion, retry_show);
		RequestRewardedVideoAd(RVAdUnitId);
		return false;
	}

	private void showLowRewardedVideoAd(long scene1 = 0L, int cid = 0, int completion = 0, int retry_show = 0)
	{
		if (rewardedAdLow != null && rewardedAdLow.IsLoaded())
		{
			lastRewardVideoShowType = 1;
			isRewardVideoLowOccupied = false;
			rewardedAdLow.Show();
			long durationTime = 0L;
			if (lastCallShowLowRVTime > 0)
			{
				durationTime = GetTimeStamp() - lastCallShowLowRVTime;
			}
			ReportAdShowChance(5, scene1, cid, completion, retry_show, durationTime);
			lastCallShowLowRVTime = GetTimeStamp();
			if (rewardedAdHigh != null && !rewardedAdHigh.IsLoaded())
			{
				RequestHighRewardedVideoAd(RVHighAdUnitId);
			}
		}
		else if (rewardedAdHigh != null && rewardedAdHigh.IsLoaded())
		{
			lastRewardVideoShowType = 2;
			isRewardVideoHighOccupied = false;
			rewardedAdHigh.Show();
			long durationTime2 = 0L;
			if (lastCallShowHighRVTime > 0)
			{
				durationTime2 = GetTimeStamp() - lastCallShowHighRVTime;
			}
			ReportAdShowChance(5, scene1, cid, completion, retry_show, durationTime2);
			lastCallShowHighRVTime = GetTimeStamp();
			RequestLowRewardedVideoAd(RVLowAdUnitId);
		}
	}

	private void showHighRewardedVideoAd(long scene1 = 0L, int cid = 0, int completion = 0, int retry_show = 0)
	{
		if (rewardedAdHigh != null && rewardedAdHigh.IsLoaded())
		{
			lastRewardVideoShowType = 2;
			isRewardVideoHighOccupied = false;
			rewardedAdHigh.Show();
			long durationTime = 0L;
			if (lastCallShowHighRVTime > 0)
			{
				durationTime = GetTimeStamp() - lastCallShowHighRVTime;
			}
			ReportAdShowChance(5, scene1, cid, completion, retry_show, durationTime);
			lastCallShowHighRVTime = GetTimeStamp();
			if (rewardedAdLow != null && !rewardedAdLow.IsLoaded())
			{
				RequestLowRewardedVideoAd(RVLowAdUnitId);
			}
		}
		else if (rewardedAdLow != null && rewardedAdLow.IsLoaded())
		{
			lastRewardVideoShowType = 1;
			isRewardVideoLowOccupied = false;
			rewardedAdLow.Show();
			long durationTime2 = 0L;
			if (lastCallShowLowRVTime > 0)
			{
				durationTime2 = GetTimeStamp() - lastCallShowLowRVTime;
			}
			ReportAdShowChance(5, scene1, cid, completion, retry_show, durationTime2);
			lastCallShowLowRVTime = GetTimeStamp();
			RequestHighRewardedVideoAd(RVHighAdUnitId);
		}
	}

	public bool IsRewardedVideoAdLoading()
	{
		if (!isRewardedVideoLowAdLoading)
		{
			return isRewardedVideoHighAdLoading;
		}
		return true;
	}

	public bool IsRewardedVideoReady()
	{
		if (rewardedAdHigh == null || !rewardedAdHigh.IsLoaded())
		{
			if (rewardedAdLow != null)
			{
				return rewardedAdLow.IsLoaded();
			}
			return false;
		}
		return true;
	}

	public void DelayHandleRewardBasedVideoClosed()
	{
		if (hasRewarded && this.CMPAdmobRewardVideoOnAdRewarded != null)
		{
			if (videoRewardedEventArgs != null)
			{
				this.CMPAdmobRewardVideoOnAdRewarded(this, videoRewardedEventArgs);
			}
			else
			{
				Reward e = new Reward();
				this.CMPAdmobRewardVideoOnAdRewarded(this, e);
			}
		}
		if (this.CMPAdmobRewardVideoOnAdClosed != null)
		{
			if (videoCloseEventArgs != null)
			{
				this.CMPAdmobRewardVideoOnAdClosed(this, videoCloseEventArgs);
				return;
			}
			EventArgs e2 = new EventArgs();
			this.CMPAdmobRewardVideoOnAdClosed(this, e2);
		}
	}

	public void ShowMediationTestSuite(string appid)
	{
	}

	public void HandleOnAdLoaded(object sender, EventArgs args)
	{
		if (s_isStartAdTimer && IsRewardedVideoReady())
		{
			s_isStartAdTimer = false;
			CancelInvoke("TryRequestAdsTimed");
		}
		if (this.CMPAdmobInterstitialOnAdLoaded != null)
		{
			this.CMPAdmobInterstitialOnAdLoaded(this, args);
		}
		isInterstitialAdLoading = false;
		MonoBehaviour.print("HandleAdLoaded event received: " + admobInterstitialAd.MediationAdapterClassName());
		long duringTime = 0L;
		if (requestISTime > 0)
		{
			duringTime = GetTimeStamp() - requestISTime;
		}
		ReportAdmobAd(admobInterstitialAd.MediationAdapterClassName(), 1, 103, true, 0, false, duringTime, 0L);
	}

	public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		if (this.CMPAdmobInterstitialOnAdFailedToLoad != null)
		{
			this.CMPAdmobInterstitialOnAdFailedToLoad(this, args);
		}
		isInterstitialAdLoading = false;
		MonoBehaviour.print("HandleFailedToReceiveAd event received with message: " + args.Message + "adapter name: " + admobInterstitialAd.MediationAdapterClassName());
		if (ISRetryCount < 3)
		{
			RequestInterstitialAd(ISAdUnitId, true);
		}
		else
		{
			StartAdTimer();
		}
		int failure = 4;
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			failure = 2;
		}
		long duringTime = 0L;
		if (requestISTime > 0)
		{
			duringTime = GetTimeStamp() - requestISTime;
		}
		ReportAdmobAd(admobInterstitialAd.MediationAdapterClassName(), 1, 104, true, failure, false, duringTime, 0L);
	}

	public void HandleOnAdOpened(object sender, EventArgs args)
	{
		if (this.CMPAdmobInterstitialOnAdOpening != null)
		{
			this.CMPAdmobInterstitialOnAdOpening(this, args);
		}
		MonoBehaviour.print("HandleAdOpened event received: " + admobInterstitialAd.MediationAdapterClassName());
		ReportAdmobAd(admobInterstitialAd.MediationAdapterClassName(), 1, 1, true, 0, false, 0L, interstitialScene1, interstitialCid, interstitialCompletion, interstitialRetry_show);
	}

	public void HandleOnAdClosed(object sender, EventArgs args)
	{
		ReportAdmobAd(admobInterstitialAd.MediationAdapterClassName(), 1, 7, true, 0, false, 0L, interstitialScene1, interstitialCid, interstitialCompletion, interstitialRetry_show);
		if (this.CMPAdmobInterstitialOnAdClosed != null)
		{
			this.CMPAdmobInterstitialOnAdClosed(this, args);
		}
		MonoBehaviour.print("HandleAdClosed event received: " + admobInterstitialAd.MediationAdapterClassName());
		admobInterstitialAd.Destroy();
		RequestInterstitialAd(ISAdUnitId);
	}

	public void HandleOnAdLeavingApplication(object sender, EventArgs args)
	{
		if (this.CMPAdmobInterstitialOnAdLeavingApplication != null)
		{
			this.CMPAdmobInterstitialOnAdLeavingApplication(this, args);
		}
		MonoBehaviour.print("HandleAdLeavingApplication event received: " + admobInterstitialAd.MediationAdapterClassName());
		ReportAdmobAd(admobInterstitialAd.MediationAdapterClassName(), 1, 2, true, 0, false, 0L, interstitialScene1, interstitialCid, interstitialCompletion, interstitialRetry_show);
	}

	public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
	{
		if (s_isStartAdTimer && IsInterstitialReady())
		{
			s_isStartAdTimer = false;
			CancelInvoke("TryRequestAdsTimed");
		}
		MonoBehaviour.print("HandleRewardBasedVideoLoaded event received");
		long duringTime = 0L;
		string text = "";
		string text2 = "unknow";
		if (sender != null)
		{
			text = ((RewardedAd)sender).MediationAdapterClassName();
			if (sender == rewardedAdLow)
			{
				text2 = "rewardedAdLow";
				isRewardedVideoLowAdLoading = false;
				RVLowRetryCount = 0;
				if (requestLowRVTime > 0)
				{
					duringTime = GetTimeStamp() - requestLowRVTime;
				}
				MonoBehaviour.print("HandleRewardBasedVideoLoaded event received: " + text + "     curRV:" + text2);
			}
			else if (sender == rewardedAdHigh)
			{
				text2 = "rewardedAdHigh";
				isRewardedVideoHighAdLoading = false;
				RVHighRetryCount = 0;
				if (requestHighRVTime > 0)
				{
					duringTime = GetTimeStamp() - requestHighRVTime;
				}
				MonoBehaviour.print("HandleRewardBasedVideoLoaded event received: " + text + "     curRV:" + text2);
			}
		}
		ReportAdmobAd(text, 5, 103, true, 0, false, duringTime, 0L);
		if (this.CMPAdmobRewardVideoOnAdLoaded != null)
		{
			this.CMPAdmobRewardVideoOnAdLoaded(this, args);
		}
	}

	public void HandleRewardBasedVideoFailedToLoad(object sender, AdErrorEventArgs args)
	{
		int failure = 4;
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			failure = 2;
		}
		string adapterName = "";
		string text = "unknow";
		long duringTime = 0L;
		if (sender != null)
		{
			adapterName = ((RewardedAd)sender).MediationAdapterClassName();
			if (sender == rewardedAdLow)
			{
				text = "rewardedAdLow";
				MonoBehaviour.print("HandleRewardBasedVideoFailedToLoad event received with message: " + args.Message + "     curRV:" + text);
				if (requestLowRVTime > 0)
				{
					duringTime = GetTimeStamp() - requestLowRVTime;
				}
				ReportAdmobAd(adapterName, 5, 104, true, failure, false, duringTime, 0L);
				isRewardedVideoLowAdLoading = false;
				if (RVLowRetryCount < 3)
				{
					RequestLowRewardedVideoAd(RVLowAdUnitId, true);
				}
				else
				{
					StartAdTimer();
				}
			}
			else if (sender == rewardedAdHigh)
			{
				text = "rewardedAdHigh";
				MonoBehaviour.print("HandleRewardBasedVideoFailedToLoad event received with message: " + args.Message + "     curRV:" + text);
				if (requestHighRVTime > 0)
				{
					duringTime = GetTimeStamp() - requestHighRVTime;
				}
				ReportAdmobAd(adapterName, 5, 104, true, failure, false, duringTime, 0L);
				isRewardedVideoHighAdLoading = false;
				if (RVHighRetryCount < 3)
				{
					RequestHighRewardedVideoAd(RVHighAdUnitId, true);
				}
				else
				{
					StartAdTimer();
				}
			}
			else
			{
				MonoBehaviour.print("HandleRewardBasedVideoFailedToLoad event received with message: " + args.Message + "     curRV:" + text);
				ReportAdmobAd(adapterName, 5, 104, true, failure, false, duringTime, 0L);
				isRewardedVideoHighAdLoading = false;
				if (RVHighRetryCount < 3)
				{
					RequestHighRewardedVideoAd(RVHighAdUnitId, true);
				}
				else
				{
					StartAdTimer();
				}
			}
		}
		else
		{
			MonoBehaviour.print("HandleRewardBasedVideoFailedToLoad event received with message: " + args.Message + "sender is null");
			ReportAdmobAd(adapterName, 5, 104, true, failure, false, duringTime, 0L);
			isRewardedVideoHighAdLoading = false;
			if (RVHighRetryCount < 3)
			{
				RequestHighRewardedVideoAd(RVHighAdUnitId, true);
			}
			else
			{
				StartAdTimer();
			}
		}
		if (this.CMPAdmobRewardVideoOnAdFailedToLoad != null)
		{
			this.CMPAdmobRewardVideoOnAdFailedToLoad(this, args);
		}
	}

	public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
	{
		isRewardVideoShowing = true;
		string text = "";
		string text2 = "unknow";
		if (sender != null)
		{
			text = ((RewardedAd)sender).MediationAdapterClassName();
			if (sender == rewardedAdLow)
			{
				text2 = "rewardedAdLow";
				isRewardVideoLowOccupied = true;
			}
			else if (sender == rewardedAdHigh)
			{
				text2 = "rewardedAdHigh";
				isRewardVideoHighOccupied = true;
			}
		}
		MonoBehaviour.print("HandleRewardBasedVideoOpened event received: " + text + "     curRV:" + text2);
		if (this.CMPAdmobRewardVideoOnAdOpening != null)
		{
			this.CMPAdmobRewardVideoOnAdOpening(this, args);
		}
		if (this.CMPAdmobRewardVideoOnAdStarted != null)
		{
			this.CMPAdmobRewardVideoOnAdStarted(this, args);
		}
		hasRewarded = false;
		hasClosed = false;
		ReportAdmobAd(text, 5, 3, true, 0, false, 0L, rewardVideoScene1, rewardVideoCid, rewardVideoCompletion, rewardVideoRetry_show);
	}

	public void HandleRewardBasedVideoFailedToShow(object sender, AdErrorEventArgs args)
	{
		string text = "";
		string text2 = "unknow";
		if (sender != null)
		{
			text = ((RewardedAd)sender).MediationAdapterClassName();
			if (sender == rewardedAdLow)
			{
				text2 = "rewardedAdLow";
			}
			else if (sender == rewardedAdHigh)
			{
				text2 = "rewardedAdHigh";
			}
		}
		MonoBehaviour.print("HandleRewardBasedVideoFailedToShow: " + args.Message + "          mediationAdapterClassName:" + text + "     curRV:" + text2);
		if (this.CMPAdmobRewardVideoOnAdFailedToShow != null)
		{
			if (Application.internetReachability == NetworkReachability.NotReachable)
			{
				args.Message = "1";
			}
			else
			{
				args.Message = "2";
			}
			this.CMPAdmobRewardVideoOnAdFailedToShow(this, args);
		}
	}

	public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
	{
		isRewardVideoShowing = false;
		string text = "";
		string text2 = "unknow";
		if (sender != null && sender == rewardedAdLow)
		{
			text = ((RewardedAd)sender).MediationAdapterClassName();
			text2 = "rewardedAdLow";
		}
		else if (sender != null && sender == rewardedAdHigh)
		{
			text = ((RewardedAd)sender).MediationAdapterClassName();
			text2 = "rewardedAdHigh";
		}
		MonoBehaviour.print("HandleRewardBasedVideoClosed event received: " + text + "     curRV:" + text2);
		hasClosed = true;
		videoCloseEventArgs = args;
		if (hasRewarded)
		{
			hasRewarded = false;
			if (this.CMPAdmobRewardVideoOnAdClosed != null)
			{
				this.CMPAdmobRewardVideoOnAdClosed(this, args);
			}
		}
		else
		{
			Invoke("DelayHandleRewardBasedVideoClosed", 0.5f);
		}
		if (sender != null)
		{
			if (sender == rewardedAdLow)
			{
				isRewardVideoLowOccupied = false;
				ReportAdmobAd(rewardedAdLow.MediationAdapterClassName(), 5, 7, true, 0, false, 0L, rewardVideoScene1, rewardVideoCid, rewardVideoCompletion, rewardVideoRetry_show);
				rewardedAdLow = null;
				RequestLowRewardedVideoAd(RVLowAdUnitId);
			}
			else if (sender == rewardedAdHigh)
			{
				isRewardVideoHighOccupied = false;
				ReportAdmobAd(rewardedAdHigh.MediationAdapterClassName(), 5, 7, true, 0, false, 0L, rewardVideoScene1, rewardVideoCid, rewardVideoCompletion, rewardVideoRetry_show);
				rewardedAdHigh = null;
				RequestHighRewardedVideoAd(RVHighAdUnitId);
			}
			else
			{
				ReportAdmobAd("", 5, 7, true, 0, false, 0L, rewardVideoScene1, rewardVideoCid, rewardVideoCompletion, rewardVideoRetry_show);
				rewardedAdLow = null;
				rewardedAdHigh = null;
				RequestRewardedVideoAd(RVAdUnitId);
			}
		}
	}

	public void HandleRewardBasedVideoRewarded(object sender, Reward args)
	{
		string text = "";
		string text2 = "unknow";
		if (sender != null)
		{
			text = ((RewardedAd)sender).MediationAdapterClassName();
			if (sender == rewardedAdLow)
			{
				text2 = "rewardedAdLow";
			}
			else if (sender == rewardedAdHigh)
			{
				text2 = "rewardedAdHigh";
			}
		}
		string type = args.Type;
		double amount = args.Amount;
		MonoBehaviour.print("HandleRewardBasedVideoRewarded event received for " + amount + " " + type + "adapter name: " + text + "     curRV:" + text2);
		hasRewarded = true;
		videoRewardedEventArgs = args;
		if (!hasClosed && this.CMPAdmobRewardVideoOnAdRewarded != null)
		{
			this.CMPAdmobRewardVideoOnAdRewarded(this, args);
		}
		ReportAdmobAd(text, 5, 4, true, 0, false, 0L, rewardVideoScene1, rewardVideoCid, rewardVideoCompletion, rewardVideoRetry_show);
	}

	public void HandleOnBannerAdLoaded(object sender, EventArgs args)
	{
		if (this.CMPAdmobBannerOnAdLoaded != null)
		{
			this.CMPAdmobBannerOnAdLoaded(this, args);
		}
		isBannerAdLoading = false;
		isBannerAdLoaded = true;
		MonoBehaviour.print("HandleBannerAdLoaded event received");
		long duringTime = 0L;
		if (requestBannerTime > 0)
		{
			duringTime = GetTimeStamp() - requestBannerTime;
		}
		ReportAdmobAd(bannerView.MediationAdapterClassName(), 6, 103, true, 0, false, duringTime, 0L);
		ReportAdmobAd(bannerView.MediationAdapterClassName(), 6, 1, true, 0, false, 0L, bannerScene1, bannerCid, bannerCompletion, bannerRetry_show);
	}

	public void HandleOnBannerAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		if (this.CMPAdmobBannerOnAdFailedToLoad != null)
		{
			this.CMPAdmobBannerOnAdFailedToLoad(this, args);
		}
		isBannerAdLoading = false;
		isBannerAdLoaded = false;
		MonoBehaviour.print("HandleFailedToReceiveBannerAd event received with message: " + args.Message);
		int failure = 4;
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			failure = 2;
		}
		long duringTime = 0L;
		if (requestBannerTime > 0)
		{
			duringTime = GetTimeStamp() - requestBannerTime;
		}
		ReportAdmobAd(bannerView.MediationAdapterClassName(), 6, 104, true, failure, false, duringTime, 0L);
	}

	public void HandleOnBannerAdOpened(object sender, EventArgs args)
	{
		if (this.CMPAdmobBannerOnAdOpening != null)
		{
			this.CMPAdmobBannerOnAdOpening(this, args);
		}
		MonoBehaviour.print("HandleBannerAdOpened event received");
		ReportAdmobAd(bannerView.MediationAdapterClassName(), 6, 1, true, 0, false, 0L, bannerScene1, bannerCid, bannerCompletion, bannerRetry_show);
	}

	public void HandleOnBannerAdClosed(object sender, EventArgs args)
	{
		if (this.CMPAdmobBannerOnAdClosed != null)
		{
			this.CMPAdmobBannerOnAdClosed(this, args);
		}
		MonoBehaviour.print("HandleBannerAdClosed event received");
		ReportAdmobAd(bannerView.MediationAdapterClassName(), 6, 7, true, 0, false, 0L, bannerScene1, bannerCid, bannerCompletion, bannerRetry_show);
	}

	public void HandleOnBannerAdLeavingApplication(object sender, EventArgs args)
	{
		if (this.CMPAdmobBannerOnAdLeavingApplication != null)
		{
			this.CMPAdmobBannerOnAdLeavingApplication(this, args);
		}
		MonoBehaviour.print("HandleBannerAdLeavingApplication event received");
		ReportAdmobAd(bannerView.MediationAdapterClassName(), 6, 2, true, 0, false, 0L, bannerScene1, bannerCid, bannerCompletion, bannerRetry_show);
	}

	public void HandleMediationTestSuiteDismissed(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleMediationTestSuiteDismissed event received");
	}

	public void ReportAdShowChance(int scene, long scene1, int cid, int completion, int retry_show, long durationTime)
	{
		ReportAdmobAd("", scene, 102, true, 0, false, durationTime, scene1, cid, completion, retry_show);
	}

	public void ReportRewardVideoBtnAvailable(bool available)
	{
		int action = 6;
		if (available)
		{
			action = 11;
		}
		ReportAdmobAd("", 5, action, true, 0, false, 0L, 0L);
	}

	public void ReportAdmobAd(string adapterName, int scene, int action, bool commercial, int failure, bool remove_ads, long duringTime, long scene1 = 0L, int cid = 0, int completion = 0, int retry_show = 0)
	{
		int ad_source = 0;
		ReportAdmobAdData(adapterName, scene, action, ad_source, commercial, failure, remove_ads, scene1, cid, completion, retry_show, duringTime);
	}

	public void ReportAdmobAdData(string adapterName, int scene, int action, int ad_source, bool commercial, int failure, bool remove_ads, long scene1 = 0L, int cid = 0, int completion = 0, int retry_show = 0, long duringTime = 0L)
	{
		long num = (int)GetTimeStamp();
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["uptime2"] = num;
		dictionary["player_time"] = num.ToString();
		dictionary["scene"] = scene;
		dictionary["action"] = action;
		dictionary["ad_source"] = ad_source;
		dictionary["commercial"] = (commercial ? 1 : 0);
		dictionary["failure"] = failure;
		dictionary["remove_ads"] = (remove_ads ? 1 : 0);
		dictionary["reserve1"] = (int)duringTime;
		dictionary["reserve2"] = failure;
		dictionary["reserve3"] = ((adapterName != null) ? adapterName : "");
		dictionary["reserve4"] = "1.3.6";
		dictionary["scene1"] = scene1;
		dictionary["cid"] = cid;
		dictionary["completion"] = completion;
		dictionary["retry_show"] = retry_show;
		dictionary["mediation_type"] = 1;
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
		Debug.Log("CMPAdmobManager onNetWorkAviable networkType:" + networkType);
		if (isRewardVideoShowing && Application.internetReachability == NetworkReachability.NotReachable && this.CMPAdmobRewardVideoOnAdFailedToShow != null)
		{
			AdErrorEventArgs adErrorEventArgs = new AdErrorEventArgs();
			adErrorEventArgs.Message = "1";
			this.CMPAdmobRewardVideoOnAdFailedToShow(this, adErrorEventArgs);
		}
		if (admobInterstitialAd != null && !IsInterstitialReady())
		{
			RequestInterstitialAd(ISAdUnitId);
		}
		if (!IsRewardedVideoReady())
		{
			RequestRewardedVideoAd(RVAdUnitId);
		}
		if (bannerView != null && !isBannerAdLoaded)
		{
			RequestBanner(BannerAdUnitId);
		}
	}

	public bool isOpenAdTest()
	{
		return false;
	}

	public string getTestDevice()
	{
		return "";
	}
}
