using UnityEngine;

namespace RS2
{
	public class SkyBoxTrigger : BaseTriggerBox
	{
		private EffectState _state;

		public Material _skybox;

		private static Material _oldSkybox;

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
			if (_oldSkybox == null)
			{
				_oldSkybox = MaterialTool.GetSkyboxMaterial();
			}
			MaterialTool.SetSkyboxMaterial(_skybox);
		}

		private void DestoryEffect()
		{
			if (_oldSkybox != null)
			{
				MaterialTool.SetSkyboxMaterial(_oldSkybox);
			}
		}

		public override void ResetElement()
		{
			base.ResetElement();
			_state = EffectState.NONE;
			DestoryEffect();
		}
	}
}
