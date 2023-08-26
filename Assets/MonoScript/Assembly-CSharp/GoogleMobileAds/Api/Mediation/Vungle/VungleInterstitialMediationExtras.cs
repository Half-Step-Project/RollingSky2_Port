namespace GoogleMobileAds.Api.Mediation.Vungle
{
	public class VungleInterstitialMediationExtras : VungleMediationExtras
	{
		public override string AndroidMediationExtraBuilderClassName
		{
			get
			{
				return "com.google.unity.mediation.vungle.VungleUnityInterstitialExtrasBuilder";
			}
		}
	}
}
