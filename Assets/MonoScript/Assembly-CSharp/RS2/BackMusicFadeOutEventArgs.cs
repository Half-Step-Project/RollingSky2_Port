using Foundation;

namespace RS2
{
	public sealed class BackMusicFadeOutEventArgs : EventArgs<BackMusicFadeOutEventArgs>
	{
		public float DelayTime;

		public float FadeOutTime = 1f;

		public float BeginVolume = 1f;

		public float EndVolume;

		public BackMusicFadeOutEventArgs Initialize(float delayTime, float fadeTime, float beginVol, float endVol)
		{
			DelayTime = delayTime;
			FadeOutTime = fadeTime;
			BeginVolume = beginVol;
			EndVolume = endVol;
			return this;
		}

		protected override void OnRecycle()
		{
			DelayTime = 0f;
			FadeOutTime = 1f;
			BeginVolume = 1f;
			EndVolume = 0f;
		}
	}
}
