namespace RS2
{
	public class ADForAndroid : IAD
	{
		public void Initialize()
		{
		}

		public bool InterstitialCanShow(ADScene scene, bool forceNetwork)
		{
			return false;
		}

		public void RequestInterstitial()
		{
		}

		public void RequestRewardeVideo()
		{
		}

		public bool RecommendCanShow(ADScene scene, bool forceNetwork)
		{
			return false;
		}

		public bool RewardeVideoCanShow(ADScene scene, bool forceNetwork)
		{
			return false;
		}

		public void ShowInterstitial(ADScene scene, ADCallbackHandler handler)
		{
		}

		public void ShowRecommend(ADScene scene, ADCallbackHandler handler)
		{
		}

		public void ShowRewardedVideo(ADScene scene, ADCallbackHandler handler)
		{
		}

		public void ApplicationPause(bool isPaused)
		{
		}
	}
}
