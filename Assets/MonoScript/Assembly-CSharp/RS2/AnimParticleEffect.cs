using UnityEngine;

namespace RS2
{
	public class AnimParticleEffect : BaseEnemy
	{
		private Animation anim;

		private Transform model;

		private Transform effect;

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
			anim = GetComponentInChildren<Animation>();
			if ((bool)anim)
			{
				anim.gameObject.SetActive(true);
				anim.wrapMode = WrapMode.Loop;
				PlayAnim(anim, true);
			}
			model = base.transform.Find("model");
			if ((bool)model)
			{
				model.gameObject.SetActive(true);
			}
			effect = base.transform.Find("effect");
			if ((bool)effect)
			{
				particles = effect.GetComponentsInChildren<ParticleSystem>();
				PlayParticle(particles, false);
			}
			commonState = CommonState.None;
		}

		public override void ResetElement()
		{
			base.ResetElement();
			if ((bool)anim)
			{
				anim.gameObject.SetActive(true);
				PlayAnim(anim, false);
			}
			if ((bool)model)
			{
				model.gameObject.SetActive(true);
			}
			PlayParticle(particles, false);
			commonState = CommonState.None;
		}

		public override void TriggerEnter(BaseRole ball)
		{
			if (commonState == CommonState.None)
			{
				if ((bool)anim)
				{
					anim.gameObject.SetActive(false);
				}
				if ((bool)model)
				{
					model.gameObject.SetActive(false);
				}
				PlayParticle(particles, true);
				PlaySoundEffect();
				commonState = CommonState.Active;
			}
		}
	}
}
