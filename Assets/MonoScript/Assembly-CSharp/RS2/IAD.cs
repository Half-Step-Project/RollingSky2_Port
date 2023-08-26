namespace RS2
{
	public interface IAD
	{
		void Initialize();

		void RequestInterstitial();

		void RequestRewardeVideo();

		bool InterstitialCanShow(ADScene scene, bool forceNetwork = true);

		bool RewardeVideoCanShow(ADScene scene, bool forceNetwork = true);

		bool RecommendCanShow(ADScene scene, bool forceNetwork = true);

		void ShowRecommend(ADScene scene, ADCallbackHandler handler);

		void ShowInterstitial(ADScene scene, ADCallbackHandler handler);

		void ShowRewardedVideo(ADScene scene, ADCallbackHandler handler);

		void ApplicationPause(bool isPaused);
	}
}
