using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class DebugAnimation
{
	[Range(0f, 1f)]
	public float DebugPercent;

	private float debugPercent;

	private Animation anim;

	public void OnDrawGizmos(Transform transform)
	{
		if (debugPercent == DebugPercent)
		{
			return;
		}
		if (anim == null)
		{
			anim = transform.GetComponentInChildren<Animation>();
		}
		if ((bool)anim)
		{
			anim.Play();
			IEnumerator enumerator = anim.GetEnumerator();
			while (enumerator.MoveNext())
			{
				((AnimationState)enumerator.Current).normalizedTime = DebugPercent;
			}
			anim.Sample();
			anim.Stop();
		}
		debugPercent = DebugPercent;
	}
}
