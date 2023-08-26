namespace GoogleMobileAds.Api.Mediation.Vungle
{
	public class VungleRewardedVideoMediationExtras : VungleMediationExtras
	{
		public override string AndroidMediationExtraBuilderClassName
		{
			get
			{
				return "com.google.unity.mediation.vungle.VungleUnityRewardedVideoExtrasBuilder";
			}
		}
	}
}
