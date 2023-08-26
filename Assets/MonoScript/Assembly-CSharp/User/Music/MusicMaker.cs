using UnityEngine;

namespace User.Music
{
	[RequireComponent(typeof(AudioSource))]
	public class MusicMaker : MonoBehaviour
	{
		public float m_speed = 5f;

		public GameObject m_startPoint;

		public AudioClip m_audioClip;

		[HideInInspector]
		public string m_koreographyTrackGUID;

		[HideInInspector]
		public float m_currentMusicTime;

		[HideInInspector]
		public float m_minMusicTime;

		[HideInInspector]
		public float m_maxMusicTime;

		public AudioSource m_audioSource;

		public AudioSource GetAudioSource
		{
			get
			{
				if (m_audioSource == null)
				{
					m_audioSource = base.gameObject.GetComponent<AudioSource>();
				}
				return m_audioSource;
			}
		}

		public void Play()
		{
			GetAudioSource.clip = m_audioClip;
			GetAudioSource.time = m_minMusicTime;
			GetAudioSource.Play();
		}

		public void Stop()
		{
			GetAudioSource.Stop();
		}

		public void Refresh()
		{
			if (m_audioClip != null)
			{
				m_currentMusicTime = 0f;
				m_minMusicTime = 0f;
				m_maxMusicTime = m_audioClip.length;
			}
		}

		public float GetLenghtByTime(float time)
		{
			return time / Time.deltaTime * (m_speed * Time.deltaTime);
		}
	}
}
