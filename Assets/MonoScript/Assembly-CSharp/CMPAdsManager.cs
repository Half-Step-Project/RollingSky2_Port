using System;
using GoogleMobileAds.Api;
using UnityEngine;

public class CMPAdsManager
{
	public const string cmpadsSdkVersion = "1.3.3";

	private static bool s_isInitBUAds;

	private static bool s_isInitAdmob;

	private static bool s_hasRewarded;

	private static bool s_isRequestAdmobBanner;

	private static bool s_isRequestBUBanner;

	private const string CMP_AdmobId = "1001";

	private const string CMP_BUAdsId = "1002";

	private static string s_admobAppid;

	private static string s_BUAdsAppid;

	private static string s_admobISAdUnitId;

	private static string s_admobRVAdUnitId;

	private static string s_BUAdsISAdUnitId;

	private static string s_BUAdsRVAdUnitId;

	private static int s_timerInterval;

	private static bool s_isAgreedGDPRPolicy;

	private static AndroidJavaObject cmp_cloud_hepler_unity;

	private static long s_ISShowDurationTime;

	private static long s_RVShowDurationTime;

	private static CMPAdsObject<CMPAdsConfig> s_adsConfig;

	public static event EventHandler<EventArgs> CMPAdsInterstitialOnAdLoaded;

	public static event EventHandler<AdFailedToLoadEventArgs> CMPAdsInterstitialOnAdFailedToLoad;

	public static event EventHandler<EventArgs> CMPAdsInterstitialOnAdOpening;

	public static event EventHandler<EventArgs> CMPAdsInterstitialOnAdClosed;

	public static event EventHandler<EventArgs> CMPAdsInterstitialOnAdLeavingApplication;

	public static event EventHandler<EventArgs> CMPAdsRewardVideoOnAdLoaded;

	public static event EventHandler<AdErrorEventArgs> CMPAdsRewardVideoOnAdFailedToLoad;

	public static event EventHandler<EventArgs> CMPAdsRewardVideoOnAdOpening;

	public static event EventHandler<EventArgs> CMPAdsRewardVideoOnAdStarted;

	public static event EventHandler<EventArgs> CMPAdsRewardVideoOnAdClosed;

	public static event EventHandler<Reward> CMPAdsRewardVideoOnAdRewarded;

	public static event EventHandler<EventArgs> CMPAdsRewardVideoOnAdLeavingApplication;

	public static event EventHandler<AdErrorEventArgs> CMPAdsRewardVideoOnAdShowException;

	public static void Initialize(string admobAppid, string BUAdsAppid, bool isAgreedGDPRPolicy, string admobRewardAdUnitId, string admobInterstitialAdUnitId, string BURewardAdUnitId, string BUInterstitialAdUnitId, int timerInterval = 0)
	{
		s_admobAppid = admobAppid;
		s_admobISAdUnitId = admobInterstitialAdUnitId;
		s_admobRVAdUnitId = admobRewardAdUnitId;
		s_BUAdsAppid = BUAdsAppid;
		s_BUAdsISAdUnitId = BUInterstitialAdUnitId;
		s_BUAdsRVAdUnitId = BURewardAdUnitId;
		s_isAgreedGDPRPolicy = isAgreedGDPRPolicy;
		s_timerInterval = timerInterval;
		string cloudJsonData = getCloudJsonData(2, "section_ad_new");
		Debug.Log("CMPAdsManager adsJsonData:" + cloudJsonData);
		if (cloudJsonData != null && cloudJsonData != "")
		{
			CMPAdsObject<CMPAdsConfig> cMPAdsObject = JsonUtility.FromJson<CMPAdsObject<CMPAdsConfig>>(cloudJsonData);
			if (cMPAdsObject != null && cMPAdsObject.ad_platform != null)
			{
				s_adsConfig = cMPAdsObject;
				foreach (CMPAdsConfig item in cMPAdsObject.ad_platform)
				{
					Debug.Log("CMPAdsManager cloud ad id: " + item.id);
					if (item != null && item.id != null && item.id.Equals("1001") && !s_isInitAdmob)
					{
						s_isInitAdmob = true;
					}
					if (item != null && item.id != null && item.id.Equals("1002") && !s_isInitBUAds)
					{
						s_isInitBUAds = true;
					}
				}
			}
		}
		if (!s_isInitAdmob && !s_isInitBUAds)
		{
			Debug.Log("CMPAdsManager init ad by local logic");
			s_isInitAdmob = true;
			CMPAdmobManager.Initialize(admobAppid, isAgreedGDPRPolicy, timerInterval);
			AddRegisterForAdmob();
			CMPAdmobManager.Instance.RequestInterstitialAd(admobInterstitialAdUnitId);
			CMPAdmobManager.Instance.RequestRewardedVideoAd(admobRewardAdUnitId);
			return;
		}
		if (s_isInitAdmob)
		{
			CMPAdmobManager.Initialize(admobAppid, isAgreedGDPRPolicy, timerInterval);
			AddRegisterForAdmob();
			CMPAdmobManager.Instance.RequestInterstitialAd(admobInterstitialAdUnitId);
			CMPAdmobManager.Instance.RequestRewardedVideoAd(admobRewardAdUnitId);
		}
		if (s_isInitBUAds)
		{
			CMPBUAdsManager.Initialize(BUAdsAppid, isAgreedGDPRPolicy, timerInterval);
			AddRegisterForBUAds();
			CMPBUAdsManager.Instance.RequestInterstitialAd(BUInterstitialAdUnitId);
			CMPBUAdsManager.Instance.RequestRewardedVideoAd(BURewardAdUnitId);
		}
	}

