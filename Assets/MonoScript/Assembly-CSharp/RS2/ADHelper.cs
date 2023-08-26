using My.Core;

namespace RS2
{
	public sealed class ADHelper : My.Core.Singleton<ADHelper>
	{
		private IAD ad;

		public void Initialize()
		{
			ad = null;
		}

		public void RequestInterstitial()
		{
			if (ad != null)
			{
				ad.RequestInterstitial();
			}
		}

		public void RequestRewardeVideo()
		{
			if (ad != null)
			{
				ad.RequestRewardeVideo();
			}
		}

		public bool InterstitialCanShow(ADScene scene)
		{
			if (ad == null)
			{
				return false;
			}
			return ad.InterstitialCanShow(scene);
		}

		public bool RewardeVideoCanShow(ADScene scene)
		{
			if (ad == null)
			{
				return false;
			}
			return ad.RewardeVideoCanShow(scene);
		}

		public bool RecommandVideoCanShow(ADScene scene)
		{
			if (ad == null)
			{
				return false;
			}
			return ad.RecommendCanShow(scene);
		}

		public bool RecommandVideoCanShow(ADScene scene, bool forceNetwork)
		{
			if (ad == null)
			{
				return false;
			}
			return ad.RecommendCanShow(scene, forceNetwork);
		}

		public void ShowInterstitial(ADScene scene, ADCallbackHandler handler)
		{
			if (ad != null)
			{
				ad.ShowInterstitial(scene, handler);
			}
		}

		public void ShowRewardedVideo(ADScene scene, ADCallbackHandler handler)
		{
			if (ad != null)
			{
				ad.ShowRewardedVideo(scene, handler);
			}
		}

		public void ShowRecommandVideo(ADScene scene, ADCallbackHandler handler)
		{
			if (ad != null)
			{
				ad.ShowRecommend(scene, handler);
			}
		}

		public void ApplicationPause(bool isPaused)
		{
			if (ad != null)
			{
				ad.ApplicationPause(isPaused);
			}
		}
	}
}
