using System.Collections;
using UnityEngine;

public class Level4 : BaseLevel
{
	private bool m_isInitialize = true;

	private float m_playAudioTime = 2f;

	private ProductTimeLine m_admission;

	public Level4(int levelID)
		: base(levelID)
	{
	}

	public override void OnInitialize()
	{
		m_admission = new ProductTimeLine(m_levelData.m_timeLineData.m_playableDirector[0].gameObject);
		m_admission.OnInitialize(null);
		GotoShowGameStartView();
		m_isInitialize = true;
	}

	public override void OnClickGameStateButton()
	{
		m_admission.m_playableDirector.gameObject.SetActive(true);
		if (m_isInitialize)
		{
			m_admission.Play();
			GotoPlayBGMusic(m_playAudioTime);
		}
		else
		{
			m_admission.Resume();
			GotoPlayBGMusic();
		}
		SetActiveForRenderer(BaseRole.theBall.gameObject, false);
		CoroutineManager.CreateCoroutine(CoroutineManagerType.BASELEVEL, WaitAdmissionFinished());
	}

	public override void OnGameEnd()
	{
	}

	public override void OnGameReset()
	{
		m_admission.m_playableDirector.gameObject.SetActive(true);
		m_admission.BeginSampling(m_playAudioTime);
		m_isInitialize = false;
	}

	public override void OnGameRebirthReset()
	{
		m_isInitialize = false;
	}

	public override void DestroyLocal()
	{
		m_admission.DestroyLocal();
	}

	private IEnumerator WaitAdmissionFinished()
	{
		yield return new WaitForSeconds((float)m_admission.m_playableDirector.duration);
		GotoStartGame();
		m_admission.Stop();
		m_admission.m_playableDirector.gameObject.SetActive(false);
		SetActiveForRenderer(BaseRole.theBall.gameObject, true);
	}

	public void SetActiveForRenderer(GameObject obj, bool active)
	{
		Renderer[] componentsInChildren = obj.GetComponentsInChildren<Renderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = active;
		}
	}
}