	public static void onCloudDataRefreshNotification()
	{
		string cloudJsonData = getCloudJsonData(2, "section_ad_new");
		Debug.Log("CMPAdsManager onCloudDataRefreshNotification json data:" + cloudJsonData);
		if (cloudJsonData == null || !(cloudJsonData != ""))
		{
			return;
		}
		bool flag = false;
		bool flag2 = false;
		CMPAdsObject<CMPAdsConfig> cMPAdsObject = JsonUtility.FromJson<CMPAdsObject<CMPAdsConfig>>(cloudJsonData);
		if (cMPAdsObject != null && cMPAdsObject.ad_platform != null)
		{
			s_adsConfig = cMPAdsObject;
			foreach (CMPAdsConfig item in cMPAdsObject.ad_platform)
			{
				Debug.Log("CMPAdsManager onCloudDataRefreshNotification cloud ad id: " + item.id);
				if (item != null && item.id != null && item.id.Equals("1001"))
				{
					flag = true;
				}
				if (item != null && item.id != null && item.id.Equals("1002"))
				{
					flag2 = true;
				}
			}
		}
		if (flag && !s_isInitAdmob)
		{
			s_isInitAdmob = true;
			CMPAdmobManager.Initialize(s_admobAppid, s_isAgreedGDPRPolicy, s_timerInterval);
			AddRegisterForAdmob();
		}
		if (flag2 && !s_isInitBUAds)
		{
			s_isInitBUAds = true;
			CMPBUAdsManager.Initialize(s_BUAdsAppid, s_isAgreedGDPRPolicy);
			AddRegisterForBUAds();
		}
		s_isInitAdmob = flag;
		s_isInitBUAds = flag2;
		retryRequestAds();
	}

	private static void retryRequestAds()
	{
		if (s_admobISAdUnitId != null || s_BUAdsISAdUnitId != null)
		{
			RequestInterstitialAd(s_admobISAdUnitId, s_BUAdsISAdUnitId);
		}
		if (s_admobRVAdUnitId != null || s_BUAdsRVAdUnitId != null)
		{
			RequestRewardedVideoAd(s_admobRVAdUnitId, s_BUAdsRVAdUnitId);
		}
	}

	public static void showAlertView(string title, string message, string buttonText, string listenerName, string eventName)
	{
	}

	public static bool GetGDPRAgreedStaus()
	{
		if (s_isInitAdmob)
		{
			return CMPAdmobManager.GetGDPRAgreedStaus();
		}
		return false;
	}

	private static string getCloudJsonData(int func_type, string section)
	{
		return "";
	}

	public static void ReportRewardAdShowChance(long scene1 = 0L, int cid = 0, int completion = 0, int retry_show = 0)
	{
		if (CMPAdmobManager.Instance != null)
		{
			if (s_RVShowDurationTime > 0)
			{
				s_RVShowDurationTime = CMPAdmobManager.Instance.GetTimeStamp() - s_RVShowDurationTime;
			}
			CMPAdmobManager.Instance.ReportAdShowChance(5, scene1, cid, completion, retry_show, s_RVShowDurationTime);
		}
	}

