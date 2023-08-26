using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ToastHandler : MonoBehaviour
{
	public Image image;

	public Text text;

	public void InitToast(string str, Action callback)
	{
		text.text = str;
		int num = (int)text.preferredWidth;
		int num2 = (int)text.rectTransform.sizeDelta.x;
		int num3 = num;
		int num4 = 1;
		if (num > num2)
		{
			num4 = num / num2 + 1;
			num3 = num2;
		}
		image.rectTransform.sizeDelta = new Vector2(num3 + 50, 20 * num4 + 50);
		FadeOut(callback);
	}

	public void FadeOut(Action callback)
	{
		image.DOFade(0f, 4f).OnComplete(delegate
		{
			callback();
			UnityEngine.Object.Destroy(base.gameObject);
		});
		text.DOFade(0f, 4f);
	}

	public void Move(float speed, int targetPos)
	{
		base.transform.DOLocalMoveY((float)targetPos * image.rectTransform.sizeDelta.y, speed);
	}
}
