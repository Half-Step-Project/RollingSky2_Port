using UnityEngine;

public class ScrollItemScaleByPos : MonoBehaviour
{
	public float maxScale = 1.2f;

	public float minScale = 0.6f;

	public float minAlpha = 0.2f;

	public float distanceScaleFactor = 0.005f;

	public float distanceAlphaFactor = 0.005f;

	public Transform center;

	private float preDistance = float.MaxValue;

	private RectTransform rectTrans;

	private CanvasGroup canvasGroup;

	private void Awake()
	{
		rectTrans = GetComponent<RectTransform>();
		canvasGroup = GetComponent<CanvasGroup>();
	}

	private void Update()
	{
		float num = Mathf.Abs(center.InverseTransformPoint(base.transform.position).x);
		if (!(Mathf.Abs(preDistance - num) < 5f))
		{
			preDistance = num;
			float value = maxScale / (num * distanceScaleFactor + 1f);
			value = Mathf.Clamp(value, minScale, maxScale);
			rectTrans.transform.localScale = new Vector3(value, value, 1f);
			float f = num * distanceAlphaFactor;
			float value2 = 1f / (Mathf.Pow(f, 2f) + 1f);
			value2 = Mathf.Clamp(value2, minAlpha, 1f);
			canvasGroup.alpha = value2;
		}
	}
}
