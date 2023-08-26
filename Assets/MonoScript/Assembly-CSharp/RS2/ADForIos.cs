using System;
using System.Collections.Generic;
using Foundation;
using GoogleMobileAds.Api;
using UnityEngine;

namespace RS2
{
	public sealed class ADForIos : IAD
	{
		private const string AdmobID = "ca-app-pub-6783879517274907~3746032805";

		private const string InterstitialUnitID = "ca-app-pub-6783879517274907/9308462623";

		private const string RewardeVideoUnitID = "ca-app-pub-6783879517274907/6727419367";

		private ADScene m_CurInterstitialScene;

		private ADScene m_CurRewardeVideoScene;

		private Dictionary<string, string> m_ADEvent = new Dictionary<string, string>();

		private event ADCallbackHandler m_InterstitialCallback;

		private event ADCallbackHandler m_RewardeVideoCallback;

		public void Initialize()
		{
			bool flag = CMPGDPRUtils.Instance.checkIfGDPRAgreedAdStayInformed();
			CMPUnityThreadDispatcher.Initialize();
			CMPAdmobManager.Initialize("ca-app-pub-6783879517274907~3746032805", flag, 0);
			CMPAdmobManager.Instance.RequestInterstitialAd("ca-app-pub-6783879517274907/9308462623");
			CMPAdmobManager.Instance.RequestRewardedVideoAd("ca-app-pub-6783879517274907/6727419367");
			CMPlaySDKUtils.Instance.OnVideoClosedCallback += HandleRecommendVideoClosed;
			CMPlaySDKUtils.Instance.OnVideoShowCallback += HandleRecommendVideoShow;
			CMPlaySDKUtils.Instance.OnVideoShowFailCallback += HandleRecommendVideoFail;
			CMPAdmobManager.Instance.CMPAdmobInterstitialOnAdOpening += HandleOnAdOpened;
			CMPAdmobManager.Instance.CMPAdmobInterstitialOnAdClosed += HandleOnAdClosed;
			CMPAdmobManager.Instance.CMPAdmobRewardVideoOnAdStarted += HandleRewardBasedVideoStarted;
			CMPAdmobManager.Instance.CMPAdmobRewardVideoOnAdOpening += HandleRewardBasedVideoOpened;
			CMPAdmobManager.Instance.CMPAdmobRewardVideoOnAdClosed += HandleRewardBasedVideoClosed;
			CMPAdmobManager.Instance.CMPAdmobRewardVideoOnAdRewarded += HandleRewardBasedVideoRewarded;
			CMPAdmobManager.Instance.CMPAdmobRewardVideoOnAdFailedToShow += HandleRewardBasedVideoShowException;
			Log.Info(string.Format("AD for IOS Initialized  gdpr: {0}", flag));
		}

		public void RequestInterstitial()
		{
			Log.Info("AD for IOS RequestInterstitial");
			CMPAdmobManager.Instance.RequestInterstitialAd("ca-app-pub-6783879517274907/9308462623");
		}

		public void RequestRewardeVideo()
		{
			Log.Info("AD for IOS RequestRewardeVideo");
			CMPAdmobManager.Instance.RequestRewardedVideoAd("ca-app-pub-6783879517274907/6727419367");
		}

		public bool InterstitialCanShow(ADScene scene, bool forceNetwork = true)
		{
			bool flag = CMPAdmobManager.Instance.IsInterstitialReady();
			if (forceNetwork && Application.internetReachability == NetworkReachability.NotReachable)
			{
				flag = false;
			}
			Log.Info(string.Format("AD for IOS InterstitialCanShow | scene {0} | can show {1}", scene, flag));
			return flag;
		}

		public bool RewardeVideoCanShow(ADScene scene, bool forceNetwork = true)
		{
			bool flag = CMPAdmobManager.Instance.IsRewardedVideoReady();
			if (forceNetwork && Application.internetReachability == NetworkReachability.NotReachable)
			{
				flag = false;
			}
			Log.Info(string.Format("AD for IOS RewardeVideoCanShow | scene {0} | can show {1}", scene, flag));
			return flag;
		}

		public bool RecommendCanShow(ADScene scene, bool forceNetwork = true)
		{
			bool flag = CMPlaySDKUtils.Instance.CanShowRewardedVideo((int)scene, true);
			if (forceNetwork && Application.internetReachability == NetworkReachability.NotReachable)
			{
				flag = false;
			}
			Log.Info(string.Format("AD for IOS RecommendCanShow | scene {0} | can show {1}", scene, flag));
			return flag;
		}

		public void ShowInterstitial(ADScene scene, ADCallbackHandler handler)
		{
			if (CMPAdmobManager.Instance.IsInterstitialReady())
			{
				m_CurInterstitialScene = scene;
				this.m_InterstitialCallback = handler;
				Log.Info(string.Format("AD for IOS ShowInterstitial | scene {0}", scene));
				CMPAdmobManager.Instance.ShowInterstitialAd(0L);
			}
			else
			{
				handler(new ADCallbackEventArgs(scene, ADStatus.InterstitialClosed));
			}
		}

