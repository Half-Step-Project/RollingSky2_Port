using RS2;
using UnityEngine;

public class ResultFormAnimationController : MonoBehaviour
{
	private ResultForm m_resultForm;

	public void Init(ResultForm resultForm)
	{
		m_resultForm = resultForm;
	}

	public void Reset()
	{
	}

	public void PlayResultBegin()
	{
	}

	public void PlayResultStars()
	{
	}

	public void StartRise()
	{
	}

	public void ShowRewardBox()
	{
	}

	private void OnDestroy()
	{
		StopAllCoroutines();
	}
}
