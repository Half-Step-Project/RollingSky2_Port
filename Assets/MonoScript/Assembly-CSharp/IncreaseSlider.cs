using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class IncreaseSlider : MonoBehaviour
{
	public Image slider;

	public Image sliderFade;

	private float fadeTime;

	private void Awake()
	{
		Color color = sliderFade.color;
		sliderFade.color = new Color(color.r, color.g, color.b, 0f);
	}

	private void OnDestroy()
	{
		CancelInvoke();
	}

	public void StartIncrease(float start, float end, float time, float fadeTime)
	{
		this.fadeTime = fadeTime;
		slider.fillAmount = start;
		sliderFade.fillAmount = start;
		Color color = sliderFade.color;
		sliderFade.color = new Color(color.r, color.g, color.b, 0f);
		DOTween.defaultEaseType = Ease.Linear;
		slider.DOFillAmount(end, time);
		sliderFade.DOFillAmount(end, time);
		sliderFade.DOFade(1f, fadeTime);
		Invoke("DoFadeOut", time);
	}

	private void DoFadeOut()
	{
		sliderFade.DOFade(0f, fadeTime);
	}
}
