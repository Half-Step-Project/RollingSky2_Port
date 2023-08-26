using UnityEngine;

public static class ParticleExtended
{
	public static void PlayParticle(this GameObject gameObject)
	{
		gameObject.GetComponentsInChildren<ParticleSystem>().PlayParticle();
	}

	public static void StopParticle(this GameObject gameObject)
	{
		gameObject.GetComponentsInChildren<ParticleSystem>().StopParticle();
	}

	public static void PlayParticle(this ParticleSystem[] particles, bool isChindren = true)
	{
		if (particles == null)
		{
			return;
		}
		for (int i = 0; i < particles.Length; i++)
		{
			if (particles[i] != null)
			{
				particles[i].Play(isChindren);
			}
		}
	}

	public static void StopParticle(this ParticleSystem[] particles, bool isChindren = true, ParticleSystemStopBehavior stopType = ParticleSystemStopBehavior.StopEmitting)
	{
		if (particles == null)
		{
			return;
		}
		for (int i = 0; i < particles.Length; i++)
		{
			if (particles[i] != null)
			{
				particles[i].Stop(isChindren, stopType);
			}
		}
	}
}
