using System.Collections.Generic;
using UnityEngine;

public class ChangeParticleLayers : MonoBehaviour
{
	public List<ParticleSystem> m_particle = new List<ParticleSystem>();

	public void ChangeLayer(int layer)
	{
		for (int i = 0; i < m_particle.Count; i++)
		{
			m_particle[i].GetComponent<Renderer>().sortingOrder = layer;
		}
	}
}
