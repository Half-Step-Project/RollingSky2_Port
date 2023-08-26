using System;
using GoogleMobileAds.Api;
using UnityEngine;

public class CMPlaySDKTest : MonoBehaviour
{
	public const string GameObject_Name = "CMPlaySDKTest";

	private static string s_admobAppId;

	private static string s_interstitialAdUnitId;

	private static string s_rewardedVideoAdUnitId;

	private static string s_bannerAdUnitId;

	private static string s_nativeAdUnitId;

	private bool isRegistered;

	private static CMPlaySDKTest instance;

	public static CMPlaySDKTest Instance
	{
		get
		{
			return instance;
		}
	}

	public static void Initialize(string admobAppId, string interstitialAdUnitId, string rewardedVideoAdUnitId, string bannerAdUnitId, string nativeAdUnitId)
	{
		s_admobAppId = admobAppId;
		s_interstitialAdUnitId = interstitialAdUnitId;
		s_rewardedVideoAdUnitId = rewardedVideoAdUnitId;
		s_bannerAdUnitId = bannerAdUnitId;
		s_nativeAdUnitId = nativeAdUnitId;
		Debug.Log("cmp_ad:admob_log_Initialize CMPlaySDKTest");
		GameObject gameObject = GameObject.Find("CMPlaySDKTest");
		if (gameObject == null)
		{
			gameObject = new GameObject("CMPlaySDKTest");
		}
		if (gameObject != null)
		{
			gameObject.AddComponent<CMPlaySDKTest>();
		}
	}

