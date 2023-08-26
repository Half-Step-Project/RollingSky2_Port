using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ImageFade : MonoBehaviour
{
	public Image target;

	private float fadeTime;

	private void Awake()
	{
		Color color = target.color;
		target.color = new Color(color.r, color.g, color.b, 0f);
	}

	private void OnDestroy()
	{
		CancelInvoke();
	}

	public void StartFade(float time, float fadeTime)
	{
		this.fadeTime = fadeTime;
		Color color = target.color;
		target.color = new Color(color.r, color.g, color.b, 0f);
		DOTween.defaultEaseType = Ease.Linear;
		target.DOFade(1f, fadeTime);
		Invoke("DoFadeOut", time);
	}

	private void DoFadeOut()
	{
		target.DOFade(0f, fadeTime);
	}
}
