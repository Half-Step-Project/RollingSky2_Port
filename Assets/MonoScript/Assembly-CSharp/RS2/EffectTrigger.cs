using Foundation;

namespace RS2
{
	public class EffectTrigger : BaseTriggerBox
	{
		private EffectState _state;

		public EffectName _effectName;

		public override bool IfRebirthRecord
		{
			get
			{
				return false;
			}
		}

		public override void Initialize()
		{
			base.Initialize();
		}

		public override void TriggerEnter(BaseRole ball)
		{
			if (_state == EffectState.NONE)
			{
				SwitchState(EffectState.PLAY);
			}
			else if (_state == EffectState.PLAY)
			{
				SwitchState(EffectState.END);
			}
		}

		private void SwitchState(EffectState state)
		{
			_state = state;
			switch (state)
			{
			case EffectState.PLAY:
				PlayEffect();
				break;
			case EffectState.END:
				DestoryEffect();
				break;
			}
		}

		private void PlayEffect()
		{
			ThemeEffectChangeEventArgs themeEffectChangeEventArgs = Mod.Reference.Acquire<ThemeEffectChangeEventArgs>();
			themeEffectChangeEventArgs.Initialize(_effectName, true);
			Mod.Event.Fire(this, themeEffectChangeEventArgs);
		}

		private void DestoryEffect()
		{
			ThemeEffectChangeEventArgs themeEffectChangeEventArgs = Mod.Reference.Acquire<ThemeEffectChangeEventArgs>();
			themeEffectChangeEventArgs.Initialize(_effectName, false);
			Mod.Event.Fire(this, themeEffectChangeEventArgs);
		}

		public override void ResetElement()
		{
			base.ResetElement();
			_state = EffectState.NONE;
		}
	}
}
