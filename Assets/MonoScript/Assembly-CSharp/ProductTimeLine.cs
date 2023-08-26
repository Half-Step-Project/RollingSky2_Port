using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ProductTimeLine
{
	public string m_path = string.Empty;

	public PlayableDirector m_playableDirector;

	private GameObject m_gameObjectPrefab;

	public Dictionary<string, PlayableBinding> m_playableBindings = new Dictionary<string, PlayableBinding>();

	public ProductTimeLine(string path)
	{
		m_path = path;
	}

	public ProductTimeLine(GameObject gameObject)
	{
		m_gameObjectPrefab = gameObject;
	}

	public void OnInitialize(Transform parent)
	{
		GameObject gameObject = null;
		gameObject = ((!(m_gameObjectPrefab != null)) ? ResourcesManager.Load<GameObject>(m_path) : m_gameObjectPrefab);
		if (gameObject != null)
		{
			GameObject gameObject2 = Object.Instantiate(gameObject);
			if (parent != null)
			{
				gameObject2.transform.parent = parent.transform;
			}
			gameObject2.transform.localPosition = Vector3.zero;
			m_playableDirector = gameObject2.GetComponent<PlayableDirector>();
			m_playableBindings = TimeLineTools.CollectAllPlayableBinding(m_playableDirector.playableAsset);
		}
	}

	public void Play()
	{
		m_playableDirector.Play();
	}

	public void Pause()
	{
		m_playableDirector.Pause();
	}

	public void Resume()
	{
		m_playableDirector.Resume();
	}

	public void Stop()
	{
		m_playableDirector.Stop();
	}

	public void BeginSampling(float time = 0f)
	{
		m_playableDirector.time = time;
		m_playableDirector.DeferredEvaluate();
	}

	public void DestroyLocal()
	{
		Object.Destroy(m_playableDirector.gameObject);
		m_playableBindings.Clear();
		m_playableDirector = null;
	}
}
