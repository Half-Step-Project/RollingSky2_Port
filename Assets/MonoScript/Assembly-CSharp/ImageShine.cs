using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ImageShine : MonoBehaviour
{
	public float time;

	public float start;

	public float end;

	public float angle;

	public float width;

	private Image image;

	private Tweener shineTweener;

	private void Awake()
	{
		image = GetComponent<Image>();
	}

	private void OnDestroy()
	{
		if (shineTweener != null)
		{
			shineTweener.Kill();
		}
	}

	public void StartShine()
	{
		if (!(image == null))
		{
			Material mat = new Material(image.material);
			image.material = mat;
			mat.SetFloat("_Angle", angle);
			mat.SetFloat("_Width", width);
			shineTweener = DOTween.To(delegate(float x)
			{
				mat.SetFloat("_Percent", x);
			}, start, end, time).SetEase(Ease.Linear);
		}
	}
}
