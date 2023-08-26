using UnityEngine;

public sealed class CollidePerformTrigger : BaseEnemy
{
	[Range(0f, 1f)]
	public float DebugPercent;

	private float debugPercent;

	private Animation anim;

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
		commonState = CommonState.None;
		anim = base.transform.Find("model").GetComponentInChildren<Animation>();
	}

	public override void ResetElement()
	{
		base.ResetElement();
		commonState = CommonState.None;
		PlayAnim(anim, false);
		PlayParticle(particles, false);
	}

	public override void TriggerEnter(BaseRole ball)
	{
		if (commonState == CommonState.None)
		{
			PlayAnim(anim, true);
			PlayParticle(particles, true);
			commonState = CommonState.Active;
		}
	}

	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		if (debugPercent != DebugPercent)
		{
			if (anim == null)
			{
				anim = GetComponentInChildren<Animation>();
			}
			SetAnimPercent(anim, DebugPercent);
			debugPercent = DebugPercent;
		}
	}
}
