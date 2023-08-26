namespace Foundation
{
	public sealed class PlaySoundParams
	{
		internal const float DefaultTime = 0f;

		internal const bool DefaultMute = false;

		internal const bool DefaultLoop = false;

		internal const int DefaultPriority = 0;

		internal const float DefaultVolume = 1f;

		internal const float DefaultFadeInSeconds = 0f;

		internal const float DefaultFadeOutSeconds = 0f;

		internal const float DefaultPitch = 1f;

		internal const float DefaultPanStereo = 0f;

		internal const float DefaultSpatialBlend = 0f;

		internal const float DefaultMaxDistance = 100f;

		public float Time { get; set; }

		public bool MuteInGroup { get; set; }

		public bool Loop { get; set; }

		public int Priority { get; set; }

		public float VolumeInGroup { get; set; }

		public float FadeInSeconds { get; set; }

		public float Pitch { get; set; }

		public float PanStereo { get; set; }

		public float SpatialBlend { get; set; }

		public float MaxDistance { get; set; }

		public PlaySoundParams()
		{
			Time = 0f;
			MuteInGroup = false;
			Loop = false;
			Priority = 0;
			VolumeInGroup = 1f;
			FadeInSeconds = 0f;
			Pitch = 1f;
			PanStereo = 0f;
			SpatialBlend = 0f;
			MaxDistance = 100f;
		}
	}
}
