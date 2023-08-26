using UnityEngine;

public class SynchronizedAnimationMidground : BaseMidground
{
	public Animation m_animation;

	private bool m_isPlaying;

	public override void Initialize()
	{
		m_isPlaying = false;
		m_animation = base.gameObject.GetComponentInChildren<Animation>();
		if (m_animation != null && m_animation.clip != null)
		{
			float time = Singleton<MenuMusicController>.Instance.GetTime();
			string text = m_animation.clip.name;
			m_animation[text].time = time;
			m_animation.Play();
			m_animation.Sample();
			m_animation.Stop();
		}
	}

	public override void UpdateElement()
	{
		if (!m_isPlaying)
		{
			if (m_animation != null && m_animation.clip != null)
			{
				float time = Singleton<MenuMusicController>.Instance.GetTime();
				string text = m_animation.clip.name;
				m_animation[text].time = time;
				m_animation.Play();
			}
			m_isPlaying = true;
		}
	}

	public override void ResetElement()
	{
		m_isPlaying = false;
		if (m_animation != null && m_animation.clip != null)
		{
			m_animation.Stop();
		}
	}

	[ContextMenu("playAutomatically to false")]
	private void OnSetStop()
	{
		m_animation = base.gameObject.GetComponentInChildren<Animation>();
		m_animation.playAutomatically = false;
	}

	[ContextMenu("playAutomatically to true")]
	private void OnSetPlay()
	{
		m_animation = base.gameObject.GetComponentInChildren<Animation>();
		m_animation.playAutomatically = true;
	}
}
