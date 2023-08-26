using Foundation;
using UnityEngine;

namespace RS2
{
	public class BeginStarEffect : BaseEnemy
	{
		public enum StarState
		{
			eWait,
			eHanging,
			eMoving,
			eEnd
		}

		public Animation modelAnim;

		private StarState currentState;

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
			currentState = StarState.eWait;
			if (modelAnim == null)
			{
				modelAnim = base.transform.Find("model").GetComponent<Animation>();
			}
			if (effectChild == null)
			{
				effectChild = base.transform.Find("model/child/effect");
				if ((bool)effectChild)
				{
					particles = effectChild.GetComponentsInChildren<ParticleSystem>();
					StopParticle();
				}
			}
			PlayHanging();
		}

		public override void ResetElement()
		{
			base.ResetElement();
			StopAndResetAll();
			currentState = StarState.eWait;
		}

		private void OnEnable()
		{
			Mod.Event.Subscribe(EventArgs<GameCutSceneEventArgs>.EventId, OnPlayMoving);
		}

		private void OnDisable()
		{
			Mod.Event.Unsubscribe(EventArgs<GameCutSceneEventArgs>.EventId, OnPlayMoving);
		}

		private void PlayHanging()
		{
			if (currentState == StarState.eWait)
			{
				modelAnim.wrapMode = WrapMode.Loop;
				modelAnim["anim01"].normalizedTime = 0f;
				modelAnim.Play("anim01");
				PlayParticle();
				currentState = StarState.eHanging;
			}
		}

		private void OnPlayMoving(object sender, EventArgs e)
		{
			if (e is GameCutSceneEventArgs && (currentState == StarState.eHanging || currentState == StarState.eWait))
			{
				modelAnim.wrapMode = WrapMode.ClampForever;
				modelAnim["anim02"].normalizedTime = 0f;
				modelAnim.Play("anim02");
				currentState = StarState.eMoving;
			}
		}

		private void StopAndResetAll()
		{
			ResetAnim(modelAnim, "anim01");
		}

		private void ResetAnim(Animation anim, string clipName)
		{
			if ((bool)anim)
			{
				anim.Play();
				anim[clipName].normalizedTime = 0f;
				anim.Sample();
				anim.Stop();
			}
		}
	}
}
