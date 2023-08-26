using UnityEngine;

namespace RS2
{
	public class WholeFogTrigger : BaseTriggerBox
	{
		public Color _fogColor;

		public float _start;

		public float _end;

		private EffectState _state;

		private static Color _initFogColor;

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
			_initFogColor = RenderSettings.fogColor;
		}

		public override void ResetElement()
		{
			base.ResetElement();
			DestoryEffect();
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
			RenderSettings.fog = true;
			RenderSettings.fogColor = _fogColor;
			RenderSettings.fogStartDistance = _start;
			RenderSettings.fogEndDistance = _end;
			RenderSettings.fogMode = FogMode.Linear;
		}

		private void DestoryEffect()
		{
			RenderSettings.fog = false;
			RenderSettings.fogColor = _initFogColor;
		}
	}
}
