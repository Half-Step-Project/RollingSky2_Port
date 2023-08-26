namespace GoogleMobileAds.Api.Mediation.Vungle
{
	public abstract class VungleMediationExtras : MediationExtras
	{
		public const string AllPlacementsKey = "all_placements";

		public const string UserIdKey = "user_id";

		public const string SoundEnabledKey = "sound_enabled";

		public override string IOSMediationExtraBuilderClassName
		{
			get
			{
				return "VungleExtrasBuilder";
			}
		}

		public VungleMediationExtras()
		{
		}

		public void SetAllPlacements(string[] allPlacements)
		{
			base.Extras.Add("all_placements", string.Join(",", allPlacements));
		}

		public void SetUserId(string userId)
		{
			base.Extras.Add("user_id", userId);
		}

		public void SetSoundEnabled(bool soundEnabled)
		{
			base.Extras.Add("sound_enabled", soundEnabled.ToString());
		}
	}
}
