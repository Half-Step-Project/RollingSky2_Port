using UnityEngine;

public class ParticleScaleAdapter : MonoBehaviour
{
	public ParticleSystem m_particle;

	private void Start()
	{
		if (m_particle != null)
		{
			Debug.Log(".......:" + MonoSingleton<GameTools>.Instacne.UIScale());
			m_particle.startSize *= MonoSingleton<GameTools>.Instacne.UIScale();
		}
	}
}