	public static void ReportInterstitialAdShowChance(long scene1 = 0L, int cid = 0, int completion = 0, int retry_show = 0)
	{
		if (CMPAdmobManager.Instance != null)
		{
			if (s_ISShowDurationTime > 0)
			{
				s_ISShowDurationTime = CMPAdmobManager.Instance.GetTimeStamp() - s_ISShowDurationTime;
			}
			CMPAdmobManager.Instance.ReportAdShowChance(1, scene1, cid, completion, retry_show, s_ISShowDurationTime);
		}
	}

	private static void AddRegisterForAdmob()
	{
		if (CMPAdmobManager.Instance != null)
		{
			CMPAdmobManager.Instance.CMPAdmobInterstitialOnAdOpening += HandleOnInterstitialAdOpened;
			CMPAdmobManager.Instance.CMPAdmobInterstitialOnAdClosed += HandleOnInterstitialAdClosed;
			CMPAdmobManager.Instance.CMPAdmobRewardVideoOnAdStarted += HandleRewardBasedVideoStarted;
			CMPAdmobManager.Instance.CMPAdmobRewardVideoOnAdOpening += HandleRewardBasedVideoOpened;
			CMPAdmobManager.Instance.CMPAdmobRewardVideoOnAdClosed += HandleRewardBasedVideoClosed;
			CMPAdmobManager.Instance.CMPAdmobRewardVideoOnAdRewarded += HandleRewardBasedVideoRewarded;
			CMPAdmobManager.Instance.CMPAdmobRewardVideoOnAdFailedToShow += HandleRewardBasedVideoShowException;
		}
	}

	private static void AddRegisterForBUAds()
	{
		if (CMPBUAdsManager.Instance != null)
		{
			CMPBUAdsManager.Instance.CMPBUAdsInterstitialOnAdLoaded += HandleOnInterstitialAdLoaded;
			CMPBUAdsManager.Instance.CMPBUAdsInterstitialOnAdFailedToLoad += HandleOnInterstitialAdFailedToLoad;
			CMPBUAdsManager.Instance.CMPBUAdsInterstitialOnAdOpening += HandleOnInterstitialAdOpened;
			CMPBUAdsManager.Instance.CMPBUAdsInterstitialOnAdClick += HandleOnInterstitialAdLeavingApplication;
			CMPBUAdsManager.Instance.CMPBUAdsInterstitialOnAdClosed += HandleOnInterstitialAdClosed;
			CMPBUAdsManager.Instance.CMPBUAdsRewardVideoOnAdLoaded += HandleRewardBasedVideoLoaded;
			CMPBUAdsManager.Instance.CMPBUAdsRewardVideoDidFailToLoad += HandleRewardBasedVideoFailedToLoad;
			CMPBUAdsManager.Instance.CMPBUAdsRewardVideoOnAdOpening += HandleRewardBasedVideoStarted;
			CMPBUAdsManager.Instance.CMPBUAdsRewardVideoOnAdOpening += HandleRewardBasedVideoOpened;
			CMPBUAdsManager.Instance.CMPBUAdsRewardVideoOnAdClosed += HandleRewardBasedVideoClosed;
			CMPBUAdsManager.Instance.CMPBUAdsRewardVideoOnAdRewarded += HandleRewardBasedVideoRewarded;
			CMPBUAdsManager.Instance.CMPBUAdsRewardVideoOnAdClick += HandleRewardBasedVideoLeftApplication;
			CMPBUAdsManager.Instance.CMPBUAdsRewardVideoShowFailed += HandleRewardBasedVideoShowException;
		}
	}

	public static void RequestInterstitialAd(string admobAdUnitId, string BUAdUnitId)
	{
		s_admobISAdUnitId = admobAdUnitId;
		s_BUAdsISAdUnitId = BUAdUnitId;
		if (s_isInitAdmob)
		{
			CMPAdmobManager.Instance.RequestInterstitialAd(admobAdUnitId);
		}
		if (s_isInitBUAds)
		{
			CMPBUAdsManager.Instance.RequestInterstitialAd(BUAdUnitId);
		}
	}

	public static bool IsInterstitialReady()
	{
		if (!s_isInitAdmob || !CMPAdmobManager.Instance.IsInterstitialReady())
		{
			if (s_isInitBUAds)
			{
				return CMPBUAdsManager.Instance.IsInterstitialReady();
			}
			return false;
		}
		return true;
	}

	public static bool IsInterstitialAdLoading()
	{
		if (!s_isInitAdmob || !CMPAdmobManager.Instance.IsInterstitialAdLoading())
		{
			if (s_isInitBUAds)
			{
				return CMPBUAdsManager.Instance.IsInterstitialAdLoading();
			}
			return false;
		}
		return true;
	}