	public void Awake()
	{
		if (instance == null)
		{
			instance = this;
			if (!base.gameObject.name.Equals("CMPlaySDKTest"))
			{
				base.gameObject.name = "CMPlaySDKTest";
			}
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	public void setAdTestId(string admobAppId, string interstitialAdUnitId, string rewardedVideoAdUnitId, string bannerAdUnitId)
	{
	}

	public void OpenAdmobTestUI()
	{
		RegisterAdEvents();
		setAdTestId(s_admobAppId, s_interstitialAdUnitId, s_rewardedVideoAdUnitId, s_nativeAdUnitId);
	}

	public void RegisterAdEvents()
	{
		Debug.Log("cmp_ad:admob_log_RegisterAdEvents 1");
		if (!isRegistered)
		{
			isRegistered = true;
			CMPAdsManager.CMPAdsInterstitialOnAdLoaded += HandleOnAdLoaded;
			CMPAdsManager.CMPAdsInterstitialOnAdFailedToLoad += HandleOnAdFailedToLoad;
			CMPAdsManager.CMPAdsInterstitialOnAdOpening += HandleOnAdOpened;
			CMPAdsManager.CMPAdsInterstitialOnAdLeavingApplication += HandleOnAdLeavingApplication;
			CMPAdsManager.CMPAdsInterstitialOnAdClosed += HandleOnAdClosed;
			CMPAdsManager.CMPAdsRewardVideoOnAdLoaded += HandleRewardBasedVideoLoaded;
			CMPAdsManager.CMPAdsRewardVideoOnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
			CMPAdsManager.CMPAdsRewardVideoOnAdShowException += HandleRewardBasedVideoFailedToShow;
			CMPAdsManager.CMPAdsRewardVideoOnAdStarted += HandleRewardBasedVideoStarted;
			CMPAdsManager.CMPAdsRewardVideoOnAdOpening += HandleRewardBasedVideoOpened;
			CMPAdsManager.CMPAdsRewardVideoOnAdClosed += HandleRewardBasedVideoClosed;
			CMPAdsManager.CMPAdsRewardVideoOnAdRewarded += HandleRewardBasedVideoRewarded;
			CMPAdsManager.CMPAdsRewardVideoOnAdLeavingApplication += HandleRewardBasedVideoLeftApplication;
		}
	}

	public void OnGUI()
	{
		if (GUILayout.Button("open admob test UI", GUILayout.Width(200f), GUILayout.Height(100f)))
		{
			Debug.Log("cmp_ad:admob_log_CMPlaySDKTest");
			OpenAdmobTestUI();
		}
	}

	public void ShowMediationTestSuite(string appid)
	{
		Debug.Log("cmp_ad:admob_log_RequestBannerAd:" + appid);
		CMPAdmobManager.Instance.ShowMediationTestSuite(appid);
	}

	public void InitAdmobSDK(string appid)
	{
		Debug.Log("cmp_ad:admob_log_InitAdmobSDK:" + appid);
		CMPAdsManager.Initialize(appid, appid, true, "", "", "", "");
		RegisterAdEvents();
	}

	public void RequestInterstitialAd(string adUnitId)
	{
		Debug.Log("cmp_ad:admob_log_RequestInterstitialAd:" + adUnitId);
		CMPAdsManager.RequestInterstitialAd(adUnitId, adUnitId);
		CMPAdsManager.IsInterstitialAdLoading();
	}

	public void RequestRewardVideoAd(string adUnitId)
	{
		Debug.Log("cmp_ad:admob_log_RequestRewardVideoAd:" + adUnitId);
		CMPAdsManager.RequestRewardedVideoAd(adUnitId, adUnitId);
		if (!CMPAdsManager.IsRewardedVideoReady())
		{
			CMPAdsManager.IsRewardedVideoAdLoading();
		}
	}

	public void ShowInterstitialAd()
	{
		Debug.Log("cmp_ad:admob_log_ShowInterstitialAd:");
		CMPAdsManager.IsInterstitialReady();
		CMPAdsManager.ShowInterstitialAd(1L, 2, 1, 2);
	}

	public void ShowRewardVideoAd()
	{
		Debug.Log("cmp_ad:admob_log_ShowRewardVideoAd:");
		CMPAdsManager.IsRewardedVideoReady();
		CMPAdsManager.ShowRewardedVideoAd(2L, 3, 1, 2);
	}

	public void checkInterstitialAdIsReady(string param)
	{
		checkInterstitialAdIsReady();
	}

	public void checkInterstitialAdIsReady()
	{
		Debug.Log("cmp_ad:admob_log_checkInterstitialAdIsReady:");
		CMPAdsManager.IsInterstitialReady();
	}

	public void checkRewardedVideoAdIsReady(string param)
	{
		checkRewardedVideoAdIsReady();
	}

	public void checkRewardedVideoAdIsReady()
	{
		Debug.Log("cmp_ad:admob_log_checkRewardedVideoAdIsReady:");
		CMPAdsManager.IsRewardedVideoReady();
	}

	public void HandleOnAdLoaded(object sender, EventArgs args)
	{
		if (typeof(CMPBUAdsEventArgs).IsInstanceOfType(args))
		{
			CMPBUAdsEventArgs cMPBUAdsEventArgs = (CMPBUAdsEventArgs)args;
			Debug.Log("admob_log_CMPlaySDKTest HandleOnAdLoaded: " + cMPBUAdsEventArgs.adUnitId);
		}
		else
		{
			Debug.Log("admob_log_CMPlaySDKTest HandleOnAdLoaded");
		}
	}

	public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		Debug.Log("admob_log_CMPlaySDKTest HandleOnAdFailedToLoad: " + args.Message);
		Debug.Log("admob_log_CMPlaySDKTest HandleOnAdFailedToLoad");
	}

	public void HandleOnAdOpened(object sender, EventArgs args)
	{
		if (typeof(CMPBUAdsEventArgs).IsInstanceOfType(args))
		{
			CMPBUAdsEventArgs cMPBUAdsEventArgs = (CMPBUAdsEventArgs)args;
			Debug.Log("admob_log_CMPlaySDKTest HandleOnAdOpened: " + cMPBUAdsEventArgs.adUnitId);
		}
		else
		{
			Debug.Log("admob_log_CMPlaySDKTest HandleOnAdOpened");
		}
	}

	public void HandleOnAdClosed(object sender, EventArgs args)
	{
		if (typeof(CMPBUAdsEventArgs).IsInstanceOfType(args))
		{
			CMPBUAdsEventArgs cMPBUAdsEventArgs = (CMPBUAdsEventArgs)args;
			Debug.Log("admob_log_CMPlaySDKTest HandleOnAdClosed: " + cMPBUAdsEventArgs.adUnitId);
		}
		else
		{
			Debug.Log("admob_log_CMPlaySDKTest HandleOnAdClosed");
		}
	}

	public void HandleOnAdLeavingApplication(object sender, EventArgs args)
	{
		if (typeof(CMPBUAdsEventArgs).IsInstanceOfType(args))
		{
			CMPBUAdsEventArgs cMPBUAdsEventArgs = (CMPBUAdsEventArgs)args;
			Debug.Log("admob_log_CMPlaySDKTest HandleOnAdLeavingApplication: " + cMPBUAdsEventArgs.adUnitId);
		}
		else
		{
			Debug.Log("admob_log_CMPlaySDKTest HandleOnAdLeavingApplication");
		}
	}

	public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
	{
		if (typeof(CMPBUAdsEventArgs).IsInstanceOfType(args))
		{
			CMPBUAdsEventArgs cMPBUAdsEventArgs = (CMPBUAdsEventArgs)args;
			Debug.Log("admob_log_CMPlaySDKTest HandleRewardBasedVideoLoaded: " + cMPBUAdsEventArgs.adUnitId);
		}
		else
		{
			Debug.Log("admob_log_CMPlaySDKTest HandleRewardBasedVideoLoaded");
		}
	}

	public void HandleRewardBasedVideoFailedToLoad(object sender, AdErrorEventArgs args)
	{
		Debug.Log(string.Concat(str3: CMPAdmobManager.Instance.IsRewardedVideoReady().ToString(), str0: "admob_log_CMPlaySDKTest HandleRewardBasedVideoFailedToLoad: ", str1: args.Message, str2: "   now have ad to show:"));
	}

	public void HandleRewardBasedVideoFailedToShow(object sender, AdErrorEventArgs args)
	{
		Debug.Log("admob_log_CMPlaySDKTest HandleRewardBasedVideoFailedToShow: " + args.Message);
	}

	public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
	{
		if (typeof(CMPBUAdsEventArgs).IsInstanceOfType(args))
		{
			CMPBUAdsEventArgs cMPBUAdsEventArgs = (CMPBUAdsEventArgs)args;
			Debug.Log("admob_log_CMPlaySDKTest HandleRewardBasedVideoOpened: " + cMPBUAdsEventArgs.adUnitId);
		}
		else
		{
			Debug.Log("admob_log_CMPlaySDKTest HandleRewardBasedVideoOpened");
		}
	}

	public void HandleRewardBasedVideoStarted(object sender, EventArgs args)
	{
		if (typeof(CMPBUAdsEventArgs).IsInstanceOfType(args))
		{
			CMPBUAdsEventArgs cMPBUAdsEventArgs = (CMPBUAdsEventArgs)args;
			Debug.Log("admob_log_CMPlaySDKTest HandleRewardBasedVideoStarted: " + cMPBUAdsEventArgs.adUnitId);
		}
		else
		{
			Debug.Log("admob_log_CMPlaySDKTest HandleRewardBasedVideoStarted");
		}
	}

	public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
	{
		if (typeof(CMPBUAdsEventArgs).IsInstanceOfType(args))
		{
			CMPBUAdsEventArgs cMPBUAdsEventArgs = (CMPBUAdsEventArgs)args;
			Debug.Log("admob_log_CMPlaySDKTest HandleRewardBasedVideoClosed: " + cMPBUAdsEventArgs.adUnitId);
		}
		else
		{
			Debug.Log("admob_log_CMPlaySDKTest HandleRewardBasedVideoClosed");
		}
	}

	public void HandleRewardBasedVideoRewarded(object sender, Reward args)
	{
		Debug.Log("admob_log_CMPlaySDKTest HandleRewardBasedVideoRewarded: " + args.Type);
	}

	public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
	{
		if (typeof(CMPBUAdsEventArgs).IsInstanceOfType(args))
		{
			CMPBUAdsEventArgs cMPBUAdsEventArgs = (CMPBUAdsEventArgs)args;
			Debug.Log("admob_log_CMPlaySDKTest HandleRewardBasedVideoLeftApplication: " + cMPBUAdsEventArgs.adUnitId);
		}
		else
		{
			Debug.Log("admob_log_CMPlaySDKTest HandleRewardBasedVideoLeftApplication");
		}
	}
}