		public void ShowRewardedVideo(ADScene scene, ADCallbackHandler handler)
		{
			if (CMPAdmobManager.Instance.IsRewardedVideoReady())
			{
				m_CurRewardeVideoScene = scene;
				this.m_RewardeVideoCallback = handler;
				Log.Info(string.Format("AD for IOS ShowRewardedVideo | scene {0}", scene));
				CMPAdmobManager.Instance.ShowRewardedVideoAd(0L);
			}
			else
			{
				handler(new ADCallbackEventArgs(scene, ADStatus.RewardeVideoFail));
			}
		}

		public void ShowRecommend(ADScene scene, ADCallbackHandler handler)
		{
			if (CMPlaySDKUtils.Instance.CanShowRewardedVideo((int)scene, true))
			{
				m_CurRewardeVideoScene = scene;
				this.m_RewardeVideoCallback = handler;
				Log.Info(string.Format("AD for IOS ShowRecommend | scene {0} ", scene));
				CMPlaySDKUtils.Instance.ShowRewardedVideo((int)scene);
			}
			else
			{
				handler(new ADCallbackEventArgs(scene, ADStatus.RecommandVideoFail));
			}
		}

		public void ApplicationPause(bool isPaused)
		{
		}

		public void HandleOnAdOpened(object sender, System.EventArgs args)
		{
			Log.Info("AD Interstitial HandleOnAdOpened");
			this.m_InterstitialCallback(new ADCallbackEventArgs(m_CurInterstitialScene, ADStatus.InterstitialOpened));
			m_ADEvent.Clear();
			m_ADEvent.Add("af_content_type", "play_interstitial");
			m_ADEvent.Add("af_content_id", "1");
			AppsFlyer.trackRichEvent("af_interstitial", m_ADEvent);
		}

		public void HandleOnAdClosed(object sender, System.EventArgs args)
		{
			Log.Info("AD Interstitial HandleOnAdClosed");
			this.m_InterstitialCallback(new ADCallbackEventArgs(m_CurInterstitialScene, ADStatus.InterstitialClosed));
		}

		public void HandleRewardBasedVideoOpened(object sender, System.EventArgs args)
		{
			Log.Info("AD Video HandleRewardBasedVideoOpened");
		}

		public void HandleRewardBasedVideoStarted(object sender, System.EventArgs args)
		{
			Log.Info("AD Video HandleRewardBasedVideoStarted");
			this.m_RewardeVideoCallback(new ADCallbackEventArgs(m_CurRewardeVideoScene, ADStatus.RewardeVideoStarted));
		}

		public void HandleRewardBasedVideoClosed(object sender, System.EventArgs args)
		{
			Log.Info("AD Video HandleRewardBasedVideoClosed");
			this.m_RewardeVideoCallback(new ADCallbackEventArgs(m_CurRewardeVideoScene, ADStatus.RewardeVideoClosed));
		}

		public void HandleRewardBasedVideoRewarded(object sender, Reward args)
		{
			Log.Info("AD Video HandleRewardBasedVideoRewarded");
			this.m_RewardeVideoCallback(new ADCallbackEventArgs(m_CurRewardeVideoScene, ADStatus.RewardeVideoSuccess));
			m_ADEvent.Clear();
			m_ADEvent.Add("af_content_type", "play_video");
			m_ADEvent.Add("af_content_id", "1");
			AppsFlyer.trackRichEvent("af_video", m_ADEvent);
		}

		public void HandleRewardBasedVideoShowException(object sender, AdErrorEventArgs args)
		{
			string message = args.Message;
			if (message == "1")
			{
				string infoById = Mod.Localization.GetInfoById(226);
				string infoById2 = Mod.Localization.GetInfoById(225);
				string infoById3 = Mod.Localization.GetInfoById(41);
				CMPAdsManager.showAlertView(infoById, infoById2, infoById3, null, null);
			}
		}

		private void HandleRecommendVideoClosed(object sender, System.EventArgs e)
		{
			CMPlaySDKUtils.VideoEventArgs videoEventArg = (CMPlaySDKUtils.VideoEventArgs)e;
			Log.Info("AD for IOS Recommend callback HandleRecommendVideoClosed");
			this.m_RewardeVideoCallback(new ADCallbackEventArgs(m_CurRewardeVideoScene, ADStatus.RecommandVideoSuccess));
		}

		private void HandleRecommendVideoShow(object sender, System.EventArgs e)
		{
			CMPlaySDKUtils.VideoEventArgs videoEventArg = (CMPlaySDKUtils.VideoEventArgs)e;
			Log.Info("AD for IOS Recommend callback HandleRecommendVideoShow");
			this.m_RewardeVideoCallback(new ADCallbackEventArgs(m_CurRewardeVideoScene, ADStatus.RewardeVideoStarted));
		}

		private void HandleRecommendVideoFail(object sender, System.EventArgs e)
		{
			CMPlaySDKUtils.VideoEventArgs videoEventArg = (CMPlaySDKUtils.VideoEventArgs)e;
			Log.Info("AD for IOS Recommend callback HandleRecommendVideoShow");
		}
	}
}
