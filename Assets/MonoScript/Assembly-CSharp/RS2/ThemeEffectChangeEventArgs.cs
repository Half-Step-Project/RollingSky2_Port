using Foundation;

namespace RS2
{
	public sealed class ThemeEffectChangeEventArgs : EventArgs<ThemeEffectChangeEventArgs>
	{
		public bool IfPlay { get; private set; }

		public EffectName ThemeEffect { get; private set; }

		public ThemeEffectChangeEventArgs Initialize(EffectName effectName, bool ifPlay)
		{
			ThemeEffect = effectName;
			IfPlay = ifPlay;
			return this;
		}

		protected override void OnRecycle()
		{
			ThemeEffect = EffectName.RainEffect;
			IfPlay = false;
		}
	}
}