	public static bool ShowInterstitialAd(long scene1 = 0L, int cid = 0, int completion = 0, int retry_show = 0)
	{
		if (s_isInitAdmob || s_isInitBUAds)
		{
			if (s_isInitAdmob && s_isInitBUAds)
			{
				if (s_adsConfig != null && s_adsConfig.ad_platform != null && s_adsConfig.ad_platform.Count > 0)
				{
					CMPAdsConfig cMPAdsConfig = s_adsConfig.ad_platform[0];
					if (cMPAdsConfig.id.Equals("1001"))
					{
						if (CMPAdmobManager.Instance.IsInterstitialReady())
						{
							CMPAdmobManager.Instance.ShowInterstitialAd(scene1, cid, completion);
						}
						else if (CMPBUAdsManager.Instance.IsInterstitialReady())
						{
							CMPBUAdsManager.Instance.ShowInterstitialAd(scene1, cid, completion, retry_show);
						}
					}
					else if (cMPAdsConfig.id.Equals("1002"))
					{
						if (CMPBUAdsManager.Instance.IsInterstitialReady())
						{
							CMPBUAdsManager.Instance.ShowInterstitialAd(scene1, cid, completion, retry_show);
						}
						else if (CMPAdmobManager.Instance.IsInterstitialReady())
						{
							CMPAdmobManager.Instance.ShowInterstitialAd(scene1, cid, completion);
						}
					}
				}
				else
				{
					CMPAdmobManager.Instance.ShowInterstitialAd(scene1, cid, completion);
				}
			}
			else if (s_isInitAdmob)
			{
				CMPAdmobManager.Instance.ShowInterstitialAd(scene1, cid, completion);
			}
			else
			{
				CMPBUAdsManager.Instance.ShowInterstitialAd(scene1, cid, completion, retry_show);
			}
			return true;
		}
		return false;
	}

	public static void RequestRewardedVideoAd(string admobAdUnitId, string BUAdUnitId)
	{
		s_admobRVAdUnitId = admobAdUnitId;
		s_BUAdsRVAdUnitId = BUAdUnitId;
		if (s_isInitAdmob)
		{
			CMPAdmobManager.Instance.RequestRewardedVideoAd(admobAdUnitId);
		}
		if (s_isInitBUAds)
		{
			CMPBUAdsManager.Instance.RequestRewardedVideoAd(BUAdUnitId);
		}
	}

	public static bool ShowRewardedVideoAd(long scene1 = 0L, int cid = 0, int completion = 0, int retry_show = 0)
	{
		if (s_isInitAdmob || s_isInitBUAds)
		{
			if (s_isInitAdmob && s_isInitBUAds)
			{
				if (s_adsConfig != null && s_adsConfig.ad_platform != null && s_adsConfig.ad_platform.Count > 0)
				{
					CMPAdsConfig cMPAdsConfig = s_adsConfig.ad_platform[0];
					if (cMPAdsConfig.id.Equals("1001"))
					{
						if (CMPAdmobManager.Instance.IsRewardedVideoReady())
						{
							CMPAdmobManager.Instance.ShowRewardedVideoAd(scene1, cid, completion);
						}
						else if (CMPBUAdsManager.Instance.IsRewardedVideoReady())
						{
							CMPBUAdsManager.Instance.ShowRewardedVideoAd(scene1, cid, completion, retry_show);
						}
					}
					else if (cMPAdsConfig.id.Equals("1002"))
					{
						if (CMPBUAdsManager.Instance.IsRewardedVideoReady())
						{
							CMPBUAdsManager.Instance.ShowRewardedVideoAd(scene1, cid, completion, retry_show);
						}
						else if (CMPAdmobManager.Instance.IsRewardedVideoReady())
						{
							CMPAdmobManager.Instance.ShowRewardedVideoAd(scene1, cid, completion);
						}
					}
				}
				else
				{
					CMPAdmobManager.Instance.ShowRewardedVideoAd(scene1, cid, completion);
				}
			}
			else if (s_isInitAdmob)
			{
				CMPAdmobManager.Instance.ShowRewardedVideoAd(scene1, cid, completion);
			}
			else
			{
				CMPBUAdsManager.Instance.ShowRewardedVideoAd(scene1, cid, completion, retry_show);
			}
			return true;
		}
		return false;
	}

