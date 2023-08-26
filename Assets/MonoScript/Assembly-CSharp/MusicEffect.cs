using System.Collections.Generic;
using UnityEngine;

public class MusicEffect : MonoBehaviour
{
	public int m_sampleNum = 20;

	public int m_sampleOffset = 5;

	public int m_sampleRate = 10;

	public bool m_isScale;

	public float m_scaleRate = 1f;

	public float m_emissionRate = 20f;

	public float m_blendRate = 20f;

	public List<Material> m_materialList = new List<Material>();

	private float[] m_sample = new float[64];

	private AudioSource m_anudio;

	private void Start()
	{
	}

	private void Update()
	{
		m_anudio.GetSpectrumData(m_sample, 0, FFTWindow.Hamming);
		float num = 0f;
		for (int i = m_sampleOffset; i < m_sampleRate; i++)
		{
			num += m_sample[i] / (float)(m_sampleRate - m_sampleOffset);
		}
		for (int j = 0; j < m_materialList.Count; j++)
		{
			if (m_isScale)
			{
				m_materialList[j].SetFloat("_Scale", 1f + num * m_scaleRate);
			}
			m_materialList[j].SetFloat("_Emission", num * m_emissionRate);
			m_materialList[j].SetFloat("_Blend", num * m_blendRate);
		}
	}
}