	public static bool IsRewardedVideoReady()
	{
		if (!s_isInitAdmob || !CMPAdmobManager.Instance.IsRewardedVideoReady())
		{
			if (s_isInitBUAds)
			{
				return CMPBUAdsManager.Instance.IsRewardedVideoReady();
			}
			return false;
		}
		return true;
	}

	public static bool IsRewardedVideoAdLoading()
	{
		if (!s_isInitAdmob || !CMPAdmobManager.Instance.IsRewardedVideoAdLoading())
		{
			if (s_isInitBUAds)
			{
				return CMPBUAdsManager.Instance.IsRewardedVideoAdLoading();
			}
			return false;
		}
		return true;
	}

	private static bool isChinaUser()
	{
		return false;
	}

	private static void HandleOnInterstitialAdLoaded(object sender, EventArgs args)
	{
		if (CMPAdsManager.CMPAdsInterstitialOnAdLoaded != null)
		{
			CMPAdsManager.CMPAdsInterstitialOnAdLoaded(sender, args);
		}
	}

	private static void HandleOnInterstitialAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		if (CMPAdsManager.CMPAdsInterstitialOnAdFailedToLoad != null)
		{
			CMPAdsManager.CMPAdsInterstitialOnAdFailedToLoad(sender, args);
		}
	}

	private static void HandleOnInterstitialAdOpened(object sender, EventArgs args)
	{
		if (CMPAdsManager.CMPAdsInterstitialOnAdOpening != null)
		{
			CMPAdsManager.CMPAdsInterstitialOnAdOpening(sender, args);
		}
	}

	private static void HandleOnInterstitialAdClosed(object sender, EventArgs args)
	{
		if (CMPAdsManager.CMPAdsInterstitialOnAdClosed != null)
		{
			CMPAdsManager.CMPAdsInterstitialOnAdClosed(sender, args);
		}
	}

	private static void HandleOnInterstitialAdLeavingApplication(object sender, EventArgs args)
	{
		if (CMPAdsManager.CMPAdsInterstitialOnAdLeavingApplication != null)
		{
			CMPAdsManager.CMPAdsInterstitialOnAdLeavingApplication(sender, args);
		}
	}

	private static void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
	{
		if (CMPAdsManager.CMPAdsRewardVideoOnAdLoaded != null)
		{
			CMPAdsManager.CMPAdsRewardVideoOnAdLoaded(sender, args);
		}
	}

	private static void HandleRewardBasedVideoFailedToLoad(object sender, AdErrorEventArgs args)
	{
		if (CMPAdsManager.CMPAdsRewardVideoOnAdFailedToLoad != null)
		{
			CMPAdsManager.CMPAdsRewardVideoOnAdFailedToLoad(sender, args);
		}
	}

	private static void HandleRewardBasedVideoOpened(object sender, EventArgs args)
	{
		s_hasRewarded = false;
		if (CMPAdsManager.CMPAdsRewardVideoOnAdOpening != null)
		{
			CMPAdsManager.CMPAdsRewardVideoOnAdOpening(sender, args);
		}
	}

	private static void HandleRewardBasedVideoStarted(object sender, EventArgs args)
	{
		s_hasRewarded = false;
		if (CMPAdsManager.CMPAdsRewardVideoOnAdStarted != null)
		{
			CMPAdsManager.CMPAdsRewardVideoOnAdStarted(sender, args);
		}
	}

	private static void HandleRewardBasedVideoShowException(object sender, AdErrorEventArgs args)
	{
		if (CMPAdsManager.CMPAdsRewardVideoOnAdShowException != null)
		{
			CMPAdsManager.CMPAdsRewardVideoOnAdShowException(sender, args);
		}
	}

	private static void HandleRewardBasedVideoClosed(object sender, EventArgs args)
	{
		if (CMPAdsManager.CMPAdsRewardVideoOnAdClosed != null)
		{
			CMPAdsManager.CMPAdsRewardVideoOnAdClosed(sender, args);
		}
	}

	private static void HandleRewardBasedVideoRewarded(object sender, Reward args)
	{
		s_hasRewarded = true;
		if (CMPAdsManager.CMPAdsRewardVideoOnAdRewarded != null)
		{
			CMPAdsManager.CMPAdsRewardVideoOnAdRewarded(sender, args);
		}
	}

	private static void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
	{
		if (CMPAdsManager.CMPAdsRewardVideoOnAdLeavingApplication != null)
		{
			CMPAdsManager.CMPAdsRewardVideoOnAdLeavingApplication(sender, args);
		}
	}
}
